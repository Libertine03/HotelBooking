using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите тег")]
        [MaxLength(30, ErrorMessage = "Превышена длина тега")]
        [Display(Name = "Тег")]
        public string TagTitle { get; set; }

    }
}
