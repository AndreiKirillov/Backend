using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SubscriptionManager.Models;

namespace SubscriptionManager.Services
{
    public static class SubscriptionService
    {

        public static Subscription GetNearestSubscription(List<Subscription> subs)
        {
            if (subs.Count < 1)
                throw new Exception("Empty input data!");

            Subscription result = subs[0];

            foreach(var sub in subs)
            {
                if (sub.PaymentDate < result.PaymentDate)
                    result = sub;
            }

            return result;
        }

    }
}
