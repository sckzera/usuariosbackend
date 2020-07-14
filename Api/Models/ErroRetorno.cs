namespace usuarios_backend.Api.Models
{
    public class ErroRetorno
    {
        public ErroRetorno(string mensagem)
        {
            _mensagem = mensagem;
        }

        private string _mensagem;

        public string Mensagem { get { return _mensagem; } }

    }
}