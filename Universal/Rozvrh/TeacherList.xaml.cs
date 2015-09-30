using SharedLib;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Main {

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeacherList : Page {
        string addClassString { get { return Data.loader.GetString("AddTeacher"); } }

        List<Teacher> teacherList { get { return Data.teachers; } }

        public TeacherList() {
            this.InitializeComponent();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e) {
            Frame.Navigate(typeof(AddTeacher), e.ClickedItem);
        }

        private void AddClassButton_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(AddTeacher));
        }
    }
}
