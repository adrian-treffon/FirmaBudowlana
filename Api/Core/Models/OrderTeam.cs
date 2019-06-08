using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmaBudowlana.Core.Models
{
    public class OrderTeam
    {
        public Guid OrderID { get; set; }
        [NotMapped]
        public Order Order { get; set; }

        public Guid TeamID { get; set; }
        [NotMapped]
        public Team Team { get; set; }
    }
}
