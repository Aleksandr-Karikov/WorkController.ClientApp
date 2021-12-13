using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkController.Common.Helper;

namespace WorkControllerAdmin.Http.RequstModels.Base
{
    internal class BaseRequest:IRequest
    {
        [JsonProperty("isAdmin")]
        public bool IsAdmin = false;
    }
}
