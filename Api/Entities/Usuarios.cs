using System;
using System.ComponentModel.DataAnnotations;
using cobric_backend.Api.Enums;

namespace usuarios_backend.Api.Entities
{
    public class Usuarios
    { 
        [Key]
        public Guid idUsuario { get; set; }

        [Required]
        [MaxLength(1)]
        public EnumTipoUsuario tipoUsuario { get; set; }

        public string nome { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string senha { get; set; }

        [MaxLength(6)]
        public string ra { get; set; }

        public string telefone { get; set; }
    }
}