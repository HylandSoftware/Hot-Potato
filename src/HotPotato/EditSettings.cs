
using System.IO;
using Newtonsoft.Json;

namespace HotPotato
{
    class EditSettings
    {
        public static void swagdoc ()
        {

        }
        public static void EditSpecLocation(string specLoc)
        {
            string json = File.ReadAllText("appsettings.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj["SpecLocation"] = specLoc;
        }
    }
}
