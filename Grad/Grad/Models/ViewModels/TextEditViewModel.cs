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
        [Required(ErrorMessage = "Введите имя файла")]
        [Display(Name = "Файл")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Задайте заголовок")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Введите описание")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Введите текст")]
        [Display(Name = "Текст")]
        public string Text { get; set; }

    }
}
