using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;
using System;

namespace FirmaBudowlana.Infrastructure.Commands.Order
{
    public class GetInvalidatedOrder: ICommand
    {
        public AdminOrderDTO Order { get; set; }
        public Guid OrderID { get; set; }
    }
}
