using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using AutoLepraTop.BL.Managers;
using AutoLepraTop.BL.Models;

namespace AutoLepraTop.API.Controllers
{
    public class AutoController : ApiController
    {
        private LepraManager _manager = new LepraManager();

        public AutoController()
        {
            Task.Factory.StartNew(() => _manager.CheckAndUpdateDbIfNeeded());
        }

        public async Task<JsonResult<ListDto<CommentDto>>> Get(int page = 1, string sort = "byrating", string from = "", string to = "")
        {
            var result = await _manager.Get(page, sort, from, to);
            return Json(result);
        }

    }
}
