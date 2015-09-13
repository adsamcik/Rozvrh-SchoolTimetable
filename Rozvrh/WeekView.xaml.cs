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

        List<ClassInstance> monday { get { return classInstances.FindAll(x => x.day == classes.WeekDay.Monday).OrderBy(x => x.from).ToList(); } }
        List<ClassInstance> tuesday { get { return classInstances.FindAll(x => x.day == classes.WeekDay.Tuesday).OrderBy(x => x.from).ToList(); } }
        List<ClassInstance> wednesday { get { return classInstances.FindAll(x => x.day == classes.WeekDay.Wednesday).OrderBy(x => x.from).ToList(); } }
        List<ClassInstance> thursday { get { return classInstances.FindAll(x => x.day == classes.WeekDay.Thursday).OrderBy(x => x.from).ToList(); } }
        List<ClassInstance> friday { get { return classInstances.FindAll(x => x.day == classes.WeekDay.Friday).OrderBy(x => x.from).ToList(); } }

        public WeekView() {
            this.InitializeComponent();
            resources = Resources;
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e) {
            Frame.Navigate(typeof(AddClassInstance), e.ClickedItem);
        }
    }
}
