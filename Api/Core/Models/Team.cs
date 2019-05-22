using System;
using System.Collections.Generic;

namespace FirmaBudowlana.Core.Models
{
    public class Team
    {
        public Guid TeamID { get; set; }
        public string Description { get; set; }
        public IEnumerable<WorkerTeam> WorkersTeams { get; set; }
        public IEnumerable<OrderTeam> OrdersTeams { get; set; }
    } 
        
}
