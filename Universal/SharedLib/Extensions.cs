using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SharedLib {
    public static class Extensions {
        public static int GetIso8601WeekOfYear(DateTime date) {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) {
                date = date.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static int GetWeekOfYear(DateTime date) {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public static DateTime WhenIsNext(ClassInstance classInstance, DateTime now) {
            int currentDay = (int)now.DayOfWeek - 1;
            if (currentDay == -1) currentDay = 6;

            int expireIn;
            if (currentDay == (int)classInstance.day)
                expireIn = classInstance.from > now.TimeOfDay ? 0 : 7;
            else
                expireIn = currentDay <= (int)classInstance.day ?
                    (int)classInstance.day - currentDay :
                    7 - currentDay + (int)classInstance.day;

            now = now.AddDays(expireIn);

            if (classInstance.weekType != WeekType.EveryWeek)
                if (GetWeekOfYear(now) % 2 == 0 && classInstance.weekType == WeekType.OddWeek)
                    now = now.AddDays(7);
                else if (GetWeekOfYear(now) % 2 == 1 && classInstance.weekType == WeekType.EvenWeek)
                    now = now.AddDays(7);

            return new DateTime(now.Year, now.Month, now.Day, classInstance.from.Hours, classInstance.from.Minutes, classInstance.from.Seconds);
        }

        static Brush defaultBrush = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0));
        static Thickness defaultThickness = new Thickness(2);
        static Brush invalidBrush = new SolidColorBrush(Colors.Red);
        static Thickness invalidThickness = new Thickness(4);

        public static void Invalid(ComboBox comboBox) {
            comboBox.BorderBrush = invalidBrush;
            comboBox.BorderThickness = invalidThickness;
        }

        public static void Invalid(TextBox textBox) {
            textBox.BorderBrush = invalidBrush;
            textBox.BorderThickness = invalidThickness;
        }

        public static void Invalid(TimePicker timePicker) {
            timePicker.BorderBrush = invalidBrush;
            timePicker.BorderThickness = invalidThickness;
        }

        public static void Valid(ComboBox comboBox) {
            comboBox.BorderBrush = defaultBrush;
            comboBox.BorderThickness = defaultThickness;
        }

        public static void Valid(TextBox textBox) {
            textBox.BorderBrush = defaultBrush;
            textBox.BorderThickness = defaultThickness;
        }

        public static void Valid(TimePicker timePicker) {
            timePicker.BorderBrush = defaultBrush;
            timePicker.BorderThickness = defaultThickness;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
            var known = new HashSet<TKey>();
            return source.Where(element => known.Add(keySelector(element)));
        }
    }

    public class ClassInstanceTeacherComparer : IEqualityComparer<ClassInstance> {

        private Func<ClassInstance, object> _funcDistinct;
        public ClassInstanceTeacherComparer(Func<ClassInstance, object> funcDistinct) {
            this._funcDistinct = funcDistinct;
        }

        public bool Equals(ClassInstance x, ClassInstance y) {
            return _funcDistinct(x).Equals(_funcDistinct(y));
        }

        public int GetHashCode(ClassInstance obj) {
            return obj.GetHashCode();
        }
    }
}
