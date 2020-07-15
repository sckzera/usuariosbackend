

using System;
using usuarios_backend.Api.Entities;

namespace usuarios_backend.Api.Repositories
{
    public interface IUsuarioRepository
    {
        Usuarios Obter(Guid codigo);
        void Cadastrar(Usuarios usuarios);
        void Alterar(Usuarios usuarios);
        void Excluir(Guid codigo);
        bool Salvar();   
        bool Existe(Guid codigo);
        bool ExisteCadastro(string email, string ra, string telefone);
        string CalculaHash(string senha);
        Usuarios ExisteSenhaEmail(string senha, string email);
        Usuarios Login(string senha, string email);
    }
}
