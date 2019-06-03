using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using FirmaBudowlana.Core.Models;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AutoMapper;



namespace FirmaBudowlana.Infrastructure.Services
{
    class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly IEncrypter _encrypter;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        private readonly IUserRepository _userRepository;

       
        public UserService(IOptions<AppSettings> appSettings, IEncrypter encrypter, Microsoft.Extensions.Configuration.IConfiguration config, IUserRepository userRepository)
        {
            _appSettings = appSettings.Value;
            _config = config;
            _encrypter = encrypter;
            _userRepository = userRepository;
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);

            // return null if user not found
            if (user == null)
                return null;

            var hash = _encrypter.GetHash(password, user.Salt);

            // return null if wrong password
            if (user.Password != hash)
                return null;


            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),

                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = creds
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

       
        public async Task Register(string firstName, string lastName, string address, string email, string password, string role="User")
        {
            var user = await _userRepository.GetAsync(email);

            if (user != null)
            {
                throw new Exception($"User with email: '{email}' already exists.");
            }

            var salt = _encrypter.GetSalt(password);
            var hash = _encrypter.GetHash(password, salt);

            user = new User
             { UserID = Guid.NewGuid(), Address = address, Email = email, FirstName = firstName, LastName = lastName, Password = hash, Role = role
            , Salt = salt, CreatedAt = DateTime.UtcNow };
            
            await _userRepository.AddAsync(user);
        }
    }
}
