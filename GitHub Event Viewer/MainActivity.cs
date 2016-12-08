using Android.App;
using Android.Widget;
using Android.OS;
using System.Json;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using Android.Views;
using System;
using Android.Content;
using System.Runtime.Serialization.Formatters.Binary;

namespace GitHub_Event_Viewer
{
    [Activity(Label = "GitHub Event Viewer", MainLauncher = true)]//, Icon = "@drawable/icon")]
    public class MainActivity : ListActivity
    {
        const int numberOfResults = 31; // 30 results plus the refresh button
        string[] items, itemId; // json results as items and their id's
        FetchAPI json; // the class refrence to our json fetcher

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.Main); NOT NEEDED

            string url = "https://api.github.com/events";
            this.json = new FetchAPI(url);

            JArray jsonArray = json.refresh(); // I know this should be done on another thread, but I don't know how to do that yet.
            displayRefreshTimer(); // Display the number of refreshes left.


            if (jsonArray != null) // IF data was returned
            { 
                int i = 1;
                this.items = new string[numberOfResults];
                this.itemId = new string[numberOfResults];

                this.items[0] = "Refresh List"; // Our first item is the refresh button
                this.itemId[0] = "";

                // Convert JsonArray into String Array
                foreach (var item in jsonArray.Children())
                {
                    if (i < numberOfResults) // only add when we are in-bounds of the array's index
                    {
                        string task = "";

                        switch (item["type"].ToString())
                        {
                            // All 30 Event types, as described by:
                            // https://developer.github.com/v3/activity/events/types/

                            case "CommitCommentEvent": task = "commented on a commit in"; break;
                            case "CreateEvent": task = "created"; break;
                            case "DeleteEvent": task = "deleted"; break;
                            case "DeploymentEvent": task = "deployed"; break;
                            case "DeploymentStatusEvent": task = "updated the deployment status of"; break;
                            case "DownloadEvent": task = "created a new download for"; break; // depreicated event, but still in old data
                            case "FollowEvent": task = "followed"; break; // depricated event, but still in old data
                            case "ForkEvent": task = "forked"; break;
                            case "ForkApplyEvent": task = "applied a patch in the fork queue for"; break;
                            case "GistEvent": task = "created/updated a gist for"; break;
                            case "GollumEvent": task = "created/updated the wiki for"; break;
                            case "IssueCommentEvent": task = "created/edited/deleted an issue comment for"; break;
                            case "IssuesEvent": task = "edited an issue for"; break;
                            case "LabelEvent": task = "created/edited/deleted a label for"; break;
                            case "MemberEvent": task = "changed member permissions on"; break;
                            case "MembershipEvent": task = "changed the team for"; break;
                            case "MilestoneEvent": task = "created/edited/deleted a milestone for"; break;
                            case "OrganizationEvent": task = "changed the organization users for"; break;
                            case "PageBuildEvent": task = "attempted to build a GitHub Pages site for"; break;
                            case "PublicEvent": task = "open sourced"; break;
                            case "PullRequestEvent": task = "opened/edited/closed a pull request for"; break;
                            case "PullRequestReviewEvent": task = "submitted a pull request review"; break;
                            case "PullRequestReviewCommentEvent": task = "submitted a comment on a pull request for"; break;
                            case "PushEvent": task = "pushed to"; break;
                            case "ReleaseEvent": task = "published a release for"; break;
                            case "RepositoryEvent": task = "created/deleted"; break;
                            case "StatusEvent": task = "changed the status on a commit for"; break;
                            case "TeamEvent": task = "created/deleted an organization team for"; break;
                            case "TeamAddEvent": task = "added a repo to the team. Repo:"; break;
                            case "WatchEvent": task = "starred"; break;
                            default: task = "edited"; break;
                        }

                        this.items[i] = item["actor"]["display_login"] + " just " + task + " " + item["repo"]["name"] + ".";
                        this.itemId[i] = item["id"].ToString();
                        i = i + 1;
                    }
                }

                while (i < numberOfResults) // If we get less than the expected number of results, fill them in to avoid null errors with the ListAdapter.
                {
                    this.items[i] = "...";
                    this.itemId[i] = "";
                    i = i + 1;
                }                

                ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, this.items);

            }
            else // ELSE IF data was returned
            {
                Android.Widget.Toast.MakeText(this, "Failed to load API data.", Android.Widget.ToastLength.Long).Show();
            } // END ELSE IF data was returned
        } // END OnCreate

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            if (position == 0) // IF refresh button hit
            {
                JArray jsonArray = json.refresh(); // I know this should be done on another thread, but I don't know how to do that yet.
                int remaining = json.getRateLimitRemaining();

                if (remaining < 2) // IF almost no refresh attempts left
                {
                    int resetTime = json.getResetTime();
                    DateTime timer = FromUnixTime(resetTime);
                    TimeZone localZone = TimeZone.CurrentTimeZone;
                    timer = TimeZoneInfo.ConvertTimeFromUtc(timer, TimeZoneInfo.Local);
                    Android.Widget.Toast.MakeText(this, "Max refreshes hit! Wait until " + timer.ToString("hh:mm:ss") + " to try again.", Android.Widget.ToastLength.Short).Show();
                }
                else // ELSE IF almost no refresh attempts left
                {
                    displayRefreshTimer();
                    int i = 1;
                    this.items = new string[numberOfResults];
                    this.itemId = new string[numberOfResults];
                    string[] images = new string[numberOfResults];

                    this.items[0] = "Refresh List"; // Our first item is the refresh button

                    // Convert JsonArray into String Array
                    foreach (var item in jsonArray.Children())
                    {
                        if (i < numberOfResults) // only add when we are in-bounds of the array's index
                        {
                            string task = "";

                            switch (item["type"].ToString())
                            {
                                // All 30 Event types, as described by:
                                // https://developer.github.com/v3/activity/events/types/

                                case "CommitCommentEvent": task = "commented on a commit in"; break;
                                case "CreateEvent": task = "created"; break;
                                case "DeleteEvent": task = "deleted"; break;
                                case "DeploymentEvent": task = "deployed"; break;
                                case "DeploymentStatusEvent": task = "updated the deployment status of"; break;
                                case "DownloadEvent": task = "created a new download for"; break; // depreicated event, but still in old data
                                case "FollowEvent": task = "followed"; break; // depricated event, but still in old data
                                case "ForkEvent": task = "forked"; break;
                                case "ForkApplyEvent": task = "applied a patch in the fork queue for"; break;
                                case "GistEvent": task = "created/updated a gist for"; break;
                                case "GollumEvent": task = "created/updated the wiki for"; break;
                                case "IssueCommentEvent": task = "created/edited/deleted an issue comment for"; break;
                                case "IssuesEvent": task = "edited an issue for"; break;
                                case "LabelEvent": task = "created/edited/deleted a label for"; break;
                                case "MemberEvent": task = "changed member permissions on"; break;
                                case "MembershipEvent": task = "changed the team for"; break;
                                case "MilestoneEvent": task = "created/edited/deleted a milestone for"; break;
                                case "OrganizationEvent": task = "changed the organization users for"; break;
                                case "PageBuildEvent": task = "attempted to build a GitHub Pages site for"; break;
                                case "PublicEvent": task = "open sourced"; break;
                                case "PullRequestEvent": task = "opened/edited/closed a pull request for"; break;
                                case "PullRequestReviewEvent": task = "submitted a pull request review"; break;
                                case "PullRequestReviewCommentEvent": task = "submitted a comment on a pull request for"; break;
                                case "PushEvent": task = "pushed to"; break;
                                case "ReleaseEvent": task = "published a release for"; break;
                                case "RepositoryEvent": task = "created/deleted"; break;
                                case "StatusEvent": task = "changed the status on a commit for"; break;
                                case "TeamEvent": task = "created/deleted an organization team for"; break;
                                case "TeamAddEvent": task = "added a repo to the team. Repo:"; break;
                                case "WatchEvent": task = "starred"; break;
                                default: task = "edited"; break;
                            }

                            this.items[i] = item["actor"]["display_login"] + " just " + task + " " + item["repo"]["name"] + ".";
                            this.itemId[i] = item["id"].ToString();
                            i = i + 1;
                        }
                    }

                    while (i < numberOfResults) // If we get less than the expected number of results, fill them in to avoid null errors with the ListAdapter.
                    {
                        items[i] = "...";
                        i = i + 1;
                    }

                    ListView.RefreshDrawableState();
                    ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, this.items);
                } // END ELSE IF almost no refresh attempts left
            }
            else // ELSE IF refresh button hit
            {
                
                string clickedId = this.itemId[position];
                var text = items[position];
                
                JArray jsonArray = json.getJsonArray();
                string[] passArray = new string[24]; // the array we will pass to the next activity that contains details about this event

                foreach (var item in jsonArray.Children()) // Get our original data for the list and loop through it
                {
                    if (string.Compare(item["id"].ToString(), clickedId, false) == 0) // if the clicked id matches the id of the original data; id's are unique
                    {
                        // put the original data into a string array to pass to the next activity
                        // use a try/catch for null references since GitLab just drops blank values, instead of returning an empty string.

                        try { passArray[0] = item["type"].ToString(); } catch (NullReferenceException e) { passArray[0] = ""; }
                        try { passArray[1] = item["public"].ToString(); } catch (NullReferenceException e) { passArray[1] = ""; }
                        try { passArray[2] = item["payload"]["ref"].ToString(); } catch (NullReferenceException e) { passArray[2] = ""; }
                        try { passArray[3] = item["payload"]["ref_type"].ToString(); } catch (NullReferenceException e) { passArray[3] = ""; }
                        try { passArray[4] = item["payload"]["master_branch"].ToString(); } catch (NullReferenceException e) { passArray[4] = ""; }
                        try { passArray[5] = item["payload"]["description"].ToString(); } catch (NullReferenceException e) { passArray[5] = ""; }
                        try { passArray[6] = item["payload"]["pusher_type"].ToString(); } catch (NullReferenceException e) { passArray[6] = ""; }
                        try { passArray[7] = item["repo"]["id"].ToString(); } catch (NullReferenceException e) { passArray[7] = ""; }
                        try { passArray[8] = item["repo"]["name"].ToString(); } catch (NullReferenceException e) { passArray[8] = ""; }
                        try { passArray[9] = item["repo"]["url"].ToString(); } catch (NullReferenceException e) { passArray[9] = ""; }
                        try { passArray[10] = item["actor"]["id"].ToString(); } catch (NullReferenceException e) { passArray[10] = ""; }
                        try { passArray[11] = item["actor"]["login"].ToString(); } catch (NullReferenceException e) { passArray[11] = ""; }
                        try { passArray[12] = item["actor"]["display_login"].ToString(); } catch (NullReferenceException e) { passArray[12] = ""; }
                        try { passArray[13] = item["actor"]["gravatar_id"].ToString(); } catch (NullReferenceException e) { passArray[13] = ""; }
                        try { passArray[14] = item["actor"]["avatar_url"].ToString(); } catch (NullReferenceException e) { passArray[14] = ""; }
                        try { passArray[15] = item["actor"]["url"].ToString(); } catch (NullReferenceException e) { passArray[15] = ""; }
                        try { passArray[16] = item["org"]["id"].ToString(); }catch (NullReferenceException e){ passArray[16] = ""; }
                        try { passArray[17] = item["org"]["login"].ToString(); } catch (NullReferenceException e) { passArray[17] = ""; }
                        try { passArray[18] = item["org"]["gravitar_id"].ToString(); } catch (NullReferenceException e) { passArray[18] = ""; }
                        try { passArray[19] = item["org"]["url"].ToString(); } catch (NullReferenceException e) { passArray[19] = ""; }
                        try { passArray[20] = item["org"]["avatar_url"].ToString(); } catch (NullReferenceException e) { passArray[20] = ""; }
                        try { passArray[21] = item["created_at"].ToString(); } catch (NullReferenceException e) { passArray[21] = ""; }
                        try { passArray[22] = item["id"].ToString(); } catch (NullReferenceException e) { passArray[22] = ""; }
                        try { passArray[23] = text.ToString();  }catch (NullReferenceException e) { passArray[23] = "";  }
                    }
                }


                Bundle bundle = new Bundle();
                bundle.PutStringArray("stringArrayPass", passArray);
                Intent intent = new Intent(this, typeof(EventDisplay));
                intent.PutExtras(bundle);
                StartActivity(intent);

            } // END ELSE IF refresh button hit
        } // END OnListItemClick

        public DateTime FromUnixTime(long unixTime)
        {
            // Unix time code here taken from:
            // http://stackoverflow.com/questions/2883576/how-do-you-convert-epoch-time-in-c
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        } // END FromUnixTime

        public void displayRefreshTimer()
        {
            // Displays the number of refreshes we have left and the time it resets at.
            // GitHub has a refresh limit of 60 per hour or 1 per minute. It resets every hour and provides this data in the API's headers.
            int remaining = json.getRateLimitRemaining();
            int resetTime = json.getResetTime();
            DateTime timer = FromUnixTime(resetTime);
            TimeZone localZone = TimeZone.CurrentTimeZone;
            timer = TimeZoneInfo.ConvertTimeFromUtc(timer, TimeZoneInfo.Local);
            Android.Widget.Toast.MakeText(this, remaining.ToString() + " refreshes remaining. Resets at " + timer.ToString("hh:mm:ss"), Android.Widget.ToastLength.Long).Show();
        }
    }
}
 