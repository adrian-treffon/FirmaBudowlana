using System;
using System.Collections.Generic;

namespace FirmaBudowlana.Core.DTO
{
    public class ReportDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<WorkerDTO> Workers { get; set; }
        public IEnumerable<TeamDTO> Teams { get; set; }
    }
}
