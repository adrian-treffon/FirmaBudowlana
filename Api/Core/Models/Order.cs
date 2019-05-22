using System;
using System.Collections.Generic;

namespace FirmaBudowlana.Core.Models
{
    public class Order
    {
       public Guid OrderID { get; set; }
       public DateTime StartDate { get; set; }
       public DateTime? EndDate { get; set; }
       public decimal? Cost { get; set; }
       public string Description { get; set; }
       public bool Validated { get; set; }
       public Guid UserID { get; set; }
       public IEnumerable<OrderTeam> OrdersTeams { get; set; }
    }
}
