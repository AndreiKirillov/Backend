using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public double Price { get; set; }

        [JsonIgnore]
        public Category _Category { get; set; }

    }
}
