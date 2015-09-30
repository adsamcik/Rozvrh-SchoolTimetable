using SharedLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Main {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddTask : Page {
        List<Class> classes { get { return Data.classes; } }

        public AddTask() {
            this.InitializeComponent();
            comboBoxClass.ItemsSource = Data.classes;
        }

        private void Ok_Click(object sender, RoutedEventArgs e) {
            if (Validate()) {
                DateTimeOffset dto = datePickerDeadline.Date;
                TimeSpan ts = timePickerDeadline.Time;
                DateTime dateTime = new DateTime(dto.Year, dto.Month, dto.Day, ts.Hours, ts.Minutes, ts.Seconds);

                if (taskInstance != null) {
                    taskInstance = Data.tasks.Find(x => x.uid == taskInstance.uid);
                    taskInstance.title = textBoxTaskTitle.Text;
                    taskInstance.description = textBoxDescription.Text;
                    taskInstance.deadline = dateTime;
                    taskInstance.notifyInDays = (int)sliderNotify.Value;
                    taskInstance.classTarget = (Class)comboBoxClass.SelectedItem;
                    Data.Save();
                }
                else {
                    taskInstance = new Task(
                        textBoxTaskTitle.Text,
                        textBoxDescription.Text,
                        dateTime,
                        (int)sliderNotify.Value,
                        (Class)comboBoxClass.SelectedItem);
                    Data.AddTask(taskInstance);
                }

                NotificationManager.RemoveScheduledNotification(taskInstance);
                if (sliderNotify.Value != 0)
                    NotificationManager.CreateTileNotification(taskInstance);

                BackgroundTasks.LiveTileBackgroundUpdater.PrepareLiveTile();

                Frame.GoBack();
            }
        }

        bool Validate() {
            bool isValid =true;
            if (string.IsNullOrWhiteSpace(textBoxTaskTitle.Text)) {
                Extensions.Invalid(textBoxTaskTitle);
                isValid = false;
            }
            else
                Extensions.Valid(textBoxTaskTitle);

            return isValid;
        }

        Task taskInstance;

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.Parameter != null) {
                taskInstance = (Task)e.Parameter;
                textBoxTaskTitle.Text = taskInstance.title;
                datePickerDeadline.Date = taskInstance.deadline.Date;
                timePickerDeadline.Time = taskInstance.deadline.TimeOfDay;
                comboBoxClass.SelectedIndex = Data.classes.FindIndex(x => x == taskInstance.classTarget);
                sliderNotify.Value = taskInstance.notifyInDays;
                textBoxDescription.Text = taskInstance.description;
                buttonDelete.Visibility = Visibility.Visible;
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Frame.GoBack();
        }

        private void buttonDeleteConfirm_Click(object sender, RoutedEventArgs e) {
            Data.DeleteTask(Data.tasks.Find(x => x.uid == taskInstance.uid));
            Frame.GoBack();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e) {
            FlyoutBase.ShowAttachedFlyout(buttonDelete);
        }
    }
}
