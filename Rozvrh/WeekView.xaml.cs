using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using System.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rozvrh {

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeekView : Page {
        public static Windows.UI.Xaml.ResourceDictionary resources;
        List<ClassInstance> classInstances { get { return Data.classInstances; } }

        List<DisplayClass>[] week = new List<DisplayClass>[5];

        public WeekView() {
            for (int i = 0; i < week.Length; i++)
                week[i] = new List<DisplayClass>();

            foreach (var @class in classInstances)
                week[(int)@class.day].Add(new DisplayClass(@class));

            foreach (var task in Data.tasks)
                if ((int)task.deadline.DayOfWeek <= 5 && (int)task.deadline.DayOfWeek > 0)
                    week[(int)task.deadline.DayOfWeek - 1].Add(new DisplayClass(task));

            for (int i = 0; i < week.Length; i++)
                week[i] = week[i].OrderBy(x => x.classInstance != null ? x.classInstance.from : x.taskInstance.deadline.TimeOfDay).ToList();

            resources = Resources;

            this.InitializeComponent();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e) {
            DisplayClass dc = (DisplayClass)e.ClickedItem;
            if (dc.classInstance != null)
                Frame.Navigate(typeof(AddClassInstance), dc.classInstance);
            else if (dc.taskInstance != null)
                Frame.Navigate(typeof(AddTask), dc.taskInstance);
        }
    }
}
