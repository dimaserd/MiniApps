using CrocoShop.Web.Model.OwnedModels;
using System;
using System.Collections.Generic;

namespace CrocoShop.Web.Logic.Settings
{
    public class CourtSettings
    {
        public string[] CourtTypes { get; set; }
        public CourtDescription[] Courts { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeFinish { get; set; }

        public TimeSpan[] GetHourTimeSpans()
        {
            var result = new List<TimeSpan>();
            var current = TimeStart;
            while (current < TimeFinish)
            {
                result.Add(current);
                current = current.Add(TimeSpan.FromHours(1));
            }

            return result.ToArray();
        }
    }
}