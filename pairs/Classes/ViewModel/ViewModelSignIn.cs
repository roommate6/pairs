using pairs.Classes.Normal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace pairs.Classes
{
    internal class ViewModelSignIn : INotifyPropertyChanged
    {
        // Constructors:
        public ViewModelSignIn()
        {
            InitializeMembers();
        }

        // Properties:
        public User DummyUser
        {
            get
            {
                return m_dummyUser;
            }
            set
            {
                m_dummyUser = value;
                NotifyPropertyChanged(nameof(DummyUser));
            }
        }
        public User DummyUserForSettings
        {
            get
            {
                return m_dummyUserForSettings;
            }
            set
            {
                if (m_dummyUserForSettings != null)
                {
                    m_dummyUserForSettings.PropertyChanged -= OnModifications;
                    m_dummyUserForSettings.Avatar.PropertyChanged -= OnModifications;
                }

                m_dummyUserForSettings = value;
                NotifyPropertyChanged(nameof(DummyUserForSettings));

                if (m_dummyUserForSettings != null)
                {
                    m_dummyUserForSettings.PropertyChanged += OnModifications;
                    m_dummyUserForSettings.Avatar.PropertyChanged += OnModifications;
                }
            }
        }
        public User SelectedUser
        {
            get
            {
                return m_selectedUser;
            }
            set
            {
                m_selectedUser = value;
                NotifyPropertyChanged(nameof(SelectedUser));
            }
        }
        public ObservableCollection<User> Users
        {
            get
            {
                return m_users;
            }
            private set
            {
                m_users = value;
                NotifyPropertyChanged(nameof(Users));
            }
        }
        public bool ExistsSelection
        {
            get
            {
                return m_existsSelectionInListBox;
            }
            set
            {
                m_existsSelectionInListBox = value;
                NotifyPropertyChanged(nameof(ExistsSelection));
            }
        }
        public bool ExistsModifications
        {
            get
            {
                return m_existsModificationsInSettings;
            }
            set
            {
                m_existsModificationsInSettings = value;
                NotifyPropertyChanged(nameof(ExistsModifications));
            }
        }
        public Decoration RandomizeAvatarButtonDecoration
        {
            get
            {
                return m_randomizeAvatarButtonDecoration;
            }
            private set
            {
                m_randomizeAvatarButtonDecoration = value;
                NotifyPropertyChanged(nameof(RandomizeAvatarButtonDecoration));
            }
        }
        public Decoration UserSelectorUpDecoration
        {
            get
            {
                return m_userSelectorUpDecoration;
            }
            private set
            {
                m_userSelectorUpDecoration = value;
                NotifyPropertyChanged(nameof(UserSelectorUpDecoration));
            }
        }
        public Decoration UserSelectorDownDecoration
        {
            get
            {
                return m_userSelectorDownDecoration;
            }
            private set
            {
                m_userSelectorDownDecoration = value;
                NotifyPropertyChanged(nameof(UserSelectorDownDecoration));
            }
        }
        public Decoration PlusSignDecoration
        {
            get
            {
                return m_plusSignDecoration;
            }
            private set
            {
                m_plusSignDecoration = value;
                NotifyPropertyChanged(nameof(PlusSignDecoration));
            }
        }
        public Decoration AboutDecoration
        {
            get
            {
                return m_aboutDecoration;
            }
            private set
            {
                m_aboutDecoration = value;
                NotifyPropertyChanged(nameof(AboutDecoration));
            }
        }
        public int AvatarIndex
        {
            get
            {
                return m_avatarIndex;
            }
            private set
            {
                NotifyPropertyChanged(nameof(AvatarIndex));
                if (value < 0)
                {
                    m_avatarIndex = 0;
                    return;
                }
                m_avatarIndex = value % SingletonNameLoader.Instance.Avatars.Count;
            }
        }
        public int AvatarIndexForDummyUserForSettings
        {
            get
            {
                return m_avatarIndexForDummyForSettings;
            }
            private set
            {
                NotifyPropertyChanged(nameof(AvatarIndexForDummyUserForSettings));
                if (value < 0)
                {
                    m_avatarIndexForDummyForSettings = 0;
                    return;
                }
                m_avatarIndexForDummyForSettings = value % SingletonNameLoader.Instance.Avatars.Count;
            }
        }
        public int SelectedUserIndex
        {
            get
            {
                return m_selectedUserIndex;
            }
            private set
            {
                NotifyPropertyChanged(nameof(SelectedUserIndex));
                if (value < 0)
                {
                    m_selectedUserIndex = Users.Count - 1;
                    return;
                }
                m_selectedUserIndex = value % Users.Count;
            }
        }
        public Game SelectedGame
        {
            get
            {
                return m_selectedGame;
            }
            set
            {
                m_selectedGame = value;
                NotifyPropertyChanged(nameof(SelectedGame));
                NotifyPropertyChanged(nameof(SelectedGameExists));
            }
        }
        public bool SelectedGameExists
        {
            get
            {
                return SelectedGame != null;
            }
        }

        // Events:
        public event PropertyChangedEventHandler PropertyChanged;

        // Methods:
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void AddDummyUser()
        {
            Users.Add(DummyUser);
            DummyUser = new User();
            AvatarIndex = 0;
        }
        public void NextAvatarForDummyUser()
        {
            if (DummyUser.Avatar.Initialized)
            {
                List<string> names = SingletonNameLoader.Instance.Avatars;
                AvatarIndex = names.IndexOf(DummyUser.Avatar.ImageName);
                ++AvatarIndex;
            }
            else
            {
                AvatarIndex = 0;
            }
            DummyUser.Avatar.ImageName = SingletonNameLoader.Instance.Avatars[AvatarIndex];
        }
        public void NextAvatarForDummyUserForSettings()
        {
            if (DummyUserForSettings.Avatar.Initialized)
            {
                List<string> names = SingletonNameLoader.Instance.Avatars;
                AvatarIndexForDummyUserForSettings = names.IndexOf(DummyUserForSettings.Avatar.ImageName);
                ++AvatarIndexForDummyUserForSettings;
            }
            else
            {
                AvatarIndexForDummyUserForSettings = 0;
            }
            DummyUserForSettings.Avatar.ImageName = SingletonNameLoader.Instance.Avatars[AvatarIndexForDummyUserForSettings];
        }
        public void ScrollUpInUsers()
        {
            if (!TestConditionForUserScroll())
            {
                return;
            }
            if (SelectedUser.Initialized)
            {
                SelectedUserIndex = Users.IndexOf(SelectedUser);
                --SelectedUserIndex;
                SelectedUser = Users[SelectedUserIndex];
                return;
            }
            SelectedUser = Users[0];
        }
        public void ScrollDownInUsers()
        {
            if (!TestConditionForUserScroll())
            {
                return;
            }
            if (SelectedUser.Initialized)
            {
                SelectedUserIndex = Users.IndexOf(SelectedUser);
                ++SelectedUserIndex;
                SelectedUser = Users[SelectedUserIndex];
                return;
            }
            SelectedUser = Users[0];
        }
        public void UpdateModificationsInSettings()
        {
            ExistsModifications = DummyUserForSettings.Username != SelectedUser.Username ||
                DummyUserForSettings.Password != SelectedUser.Password ||
                DummyUserForSettings.Avatar.ImageName != SelectedUser.Avatar.ImageName;
        }
        public void RemoveSelectedUser()
        {
            Users.Remove(SelectedUser);
        }
        public void CopySelectedUserInDummyUserForSettings()
        {
            DummyUserForSettings = new User(SelectedUser.Username, SelectedUser.Password, SelectedUser.Avatar.ImageName, SelectedUser.PlayedGames, SelectedUser.WonGames);
        }
        public void CopyDummyUserForSettingsInSelectedUser()
        {
            SelectedUser.Username = DummyUserForSettings.Username;
            SelectedUser.Password = DummyUserForSettings.Password;
            SelectedUser.Avatar.ImageName = DummyUserForSettings.Avatar.ImageName;
        }
        public void SerializeUsers()
        {
            UtilitySerialization.SerializeJson(UtilitySerialization.FULL_PATH_TO_USERS, Users);
        }
        private void OnModifications(object sender, PropertyChangedEventArgs args)
        {
            UpdateModificationsInSettings();
        }
        public void ResetSelections()
        {
            SelectedGame = null;
        }

        // Tests:
        private bool TestConditionForUserScroll()
        {
            if (Users.Count == 0)
            {
                return false;
            }
            return true;
        }

        // Initializers:
        private void InitializeMembers()
        {
            Users = new ObservableCollection<User>(SingletonUserLoader.Instance.Users);
            DummyUser = new User();
            SelectedUser = new User();
            ExistsSelection = false;
            ExistsModifications = false;
            RandomizeAvatarButtonDecoration = new Decoration("casino.png");
            UserSelectorUpDecoration = new Decoration("up_arrow.png");
            UserSelectorDownDecoration = new Decoration("down_arrow.png");
            PlusSignDecoration = new Decoration("plus_sign.png");
            AboutDecoration = new Decoration("about.png");
        }

        // Private members:
        private User m_dummyUser;
        private User m_dummyUserForSettings;
        private User m_selectedUser;
        private Game m_selectedGame;
        private bool m_existsSelectionInListBox;
        private bool m_existsModificationsInSettings;
        private ObservableCollection<User> m_users;
        private int m_avatarIndex;
        private int m_avatarIndexForDummyForSettings;
        private int m_selectedUserIndex;
        private Decoration m_randomizeAvatarButtonDecoration;
        private Decoration m_userSelectorUpDecoration;
        private Decoration m_userSelectorDownDecoration;
        private Decoration m_plusSignDecoration;
        private Decoration m_aboutDecoration;
    }
}
