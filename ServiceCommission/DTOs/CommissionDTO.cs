using ServiceCommission.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCommission.DTOs
{
    public class CommissionDTO
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int Order { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required field")]
        public SituationEnum Situation { get; set; }

        [Required(ErrorMessage = "Required field")]
        public decimal Value { get; set; }

        private string _note;
        public string Note { get => _note ?? string.Empty; set => _note = value; }
    }
}
