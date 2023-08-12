using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaApi")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        protected APIResponse _response;
        public readonly ILogger<VillaApiController> _logger;
        public readonly IVillaRepository _dbvilla;
        public readonly IMapper _mapper;

        public VillaApiController(ILogger<VillaApiController> logger, IVillaRepository dbvilla, IMapper map)
        {
            _logger = logger;
            _dbvilla = dbvilla;
            _mapper = map;
            this._response = new();
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Getting all Villas");
                var villas = await _dbvilla.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaDTO>>(villas);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = new List<String>() { ex.Message };
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Id can't be zero");
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var villa = await _dbvilla.GetAsync((u => u.Id == id));
                if (villa == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaDTO>(villa);
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.ErrorMessage = new List<String>() { ex.Message };
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVilla(VillaCreateDTO villaDTO)
        {
            try
            {
                if (await _dbvilla.GetAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("Custom error", "Villa already exists");
                    _response.IsSuccess = false;
                    return BadRequest(ModelState);
                }
                if (villaDTO == null)
                {
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var villa = _mapper.Map<Villa>(villaDTO);
                await _dbvilla.CreateAsync(villa);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaDTO>(villa);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = new List<String>() { ex.Message };
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

        }


        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var villa = _dbvilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _dbvilla.RemoveAsync(villa.Result);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = new List<String>() { ex.Message };
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO villadto)
        {
            try
            {
                if (id <= 0 || id != villadto.Id || villadto == null) return BadRequest();
                var villa = await _dbvilla.GetAsync(x => x.Id == id);
                if (villa == null) { 
                    _response.IsSuccess=false;
                    return NotFound(_response); 
                }
                var model = _mapper.Map<Villa>(villadto);
                var result = await _dbvilla.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaDTO>(result);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = new List<String>() { ex.Message };
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> villaDTO)
        {
            if (id <= 0 || villaDTO == null) return BadRequest();
            var villa = _dbvilla.GetAsync(x => x.Id == id);
            if (villa == null) return NotFound();
            var villaDTOObject = _mapper.Map<VillaDTO>(villaDTO);
            villaDTO.ApplyTo(villaDTOObject, ModelState);
            var model = _mapper.Map<Villa>(villaDTOObject);
            await _dbvilla.UpdateAsync(model);
            return NoContent();

        }
    }
}
