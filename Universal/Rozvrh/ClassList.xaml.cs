using SharedLib;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rozvrh {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClassList : Page {
        string addClassString { get { return Data.loader.GetString("AddClass"); } }

        List<Class> classList { get { return Data.classes; } }

        public ClassList() {
            this.InitializeComponent();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e) {
            Frame.Navigate(typeof(AddClass), e.ClickedItem);
        }

        private void AddClassButton_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(AddClass));
        }
    }
}
