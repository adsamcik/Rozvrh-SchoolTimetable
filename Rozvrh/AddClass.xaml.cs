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
    public sealed partial class AddClass : Page {
        public AddClass() {
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
                Data.AddClass(new Class(textBoxName.Text, textBoxShortName.Text));
                Frame.GoBack();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Frame.GoBack();
        }
    }
}
