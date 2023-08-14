using AutoMapper;
using MagicVilla_VillaAPI.Controllers.v1;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers.v2
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("2.0")]
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
            _response = new();
        }


        [HttpGet]
        //[MapToApiVersion("2.0")]
        public IEnumerable<string> Get()
        {
            return new string[] { "1", "2" };
        }



    }

}
