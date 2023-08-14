using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Service.IService
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO obj);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO obj);
    }
}
