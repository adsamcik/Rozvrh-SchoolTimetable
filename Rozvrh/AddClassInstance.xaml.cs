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
    public sealed partial class AddClassInstance : Page {
        public AddClassInstance() {
            this.InitializeComponent();
            comboBoxClass.ItemsSource = Data.classes;

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            string[] days = new string[5];

            days[0] = loader.GetString("Monday");
            days[1] = loader.GetString("Tuesday");
            days[2] = loader.GetString("Wednesday");
            days[3] = loader.GetString("Thursday");
            days[4] = loader.GetString("Friday");

            comboBoxDay.ItemsSource = days;
        }

        private void addButton_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(AddClass));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if(Frame.CanGoBack)
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e) {
            Data.classInstances.Add(new ClassInstance((Class)comboBoxClass.SelectedItem, timePickerFrom.Time, timePickerTo.Time, textBoxRoom.Text, (classes.WeekDay)comboBoxDay.SelectedIndex));
            Frame.GoBack();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e) {
            Frame.GoBack();
        }
    }
}
