using pairs.Classes;
using System.Windows;
using System.Collections.Generic;
using System;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using pairs.Windows;

namespace pairs
{
    /// <summary>
    /// Interaction logic for WindowMain.xaml
    /// </summary>
    public partial class WindowMain : Window
    {
        // Constructors:
        public WindowMain()
        {
            InitializeComponent();
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

        // Events:
        private void ButtonAvatar_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NextAvatarForDummyUser();
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!SignInFormHasAllFieldsFilled())
            {
                MessageBox.Show("Complete all the form fields before adding a new user.");
                return;
            }
            ViewModel.AddDummyUser();
        }
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveSelectedUser();
        }
        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetSelections(); // this sets selectedgame to null so that you can't acces someone elses slectedgame via another user
            WindowIntermediateMainGame intermediate = new WindowIntermediateMainGame(this);
            Hide();
            intermediate.Show();
        }
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ListBoxUsers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ViewModel.SelectedUser = (sender as ListBox).SelectedItem as User;
            if (ViewModel.SelectedUser == null)
            {
                ViewModel.SelectedUser = new User();
                ViewModel.ExistsSelection = false;
                return;
            }
            ViewModel.ExistsSelection = true;
        }
        private void ButtonRandomizeAvatar_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DummyUser.Avatar.Randomize();
        }
        private void UserSelectorButtonDown_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ViewModel.UserSelectorDownDecoration.ImageName = "hovered_down_arrow.png";
        }
        private void UserSelectorButtonUp_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ViewModel.UserSelectorUpDecoration.ImageName = "hovered_up_arrow.png";
        }
        private void UserSelectorButtonDown_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ViewModel.UserSelectorDownDecoration.ImageName = "down_arrow.png";
        }
        private void UserSelectorButtonUp_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ViewModel.UserSelectorUpDecoration.ImageName = "up_arrow.png";
        }
        private void UserSelectorButtonDown_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.UserSelectorDownDecoration.ImageName = "pressed_down_arrow.png";
        }
        private void UserSelectorButtonDown_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.UserSelectorDownDecoration.ImageName = "hovered_down_arrow.png";
        }
        private void UserSelectorButtonUp_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.UserSelectorUpDecoration.ImageName = "pressed_up_arrow.png";
        }
        private void UserSelectorButtonUp_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.UserSelectorUpDecoration.ImageName = "hovered_up_arrow.png";
        }
        private void UserSelectorButtonUp_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ScrollUpInUsers();
            ListBoxUsers.SelectedItem = ViewModel.SelectedUser;
            ListBoxUsers.Focus();
        }
        private void UserSelectorButtonDown_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ScrollDownInUsers();
            ListBoxUsers.SelectedItem = ViewModel.SelectedUser;
            ListBoxUsers.Focus();
        }
        private void ButtonSettingsForSelectedUser_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetSelections(); // this sets selectedgame to null so that you can't acces someone elses slectedgame via another user
            ViewModel.CopySelectedUserInDummyUserForSettings();
            WindowUserSettings window = new WindowUserSettings(this);
            Hide();
            window.Show();
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "Student: Ștefan-Sebastian Neicu" + Environment.NewLine + "Grupa: 10LF212");
        }
        private void About_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ViewModel.AboutDecoration.ImageName = "hovered_about.png";
        }
        private void About_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ViewModel.AboutDecoration.ImageName = "about.png";
        }
        private void About_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.AboutDecoration.ImageName = "pressed_about.png";
        }
        private void About_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.AboutDecoration.ImageName = "hovered_about.png";
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.SerializeUsers();
        }

        // Methods:
        private bool SignInFormHasAllFieldsFilled()
        {
            if (ViewModel.DummyUser.Initialized)
            {
                return true;
            }
            return false;
        }
    }
}
