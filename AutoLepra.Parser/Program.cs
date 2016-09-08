using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CsQuery;
using Newtonsoft.Json;

namespace AutoLepra.Parser
{
    class Program
    {
        #region Ids
        private static readonly List<int> posts = new List<int>
        {
            866297,
            891938,
            922399,
            940359,
            956116,
            971316,
            978766,
            983395,
            992020,
            1005864,
            1017082,
            1030009,
            1040005,
            1047473,
            1054171,
            1061778,
            1065581,
            1074124,
            1082261,
            1087201,
            1092228,
            1097748,
            1105019,
            1111862,
            1117162,
            1128011,
            1138099,
            1148500,
            1157865,
            1169023,
            1177474,
            1189722,
            1203852,
            1211174,
            1219592,
            1230571,
            1241053,
            1253085,
            1262334,
            1277024,
            1286902,
            1295018,
            1302691,
            1310416,
            1316807,
            1325010,
            1330153,
            1336220,
            1340977,
            1346769,
            1352941,
            1359266,
            1364314,
            1369369,
            1376116,
            1382738,
            1390142,
            1395831,
            1401045,
            1407064,
            1412380,
            1420446,
            1426084,
            1431499,
            1438311,
            1442885,
            1447865,
            1452143,
            1457547,
            1462107,
            1467471,
            1472800,
            1477903,
            1483469,
            1492017,
            1496862,
            1502681,
            1507366,
            1513558,
            1519862,
            1527435,
            1534351,
            1545036,
            1552810,
            1560735,
            1568831,
            1581178,
            1590259,
            1596882,
            1604944,
            1612496,
            1619065,
            1630398,
            1644393,
            1652753,
            1660690,
            1669047,
            1674932,
            1692804,
            1709850,
            1726706,
            1736842,
            1745210,
            1757674,
            1772223,
            1788828,
            1818548
        };
        #endregion

        private const string MainUri = @"https://auto.leprosorium.ru/comments/{0}/";
        private static readonly string JsonPath = /*Path.GetTempPath()*/ @"C:\Users\dvo\" + Guid.NewGuid() + "\\";
        static List<Video> Videos = new List<Video>();

        private static void RemoveDoubles()
        {
            Directory.CreateDirectory(JsonPath);
            var comparer = new VideoComparer();
            Videos = JsonConvert.DeserializeObject<List<Video>>(File.ReadAllText(JsonPath + "byVote_doubles" + ".json"));

            Console.WriteLine("Doubles: " + (Videos.Count - Videos.Distinct(comparer).Count()));
            Videos = Videos.Distinct(comparer).ToList();
            Videos.Sort();
            var json = JsonConvert.SerializeObject(Videos);
            File.WriteAllText(JsonPath + "byVote" + ".json", json);
            
            Videos.Sort((video, video1) => video1.TimeStamp - video.TimeStamp);
            Videos.RemoveAll(v => v.Vote <= 0);
            json = JsonConvert.SerializeObject(Videos);
            File.WriteAllText(JsonPath + "byDateWithoutBad" + ".json", json);
            
        }

        static void Main(string[] args)
        {
            var s = new Stopwatch();
            s.Start();
            for (int index = 0; index < posts.Count; index++)
            {
                int post = posts[index];
                Parse(post);
                Console.WriteLine(index + " | " + Videos.Count);
            }
            Directory.CreateDirectory(JsonPath);
            Console.WriteLine("Doubles: "+ (Videos .Count - Videos.Distinct().Count()));
            Videos = Videos.Distinct().ToList();
            Videos.Sort();
            var json = JsonConvert.SerializeObject(Videos);
            File.WriteAllText(JsonPath + "byVote" + ".json", json);

            Videos.Sort((video, video1) => video1.TimeStamp - video.TimeStamp);
            Videos.RemoveAll(v => v.Vote <= 0);
            json = JsonConvert.SerializeObject(Videos);
            File.WriteAllText(JsonPath + "byDateWithoutBad" + ".json", json);

            s.Stop();
            Console.WriteLine(s.Elapsed);
            Console.WriteLine(JsonPath);
            Console.Read();
        }
        
        private static void Parse(int post)
        {
            var link = string.Format(MainUri, post);

            var stream = GetRequest(link);

            CQ dom = stream;
            var comments = dom["a[href*='youtube']"];

            foreach (var comment in comments)
            {
                var commentInner = comment.ParentNode.ParentNode;
                var body = commentInner.ChildElements.FirstOrDefault(e => e.HasClass("c_body"));
                if (body != null)
                {
                    var bodyText = WebUtility.HtmlDecode(body.Cq().Html());
                    var footer = commentInner.ChildElements.First(e => e.HasClass("c_footer"));
                    var ddi = footer.ChildElements.First(e => e.HasClass("ddi"));
                    var a = ddi.FirstElementChild.Attributes["href"].Replace(@"\n","http:");
                    var jsDate = ddi.ChildElements.First(e => e.HasClass("js-date"));
                    var timeStamp = jsDate.Attributes ["data-epoch_date"];
                    var commentId = a.Split('#') [1];
                    var rating = GetRequest("https://auto.leprosorium.ru/ajax/vote/list/", string.Format("comment={0}&limit=15&offset=0&csrf_token=", commentId));
                    var r = JsonConvert.DeserializeObject<Rating>(rating);
                    int realRating = 0;
                    if(r.pros != null)
                    {
                        realRating += r.pros_count;
                    }
                    if (r.cons != null)
                    {
                        realRating -= r.cons_count;
                    }

                    Videos.Add(new Video { Comment = bodyText, Vote = realRating, Link = a, TimeStamp = Convert.ToInt32(timeStamp)});
                }
            }
        }

        private static string GetRequest(string link, string requestData = "")
        {
            CookieContainer c = new CookieContainer();
            c.Add(new Uri(link), new Cookie("uid", ""));
            c.Add(new Uri(link), new Cookie("sid", ""));

            HttpWebRequest request = WebRequest.CreateHttp(link);
            if(request != null)
            {
                request.Proxy = null;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0";
                request.CookieContainer = c;
                request.Host = "auto.leprosorium.ru";
                request.Headers.Add("Cookie",
                                        "_ga=; uid=; sid=; _gat=1");
               
                byte[] data = null;
                if (!string.IsNullOrEmpty(requestData))
                {
                    var encoding = new ASCIIEncoding();
                    data = encoding.GetBytes(requestData);
                }
                if (data != null)
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = requestData.Length;
                    request.Method = "POST";
                    using (Stream newStream = request.GetRequestStream())
                    {
                        newStream.Write(data, 0, requestData.Length);
                    }
                }
            }

            string stream = "";
            if(request != null)
            {
                using(var response = request.GetResponse() as HttpWebResponse)
                {
                    if(response != null)
                    {
                        using(Stream receiveStream = response.GetResponseStream())
                        {
                            if(receiveStream != null)
                            {
                                using(var readStream = new StreamReader(receiveStream, Encoding.UTF8))
                                {
                                    stream = readStream.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            return stream;
        }
    }
}
