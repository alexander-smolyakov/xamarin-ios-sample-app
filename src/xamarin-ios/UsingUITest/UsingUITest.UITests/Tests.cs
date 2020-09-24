using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UsingUITest.UITests
{
	//[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public class Tests
	{
		IApp app;
		Platform platform;

		static readonly Func<AppQuery, AppQuery> InitialMessage = c => c.Marked("MyLabel").Text("Hello, Xamarin.Forms!");
		static readonly Func<AppQuery, AppQuery> Button = c => c.Marked("MyButton");
		static readonly Func<AppQuery, AppQuery> DoneMessage = c => c.Marked("MyLabel").Text("Was clicked");

        public string PathToAPK = string.Empty;

        public Tests(Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public void BeforeEachTest()
		{
            string currentFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            FileInfo fi = new FileInfo(currentFile);
            string dir = fi.Directory.Parent.Parent.Parent.FullName;

            // PathToAPK is a property or an instance variable in the test class
            PathToAPK = Path.Combine(dir, "Droid", "bin", "Release", "com.xamarin.usinguitest.apk");

            if (platform == Platform.Android)
            {
                app = ConfigureApp.Android.ApkFile(PathToAPK).StartApp();
            }

            //app = AppInitializer.StartApp(platform);
		}

		[Test]
		public void AppLaunches()
		{
			//app.Repl();
			// Arrange - Nothing to do because the queries have already been initialized.
			AppResult[] result = app.Query(InitialMessage);
			Assert.IsTrue(result.Any(), "The initial message string isn't correct - maybe the app wasn't re-started?");

			// Act
			app.Tap(Button);

			// Assert
			result = app.Query(DoneMessage);
			Assert.IsTrue(result.Any(), "The 'clicked' message is not being displayed.");
		}
	}
}

