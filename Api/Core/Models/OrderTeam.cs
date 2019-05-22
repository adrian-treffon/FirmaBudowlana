using System;

namespace FirmaBudowlana.Core.Models
{
    public class OrderTeam
    {
        public Guid OrderID { get; set; }
        public Order Order { get; set; }

        public Guid TeamID { get; set; }
        public Team Team { get; set; }
    }
}
