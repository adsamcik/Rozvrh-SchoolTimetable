using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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

            string[] classTypes = new string[5];
            classTypes[0] = Data.loader.GetString("Standard");
            classTypes[1] = Data.loader.GetString("Lecture");
            classTypes[2] = Data.loader.GetString("Exercise");
            classTypes[3] = Data.loader.GetString("Lab");
            classTypes[4] = Data.loader.GetString("Workshop");
            comboBoxClassType.ItemsSource = classTypes;

            string[] days = new string[5];
            days[0] = Data.loader.GetString("Monday");
            days[1] = Data.loader.GetString("Tuesday");
            days[2] = Data.loader.GetString("Wednesday");
            days[3] = Data.loader.GetString("Thursday");
            days[4] = Data.loader.GetString("Friday");
            comboBoxDay.ItemsSource = days;

            string[] weekTypes = new string[3];
            weekTypes[0] = Data.loader.GetString("EveryWeek");
            weekTypes[1] = Data.loader.GetString("OddWeek");
            weekTypes[2] = Data.loader.GetString("EvenWeek");
            comboBoxWeek.ItemsSource = weekTypes;

            comboBoxTeacher.ItemsSource = Data.teachers;
        }

        bool Validate() {
            bool isValid = true;

            if (comboBoxClass.SelectedValue == null) {
                Extensions.Invalid(comboBoxClass);
                isValid = false;
            }
            else
                Extensions.Valid(comboBoxClass);

            if (comboBoxClassType.SelectedValue == null) {
                Extensions.Invalid(comboBoxClassType);
                isValid = false;
            }
            else
                Extensions.Valid(comboBoxClassType);

            if (comboBoxWeek.SelectedValue == null) {
                Extensions.Invalid(comboBoxWeek);
                isValid = false;
            }
            else
                Extensions.Valid(comboBoxWeek);

            //During testing didn't worked visualy, so disabled for now
            /*if (timePickerTo.Time < timePickerFrom.Time) {
                Extensions.Invalid(timePickerTo);
                isValid = false;
            }
            else
                Extensions.Valid(timePickerTo);*/

            if (comboBoxDay.SelectedValue == null) {
                Extensions.Invalid(comboBoxDay);
                isValid = false;
            }
            else
                Extensions.Valid(comboBoxDay);

            return isValid;
        }

        ClassInstance editObject;

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            ClassInstance cInstance = (ClassInstance)e.Parameter;

            if (cInstance != null) {
                comboBoxClass.SelectedIndex = Data.classes.FindIndex(x => x == cInstance.classData);
                comboBoxClassType.SelectedIndex = (int)cInstance.classType;
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
            if (Validate()) {
                NavigationCacheMode = NavigationCacheMode.Disabled;
                if (editObject != null) {
                    editObject.classData = (Class)comboBoxClass.SelectedItem;
                    editObject.classType = (ClassType)comboBoxClassType.SelectedIndex;
                    editObject.weekType = (WeekType)comboBoxWeek.SelectedIndex;
                    editObject.from = timePickerFrom.Time;
                    editObject.to = timePickerTo.Time;
                    editObject.room = textBoxRoom.Text;
                    editObject.day = (WeekDay)comboBoxDay.SelectedIndex;
                    editObject.teacher = (Teacher)comboBoxTeacher.SelectedItem;
                    Data.Save();
                }
                else
                    Data.AddClassInstance(new ClassInstance((Class)comboBoxClass.SelectedItem, (ClassType)comboBoxClassType.SelectedIndex, timePickerFrom.Time, timePickerTo.Time, textBoxRoom.Text, (WeekDay)comboBoxDay.SelectedIndex, (WeekType)comboBoxWeek.SelectedIndex, (Teacher)comboBoxTeacher.SelectedValue));

                Frame.GoBack();
            }
        }

        private void GoBack_Trigger(object sender, BackRequestedEventArgs e) {
            if (Frame.CurrentSourcePageType == this.GetType()) {
                NavigationCacheMode = NavigationCacheMode.Disabled;
            }
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
