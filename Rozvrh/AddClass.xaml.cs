using SharedLib;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rozvrh {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddClass : Page {
        public AddClass() {
            this.InitializeComponent();
        }

        Class editObject;

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.Parameter != null && e.Parameter.GetType() == typeof(Class)) {
                editObject = (Class)e.Parameter;
                textBoxName.Text = editObject.name;
                textBoxShortName.Text = editObject.shortName;
                buttonDelete.Visibility = Visibility.Visible;
            }
        }

        bool Validate() {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(textBoxName.Text)) {
                Extensions.Invalid(textBoxName);
                isValid = false;
            }
            else
                Extensions.Valid(textBoxName);

            if (string.IsNullOrWhiteSpace(textBoxShortName.Text)) {
                Extensions.Invalid(textBoxShortName);
                isValid = false;
            }
            else
                Extensions.Valid(textBoxShortName);

            return isValid;
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            if (Validate()) {
                if (editObject != null) {
                    editObject.name = textBoxName.Text;
                    editObject.shortName = textBoxShortName.Text;
                    Data.Save();
                }
                else
                    Data.AddClass(new Class(textBoxName.Text, textBoxShortName.Text));

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
            Data.DeleteClass(editObject);
            Frame.GoBack();
        }
    }
}
