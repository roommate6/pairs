using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pairs.Classes
{
    [Serializable]
    internal class TwoDimensionalCoordinates
    {
        // Constructors:
        public TwoDimensionalCoordinates(int x, int y)
        {
            InitializeMembers(x, y);
        }
        public TwoDimensionalCoordinates()
        {
            InitializeMembers();
        }

        // Properties:
        public int X
        {
            get
            {
                return m_x;
            }
            set
            {
                m_x = value;
            }
        }
        public int Y
        {
            get
            {
                return m_y;
            }
            set
            {
                m_y = value;
            }
        }
        [JsonIgnore]
        public string StringValue
        {
            get
            {
                return X.ToString() + "_" + Y.ToString();
            }
        }

        // Methods:
        public static TwoDimensionalCoordinates Parse(string stringValue)
        {
            string[] parsedStringValue = stringValue.Split(new char[] { '_' });
            if (parsedStringValue.Length != 2)
            {
                throw new ArgumentException(
                    "The argument stringValue is not a valid string representaiton of a two-dimensional coordinates.");
            }
            int x = int.Parse(parsedStringValue[0]);
            int y = int.Parse(parsedStringValue[1]);
            return new TwoDimensionalCoordinates(x, y);
        }

        // Initializers:
        private void InitializeMembers(int x, int y)
        {
            X = x;
            Y = y;
        }
        private void InitializeMembers()
        {
            X = 0;
            Y = 0;
        }

        // Private members:
        private int m_x;
        private int m_y;
    }
}
