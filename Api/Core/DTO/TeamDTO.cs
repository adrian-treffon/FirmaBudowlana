using FirmaBudowlana.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Core.DTO
{
    public class TeamDTO
    {
        public Guid TeamID { get; set; }
        public string Description { get; set; }
        public List<Worker> Workers { get; set; }
    }
}
