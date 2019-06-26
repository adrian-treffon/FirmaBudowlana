using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;
using System.Security.Claims;

namespace FirmaBudowlana.Infrastructure.Commands.Order
{
    public class AddClientOrder :ICommand
    {
        public ClientOrderDTO Order { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
}
