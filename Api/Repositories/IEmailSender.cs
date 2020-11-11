using System.Threading.Tasks;

namespace usuarios_backend.Api.Repositories
{
     public interface IEmailSender
    {
        Task SendEmailAsync(string email, string message);
        Task TesteEnvioEmail(string email, string mensagem);
    }
}