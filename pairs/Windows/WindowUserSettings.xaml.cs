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
    /// Interaction logic for WindowUserSettings.xaml
    /// </summary>
    public partial class WindowUserSettings : Window
    {
        // Constructors:
        public WindowUserSettings(Window parent)
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
        private void ButtonModify_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CopyDummyUserForSettingsInSelectedUser();
            ViewModel.UpdateModificationsInSettings();
        }
        private void ButtonAvatar_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NextAvatarForDummyUserForSettings();
        }
        private void ButtonRandomizeAvatar_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DummyUserForSettings.Avatar.Randomize();
        }
        private void ListBoxGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedGame = (sender as ListBox).SelectedItem as Game;
        }
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedUser.Games.Remove(ViewModel.SelectedGame);
        }
        private void ResetStatistics_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedUser.ResetStatistics();
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
            BackPressed = false;
            Owner = parent;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ViewModel = Owner.DataContext as ViewModelSignIn;
        }

        // Private members:
        private bool m_backPressed;
    }
}
