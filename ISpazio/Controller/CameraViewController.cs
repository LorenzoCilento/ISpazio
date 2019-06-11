using System;

using UIKit;
using ARKit;
using Foundation;
using System.Linq;
using SceneKit;
using Vision;
using OpenTK;
using System.Collections.Generic;
using CoreGraphics;
using NewTestArKit.Model;
using NewTestArKit.Delegate;
using NewTestArKit.Utility;

namespace NewTestArKit
{
    public partial class CameraViewController : UIViewController
    {
        private List<SCNNode> sphereArray = new List<SCNNode>();
        private List<SCNNode> sphereRectArray = new List<SCNNode>();
        private List<SCNNode> textArray = new List<SCNNode>();
        private List<double> distanceList = new List<double>();
        private Dictionary<ARPlaneAnchor, Plane> planes = new Dictionary<ARPlaneAnchor, Plane>();

        public Dictionary<ARPlaneAnchor,Plane> Planes { get => planes; }

        private int nodeCalculated = 0;
        private double distance = 0;

        private MyObject tmpObj = new MyObject();

        private List<MyObject> myObjects = new List<MyObject>();

        private VNObservation[] resultObservation;
        private VNDetectRectanglesRequest rectanglesRequest;

        private bool isActiveFindRectangle = false;
        public bool IsActiveFindRectangle
        {
            get { return isActiveFindRectangle; }
            set { isActiveFindRectangle = value; }
        }

        private bool isActiveMeasyreAccuracy = false;
        public bool IsActiveMeasureAccuracy
        {
            get => isActiveMeasyreAccuracy;
            set { isActiveMeasyreAccuracy = value; }
        }

        public int AccuracyValue { get; set; }

        private int countMeasure = 0;

        private CGPoint ScreenCenter { get; set; }

        public UIView SceneView { get => sceneView; }

        public UIView MessageView { get { return messageView; } }

        public UILabel MessageLabel { get => messageLabel; }

        protected CameraViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            setupSceneView();
            setupConfigurationAndStartSession();
            setupRectangle();
            this.NavigationController.NavigationBarHidden = true;
            sceneView.Session.Delegate = new MyARSessionDelegate(this);
        }

        public override void ViewDidAppear(bool animated)
        {
            reset();
            setupConfigurationAndStartSession();
            //startPresentationPanel();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            sceneView.Session.Pause();
        }

        private void startPresentationPanel()
        {
            PresentationPopoverViewController vc = new PresentationPopoverViewController
            {
                ModalPresentationStyle = UIModalPresentationStyle.FormSheet,
                ModalTransitionStyle = UIModalTransitionStyle.CoverVertical,
            };

            vc.View.BackgroundColor = UIColor.Red;

            PresentViewController(vc, true, null);
        }

        private void setupRectangle()
        {
            rectanglesRequest = new VNDetectRectanglesRequest(RectagleDetect);
            rectanglesRequest.MaximumObservations = 1;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            var touch = (UITouch)touches.FirstOrDefault();

            if (isActiveFindRectangle)
            {
                findRectangle(sceneView.Session.CurrentFrame);
            }

            if (touch != null)
            {
                measureDistance();
            }
        }

        private void measureDistance()
        {
            var midX = sceneView.Frame.GetMidX();
            var midY = sceneView.Frame.GetMidY();

            var touchLocation = new CGPoint(midX, midY);

            var result = sceneView.HitTest(touchLocation, ARHitTestResultType.ExistingPlaneUsingExtent);

            if (result.FirstOrDefault() != null)
            {
                var res = result.First();
                addSphereNode(worldTransform(res.WorldTransform), UIColor.Red, sphereArray);

                if (sphereArray.Count() >= 2)
                {
                    distance += distancePointToPoint();
                    addText(distance.ToString());
                    eraseButton.Hidden = false;
                    okButton.Hidden = false;
                    addMessage("Se la misura è corretta clicca sulla spunta per confermare o su cancella per ripetere", 5);
                }
            }
        }

        partial void OkButton_TouchUpInside(UIButton sender)
        {
            if (isActiveMeasyreAccuracy)
            {
                distanceList.Add(distance);
                countMeasure++;
                reset();
                if (countMeasure == AccuracyValue)
                {
                    addDistance(mediaMeasure(distanceList));
                    distanceList.Clear();
                    countMeasure = 0;
                    reset();
                }
            }
            else
            {
                addDistance(distance);
                addMessage("Seleziona in alto la prossima dimensione", 3);
            }

        }

        private double distancePointToPoint()
        {
            var startPoint = sphereArray[nodeCalculated].Position;
            var endPoint = sphereArray[nodeCalculated + 1].Position;
            nodeCalculated += 1;

            return getDistance(startPoint, endPoint);
        }

        private void addDistance(double dst)
        {
            switch (dimensionChoise.SelectedSegment)
            {
                case 0:
                    tmpObj.Height = dst;
                    break;
                case 1:
                    tmpObj.Width = dst;
                    break;
                case 2:
                    tmpObj.Depth = dst;
                    break;
                default:
                    break;
            }


        }

        partial void EraseButton_TouchUpInside(UIButton sender)
        {
            reset();
        }

        public void findRectangle(ARFrame currentFrame)
        {
            var handler = new VNImageRequestHandler(currentFrame.CapturedImage, new NSDictionary());
            NSError error;

            VNRequest[] request = { rectanglesRequest };
            handler.Perform(request, out error);

            if (error != null)
                Console.Error.WriteLine(error);

        }

        public void RectagleDetect(VNRequest request, NSError err)
        {
            if (err != null)
            {
                Console.WriteLine(err);
                return;
            }

            resultObservation = request.GetResults<VNRectangleObservation>();

            foreach (var rect in resultObservation)
            {
                drawRect((VNRectangleObservation)rect);
            }
        }

        public CGPoint convertFromCamera(CGPoint point)
        {
            return new CGPoint(point.Y * sceneView.Frame.Width, point.X * sceneView.Frame.Height);
        }

        public void drawRect(VNRectangleObservation rect)
        {
            var widthRect = (nuint)sceneView.Frame.Width;
            var heightRect = (nuint)sceneView.Frame.Height;

            var bottomLeft = VNUtils.GetImagePoint(new CGPoint(rect.BottomLeft.Y, rect.BottomLeft.X), widthRect, heightRect);
            var bottomRight = VNUtils.GetImagePoint(new CGPoint(rect.BottomRight.Y, rect.BottomRight.X), widthRect, heightRect);
            var topLeft = VNUtils.GetImagePoint(new CGPoint(rect.TopLeft.Y, rect.TopLeft.X), widthRect, heightRect);
            var topRight = VNUtils.GetImagePoint(new CGPoint(rect.TopRight.Y, rect.TopRight.X), widthRect, heightRect);

            var bl = sceneView.HitTest(bottomLeft, ARHitTestResultType.ExistingPlaneUsingExtent);
            var br = sceneView.HitTest(bottomRight, ARHitTestResultType.ExistingPlaneUsingExtent);
            var tl = sceneView.HitTest(topLeft, ARHitTestResultType.ExistingPlaneUsingExtent);
            var tr = sceneView.HitTest(topRight, ARHitTestResultType.ExistingPlaneUsingExtent);

            var color = UIColor.Green;
            if (bl.FirstOrDefault() != null)
            {
                addSphereNode(worldTransform(bl.First().WorldTransform), color, sphereRectArray);
                addSphereNode(worldTransform(br.First().WorldTransform), color, sphereRectArray);
                addSphereNode(worldTransform(tl.First().WorldTransform), color, sphereRectArray);
                addSphereNode(worldTransform(tr.First().WorldTransform), color, sphereRectArray);
            }

        }

        public double getDistance(SCNVector3 startPoint, SCNVector3 endPoint)
        {
            var a = startPoint.X - endPoint.X;
            var b = startPoint.Y - endPoint.Y;
            var c = startPoint.Z - endPoint.Z;

            return Math.Round((Math.Abs(Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2) + Math.Pow(c, 2))) * 100), 2);
        }


        public SCNVector3 worldTransform(NMatrix4 matrix4)
        {
            var m = matrix4.Column3;
            return new SCNVector3(m.X, m.Y, m.Z);
        }


        public void clearList(List<SCNNode> l)
        {
            foreach (var s in l)
            {
                s.RemoveFromParentNode();
            }
        }


        public void reset()
        {
            clearList(sphereArray);

            clearList(sphereRectArray);

            clearList(textArray);

            distance = 0;
            nodeCalculated = 0;
            sphereArray.Clear();
            sphereRectArray.Clear();
            eraseButton.Hidden = true;
            okButton.Hidden = true;
        }

        partial void resetButtonPressed(UIButton sender)
        {
            reset();
            setupConfigurationAndStartSession();
        }

        public void addSphereNode(SCNVector3 position, UIColor color, List<SCNNode> array)
        {
            var sphere = SCNSphere.Create(0.003f);
            sphere.FirstMaterial.Diffuse.Contents = color;

            var sphereNode = SCNNode.FromGeometry(sphere);
            sphereNode.Position = position;

            array.Add(sphereNode);

            sceneView.Scene.RootNode.AddChildNode(sphereNode);
        }

        public void addText(String text)
        {
            var position = sphereArray[nodeCalculated].Position;

            var textGeom = SCNText.Create(text, 0.5f);
            textGeom.FirstMaterial.Diffuse.Contents = UIColor.Red;

            var textNode = new SCNNode();
            textNode = SCNNode.FromGeometry(textGeom);
            textNode.Scale = new SCNVector3(0.003f, 0.003f, 0.003f);
            textNode.Position = new SCNVector3(position.X, position.Y + 0.01f, position.Z);

            textArray.Add(textNode);
            sceneView.Scene.RootNode.AddChildNode(textNode);
        }


        public void setupSceneView()
        {
            sceneView.Delegate = new MySCNViewDelegate(this);
            sceneView.DebugOptions = ARSCNDebugOptions.ShowFeaturePoints;
            sceneView.AutoenablesDefaultLighting = true;
        }

        public void setupConfigurationAndStartSession()
        {
            var configuration = new ARWorldTrackingConfiguration()
            {
                PlaneDetection = ARPlaneDetection.Horizontal,
                LightEstimationEnabled = true,
                //WorldAlignment = ARWorldAlignment.Gravity,
                AutoFocusEnabled = true,
                EnvironmentTexturing = AREnvironmentTexturing.Automatic,
            };

            initMessageView();

            sceneView.Session.Run(configuration, ARSessionRunOptions.RemoveExistingAnchors | ARSessionRunOptions.ResetTracking);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            var identifier = segue.Identifier;
            var destination = segue.DestinationViewController;

            switch (identifier)
            {
                case "showDetailViewController":
                    var detail = destination as DetailViewController;
                    detail.Obj = tmpObj;
                    break;
                case "showOptionViewController":
                    var option = destination as OptionViewController;
                    option.IsActiveFindRectangle = IsActiveFindRectangle;
                    option.IsActiveMeasureAccuracy = IsActiveMeasureAccuracy;
                    option.AccuracyValue = AccuracyValue;
                    break;
                default:
                    break;
            }

        }

        partial void dimensionChoiseValueChangedPressed(UISegmentedControl sender)
        {
            //reset();
        }

        [Action("UnwindToCameraViewController:")]
        public void UnwindToCameraViewController(UIStoryboardSegue segue)
        {
            var identifier = segue.Identifier;
            var source = segue.SourceViewController;

            switch (identifier)
            {
                case "unwindFromDetailViewControllerToCameraViewController":
                    var detail = source as DetailViewController;
                    tmpObj = detail.Obj;
                    break;
                case "unwindFromOptionViewControllerToCameraViewController":
                    var option = source as OptionViewController;
                    IsActiveFindRectangle = option.IsActiveFindRectangle;
                    IsActiveMeasureAccuracy = option.IsActiveMeasureAccuracy;
                    AccuracyValue = option.AccuracyValue;
                    break;
                default:
                    break;
            }
        }

        private double mediaMeasure(List<double> list)
        {
            var count = 0;
            double value = 0;

            foreach (var a in list)
            {
                count++;
                value += a;
            }
            return Math.Round(value / count,2);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void initMessageView()
        {
            messageView.Hidden = false;
            UIView.Animate(
                       duration: 1,
                       delay: 0,
                       options: UIViewAnimationOptions.CurveEaseIn,
                       animation: () =>
                       {
                           MessageView.Alpha = 1.0f;
                       },
                       completion: () =>
                       {

                       }
                   );
        }

        public void endMessageView(int timeDelay)
        {
            UIView.Animate(
                       duration: 2,
                       delay: timeDelay,
                       options: UIViewAnimationOptions.CurveEaseOut,
                       animation: () =>
                       {
                           MessageView.Alpha = 0.0f;
                       },
                       completion: () =>
                       {
                           MessageView.Hidden = true;
                       }
                   );
        }

        private void addMessage(string message, int timeDelay)
        {
            initMessageView();
            messageLabel.Text = message;
            endMessageView(timeDelay);
        }
    }
}
