using System;
using ARKit;
using Foundation;
using SceneKit;
using UIKit;
using NewTestArKit.Utility;
using System.Collections.Generic;
using CoreFoundation;

namespace NewTestArKit
{
    public class MySCNViewDelegate : ARSCNViewDelegate
    {
        private CameraViewController viewController;

        public MySCNViewDelegate(CameraViewController controller)
        {
            viewController = controller;
        }

        protected MySCNViewDelegate(NSObjectFlag t) : base(t)
        {
        }

        protected internal MySCNViewDelegate(IntPtr handle) : base(handle)
        {
        }

        public override void DidAddNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            if (anchor is ARPlaneAnchor planeAnchor)
            {
                DispatchQueue.MainQueue.DispatchAsync(() =>
                {
                    addPlane(node, planeAnchor);
                    viewController.MessageLabel.Text = "Inizia a Misurare. Punta col mirino e poi tocca su una qualsiasi parte dello schermo per segnare il punto";
                    endMessageView(6);
                });
            }
        }

        //public SCNNode createPlaneNode(ARPlaneAnchor planeAnchor)
        //{
        //    var plane = SCNPlane.Create(planeAnchor.Extent.X, planeAnchor.Extent.Z);
        //    var planeNode = SCNNode.FromGeometry(plane);

        //    plane.FirstMaterial.Diffuse.Contents = UIColor.Red;
        //    plane.FirstMaterial.Transparency = 0.3f;

        //    planeNode.Position = new SCNVector3(planeAnchor.Center.X, 0, planeAnchor.Center.Z);
        //    planeNode.Transform = SCNMatrix4.CreateRotationX((float)-Math.PI / 2);

        //    return planeNode;
        //}

        public override void Update(ISCNSceneRenderer renderer, double timeInSeconds)
        {

        }

        public override void DidUpdateNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            if (anchor is ARPlaneAnchor planeAnchor)
            {
                DispatchQueue.MainQueue.DispatchAsync(() =>
                {
                    updatePlane(planeAnchor);
                });
            }
        }

        public override void DidRenderScene(ISCNSceneRenderer renderer, SCNScene scene, double timeInSeconds)
        {

        }

        private void addPlane(SCNNode node, ARPlaneAnchor anchor)
        {
            Plane plane = new Plane(anchor);
            viewController.Planes.Add(anchor, plane);

            node.AddChildNode(plane);
        }

        private void updatePlane(ARPlaneAnchor anchor)
        {
            if (viewController.Planes.ContainsKey(anchor))
            {
                viewController.Planes[anchor].update(anchor);
            }
        }

        public void endMessageView(int timeDelay)
        {
            UIView.Animate(
                       duration: 2,
                       delay: timeDelay,
                       options: UIViewAnimationOptions.CurveEaseOut,
                       animation: () =>
                       {
                           viewController.MessageView.Alpha = 0.0f;
                       },
                       completion: () =>
                       {
                           viewController.MessageView.Hidden = true;
                       }
                   );
        }
    }
}
