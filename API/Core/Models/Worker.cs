using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace FirmaBudowlana.Core.Models
{
    public class Worker
    {
        public Guid WorkerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public decimal ManHour { get; set; }

        public bool Active { get; set; } = true;

        [JsonIgnore]
        public IEnumerable<WorkerTeam> WorkerTeam { get; set; }
    }
}
