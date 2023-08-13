using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        public readonly ILogger<VillaApiController> _logger;
        public readonly IVillaNumberRepository _dbVillaNumber;
        public readonly IVillaRepository _dbVilla;
        public readonly IMapper _mapper;

        public VillaNumberAPIController(ILogger<VillaApiController> logger, IVillaNumberRepository dbvillaNumber, 
            IMapper map, IVillaRepository dbvilla)
        {
            _logger = logger;
            _dbVillaNumber = dbvillaNumber;
            _dbVilla = dbvilla;
            _mapper = map;
            this._response = new();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                _logger.LogInformation("Getting all Villa Numbers");
                var villaNumbers = await _dbVillaNumber.GetAllAsync(includeProperties: "Villa");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumbers);
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
        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Id can't be zero");
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var villaNumber = await _dbVillaNumber.GetAsync((u => u.VillaNo == id), includeProperties:"Villa");
                if (villaNumber == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
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
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumberDTO)
        {
            try
            {
                if (await _dbVillaNumber.GetAsync(u => u.VillaNo == villaNumberDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessage", "VillaNumber already exists");
                    _response.IsSuccess = false;
                    return BadRequest(ModelState);
                }
                var ss = await _dbVilla.GetAsync(u => u.Id == villaNumberDTO.VillaId);
                if (await _dbVilla.GetAsync(u => u.Id == villaNumberDTO.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessage", "VillaId not exist");
                    _response.IsSuccess = false;
                    return BadRequest(ModelState);
                }
                if (villaNumberDTO == null)
                {
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var villaNumber = _mapper.Map<VillaNumber>(villaNumberDTO);
                await _dbVillaNumber.CreateAsync(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = new List<String>() { ex.Message };
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

        }


        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var villaNumber = _dbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _dbVillaNumber.RemoveAsync(villaNumber.Result);
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

        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO villaNumberDTO)
        {
            try
            {
                if (id <= 0 || id != villaNumberDTO.VillaNo || villaNumberDTO == null) return BadRequest();
                var villaNumber = await _dbVillaNumber.GetAsync(x => x.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                var model = _mapper.Map<VillaNumber>(villaNumberDTO);
                var result = await _dbVillaNumber.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaNumberDTO>(result);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = new List<String>() { ex.Message };
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

        }

    }
}
