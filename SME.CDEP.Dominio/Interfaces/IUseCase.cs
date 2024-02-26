namespace SME.CDEP.Dominio
{
    public interface IUseCase<in TParameter, TResponse>
    {
        Task<TResponse> Executar(TParameter param);
    }
}
