using Newtonsoft.Json;
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
       public bool Paid { get; set; }
       public Guid UserID { get; set; }

       [JsonIgnore]
       public List<OrderTeam> OrdersTeams { get; set; }
    }
}
