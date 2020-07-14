using System;
using System.ComponentModel.DataAnnotations;

namespace usuarios_backend.Api.Models
{
    public class UsuariosInclusao
    {
        public string nome { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public string email { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public string senha { get; set; }

        [MaxLength(6)]
        public string ra { get; set; }

        public string telefone { get; set; }
    }
}