using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace monorun
{
    public partial class HighscorePage : PhoneApplicationPage
    {
        public HighscorePage()
        {
            InitializeComponent();
/*
			HighscoreList.Items.Add("Phoebe Capricornus");
			HighscoreList.Items.Add("Phoebe Fornax");
			HighscoreList.Items.Add("Iapetos Coma Beren");
			HighscoreList.Items.Add("Rhea Pictor");
			HighscoreList.Items.Add("Hyperion Taurus");
			HighscoreList.Items.Add("Kreios Andromeda");
			HighscoreList.Items.Add("Phoebe Fornax");
			HighscoreList.Items.Add("Iapetos Coma Beren");
			HighscoreList.Items.Add("Rhea Pictor");
			HighscoreList.Items.Add("Hyperion Taurus");
 */

        }

		private void View_Main(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
			NavigationService.RemoveBackEntry();
		}
    }
}