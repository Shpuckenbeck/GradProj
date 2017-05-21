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
        [Required(ErrorMessage = "Задайте логин")]
        [Display (Name = "Логин")]
        public override string UserName { get; set; }
        [Required(ErrorMessage = "Задайте номер телефона")]
        [Display(Name = "Телефон")]
        public override string PhoneNumber{ get; set; }
        [Display(Name = "Телефон подтверждён")]
        public override Boolean PhoneNumberConfirmed { get; set; }
        [Display(Name = "Неудачный вход")]
        public override int AccessFailedCount{ get; set; }
        [Display(Name = "Отображаемое имя")]
        public string displayedname { get; set; }
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

   
  
    public class Article
    {

        public int ArticleID { get; set; }
        [Required(ErrorMessage = "Введите название")]
        [Display(Name = "Название")]
        public string ArtName { get; set; }
        [Display(Name = "Содержимое")]
        public string content { get; set; }
        [Required(ErrorMessage = "Введите описание")]
        [Display(Name = "Описание")]
        public string ArtDescr { get; set; }
        [Required(ErrorMessage = "Выберите дату")]
        [DataType(DataType.Date)]
        [Display(Name = "Срок сдачи")]
        public DateTime Deadline { get; set; }
        [Display(Name = "Заблокирована")]
        public string blocked { get; set; }
    }

    public class State
    {

        public int StateId { get; set; }
        [Required(ErrorMessage = "Выберите состояние")]
        [Display(Name = "Состояние")]
        public int StatusId { get; set; }
        [Display(Name = "Дата")]
        public Status Status { get; set; }
        //[Required(ErrorMessage = "Введите текст примечания")]
        [Display(Name = "Примечание")]
        public string StateDescr { get; set; }
        [Required(ErrorMessage = "Выберите дату")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата")]
        public DateTime StateDate { get; set; }
        [Required(ErrorMessage = "Выберите статью")]
        [Display(Name = "Статья")]
        public int ArticleId { get; set; }
        [Display(Name = "Статья")]
        public Article Article { get; set; }

    }

    public class Author
    {
        
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "Выберите пользователя")]
        [Display(Name = "Автор")]
        public string UserId { get; set; }
        [Display(Name = "Автор")]
        public User User { get; set; }
        [Required(ErrorMessage = "Выберите статью")]
        [Display(Name = "Статья")]
        public int ArticleID { get; set; }
        [Display(Name = "Статья")]
        public Article Article { get; set; }
    }

    public class Note
    {

        public int NoteId { get; set; }
        [Required]
        public int ArticleId { get; set; }
        [Display(Name = "Статья")]
        public Article Article { get; set; }
        [Required(ErrorMessage = "Введите текст замечания")]
        [Display(Name = "Замечание")]
        public string NoteDescr { get; set; }
        [Required(ErrorMessage = "Выберите дату")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата")]
        public DateTime NoteDate { get; set; }
        [Display(Name ="Исправлено")]
        public Boolean Fixed { get; set;}
        [Display(Name = "Проверено")]
        public Boolean Checked { get; set; }

    }

    public class Status
    {

        public int StatusId { get; set; }
        [Required(ErrorMessage = "Введите название состояния")]
        [Display(Name = "Название")]
        public string StatusName { get; set; }
        
    }

    public class Picture
    {
        public int PictureId { get; set; }
        [Required(ErrorMessage = "Введите название файла")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Дата")]
        public DateTime uploaddate { get; set; }
        public string Path { get; set; }
    }

}
