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

        public static List<Subscription> GetDebtSubscriptions(List<Subscription> subs)
        {
            if (subs.Count < 1)
                throw new Exception("Empty input data!");

            List<Subscription> debt_subs = new List<Subscription>();

            foreach(var sub in subs)
            {
                if (sub.PaymentDate < DateTime.Today)
                    debt_subs.Add(sub);
            }

            return debt_subs;
        }

        public static void RefreshSubscription(ref Subscription sub)
        {
            if (sub == null)
                throw new Exception("Empty input data!");

            sub.PaymentDate = sub.PaymentDate.AddMonths(1);
        }

        public static void RefreshAllSubscriptions(ref List<Subscription> subs)
        {
            if (subs.Count < 1)
                throw new Exception("Empty input data!");

            foreach (var sub in subs)
            {
                if (sub.PaymentDate < DateTime.Today)     // 
                    sub.PaymentDate = sub.PaymentDate.AddMonths(1);          
            }
        }

    }
}
