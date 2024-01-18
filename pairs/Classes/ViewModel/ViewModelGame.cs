using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pairs.Classes
{
    internal class ViewModelGame
    {
        // Constructor:
        public ViewModelGame(Game game)
        {
            InitializeMembers(game);
        }

        // Properties:
        public Game Game
        {
            get
            {
                return m_game;
            }
            set
            {
                m_game = value;
            }
        }

        // Initializers:
        private void InitializeMembers(Game game)
        {
            Game = game;
        }

        // Private members:
        private Game m_game;
    }
}
