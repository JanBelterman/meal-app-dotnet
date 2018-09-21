using System;
using System.Collections.Generic;

namespace MaaltijdApplicatie.Models.Logic {

    public static class DateTimeExtensions {

        public static IEnumerable<DateTime> GetDatesForComingTwoWeeks(this DateTime startDate) {

            var dates = new List<DateTime>();

            var endDate = startDate.AddDays(14);

            for (var dt = startDate; dt < endDate; dt = dt.AddDays(1)) {
                dates.Add(dt);
            }

            return dates;

        }

    }

}
