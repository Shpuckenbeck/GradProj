using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grad.Models
{
    public class AddStateViewModel
    {
        [Display(Name ="Статья")]
        public int artid { get; set; }

        [Display(Name = "Название")]
        public string name { get; set; }

        [Display(Name = "Состояние")]
        public int status { get; set; }

        [Display(Name = "Описание")]
        public string descr { get; set; }

        [Required(ErrorMessage = "Выберите дату")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата")]
        public DateTime date { get; set; }


    }
}
