﻿using FirmaBudowlana.Core.Models;
using System;
using System.Collections.Generic;

namespace FirmaBudowlana.Core.DTO
{
    public class AdminOrderDTO
    {
        public Guid OrderID { get; set; }
        public Guid UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public bool Paid { get; set; }
        public bool Validated { get; set; }
        public IEnumerable<Team> Teams { get; set; }
    }
}
