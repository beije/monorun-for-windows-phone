using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Input;

namespace monorun
{
    public partial class EndGamePage : PhoneApplicationPage
    {
		private ApiHandler api;

		public EndGamePage()
        {
            InitializeComponent();
			api = (Application.Current as App).api;
        }

		private void View_Main(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
			NavigationService.RemoveBackEntry();
		}
		private void usernameKeyEventListener(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				Object SubmitButton = LayoutRoot.FindName("submitScoreButton");
				if( SubmitButton is Button )
				{
					Button btn = (Button) SubmitButton;
					btn.Focus();
				}
				
			}
		}
		private void updateScore( Object sender, RoutedEventArgs e )
		{
			Object usernameObject = LayoutRoot.FindName("Username");
			if (usernameObject is TextBox)
			{
				TextBox usernameInput = (TextBox) usernameObject;
				if (usernameInput.Text != "Enter your username!"){
					api.updateScore(usernameInput.Text);
				}
			}

			NavigationService.Navigate(new Uri("/HighscorePage.xaml", UriKind.Relative));
			NavigationService.RemoveBackEntry();
		}
		private void usernameFocused(object sender, RoutedEventArgs e)
		{
			TextBox t = (TextBox)sender;
			t.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 86, 197, 219));
			if( t.Text.Trim() == "Enter your username!")
			{
				t.Text = "";
				t.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
			}
		}
		private void usernameBlured(object sender, RoutedEventArgs e)
		{
			TextBox t = (TextBox) sender;
			t.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 157, 157, 157));
			if (t.Text.Trim() == "")
			{
				t.Text = "Enter your username!";
				t.Foreground = new SolidColorBrush(Color.FromArgb(255, 86, 197, 219));
			}
		}
    }
}