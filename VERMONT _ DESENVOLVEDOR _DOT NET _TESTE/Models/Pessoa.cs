using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VERMONT.Web.Models
{
    public class Pessoa
    {
        public Pessoa()
        {
            Contatos = new List<Contato>();
        }

        public int IdPessoa { get; set; }

        [Display(Name = "Nome Pessoa")]
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Tipo Contato")]
        public int? IdTipoContato { get; set; }

        [Display(Name = "Nome Contato")]
        public string NomeContato { get; set; }

        [Display(Name = "Info Contato")]
        public string InfoContato { get; set; }
        

        public List<Contato> Contatos { get; set; }
    }
}