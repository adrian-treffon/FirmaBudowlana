using FirmaBudowlana.Infrastructure.Commands.User;
using FirmaBudowlana.Infrastructure.Exceptions;
using FirmaBudowlana.Infrastructure.Services;
using Komis.Infrastructure.Commands;
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
            if (command.UserCredentials == null) throw new ServiceException(ErrorCodes.PustyRequest,$"Post request account/register is empty");

            await _userService.Register(command.UserCredentials.FirstName, command.UserCredentials.LastName, command.UserCredentials.Address,
                command.UserCredentials.Email, command.UserCredentials.Password);
        }
    }
}
