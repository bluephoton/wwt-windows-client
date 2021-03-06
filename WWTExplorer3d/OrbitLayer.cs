﻿using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

namespace TerraViewer
{
    public class OrbitLayer : Layer
    {
        List<ReferenceFrame> frames = new List<ReferenceFrame>();

        public List<ReferenceFrame> Frames
        {
            get { return frames; }
            set { frames = value; }
        }

        OrbitLayerUI primaryUI;
        public override LayerUI GetPrimaryUI()
        {
            if (primaryUI == null)
            {
                primaryUI = new OrbitLayerUI(this);
            }

            return primaryUI;
        }

        public override void CleanUp()
        {
            foreach (var frame in frames)
            {
                if (frame.Orbit != null)
                {
                    frame.Orbit.CleanUp();
                    frame.Orbit = null;
                }
            }
        }

        public override void WriteLayerProperties(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteAttributeString("PointOpacity", PointOpacity.ToString());
            xmlWriter.WriteAttributeString("PointColor", SavedColor.Save(pointColor));

        }

        private double pointOpacity = 1;

        [LayerProperty]
        public double PointOpacity
        {
            get { return pointOpacity; }
            set
            {
                if (pointOpacity != value)
                {
                    version++;

                    pointOpacity = value;

                }
            }
        }

        Color pointColor = Color.Yellow;

        [LayerProperty]
        public Color PointColor
        {
            get { return pointColor; }
            set
            {
                if (pointColor != value)
                {
                    version++;
                    pointColor = value;

                }
            }
        }

        public override double[] GetParams()
        {
            var paramList = new double[6];
            paramList[0] = pointOpacity;
            paramList[1] = Color.R / 255;
            paramList[2] = Color.G / 255;
            paramList[3] = Color.B / 255;
            paramList[4] = Color.A / 255;
            paramList[5] = Opacity;


            return paramList;
        }

        public override string[] GetParamNames()
        {
            return new[] { "PointOpacity", "Color.Red", "Color.Green", "Color.Blue", "Color.Alpha", "Opacity" };
        }

        public override BaseTweenType[] GetParamTypes()
        {
            return new[] { BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Linear };
        }

        public override void SetParams(double[] paramList)
        {
            if (paramList.Length == 6)
            {
                pointOpacity = paramList[0];
                Opacity = (float)paramList[5];
                var color = Color.FromArgb((int)(paramList[4] * 255), (int)(paramList[1] * 255), (int)(paramList[2] * 255), (int)(paramList[3] * 255));
                Color = color;
                
            }
        }


        public override void InitializeFromXml(XmlNode node)
        {
            PointOpacity = double.Parse(node.Attributes["PointOpacity"].Value);
            PointColor = SavedColor.Load(node.Attributes["PointColor"].Value);
            
        }


        public override bool Draw(RenderContext11 renderContext, float opacity, bool flat)
        {
            var matSaved = renderContext.World;
            renderContext.World = renderContext.WorldBaseNonRotating;

            foreach (var frame in frames)
            {
                if (frame.ShowOrbitPath)
                {
                    if (frame.Orbit == null)
                    {
                        frame.Orbit = new Orbit(frame.Elements, 360, Color, 1, (float)renderContext.NominalRadius);
                    }
                    frame.Orbit.Draw3D(renderContext, opacity * Opacity, new Vector3d(0, 0, 0));
                }
            }
            renderContext.World = matSaved;
            return true;
        }

        string filename = "";

        public override void AddFilesToCabinet(FileCabinet fc)
        {
            var fName = filename;

            var copy = true;

            var fileName = fc.TempDirectory + string.Format("{0}\\{1}.txt", fc.PackageID, ID);
            var path = fName.Substring(0, fName.LastIndexOf('\\') + 1);
            var path2 = fileName.Substring(0, fileName.LastIndexOf('\\') + 1);

            if (copy)
            {
                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }
                if (File.Exists(fName) && !File.Exists(fileName))
                {
                    File.Copy(fName, fileName);
                }


            }

            if (File.Exists(fileName))
            {
                fc.AddFile(fileName);
            }

        }

        public override void LoadData(string path)
        {
            filename = path;
            if (File.Exists(filename))
            {
                var data = File.ReadAllLines(path);
                frames.Clear();
                for (var i = 0; i < data.Length; i += 2)
                {
                    var line1 = i;
                    var line2 = i + 1;
                    var frame = new ReferenceFrame();
                    if (data[i].Substring(0, 1) != "1")
                    {
                        line1++;
                        line2++;
                        frame.Name = data[i].Trim();
                        i++;
                    }
                    else if (data[i].Substring(0, 1) == "1")
                    {
                        frame.Name = data[i].Substring(2, 5);
                    }
                    else
                    {
                        i -= 2;
                        continue;
                    }

                    frame.Reference = ReferenceFrames.Custom;
                    frame.Oblateness = 0;
                    frame.ShowOrbitPath = true;
                    frame.ShowAsPoint = true;
                    frame.ReferenceFrameType = ReferenceFrameTypes.Orbital;
                    frame.Scale = 1;
                    frame.SemiMajorAxisUnits = AltUnits.Meters;
                    frame.MeanRadius = 10;
                    frame.Oblateness = 0;
                    frame.FromTLE(data[line1], data[line2], 398600441800000);
                    frames.Add(frame);
                }
            }
        }
    }

    public class OrbitLayerUI : LayerUI
    {
        readonly OrbitLayer layer;
        bool opened = true;

        public OrbitLayerUI(OrbitLayer layer)
        {
            this.layer = layer;
        }
        IUIServicesCallbacks callbacks;

        public override void SetUICallbacks(IUIServicesCallbacks callbacks)
        {
            this.callbacks = callbacks;
        }
        public override bool HasTreeViewNodes
        {
            get
            {
                return true;
            }
        }

        public override List<LayerUITreeNode> GetTreeNodes()
        {
            var nodes = new List<LayerUITreeNode>();
            foreach (var frame in layer.Frames)
            {

                var node = new LayerUITreeNode();
                node.Name = frame.Name;


                node.Tag = frame;
                node.Checked = frame.ShowOrbitPath;
                node.NodeSelected += node_NodeSelected;
                node.NodeChecked += node_NodeChecked;
                nodes.Add(node);
            }
            return nodes;
        }

        void node_NodeChecked(LayerUITreeNode node, bool newState)
        {
            var frame = (ReferenceFrame)node.Tag;

            if (frame != null)
            {
                frame.ShowOrbitPath = newState;
            }
        }



        void node_NodeSelected(LayerUITreeNode node)
        {
            if (callbacks != null)
            {
                var frame = (ReferenceFrame)node.Tag;

                var rowData = new Dictionary<string, string>();

                rowData.Add("Name", frame.Name);
                rowData.Add("SemiMajor Axis", frame.SemiMajorAxis.ToString());
                rowData.Add("SMA Units", frame.SemiMajorAxisUnits.ToString());
                rowData.Add("Inclination", frame.Inclination.ToString());
                rowData.Add("Eccentricity", frame.Eccentricity.ToString());
                rowData.Add("Long of Asc. Node", frame.LongitudeOfAscendingNode.ToString());
                rowData.Add("Argument Of Periapsis", frame.ArgumentOfPeriapsis.ToString());
                rowData.Add("Epoch", frame.Epoch.ToString());
                rowData.Add("Mean Daily Motion", frame.MeanDailyMotion.ToString());
                rowData.Add("Mean Anomoly at Epoch", frame.MeanAnomolyAtEpoch.ToString());
                callbacks.ShowRowData(rowData);
            }
        }

        public override List<LayerUIMenuItem> GetNodeContextMenu(LayerUITreeNode node)
        {
            return base.GetNodeContextMenu(node);
        }
    }
}
