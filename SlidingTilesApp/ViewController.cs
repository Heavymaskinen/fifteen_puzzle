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
            puzzle = new GraphicPuzzle(boardWidth, boardHeight, boardView);
            puzzle.Shuffle();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            if (touches.Count == 1)
            {
                if (touches.AnyObject is UITouch)
                {
                    var myTouch = (UITouch)touches.AnyObject;
                    var touchedView = myTouch.View;

                    if (touchedView is GameTile tile)
                    {
                        HandleTileTouch(tile);
                    }
                }
            }
        }

        private void HandleTileTouch(GameTile tile)
        {
            puzzle.Move(new System.Drawing.Point(tile.Col + 1, tile.Row + 1));
            if (puzzle.IsCompleted)
            {
                ShowPopup();
            }
        }

        private void ShowPopup()
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

                this.ShowViewController(alert, this);
            }));
        }

        partial void ShuffleButton_TouchUpInside(UIButton sender)
        {
            puzzle.Shuffle();
        }
    }
}