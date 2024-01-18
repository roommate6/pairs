using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Xml.Linq;

namespace pairs.Classes
{
    [Serializable]
    internal class Game : INotifyPropertyChanged
    {
        [Serializable]
        public enum State
        {
            InProgress,
            Won,
            Lost
        }

        // Public members:
        public static readonly int DEFAULT_MAX_LEVEL_INDEX = 3;
        public static readonly int DEFAULT_MAX_MAX_LEVEL_INDEX = 10;
        public static readonly int DEFAULT_MIN_MAX_LEVEL_INDEX = 2;
        public static readonly int DEFAULT_DELAY_BETWEEN_MOVES = 1000; //ms

        // Constructors:
        public Game(string name, TwoDimensionalCoordinates dimensions, int maxLevelIndex)
        {
            InitializeMembers(name, dimensions, maxLevelIndex);
        }
        public Game(string name, TwoDimensionalCoordinates dimensions)
        {
            InitializeMembers(name, dimensions, DEFAULT_MAX_LEVEL_INDEX);
        }
        public Game()
        {
            InitializeMembers();
        }

        // Event handlers:
        public event PropertyChangedEventHandler PropertyChanged;

        // Properties:
        public TwoDimensionalCoordinates Dimensions
        {
            get
            {
                return m_dimensions;
            }
            set
            {
                NotifyPropertyChanged(nameof(Dimensions));
                m_dimensions = value;
                if (value != null)
                {
                    m_numberOfPairs = value.X * value.Y / 2;
                    return;
                }
                m_numberOfPairs = 0;
            }
        }
        public ObservableCollection<ObservableCollection<Tile>> Matrix
        {
            get
            {
                return m_currentLevelMatrixConfiguration;
            }
            set
            {
                m_currentLevelMatrixConfiguration = value;
                NotifyPropertyChanged(nameof(Matrix));
            }
        }
        public int CurrentLevelIndex
        {
            get
            {
                return m_currentLevelIndex;
            }
            set
            {
                m_currentLevelIndex = value;
                NotifyPropertyChanged(nameof(CurrentLevelIndex));
            }
        }
        public int CurrentLevelSecondsLeft
        {
            get
            {
                return m_currentLevelSecondsLeft;
            }
            set
            {
                m_currentLevelSecondsLeft = value;
                NotifyPropertyChanged(nameof(CurrentLevelSecondsLeft));
            }
        }
        public int MaxLevelIndex
        {
            get
            {
                return m_maxLevelIndex;
            }
            set
            {
                m_maxLevelIndex = value;
                NotifyPropertyChanged(nameof(MaxLevelIndex));
            }
        }
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }
        public State LevelState
        {
            get
            {
                return m_levelState;
            }
            set
            {
                m_levelState = value;
                NotifyPropertyChanged(nameof(LevelState));
            }
        }
        public State GameState
        {
            get
            {
                return m_gameState;
            }
            set
            {
                m_gameState = value;
                NotifyPropertyChanged(nameof(GameState));
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
                NotifyPropertyChanged(nameof(DiscoveredPairs));
            }
        }
        private int NumberOfPairs
        {
            get
            {
                return m_numberOfPairs;
            }
        }
        private Timer Delayer
        {
            get
            {
                return m_delayer;
            }
            set
            {
                m_delayer = value;
            }
        }
        private Timer SecondsCounter
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

        // Methods:
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void StartGame()
        {
            if (GameState == State.InProgress)
            {
                SecondsCounter.Start();
            }
        }
        public void PauseGame()
        {
            if (FirstTurnedTileCoordinates != null)
            {
                Matrix[FirstTurnedTileCoordinates.Y][FirstTurnedTileCoordinates.X].UpFaced = false;
                FirstTurnedTileCoordinates = null;
            }
            SecondsCounter.Stop();
        }
        public void ClickTile(TwoDimensionalCoordinates coordinates)
        {
            if (LevelState != State.InProgress)
            {
                return;
            }
            if (FirstTurnedTileCoordinates == null)
            {
                FirstTurnedTileCoordinates = coordinates;
                Matrix[coordinates.Y][coordinates.X].UpFaced = true;
                return;
            }
            if (LastTurnedTileCoordinates == null)
            {
                LastTurnedTileCoordinates = coordinates;
                Matrix[coordinates.Y][coordinates.X].UpFaced = true;
                Delayer.Start();
            }
        }
        private void ResolveTiles(object sender, ElapsedEventArgs args)
        {
            Delayer.Stop();
            if (Matrix[FirstTurnedTileCoordinates.Y][FirstTurnedTileCoordinates.X].ImageName ==
                Matrix[LastTurnedTileCoordinates.Y][LastTurnedTileCoordinates.X].ImageName)
            {
                Matrix[FirstTurnedTileCoordinates.Y][FirstTurnedTileCoordinates.X].Visibility = Visibility.Hidden;
                Matrix[LastTurnedTileCoordinates.Y][LastTurnedTileCoordinates.X].Visibility = Visibility.Hidden;
                ++DiscoveredPairs;
                if (DiscoveredPairs == NumberOfPairs)
                {
                    SecondsCounter.Stop();
                    LevelState = State.Won;
                }
            }
            else
            {
                Matrix[FirstTurnedTileCoordinates.Y][FirstTurnedTileCoordinates.X].UpFaced = false;
                Matrix[LastTurnedTileCoordinates.Y][LastTurnedTileCoordinates.X].UpFaced = false;
            }
            FirstTurnedTileCoordinates = null;
            LastTurnedTileCoordinates = null;
        }
        private void Tick(object sender, ElapsedEventArgs args)
        {
            if (CurrentLevelSecondsLeft <= 0 && LevelState == State.InProgress)
            {
                SecondsCounter.Stop();
                LevelState = State.Lost;
                return;
            }
            --CurrentLevelSecondsLeft;
        }
        private void NextLevel(object sender, ElapsedEventArgs args)
        {
            (sender as Timer).Stop();
            if (CurrentLevelIndex == MaxLevelIndex)
            {
                GameState = State.Won;
                return;
            }
            ++CurrentLevelIndex;
            ResetMatrix();
            if (NumberOfPairs < 4)
            {
                CurrentLevelSecondsLeft = NumberOfPairs * 4;
            }
            else
            {
                CurrentLevelSecondsLeft = NumberOfPairs * 10;
            }
            DiscoveredPairs = 0;
            LevelState = State.InProgress;
            SecondsCounter.Start();
        }
        private void ResetMatrix()
        {
            int matrixElements = Dimensions.Y * Dimensions.X;
            if (matrixElements % 2 == 0)
            {
                ResetMatrixWithEvenNumberOfElements();
                return;
            }
            ResetMatrixWithOddNumberOfElements();
        }
        private void ResetMatrixWithEvenNumberOfElements()
        {
            List<string> tileNames = SingletonNameLoader.Instance.Tiles.GetRange(0, NumberOfPairs);
            Dictionary<string, int> tileNameAppearances = new Dictionary<string, int>();
            Random random = new Random();
            foreach (string tileName in tileNames)
            {
                tileNameAppearances[tileName] = 0;
            }
            for (int i = 0; i < Dimensions.Y; ++i)
            {
                for (int j = 0; j < Dimensions.X; ++j)
                {
                    string tileName;
                    do
                    {
                        tileName = tileNames[random.Next(tileNames.Count)];
                    } while (tileNameAppearances[tileName] >= 2);
                    ++tileNameAppearances[tileName];
                    Matrix[i][j].ImageName = tileName;
                    Matrix[i][j].UpFaced = false;
                    Matrix[i][j].Visibility = Visibility.Visible;
                }
            }
        }
        private void ResetMatrixWithOddNumberOfElements()
        {
            List<string> tileNames = SingletonNameLoader.Instance.Tiles.GetRange(0, NumberOfPairs);
            Dictionary<string, int> tileNameAppearances = new Dictionary<string, int>();
            Random random = new Random();
            foreach (string tileName in tileNames)
            {
                tileNameAppearances[tileName] = 0;
            }
            for (int i = 0; i < Dimensions.Y - 1; ++i)
            {
                for (int j = 0; j < Dimensions.X; ++j)
                {
                    string tileName;
                    do
                    {
                        tileName = tileNames[random.Next(tileNames.Count)];
                    } while (tileNameAppearances[tileName] >= 2);
                    ++tileNameAppearances[tileName];
                    Matrix[i][j].ImageName = tileName;
                    Matrix[i][j].UpFaced = false;
                    Matrix[i][j].Visibility = Visibility.Visible;
                }
            }
            for (int j = 0; j < Dimensions.X - 1; ++j)
            {
                string tileName;
                do
                {
                    tileName = tileNames[random.Next(tileNames.Count)];
                } while (tileNameAppearances[tileName] >= 2);
                ++tileNameAppearances[tileName];
                Matrix[Dimensions.Y - 1][j].ImageName = tileName;
                Matrix[Dimensions.Y - 1][j].UpFaced = false;
                Matrix[Dimensions.Y - 1][j].Visibility = Visibility.Visible;
            }
        }
        private void ResolveGameState(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(LevelState))
            {
                return;
            }
            if (LevelState == State.InProgress)
            {
                return;
            }
            if (LevelState == State.Lost)
            {
                GameState = State.Lost;
                return;
            }
            if (CurrentLevelIndex == MaxLevelIndex)
            {
                GameState = State.Won;
                return;
            }
            Timer timer = new Timer(2000);
            timer.Elapsed += NextLevel;
            timer.Start();
        }

        // Initializers:
        private void InitializeMembers(string name, TwoDimensionalCoordinates dimensions, int maxLevelIndex)
        {
            PropertyChanged += ResolveGameState;
            Name = name;
            Dimensions = dimensions;
            MaxLevelIndex = maxLevelIndex;
            GameState = State.InProgress;
            LevelState = State.InProgress;
            CurrentLevelIndex = 1;
            if (NumberOfPairs < 5)
            {
                CurrentLevelSecondsLeft = NumberOfPairs * 4;
            }
            else
            {
                CurrentLevelSecondsLeft = NumberOfPairs * 10;
            }
            Delayer = new Timer(DEFAULT_DELAY_BETWEEN_MOVES);
            Delayer.Elapsed += ResolveTiles;
            SecondsCounter = new Timer(1000);
            SecondsCounter.Elapsed += Tick;
            InitializeMatrix();
        }
        private void InitializeMembers()
        {
            PropertyChanged += ResolveGameState;
            Name = null;
            Dimensions = null;
            MaxLevelIndex = DEFAULT_MAX_LEVEL_INDEX;
            GameState = State.InProgress;
            LevelState = State.InProgress;
            CurrentLevelIndex = 0;
            CurrentLevelSecondsLeft = 0;
            Delayer = new Timer(DEFAULT_DELAY_BETWEEN_MOVES);
            Delayer.Elapsed += ResolveTiles;
            SecondsCounter = new Timer(1000);
            SecondsCounter.Elapsed += Tick;
            Matrix = null;
        }
        private void InitializeMatrix()
        {
            int matrixElements = Dimensions.Y * Dimensions.X;
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
            for (int i = 0; i < Dimensions.Y; ++i)
            {
                ObservableCollection<Tile> row = new ObservableCollection<Tile>();
                for (int j = 0; j < Dimensions.X; ++j)
                {
                    string tileName;
                    do
                    {
                        tileName = tileNames[random.Next(tileNames.Count)];
                    } while (tileNameAppearances[tileName] >= 2);
                    ++tileNameAppearances[tileName];
                    row.Add(new Tile(tileName));
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

            for (int i = 0; i < Dimensions.Y - 1; ++i)
            {
                ObservableCollection<Tile> row = new ObservableCollection<Tile>();
                for (int j = 0; j < Dimensions.X; ++j)
                {
                    string tileName;
                    do
                    {
                        tileName = tileNames[random.Next(tileNames.Count)];
                    } while (tileNameAppearances[tileName] >= 2);
                    ++tileNameAppearances[tileName];
                    row.Add(new Tile(tileName));
                }
                Matrix.Add(row);
            }
            ObservableCollection<Tile> finalRow = new ObservableCollection<Tile>();
            for (int j = 0; j < Dimensions.X - 1; ++j)
            {
                string tileName;
                do
                {
                    tileName = tileNames[random.Next(tileNames.Count)];
                } while (tileNameAppearances[tileName] >= 2);
                ++tileNameAppearances[tileName];
                finalRow.Add(new Tile(tileName));
            }
            finalRow.Add(new Tile(Tile.NULL_TILE_IMAGE_NAME, true));
            Matrix.Add(finalRow);
        }

        // Private members:
        private string m_name;
        private TwoDimensionalCoordinates m_dimensions;
        private int m_maxLevelIndex;
        private State m_gameState;
        private int m_numberOfPairs;

        private ObservableCollection<ObservableCollection<Tile>> m_currentLevelMatrixConfiguration;
        private TwoDimensionalCoordinates m_firstTurnedTileCoordinates;
        private TwoDimensionalCoordinates m_lastTurnedTileCoordinates;
        private int m_currentLevelIndex;
        private int m_currentLevelSecondsLeft;
        private Timer m_delayer;
        private Timer m_secondsCounter;
        private int m_discoveredPairs;
        private State m_levelState;
    }
}
