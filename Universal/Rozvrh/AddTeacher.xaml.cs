using SharedLib;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rozvrh {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddTeacher : Page {
        public AddTeacher() {
            this.InitializeComponent();
        }

        bool Validate() {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(textBoxName.Text)) {
                Extensions.Invalid(textBoxName);
                isValid = false;
            }
            else
                Extensions.Valid(textBoxName);

            if (string.IsNullOrWhiteSpace(textBoxSurname.Text)) {
                Extensions.Invalid(textBoxSurname);
                isValid = false;
            }
            else
                Extensions.Valid(textBoxSurname);

            return isValid;
        }

        Teacher teacherInstance;

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.Parameter != null && e.Parameter.GetType() == typeof(Teacher)) {
                teacherInstance = (Teacher)e.Parameter;
                textBoxDegree.Text = teacherInstance.degree;
                textBoxName.Text = teacherInstance.name;
                textBoxSurname.Text = teacherInstance.surname;
                textBoxEmail.Text = teacherInstance.email;
                textBoxPhone.Text = teacherInstance.phone;
                buttonDelete.Visibility = Visibility.Visible;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            if (Validate()) {
                if (teacherInstance != null) {
                    teacherInstance.degree = textBoxDegree.Text;
                    teacherInstance.name = textBoxName.Text;
                    teacherInstance.surname = textBoxSurname.Text;
                    teacherInstance.email = textBoxEmail.Text;
                    teacherInstance.phone = textBoxPhone.Text;
                    Data.Save();
                }
                else
                    Data.AddTeacher(new Teacher(textBoxName.Text, textBoxSurname.Text, textBoxEmail.Text, textBoxPhone.Text, textBoxDegree.Text));

                BackgroundTasks.LiveTileBackgroundUpdater.PrepareLiveTile();
                Frame.GoBack();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Frame.GoBack();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e) {
            FlyoutBase.ShowAttachedFlyout(buttonDelete);
        }

        private void buttonDeleteConfirm_Click(object sender, RoutedEventArgs e) {
            Data.DeleteTeacher(teacherInstance);
            Frame.GoBack();
        }
    }
}
