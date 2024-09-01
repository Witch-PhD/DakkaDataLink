using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PitBoss
{
    public class KeyCombo : IEquatable<KeyCombo>
    {
        public KeyCombo(Key? key1 = null, Key? key2 = null, Key? key3 = null)
        {
            theComboList = new List<Key?>();
            if (key1 != null) theComboList.Add(key1);
            if (key2 != null) theComboList.Add(key2);
            if (key3 != null) theComboList.Add(key3);
        }

        public KeyCombo(List<Key?> theComboList)
        {
            this.theComboList = theComboList;
        }

        public List<Key?> theComboList;

        public override string ToString()
        {
            string theString = "";
            foreach (Key? key in theComboList)
            {
                theString += key.ToString() + " + ";
            }
            theString = theString.Remove(theString.Length - 3); // Remove the final " + "
            return theString;
        }

        public static bool operator ==(KeyCombo thisCombo, KeyCombo thatCombo)
        {
            foreach (Key myKey in thisCombo.theComboList)
            {
                if (!(thatCombo.theComboList.Contains(myKey)))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator !=(KeyCombo thisCombo, KeyCombo thatCombo)
        {
            throw new NotImplementedException();
        }

        bool IEquatable<KeyCombo>.Equals(KeyCombo? other)
        {
            foreach (Key myKey in theComboList)
            {
                if (!(other.theComboList.Contains(myKey)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
