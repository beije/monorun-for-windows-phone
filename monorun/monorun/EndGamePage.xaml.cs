﻿using System;
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

			System.Windows.Documents.Run userScore = (System.Windows.Documents.Run)LayoutRoot.FindName("userScore");
			userScore.Text = (String)api.LatestHighscore.score.ToString();

			System.Windows.Documents.Run userHalfLife = (System.Windows.Documents.Run) LayoutRoot.FindName("userHalfLife");
			userHalfLife.Text = (String) Math.Floor(api.LatestHighscore.score/1000).ToString();

			if( !api.isOnline ) 
			{
				Button SubmitButton = (Button) LayoutRoot.FindName("submitScoreButton");
				SubmitButton.Visibility = System.Windows.Visibility.Collapsed;

				TextBox usernameInput = (TextBox)LayoutRoot.FindName("Username");
				usernameInput.Text = "No internet connection.";
				usernameInput.IsEnabled = false;
			}

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
				Button SubmitButton = (Button) LayoutRoot.FindName("submitScoreButton");
				SubmitButton.Focus();
			}
		}

		private void updateScore( Object sender, RoutedEventArgs e )
		{
			TextBox usernameInput = (TextBox)LayoutRoot.FindName("Username");
			String newUsername = api.LatestHighscore.username;

			if (usernameInput.Text != "Enter your username!") {
				newUsername = usernameInput.Text;
			}
			
			api.LatestHighscore.username = newUsername;

			Action cb = () => {
				NavigationService.Navigate(new Uri("/HighscorePage.xaml", UriKind.Relative));
				NavigationService.RemoveBackEntry();
			};

			api.updateScore(newUsername, cb);
		}

		private void usernameFocused(object sender, RoutedEventArgs e)
		{
			TextBox t = (TextBox) sender;
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