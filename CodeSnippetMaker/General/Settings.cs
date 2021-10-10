using CodeSnippetMaker.Models;
using Newtonsoft.Json;
using System.IO;

namespace CodeSnippetMaker.General
{
    public static class Settings
    {
        public static void Save(ViewModel view)
        {
            if (Directory.Exists(Helper.JsonFolder) == false)
            {
                Directory.CreateDirectory(Helper.JsonFolder);
            }

            string[] values = { view.Author,
                                view.Language,
                                view.ExportFolder };

            string json = JsonConvert.SerializeObject(values);
            File.WriteAllText(Helper.JsonPath, json);
        }

        public static string[] Load()
        {
            string json = File.ReadAllText(Helper.JsonPath);
            string[] values = JsonConvert.DeserializeObject<string[]>(json);
            return values;
        }
    }
}