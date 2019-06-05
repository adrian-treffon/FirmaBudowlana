using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FirmaBudowlana.Core.Models
{
    public class Team
    {
        public Guid TeamID { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public List<WorkerTeam> WorkersTeams { get; set; }
        [JsonIgnore]
        public List<OrderTeam> OrdersTeams { get; set; }
    } 
        
}
