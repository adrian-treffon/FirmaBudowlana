using FirmaBudowlana.Core.Models;
using System;
using System.Collections.Generic;


namespace FirmaBudowlana.Core.DTO
{
    public class OrderToPaidDTO
    {
        public Guid OrderID { get; set; }
        public string Description { get; set; }
        public List<TeamDTO> Teams { get; set; }
        public decimal PaymentCost { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public OrderToPaidDTO()
        {
            Teams = new List<TeamDTO>();
        }
    }
}
