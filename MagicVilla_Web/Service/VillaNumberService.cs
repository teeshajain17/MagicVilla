using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Service.IService;
using static MagicVilla_Utilities.SD;

namespace MagicVilla_Web.Service
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string villaAPIUrl;
        public VillaNumberService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            villaAPIUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.POST,
                Data = dto,
                Url = villaAPIUrl + "/api/v1/VillaNumberApi",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.DELETE,
                Url = villaAPIUrl + "/api/v1/VillaNumberApi/" + id,
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.GET,
                Url = villaAPIUrl + "/api/v1/VillaNumberApi/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.GET,
                Url = villaAPIUrl + "/api/v1/VillaNumberApi/",
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = ApiType.PUT,
                Data = dto,
                Url = villaAPIUrl + "/api/v1/VillaNumberApi/" + dto.VillaNo,
                Token = token
            });
        }
    }
}
