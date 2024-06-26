﻿using System.ComponentModel.DataAnnotations;

namespace SME.CDEP.Aplicacao.DTOS;

public class AcervoArteGraficaCadastroDTO : AcervoCadastroDTO
{
    [MaxLength(100, ErrorMessage = "A localização do acervo arte gráfica não pode conter mais que 100 caracteres")]
    public string? Localizacao { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a procedência do acervo arte gráfica")]
    [MaxLength(200, ErrorMessage = "A procedência do acervo arte gráfica não pode conter mais que 200 caracteres")]
    public string Procedencia { get; set; }
    
    public bool CopiaDigital { get; set; }
    public bool PermiteUsoImagem { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador da conservação do acervo arte gráfica")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador da conservação do acervo arte gráfica deve ser maior que zero")]
    public long ConservacaoId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador da cromia do acervo arte gráfica")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador da cromia do acervo arte gráfica deve ser maior que zero")]
    public long CromiaId { get; set; }
    
    public string? Largura { get; set; }
    public string? Altura { get; set; }
    public string? Diametro { get; set; }
    
    [MaxLength(100, ErrorMessage = "A técnica do acervo arte gráfica não pode conter mais que 100 caracteres")]
    public string? Tecnica { get; set; }
    
    [Required(ErrorMessage = "É necessário informar o identificador do suporte do acervo arte gráfica")]
    [Range(1, long.MaxValue, ErrorMessage = "O identificador do suporte do acervo arte gráfica deve ser maior que zero")]
    public long SuporteId { get; set; }
    
    [Required(ErrorMessage = "É necessário informar a quantidade do acervo arte gráfica")]
    [Range(1, long.MaxValue, ErrorMessage = "A quantidade do acervo arte gráfica deve ser maior que zero")]
    public long Quantidade { get; set; }
    
    public long[]? Arquivos { get; set; }
}