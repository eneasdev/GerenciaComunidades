using Novo.Models.ReservaModels;

namespace Novo.Services
{
    public interface IReservaService
    {
        void ResetarReservas();
        bool ReservaExiste(ReservarAmbienteViewModel candidata);
    }
}
