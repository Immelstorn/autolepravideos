using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using AutoLepraTop.BL.Managers;

namespace AutoLepraTop.API.Controllers
{
    public class AutoController : ApiController
    {
        private LepraManager _manager = new LepraManager();
        public IHttpActionResult Get()
        {

            Task.Factory.StartNew(() => _manager.CheckAndUpdateDbIfNeeded());
            return Ok();
        }

    }
}
