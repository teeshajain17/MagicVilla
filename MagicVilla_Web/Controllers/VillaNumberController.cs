using MagicVilla_Utilities;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        public readonly IVillaNumberService _villaNumberService;
        public readonly IVillaService _villaService;

        public VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _villaService = villaService;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexVillaNumber()
        {
            var list = new List<VillaNumberDTO>();
            var response = await _villaNumberService.GetAllAsync<APIResponse>(HttpContext?.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVillaNumber()

        {
            var villaNumberVM = new VillaNumberCreateVM();
            var response = await _villaService.GetAllAsync<APIResponse>(HttpContext?.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {

            if (ModelState.IsValid)
            {
                var res = await _villaNumberService.CreateAsync<APIResponse>(model.villaNumber, HttpContext?.Session.GetString(SD.SessionToken));
                if (res != null && res.IsSuccess && res.ErrorMessage?.Count ==0)
                {
                    TempData["success"] = "Villa Number Created successfully.";
                    return RedirectToAction("IndexVillaNumber");
                }
                else if (res?.ErrorMessage?.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessage", res.ErrorMessage.FirstOrDefault());
                }
            }
            var response = await _villaService.GetAllAsync<APIResponse>(HttpContext?.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            else if (response.ErrorMessage.Count > 0)
            {
                ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());
            }
            TempData["error"] = "An Error Occured."; 
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(int villaId)
        {
            var villaNumberVM = new VillaNumberUpdateVM();

            var response = await _villaNumberService.GetAsync<APIResponse>(villaId, HttpContext?.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<VillaNumberUpdateDTO>(Convert.ToString(response.Result));
                villaNumberVM.villaNumber = model;
            }
            response = await _villaService.GetAllAsync<APIResponse>(HttpContext?.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(villaNumberVM);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var res = await _villaNumberService.UpdateAsync<APIResponse>(model.villaNumber, HttpContext?.Session.GetString(SD.SessionToken));
                if (res != null && res.IsSuccess && res.ErrorMessage?.Count == 0)
                {
                    TempData["success"] = "Villa Number Updated successfully.";
                    return RedirectToAction("IndexVillaNumber");
                }
                else if (res?.ErrorMessage?.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessage", res.ErrorMessage.FirstOrDefault());
                }
            }
            var response = await _villaService.GetAllAsync<APIResponse>(HttpContext?.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            else if (response.ErrorMessage.Count > 0)
            {
                ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());
            }
            TempData["error"] = "An Error Occured.";
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVillaNumber(int villaId)
        {
            var villaNumberVM = new VillaNumberDeleteVM();

            var response = await _villaNumberService.GetAsync<APIResponse>(villaId, HttpContext?.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaNumberVM.villaNumber = model;
            }
            response = await _villaService.GetAllAsync<APIResponse>(HttpContext?.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(villaNumberVM);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {

            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.villaNumber.VillaNo, HttpContext?.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Number Deleted successfully.";
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            TempData["error"] = "An Error Occured.";
            return View(model);
        }

    }
}
