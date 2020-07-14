using AutoMapper;

namespace usuarios_backend.Api.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Models.UsuariosInclusao, Entities.Usuarios>();
        }
    }
}