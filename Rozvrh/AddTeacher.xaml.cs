using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rozvrh {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddTeacher : Page {
        public AddTeacher() {
            this.InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            Data.AddTeacher(new Teacher(textBoxName.Text, textBoxSurname.Text, textBoxEmail.Text, textBoxPhone.Text, textBoxDegree.Text));
            Frame.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Frame.GoBack();
        }
    }
}
