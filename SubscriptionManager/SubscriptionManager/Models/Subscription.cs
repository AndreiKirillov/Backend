using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SubscriptionManager.Models
{
    public class Subscription
    {
        public int Id { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        public Category _Category { get; set; }

    }
}
