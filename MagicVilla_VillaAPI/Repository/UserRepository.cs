using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepositories;
using Microsoft.IdentityModel.Tokens;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secret;
        public UserRepository(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            secret = config.GetValue<string>("AppSetting:Secret");
            
        }
        public bool IsUniqueUser(string username)
        {
            LocalUser user = _db.LocalUsers.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
            return user == null;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            LocalUser? user = _db.LocalUsers.FirstOrDefault(x => x.Username.ToLower() == loginRequestDTO.UserName.ToLower()
            && x.Password == loginRequestDTO.Password);

            if(user == null)
            {
                return new()
                {
                    User = null,
                    Token = ""
                };
            }
            //Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.Now.AddDays(7)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new()
            {
                User = user,
                Token = tokenHandler.WriteToken(token)
            };
            return loginResponseDTO;
        }

        public async Task<LocalUser> Register(RegisterationRequestDTO registerRequestDTO)
        {
            LocalUser user = new()
            {
                Username = registerRequestDTO.Username,
                Name = registerRequestDTO.Name,
                Password = registerRequestDTO.Password,
                Role = registerRequestDTO.Role
            };
            await _db.LocalUsers.AddAsync(user);
            await _db.SaveChangesAsync();
            user.Password = "";
            return user;

        }
    }
}
