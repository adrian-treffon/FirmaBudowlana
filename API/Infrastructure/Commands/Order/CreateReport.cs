using FirmaBudowlana.Core.DTO;
using Komis.Infrastructure.Commands;
using System.Collections.Generic;


namespace FirmaBudowlana.Infrastructure.Commands.Order
{
    public class CreateReport : ICommand
    {
        public ReportDTO Report { get; set; }
        public List<ComparisonOrderDTO> Orders { get; set; } = new List<ComparisonOrderDTO>();
    }
}
