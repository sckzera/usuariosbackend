using System;
using System.ComponentModel.DataAnnotations;

namespace usuarios_backend.Api.Models
{
    public class UsuariosLogin
    {
        public string email { get; set; }
        public string senha { get; set; }
    }
}