using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;


namespace FirmaBudowlana.Infrastructure.Commands.User
{
    public class Register : ICommand
    {
        public UserRegisterDTO UserCredentials { get; set; }
    }
}
