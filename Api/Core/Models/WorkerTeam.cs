using System;


namespace FirmaBudowlana.Core.Models
{
    public class WorkerTeam
    {
        public Guid WorkerID { get; set; }
        public Worker Worker { get; set; }

        public Guid TeamID { get; set; }
        public Team Team { get; set; }
    }
}
