using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace pairs.Classes
{
    [Serializable]
    internal class Tile : ImageSource
    {
        // Public static members:
        public static readonly string PATH_TO_TILES = @"Images\Tiles";
        public static readonly string FULL_PATH_TO_TILES = FilePath.PATH_RESOURCES + PATH_TO_TILES;
        public static readonly bool DEFAULT_UP_FACED_VALUE = false;
        public static readonly Visibility DEFAULT_VISIBILITY_VALUE = Visibility.Visible;
        public static readonly string DOWN_FACED_IMAGE_NAME = "down_faced.png";
        public static readonly string NULL_TILE_IMAGE_NAME = "null_tile.png";
        public static readonly TwoDimensionalCoordinates NULL_POSITION_VALUE = new TwoDimensionalCoordinates(-1, -1);
        public static readonly FilePath FILE_PATH_TO_DOWN_FACED_IMAGE = new FilePath(PATH_TO_TILES, DOWN_FACED_IMAGE_NAME);

        // Constructors:
        public Tile(string imageName, bool upFaced, Visibility visibility) : base(PATH_TO_TILES, imageName, NULL_TILE_IMAGE_NAME)
        {
            InitializeMembers(upFaced, visibility);
        }
        public Tile(string imageName, bool upFaced) : base(PATH_TO_TILES, imageName, NULL_TILE_IMAGE_NAME)
        {
            InitializeMembers(upFaced, DEFAULT_VISIBILITY_VALUE);
        }
        public Tile(string imageName) : base(PATH_TO_TILES, imageName, NULL_TILE_IMAGE_NAME)
        {
            InitializeMembers(DEFAULT_UP_FACED_VALUE, DEFAULT_VISIBILITY_VALUE);
        }
        public Tile() : base(PATH_TO_TILES, NULL_TILE_IMAGE_NAME)
        {
            InitializeMembers();
        }

        // Properties:
        [JsonIgnore]
        public override string Source
        {
            get
            {
                if (!UpFaced)
                {
                    return FILE_PATH_TO_DOWN_FACED_IMAGE.FullPath;
                }
                return ImageFilePath.FullPath;
            }
        }
        public bool UpFaced
        {
            get
            {
                return m_upFaced;
            }
            set
            {
                m_upFaced = value;
                NotifyPropertyChanged(nameof(UpFaced));
                NotifyPropertyChanged(nameof(Source));
                NotifyPropertyChanged(nameof(CanBePressed));
            }
        }
        [JsonIgnore]
        public bool CanBePressed
        {
            get
            {
                return !UpFaced;
            }
        }
        [JsonIgnore]
        public override bool Initialized
        {
            get
            {
                return ImageName != NULL_TILE_IMAGE_NAME;
            }
        }
        public Visibility Visibility
        {
            get
            {
                return m_visibility;
            }
            set
            {
                m_visibility = value;
                NotifyPropertyChanged(nameof(Visibility));
            }
        }

        // Initializers:
        protected void InitializeMembers(bool upFaced, Visibility visibility)
        {
            Visibility = visibility;
            UpFaced = upFaced;
        }
        private void InitializeMembers()
        {
            Visibility = DEFAULT_VISIBILITY_VALUE;
            UpFaced = DEFAULT_UP_FACED_VALUE;
        }

        // Private members:
        private Visibility m_visibility;
        private bool m_upFaced;
    }
}
