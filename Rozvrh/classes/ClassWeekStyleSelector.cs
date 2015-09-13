using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Rozvrh {
    public class ClassWeekStyleSelector : DataTemplateSelector {
        public DataTemplate standardClassTemplate { get; set; }

        public DataTemplate wrongWeekClassTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) {
            var listItem = (ClassInstance)item;
            int currentWeek = System.Convert.ToInt32(Math.Ceiling((double)DateTime.Now.DayOfYear / 7)) % 2 != 0 ? 1 : 2;
            if (listItem.weekType == 0 || (int)listItem.weekType == currentWeek)
                return WeekView.resources["standardClassTemplate"] as DataTemplate;
            else
                return WeekView.resources["wrongWeekClassTemplate"] as DataTemplate;

        }
    }
}
