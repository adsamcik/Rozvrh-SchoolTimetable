using SharedLib;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rozvrh {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page {
        public Settings() {
            this.InitializeComponent();
        }

        private void buttonExport_Click(object sender, RoutedEventArgs e) {
            Data.SaveToFile();
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e) {
            Data.LoadFromFile();
        }
    }
}
