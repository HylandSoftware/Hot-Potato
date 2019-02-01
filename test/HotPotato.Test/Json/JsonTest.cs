using System.Collections.Generic;
using HotPotato;
using System.IO;
using Newtonsoft.Json;
using Xunit;

namespace HotPotato.Http.Default
{
    public class JsonTest
    {
        [Fact]
        public void SpecLocation_Changes()
        {
            string specLoc = "M:\\Source\\dotnet-full-sample\\edit.yaml";
            EditSettings.EditSpecLocation(specLoc);
            string json = File.ReadAllText("appsettings.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            Assert.Equal(specLoc, jsonObj["SpecLocation"].ToString());
        }
    }
}
