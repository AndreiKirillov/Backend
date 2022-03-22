using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FinanceCounterBackend.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public double Ammount;

        public Category category;

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
