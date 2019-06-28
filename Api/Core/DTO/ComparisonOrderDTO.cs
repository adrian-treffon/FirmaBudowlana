using FirmaBudowlana.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Core.DTO
{
    public class ComparisonOrderDTO
    {
        public Guid OrderID { get; set; }
        public Guid UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }

        public List<TeamDTO> Teams { get; set; }
        public bool Paid { get; set; }

        public ComparisonOrderDTO()
        {
            Teams = new List<TeamDTO>();
        }
    }
}
