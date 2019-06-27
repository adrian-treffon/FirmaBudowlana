using FirmaBudowlana.Core.DTO;
using FirmaBudowlana.Core.Repositories;
using FirmaBudowlana.Infrastructure.Commands.User;
using FirmaBudowlana.Infrastructure.Exceptions;
using FirmaBudowlana.Infrastructure.Services;
using Komis.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Infrastructure.Handlers.User
{
    public class LoginHandler : ICommandHandler<Login>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
      

        public LoginHandler(IUserService userService, IUserRepository userRepository, IOrderRepository orderRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(Login command)
        {
            if (command.LoginCredentials == null) throw new ServiceException(ErrorCodes.PustyRequest,$"Post request account/login is empty");

            var token = await _userService.Login(command.LoginCredentials.Email, command.LoginCredentials.Password);

            var user = await _userRepository.GetAsync(command.LoginCredentials.Email);

            if (token == null && user == null)
                throw new ServiceException(ErrorCodes.NiepoprawnyFormat,"Niepoprawny login lub hasło" );

           command.Token = new TokenDTO()
            {
                Token = token,
                User = user
            };
        }
    }
}
