using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Android.Graphics;
using System.Net;

namespace GitHub_Event_Viewer
{
    [Activity(Label = "Event Details")]
    public class EventDisplay : Activity
    {
        private FetchAPI json;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EventDisplay);
            /*
                        passArray Values:
                        [0] = item["type"]
                        [1] = item["public"]
                        [2] = item["payload"]["ref"]
                        [3] = item["payload"]["ref_type"]
                        [4] = item["payload"]["master_branch"]
                        [5] = item["payload"]["description"]
                        [6] = item["payload"]["pusher_type"]
                        [7] = item["repo"]["id"]
                        [8] = item["repo"]["name"]
                        [9] = item["repo"]["url"]
                        [10] = item["actor"]["id"]
                        [11] = item["actor"]["login"]
                        [12] = item["actor"]["display_login"]
                        [13] = item["actor"]["gravatar_id"]
                        [14] = item["actor"]["avatar_url"]
                        [15] = item["actor"]["url"]
                        [16] = item["org"]["id"]
                        [17] = item["org"]["login"]
                        [18] = item["org"]["gravatar_id"]
                        [19] = item["org"]["url"]
                        [20] = item["org"]["avatar_url"]
                        [21] = item["created_at"]
                        [22] = item["id"]
                        [23] = text // original list text
            */

            String[] passArray = Intent.Extras.GetStringArray("stringArrayPass"); // Get the array of event data we passed
            
            TextView textEventId = FindViewById<TextView>(Resource.Id.TextEventId);
            TextView textEventDetails = FindViewById<TextView>(Resource.Id.TextEventDetails);
            TextView textActorName = FindViewById<TextView>(Resource.Id.TextActorName);
            ImageView imageAvatar = FindViewById<ImageView>(Resource.Id.ImageAvatar);
            Button buttonUserURL = FindViewById<Button>(Resource.Id.ButtonUserURL);
            TextView textRepoName = FindViewById<TextView>(Resource.Id.TextRepoName);
            TextView textRepoId = FindViewById<TextView>(Resource.Id.TextRepoId);
            Button buttonRepoURL = FindViewById<Button>(Resource.Id.ButtonRepoURL);
            Button buttonBack = FindViewById<Button>(Resource.Id.ButtonBack);

            
            try { textEventId.Text = "Event Id: " + passArray[22].ToString(); } catch (NullReferenceException e) { textEventId.Text = "Event Id: N/A"; }
            textEventDetails.Text = passArray[23].ToString();
            textActorName.Text = passArray[12].ToString();

            if (passArray[14].ToString().Length > 1)
            {
                Bitmap imageBitmap = GetImageBitmapFromUrl(passArray[14].ToString());
                imageAvatar.SetImageBitmap(imageBitmap);
            }

            if (passArray[11].ToString().Length < 2) // if the user login name is not set, disable the button
            {
                buttonUserURL.Enabled = false;
            }

            textRepoName.Text = "Name: " + passArray[8].ToString();
            textRepoId.Text = "Id: " + passArray[7].ToString();

            if (passArray[9].ToString().Length < 2) // if the repo api url is not set, disable the button
            {
                buttonRepoURL.Enabled = false;
            }

            buttonUserURL.Click += delegate { // Go back to previous activity
                var uri = Android.Net.Uri.Parse("https://github.com/" + passArray[11].ToString());
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            };

            buttonRepoURL.Click += delegate { // Go back to previous activity
                var uri = Android.Net.Uri.Parse(passArray[9].ToString());
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            };

            buttonBack.Click += delegate { // Go back to previous activity
                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                };
        }


        private Bitmap GetImageBitmapFromUrl(string url)
        {
            // Bitmap loading code taken from:
            // http://stackoverflow.com/questions/23860511/load-image-from-url-to-imageview-c-sharp
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("user-agent", "GitHub_Event_Viewer");
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
                else
                {
                    Android.Widget.Toast.MakeText(this, "Failed to load user avatar!", Android.Widget.ToastLength.Short).Show();
                }
            }

            return imageBitmap;
        }
    }
}
 
 