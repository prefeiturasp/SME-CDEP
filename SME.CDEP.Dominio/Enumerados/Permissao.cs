using SME.SGP.Infra.Dominio.Atributos;

namespace SME.CDEP.Infra.Dominio.Enumerados
{
    public enum Permissao
    {
        //Cadastros/Acervo
        [PermissaoMenu(Menu = "Acervo", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 1, Url = "/acervo", EhConsulta = true)]
        ACR_C = 265,
        [PermissaoMenu(Menu = "Acervo", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 1, Url = "/acervo", EhInclusao = true)]
        ACR_I = 266,
        [PermissaoMenu(Menu = "Acervo", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 1, Url = "/acervo", EhExclusao = true)]
        ACR_E = 267,
        [PermissaoMenu(Menu = "Acervo", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 1, Url = "/acervo", EhAlteracao = true)]
        ACR_A = 268,

        //Cadastros/Assunto
        [PermissaoMenu(Menu = "Assunto", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 2, Url = "/assunto", EhConsulta = true)]
        ASS_C = 261,
        [PermissaoMenu(Menu = "Assunto", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 2, Url = "/assunto", EhInclusao = true)]
        ASS_I = 262,
        [PermissaoMenu(Menu = "Assunto", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 2, Url = "/assunto", EhExclusao = true)]
        ASS_E = 263,
        [PermissaoMenu(Menu = "Assunto", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 2, Url = "/assunto", EhAlteracao = true)]
        ASS_A = 264,
        
        //Cadastros/Autor
        [PermissaoMenu(Menu = "Autor", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 3, Url = "/autor", EhConsulta = true)]
        AUT_C = 249,
        [PermissaoMenu(Menu = "Autor", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 3, Url = "/autor", EhInclusao = true)]
        AUT_I = 250,
        [PermissaoMenu(Menu = "Autor", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 3, Url = "/autor", EhExclusao = true)]
        AUT_E = 251,
        [PermissaoMenu(Menu = "Autor", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 3, Url = "/autor", EhAlteracao = true)]
        AUT_A = 252,
        
        //Cadastros/Crédito
        [PermissaoMenu(Menu = "Crédito", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 4, Url = "/credito", EhConsulta = true)]
        CRD_C = 245,
        [PermissaoMenu(Menu = "Crédito", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 4, Url = "/credito", EhInclusao = true)]
        CRD_I = 246,
        [PermissaoMenu(Menu = "Crédito", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 4, Url = "/credito", EhExclusao = true)]
        CRD_E = 247,
        [PermissaoMenu(Menu = "Crédito", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 4, Url = "/credito", EhAlteracao = true)]
        CRD_A = 248,
        
        //Cadastros/Editora
        [PermissaoMenu(Menu = "Editora", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 5, Url = "/editora", EhConsulta = true)]
        EDT_C = 253,
        [PermissaoMenu(Menu = "Editora", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 5, Url = "/editora", EhInclusao = true)]
        EDT_I = 254,
        [PermissaoMenu(Menu = "Editora", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 5, Url = "/editora", EhExclusao = true)]
        EDT_E = 255,
        [PermissaoMenu(Menu = "Editora", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 5, Url = "/editora", EhAlteracao = true)]
        EDT_A = 256,
        
        //Cadastros/Série-Coleção
        [PermissaoMenu(Menu = "Série/Coleção", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 6, Url = "/serie-colecao", EhConsulta = true)]
        SRC_C = 257,
        [PermissaoMenu(Menu = "Série/Coleção", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 6, Url = "/serie-colecao", EhInclusao = true)]
        SRC_I = 258,
        [PermissaoMenu(Menu = "Série/Coleção", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 6, Url = "/serie-colecao", EhExclusao = true)]
        SRC_E = 259,
        [PermissaoMenu(Menu = "Série/Coleção", Icone = "fas fa-print", Agrupamento = "Cadastros", OrdemAgrupamento = 1, OrdemMenu = 6, Url = "/serie-colecao", EhAlteracao = true)]
        SRC_A = 260,
        
        //Operações/Atendimento de solicitações
        [PermissaoMenu(Menu = "Atendimento de solicitações", Icone = "fas fa-print", Agrupamento = "Operações", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/atendimento-solicitacoes", EhConsulta = true)]
        ATS_C = 269,
        [PermissaoMenu(Menu = "Atendimento de solicitações", Icone = "fas fa-print", Agrupamento = "Operações", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/atendimento-solicitacoes", EhInclusao = true)]
        ATS_I = 270,
        [PermissaoMenu(Menu = "Atendimento de solicitações", Icone = "fas fa-print", Agrupamento = "Operações", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/atendimento-solicitacoes", EhExclusao = true)]
        ATS_E = 271,
        [PermissaoMenu(Menu = "Atendimento de solicitações", Icone = "fas fa-print", Agrupamento = "Operações", OrdemAgrupamento = 2, OrdemMenu = 1, Url = "/atendimento-solicitacoes", EhAlteracao = true)]
        ATS_A = 272,
        
        //Operações/Solicitações
        [PermissaoMenu(Menu = "Solicitações", Icone = "fas fa-print", Agrupamento = "Operações", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "/solicitacoes", EhConsulta = true)]
        SLC_C = 273,
        [PermissaoMenu(Menu = "Solicitações", Icone = "fas fa-print", Agrupamento = "Operações", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "/solicitacoes", EhInclusao = true)]
        SLC_I = 274,
        [PermissaoMenu(Menu = "Solicitações", Icone = "fas fa-print", Agrupamento = "Operações", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "/solicitacoes", EhExclusao = true)]
        SLC_E = 275,
        [PermissaoMenu(Menu = "Solicitações", Icone = "fas fa-print", Agrupamento = "Operações", OrdemAgrupamento = 2, OrdemMenu = 2, Url = "/solicitacoes", EhAlteracao = true)]
        SLC_A = 276,
    }
}