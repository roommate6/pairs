using pairs.Classes;
using pairs.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace pairs.Windows
{
    public partial class WindowGame : Window
    {
        // Constructors:
        public WindowGame(Window parent)
        {
            InitializeComponent();
            InitializeMembers(parent);
        }

        // Properties:
        private ViewModelGame ViewModel
        {
            get
            {
                return DataContext as ViewModelGame;
            }
            set
            {
                DataContext = value;
            }
        }
        private bool BackOrSaveGamePressedOrGameOver
        {
            get
            {
                return m_backOrSaveGamePressedOrGameOver;
            }
            set
            {
                m_backOrSaveGamePressedOrGameOver = value;
            }
        }

        // Methods:
        private void GameStateChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(Game.GameState))
            {
                return;
            }
            Dispatcher.Invoke(ResolveGameOver);
        }
        private void ResolveGameOver()
        {
            ++(Owner.DataContext as ViewModelSignIn).SelectedUser.PlayedGames;
            if (ViewModel.Game.GameState == Game.State.Lost)
            {
                MessageBox.Show(this, "Time is up! Game over!");
            }
            else
            {
                ++(Owner.DataContext as ViewModelSignIn).SelectedUser.WonGames;
                MessageBox.Show(this, "Congrats! You won the game!");
            }
            (Owner.DataContext as ViewModelSignIn).SelectedUser.Games.Remove(ViewModel.Game);
            BackOrSaveGamePressedOrGameOver = true;
            Close();
        }

        // Events:
        private void TileButton_Click(object sender, RoutedEventArgs e)
        {
            TwoDimensionalCoordinates buttonCoordonates = TwoDimensionalCoordinates.Parse((sender as Button).Uid);
            ViewModel.Game.ClickTile(buttonCoordonates);
        }
        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Game.PauseGame();
            BackOrSaveGamePressedOrGameOver = true;
            ViewModel.Game.PropertyChanged -= GameStateChanged;
            (Owner.DataContext as ViewModelSignIn).SelectedUser.Games.Add(ViewModel.Game);
            Close();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            BackOrSaveGamePressedOrGameOver = true;
            Close();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ViewModel.Game.PauseGame();
            if (BackOrSaveGamePressedOrGameOver)
            {
                Owner.Show();
                return;
            }
            Owner.Close();
        }

        // Initializers:
        private void InitializeMembers(Window parent)
        {
            BackOrSaveGamePressedOrGameOver = false;
            Owner = parent;
            Resources["SelectedUserUsername"] = (Owner.DataContext as ViewModelSignIn).SelectedUser.Username;
            Resources["SelectedUserAvatarSource"] = new BitmapImage(new Uri((Owner.DataContext as ViewModelSignIn).SelectedUser.Avatar.Source));
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            DataContext = new ViewModelGame(((Owner as WindowIntermediateMainGame).DataContext as ViewModelSignIn).SelectedGame);
            ButtonsUniformGrid.Rows = ViewModel.Game.Dimensions.Y;
            ButtonsUniformGrid.Columns = ViewModel.Game.Dimensions.X;
            int maxBetweenRowsAndColumns = ButtonsUniformGrid.Rows > ButtonsUniformGrid.Columns ? ButtonsUniformGrid.Rows : ButtonsUniformGrid.Columns;
            int tileDimension = 640 / maxBetweenRowsAndColumns;
            ButtonsUniformGrid.Height = ButtonsUniformGrid.Rows * tileDimension;
            ButtonsUniformGrid.Width = ButtonsUniformGrid.Columns * tileDimension;
            for (int i = 0; i < ButtonsUniformGrid.Rows; ++i)
            {
                for (int j = 0; j < ButtonsUniformGrid.Columns; ++j)
                {
                    Tile currentTile = ViewModel.Game.Matrix[i][j];
                    Button tileButton = new Button();
                    Image tileImage = new Image();

                    Binding bindSourceWithImage = new Binding(nameof(Tile.Source));
                    bindSourceWithImage.Source = currentTile;
                    tileImage.SetBinding(Image.SourceProperty, bindSourceWithImage);

                    Binding bindTileVisibilityWithButtonVisibility = new Binding(nameof(Tile.Visibility));
                    bindTileVisibilityWithButtonVisibility.Source = currentTile;
                    tileButton.SetBinding(VisibilityProperty, bindTileVisibilityWithButtonVisibility);

                    Binding bindTileUpFacedWithButtonIsEnabled = new Binding(nameof(Tile.CanBePressed));
                    bindTileUpFacedWithButtonIsEnabled.Source = currentTile;
                    tileButton.SetBinding(IsEnabledProperty, bindTileUpFacedWithButtonIsEnabled);

                    tileButton.Uid = j.ToString() + "_" + i.ToString();
                    tileButton.Click += TileButton_Click;
                    tileButton.Content = tileImage;
                    ButtonsUniformGrid.Children.Add(tileButton);
                }
            }
            ViewModel.Game.PropertyChanged += GameStateChanged;
            ViewModel.Game.StartGame();
        }

        // Private members:
        private bool m_backOrSaveGamePressedOrGameOver;
    }
}
