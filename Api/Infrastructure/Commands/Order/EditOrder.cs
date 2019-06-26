using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;


namespace FirmaBudowlana.Infrastructure.Commands.Order
{
    public class EditOrder :ICommand
    {
        public AdminOrderDTO Order { get; set; }
    }
}
