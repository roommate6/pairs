using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace pairs.Classes
{
    [Serializable]
    internal class User : INotifyPropertyChanged
    {
        // Public static members:
        public static readonly string DEFAULT_USERNAME = "";
        public static readonly string DEFAULT_PASSWORD = "";

        // Constructors:
        public User(string username, string password, string avatarImageName, int playedGames, int wonGames)
        {
            InitializeMembers(username, password, avatarImageName, playedGames, wonGames);
        }
        public User(string username, string password)
        {
            InitializeMembers(username, password, Avatar.NULL_AVATAR_IMAGE_NAME, 0, 0);
        }
        public User()
        {
            InitializeMembers(DEFAULT_USERNAME, DEFAULT_PASSWORD, Avatar.NULL_AVATAR_IMAGE_NAME, 0, 0);
        }

        // Properties:
        public string Username
        {
            get
            {
                return m_username;
            }
            set
            {
                m_username = value;
                NotifyPropertyChanged("Username");
                NotifyPropertyChanged("Initialized");
            }
        }
        public string Password
        {
            get
            {
                return m_password;
            }
            set
            {
                m_password = value;
                NotifyPropertyChanged("Password");
                NotifyPropertyChanged("Initialized");
            }
        }
        public Avatar Avatar
        {
            get
            {
                return m_avatar;
            }
            set
            {
                m_avatar = value;
                NotifyPropertyChanged("Avatar");
                NotifyPropertyChanged("Initialized");
            }
        }
        public int PlayedGames
        {
            get
            {
                return m_playedGames;
            }
            set
            {
                m_playedGames = value;
                NotifyPropertyChanged(nameof(PlayedGames));
                NotifyPropertyChanged(nameof(LostGames));
            }
        }
        public int WonGames
        {
            get
            {
                return m_wonGames;
            }
            set
            {
                m_wonGames = value;
                NotifyPropertyChanged(nameof(WonGames));
                NotifyPropertyChanged(nameof(LostGames));
            }
        }
        public ObservableCollection<Game> Games
        {
            get
            {
                return m_savedGames;
            }
            set
            {
                m_savedGames = value;
            }
        }
        [JsonIgnore]
        public int LostGames
        {
            get
            {
                return PlayedGames - WonGames;
            }
        }
        [JsonIgnore]
        public bool Initialized
        {
            get
            {
                return Username != DEFAULT_USERNAME && Password != DEFAULT_PASSWORD && Avatar.Initialized;
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
        private void RemoveNullArguments(ref string username, ref string password, ref string avatarImageName)
        {
            if (username == null)
            {
                username = DEFAULT_USERNAME;
            }
            if (password == null)
            {
                password = DEFAULT_PASSWORD;
            }
            if (avatarImageName == null)
            {
                avatarImageName = Avatar.NULL_AVATAR_IMAGE_NAME;
            }
        }
        public void ResetStatistics()
        {
            PlayedGames = 0;
            WonGames = 0;
        }

        // Initializers:
        private void InitializeMembers(string username, string password, string avatarImageName, int playedGames, int wonGames)
        {
            RemoveNullArguments(ref username, ref password, ref avatarImageName);
            Username = username;
            Password = password;
            Avatar = new Avatar(avatarImageName);
            Games = new ObservableCollection<Game>();
            PlayedGames = playedGames;
            WonGames = wonGames;
        }

        // Private members:
        private string m_username;
        private string m_password;
        private Avatar m_avatar;
        private ObservableCollection<Game> m_savedGames;
        private int m_playedGames;
        private int m_wonGames;
    }
}
