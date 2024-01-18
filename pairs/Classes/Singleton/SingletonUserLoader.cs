using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pairs.Classes
{
    sealed class SingletonUserLoader
    {
        // Class members:
        public static readonly string PATH_TO_PERSISTENCE = "Persistence";
        public static readonly string FULL_PATH_TO_PERSISTENCE = FilePath.PATH_RESOURCES + PATH_TO_PERSISTENCE;
        public static readonly string PERSISTENCE_USERS_FILE_NAME = "users.json";
        private static SingletonUserLoader m_instance = null;
        private List<User> m_users;

        // Constructors:
        private SingletonUserLoader()
        {
            InitializeMembers();
        }

        // Properties:
        public static SingletonUserLoader Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new SingletonUserLoader();
                }
                return m_instance;
            }
        }
        public List<User> Users
        {
            get
            {
                return m_users;
            }
            private set
            {
                m_users = value;
            }
        }

        // Initializers:
        private void InitializeMembers()
        {
            InitializeUsers();
        }
        private void InitializeUsers()
        {
            FilePath usersPath = new FilePath(PATH_TO_PERSISTENCE, PERSISTENCE_USERS_FILE_NAME);
            Users = UtilitySerialization.DeserializeJson(usersPath.FullPath, typeof(List<User>)) as List<User>;
            if (Users == null)
            {
                Users = new List<User>();
            }
        }
    }
}
