using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fina.Core.Requests.Categories
{
    public class UpdateCategoryRequest : Request
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Título inválido")]
        [MaxLength(80, ErrorMessage = "O Título deve conter até 80 Caracteres.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Descrição Inválida")]
        public string Description { get; set; }
    }
}
