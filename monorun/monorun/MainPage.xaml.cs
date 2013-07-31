using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace monorun
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        // Simple button Click event handler to take us to the second page
        private void Play_Game(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
			NavigationService.RemoveBackEntry();
        }
        // Simple button Click event handler to take us to the second page
        private void View_Highscore(object sender, RoutedEventArgs e)
        {
			NavigationService.Navigate(new Uri("/HighscorePage.xaml", UriKind.Relative));
			NavigationService.RemoveBackEntry();
        }
    }
}