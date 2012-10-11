using System;
using NUnit.Framework;
using MonoTouch.UIKit;
using MonoTouch.Accounts;
using Xamarin.Social.Services;

namespace Xamarin.Social.iOS.Test
{
	[TestFixture]
	public class AppDotNetTest
	{
		AppDotNetService CreateService ()
		{
			return new AppDotNetService () {
				ClientId = "3uedEnHtN2Wuup6aUpK83uDAVUXcuSv7",
				RedirectUrl = new Uri ("http://xamarin.com"),
			};
		}

		[Test]
		public void Manual_Authenticate ()
		{
			var service = CreateService ();
			var vc = service.GetAuthenticateUI ((s, e) => {
				Console.WriteLine ("AUTHENTICATE RESULT = " + e.Account);
				AppDelegate.Shared.RootViewController.DismissModalViewControllerAnimated (true);
			});
			AppDelegate.Shared.RootViewController.PresentViewController (vc, true, null);
		}

		[Test]
		public void Manual_ShareText ()
		{
			var service = CreateService ();
			
			var item = new Item {
				Text = "This is just a test. Don't mind me...",
			};

			var vc = service.GetShareUI (item, result => {
				Console.WriteLine ("SHARE RESULT = " + result);
				item.Dispose ();
				AppDelegate.Shared.RootViewController.DismissModalViewControllerAnimated (true);
			});
			AppDelegate.Shared.RootViewController.PresentViewController (vc, true, null);
		}

		[Test]
		public void Manual_ShareTextLinks ()
		{
			var service = CreateService ();
			
			var item = new Item {
				Text = "This is just a test. Don't mind me...",
			};
			item.Links.Add (new Uri ("http://xamarin.com"));
			item.Links.Add (new Uri ("https://twitter.com/xamarinhq"));

			var vc = service.GetShareUI (item, result => {
				Console.WriteLine ("SHARE RESULT = " + result);
				item.Dispose ();
				AppDelegate.Shared.RootViewController.DismissModalViewControllerAnimated (true);
			});
			AppDelegate.Shared.RootViewController.PresentViewController (vc, true, null);
		}

		[Test]
		public void HomeTimeline ()
		{
			var service = CreateService ();
			
			var accounts = service.GetAccountsAsync ().Result;
			
			var req = service.CreateRequest ("GET", new Uri ("https://alpha-api.app.net/stream/0/posts/stream"), accounts[0]);

			var content = req.GetResponseAsync ().Result.GetResponseText ();
			
			Assert.That (content.Contains ("canonical_url"));
		}
	}
}