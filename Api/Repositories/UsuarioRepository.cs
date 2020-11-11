

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

        public Usuarios ExisteSenhaEmail(string senha, string email){

            var retorno = CalculaHash(senha);

            var validacao = _context.Usuarios.Where( s => s.senha == retorno
            && s.email == email);

            if(validacao.Count() == 0)
                return null;

            return validacao.FirstOrDefault();
        }

          public Usuarios ExisteSenhaEmailDecrypt(string senha, string email){

            var validacao = _context.Usuarios.Where( s => s.email.Contains(email)).Single();
          
            if(validacao == null)
                  return null;

            var retorno = DecryptHash(validacao.senha);

            if(!senha.Equals(retorno)){
                return null;
            }

            return validacao;
        }

        public string CalculaHash(string senha)
        {
            try
            {
               string passcode = "240"; 
               string senha2 = EncryptStringSample.StringCipher.Encrypt(senha, passcode);
               return senha2;
            }
            catch (Exception err)
            {
                return null; // Caso encontre erro retorna nulo
            }
        }

         public string DecryptHash(string senha)
        {
            try
            {
               string passcode = "240"; 
               string senha2 = EncryptStringSample.StringCipher.Decrypt(senha, passcode);
               return senha2;
            }
            catch (Exception err)
            {
                return null; // Caso encontre erro retorna nulo
            }
        }

        public Usuarios Login(string senha, string email)
        {
            if(String.IsNullOrEmpty(email) && String.IsNullOrEmpty(senha))
                return null;
 
            var retorno = ExisteSenhaEmailDecrypt(senha, email);

            if(retorno == null)
                return retorno;

            return retorno;
        }

        public string Obter(string email)
        {
            var usuario =  _context.Usuarios
                .Where(c => c.email.Contains(email))
                .FirstOrDefault();
            
            var retorno = DecryptHash(usuario.senha);
            
             return retorno;
        }
    }
}