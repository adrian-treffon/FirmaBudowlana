using System;

namespace FirmaBudowlana.Core.DTO
{
    public class PaymentDTO
    {
        public Guid WorkerID { get; set; }
        public Guid OrderID { get; set; }
        public decimal Amount { get; set; }
    }
}
