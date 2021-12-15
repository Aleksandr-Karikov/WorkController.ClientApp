using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkController.Client.Http.RequstModels.Base;

namespace WorkController.Client.Http.RequstModels
{
    class RegisterModel: BaseRequest
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("LastName")]
        public string LastName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("chiefId")]
        public int? ChiefID { get; set; }

    }
}
