Hello reviewer.
Before you begin, I just wanted to say that I have never done a mobile project before. I didn't even know what tools were needed to even begin one. Most of my time was spent on Google just trying to get the emulator to run. I'm in no way complaining, but asking that you judge me based on what I was able to learn in this week. While my project is lacking in many ways, I at least now know what I don't know. If I had more time, or better someone to ask questions from, I believe my project would easily be much better. Again, I'm not complaining, only asking that you remember my starting point as you look over my project. Thank you. 

I included all files from the Visual Studio solution folder and I included all steps since you did say that "Submissions we can't run per the instructions will be rejected". I thought it would be better to include too much rather than too little.

Basic Xamarin/Android SDK setup: (I'm sure you already know how to do this.)

1) Download and install Xamarin/Visual Studio from: https://www.xamarin.com/download
2) After installing Xamarin/Visual Studio, setup the android SDK by going to "Tools" > "Android" > "Android SDK Manager".
  Find "Android 7.0 (API 24)" and look at the packages below its category.
  Check the boxes next to "Intel x86 Atom System Image", "Intel x86 Atom_64 System Image", "SDK Platform", "Google APIs", and "Sources for Android SDK".
  Also select the "Intel x86 Emulator Accelerator (HAXM installer)" under "Extras".
  Click on "Install Packages" and agree to all Licenses.
  (Be sure that all packages are shown as having checkmarks on the left. If one has an "X", click on it and agree to its license individually.)
3) Take note of the "SDK Path" listed at the top of the "Android SDK Manager" window from before. 
  Go to that directory and run the installer inside of /extras/intel/Hardware_Accelerated_Execution_Manager/ .
  Follow the installation instructions it gives you. Make sure you have the latest 64 bit Java jdk installed with the Windows path   variable for it set!
4) Back in Xamarin/Visual Studio, go to "Tools" > "Android" > "Android Emulator Manager".
  On the right, click on "Create". 
  Set "AVD Name" to "Nexus5".
  Set "Device" to "Nexus 5 (4.95", 1080 x 1920: xxhdpi)".
  Set "Target" to "Android 7.0 - API Level 24".
  Set "CPU/ABI" to "Intel Atom (x86)".
  Set "Skin" to "No Skin".
  Set "Front Camera" and "Back Camera" to "None".
  Set "RAM" to "1024" and "VM Heap" to "64".
  Set "Internal Storage" to "2000 MiB".
  Set "SD Card Size" to "500 MiB".
  Check "Use Host GPU".
  Click on "OK" button on both windows.


Running the project:

  5) Download the Solution from GitHub to your computer. ( https://github.com/TechAdvancer/techinterview-github/archive/master.zip )
  6) Find it and unzip it.
  7) In Visual Studio/Xamarin, go to "File" > "Open" > "Project/Solution" and find the project you just downloaded.
  8) Open "GitHub Event Viewer.sln".
  9) Go to "Build" > "Rebuild Solution" and wait for it to finish.
  10) Find the green arrow in the toolbar and check that "Nexus5 (Android 7.0 - API 24)" is written next to it. If not, click on the dropdown arrow next to the text and select it. Then click on the green arrow to run the project.

The Project:

  The viewer uses a ListView to show the user, event, and repo information. The very first entry is the refresh button to get updates with. Hitting it tells us the number of refreshes we have left since GitHub limits the number of refreshes to 60 per hour. Clicking on an entry gives you the Event Details.

  The Event Details activity gives you the repository name, id, and a link to its API. It also gives you the user who initiated the event and shows you his/her avatar. It also gives you a quick link to their profile page. Going back auto refreshes the list for you.

  That is pretty much all there is to it.

============================================================================================


# Technical Interview Homework: GitHub Dashboard


##Purpose
The purpose of this exercise is to assess the candidate’s ability to build cross platform software clients that satisfy stated requirements. The completed assignment should not only satisfy the requirements outlined below, but also give the candidate an opportunity to show-off their skills.

##Prerequisites
- Candidates must have a GitHub account

##Instructions
1. Fork this repository - [https://github.com/Praeses/techinterview-github](https://github.com/Praeses/techinterview-github)
2. Create a client that satisfies the requirements below
3. Include, at the top of this README, instructions required for the reviewer to run the submission
4. Include, at the top of this README, any other information that will be useful to the reviewer
5. Create a pull request prior to the due date to have your submission reviewed

Once the submission is reviewed the candidate will be notified and possibly invited to participate in a follow-up interview where interviewers will collaboratively work with the candidate to review the submission, discuss possible enhancements, and possibly implement a new feature. 

#####Additional Notes...

- Feel free to ask your point of contact any clarifying questions you might have. 
- Submissions must be relatively trivial to run as outlined in the candidate's instructions. We suggest that you test the run instructions on a clean clone of your repository. **Submissions we can't run per the instructions will be rejected.**
- Client technology for the submission is at the discretion of the candidate, Preferred platforms include the following...
	- **Xamarin** - Xamarin Studio or Visual Studio
	- **HTML/CSS/JS (touch friendly, tablet/phone targeted)**
	- **Native iOS** - Xcode 
	- **Native Android** - Android Studio
	- **Windows/Phone** - Visual Studio
- Third party libraries or packages are acceptable but must be managed via a package manager i.e. Nuget, CocoaPods, npm, bower, etc. Third party components will NOT be manually installed by the reviewer.

##Assessment

Cross platform client development requires a familiarity and aptitude to work with...

- Client platforms: iOS, Android, Windows, Web Browsers, etc.
- Presentation layer frameworks: Native iOS & Android, Cordova, HTML/CSS/JS, etc.
- HTTP based APIs

#####Assessment will focus on the candidate's ability to...

- Review and understand API documentation.
- Consume an API, and present API content in a client. 
- Write clear, understandable, and maintainable code. Please use the predominant coding style for whatever language the submission is written in.
- Create a simple and understandable user interface and user experience. Note that clear and understandable does NOT mean graphically interesting.
- The user experience should be targeted at a mobile screen size, preferably tablet optimized yet functional on a phone sized screen.

#####Exceptional Candidates will...

- Submit a solution that will run on both iOS and Android
- Have a simple architecture that is easy to enhance and extend

##Requirements - GitHub Dashboard 
GitHub has a public API that will be used for this assignment. We will be displaying content from the GitHub API in a user "dashboard."

#####Resources
- [GitHub API](https://developer.github.com/v3/ "GitHub API")

#####Minimum Requirements
- The client will have a title clearly indicating the purpose and content of the client.
- The client will be touch friendly.
- The client will display a feed of GitHub public events available at the following endpoint
	- Public Events url: [https://api.github.com/events](https://api.github.com/events)
	- Events Documentation: [https://developer.github.com/v3/activity/events/](https://developer.github.com/v3/activity/events/)
- Each public event displayed in the feed will display user friendly values for...
	- Username
	- Event Type
	- Repository to which the event applies
- The client will have a button to refresh the feed of public events.
- The client will allow a user to tap the public event and display event details.

#####Optional (stretch) Enhancements
- Implement a "pull down" to refresh the feed of public events.
- On the public event details screen...
	- Display a user's avatar next to their name.
	- Provide a link/button to display in the app or a separate browser window the GitHub repository's main web page.
- Authenticate a GitHub user
- Display the authenticated user's username and avatar in client-platform typical location
- Display a feed of the Events performed by the authenticated user
	- [https://developer.github.com/v3/activity/events/#list-events-performed-by-a-user](https://developer.github.com/v3/activity/events/#list-events-performed-by-a-user)
- Support screen rotation
