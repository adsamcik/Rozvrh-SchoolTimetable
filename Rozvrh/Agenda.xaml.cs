using SharedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rozvrh {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Agenda : Page {
        public static ResourceDictionary resources;
        List<DisplayClass> upcomingList = new List<DisplayClass>();
        public Agenda() {
            resources = Resources;

            foreach (var classInstance in Data.classInstances)
                upcomingList.Add(new DisplayClass(classInstance));

            foreach (var taskInstance in Data.tasks)
                upcomingList.Add(new DisplayClass(taskInstance));

            upcomingList = upcomingList.OrderBy(x => x.taskInstance == null ? Extensions.WhenIsNext(x.classInstance) : x.taskInstance.deadline).ToList();

            this.InitializeComponent();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
            DisplayClass dc = (DisplayClass)e.ClickedItem;
            if (dc.classInstance != null)
                Frame.Navigate(typeof(AddClassInstance), dc.classInstance);
            else if (dc.taskInstance != null)
                Frame.Navigate(typeof(AddTask), dc.taskInstance);
        }
    }
}
