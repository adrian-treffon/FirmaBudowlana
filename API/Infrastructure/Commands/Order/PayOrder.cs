using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;

namespace FirmaBudowlana.Infrastructure.Commands.Order
{
    public class PayOrder : ICommand
    {
        public OrderToPaidDTO Order { get; set; }
    }
}
