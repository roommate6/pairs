using pairs.Classes.Normal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pairs.Classes
{
    sealed class SingletonNameLoader
    {
        // Class members:
        private static SingletonNameLoader m_instance = null;
        private List<string> m_avatarNames;
        private List<string> m_tileNames;
        private List<string> m_decorationNames;

        // Constructors:
        private SingletonNameLoader()
        {
            InitializeMembers();
        }

        // Properties:
        public static SingletonNameLoader Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new SingletonNameLoader();
                }
                return m_instance;
            }
        }
        public List<string> Avatars
        {
            get
            {
                return m_avatarNames;
            }
            private set
            {
                m_avatarNames = value;
            }
        }
        public List<string> Tiles
        {
            get
            {
                return m_tileNames;
            }
            private set
            {
                m_tileNames = value;
            }
        }
        public List<string> Decorations
        {
            get
            {
                return m_decorationNames;
            }
            private set
            {
                m_decorationNames = value;
            }
        }

        // Methods:
        private List<string> GetAllFileNamesFromDirectoryExceptNameList(string pathToDirectory, List<string> exceptedFileNames)
        {
            List<string> names = new List<string>();
            DirectoryInfo directoryInfo = new DirectoryInfo(pathToDirectory);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                if (exceptedFileNames.Contains(fileInfo.Name))
                {
                    continue;
                }
                names.Add(fileInfo.Name);
            }
            return names;
        }

        // Initializers:
        private void InitializeMembers()
        {
            Avatars = GetAllFileNamesFromDirectoryExceptNameList(Avatar.FULL_PATH_TO_AVATARS,
                new List<string>() { Avatar.NULL_AVATAR_IMAGE_NAME });

            Tiles = GetAllFileNamesFromDirectoryExceptNameList(Tile.FULL_PATH_TO_TILES,
                new List<string>() { Tile.NULL_TILE_IMAGE_NAME, Tile.DOWN_FACED_IMAGE_NAME });

            Decorations = GetAllFileNamesFromDirectoryExceptNameList(Decoration.FULL_PATH_TO_DECORATIONS,
                new List<string>() { Decoration.NULL_DECORATION_IMAGE_NAME });
        }
    }
}
