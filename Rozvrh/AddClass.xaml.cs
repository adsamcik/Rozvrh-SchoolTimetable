﻿using System;
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
            comboBoxTeacher.ItemsSource = Data.teachers;
        }

        private void addTeacherButton_Click(object sender, RoutedEventArgs e) {
            Data.classes.Add(new Class(textBoxName.Text, textBoxShortName.Text, (Teacher)comboBoxTeacher.SelectedItem));
            Frame.Navigate(typeof(AddTeacher), Frame);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (Frame.CanGoBack)
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e) {
            Data.classes.Add(new Class(textBoxName.Text, textBoxShortName.Text, (Teacher)comboBoxTeacher.SelectedValue));
            Frame.GoBack();
        }
    }
}
