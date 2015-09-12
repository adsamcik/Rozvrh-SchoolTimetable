using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rozvrh {

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeekView : Page {
        List<ClassInstance> classInstances { get { return Data.classInstances; } }

        List<ClassInstance> monday { get { return classInstances.FindAll(x => x.day == classes.WeekDay.Monday); } }
        List<ClassInstance> tuesday { get { return classInstances.FindAll(x => x.day == classes.WeekDay.Tuesday); } }
        List<ClassInstance> wednesday { get { return classInstances.FindAll(x => x.day == classes.WeekDay.Wednesday); } }
        List<ClassInstance> thursday { get { return classInstances.FindAll(x => x.day == classes.WeekDay.Thursday); } }
        List<ClassInstance> friday { get { return classInstances.FindAll(x => x.day == classes.WeekDay.Friday); } }

        public WeekView() {
            this.InitializeComponent();

        }
    }
}
