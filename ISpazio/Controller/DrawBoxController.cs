﻿using Foundation;
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
        private Box box;
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
            box = boxDAO.getBox(IDBox);
        }

        private void updateData()
        {
            items = itemDAO.getAllItemInBox(IDBox);
        }

        private void initLight()
        {
            // omnidirectional light
            var light = SCNLight.Create();
            var lightNode = SCNNode.Create();
            light.LightType = SCNLightType.Omni;
            light.Color = UIColor.Blue;
            lightNode.Light = light;
            lightNode.Position = new SCNVector3(-40, 40, 60);
            scene.RootNode.AddChildNode(lightNode);

            // ambient light
            var ambientLight = SCNLight.Create();
            var ambientLightNode = SCNNode.Create();
            ambientLight.LightType = SCNLightType.Ambient;
            ambientLight.Color = UIColor.Purple;
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
            var w = (nfloat)Math.Round(box.Width, 0, MidpointRounding.AwayFromZero);
            var h = (nfloat)Math.Round(box.Height, 0, MidpointRounding.AwayFromZero);
            var d = (nfloat)Math.Round(box.Depth, 0, MidpointRounding.AwayFromZero);

            var boxGeometry = SCNBox.Create(w + 0.1f, h + 0.1f, d + 0.1f, 0);
            boxNode = SCNNode.FromGeometry(boxGeometry);

            boxGeometry.FirstMaterial.Diffuse.Contents = UIColor.LightGray;
            boxGeometry.FirstMaterial.Transparency = 0.5f;
            boxNode.Position = new SCNVector3(0, 0, 0);

            scene.RootNode.AddChildNode(boxNode);
        }

        private void drawItem()
        {
            foreach (var i in items)
            {
                var w = (nfloat)i.PackDimX;
                var h = (nfloat)i.PackDimY;
                var d = (nfloat)i.PackDimZ;

                var item = SCNBox.Create(w, h, d, 0);
                var itemNode = SCNNode.FromGeometry(item);
                item.FirstMaterial.Diffuse.Contents = randomColor();
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
            updateData();
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
                if (v.Key.Id.Equals(item.Id))
                    v.Value.Geometry.FirstMaterial.Transparency = 0.8f;
                else
                    v.Value.Geometry.FirstMaterial.Transparency = 1.0f;
        }

        private SCNVector3 calculateRelativePosition(Item i)
        {
            var offSetItemX = (i.PackDimX / 2);
            var offSetItemY = (i.PackDimY / 2);
            var offSetItemZ = (i.PackDimZ / 2);

            var wBox = Math.Round(box.Width, 0, MidpointRounding.AwayFromZero);
            var hBox = Math.Round(box.Height, 0, MidpointRounding.AwayFromZero);
            var dBox = Math.Round(box.Depth, 0, MidpointRounding.AwayFromZero);

            var offSetBoxX = (decimal)(-1 * dBox / 2);
            var offSetBoxY = (decimal)(-1 * hBox / 2);
            var offSetBoxZ = (decimal)(-1 * wBox / 2);

            var offSetX = (float)(offSetBoxX + offSetItemX + i.CoordX);
            var offSetY = (float)(offSetBoxY + offSetItemY + i.CoordY);
            var offSetZ = (float)(offSetBoxZ + offSetItemZ + i.CoordZ);

            return new SCNVector3(offSetX, offSetY, offSetZ);
        }

        private UIColor randomColor()
        {
            Random random = new Random();
            int r = random.Next(0, 256);
            int g = random.Next(0, 256);
            int b = random.Next(0, 256);


            return UIColor.FromRGB(r, g, b);
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

                default:
                    break;
            }
        }

    }
}