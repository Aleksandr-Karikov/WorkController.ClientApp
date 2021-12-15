using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkController.Client.Http.RequstModels.Base;
namespace WorkController.Client.Http.RequstModels
{
    internal class LoginModel:BaseRequest
    {
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
