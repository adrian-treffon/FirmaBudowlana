using FirmaBudowlana.Core.Models;
using System;
using System.Collections.Generic;


namespace FirmaBudowlana.Core.DTO
{
    public class OrderToPaidDTO
    {
        public Guid OrderID { get; set; }
        public string Description { get; set; }
        public IEnumerable<Guid> TeamsID { get; set; }
    }
}
