using Komis.Infrastructure.Commands;
using System;


namespace FirmaBudowlana.Infrastructure.Commands.Order
{
    public class DeleteOrder : ICommand
    {
        public Guid OrderID { get; set; }
    }
}
