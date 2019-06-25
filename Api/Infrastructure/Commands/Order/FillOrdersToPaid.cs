using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;
using System.Collections.Generic;


namespace FirmaBudowlana.Infrastructure.Commands.Order
{
    public class FillOrdersToPaid : ICommand
    {
        public IEnumerable<OrderToPaidDTO> Orders {get;set;}
    }
}
