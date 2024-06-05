using HotelBooking.DataAccess.Repository.Interfaces;
using HotelBooking.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HotelBookingWeb.Areas.Admin.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Hotel> hotels = await GetHotels();
			ViewBag.ActiveTab = "Index";
			return View(hotels);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == 0 || id == null) return NotFound();

            Hotel? hotel = await _unitOfWork.Hotel.GetAsync(h => h.Id == id, includeProp: "Tags,Rooms");
            IEnumerable<Room> _rooms = await _unitOfWork.Room.GetAllAsync();

            hotel.Rooms = _rooms.Where(r => r.HotelId == id).ToList();

            IEnumerable<Tag> tags = await _unitOfWork.Tag.GetAllAsync();
            hotel.Tags = tags.Join(hotel.TagsID, tag => tag.Id, id => id, (tag, id) => tag).ToList();

            return View(hotel);
        }

        public IActionResult Privacy()
        {
			ViewBag.ActiveTab = "Privacy";
			return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        #region Hotels Partial View
        public async Task<IActionResult> HotelsPV()
        {
            IEnumerable<Hotel> hotels = await GetHotels();

            return PartialView(hotels);
        }

        public async Task<IActionResult> HotelsSearch(string searchString)
        {
            var hotels = await GetHotels();
            ViewData["SearchString"] = searchString;
            if (!string.IsNullOrEmpty(searchString))
            {
                hotels = hotels.Where(h =>
                    h.Name!.ToLower().Contains(searchString.ToLower()) ||
                    h.Country!.ToLower().Contains(searchString.ToLower()) ||
                    h.City!.ToLower().Contains(searchString.ToLower())
                );
            }

            return PartialView("HotelsPV", hotels);
        }

        public async Task<IActionResult> FilterHotels(string orderBy, decimal minPriceValue, decimal maxPriceValue, List<string> selectedTags)
        {
            var hotels = await GetHotels();

            if(!string.IsNullOrEmpty(orderBy))
            {
                switch(orderBy)
                {
                    case "по убыванию цены":
                        {
                            hotels = hotels.OrderByDescending(h => h.minPriceForRoom);
                            break;
                        }
                    case "по возрастанию цены":
                        {
                            hotels = hotels.OrderBy(h => h.minPriceForRoom);
                            break;
                        }
                }
            }

            if(minPriceValue > 0 && maxPriceValue > 0 && maxPriceValue > minPriceValue)
            {
                hotels = hotels.Where(h => h.minPriceForRoom >= minPriceValue && h.minPriceForRoom <= maxPriceValue);
            }

            if (selectedTags.Count > 0 && selectedTags[0] != null)
            {
                selectedTags = selectedTags[0].Split(",").ToList();

                List<int> hotelsIdsToDelete = new List<int>();

                foreach(var hotel in hotels.ToList())
                {
                    foreach(var tag in hotel.Tags)
                    {
                        foreach(var t in selectedTags)
                        {
                            if(t.ToLower().Equals(tag.TagTitle.ToLower()))
                            {
                                hotelsIdsToDelete.Add(hotel.Id);
                            }
                        }
                    }
                }

                hotels = hotels.Where(hotel => hotelsIdsToDelete.Contains(hotel.Id));
			}

            if (hotels.Count() == 0) ViewData["SearchString"] = "ѕо данным фильтрам отели не найдены!";

            return PartialView("HotelsPV", hotels);
        }
        #endregion

        //Helper method to get hotels with tags
        private async Task<IEnumerable<Hotel>> GetHotels()
        {
            IEnumerable<Hotel> hotels = await _unitOfWork.Hotel.GetAllAsync(includeProp: "Tags");

            IEnumerable<Tag> tags = await _unitOfWork.Tag.GetAllAsync();
            ViewBag.TagsList = tags;

            foreach (var hotel in hotels)
            {
                hotel.Tags = tags.Join(hotel.TagsID, tag => tag.Id, id => id, (tag, id) => tag).ToList();
            }

            return hotels;
        }
    }
}
