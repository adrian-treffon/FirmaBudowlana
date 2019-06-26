using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;


namespace FirmaBudowlana.Infrastructure.Commands.Order
{
    public class ValidateOrder : ICommand
    {
        public AdminOrderDTO Order { get; set; }
    }
}
