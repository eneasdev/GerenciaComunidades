using Novo.Models.Enums;

namespace Novo.Models.Domain
{
    public class Usuario
    {
        public Usuario()
        {

        }

        public Usuario(string login, string senha, NivelAcesso nivelAcesso = NivelAcesso.Usuario)
        {
            Login = login;
            Senha = senha;
            NivelAcesso = nivelAcesso;
        }

        public int IdUsuario { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public NivelAcesso NivelAcesso { get; set; }
    }
}
