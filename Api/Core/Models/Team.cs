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
        public List<WorkerTeam> WorkerTeam { get; set; }
        [JsonIgnore]
        public List<OrderTeam> OrderTeam { get; set; }

        public Team()
        {
            WorkerTeam = new List<WorkerTeam>();
            OrderTeam = new List<OrderTeam>();
        }
    } 
        
}
