using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;
using Microsoft.Extensions.Primitives;
using System;
using System.Security.Claims;


namespace FirmaBudowlana.Infrastructure.Commands.User
{
    public class GetUserSpecifyOrder : ICommand
    {
        public StringValues Token { get; set; }
        public Guid UserID { get; set; }
        public Guid OrderID { get; set; }
        public ClaimsPrincipal User { get; set; }
        public AdminOrderDTO Order { get; set; }
    }
}
