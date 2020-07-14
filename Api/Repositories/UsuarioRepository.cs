

using System;
using System.Linq;
using System.Security.Cryptography;
using usuarios_backend.Api.DbContexts;
using usuarios_backend.Api.Entities;

namespace usuarios_backend.Api.Repositories
{
    public class UsuarioRepository : IUsuarioRepository, IDisposable
    {
        private readonly UsuarioContext _context;

        public UsuarioRepository(UsuarioContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Cadastrar(Usuarios usuarios)
        {
            if (usuarios == null)
            {
                throw new ArgumentNullException(nameof(usuarios));
            }

            usuarios.idUsuario = Guid.NewGuid();
            
            usuarios.senha = CalculaHash(usuarios.senha);

            _context.Usuarios.Add(usuarios);
        }

        public Usuarios Obter(Guid codigo)
        {
            if (codigo == Guid.Empty)
                throw new ArgumentNullException(nameof(codigo));

            return _context.Usuarios
                .Where(c => c.idUsuario == codigo)
                .FirstOrDefault();
        }

        public void Excluir(Guid codigo)
        {
            if (codigo == Guid.Empty)
                throw new ArgumentNullException(nameof(codigo));

            var veiculo = _context.Usuarios.Where(q => q.idUsuario == codigo).First();

            _context.Usuarios.Remove(veiculo);
        }

        public bool Salvar()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Alterar(Usuarios usuarios)
        {
            if (usuarios == null)
                throw new ArgumentNullException(nameof(usuarios));

            if (usuarios.idUsuario == Guid.Empty)
                throw new ArgumentNullException(nameof(usuarios.idUsuario));

            var oldUsuario = Obter(usuarios.idUsuario);

            if (oldUsuario == null)
                throw new NullReferenceException(nameof(oldUsuario));

            _context.Usuarios.Update(oldUsuario).CurrentValues.SetValues(usuarios);
        }

        // Verifica se o GUID informado existe no banco de dados
        public bool Existe(Guid codigo)
        {
            if (codigo == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(codigo));
            }
            return _context.Usuarios.Any(a => a.idUsuario == codigo);
        }

        public bool ExisteCadastro(string email, string ra, string telefone){ 

           var validacaoEmail = _context.Usuarios.Where( e => e.email.Contains(email)
            || e.ra.Contains(ra)
            || e.telefone.Contains(telefone));

            if(validacaoEmail.Count() == 0)
            return false;

            return true;
        }

        public bool ExisteSenha(string senha){
            var retorno = CalculaHash(senha);

            var validacao = _context.Usuarios.Where( s => s.senha == retorno);

            if(validacao.Count() == 0)
                return false;

            return true;
        }

        public string CalculaHash(string senha)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(senha);
                byte[] hash = md5.ComputeHash(inputBytes);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString(); // Retorna senha criptografada 
            }
            catch (Exception)
            {
                return null; // Caso encontre erro retorna nulo
            }
        }
    }
}