using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace FirmaBudowlana.Infrastructure.Commands.User
{
    public class GetUserOrders : ICommand
    {
        public StringValues Token { get; set; }
        public Guid UserID { get; set; }
        public ClaimsPrincipal User { get; set; }
        public IEnumerable<AdminOrderDTO> Orders { get; set; }
    }
}
