using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class BookedRoom //Модель, которая будет отображаться при бронировании пользователем отеля и в списке забронированных номеров
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Выбранный номер")]
        public string RoomTitle { get; set; }

        [Required(ErrorMessage = "Выберите для скольки человек")]
        [Display(Name = "Количество человек для заселения")]
        public int NumOfResidents { get; set; }

        [Display(Name = "Дата въезда")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime ArrivalDate { get; set; }

        [Display(Name = "Дата выезда")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DepartureDate { get; set; }

        public string? userId { get; set; }

        public int hotelId { get; set; }

    }
}
