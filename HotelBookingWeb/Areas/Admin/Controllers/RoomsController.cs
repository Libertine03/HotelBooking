using HotelBooking.DataAccess.Repository.Interfaces;
using HotelBooking.Models;
using HotelBooking.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class RoomsController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHost;
        public RoomsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost)
        {
            _unitOfWork = unitOfWork;
            _webHost = webHost;
        }

        public async Task<IActionResult> Index(int hotelId)
        {
            var _rooms = await _unitOfWork.Room.GetAllAsync();
            _rooms = _rooms.Where(r => r.HotelId == hotelId);

			ViewBag.hotelId = hotelId;

			return View(_rooms);
        }

        public async Task<IActionResult> Upsert(int? id, int? hotelId)
        {
            Room _room;
			if (id == 0 || id == null)
            {
                _room = new Room() { HotelId = hotelId }; 
            }
            else
            {
                _room = await _unitOfWork.Room.GetAsync(r => r.Id == id);
            }
            return View(_room);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Room room, IFormFileCollection files)
        {
			if (files.Count > 5)
			{
				TempData["Error"] = "Ошибка при загрузке изображений";
				return RedirectToAction("Index");
			}

            if(ModelState.IsValid)
            {
				if(files.Count > 0)
				{
					if(room.ImagesUrl.Count > 0)
					{
						foreach(var image in room.ImagesUrl)
						{
							var oldImagePath = Path.Combine(_webHost.WebRootPath, image.TrimStart('/'));
							if (System.IO.File.Exists(oldImagePath))
							{
								System.IO.File.Delete(oldImagePath);
							}
						}
						room.ImagesUrl.Clear();
					}

					for (int i = 0; i < files.Count; i++)
					{
						string fileName = Guid.NewGuid().ToString() + Path.GetExtension(files[i].FileName);
						string roomPath = Path.Combine(_webHost.WebRootPath, @"images\hotels");

						using (var FileStream = new FileStream(Path.Combine(roomPath, fileName), FileMode.Create))
						{
							files[i].CopyTo(FileStream);
						}

						string finalFilePath = @"/images/hotels/" + fileName;

						room.ImagesUrl.Add(finalFilePath);
					}
				}

				if(room.Id == 0)
				{
					await _unitOfWork.Room.AddAsync(room);
					TempData["Success"] = "Успешно добавлено";
				}
				else
				{
					await _unitOfWork.Room.Update(room);
					TempData["Success"] = "Успешно обновлено";
				}
				var hotel = await _unitOfWork.Hotel.GetAsync(h => h.Id == room.HotelId);
				
				if(hotel.minPriceForRoom < room.Price)
				{
					hotel.minPriceForRoom = room.Price;
				}

				await _unitOfWork.SaveAsync();
				return RedirectToAction("Index", new {hotelId = room.HotelId});
			}
            return View(room);
        }

		[HttpDelete]
		public async Task<IActionResult> Delete(int? id)
		{
			Room? _room = await _unitOfWork.Room.GetAsync(h => h.Id == id);
			if (_room == null)
			{
				return Json(new { success = false, message = "Ошибка удаления" });
			}

			if (_room.ImagesUrl != null && _room.ImagesUrl.Count > 0)
			{
				foreach (var image in _room.ImagesUrl)
				{
					var oldImagePath = Path.Combine(_webHost.WebRootPath, image.TrimStart('/'));
					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath);
					}
				}
				_room.ImagesUrl.Clear();
			}

			await _unitOfWork.Room.RemoveAsync(_room);
			await _unitOfWork.SaveAsync();
			return Json(new { success = true, message = "Комната успешно удалена" });
		}
	}
}
