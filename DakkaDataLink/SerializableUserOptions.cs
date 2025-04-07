using System.IO;
using System.Windows.Media;
using System.Xml.Serialization;

namespace DakkaDataLink
{
    public class SerializableUserOptions : UserOptions
    {
        public SerializableUserOptions(UserOptions options)
        {
            OverlayBackgroundColor_BrushName = options.OverlayBackgroundColor.ToString();
            OverlayValuesFontColor_BrushName = options.OverlayValuesFontColor.ToString();
            OverlayLabelsFontColor_BrushName = options.OverlayLabelsFontColor.ToString();
            OverlayAlertColor_BrushName = options.OverlayAlertBorderColor.ToString();
            OverlayFontSize = options.OverlayFontSize;
            OverlayOpacity = options.OverlayOpacity;
            AudioAlertVolume = options.AudioAlertVolume;
            AudioAlertFile = options.AudioAlertFile;
            Language = options.Language;
            SaveSelectedLanguage = options.SaveSelectedLanguage;
            MyCallSign = options.MyCallSign;
            LastServerIp = options.LastServerIp;

            BindingDictionary_List = new List<string>();
            foreach (KeyValuePair<string, KeyCombo> entry in options.BindingDictionary)
            {
                BindingDictionary_List.Add(entry.Key);
                BindingDictionary_List.Add(entry.Value.ToString());
            }

        }

        private SerializableUserOptions()
        {

        }

        public void SerializeToFile(string filePath)
        {
            
            XmlSerializer ser = new XmlSerializer(typeof(SerializableUserOptions));
            TextWriter writer = new StreamWriter(filePath);
            ser.Serialize(writer, this);
            writer.Close();
        }

        public UserOptions? DeserializeFrom(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableUserOptions));
            SerializableUserOptions tempObject;
            using (Stream reader = new FileStream(filePath, FileMode.Open))
            {
                tempObject = (SerializableUserOptions)serializer.Deserialize(reader);
                if (tempObject == null)
                {
                    return null;
                }

                BrushConverter brushConverter = new BrushConverter();
                OverlayBackgroundColor = (SolidColorBrush)brushConverter.ConvertFromString(tempObject.OverlayBackgroundColor_BrushName);
                OverlayValuesFontColor = (SolidColorBrush)brushConverter.ConvertFromString(tempObject.OverlayValuesFontColor_BrushName);
                OverlayLabelsFontColor = (SolidColorBrush)brushConverter.ConvertFromString(tempObject.OverlayLabelsFontColor_BrushName);
                OverlayAlertBorderColor = (SolidColorBrush)brushConverter.ConvertFromString(tempObject.OverlayAlertColor_BrushName);

                OverlayOpacity = tempObject.OverlayOpacity;
                OverlayFontSize = tempObject.OverlayFontSize;

                AudioAlertVolume = tempObject.AudioAlertVolume;
                AudioAlertFile = tempObject.AudioAlertFile;

                Language = tempObject.Language;
                SaveSelectedLanguage = tempObject.SaveSelectedLanguage;

                MyCallSign = tempObject.MyCallSign;

                LastServerIp = tempObject.LastServerIp;

                for (int i=0; i < tempObject.BindingDictionary_List.Count - 1; i += 2)
                {
                    string dictKeyString = tempObject.BindingDictionary_List[i];
                    string valueString = tempObject.BindingDictionary_List[i + 1];

                    List<System.Windows.Input.Key?> keysInCombo = new List<System.Windows.Input.Key?>();
                    string[] keyStringsInCombo = valueString.Split("+");

                    foreach (string key in keyStringsInCombo)
                    {
                        System.Windows.Input.Key parsedKey;
                        string trimmed = key.Trim();
                        System.Enum.TryParse<System.Windows.Input.Key>(trimmed, out parsedKey);
                        keysInCombo.Add(parsedKey);
                    }

                    KeyCombo theCombo = new KeyCombo(keysInCombo);

                    BindingDictionary[dictKeyString] = theCombo;
                }
            }
            return this;
        }

        public string OverlayBackgroundColor_BrushName;
        public string OverlayValuesFontColor_BrushName;
        public string OverlayLabelsFontColor_BrushName;
        public string OverlayAlertColor_BrushName;

        /// <summary>
        /// Valid format is index 2n == key, index 2n+1 == value
        /// </summary>
        public List<string> BindingDictionary_List;
    }
}
