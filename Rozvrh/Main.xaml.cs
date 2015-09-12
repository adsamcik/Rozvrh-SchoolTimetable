using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Rozvrh {

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Main : Page {
        public static Main instance;

        public Main() {
            this.InitializeComponent();
            instance = this;

            Data.Initialize();

            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            // Title bar colors. Alpha must be 255.
            titleBar.BackgroundColor = new Color() { A = 255, R = 60, G = 120, B = 240 };
            titleBar.ForegroundColor = new Color() { A = 255, R = 220, G = 220, B = 255 };
            titleBar.InactiveBackgroundColor = new Color() { A = 255, R = 60, G = 120, B = 240 };
            titleBar.InactiveForegroundColor = new Color() { A = 255, R = 220, G = 220, B = 255 };

            // Title bar button background colors. Alpha is respected when the view is extended
            // into the title bar (see scenario 2). Otherwise, Alpha is ignored and treated as if it were 255.
            titleBar.ButtonBackgroundColor = new Color() { A = 255, R = 60, G = 120, B = 240 };
            titleBar.ButtonHoverBackgroundColor = new Color() { A = 255, R = 50, G = 100, B = 200 };
            titleBar.ButtonPressedBackgroundColor = new Color() { A = 255, R = 232, G = 211, B = 162 };
            titleBar.ButtonInactiveBackgroundColor = new Color() { A = 255, R = 135, G = 141, B = 199 };
            

            // Title bar button foreground colors. Alpha must be 255.
            titleBar.ButtonForegroundColor = new Color() { A = 255, R = 220, G = 220, B = 255 };
            titleBar.ButtonHoverForegroundColor = new Color() { A = 255, R = 220, G = 220, B = 255 };
            titleBar.ButtonPressedForegroundColor = new Color() { A = 255, R = 220, G = 220, B = 255 };
            titleBar.ButtonInactiveForegroundColor = new Color() { A = 255, R = 220, G = 220, B = 255 };

            Content.Navigate(typeof(WeekView));

            SystemNavigationManager.GetForCurrentView().BackRequested += (sender, args) => {
                if (Content.CanGoBack)
                    Content.GoBack();
            };

            ResourceLoader resourceLoader = new ResourceLoader();
            foreach (var link in _navLinks)
                link.Label = resourceLoader.GetString(link.Label);

            foreach (var link in _displayStyles)
                link.Label = resourceLoader.GetString(link.Label);
        }

        private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e) {
            NavLink link = (NavLink)e.ClickedItem;
            Content.Navigate(link.Page);
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e) {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }

        public ObservableCollection<NavLink> NavLinks {get { return _navLinks;} }
        private ObservableCollection<NavLink> _navLinks = new ObservableCollection<NavLink>()
        {
            new NavLink() { Label = "Add", Symbol = Symbol.Add, Page = typeof(AddClassInstance)  },
            new NavLink() { Label = "ClassClass", Symbol = Symbol.Library, Page = typeof(AddClass) },
            new NavLink() { Label = "Teachers", Symbol = Symbol.People, Page = typeof(AddTeacher) }
        };

        public ObservableCollection<NavLink> DisplayStyles {get {return _displayStyles;}}
        private ObservableCollection<NavLink> _displayStyles = new ObservableCollection<NavLink>()
        {
            new NavLink() { Label = "AgendaView", Symbol = Symbol.SlideShow, Page = typeof(Agenda)  },
            new NavLink() { Label = "WeekView", Symbol = Symbol.CalendarWeek, Page = typeof(WeekView) }
        };

        private void Content_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e) {
            if (Content.CanGoBack)
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }
    }

    public class NavLink {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public Type Page { get; set; }
    }
}
