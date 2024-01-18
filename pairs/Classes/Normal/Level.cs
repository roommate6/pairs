using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace pairs.Classes
{
    [Serializable]
    internal class Level : INotifyPropertyChanged
    {
        // Inner classes:
        [Serializable]
        public enum LevelState
        {
            InProgress,
            Won,
            Lost
        }

        // Public members:
        public event PropertyChangedEventHandler PropertyChanged;

        // Constructors:
        public Level(TwoDimensionalCoordinates dimensions)
        {
            InitializeMembers(dimensions);
        }
        public Level()
        {
            InitializeMembers();
        }

        // Properties:
        [JsonIgnore]
        public int SecondsRemained
        {
            get
            {
                return m_secondsRemained;
            }
            set
            {
                m_secondsRemained = value;
                NotifyPropertyChanged(nameof(SecondsRemained));
            }
        }
        public LevelState State
        {
            get
            {
                return m_state;
            }
            set
            {
                m_state = value;
                NotifyPropertyChanged(nameof(State));
            }
        }
        private TwoDimensionalCoordinates FirstTurnedTileCoordinates
        {
            get
            {
                return m_firstTurnedTileCoordinates;
            }
            set
            {
                m_firstTurnedTileCoordinates = value;
            }
        }
        private TwoDimensionalCoordinates LastTurnedTileCoordinates
        {
            get
            {
                return m_lastTurnedTileCoordinates;
            }
            set
            {
                m_lastTurnedTileCoordinates = value;
            }
        }
        public ObservableCollection<ObservableCollection<Tile>> Matrix
        {
            get
            {
                return m_matrixTile;
            }
            set
            {
                m_matrixTile = value;
            }
        }
        public TwoDimensionalCoordinates Dimensions
        {
            get
            {
                return m_dimensions;
            }
            set
            {
                m_dimensions = value;
            }
        }
        public int DiscoveredPairs
        {
            get
            {
                return m_discoveredPairs;
            }
            set
            {
                m_discoveredPairs = value;
            }
        }
        public int NumberOfPairs
        {
            get
            {
                return m_numberOfPairs;
            }
            set
            {
                m_numberOfPairs = value;
            }
        }
        private Timer Timer
        {
            get
            {
                return m_timer;
            }
            set
            {
                m_timer = value;
            }
        }
        private Timer Counter
        {
            get
            {
                return m_secondsCounter;
            }
            set
            {
                m_secondsCounter = value;
            }
        }

        // Methods:
        public void ClickTile(TwoDimensionalCoordinates coordinates)
        {
            if (State != LevelState.InProgress)
            {
                return;
            }
            if (FirstTurnedTileCoordinates == null)
            {
                FirstTurnedTileCoordinates = coordinates;
                Matrix[coordinates.X][coordinates.Y].UpFaced = true;
                return;
            }
            if (LastTurnedTileCoordinates == null)
            {
                LastTurnedTileCoordinates = coordinates;
                Matrix[coordinates.X][coordinates.Y].UpFaced = true;
                Timer = new Timer();
                Timer.Interval = 1000;
                Timer.Elapsed += ResolveTiles;
                Timer.Start();
            }
        }
        private void ResolveTiles(object sender, ElapsedEventArgs args)
        {
            Timer.Stop();
            Timer = null;
            if (Matrix[FirstTurnedTileCoordinates.X][FirstTurnedTileCoordinates.Y].ImageName ==
                Matrix[LastTurnedTileCoordinates.X][LastTurnedTileCoordinates.Y].ImageName)
            {
                Matrix[FirstTurnedTileCoordinates.X][FirstTurnedTileCoordinates.Y].Visibility = Visibility.Hidden;
                Matrix[LastTurnedTileCoordinates.X][LastTurnedTileCoordinates.Y].Visibility = Visibility.Hidden;
                ++DiscoveredPairs;
                if(DiscoveredPairs == NumberOfPairs)
                {
                    Counter.Stop();
                    State = LevelState.Won;
                }
            }
            else
            {
                Matrix[FirstTurnedTileCoordinates.X][FirstTurnedTileCoordinates.Y].UpFaced = false;
                Matrix[LastTurnedTileCoordinates.X][LastTurnedTileCoordinates.Y].UpFaced = false;
            }
            FirstTurnedTileCoordinates = null;
            LastTurnedTileCoordinates = null;
        }
        public void StartLevel()
        {
            Counter.Start();
        }
        public void PauseLevel()
        {
            Counter.Stop();
        }
        private void Tick(object sender, ElapsedEventArgs args)
        {
            if(SecondsRemained == 0 && State == LevelState.InProgress)
            {
                State = LevelState.Lost;
                Counter.Stop();
                return;
            }
            --SecondsRemained;
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void Reset()
        {

        }

        // Initializers:
        private void InitializeMembers(TwoDimensionalCoordinates dimensions)
        {
            Dimensions = dimensions;
            NumberOfPairs = Dimensions.X * Dimensions.Y / 2;
            DiscoveredPairs = 0;
            InitializeMatrix();
            FirstTurnedTileCoordinates = null;
            LastTurnedTileCoordinates = null;
            State = LevelState.InProgress;
            Counter = new Timer(1000);
            Counter.Elapsed += Tick;
            SecondsRemained = NumberOfPairs * 10;
        }
        private void InitializeMatrix()
        {
            int matrixElements = Dimensions.X * Dimensions.Y;
            if (matrixElements % 2 == 0)
            {
                InitializeMatrixWithEvenNumberOfElements();
                return;
            }
            InitializeMatrixWithOddNumberOfElements();
        }
        private void InitializeMatrixWithEvenNumberOfElements()
        {
            Matrix = new ObservableCollection<ObservableCollection<Tile>>();
            List<string> tileNames = SingletonNameLoader.Instance.Tiles.GetRange(0, NumberOfPairs);
            Dictionary<string, int> tileNameAppearances = new Dictionary<string, int>();
            Random random = new Random();
            foreach (string tileName in tileNames)
            {
                tileNameAppearances[tileName] = 0;
            }
            for (int i = 0; i < Dimensions.X; ++i)
            {
                ObservableCollection<Tile> row = new ObservableCollection<Tile>();
                for (int j = 0; j < Dimensions.Y; ++j)
                {
                    string tileName;
                    do
                    {
                        tileName = tileNames[random.Next(tileNames.Count)];
                    } while (tileNameAppearances[tileName] >= 2);
                    ++tileNameAppearances[tileName];
                    row.Add(new Tile(tileName, new TwoDimensionalCoordinates(i, j)));
                }
                Matrix.Add(row);
            }
        }
        private void InitializeMatrixWithOddNumberOfElements()
        {
            Matrix = new ObservableCollection<ObservableCollection<Tile>>();
            List<string> tileNames = SingletonNameLoader.Instance.Tiles.GetRange(0, NumberOfPairs);
            Dictionary<string, int> tileNameAppearances = new Dictionary<string, int>();
            Random random = new Random();
            foreach (string tileName in tileNames)
            {
                tileNameAppearances[tileName] = 0;
            }

            for (int i = 0; i < Dimensions.X - 1; ++i)
            {
                ObservableCollection<Tile> row = new ObservableCollection<Tile>();
                for (int j = 0; j < Dimensions.Y; ++j)
                {
                    string tileName;
                    do
                    {
                        tileName = tileNames[random.Next(tileNames.Count)];
                    } while (tileNameAppearances[tileName] >= 2);
                    ++tileNameAppearances[tileName];
                    row.Add(new Tile(tileName, new TwoDimensionalCoordinates(i, j)));
                }
                Matrix.Add(row);
            }
            ObservableCollection<Tile> finalRow = new ObservableCollection<Tile>();
            for (int j = 0; j < Dimensions.Y - 1; ++j)
            {
                string tileName;
                do
                {
                    tileName = tileNames[random.Next(tileNames.Count)];
                } while (tileNameAppearances[tileName] >= 2);
                ++tileNameAppearances[tileName];
                finalRow.Add(new Tile(tileName, new TwoDimensionalCoordinates(Dimensions.X - 1, j)));
            }
            finalRow.Add(new Tile(Tile.NULL_TILE_IMAGE_NAME, true, new TwoDimensionalCoordinates(Dimensions.X - 1, Dimensions.Y - 1)));
            Matrix.Add(finalRow);
        }
        private void InitializeMembers()
        {
            Dimensions = null;
            NumberOfPairs = 0;
            DiscoveredPairs = 0;
            Matrix = null;
            FirstTurnedTileCoordinates = null;
            LastTurnedTileCoordinates = null;
            State = LevelState.InProgress;
            Counter = new Timer(1000);
            Counter.Elapsed += Tick;
            SecondsRemained = 0;
        }

        // Private members:
        private ObservableCollection<ObservableCollection<Tile>> m_matrixTile;
        private TwoDimensionalCoordinates m_dimensions;
        private int m_discoveredPairs;
        private int m_numberOfPairs;
        private TwoDimensionalCoordinates m_firstTurnedTileCoordinates;
        private TwoDimensionalCoordinates m_lastTurnedTileCoordinates;
        private Timer m_timer;
        private Timer m_secondsCounter;
        private LevelState m_state;
        private int m_secondsRemained;
    }
}
