using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using CocosSharp;
using Android.Media;

namespace CocoVideoProject
{
	[Activity(Label = "CocoVideoProject", MainLauncher = true, Icon = "@drawable/icon",ScreenOrientation = ScreenOrientation.Landscape,
		 AlwaysRetainTaskState = true,
		 LaunchMode = LaunchMode.SingleInstance,
		 ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
	public class MainActivity : Activity
	{
		private VideoView videoView;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our game view from the layout resource,
			// and attach the view created event to it
			CCGameView gameView = (CCGameView)FindViewById(Resource.Id.GameView);
			gameView.DrawingCacheBackgroundColor = Android.Graphics.Color.Transparent;
			gameView.SetBackgroundColor(Android.Graphics.Color.Transparent);
			gameView.SetZ(10);
			gameView.ViewCreated += LoadGame;

			PlayVideo();
		}

		void LoadGame(object sender, EventArgs e)
		{
			CCGameView gameView = sender as CCGameView;

			if (gameView != null)
			{
				var contentSearchPaths = new List<string>() { "Fonts", "Sounds" };
				CCSizeI viewSize = gameView.ViewSize;

				int width = viewSize.Width;//1024;
				int height = viewSize.Height;//768;

				// Set world dimensions
				gameView.DesignResolution = new CCSizeI(width, height);

				// Determine whether to use the high or low def versions of our images
				// Make sure the default texel to content size ratio is set correctly
				// Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)
				if (width < viewSize.Width)
				{
					contentSearchPaths.Add("Images/Hd");
					CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
				}
				else
				{
					contentSearchPaths.Add("Images/Ld");
					CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
				}

				gameView.ContentManager.SearchPaths = contentSearchPaths;

				CCScene gameScene = new CCScene(gameView);
				gameScene.AddLayer(new GameLayer());
				gameView.RunWithScene(gameScene);
			}
		}

		  private void PlayVideo()
        {
           videoView = FindViewById<VideoView>(Resource.Id.videoView1);
			  videoView.SetZ(-100);
           videoView.Completion += videoCompleted;
           //'com.orodriguez.anchorman' is the package name this is specific to the application
           //I think you can use something like get application path to get project specific path...
           var videoPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Download/StarWarsEmoji.mp4";
			
           videoView.SetVideoPath(videoPath);
           videoView.RequestFocus();
           videoView.Start();
        }
  
        private void videoCompleted(object sender, EventArgs e)
        {
           if (sender is MediaPlayer)
           {
              var mp = (MediaPlayer)sender;
              mp.SeekTo(1);
              mp.Start();
           }
  
           videoView.BringToFront();
           videoView.Start();
        }

	}
}

