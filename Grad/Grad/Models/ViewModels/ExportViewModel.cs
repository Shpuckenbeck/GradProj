using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Grad.Models
{
    public class ExportViewModel
    {

        [Required]
        [Display(Name = "Имя файла")]
        public string filename { get; set; }

        public int artid { get; set; }
    }
}
