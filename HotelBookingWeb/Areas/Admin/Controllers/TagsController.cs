using HotelBooking.DataAccess.Repository.Interfaces;
using HotelBooking.Models;
using HotelBooking.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class TagsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TagsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Tag> _tags = await _unitOfWork.Tag.GetAllAsync();
            return View(_tags);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Tag tag)
        {
            if(ModelState.IsValid)
            {
                await _unitOfWork.Tag.AddAsync(tag);
                await _unitOfWork.SaveAsync();
                TempData["Success"] = "Тег добавлен";
                return RedirectToAction("Index");
            }

			TempData["Error"] = "Тег не добавлен";
            return View();
		}

        public async Task<IActionResult> Edit(int? id)
        {
            var Tag = await _unitOfWork.Tag.GetAsync(u => u.Id == id);
            if(Tag != null)
            {
                return View(Tag);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Tag tag)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.Tag.Update(tag);
                await _unitOfWork.SaveAsync();
                TempData["Success"] = "Отредактировано";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Ошибка";
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var Tag = await _unitOfWork.Tag.GetAsync(u => u.Id == id);
            if (Tag != null)
            {
                return View(Tag);
            }

            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            var Tag = await _unitOfWork.Tag.GetAsync(u => u.Id == id);
            if (Tag != null)
            {
                await _unitOfWork.Tag.RemoveAsync(Tag);
                await _unitOfWork.SaveAsync();
                TempData["Success"] = "Удалено";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Ошибка";
            return View();
        }
    }
}
