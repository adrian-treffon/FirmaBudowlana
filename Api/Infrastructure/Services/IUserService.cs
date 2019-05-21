using FirmaBudowlana.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Services
{
    public interface IUserService :IService
    {
        Task<SecurityToken> Login(string username, string password);

        Task Register(string firstName, string lastName, string address, string email, string password, string role = "User");
    }
}
