using System.ComponentModel.DataAnnotations;

namespace usuarios_backend.Api.Models
{
    public class EmailModel
    {
        [Required, Display(Name = "Email de destino"), EmailAddress]
        public string Destino { get; set; }
    }
}