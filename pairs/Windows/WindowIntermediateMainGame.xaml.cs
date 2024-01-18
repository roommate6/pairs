using pairs.Classes;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for WindowIntermediateMainGame.xaml
    /// </summary>
    public partial class WindowIntermediateMainGame : Window
    {
        // Constructors:
        public WindowIntermediateMainGame(Window parent)
        {
            InitializeComponent();
            InitializeMembers(parent);
        }

        // Properties:
        private ViewModelSignIn ViewModel
        {
            get
            {
                return DataContext as ViewModelSignIn;
            }
            set
            {
                DataContext = value;
            }
        }
        private bool BackPressed
        {
            get
            {
                return m_backPressed;
            }
            set
            {
                m_backPressed = value;
            }
        }

        // Events:
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            BackPressed = true;
            Close();
        }
        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            WindowGame game = new WindowGame(this);
            game.Show();
            Hide();
        }
        private void ListBoxGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedGame = (sender as ListBox).SelectedItem as Game;
        }
        private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            int rows;
            int columns;
            try
            {
                rows = int.Parse(textBoxNewGameDimensionsY.Text);
                columns = int.Parse(textBoxNewGameDimensionsX.Text);
                if (rows < 1 || columns < 1) {
                    throw new ArgumentException("You can't use dimensions that are less then 1.");
                }
                if (rows * columns / 2 > SingletonNameLoader.Instance.Tiles.Count)
                {
                    throw new ArgumentException("The dimensions are to big. Try something smaller");
                }
            }
            catch (FormatException fe) {
                MessageBox.Show(fe.Message);
                return;
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show(ae.Message);
                return;
            }
            ViewModel.SelectedGame = new Game(textBoxNewGameName.Text, new TwoDimensionalCoordinates(
                columns,
                rows
                ));
            WindowGame game = new WindowGame(this);
            game.Show();
            Hide();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (BackPressed)
            {
                Owner.Show();
                return;
            }
            Owner.Close();
        }

        // Initializers:
        private void InitializeMembers(Window parent)
        {
            Owner = parent;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ViewModel = Owner.DataContext as ViewModelSignIn;
        }

        // Private members:
        private bool m_backPressed;
    }
}
