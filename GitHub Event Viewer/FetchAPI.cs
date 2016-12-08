using System.Json;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GitHub_Event_Viewer
{
    [Serializable()]
    public class FetchAPI
    {
        private string url;
        private string json;
        private int rateLimitRemaining, rateResetTime;

        public FetchAPI(string url)
        {
            this.url = url;
        }

        public JArray refresh()
        {
            // Get Current Epoch Seconds Below:
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;

            if(rateLimitRemaining > -1) // if rateLimitRemaining is even set
            {
                if(rateLimitRemaining < 2) // if we have no refresh attempts left
                {
                    if (rateResetTime >= secondsSinceEpoch) // if we still have to wait for the attempt counter to reset
                    {
                        return JArray.Parse(this.json); // return old data
                    }
                }
            }

            var request = (HttpWebRequest)WebRequest.Create(this.url);
                request.UserAgent = "GitHub_Event_Viewer";  // UserAgent needs to be set or we will get a 403 Forbidden Error
                request.Accept = "application/vnd.github.v3+json"; // Accept type is recommended by GitHub API
                request.ContentType = "application/json";
                request.Method = "GET";
            HttpWebResponse response = null;
            
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e) // If we fail to get the web data, return null.
            {
                System.Console.Out.WriteLine("Failed to fetch API data.\n"+e.ToString());
                return null;
            }
            

            string limitRemaining = response.Headers["X-RateLimit-Remaining"]; // Get the number of attempts we have left. Github limits to 60 requests per hour; aka once per minute
            this.rateLimitRemaining = Int32.Parse(limitRemaining);

            string limitReset = response.Headers["X-RateLimit-Reset"]; // Get the reset timer for when we get more attempts
            this.rateResetTime = Int32.Parse(limitReset);

            var rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();
            this.json = rawJson;

            JArray jsonArray = JArray.Parse(rawJson);

            return jsonArray;
        } // END refresh

        public string getRawJson()
        {
            return this.json;
        }

        public JArray getJsonArray()
        {
            return JArray.Parse(this.json);
        }

        public int getRateLimitRemaining()
        {
            return rateLimitRemaining;
        }

        public int getResetTime()
        {
            return rateResetTime;
        }
    }
}