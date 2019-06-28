
using FirmaBudowlana.Core.Models;
using System;
using System.Collections.Generic;

namespace FirmaBudowlana.Core.DTO
{
    public class WorkerDTO
    {
        public Guid WorkerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public decimal ManHour { get; set; }

        public List<Team> Teams { get; set; }

        public WorkerDTO()
        {
            Teams = new List<Team>();
        }
    }
}
