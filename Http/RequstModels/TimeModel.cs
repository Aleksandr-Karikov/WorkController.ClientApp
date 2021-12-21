using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkController.Common.Helper;

namespace WorkController.Client.Http.RequstModels
{
    internal class TimeModel : IRequest
    {
        [JsonProperty("Milliseconds")]
        public int Time { get; set; }
        [JsonProperty("UserId")]
        public int Id { get; set; }
        [JsonProperty("DateTime")]
        public DateTime Date { get; set; }
        [JsonProperty("Screens")]
        public List<byte[]> Screens {get;set;}
    }
}
