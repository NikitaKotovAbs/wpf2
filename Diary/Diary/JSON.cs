using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary
{
    public class JSON
    {


        public void JsonFileManager(string path, string fileName)
        {
            if (!File.Exists(path + fileName))
            {
                File.Create(path + fileName);
            }
        }
        public static List<Note> JsonReader(List<Note> notes, string path, string fileName)
        {
            notes = new List<Note>();

            if (!File.Exists(path + fileName))
            {
                new JSON().JsonFileManager(path, fileName);
            }
            else
            {
                string jsonString = File.ReadAllText(path + fileName);
                if (jsonString.Length != 0)
                {
                    notes = JsonConvert.DeserializeObject<List<Note>>(jsonString);
                }
            }
            return notes;
        }

        public static void JsonWriter(string path, string fileName, List<Note> notesJson)
        {
            if (!File.Exists(path + fileName))
            {
                new JSON().JsonFileManager(path, fileName);
            }
            else
            {
                string jsonString = JsonConvert.SerializeObject(notesJson);
                File.WriteAllText(path + fileName, jsonString);
            }
        }
    }
}
