using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBooking.Models.ViewModels
{
    public class HotelVM
    {
        public Hotel Hotel { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> Tags { get; set; }
    }
}
