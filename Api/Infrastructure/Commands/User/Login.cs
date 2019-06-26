using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;


namespace FirmaBudowlana.Infrastructure.Commands.User
{
    public class Login :ICommand
    {
        public UserLoginDTO LoginCredentials { get; set; }

        public TokenDTO Token { get; set; } 
    }
}
