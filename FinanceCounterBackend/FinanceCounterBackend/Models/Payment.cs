using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FinanceCounterBackend.Models
{
    public class Payment
    {
        private int Id { get; set; }

        private double Ammount;

        private Category category;

        [DataType(DataType.Date)]
        private DateTime Date { get; set; }
    }
}
