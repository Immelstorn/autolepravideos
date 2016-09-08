using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AutoLepraTop.Models;
using Newtonsoft.Json;

namespace AutoLepraTop.Controllers
{
    public class HomeController : Controller
    {
        const int ElementsOnPage = 50;
        public ActionResult Index()
        {
            return Page(0);
        }

        public ActionResult SortByDate()
        {
            var c = new HttpCookie("SortByDate") {Value = "true"};
            Response.SetCookie(c);
            Request.Cookies.Set(c);
            return Page(0);
        }

        public ActionResult SortByVote()
        {
            var c = new HttpCookie("SortByDate") { Value = "false" };
            Response.SetCookie(c);
            Request.Cookies.Set(c);
            return Page(0);
        }

        public ActionResult Page(int page)
        {
            var c = Request.Cookies.Get("SortByDate");
            string filePath = Server.MapPath(Url.Content(c == null || c.Value == "false"
                    ? "~/Content/byVote.json"
                    : "~/Content/byDateWithoutBad.json"));
           
            var json = System.IO.File.ReadAllText(filePath);
            var videos = JsonConvert.DeserializeObject<List<Video>>(json);
            var pageCount = videos.Count / ElementsOnPage + 1;

            if(page < 0)
            {
                page = 0;
            }
            if(page >= pageCount)
            {
                page = pageCount - 1;
            }

            var startElem = page * ElementsOnPage;
            var model = new MainModel
            {
                PageNumber = page,
                PagesCount = pageCount,
                Videos = videos.GetRange(startElem, videos.Count - startElem >= ElementsOnPage ? ElementsOnPage : videos.Count - startElem)
            };

            return View("Index", model);
        }
    }

}