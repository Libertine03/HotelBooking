using HotelBooking.DataAccess.Repository.Interfaces;
using HotelBooking.Models;
using HotelBooking.Models.ViewModels;
using HotelBooking.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBookingWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class HotelsController : Controller
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly IUnitOfWork _unitOfWork;

        public HotelsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost)
        {
			_unitOfWork = unitOfWork;
            _webHost = webHost;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Hotel> _hotels = await _unitOfWork.Hotel.GetAllAsync(includeProp: "Tags");
            return View(_hotels);
        }

        /// <summary>
        /// Create methods
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<Tag> tags = await _unitOfWork.Tag.GetAllAsync();

            HotelVM? hotelVM = new HotelVM()
            {
                Tags = tags.Select(u => new SelectListItem { Text = u.TagTitle, Value = u.Id.ToString()}),
                Hotel = new Hotel()
            };

			//Если id не задан, значит пользователь хочет добавить отель, иначе - редактирует
			if (id == 0 || id == null)
            {
                return View(hotelVM);
            }
            else
            {
                hotelVM.Hotel = await _unitOfWork.Hotel.GetAsync(h => h.Id == id);
                return View(hotelVM);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(HotelVM hotelVM, IFormFileCollection files)
        {
            if(files.Count > 5)
            {
                TempData["Error"] = "Ошибка при загрузке изображений";
                return RedirectToAction("Index");
            }

			if (ModelState.IsValid)
            {
                if(files.Count > 0)
                {
                    //Delete old images when update
                    if (hotelVM.Hotel.ImagesUrl.Count > 0)
                    {
                        foreach (var image in hotelVM.Hotel.ImagesUrl)
                        {
                            var oldImagePath = Path.Combine(_webHost.WebRootPath, image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        hotelVM.Hotel.ImagesUrl.Clear();
                    }

                    //Insert new Images
                    for (int i = 0; i < files.Count; i++)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(files[i].FileName);
                        string hotelPath = Path.Combine(_webHost.WebRootPath, @"images\hotels");

                        using (var FileStream = new FileStream(Path.Combine(hotelPath, fileName), FileMode.Create))
                        {
                            files[i].CopyTo(FileStream);
                        }

                        string finalFilePath = @"/images/hotels/" + fileName;

                        hotelVM.Hotel.ImagesUrl.Add(finalFilePath);
                    }
                }
                

				if (hotelVM.Hotel.Id == 0)
				{
					await _unitOfWork.Hotel.AddAsync(hotelVM.Hotel);
					TempData["success"] = "Успешно добавлено";
				}
				else
				{
					await _unitOfWork.Hotel.Update(hotelVM.Hotel);
					TempData["success"] = "Успешно обновлено";
				}

				await _unitOfWork.SaveAsync();
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Ошибка";
			return View(hotelVM);
        }

        /// <summary>
        /// Delete method (API call for js code)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            Hotel? _hotel = await _unitOfWork.Hotel.GetAsync(h => h.Id == id);
            if(_hotel == null)
            {
                return Json(new { success = false, message = "Ошибка удаления" });
            }

            if(_hotel.ImagesUrl != null && _hotel.ImagesUrl.Count > 0)
            {
                foreach (var image in _hotel.ImagesUrl)
                {
                    var oldImagePath = Path.Combine(_webHost.WebRootPath, image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _hotel.ImagesUrl.Clear();
            }

            await _unitOfWork.Hotel.RemoveAsync(_hotel);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Успешно удалено" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Hotel> hotels = await _unitOfWork.Hotel.GetAllAsync(includeProp: "Tags");
            return Json(new {data = hotels});
        }
    }
}
