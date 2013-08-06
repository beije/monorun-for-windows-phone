using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System.Windows.Media;

namespace monorun
{
    public partial class HighscorePage : PhoneApplicationPage
    {
		ApiHandler api;
		List<Highscore> topHighscores;

        public HighscorePage()
        {
            InitializeComponent();
			
			api = (Application.Current as App).api;
			//api.doRequest("get", loadHighscore);

			Action<List<Highscore>> cb = (list) => {
				topHighscores = list;
				renderHighscoreTable();
			};

			api.getHighscore(cb);

        }

		private void renderHighscoreTable()
		{
			highscoreList = (Grid)LayoutRoot.FindName("highscoreList");
			int loopEnd = 10;
			if( api.LatestHighscore != null ) {
				loopEnd = (api.LatestHighscore.position > 10 ? topHighscores.Count()-2 : topHighscores.Count());
			}
			
			for( int i = 0; i < loopEnd; i++)
			{
				Boolean highlight = (api.LatestHighscore.id ==  topHighscores[i].id ? true : false );
				addRow(highscoreList, topHighscores[i].position.ToString(), topHighscores[i].username, topHighscores[i].score.ToString(), highlight);
			}
			if( loopEnd < topHighscores.Count() )
			{
				addRow(highscoreList, "...", "...", "...", false);
				addRow(highscoreList, api.LatestHighscore.position.ToString(), api.LatestHighscore.username, api.LatestHighscore.score.ToString(), true);
			}

		}

		private void addRow(Grid targetGrid, string position, string username, string score, Boolean highlight) 
		{
			RowDefinition newRow = new RowDefinition();
			targetGrid.RowDefinitions.Add(newRow);
			int rowNumber = targetGrid.RowDefinitions.Count() - 1;

			SolidColorBrush dark = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
			SolidColorBrush light = new SolidColorBrush(Color.FromArgb(255, 86, 197, 219));

			TextBlock positionCell = new TextBlock();
			positionCell.Text = (String)position;
			positionCell.VerticalAlignment = System.Windows.VerticalAlignment.Center;
			positionCell.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
			positionCell.Foreground = ( highlight == true ? light : dark );
			Grid.SetRow(positionCell, rowNumber);
			Grid.SetColumn(positionCell, 0);

			TextBlock usernameCell = new TextBlock();
			usernameCell.Text = (String)username;
			usernameCell.VerticalAlignment = System.Windows.VerticalAlignment.Center;
			usernameCell.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
			usernameCell.Foreground = (highlight == true ? light : dark);
			Grid.SetRow(usernameCell, rowNumber);
			Grid.SetColumn(usernameCell, 1);

			TextBlock scoreCell = new TextBlock();
			scoreCell.Text = (String)score;
			scoreCell.VerticalAlignment = System.Windows.VerticalAlignment.Center;
			scoreCell.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
			scoreCell.Foreground = (highlight == true ? light : dark);
			Grid.SetRow(scoreCell, rowNumber);
			Grid.SetColumn(scoreCell, 2);

			targetGrid.Children.Add(positionCell);
			targetGrid.Children.Add(usernameCell);
			targetGrid.Children.Add(scoreCell);
		}
		private void View_Main(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
			NavigationService.RemoveBackEntry();
		}
    }
}