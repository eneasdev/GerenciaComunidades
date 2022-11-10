namespace Novo.Services
{
    public interface IReservaService
    {
        void ResetarReservas();
        bool ReservaExiste(int idAmbiente, DateTime dataInicial, DateTime dataFinal);
    }
}
