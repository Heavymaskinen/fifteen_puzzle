using CoreGraphics;
using Foundation;
using SlidingTilesApp.Logic;
using System;
using System.Diagnostics;
using UIKit;

namespace SlidingTilesApp
{
    public partial class ViewController : UIViewController
    {
        private float boardWidth;
        private float boardHeight;
        private GraphicPuzzle puzzle;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            boardWidth = (float)boardView.Frame.Size.Width;
            boardHeight = (float)boardView.Frame.Size.Height;
            DidReceiveMemoryWarning();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            puzzle = new GraphicPuzzle(boardWidth, boardHeight, 2, boardView);
            puzzle.Shuffle();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            if (touches.Count == 1)
            {
                try
                {
                    // Get the touch that was activated within the view
                    var myTouch = (UITouch)touches.AnyObject;
                    var touchedView = myTouch.View;

                    if (touchedView is GameTile)
                    {
                        GameTile tile = ((GameTile)touchedView);
                        puzzle.Move(new System.Drawing.Point(tile.Col+1, tile.Row+1));
                        if (puzzle.IsCompleted)
                        {
                            UIApplication.SharedApplication.InvokeOnMainThread(new Action(() =>
                            {
                                var alert = UIAlertController.Create("HURRAH!", "You completed the game!",
                                                                     UIAlertControllerStyle.Alert);
                                alert.AddAction(UIAlertAction.Create("OK",
                                                                     UIAlertActionStyle.Default, a =>
                                                                     {
                                                                         puzzle.Shuffle();
                                                                     }));

                                // Display the UIAlertController to the current view
                                this.ShowViewController(alert, this);
                            }));
                        }
                    }
                    else
                    {
                        Debug.WriteLine("touchedView is " + touchedView.GetType());
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("touchedView is not a UIImageView: " + e.Message);
                }
            }
        }

        partial void ResetButton_TouchUpInside(UIButton sender)
        {
            foreach (var v in boardView.Subviews)
            {
                v.RemoveFromSuperview();
            }

            puzzle = new GraphicPuzzle(boardWidth, boardHeight, 2, boardView);
            puzzle.Draw();
        }

        partial void ShuffleButton_TouchUpInside(UIButton sender)
        {
            puzzle.Shuffle();
        }
    }
}