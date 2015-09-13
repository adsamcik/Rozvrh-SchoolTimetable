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

            SystemNavigationManager.GetForCurrentView().BackRequested += GoBack_Trigger;

            comboBoxClass.ItemsSource = Data.classes;

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            string[] days = new string[5];

            days[0] = loader.GetString("Monday");
            days[1] = loader.GetString("Tuesday");
            days[2] = loader.GetString("Wednesday");
            days[3] = loader.GetString("Thursday");
            days[4] = loader.GetString("Friday");

            comboBoxDay.ItemsSource = days;

            string[] weekTypes = new string[3];
            weekTypes[0] = loader.GetString("EveryWeek");
            weekTypes[1] = loader.GetString("OddWeek");
            weekTypes[2] = loader.GetString("EvenWeek");

            comboBoxWeek.ItemsSource = weekTypes;
            comboBoxTeacher.ItemsSource = Data.teachers;
        }

        ClassInstance editObject;

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            ClassInstance cInstance = (ClassInstance)e.Parameter;

            if (cInstance != null) {
                comboBoxClass.SelectedIndex = Data.classes.FindIndex(x => x == cInstance.classData);
                comboBoxWeek.SelectedIndex = (int)cInstance.weekType;
                timePickerFrom.Time = cInstance.from;
                timePickerTo.Time = cInstance.to;
                textBoxRoom.Text = cInstance.room;
                comboBoxDay.SelectedIndex = (int)cInstance.day;
                comboBoxTeacher.SelectedIndex = Data.teachers.FindIndex(x => x == cInstance.teacher);
                editObject = cInstance;
            }
        }


        private void Ok_Click(object sender, RoutedEventArgs e) {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            if (editObject != null) {
                editObject.classData = (Class)comboBoxClass.SelectedItem;
                editObject.weekType = (WeekType)comboBoxWeek.SelectedIndex;
                editObject.from = timePickerFrom.Time;
                editObject.to = timePickerTo.Time;
                editObject.room = textBoxRoom.Text;
                editObject.day = (classes.WeekDay)comboBoxDay.SelectedIndex;
                editObject.teacher = (Teacher)comboBoxTeacher.SelectedItem;
                Data.Save();
            }
            else
                Data.AddClassInstance(new ClassInstance((Class)comboBoxClass.SelectedItem, timePickerFrom.Time, timePickerTo.Time, textBoxRoom.Text, (classes.WeekDay)comboBoxDay.SelectedIndex, (WeekType)comboBoxWeek.SelectedIndex, (Teacher)comboBoxTeacher.SelectedValue));

            Frame.GoBack();
        }

        private void GoBack_Trigger(object sender, BackRequestedEventArgs e) {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            Frame.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            Frame.GoBack();
        }

        private void AddClass_Click(object sender, RoutedEventArgs e) {
            NavigationCacheMode = NavigationCacheMode.Required;
            Frame.Navigate(typeof(AddClass));
        }

        private void AddTeacher_Click(object sender, RoutedEventArgs e) {
            NavigationCacheMode = NavigationCacheMode.Required;
            Frame.Navigate(typeof(AddTeacher), Frame);
        }
    }
}
