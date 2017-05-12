using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grad.Models
{
    public class AddAuthViewModel
    {
        [Display(Name = "Статья")]
        public int artid { get; set; }

        [Display(Name = "Название")]
        public string name { get; set; }

        [Display(Name = "Авторы")]
        public string[] authors { get; set; }

        
    }
}
