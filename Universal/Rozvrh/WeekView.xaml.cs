using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using System.Linq;
using System;
using SharedLib;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rozvrh {

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeekView : Page {
        public static Windows.UI.Xaml.ResourceDictionary resources;

        List<DisplayClass>[] week = new List<DisplayClass>[6];

        public WeekView() {
            resources = Resources;

            Initialize();

            Debug.WriteLine("IsFinished " + Data.loadingFinished);
        }

        async void Initialize() {
            while (!Data.loadingFinished) await System.Threading.Tasks.Task.Delay(10);

            DateTime now = DateTime.Now;
            for (int i = 0; i < week.Length; i++)
                week[i] = new List<DisplayClass>();

            List<ClassInstance> classList = Data.classInstances;

            foreach (var @class in classList)
                week[(int)@class.weekDay].Add(new DisplayClass(@class));

            for (int i = 0; i < Data.tasks.Count; i++) {
                Task task = Data.tasks[i];
                if (task.deadline < now) {
                    Data.ArchiveTask(task);
                    i--;
                }
                else {
                    if ((int)task.deadline.DayOfWeek <= 5 && (int)task.deadline.DayOfWeek > 0)
                        week[(int)task.deadline.DayOfWeek - 1].Add(new DisplayClass(task));
                    else
                        week[5].Add(new DisplayClass(task));
                }

            }


            for (int i = 0; i < week.Length; i++)
                week[i] = week[i].OrderBy(x => x.classInstance != null ? x.classInstance.from : x.taskInstance.deadline.TimeOfDay).ToList();

            this.InitializeComponent();

            gridViewMonday.ItemsSource = week[0];
            gridViewTuesday.ItemsSource = week[1];
            gridViewWednesday.ItemsSource = week[2];
            gridViewThursday.ItemsSource = week[3];
            gridViewFriday.ItemsSource = week[4];
            gridViewWeekend.ItemsSource = week[5];
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
