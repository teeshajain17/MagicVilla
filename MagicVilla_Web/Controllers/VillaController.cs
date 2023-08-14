using MagicVilla_Utilities;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        public readonly IVillaService _villaService;

        public VillaController(IVillaService villaService)
        {
            _villaService = villaService;
        }
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> IndexVilla()
        {
            var list = new List<VillaDTO>();
            var response = await _villaService.GetAllAsync<APIResponse>(HttpContext?.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> CreateVilla()

        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {

            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponse>(model, HttpContext?.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Created successfully.";
                    return RedirectToAction("IndexVilla");
                }
            }
            TempData["error"] = "An Error Occured.";
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId, HttpContext?.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<VillaUpdateDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            else if (response.ErrorMessage?.Count > 0)
            {
                ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {

            if (ModelState.IsValid)
            {
                var response = await _villaService.UpdateAsync<APIResponse>(model, HttpContext?.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Updated successfully.";
                    return RedirectToAction("IndexVilla");
                }
                else if (response.ErrorMessage.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());
                }
            }
            TempData["error"] = "An Error Occured.";
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId, HttpContext?.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            else if (response.ErrorMessage.Count > 0)
            {
                ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {
            var response = await _villaService.DeleteAsync<APIResponse>(model.Id, HttpContext?.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Deleted successfully.";
                return RedirectToAction("IndexVilla");
            }
            else if (response.ErrorMessage?.Count > 0)
            {
                ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());
            }
            TempData["error"] = "An Error Occured.";
            return View(model);
        }

    }
}
