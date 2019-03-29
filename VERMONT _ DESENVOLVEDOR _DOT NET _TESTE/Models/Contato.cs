using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VERMONT.Web.Models
{
    public class Contato
    {
        public int IdContato { get; set; }

        [Display(Name = "Pessoa")]
        public int IdPessoa { get; set; }

        [Display(Name = "Tipo Contato")]
        [Required(ErrorMessage = "O campo Tipo Contato é obrigatório.")]
        public int IdTipoContato { get; set; }

        [Display(Name = "Nome Contato")]
        [Required(ErrorMessage = "O campo Nome Contato é obrigatório.")]
        public string NomeContato { get; set; }

        [Display(Name = "Info Contato")]
        [Required(ErrorMessage = "O campo Contato é obrigatório.")]
        public string InfoContato { get; set; }
         
        public TipoContato TipoContato { get; set; }

        public Pessoa Pessoa { get; set; }

    }

    public class ContatoFiltro
    {
         
        [Display(Name = "Nome Pessoa")]
        public string NomePessoa { get; set; }

        [Display(Name = "E-mail")] 
        public string Email { get; set; }

        [Display(Name = "Telefone")] 
        public string Telefone { get; set; }
         

    }
}