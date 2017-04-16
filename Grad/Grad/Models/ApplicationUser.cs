using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Grad.Models
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Задайте имя")]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Задайте фамилию")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

    }

    public class ArticleRequest
    {
        public int ArticleRequestId { get; set; }
        [Required(ErrorMessage = "Введите название")]
        [Display(Name = "Название")]
        public string ReqName { get; set; }
        [Required(ErrorMessage = "Введите описание")]
        [Display(Name = "Описание")]
        public string ReqDescr { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Выберите дату")]
        [Display(Name = "Срок сдачи")]
        public DateTime ReqDate { get; set; }
    }

   
    //localdb mssqllocaldb 12.0.2000
    public class Article
    {

        public int ArticleID { get; set; }
        [Required(ErrorMessage = "Введите название")]
        [Display(Name = "Название")]
        public string ArtName { get; set; }
        [Required(ErrorMessage = "Введите описание")]
        [Display(Name = "Описание")]
        public string ArtDescr { get; set; }
        [Required(ErrorMessage = "Выберите дату")]
        [DataType(DataType.Date)]
        [Display(Name = "Срок сдачи")]
        public DateTime Deadline { get; set; }
    }

    public class State
    {

        public int StateId { get; set; }
        [Required(ErrorMessage = "Введите текст примечания")]
        [Display(Name = "Примечание")]
        public string StateDescr { get; set; }
        [Required(ErrorMessage = "Выберите дату")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата")]
        public DateTime StateDate { get; set; }
        [Required(ErrorMessage = "Выберите статью")]
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }

    public class Author
    {

        public int AuthorId { get; set; }
        [Required(ErrorMessage = "Выберите пользователя")]
        public int Id { get; set; }
        public User User { get; set; }
        [Required(ErrorMessage = "Выберите статью")]
        public int AtricleId { get; set; }
        public Article Article { get; set; }
    }

    public class Note
    {

        public int NoteId { get; set; }
        [Required]
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        [Required(ErrorMessage = "Введите текст замечания")]
        [Display(Name = "Замечание")]
        public string NoteDescr { get; set; }
        [Required(ErrorMessage = "Выберите дату")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата")]
        public DateTime NoteDate { get; set; }
    }
}
