using Foundation;
using System;
using UIKit;
using SceneKit;
using NewTestArKit.Connection;
using System.Collections.Generic;
using NewTestArKit.Model;

namespace NewTestArKit
{
    public partial class DrawBoxController : UIViewController
    {
        private ItemDAO itemDAO;
        private BoxDAO boxDAO;
        private List<Item> items;
        private SCNScene scene;
        private SCNNode boxNode;
        private Box Box;
        private Dictionary<Item, SCNNode> itemsNodeMap;

        public int IDBox { get; set; }

        public DrawBoxController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            initScene();
            initDAO();
            //initLight();
            initCamera();
            loadData();

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            itemsNodeMap.Clear();
            updateInfoBox();
            drawBox();
            draw();
        }

        private void initScene()
        {
            scene = SCNScene.Create();
            sceneView.Scene = scene;
            sceneView.AutoenablesDefaultLighting = true;
            itemsNodeMap = new Dictionary<Item, SCNNode>();
        }

        private void initDAO()
        {
            itemDAO = new ItemDAO();
            boxDAO = new BoxDAO();
        }

        private void loadData()
        {
            items = itemDAO.getAllItemInBox(IDBox);
            Box = boxDAO.getBox(IDBox);
        }

        private void initLight()
        {
            // omnidirectional light
            var light = SCNLight.Create();
            var lightNode = SCNNode.Create();
            light.LightType = SCNLightType.Omni;
            light.Color = UIColor.White;
            lightNode.Light = light;
            lightNode.Position = new SCNVector3(-40, 40, 60);
            scene.RootNode.AddChildNode(lightNode);

            // ambient light
            var ambientLight = SCNLight.Create();
            var ambientLightNode = SCNNode.Create();
            ambientLight.LightType = SCNLightType.Ambient;
            ambientLight.Color = UIColor.White;
            ambientLightNode.Light = ambientLight;
            scene.RootNode.AddChildNode(ambientLightNode);
        }

        private void initCamera()
        {
            // camera
            var camera = new SCNCamera
            {
                XFov = 80,
                YFov = 80
            };
            var cameraNode = new SCNNode
            {
                Camera = camera,
                Position = new SCNVector3(0, 0, 30)
            };
            scene.RootNode.AddChildNode(cameraNode);
            sceneView.AllowsCameraControl = true;
        }

        private void drawBox()
        {
            var border = 0.05f;
            //var w = (nfloat)(Math.Round(Box.Width, 0, MidpointRounding.AwayFromZero) + border);
            //var h = (nfloat)(Math.Round(Box.Height, 0, MidpointRounding.AwayFromZero) + border);
            //var d = (nfloat)(Math.Round(Box.Depth, 0, MidpointRounding.AwayFromZero) + border);

            var w = (nfloat)Box.Width + border;
            var h = (nfloat)Box.Height + border;
            var d = (nfloat)Box.Depth + border;

            var boxGeometry = SCNBox.Create(d, h, w, 0);
            boxNode = SCNNode.FromGeometry(boxGeometry);

            boxGeometry.FirstMaterial.Diffuse.Contents = UIColor.Black;
            boxGeometry.FirstMaterial.FillMode = SCNFillMode.Lines;
            boxGeometry.FirstMaterial.DoubleSided = true;
            boxNode.Position = new SCNVector3(0, 0, 0);

            scene.RootNode.AddChildNode(boxNode);
        }

        private void drawItem()
        {
            bool RColor = true;
            foreach (var i in items)
            {
                var w = (nfloat)i.PackDimX;
                var h = (nfloat)i.PackDimY;
                var d = (nfloat)i.PackDimZ;

                var item = SCNBox.Create(w, h, d, 0);
                var itemNode = SCNNode.FromGeometry(item);
                if (RColor)
                {
                    item.FirstMaterial.Diffuse.Contents = randomColor(RColor);
                    RColor = false;
                }
                else
                {
                    item.FirstMaterial.Diffuse.Contents = randomColor(RColor);
                    RColor = true;
                }
                itemNode.Position = calculateRelativePosition(i);

                itemsNodeMap.Add(i, itemNode);
                scene.RootNode.AddChildNode(itemNode);
            }
        }

        private void draw()
        {
            drawItem();
        }

        public void reDraw()
        {
            loadData();
            draw();
        }

        private void removeListFromParentNodeAndClear(List<SCNNode> list)
        {
            foreach (var v in list)
                v.RemoveFromParentNode();
            list.Clear();
        }

        public void removeNodeFromScene(Item item)
        {
            foreach (KeyValuePair<Item, SCNNode> v in itemsNodeMap)
                if (v.Key.Id.Equals(item.Id))
                    v.Value.RemoveFromParentNode();
        }

        public void highlightedBox(Item item)
        {
            foreach (KeyValuePair<Item, SCNNode> v in itemsNodeMap)
                if (!v.Key.Id.Equals(item.Id))
                    v.Value.Geometry.FirstMaterial.Transparency = 0.6f;
                else
                    v.Value.Geometry.FirstMaterial.Transparency = 1.0f;
        }

        private SCNVector3 calculateRelativePosition(Item i)
        {
            var offSetItemX = (i.PackDimX / 2);
            var offSetItemY = (i.PackDimY / 2);
            var offSetItemZ = (i.PackDimZ / 2);

            //var wBox = Math.Round(Box.Width, 0, MidpointRounding.AwayFromZero);
            //var hBox = Math.Round(Box.Height, 0, MidpointRounding.AwayFromZero);
            //var dBox = Math.Round(Box.Depth, 0, MidpointRounding.AwayFromZero);

            var wBox = Box.Width;
            var hBox = Box.Height;
            var dBox = Box.Depth;

            var offSetBoxX = (decimal)(-1 * dBox / 2);
            var offSetBoxY = (decimal)(-1 * hBox / 2);
            var offSetBoxZ = (decimal)(-1 * wBox / 2);

            var offSetX = (float)(offSetBoxX + offSetItemX + i.CoordX);
            var offSetY = (float)(offSetBoxY + offSetItemY + i.CoordY);
            var offSetZ = (float)(offSetBoxZ + offSetItemZ + i.CoordZ);

            return new SCNVector3(offSetX, offSetY, offSetZ);
        }

        private UIColor randomColor(bool chose)
        {
            if (chose)
                return UIColor.FromRGB(234, 181, 67);
            else
                return UIColor.FromRGB(249, 127, 81);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            var identifier = segue.Identifier;
            var destination = segue.DestinationViewController;


            switch (identifier)
            {
                case "showItemController":
                    var itemController = destination as ShowItemController;
                    itemController.IDBox = IDBox;
                    itemController.DrawBoxController = this;
                    break;
                case "showDetailBoxController":
                    var detail = destination as DetailBoxController;
                    detail.IDBox = IDBox;
                    break;

                default:
                    break;
            }
        }

        private void updateInfoBox()
        {
            nameLabel.Text = Box.Name;
            heightLabel.Text = "H: " + Box.Height.ToString() + " cm";
            widthLabel.Text = "W: " + Box.Width.ToString() + " cm";
            depthLabel.Text = "D: " + Box.Depth.ToString() + " cm";
            updateFreeSpace();
        }

        private void updateFreeSpace()
        {
            var value = (float)(Box.RemainVolume / Box.Volume);
            var percent = Math.Round(value * 100, 1);

            freeSpaceLabel.Text = percent + " %";
            if (value >= 1.0f)
                freeSpaceProgressBar.Progress = 0f;
            else
                freeSpaceProgressBar.Progress = 1 - value;
        }

        public void removeItem(Item item)
        {
            removeNodeFromScene(item);
            loadData();
            updateFreeSpace();
        }

    }
}