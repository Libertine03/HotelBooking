using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class Room // Модель комнаты, которая будет храниться для каждого отеля своя
    {
        public Room()
        {
            ImagesUrl = new List<string>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название номера")]
        [Display(Name = "Название номера")]
        public string RoomName { get; set; }

        [Required(ErrorMessage = "Необходимо ввести значение")]
        [Display(Name = "Стоимость данного номера в отеле за ночь")]
        [Range(1, 9999999, ErrorMessage = "Выход за диапазон (1, 9999999)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Необходимо ввести теги (через , или ;)")]
        [Display(Name = "Введите теги номера (через , или ;)")]
        public string Tags { get; set; }

        [Display(Name = "Изображения номера")]
        [ValidateNever]
        public List<string> ImagesUrl { get; set; }

        public int? HotelId { get; set; }

        [ForeignKey("HotelId")]
		[ValidateNever]
		public Hotel Hotel { get; set; }
    }
}
