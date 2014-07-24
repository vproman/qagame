Here's what this template, which is based on the Hub app universal
template, comes with:

- An About page in Windows Phone, and About pages that show up in the
Settings charm in Windows.  See the Shared/About directory for the
source to these pages.  The Windows Phone About page itself is in
WindowsPhone/About/About.xaml, and the Windows About pages are set up
in Shared/App.xaml.cs.  Search for "TODOABOUT" to find things you should
change to fit your app.

- Sample settings that the user can configure, including an enumeration
setting.  These are defined in Shared/Model/UserSettings.cs, and can be
set by the user in the About page in Windows Phone or the Settings page
in Windows (Shared/About/Settings.xaml)

- A first-run tutorial that runs the first time the user runs the app.
(and can be re-run from the Tips section of the About page)  To add
or change the screens, make changes to the children of the FirstRunCanvas
in the appropriate HubPage.xaml, then update MAX_FIRSTRUNINDEX and the
UpdateFirstRunCanvas() method in the appropriate HubPage.xaml.cs.  If you'd
like to disable the first-run tutorial, just comment out the line in CheckFirstRun()
in the appropriate HubPage.xaml.cs.  If you'd like to always make the first-run
tutorial run on launch (for testing purposes), set alwaysDoFirstRun = true in
CheckFirstRun() in Shared/Utils.cs.  Search for "COMMENTFIRSTRUN" to find places
you can customize the tutorial.

- This template does _not_ come with a rating reminder - to get one, add
the "AppPromo" package in NuGet, and see
http://code.msdn.microsoft.com/Improve-app-ratings-with-a-6139caa5#content
for more info about the package.

Thanks for using this template!  If you find it useful, or have any
problems or suggestions, please drop me a line!

-Greg Stoll
Developer Ambassador for Windows Phone
ext-greg.stoll@microsoft.com
@gregstoll
http://austin.devnokia.com
