using System;
using ARKit;
using UIKit;
namespace NewTestArKit.Delegate
{
    public class MyARSessionDelegate : ARSessionDelegate
    {
        private CameraViewController cameraView { get; set; }

        public MyARSessionDelegate(CameraViewController controller)
        {
            this.cameraView = controller;
        }

        public override void CameraDidChangeTrackingState(ARSession session, ARCamera camera)
        {
            var state = camera.TrackingState;
            var messageLabel = cameraView.MessageLabel;

            switch (state)
            {
                case ARTrackingState.NotAvailable:
           
                    messageLabel.Text = "Non è possibile rilevare l'ambiente circostante";
                    break;
                case ARTrackingState.Normal:
                    messageLabel.Text = "Inquadra lentamente l'ambiente circostante";
                    break;
                case ARTrackingState.Limited:
                    var reason = camera.TrackingStateReason;
                 
                    messageLabel.Text = "Visuale limitata";
                    switch (reason)
                    {
                        case ARTrackingStateReason.ExcessiveMotion:
                           
                            messageLabel.Text = "Eccessivo Movimento";
                            break;
                        case ARTrackingStateReason.Initializing:
                            
                            messageLabel.Text = "Inizializzazione";
                            break;
                        case ARTrackingStateReason.InsufficientFeatures:
                            
                            messageLabel.Text = "Non vi è abbastanza luce o si sta puntado su una superfice riflettente";
                            break;
                        case ARTrackingStateReason.None:
                            Console.WriteLine("None");
                            break;
                        case ARTrackingStateReason.Relocalizing:
                            
                            messageLabel.Text = "Relocalizing";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
