using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grad.Models
{
    public class TextEditViewModel
    {
        public int artid;
        [Display(Name = "Статья")]
        public string Name { get; set; }       

        [Required(ErrorMessage = "Введите текст")]
        [Display(Name = "Текст")]
        public string Text { get; set; }

        [Display(Name = "Замечания")]
        public int[] notes{ get; set; }

    }
}
