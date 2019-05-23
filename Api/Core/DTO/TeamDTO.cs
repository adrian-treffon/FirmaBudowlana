using FirmaBudowlana.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaBudowlana.Core.DTO
{
    public class TeamDTO
    {
        public string Description { get; set; }
        public IEnumerable<Worker> Workers { get; set; }
    }
}
