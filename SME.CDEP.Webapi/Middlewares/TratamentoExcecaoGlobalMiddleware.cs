﻿using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Serialization;
using SME.CDEP.Aplicacao.DTOS;
using SME.CDEP.Dominio.Excecoes;
using SME.CDEP.Infra.Servicos.Mensageria.Log;

namespace SME.CDEP.Webapi.Middlewares
{
    public class TratamentoExcecaoGlobalMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IServicoLogs servicoLogs;

        public TratamentoExcecaoGlobalMiddleware(RequestDelegate next, IServicoLogs servicoLogs)
        {
            this.next = next;
            this.servicoLogs = servicoLogs ?? throw new ArgumentNullException(nameof(servicoLogs));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (NegocioException nex)
            {
                if (nex.Mensagens.Any())
                {
                    await servicoLogs.Enviar(string.Join(" - ", nex.Mensagens), observacao: nex.Message, rastreamento: nex.StackTrace);
                    await TratarExcecao(context, nex.Mensagens, nex.StatusCode);
                }
                else
                {
                    await servicoLogs.Enviar(nex.Message, observacao: nex.Message, rastreamento: nex.StackTrace);
                    await TratarExcecao(context, new List<string>() {nex.Message}, nex.StatusCode);
                }
            }
            catch (Exception ex)
            {
                var mensagem = "Houve um comportamento inesperado do sistema CDEP. Por favor, contate a SME.";
                await servicoLogs.Enviar(mensagem, observacao: ex.Message, rastreamento: ex.StackTrace);
                await TratarExcecao(context, new List<string>() {mensagem});
            }
        }

        private async Task TratarExcecao(HttpContext context, List<string> mensagens, int statusCode = (int)HttpStatusCode.InternalServerError)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new RetornoBaseDTO(mensagens), new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }
    }

    public static class TratamentoExcecaoGlobalMiddlewareExtensions
    {
        public static IApplicationBuilder UseTratamentoExcecoesGlobalMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TratamentoExcecaoGlobalMiddleware>();
        }
    }

}
