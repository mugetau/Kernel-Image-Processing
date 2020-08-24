using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEnhancement.Models
{
    public static class JSONManager
    {
        public static string JSONExport(ImagePath imgPath)
        {
            string jsonString = "";
            try
            {
                if (imgPath == null)
                    throw new ArgumentException();

                string filePath = "config.json";

                jsonString = JsonConvert.SerializeObject(imgPath);
                System.IO.File.WriteAllText(filePath, jsonString);

            }
            catch (Exception e)
            {
                //process the exception in a better way
                Console.WriteLine(e.Message);
            }

            return jsonString;
        }
    }
}
