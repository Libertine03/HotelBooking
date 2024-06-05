using HotelBooking.DataAccess.Data;
using HotelBooking.DataAccess.Repository.Interfaces;
using HotelBooking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HotelBookingWeb.Areas.Admin.Controllers
{
    [Area("Customer")]
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public BookingController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? roomId)
        {
            BookedRoom? _BookingRoom = new BookedRoom();
            Room _room = await _unitOfWork.Room.GetAsync(h => h.Id == roomId);

            if(_room != null)
            {
				_BookingRoom.RoomTitle = _room.RoomName;
                _BookingRoom.hotelId = (int)_room.HotelId;
			}
            _BookingRoom.ArrivalDate = DateTime.UtcNow;
            _BookingRoom.DepartureDate = DateTime.UtcNow;
			return View(_BookingRoom);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BookedRoom bookedRoom)
        {
            if(bookedRoom.ArrivalDate < DateTime.Now)
            {
                TempData["Error"] = "Дата заезда не может быть меньше текущей даты!";
                return View(bookedRoom);
            }
            else if(bookedRoom.ArrivalDate >= bookedRoom.DepartureDate)
            {
                TempData["Error"] = "Дата заезда не может быть позже даты выезда. Необходимо поменять дату!";
                return View(bookedRoom);
            }

            if(ModelState.IsValid)
            {
                bookedRoom.userId = _userManager.GetUserId(User);
                bookedRoom.ArrivalDate = bookedRoom.ArrivalDate.ToUniversalTime();
				bookedRoom.DepartureDate = bookedRoom.DepartureDate.ToUniversalTime();

				await _unitOfWork.BookingRoom.AddAsync(bookedRoom);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Вы успешно забронировали номер в отеле.";
                return RedirectToAction("BookRoom");
            }
            TempData["Error"] = "Ошибка при бронировании номера.";
            return View(bookedRoom);
        }

        public async Task<IActionResult> BookRoom()
        {
            var rooms = await _unitOfWork.BookingRoom.GetAllAsync();
            rooms = rooms.Where(u => u.userId == _userManager.GetUserId(User));
            ViewBag.ActiveTab = "BookedRooms";

			return View(rooms);
        }
    }
}
