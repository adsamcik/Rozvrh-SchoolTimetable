using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Main {
    class AgendaStyleSelector : DataTemplateSelector {

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) {
            var listItem = (DisplayClass)item;

            if (listItem.classInstance != null)
                return Agenda.resources["standardClassTemplate"] as DataTemplate;
            else if (listItem.taskInstance != null)
                return Agenda.resources["standardTaskTemplate"] as DataTemplate;

            throw new ArgumentNullException("not task or classInstance");

        }
    }
}
