
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Models
{
    public class Hotel
    {
        public Hotel()
        {
			ImagesUrl = new List<string>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название отеля не введено.")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина названия отеля.")]
        [DisplayName("Название отеля")]
        public string Name { get; set; }

		[Required(ErrorMessage = "Введите страну отеля")]
		[DisplayName("Страна отеля")]
		public string Country { get; set; }

		[Required(ErrorMessage = "Введите улицу отеля")]
		[DisplayName("Улица отеля")]
		public string Street { get; set; }

		[Required(ErrorMessage = "Введите город отеля")]
		[DisplayName("Город отеля")]
		public string City { get; set; }

		[Required(ErrorMessage = "Введите описание отеля")]
		[DataType(DataType.MultilineText, ErrorMessage = "Проблемы при вводе текста")]
		[DisplayName("Описание отеля")]
		public string Description { get; set; }

		[Display(Name = "Рейтинг пользователей")]
		public int? Rating { get; set; }

		[Display(Name = "Номер телефона отеля")]
		[DataType(DataType.PhoneNumber, ErrorMessage = "Ошибка при вводе номера телефона")]
		[ValidateNever]
        public string? phoneNumber { get; set; }

        [Display(Name = "Почта для связи")]
		[DataType(DataType.EmailAddress, ErrorMessage = "Неверный ввод e-mail")]
        [ValidateNever]
        public string? Email { get; set; }

        [Display(Name = "Ссылка на сайт")]
        [ValidateNever]
        public string? siteUrl { get; set; }

        public List<int> TagsID { get; set; }

		[ForeignKey("TagsID")]
		[Display(Name = "Тег для пользователя")]
		[ValidateNever]
		public List<Tag> Tags { get; set; }

		[DisplayName("Изображение отеля")]
		[ValidateNever]
		public List<string> ImagesUrl { get; set; }

        [Display(Name = "Номера отеля")]
        [ValidateNever]
        public List<Room> Rooms { get; set; }

		[Required]
		public decimal minPriceForRoom { get; set; }
    }
}