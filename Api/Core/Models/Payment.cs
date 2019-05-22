using System;

namespace FirmaBudowlana.Core.Models
{
    public class Payment
    {
        public Guid PaymentID { get; set; }
        public Guid WorkerID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}
