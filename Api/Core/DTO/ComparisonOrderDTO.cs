using FirmaBudowlana.Core.Models;
using System;
using System.Collections.Generic;

namespace FirmaBudowlana.Core.DTO
{
    public class ComparisonOrderDTO
    {
        public Guid OrderID { get; set; }
        public ComparisonUserDTO User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public List<TeamDTO> Teams { get; set; }
        public List<Payment> Payments { get; set; }
        public bool Paid { get; set; }
        

        public ComparisonOrderDTO()
        {
            Teams = new List<TeamDTO>();
            Payments = new List<Payment>();
        }
    }
}
