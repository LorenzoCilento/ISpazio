using System;
using SceneKit;
using ARKit;
using UIKit;
namespace NewTestArKit.Utility
{
    public class Plane : SCNNode
    {
        public ARPlaneAnchor PlaneAnchor { get; set; }
        public SCNNode PlaneNode { get; set; }
        public SCNPlane PlaneGeometry { get; set; }


        public Plane(ARPlaneAnchor anchor)
        {
            this.PlaneAnchor = anchor;

            var grid = UIImage.FromBundle("plane_grid2.png");

            this.PlaneGeometry = SCNPlane.Create(PlaneAnchor.Extent.X, PlaneAnchor.Extent.Z);
            this.PlaneNode = SCNNode.FromGeometry(PlaneGeometry);

            PlaneGeometry.FirstMaterial.Transparency = 0.5f;
            PlaneGeometry.FirstMaterial.Diffuse.Contents = grid;

            PlaneNode.Transform = SCNMatrix4.CreateRotationX((float)-Math.PI / 2);
            PlaneNode.Position = new SCNVector3(PlaneAnchor.Center.X, 0, PlaneAnchor.Center.Z);

            AddChildNode(PlaneNode);
        }

        public void update(ARPlaneAnchor anchor)
        {
            this.PlaneAnchor = anchor;

            this.PlaneGeometry.Width = PlaneAnchor.Extent.X;
            this.PlaneGeometry.Height = PlaneAnchor.Extent.Z;

            this.PlaneNode.Position = new SCNVector3(PlaneAnchor.Center.X, 0, PlaneAnchor.Center.Z);
        }
    }
}
