using System;
using System.Collections.Generic;

namespace FirmaBudowlana.Core.Models
{
    public class Team
    {
        public Guid TeamID { get; set; }
        public string Description { get; set; }

        public List<WorkerTeam> WorkersTeams { get; set; }
        public List<OrderTeam> OrdersTeams { get; set; }
    } 
        
}
