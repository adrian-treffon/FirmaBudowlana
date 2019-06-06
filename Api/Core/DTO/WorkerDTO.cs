
using System;

namespace FirmaBudowlana.Core.DTO
{
    public class WorkerDTO
    {
        public Guid WorkerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public decimal ManHour { get; set; }
    }
}
