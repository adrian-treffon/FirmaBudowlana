using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirmaBudowlana.Core.Models
{
    public class WorkerTeam
    {
        public Guid WorkerID { get; set; }

        [NotMapped]
        public Worker Worker { get; set; }
        public Guid TeamID { get; set; }
        [NotMapped]
        public Team Team { get; set; }
    }
}
