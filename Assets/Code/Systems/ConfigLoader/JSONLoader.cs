using System.IO;
using UnityEngine;

namespace Systems.ConfigLoader
{
    public class JsonLoader : IConfigLoader
    {
        private string _loadPath;

        public JsonLoader(string loadPath)
        {
            _loadPath = loadPath;
        }


        public bool TryLoadConfig<T>(string configName, out T config)
        {
            var filePath = ConstructFullPath(configName);

            if (!File.Exists(filePath))
            {
                Debug.LogError("Config file not found at path: " + filePath);
                config = default;
                return false;
            }

            var content = File.ReadAllText(filePath);
            config = JsonUtility.FromJson<T>(content);
            return true;
        }

        private string ConstructFullPath(string configName) => Path.Combine(_loadPath, $"{configName}.json");

    }

}