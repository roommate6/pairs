using System;
using System.IO;
using System.Windows.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace pairs.Classes
{
    internal class UtilitySerialization
    {
        // Class members:
        public static readonly string PATH_TO_PERSISTENCE = "Persistence";
        public static readonly string FULL_PATH_TO_PERSISTENCE = FilePath.PATH_RESOURCES + PATH_TO_PERSISTENCE;
        public static readonly string USERS_NAME = "users.json";

        // Methods:
        public static void SerializeJson2(string filePath, object input)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
                {
                    
                    new JsonSerializer().Serialize(jsonWriter, input);
                }
            }
        }public static void SerializeJson(string filePath, object input)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            string jsonObject = JsonConvert.SerializeObject(input, Formatting.Indented);
            File.WriteAllText(filePath, jsonObject);
        }
        public static object DeserializeJson(string filePath, Type outputType)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            return JsonConvert.DeserializeObject(File.ReadAllText(filePath), outputType);
        }

        // Properties:
        public static string PATH_TO_USERS
        {
            get
            {
                return PATH_TO_PERSISTENCE + @"\" + USERS_NAME;
            }
        }
        public static string FULL_PATH_TO_USERS
        {
            get
            {
                return FULL_PATH_TO_PERSISTENCE + @"\" + USERS_NAME;
            }
        }
    }
}
