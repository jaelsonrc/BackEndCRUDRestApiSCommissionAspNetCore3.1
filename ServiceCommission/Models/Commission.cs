using ServiceCommission.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCommission.Models
{
    public class Commission: Entity
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public User User { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public SituationEnum Situation { get; set; }
        public decimal Value { get; set; }
        public string Note { get; set; }
    }
}
