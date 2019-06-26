using FirmaBudowlana.Infrastructure.Commands.User;
using FirmaBudowlana.Infrastructure.Services;
using Komis.Infrastructure.Commands;
using System;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.User
{
    public class RegisterHandler : ICommandHandler<Register>
    {
        private readonly IUserService _userService;

        public RegisterHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task HandleAsync(Register command)
        {
            if (command.UserCredentials == null) throw new Exception($"Post request account/register is empty");

            await _userService.Register(command.UserCredentials.FirstName, command.UserCredentials.LastName, command.UserCredentials.Address,
                command.UserCredentials.Email, command.UserCredentials.Password);
        }
    }
}
