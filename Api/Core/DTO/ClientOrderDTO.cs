using System;


namespace FirmaBudowlana.Core.DTO
{
    public class ClientOrderDTO
    {
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public Guid UserID { get; set; }
    }
}
