using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace CocoVideoProject
{
	public class GameLayer : CCLayerColor
	{

		// Define a label variable
		CCSprite trooperSprite;

		public GameLayer() : base(CCColor4B.Transparent)
		{

			// create and initialize a Label

			trooperSprite = new CCSprite("stormTrooper");
			// add the label as a child to this Layer
			AddChild(trooperSprite, 22);

		}

		protected override void AddedToScene()
		{
			base.AddedToScene();

			// Use the bounds to layout the positioning of our drawable assets
			var bounds = VisibleBoundsWorldspace;
			trooperSprite.Position = bounds.Center;
			trooperSprite.ZOrder = 100;

			var touchListener = new CCEventListenerTouchAllAtOnce();
			touchListener.OnTouchesEnded = OnTouchEnded;
			AddEventListener(touchListener, this);

			Schedule(RunBallLogic);
		}

		private void OnTouchEnded(List<CCTouch> arg1, CCEvent arg2)
		{
			trooperSprite.Position = VisibleBoundsWorldspace.Center;
			if (ballyVelocity > 0)
			{
				ballyVelocity *= -1;
			}
		}

		float ballxVelocity;
		float ballyVelocity;
		const float gravity = 140;

		void RunBallLogic(float frameTimeInSeconds)
		{
			float screenRight = VisibleBoundsWorldspace.MaxX;
			float screenLeft = VisibleBoundsWorldspace.MinX;
			float screenBottom = VisibleBoundsWorldspace.MinY;
			float screenTop = VisibleBoundsWorldspace.MaxX;

			ballyVelocity += frameTimeInSeconds * -gravity;

			//Make sure doesn't go past bottom
			const float maXVelocity = 600;
			bool isMovingDownward = ballyVelocity < 0;
			bool ballHitBottom = trooperSprite.BoundingBoxTransformedToParent.MinY <= 0;
			//TODO: don't hardcord the height of frame 1080
			bool ballHitTop = trooperSprite.BoundingBoxTransformedToParent.MaxY >= 1080;
			if (isMovingDownward && ballHitBottom)
			{
				//Reverse direction of ball in y direction
				ballyVelocity *= -1;
				//Random x velocity
				ballxVelocity = CCRandom.GetRandomFloat(maXVelocity * -1, maXVelocity);
			}
			else if (!isMovingDownward && ballHitTop)
			{
				//Reverse direction of ball in y direction
				ballyVelocity *= -1;
				ballxVelocity = CCRandom.GetRandomFloat(maXVelocity * -1, maXVelocity);
			}
			trooperSprite.PositionX += ballxVelocity * frameTimeInSeconds;
			trooperSprite.PositionY += ballyVelocity * frameTimeInSeconds;

			float ballRight = trooperSprite.BoundingBoxTransformedToParent.MaxX;
			float ballLeft = trooperSprite.BoundingBoxTransformedToParent.MinX;

			//Make sure doesn't fly off sides of screen
			bool shouldRelfectXVelocity = (ballRight > screenRight && ballxVelocity > 0) || (ballLeft < screenLeft && ballxVelocity < 0);
			if (shouldRelfectXVelocity)
			{
				ballxVelocity *= -1;
			}

		}
	}
}

