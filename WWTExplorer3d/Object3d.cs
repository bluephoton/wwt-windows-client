﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Xml;
using SharpDX;
using System.Windows.Forms;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Color = System.Drawing.Color;

namespace TerraViewer
{
    public class Object3dLayer : Layer , IUiController
    {

        Object3dLayerUI primaryUI;
        public override LayerUI GetPrimaryUI()
        {
            if (primaryUI == null)
            {
                primaryUI = new Object3dLayerUI(this);
            }

            return primaryUI;
        }


        public Object3d object3d;

        double heading;
        bool flipV = true;

        [LayerProperty]
        public bool FlipV
        {
            get { return flipV; }
            set
            {
                if (flipV != value)
                {
                    flipV = value;
                    if (object3d != null)
                    {
                        object3d.FlipV = flipV;
                        object3d.Reload();
                    }
                    version++;
                }
            }
        }

        bool flipHandedness;

        [LayerProperty]
        public bool FlipHandedness
        {
            get { return flipHandedness; }
            set
            {
                if (flipHandedness != value)
                {
                    flipHandedness = value;
                    if (object3d != null)
                    {
                        object3d.FlipHandedness = flipHandedness;
                        object3d.Reload();
                    }
                    version++;
                }
            }
        }




        bool smooth = true;

        [LayerProperty]
        public bool Smooth
        {
            get { return smooth; }
            set
            {
                if (smooth != value)
                {
                    smooth = value;
                    if (object3d != null)
                    {
                        object3d.Smooth = smooth;
                        object3d.Reload();
                    }
                    version++;
                }
            }
        }

        bool twoSidedGeometry;

        [LayerProperty]
        public bool TwoSidedGeometry
        {
            get { return twoSidedGeometry; }
            set
            {
                if (twoSidedGeometry != value)
                {
                    twoSidedGeometry = value;
                    version++;
                }
            }
        }

        [LayerProperty]
        public double Heading
        {
            get { return heading; }
            set
            {
                if (heading != value)
                {
                    version++;
                    heading = value;
                }
            }
        }
        double pitch;

        [LayerProperty]
        public double Pitch
        {
            get { return pitch; }
            set
            {
                if (pitch != value)
                {
                    version++;
                    pitch = value;
                }
            }
        }
        double roll;

        [LayerProperty]
        public double Roll
        {
            get { return roll; }
            set
            {
                if (roll != value)
                {
                    version++;
                    roll = value;
                }
            }
        }
        Vector3d scale = new Vector3d(1, 1, 1);

        [LayerProperty]
        public Vector3d Scale
        {
            get { return scale; }
            set
            {
                if (scale != value)
                {
                    version++;
                    scale = value;
                }
            }
        }
        Vector3d translate = new Vector3d(0, 0, 0);

        [LayerProperty]
        public Vector3d Translate
        {
            get { return translate; }
            set
            {
                if (translate != value)
                {
                    version++;
                    translate = value;
                }
            }
        }


        int lightID;

         [LayerProperty]
        public int LightID
        {
            get { return lightID; }
            set { lightID = value; }
        }



        bool dirty;
        public override void CleanUp()
        {
            if (object3d != null)
            {
                object3d.Dispose();
                GC.SuppressFinalize(object3d);
            }
            object3d = null;
            dirty = true;
        }

        public override void ColorChanged()
        {
            if (object3d != null)
            {
                object3d.Color = Color;
                //object3d.Reload();
            }
        }

        public bool ObjType = false;

        public override void WriteLayerProperties(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteAttributeString("FlipV", FlipV.ToString());
            xmlWriter.WriteAttributeString("FlipHandedness", FlipHandedness.ToString());
            xmlWriter.WriteAttributeString("Smooth", Smooth.ToString());
            xmlWriter.WriteAttributeString("TwoSidedGeometry", TwoSidedGeometry.ToString());
            xmlWriter.WriteAttributeString("Heading", Heading.ToString());
            xmlWriter.WriteAttributeString("Pitch", Pitch.ToString());
            xmlWriter.WriteAttributeString("Roll", Roll.ToString());
            xmlWriter.WriteAttributeString("Scale", Scale.ToString());
            xmlWriter.WriteAttributeString("Translate", Translate.ToString());
            xmlWriter.WriteAttributeString("LightID", LightID.ToString());
            xmlWriter.WriteAttributeString("Obj", ObjType.ToString());

        }

        public override double[] GetParams()
        {
            var paramList = new double[14];
            paramList[0] = heading;
            paramList[1] = pitch;
            paramList[2] = roll;
            paramList[3] = scale.X;
            paramList[4] = scale.Y;
            paramList[5] = scale.Z;
            paramList[6] = translate.X;
            paramList[7] = translate.Y;
            paramList[8] = translate.Z;
            paramList[9] = Color.R / 255;
            paramList[10] = Color.G / 255;
            paramList[11] = Color.B / 255;
            paramList[12] = Color.A / 255;
            paramList[13] = Opacity;

            return paramList;
        }

        public override string[] GetParamNames()
        {
            return new[] { "Heading", "Pitch", "Roll", "Scale.X", "Scale.Y", "Scale.Z", "Translate.X", "Translate.Y", "Translate.Z", "Color.Red", "Color.Green", "Color.Blue", "Color.Alpha", "Opacity" };
        }

        public override BaseTweenType[] GetParamTypes()
        {
            return new[] { BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Power, BaseTweenType.Power, BaseTweenType.Power, BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Linear };
        }

        public override void SetParams(double[] paramList)
        {
            if (paramList.Length == 14)
            {
                heading = paramList[0];
                pitch = paramList[1];
                roll = paramList[2];
                scale.X = paramList[3];
                scale.Y = paramList[4];
                scale.Z = paramList[5];
                translate.X = paramList[6];
                translate.Y = paramList[7];
                translate.Z = paramList[8];

                Opacity = (float)paramList[13];
                var color = Color.FromArgb((int)(paramList[12] * 255), (int)(paramList[9] * 255), (int)(paramList[10] * 255), (int)(paramList[11] * 255));
                Color = color;

            }

        }

        public event EventHandler PropertiesChanged;

        public void FireChanged()
        {
            if (PropertiesChanged != null)
            {
                PropertiesChanged.Invoke(this, new EventArgs());
            }
        }

        public override IUiController GetEditUI()
        {
            return this as IUiController;
        }

        public override void InitializeFromXml(XmlNode node)
        {
            FlipV = Boolean.Parse(node.Attributes["FlipV"].Value);

            if (node.Attributes["FlipHandedness"] != null)
            {
                FlipHandedness = Boolean.Parse(node.Attributes["FlipHandedness"].Value);
            }
            else
            {
                FlipHandedness = false;
            }

            if (node.Attributes["Smooth"] != null)
            {
                Smooth = Boolean.Parse(node.Attributes["Smooth"].Value);
            }
            else
            {
                Smooth = true;
            }

            if (node.Attributes["TwoSidedGeometry"] != null)
            {
                TwoSidedGeometry = Boolean.Parse(node.Attributes["TwoSidedGeometry"].Value);
            }
            else
            {
                TwoSidedGeometry = false;
            }

            if (node.Attributes["Obj"] != null)
            {
                ObjType = Boolean.Parse(node.Attributes["Obj"].Value);
            }
            else
            {
                ObjType = false;
            }

            Heading = double.Parse(node.Attributes["Heading"].Value);
            Pitch = double.Parse(node.Attributes["Pitch"].Value);
            Roll = double.Parse(node.Attributes["Roll"].Value);
            Scale = Vector3d.Parse(node.Attributes["Scale"].Value);
            Translate = Vector3d.Parse(node.Attributes["Translate"].Value);

            if (node.Attributes["LightID"] != null)
            {
                LightID = int.Parse(node.Attributes["LightID"].Value);
            }
        }

        static TriangleList TranslateUI;
        static LineList TranslateUILines;

        static TriangleList ScaleUI;

        //public static SimpleLineList11 sketch = null;
        static void InitTranslateUI()
        {
            TranslateUILines = new LineList();
            TranslateUILines.TimeSeries = false;
            TranslateUILines.DepthBuffered = false;
            TranslateUILines.ShowFarSide = true;

            TranslateUI = new TriangleList();
            TranslateUI.DepthBuffered = false;
            TranslateUI.TimeSeries = false;
            TranslateUI.WriteZbuffer = false;

            var twoPi = Math.PI * 2;
            var step = twoPi / 45;
            var rad = .05;

            // X

            for (double a = 0; a < twoPi; a += step)
            {
                var pnt1 = new Vector3d(1 - rad * 4, 0, 0);
                var pnt2 = new Vector3d(1 - rad * 4, Math.Cos(a) * rad, Math.Sin(a) * rad);
                var pnt3 = new Vector3d(1 - rad * 4, Math.Cos(a + step) * rad, Math.Sin(a + step) * rad);
                TranslateUI.AddTriangle(pnt1, pnt2, pnt3, Color.Red, new Dates());
            }
            for (double a = 0; a < twoPi; a += step)
            {
                var pnt1 = new Vector3d(1, 0, 0);
                var pnt3 = new Vector3d(1 - rad * 4, Math.Cos(a) * rad, Math.Sin(a) * rad);
                var pnt2 = new Vector3d(1 - rad * 4, Math.Cos(a + step) * rad, Math.Sin(a + step) * rad);
                TranslateUI.AddTriangle(pnt1, pnt2, pnt3, Color.FromArgb(255, Math.Max(0, (int)(Math.Sin(a) * 128)), Math.Max(0, (int)(Math.Sin(a) * 128))), new Dates());
            }

            TranslateUILines.AddLine(new Vector3d(0, 0, 0), new Vector3d(1, 0, 0), Color.Red);

            // Y
            for (double a = 0; a < twoPi; a += step)
            {
                var pnt1 = new Vector3d(0, 1 - rad * 4, 0);
                var pnt3 = new Vector3d(Math.Cos(a) * rad, 1 - rad * 4, Math.Sin(a) * rad);
                var pnt2 = new Vector3d(Math.Cos(a + step) * rad, 1 - rad * 4, Math.Sin(a + step) * rad);
                TranslateUI.AddTriangle(pnt1, pnt2, pnt3, Color.Green, new Dates());
            }

            for (double a = 0; a < twoPi; a += step)
            {
                var pnt1 = new Vector3d(0, 1, 0);
                var pnt2 = new Vector3d(Math.Cos(a) * rad, 1 - rad * 4, Math.Sin(a) * rad);
                var pnt3 = new Vector3d(Math.Cos(a + step) * rad, 1 - rad * 4, Math.Sin(a + step) * rad);
                TranslateUI.AddTriangle(pnt1, pnt2, pnt3, Color.FromArgb(Math.Max(0, (int)(Math.Sin(a) * 128)), 255, Math.Max(0, (int)(Math.Sin(a) * 128))), new Dates());
            }

            TranslateUILines.AddLine(new Vector3d(0, 0, 0), new Vector3d(0, 1, 0), Color.Green);

            // Z
            for (double a = 0; a < twoPi; a += step)
            {
                var pnt1 = new Vector3d(0, 0, 1 - rad * 4);
                var pnt2 = new Vector3d(Math.Cos(a) * rad, Math.Sin(a) * rad, 1 - rad * 4);
                var pnt3 = new Vector3d(Math.Cos(a + step) * rad, Math.Sin(a + step) * rad, 1 - rad * 4);
                TranslateUI.AddTriangle(pnt1, pnt2, pnt3, Color.Blue, new Dates());
            }

            for (double a = 0; a < twoPi; a += step)
            {
                var pnt1 = new Vector3d(0, 0, 1);
                var pnt3 = new Vector3d(Math.Cos(a) * rad, Math.Sin(a) * rad, 1 - rad * 4);
                var pnt2 = new Vector3d(Math.Cos(a + step) * rad, Math.Sin(a + step) * rad, 1 - rad * 4);
                TranslateUI.AddTriangle(pnt1, pnt2, pnt3, Color.FromArgb(Math.Max(0, (int)(Math.Sin(a) * 128)), Math.Max(0, (int)(Math.Sin(a) * 128)), 255), new Dates());
            }

            TranslateUILines.AddLine(new Vector3d(0, 0, 0), new Vector3d(0, 0, 1), Color.Blue);
            InitRotateUI();
            InitScaleUI();
        }

        static void InitScaleUI()
        {
            ScaleUI = new TriangleList();
            ScaleUI.DepthBuffered = false;
            ScaleUI.TimeSeries = false;
            ScaleUI.WriteZbuffer = false;

            var twoPi = Math.PI * 2;
            var step = twoPi / 45;
            var rad = .05;

            // X

            MakeCube(ScaleUI, new Vector3d(1 - rad * 2, 0, 0), rad * 2, Color.Red);
            MakeCube(ScaleUI, new Vector3d(0, 1 - rad * 2, 0), rad * 2, Color.Green);
            MakeCube(ScaleUI, new Vector3d(0, 0, 1 - rad * 2), rad * 2, Color.Blue);

        }

        static void MakeCube(TriangleList tl, Vector3d center, double size, Color color)
        {

            var dark = Color.FromArgb((int)(color.R * .6), (color.G), (int)(color.B * .6));
            var med = Color.FromArgb((int)(color.R * .8), (int)(color.G * .8), (int)(color.B * .8));


            tl.AddQuad(
                new Vector3d(center.X + size, center.Y + size, center.Z + size),
                new Vector3d(center.X + size, center.Y + size, center.Z - size),
                new Vector3d(center.X - size, center.Y + size, center.Z + size),
                new Vector3d(center.X - size, center.Y + size, center.Z - size),
                color, new Dates());

            tl.AddQuad(
                new Vector3d(center.X + size, center.Y - size, center.Z + size),
                new Vector3d(center.X - size, center.Y - size, center.Z + size),
                new Vector3d(center.X + size, center.Y - size, center.Z - size),
                new Vector3d(center.X - size, center.Y - size, center.Z - size),
                color, new Dates());


            tl.AddQuad(
               new Vector3d(center.X - size, center.Y + size, center.Z + size),
               new Vector3d(center.X - size, center.Y + size, center.Z - size),
               new Vector3d(center.X - size, center.Y - size, center.Z + size),
               new Vector3d(center.X - size, center.Y - size, center.Z - size),
               dark, new Dates());

            tl.AddQuad(
                new Vector3d(center.X + size, center.Y + size, center.Z + size),
                new Vector3d(center.X + size, center.Y - size, center.Z + size),
                new Vector3d(center.X + size, center.Y + size, center.Z - size),
                new Vector3d(center.X + size, center.Y - size, center.Z - size),
                dark, new Dates());

            tl.AddQuad(
              new Vector3d(center.X + size, center.Y + size, center.Z + size),
              new Vector3d(center.X - size, center.Y + size, center.Z + size),
              new Vector3d(center.X + size, center.Y - size, center.Z + size),
              new Vector3d(center.X - size, center.Y - size, center.Z + size),
              med, new Dates());

            tl.AddQuad(
              new Vector3d(center.X + size, center.Y + size, center.Z - size),
              new Vector3d(center.X + size, center.Y - size, center.Z - size),
              new Vector3d(center.X - size, center.Y + size, center.Z - size),
              new Vector3d(center.X - size, center.Y - size, center.Z - size),
              med, new Dates());

        }

        static TriangleList RotateUi;

        static void InitRotateUI()
        {
            RotateUi = new TriangleList();
            RotateUi.DepthBuffered = false;
            RotateUi.TimeSeries = false;
            RotateUi.WriteZbuffer = false;

            var twoPi = Math.PI * 2;
            var step = twoPi / 40;
            var rad = .05;
            var index = 0;

            for (double a = 0; a < twoPi; a += step)
            {
                var start = (index % 10) == 0;
                var end = ((index + 1) % 10) == 0;
                var pnt1 = new Vector3d(rad * (start ? 0 : (end ? 1.5 : 1)), Math.Cos(a), Math.Sin(a));
                var pnt2 = new Vector3d(-rad * (start ? 0 : (end ? 1.5 : 1)), Math.Cos(a), Math.Sin(a));
                var pnt3 = new Vector3d(rad * (start ? 1.5 : (end ? 0 : 1)), Math.Cos(a + step), Math.Sin(a + step));
                var pnt4 = new Vector3d(-rad * (start ? 1.5 : (end ? 0 : 1)), Math.Cos(a + step), Math.Sin(a + step));
                RotateUi.AddQuad(pnt1, pnt3, pnt2, pnt4, Color.FromArgb(192, Color.DarkRed), new Dates());
                //TranslateUI.AddQuad(pnt1, pnt2, pnt3, pnt4, System.Drawing.Color.FromArgb(192, Math.Max(0, (int)(Math.Sin(a) * 128)), Math.Max(0, (int)(Math.Sin(a) * 128))), new Dates());
                index++;
            }

            index = 0;
            for (double a = 0; a < twoPi; a += step)
            {
                var start = (index % 10) == 0;
                var end = ((index + 1) % 10) == 0;
                var pnt1 = new Vector3d(Math.Cos(a), Math.Sin(a), rad * (start ? 0 : (end ? 1.5 : 1)));
                var pnt2 = new Vector3d(Math.Cos(a), Math.Sin(a), -rad * (start ? 0 : (end ? 1.5 : 1)));
                var pnt3 = new Vector3d(Math.Cos(a + step), Math.Sin(a + step), rad * (start ? 1.5 : (end ? 0 : 1)));
                var pnt4 = new Vector3d(Math.Cos(a + step), Math.Sin(a + step), -rad * (start ? 1.5 : (end ? 0 : 1)));
                RotateUi.AddQuad(pnt1, pnt3, pnt2, pnt4, Color.FromArgb(192, Color.DarkBlue), new Dates());
                //TranslateUI.AddQuad(pnt1, pnt2, pnt3, pnt4, System.Drawing.Color.FromArgb(192, Math.Max(0, (int)(Math.Sin(a) * 128)), Math.Max(0, (int)(Math.Sin(a) * 128))), new Dates());
                index++;
            }

            index = 0;
            for (double a = 0; a < twoPi; a += step)
            {
                var start = (index % 10) == 0;
                var end = ((index + 1) % 10) == 0;
                var pnt1 = new Vector3d(Math.Cos(a), rad * (start ? 0 : (end ? 1.5 : 1)), Math.Sin(a));
                var pnt2 = new Vector3d(Math.Cos(a), -rad * (start ? 0 : (end ? 1.5 : 1)), Math.Sin(a));
                var pnt3 = new Vector3d(Math.Cos(a + step), rad * (start ? 1.5 : (end ? 0 : 1)), Math.Sin(a + step));
                var pnt4 = new Vector3d(Math.Cos(a + step), -rad * (start ? 1.5 : (end ? 0 : 1)), Math.Sin(a + step));
                RotateUi.AddQuad(pnt1, pnt2, pnt3, pnt4, Color.FromArgb(192, Color.DarkGreen), new Dates());
                //TranslateUI.AddQuad(pnt1, pnt2, pnt3, pnt4, System.Drawing.Color.FromArgb(192, Math.Max(0, (int)(Math.Sin(a) * 128)), Math.Max(0, (int)(Math.Sin(a) * 128))), new Dates());
                index++;
            }

            // X
            index = 0;
            for (double a = 0; a < twoPi; a += step)
            {
                var start = (index % 10) == 0;
                var end = ((index + 1) % 10) == 0;
                var pnt1 = new Vector3d(-rad * (start ? 0 : (end ? 1.5 : 1)), Math.Cos(a), Math.Sin(a));
                var pnt2 = new Vector3d(rad * (start ? 0 : (end ? 1.5 : 1)), Math.Cos(a), Math.Sin(a));
                var pnt3 = new Vector3d(-rad * (start ? 1.5 : (end ? 0 : 1)), Math.Cos(a + step), Math.Sin(a + step));
                var pnt4 = new Vector3d(rad * (start ? 1.5 : (end ? 0 : 1)), Math.Cos(a + step), Math.Sin(a + step));
                RotateUi.AddQuad(pnt1, pnt3, pnt2, pnt4, Color.Red, new Dates());
                //TranslateUI.AddQuad(pnt1, pnt2, pnt3, pnt4, System.Drawing.Color.FromArgb(255, Math.Max(0, (int)(Math.Sin(a) * 128)), Math.Max(0, (int)(Math.Sin(a) * 128))), new Dates());
                index++;
            }



            //Y
            index = 0;
            for (double a = 0; a < twoPi; a += step)
            {
                var start = (index % 10) == 0;
                var end = ((index + 1) % 10) == 0;
                var pnt1 = new Vector3d(Math.Cos(a), Math.Sin(a), -rad * (start ? 0 : (end ? 1.5 : 1)));
                var pnt2 = new Vector3d(Math.Cos(a), Math.Sin(a), rad * (start ? 0 : (end ? 1.5 : 1)));
                var pnt3 = new Vector3d(Math.Cos(a + step), Math.Sin(a + step), -rad * (start ? 1.5 : (end ? 0 : 1)));
                var pnt4 = new Vector3d(Math.Cos(a + step), Math.Sin(a + step), rad * (start ? 1.5 : (end ? 0 : 1)));
                RotateUi.AddQuad(pnt1, pnt3, pnt2, pnt4, Color.Blue, new Dates());
                //TranslateUI.AddQuad(pnt1, pnt2, pnt3, pnt4, System.Drawing.Color.FromArgb(255, Math.Max(0, (int)(Math.Sin(a) * 128)), Math.Max(0, (int)(Math.Sin(a) * 128))), new Dates());
                index++;
            }



            //Z
            index = 0;
            for (double a = 0; a < twoPi; a += step)
            {
                var start = (index % 10) == 0;
                var end = ((index + 1) % 10) == 0;
                var pnt1 = new Vector3d(Math.Cos(a), -rad * (start ? 0 : (end ? 1.5 : 1)), Math.Sin(a));
                var pnt2 = new Vector3d(Math.Cos(a), rad * (start ? 0 : (end ? 1.5 : 1)), Math.Sin(a));
                var pnt3 = new Vector3d(Math.Cos(a + step), -rad * (start ? 1.5 : (end ? 0 : 1)), Math.Sin(a + step));
                var pnt4 = new Vector3d(Math.Cos(a + step), rad * (start ? 1.5 : (end ? 0 : 1)), Math.Sin(a + step));
                RotateUi.AddQuad(pnt1, pnt2, pnt3, pnt4, Color.Green, new Dates());
                //TranslateUI.AddQuad(pnt1, pnt2, pnt3, pnt4, System.Drawing.Color.FromArgb(255, Math.Max(0, (int)(Math.Sin(a) * 128)), Math.Max(0, (int)(Math.Sin(a) * 128))), new Dates());
                index++;
            }
        }
        

        Vector2d xHandle;
        Vector2d yHandle;
        Vector2d zHandle;

        Vector2d[] hprHandles = new Vector2d[6];

        double uiScale = 1;

        public override bool Draw(RenderContext11 renderContext, float opacity, bool flat)
        {

            var oldWorld = renderContext.World;
            var rotation = Matrix3d.RotationZ(-roll / 180f * Math.PI) * Matrix3d.RotationX(-pitch / 180f * Math.PI) * Matrix3d.RotationY(heading / 180f * Math.PI);

            renderContext.World = rotation * Matrix3d.Scaling(scale.X, scale.Y, scale.Z) * Matrix3d.Translation(translate) * oldWorld;
            renderContext.TwoSidedLighting = TwoSidedGeometry;
            renderContext.setRasterizerState(TwoSidedGeometry ? TriangleCullMode.Off : TriangleCullMode.CullCounterClockwise);
            if (lightID > 0)
            {
                //draw light

             //   Planets.DrawPointPlanet(renderContext, new Vector3d(), 1, Color, false, 1.0f);
            }
            else
            {
                if (object3d != null)
                {
                    object3d.Color = Color;
                    object3d.Render(renderContext, opacity * Opacity);
                }
            }
            renderContext.setRasterizerState(TriangleCullMode.CullCounterClockwise);
            renderContext.TwoSidedLighting = false;

            if (showEditUi)
            {
                if (lightID > 0)
                {
                    //draw light

                    Planets.DrawPointPlanet(renderContext, new Vector3d(), 1, Color, false, 1.0f);
                }

                var oldDepthMode = renderContext.DepthStencilMode = DepthStencilMode.Off;
                renderContext.World = Matrix3d.Translation(translate) * oldWorld;

                var wvp = renderContext.World * renderContext.View * renderContext.Projection;

                var vc = new Vector3d(0, 0, 0);
                var vc1 = new Vector3d(.001, 0, 0);
                var vc2 = new Vector3d(0, .001, 0);
                var vc3 = new Vector3d(0, 0, .001);
                var vs = Vector3d.TransformCoordinate(vc, wvp);
                var vs1 = Vector3d.TransformCoordinate(vc1, wvp);
                var vs2 = Vector3d.TransformCoordinate(vc2, wvp);
                var vs3 = Vector3d.TransformCoordinate(vc3, wvp);

                var vsa = new Vector2d(vs.X, vs.Y);
                var vsa1 = new Vector2d(vs1.X, vs1.Y) - vsa;
                var vsa2 = new Vector2d(vs2.X, vs2.Y) - vsa;
                var vsa3 = new Vector2d(vs3.X, vs3.Y) - vsa;

                uiScale = .0003 / Math.Sqrt((vsa1.Length * vsa1.Length + vsa2.Length * vsa2.Length + vsa3.Length * vsa3.Length));

                var matUIScale = Matrix3d.Scaling(uiScale, uiScale, uiScale);

                renderContext.World = matUIScale * renderContext.World;

                wvp = renderContext.World * renderContext.View * renderContext.Projection;


                vc1 = new Vector3d(.9, 0, 0);
                vc2 = new Vector3d(0, .9, 0);
                vc3 = new Vector3d(0, 0, .9);
                vs = Vector3d.TransformCoordinate(vc, wvp);
                vs1 = Vector3d.TransformCoordinate(vc1, wvp);
                vs2 = Vector3d.TransformCoordinate(vc2, wvp);
                vs3 = Vector3d.TransformCoordinate(vc3, wvp);

                double h = renderContext.ViewPort.Height;
                double w = renderContext.ViewPort.Width;

                xHandle = new Vector2d((vs1.X + 1) * w / 2, h - ((vs1.Y + 1) * h / 2));
                yHandle = new Vector2d((vs2.X + 1) * w / 2, h - ((vs2.Y + 1) * h / 2));
                zHandle = new Vector2d((vs3.X + 1) * w / 2, h - ((vs3.Y + 1) * h / 2));


                // draw UI
                if (TranslateUI == null)
                {
                    InitTranslateUI();

                }

                var showTranslate = Control.ModifierKeys != Keys.Control && Control.ModifierKeys != Keys.Shift;
                var showRotate = Control.ModifierKeys == Keys.Control;
                var showScale = Control.ModifierKeys == Keys.Shift;

                if (showTranslate)
                {
                    TranslateUILines.DrawLines(renderContext, 1.0f);

                    TranslateUI.Draw(renderContext, 1.0f, TriangleList.CullMode.Clockwise);
                }
                else
                {
                    if (showScale)
                    {
                        TranslateUILines.DrawLines(renderContext, 1.0f);
                        ScaleUI.Draw(renderContext, 1.0f, TriangleList.CullMode.Clockwise);
                    }
                    else
                    {
                        xHandle = new Vector2d(-1000, 0);
                        yHandle = new Vector2d(-1000, 0);
                        zHandle = new Vector2d(-1000, 0);
                    }
                }

                renderContext.World = rotation * renderContext.World;

                if (showRotate)
                {
                    wvp = renderContext.World * renderContext.View * renderContext.Projection;

                    var hprPoints = new Vector3d[]
                                    {
                                        new Vector3d(0,0,1),
                                        new Vector3d(0,0,-1),
                                        new Vector3d(0,1,0),
                                        new Vector3d(0,-1,0),
                                        new Vector3d(-1,0,0),
                                        new Vector3d(1,0,0)
                                    };
                    hprHandles = new Vector2d[6];
                    for (var i = 0; i < 6; i++)
                    {
                        var vt = Vector3d.TransformCoordinate(hprPoints[i], wvp);
                        hprHandles[i] = new Vector2d((vt.X + 1) * w / 2, h - ((vt.Y + 1) * h / 2));
                    }

                    RotateUi.Draw(renderContext, 1.0f, TriangleList.CullMode.Clockwise);
                }
                else
                {
                    hprHandles = new Vector2d[0];
                }




                oldDepthMode = renderContext.DepthStencilMode = oldDepthMode;
      
                //restore matrix
                renderContext.World = oldWorld;
                showEditUi = false;
            }
            renderContext.World = oldWorld;

            return true;
        }

        public override void AddFilesToCabinet(FileCabinet fc)
        {
            if (object3d != null)
            {
                var fName = object3d.Filename;

                var copy = true;
                //bool copy = !fName.Contains(ID.ToString());
                var ext = ObjType ? "obj" : "3ds";
                var fileName = fc.TempDirectory + string.Format("{0}\\{1}.{2}", fc.PackageID, ID, ext);
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

                    foreach (var meshfile in object3d.meshFilenames)
                    {
                        if (!String.IsNullOrEmpty(meshfile))
                        {
                            var textureFilename = fc.TempDirectory + string.Format("{0}\\{1}", fc.PackageID, meshfile);
                            var mFilename = path + "\\" + meshfile;
                            var newfilename = Object3d.FindFile(mFilename);
                            if (string.IsNullOrEmpty(newfilename))
                            {
                                newfilename = Object3d.FindFileFuzzy(mFilename);
                            }



                            if (File.Exists(newfilename) && !File.Exists(textureFilename))
                            {
                                File.Copy(newfilename, textureFilename);
                            }
                        }
                    }
                }

                if (File.Exists(fileName))
                {
                    fc.AddFile(fileName);
                }

                foreach (var meshfile in object3d.meshFilenames)
                {
                    if (!string.IsNullOrEmpty(meshfile))
                    {
                        var textureFilename = fc.TempDirectory + string.Format("{0}\\{1}", fc.PackageID, meshfile);
                        fc.AddFile(textureFilename);
                    }
                }
            }
        }

        public override void LoadData(string path)
        {

            // ObjType = true;

            if (path.ToLower().EndsWith(".obj"))
            {
                ObjType = true;
            }

            if (lightID == 0)
            {
                if (ObjType)
                {
                    object3d = new Object3d(path.Replace(".txt", ".obj"), FlipV, flipHandedness, true, Color);
                }
                else
                {
                    object3d = new Object3d(path.Replace(".txt", ".3ds"), FlipV, flipHandedness, true, Color);
                }
            }
        }

        public void Render(Earth3d window)
        {
            showEditUi = true;
            return ;
        }
        bool showEditUi;
        public void PreRender(Earth3d window)
        {
            showEditUi = true;
            return ;
        }

        enum Draging { None, X, Y, Z, HP, PR, RH, HP1, PR1, RH1,  Scale };

        Draging dragMode = Draging.None;

        Point pntDown;
        double valueOnDown;
        double valueOnDown2;

        double hitDist = 20;

        public bool MouseDown(object sender, MouseEventArgs e)
        {
            pntDown = e.Location;

            var pnt = new Vector2d(e.X, e.Y);

            if (Control.ModifierKeys == Keys.Shift)
            {
                if ((pnt - xHandle).Length < hitDist)
                {
                    dragMode = Draging.Scale;
                    valueOnDown = scale.X;
                    return true;
                }

                if ((pnt - yHandle).Length < hitDist)
                {
                    dragMode = Draging.Scale;
                    valueOnDown = scale.Y;
                    return true;
                }

                if ((pnt - zHandle).Length < hitDist)
                {
                    dragMode = Draging.Scale;
                    valueOnDown = scale.Z;
                    return true;
                }
            }
            else
            {
                if ((pnt - xHandle).Length < hitDist)
                {
                    dragMode = Draging.X;
                    valueOnDown = translate.X;
                    return true;
                }

                if ((pnt - yHandle).Length < hitDist)
                {
                    dragMode = Draging.Y;
                    valueOnDown = translate.Y;
                    return true;
                }

                if ((pnt - zHandle).Length < hitDist)
                {
                    dragMode = Draging.Z;
                    valueOnDown = translate.Z;
                    return true;
                }
            }

            for (var i = 0; i < hprHandles.Length; i++)
            {
                if ((pnt - hprHandles[i]).Length < hitDist)
                {
                    switch (i)
                    {
                        case 0:
                            dragMode = Draging.HP;
                            valueOnDown = heading;
                            valueOnDown2 = pitch;
                            return true;
                        case 1:
                            dragMode = Draging.HP1;
                            valueOnDown = heading;
                            valueOnDown2 = pitch;
                            return true;
                        case 2:
                            dragMode = Draging.PR;
                            valueOnDown = pitch;
                            valueOnDown2 = roll;
                            return true;
                        case 3:
                            dragMode = Draging.PR1;
                            valueOnDown = pitch;
                            valueOnDown2 = roll;
                            return true;
                        case 4: 
                            dragMode = Draging.RH;
                            valueOnDown = roll;
                            valueOnDown2 = heading;
                            return true;
                        case 5:
                            dragMode = Draging.RH1;
                            valueOnDown = roll;
                            valueOnDown2 = heading;
                            return true;
                        default:
                            break;
                    }
                }
            }

            return false;
        }

        public bool MouseUp(object sender, MouseEventArgs e)
        {
            if (dragMode != Draging.None)
            {
                dragMode = Draging.None;
                lockPreferedAxis = false;
                return true;
            }
            return false;
        }

        bool lockPreferedAxis;
        bool preferY;

        public bool MouseMove(object sender, MouseEventArgs e)
        {

            //pntDown = e.Location;

            if (dragMode != Draging.None)
            {
                double dist = 0;
                double distX = e.X - pntDown.X;
                double distY = -(e.Y - pntDown.Y);

                if (lockPreferedAxis)
                {
                    if (preferY)
                    {
                        dist = distY;
                        preferY = true;
                        Cursor.Current = Cursors.SizeNS;
                    }
                    else
                    {
                        dist = distX;
                        preferY = false;
                        Cursor.Current = Cursors.SizeWE;
                    }
                }
                else
                {
                    if (Math.Abs(distX) > Math.Abs(distY))
                    {
                        dist = distX;
                        preferY = false;
                    }
                    else
                    {
                        dist = distY;
                        preferY = true;
                    }
                    if (dist > 5)
                    {
                        lockPreferedAxis = true;
                    }
                }

                switch (dragMode)
                {
                    case Draging.None:
                        break;
                    case Draging.X:
                        translate.X = valueOnDown + (12 * uiScale * (dist / Earth3d.MainWindow.RenderContext11.ViewPort.Width));
                        break;
                    case Draging.Y:
                        translate.Y = valueOnDown + (12 * uiScale * (dist / Earth3d.MainWindow.RenderContext11.ViewPort.Width));
                        break;
                    case Draging.Z:
                        translate.Z = valueOnDown + (12 * uiScale * (dist / Earth3d.MainWindow.RenderContext11.ViewPort.Width));
                        break;
                    case Draging.HP:
                        heading = valueOnDown - distX / 4;
                        pitch = valueOnDown2 + distY / 4;
                        break;
                    case Draging.PR:
                        pitch = valueOnDown + distY / 4;
                        roll = valueOnDown2 - distX / 4;
                        break;
                    case Draging.RH:
                        roll = valueOnDown + distY / 4;
                        heading = valueOnDown2 - distX / 4;
                        break;
                    case Draging.HP1:
                        heading = valueOnDown - distX / 4;
                        pitch = valueOnDown2 - distY / 4;
                        break;
                    case Draging.PR1:
                        pitch = valueOnDown + distY / 4;
                        roll = valueOnDown2 + distX / 4;
                        break;
                    case Draging.RH1:
                        roll = valueOnDown - distY / 4;
                        heading = valueOnDown2 - distX / 4;
                        break;
                    case Draging.Scale:
                        scale.X = scale.Y = scale.Z = valueOnDown * Math.Pow(2, (dist / 100));
                        break;
                    default:
                        break;
                }
                FireChanged();
                return true;
            }
            var pnt = new Vector2d(e.X, e.Y);


            if ((pnt - xHandle).Length < hitDist)
            {
                Cursor.Current = Cursors.SizeAll;
                return true;
            }

            if ((pnt - yHandle).Length < hitDist)
            {
                Cursor.Current = Cursors.SizeAll;
                return true;
            }

            if ((pnt - zHandle).Length < hitDist)
            {
                Cursor.Current = Cursors.SizeAll;
                return true;
            }

            for (var i = 0; i < hprHandles.Length; i++)
            {
                if ((pnt - hprHandles[i]).Length < hitDist)
                {
                    Cursor.Current = Cursors.SizeAll;
                    return true;
                }
            }

            return false;
        }

        public bool MouseClick(object sender, MouseEventArgs e)
        {


            return false;
        }

        public bool Click(object sender, EventArgs e)
        {
            return false;
        }

        public bool MouseDoubleClick(object sender, MouseEventArgs e)
        {
            return false;
        }

        public bool KeyDown(object sender, KeyEventArgs e)
        {
            return false;
        }

        public bool KeyUp(object sender, KeyEventArgs e)
        {
            return false;
        }

        public bool Hover(Point pnt)
        {
            return false;
        }
    }

    public class Mesh : IDisposable
    {
        public struct Group
        {
            public int startIndex;
            public int indexCount;
            public int materialIndex;
        }

        public void Dispose()
        {
            if (vertexBuffer != null)
            {
                vertexBuffer.Dispose();
                GC.SuppressFinalize(vertexBuffer);
                vertexBuffer = null;
            }

            if (tangentVertexBuffer != null)
            {
                tangentVertexBuffer.Dispose();
                GC.SuppressFinalize(tangentVertexBuffer);
                tangentVertexBuffer = null;
            }

            if (indexBuffer != null)
            {
                indexBuffer.Dispose();
                GC.SuppressFinalize(indexBuffer);
                indexBuffer = null;
            }
        }

        public Mesh(PositionNormalTextured[] vertices, uint[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;

            var points = new Vector3[vertices.Length];
            for (var i = 0; i < vertices.Length; ++i)
                points[i] = vertices[i].Position;
            boundingSphere = BoundingSphere.FromPoints(points);
        }

        public Mesh(PositionNormalTextured[] vertices, int[] indices)
        {
            this.vertices = vertices;

            this.indices = new uint[indices.Length];
            for (var c = 0; c < indices.Length; c++)
            {
                this.indices[c] = (uint)indices[c];
            }

            var points = new Vector3[vertices.Length];
            for (var i = 0; i < vertices.Length; ++i)
                points[i] = vertices[i].Position;
            boundingSphere = BoundingSphere.FromPoints(points);
        }

        // Create a mesh from vertices with tangents, for use with a normal map
        public Mesh(PositionNormalTexturedTangent[] vertices, uint[] indices)
        {
            tangentVertices = vertices;
            this.indices = indices;

            var points = new Vector3[tangentVertices.Length];
            for (var i = 0; i < tangentVertices.Length; ++i)
                points[i] = tangentVertices[i].Position;
            boundingSphere = BoundingSphere.FromPoints(points);
        }

        //public void setMaterialGroups(Group[] groups)
        //{
        //    attributeGroups = groups;
        //}

        public void setObjects(List<ObjectNode> objects)
        {
            this.objects = objects;
        }

        // Convert the vertex data to a GPU vertex buffer
        public void commitToDevice(Device device)
        {
            if (vertices != null)
            {
                vertexBuffer = new GenVertexBuffer<PositionNormalTextured>(device, vertices);
            }
            else if (tangentVertices != null)
            {
                tangentVertexBuffer = new GenVertexBuffer<PositionNormalTexturedTangent>(device, tangentVertices);
            }

            indexBuffer = new IndexBuffer11(device, indices);
        }

        public void beginDrawing(RenderContext11 renderContext)
        {
            if (vertexBuffer != null)
            {
                renderContext.SetVertexBuffer(vertexBuffer);
            }
            else if (tangentVertexBuffer != null)
            {
                renderContext.SetVertexBuffer(tangentVertexBuffer);
            }

            if (indexBuffer != null)
            {
                renderContext.SetIndexBuffer(indexBuffer);
            }

            var devContext = renderContext.Device.ImmediateContext;
            devContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }

        public void drawSubset(RenderContext11 renderContext, int materialIndex)
        {
            if (indexBuffer == null || objects == null)
            {
                return;
            }

            var vertexLayout = PlanetShader11.StandardVertexLayout.PositionNormalTex;
            if (tangentVertexBuffer != null)
            {
                vertexLayout = PlanetShader11.StandardVertexLayout.PositionNormalTexTangent;
            }

            var devContext = renderContext.Device.ImmediateContext;
            devContext.InputAssembler.InputLayout = renderContext.Shader.inputLayout(vertexLayout);


            DrawHierarchy(objects, materialIndex, devContext, 0);
        }

        private void DrawHierarchy(List<ObjectNode> nodes, int materialIndex, DeviceContext devContext, int depth)
        {
            if (depth > 1212)
            {
                return;
            }
            foreach (var node in nodes)
            {
                if (node.DrawGroup != null && node.Enabled)
                {
                    foreach (var group in node.DrawGroup)
                    {
                        if (group.materialIndex == materialIndex)
                        {
                            devContext.DrawIndexed(group.indexCount, group.startIndex, 0);
                        }
                    }
                }
                DrawHierarchy(node.Children, materialIndex, devContext, depth + 1);
            }
        }

        public BoundingSphere BoundingSphere
        {
            get
            {
                return boundingSphere;
            }
        }

        private GenVertexBuffer<PositionNormalTextured> vertexBuffer;
        private GenVertexBuffer<PositionNormalTexturedTangent> tangentVertexBuffer;
        private IndexBuffer11 indexBuffer;

        // Only one of these two will be non-null
        private readonly PositionNormalTextured[] vertices;
        private readonly PositionNormalTexturedTangent[] tangentVertices;
        private readonly uint[] indices;

        private readonly BoundingSphere boundingSphere;

        //Group[] attributeGroups;
        List<ObjectNode> objects;

        public List<ObjectNode> Objects
        {
            get { return objects; }
            set { objects = value; }
        }
    }


    // Summary:
    //    Custom vertex format with position, normal, texture coordinate, and tangent vector. The
    //    tangent vector is stored in the second texture coordinate.
    public struct PositionNormalTexturedTangent
    {
        // Summary:
        //     Retrieves the Microsoft.DirectX.Direct3D.VertexFormats for the current custom
        //     vertex.
        //public static  VertexFormats Format = VertexFormats.Position |
        //                                      VertexFormats.Normal   |
        //                                      VertexFormats.Texture2 |
        //                                      VertexTextureCoordinate.Size3(1);
        public float X;
        //
        // Summary:
        //     Retrieves or sets the y component of the position.
        public float Y;
        //
        // Summary:
        //     Retrieves or sets the z component of the position.
        public float Z;
        // Summary:
        //     Retrieves or sets the nx component of the vertex normal.
        public float Nx;
        //
        // Summary:
        //     Retrieves or sets the ny component of the vertex normal.
        public float Ny;
        //
        // Summary:
        //     Retrieves or sets the nz component of the vertex normal.
        public float Nz;
        //
        // Summary:
        //     Retrieves or sets the u component of the texture coordinate.
        public float Tu;
        //
        // Summary:
        //     Retrieves or sets the v component of the texture coordinate.
        public float Tv;
        //
        // Summary:
        //     Retrieves or sets the x component of the tangent vector
        public float Tanx;
        //
        // Summary:
        //     Retrieves or sets the y component of the tangent vector
        public float Tany;
        //
        // Summary:
        //     Retrieves or sets the z component of the tangent vector
        public float Tanz;

        //
        // Summary:
        //     Initializes a new instance of the PositionNormalTexturedTangent
        //     class.
        //
        public PositionNormalTexturedTangent(Vector3 position, Vector3 normal, Vector2 texCoord, Vector3 tangent)
        {
            X = position.X;
            Y = position.Y;
            Z = position.Z;
            Nx = normal.X;
            Ny = normal.Y;
            Nz = normal.Z;
            Tu = texCoord.X;
            Tv = texCoord.Y;
            Tanx = tangent.X;
            Tany = tangent.Y;
            Tanz = tangent.Z;
        }

        // Summary:
        //     Retrieves or sets the vertex normal data.
        public Vector3 Normal
        {
            get
            {
                return new Vector3(Nx, Ny, Nz);
            }
            set
            {
                Nx = value.X;
                Ny = value.Y;
                Nz = value.Z;
            }
        }

        // Summary:
        //     Retrieves or sets the vertex position.
        public Vector3 Position
        {
            get
            {
                return new Vector3(X, Y, Z);
            }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        // Summary:
        //     Retrieves or sets the texture coordinate.
        public Vector2 TexCoord
        {
            get
            {
                return new Vector2(Tu, Tv);
            }
            set
            {
                Tu = value.X;
                Tv = value.Y;
            }
        }

        // Summary:
        //     Retrieves or sets the vertex tangent.
        public Vector3 Tangent
        {
            get
            {
                return new Vector3(Tanx, Tany, Tanz);
            }
            set
            {
                Tanx = value.X;
                Tany = value.Y;
                Tanz = value.Z;
            }
        }


        // Summary:
        //     Retrieves the size of the PositionNormalTexturedTangent
        //     structure.
        public static int StrideSize
        {
            get
            {
                return 44;
            }

        }

        // Summary:
        //     Obtains a string representation of the current instance.
        //
        // Returns:
        //     String that represents the object.
        public override string ToString()
        {
            return string.Format(
                "X={0}, Y={1}, Z={2}, Nx={3}, Ny={4}, Nz={5}, U={6}, V={7}, TanX={8}, TanY={9}, TanZ={10}",
                X, Y, Z, Nx, Ny, Nz, Tu, Tv, Tanx, Tany, Tanz
                );
        }
    }


    public class Object3d
    {
        public bool FlipHandedness = false;
        public bool FlipV = true;
        public bool Smooth = true;
        public string Filename;
        Mesh mesh; // Our mesh object in sysmem
        readonly List<Material> meshMaterials = new List<Material>(); // Materials for our mesh
        readonly List<Texture11> meshTextures = new List<Texture11>(); // Textures for our mesh
        readonly List<Texture11> meshSpecularTextures = new List<Texture11>(); // Specular textures for our mesh
        readonly List<Texture11> meshNormalMaps = new List<Texture11>(); // Normal maps for our mesh
        public List<String> meshFilenames = new List<string>(); // filenames for meshes

        public Color Color = Color.White;

        public Object3d(string filename, bool flipV, bool flipHandedness, bool smooth, Color color)
        {
            Color = color;
            Smooth = smooth;
            FlipV = flipV;
            FlipHandedness = flipHandedness;
            Filename = filename;
            if (Filename.ToLower().EndsWith(".obj"))
            {
                LoadMeshFromObj(Filename);
            }
            else
            {
                LoadMeshFrom3ds(Filename, 1.0f);
            }
        }

        internal void Reload()
        {
            if (!ISSLayer)
            {
                Dispose();
                if (Filename.ToLower().EndsWith(".obj"))
                {
                    LoadMeshFromObj(Filename);
                }
                else
                {
                    LoadMeshFrom3ds(Filename, 1.0f);
                }
            }
        }


        struct VertexPosition
        {
            public Vector3 position;
            public uint index;
        };



        private static int CompareVector3(Vector3 v0, Vector3 v1)
        {
            if (v0.X < v1.X)
            {
                return -1;
            }
            if (v0.X > v1.X)
            {
                return 1;
            }
            if (v0.Y < v1.Y)
            {
                return -1;
            }
            if (v0.Y > v1.Y)
            {
                return 1;
            }
            if (v0.Z < v1.Z)
            {
                return -1;
            }
            if (v0.Z > v1.Z)
            {
                return 1;
            }
            return 0;
        }

        private static int CompareVector(Vector2 v0, Vector2 v1)
        {
            if (v0.X < v1.X)
            {
                return -1;
            }
            if (v0.X > v1.X)
            {
                return 1;
            }
            if (v0.Y < v1.Y)
            {
                return -1;
            }
            if (v0.Y > v1.Y)
            {
                return 1;
            }
            return 0;
        }


        // Calculate per-vertex normals by averaging face normals. Normals of adjacent faces with an
        // angle of greater than crease angle are not included in the average. CalculateVertexNormalsMerged
        // is slower than the other normal generation method, CalculateVertexNormals, but it produces better
        // results. Vertices with identical positions (bot possibly different texture coordinates) are treated
        // as the same vertex for purposes of normal calculation. This allows smooth normals across texture
        // wrap seams.
        //
        // This method returns an array of vertex normals, one for each index in the index list
        private Vector3[] CalculateVertexNormalsMerged(List<PositionNormalTextured> vertexList, List<int> indexList, float creaseAngleRad)
        {
            if (vertexList.Count == 0)
            {
                return null;
            }

            var vertexCount = vertexList.Count;
            var triangleCount = indexList.Count / 3;

            // Create a list of vertices sorted by their positions. This will be used to
            // produce a list of vertices with unique positions.
            var vertexPositions = new List<VertexPosition>();
            for (var vertexIndex = 0; vertexIndex < vertexList.Count; ++vertexIndex)
            {
                VertexPosition vp;
                //todo11 this should be native..
                vp.position = vertexList[vertexIndex].Pos;
                vp.index = (uint)vertexIndex;
                vertexPositions.Add(vp);
            }

            vertexPositions.Sort((v0, v1) => { return CompareVector3(v0.position, v1.position); });

            // vertexMap will map a vertex index to the index of a vertex with a unique position
            var vertexMap = new int[vertexPositions.Count];
            var uniqueVertexCount = 0;
            for (var vertexIndex = 0; vertexIndex < vertexPositions.Count; vertexIndex++)
            {
                if (vertexIndex == 0 || CompareVector3(vertexPositions[vertexIndex].position, vertexPositions[vertexIndex - 1].position) != 0)
                {
                    ++uniqueVertexCount;
                }
                vertexMap[vertexPositions[vertexIndex].index] = uniqueVertexCount - 1;
            }

            var vertexInstanceCounts = new int[uniqueVertexCount];
            foreach (var vertexIndex in indexList)
            {
                var uniqueIndex = vertexMap[vertexIndex];
                vertexInstanceCounts[uniqueIndex]++;
            }

            // vertexInstances contains the list of faces each vertex is referenced in
            var vertexInstances = new int[uniqueVertexCount][];
            for (var i = 0; i < uniqueVertexCount; ++i)
            {
                var count = vertexInstanceCounts[i];
                if (count > 0)
                {
                    vertexInstances[i] = new int[count];
                }
            }

            // For each vertex, record all faces which include it
            for (var i = 0; i < indexList.Count; ++i)
            {
                var faceIndex = i / 3;
                var uniqueIndex = vertexMap[indexList[i]];
                vertexInstances[uniqueIndex][--vertexInstanceCounts[uniqueIndex]] = faceIndex;
            }

            // At this point, vertexInstanceCounts should contain nothing but zeroes

            // Compute normals for all faces
            var faceNormals = new Vector3[triangleCount];
            for (var i = 0; i < triangleCount; ++i)
            {
                // The face normal is just the cross product of the two edge vectors
                var i0 = indexList[i * 3 + 0];
                var i1 = indexList[i * 3 + 1];
                var i2 = indexList[i * 3 + 2];
                var edge0 = vertexList[i1].Position - vertexList[i0].Position;
                var edge1 = vertexList[i2].Position - vertexList[i1].Position;
                faceNormals[i] = Vector3.Cross(edge0, edge1);

                faceNormals[i].Normalize();
            }

            // Finally, average the face normals
            var newVertexCount = triangleCount * 3;
            var vertexNormals = new Vector3[newVertexCount];
            var cosCreaseAngle = Math.Min(0.9999f, (float)Math.Cos(creaseAngleRad));
            for (var i = 0; i < newVertexCount; ++i)
            {
                var vertexIndex = indexList[i];
                var uniqueIndex = vertexMap[vertexIndex];
                var faceNormal = faceNormals[i / 3];

                var sum = Vector3.Zero;
                foreach (var faceIndex in vertexInstances[uniqueIndex])
                {
                    var n = faceNormals[faceIndex];
                    if (Vector3.Dot(faceNormal, n) > cosCreaseAngle)
                    {
                        sum += n;
                    }
                }

                vertexNormals[i] = sum;
                vertexNormals[i].Normalize();
            }

            return vertexNormals;
        }


        // Calculate tangent vectors at each vertex. The 'face tangent' is a direction in the plane of the
        // triangle and parallel to the direction of increasing tex coord u, i.e. the partial derivative
        // with respect to u of the triangle's plane equation expressed in terms of the texture coordinate
        // (u, v). Partial derivatives of the triangles containing a vertex are averaged to compute the
        // vertex tangent. Faces are not included in the when the angle formed with the test face is
        // greater than the crease angle, or when the texture texture coordinates are not continuous.
        //
        // This method returns an array of vertex normals, one for each index in the index list
        private Vector3[] CalculateVertexTangents(List<PositionNormalTextured> vertexList, List<uint> indexList, float creaseAngleRad)
        {
            if (vertexList.Count == 0)
            {
                return null;
            }

            var vertexCount = vertexList.Count;
            var triangleCount = indexList.Count / 3;

            // Create a list of vertices sorted by their positions. This will be used to
            // produce a list of vertices with unique positions.
            var vertexPositions = new List<VertexPosition>();
            for (var vertexIndex = 0; vertexIndex < vertexList.Count; ++vertexIndex)
            {
                VertexPosition vp;
                vp.position = vertexList[vertexIndex].Pos;
                vp.index = (uint)vertexIndex;
                vertexPositions.Add(vp);
            }

            vertexPositions.Sort((v0, v1) => { return CompareVector3(v0.position, v1.position); });

            // vertexMap will map a vertex index to the index of a vertex with a unique position
            var vertexMap = new uint[vertexPositions.Count];
            var uniqueVertexCount = 0;
            for (var vertexIndex = 0; vertexIndex < vertexPositions.Count; vertexIndex++)
            {
                if (vertexIndex == 0 || CompareVector3(vertexPositions[vertexIndex].position, vertexPositions[vertexIndex - 1].position) != 0)
                {
                    ++uniqueVertexCount;
                }
                vertexMap[vertexPositions[vertexIndex].index] = (uint)(uniqueVertexCount - 1);
            }

            var vertexInstanceCounts = new int[uniqueVertexCount];
            foreach (int vertexIndex in indexList)
            {
                var uniqueIndex = vertexMap[vertexIndex];
                vertexInstanceCounts[uniqueIndex]++;
            }

            // vertexInstances contains the list of faces each vertex is referenced in
            var vertexInstances = new int[uniqueVertexCount][];
            for (var i = 0; i < uniqueVertexCount; ++i)
            {
                var count = vertexInstanceCounts[i];
                if (count > 0)
                {
                    vertexInstances[i] = new int[count];
                }
            }

            // For each vertex, record all faces which include it
            for (var i = 0; i < indexList.Count; ++i)
            {
                var faceIndex = i / 3;
                var uniqueIndex = vertexMap[indexList[i]];
                vertexInstances[uniqueIndex][--vertexInstanceCounts[uniqueIndex]] = faceIndex;
            }

            // At this point, vertexInstanceCounts should contain nothing but zeroes

            // Compute partial derivatives for all faces
            var partials = new Vector3[triangleCount];
            for (var i = 0; i < triangleCount; ++i)
            {
                var v0 = vertexList[(int)indexList[i * 3 + 0]];
                var v1 = vertexList[(int)indexList[i * 3 + 1]];
                var v2 = vertexList[(int)indexList[i * 3 + 2]];
                var edge0 = v1.Position - v0.Position;
                var edge1 = v2.Position - v0.Position;
                var m00 = v1.Tu - v0.Tu;
                var m01 = v1.Tv - v0.Tv;
                var m10 = v2.Tu - v0.Tu;
                var m11 = v2.Tv - v0.Tv;

                var determinant = m00 * m11 - m01 * m10;
                if (Math.Abs(determinant) < 1.0e-6f)
                {
                    // No unique vector; just select one of the edges
                    if (edge0.LengthSquared() > 0.0f)
                    {
                        partials[i] = edge0;
                        partials[i].Normalize();
                    }
                    else
                    {
                        // Degenerate edge; just use the unit x vector
                        partials[i] = new Vector3(1.0f, 0.0f, 0.0f);
                    }
                }
                else
                {
                    // Matrix n is the inverse of m
                    var invDeterminant = 1.0f / determinant;
                    var n00 = m11 * invDeterminant;
                    var n01 = -m01 * invDeterminant;
                    var n10 = -m10 * invDeterminant;
                    var n11 = m00 * invDeterminant;

                    partials[i] = n00 * edge0 + n01 * edge1;
                    partials[i].Normalize();
                }
            }

            // Finally, average the partial derivatives
            var newVertexCount = triangleCount * 3;
            var tangents = new Vector3[newVertexCount];
            var cosCreaseAngle = Math.Min(0.9999f, (float)Math.Cos(creaseAngleRad));
            for (var i = 0; i < newVertexCount; ++i)
            {
                var vertexIndex = indexList[i];
                var uniqueIndex = vertexMap[(int)vertexIndex];
                var du = partials[i / 3];

                var sum = Vector3.Zero;
                foreach (var faceIndex in vertexInstances[uniqueIndex])
                {
                    var T = partials[faceIndex];
                    if (Vector3.Dot(du, T) > cosCreaseAngle)
                    {
                        sum += T;
                    }
                }

                var N = vertexList[(int)vertexIndex].Normal;

                // Make the tangent orthogonal to the vertex normal
                tangents[i] = sum - Vector3.Dot(N, sum) * N;
                tangents[i].Normalize();
            }

            return tangents;
        }


        // Calculate per-vertex normals by averaging face normals. Normals of adjacent faces with an
        // angle of greater than crease angle are not included in the average.
        //
        // This method returns an array of vertex normals, one for each index in the index list
        private Vector3[] CalculateVertexNormals(List<PositionNormalTextured> vertexList, List<int> indexList, float creaseAngleRad)
        {
            var vertexCount = vertexList.Count;
            var triangleCount = indexList.Count / 3;

            // vertexInstanceCounts contains the number of times each vertex is referenced in the mesh 
            var vertexInstanceCounts = new int[vertexCount];
            foreach (var vertexIndex in indexList)
            {
                vertexInstanceCounts[vertexIndex]++;
            }

            // vertexInstances contains the list of faces each vertex is referenced in
            var vertexInstances = new int[vertexCount][];
            for (var i = 0; i < vertexCount; ++i)
            {
                var count = vertexInstanceCounts[i];
                if (count > 0)
                {
                    vertexInstances[i] = new int[count];
                }
            }

            // For each vertex, record all faces which include it
            for (var i = 0; i < indexList.Count; ++i)
            {
                var faceIndex = i / 3;
                var vertexIndex = indexList[i];
                vertexInstances[vertexIndex][--vertexInstanceCounts[vertexIndex]] = faceIndex;
            }

            // At this point, vertexInstanceCounts should contain nothing but zeroes

            // Compute normals for all faces
            var faceNormals = new Vector3[triangleCount];
            for (var i = 0; i < triangleCount; ++i)
            {
                // The face normal is just the cross product of the two edge vectors
                var i0 = indexList[i * 3 + 0];
                var i1 = indexList[i * 3 + 1];
                var i2 = indexList[i * 3 + 2];
                var edge0 = vertexList[i1].Position - vertexList[i0].Position;
                var edge1 = vertexList[i2].Position - vertexList[i1].Position;
                faceNormals[i] = Vector3.Cross(edge0, edge1);

                faceNormals[i].Normalize();
            }

            // Finally, average the face normals
            var newVertexCount = triangleCount * 3;
            var vertexNormals = new Vector3[newVertexCount];
            var cosCreaseAngle = Math.Min(0.9999f, (float)Math.Cos(creaseAngleRad));
            for (var i = 0; i < newVertexCount; ++i)
            {
                var vertexIndex = indexList[i];
                var faceNormal = faceNormals[i / 3];

                var sum = Vector3.Zero;
                foreach (var faceIndex in vertexInstances[vertexIndex])
                {
                    var n = faceNormals[faceIndex];
                    if (Vector3.Dot(faceNormal, n) > cosCreaseAngle)
                    {
                        sum += n;
                    }
                }

                vertexNormals[i] = sum;
                vertexNormals[i].Normalize();
            }

            return vertexNormals;
        }

        // Add textures to ensure that we have as many textures as 
        private void addMaterial(Material material)
        {
            meshMaterials.Add(material);
            while (meshTextures.Count < meshMaterials.Count)
            {
                meshTextures.Add(null);
            }

            while (meshSpecularTextures.Count < meshMaterials.Count)
            {
                meshSpecularTextures.Add(null);
            }

            while (meshNormalMaps.Count < meshMaterials.Count)
            {
                meshNormalMaps.Add(null);
            }
        }

        // Load a color chunk from a 3ds file
        // Colors may be stored in a 3ds file either as 3 floats or 3 bytes
        private Color LoadColorChunk(BinaryReader br)
        {
            var chunkID = br.ReadUInt16();
            var chunkLength = br.ReadUInt32();
            var color = Color.Black;

            if ((chunkID == 0x0010 || chunkID == 0x0013) && chunkLength == 18)
            {
                // Need to guard against values outside of [0, 1], otherwise Color.FromArgb
                // will throw an exception.
                var r = Math.Max(0.0f, Math.Min(1.0f, br.ReadSingle()));
                var g = Math.Max(0.0f, Math.Min(1.0f, br.ReadSingle()));
                var b = Math.Max(0.0f, Math.Min(1.0f, br.ReadSingle()));
                color = Color.FromArgb(255, (int)(255.0f * r), (int)(255.0f * g), (int)(255.0f * b));
            }
            else if ((chunkID == 0x0011 || chunkID == 0x0012) && chunkLength == 9)
            {
                color = Color.FromArgb(255, br.ReadByte(), br.ReadByte(), br.ReadByte());
            }
            else
            {
                // Unknown color block; ignore it
                br.ReadBytes((int)chunkLength - 6);
            }

            return color;
        }


        // Load a percentage chunk from a 3ds file
        // A percentage may be stored as either a float or a 16-bit integer
        private float LoadPercentageChunk(BinaryReader br)
        {
            var chunkID = br.ReadUInt16();
            var chunkLength = br.ReadUInt32();
            var percentage = 0.0f;

            if (chunkID == 0x0030 && chunkLength == 8)
            {
                percentage = br.ReadUInt16();
            }
            else if (chunkID == 0x0031 && chunkLength == 10)
            {
                percentage = br.ReadSingle();
            }
            else
            {
                // Unknown percentage block; ignore it
                br.ReadBytes((int)chunkLength - 6);
            }

            return percentage;
        }

        readonly Dictionary<string, Texture11> TextureCache = new Dictionary<string, Texture11>();

        private void LoadMeshFromObj(string filename)
        {
            // Force garbage collection to ensure that unmanaged resources are released.
            // Temporary workaround until unmanaged resource leak is identified
            GC.Collect();

            var objectFound = false;
            //Dictionary<string, ObjectNode> objectTable = new Dictionary<string, ObjectNode>();
            var objects = new List<ObjectNode>();
            var currentObject = new ObjectNode();
            currentObject.Name = "Default";

            var triangleCount = 0;
            var vertexCount = 0;

 //           List<Mesh.Group> matGroups = new List<Mesh.Group>();

            var vertexList = new List<PositionNormalTextured>();
            var vertList = new List<Vector3>();
            var normList = new List<Vector3>();
            var uvList = new List<Vector2>();

            vertList.Add(new Vector3());
            normList.Add(new Vector3());
            uvList.Add(new Vector2());


            var indexList = new List<int>();
            var attribList = new List<int>();
            var applyLists = new List<int[]>();
            var applyListsIndex = new List<int>();
            var materialNames = new List<string>();
            var currentMaterialIndex = -1;
            var currentMaterial = new Material();
            var currentGroup = new Mesh.Group();
        

            var currentIndex = 0;


            //initialize the default material

            currentMaterial = new Material();
            currentMaterial.Diffuse = Color;
            currentMaterial.Ambient = Color;
            currentMaterial.Specular = Color.White;
            currentMaterial.SpecularSharpness = 30.0f;
            currentMaterial.Opacity = 1.0f;
            currentMaterial.Default = true;

            //initialize the group
            currentGroup.startIndex = 0;
            currentGroup.indexCount = 0;
            currentGroup.materialIndex = 0;

            using (Stream fs = new FileStream(filename, FileMode.Open,FileAccess.Read))
            {
                var sr = new StreamReader(fs);
          
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Replace("  ", " ");

                    var parts = line.Trim().Split(new[] { ' ' });

                    if (parts.Length > 0)
                    {
                        switch (parts[0])
                        {
                            case "mtllib":
                                {
                                    var path = filename.Substring(0, filename.LastIndexOf('\\') + 1);
                                    var matFile = path + "\\" + parts[1];
                                    LoadMatLib(matFile);
                                }
                                break;
                            case "usemtl":
                                var materialName = parts[1];
                                if (matLib.ContainsKey(materialName))
                                {
                                    if (currentMaterialIndex == -1 && currentIndex > 0)
                                    {
                                        addMaterial(currentMaterial);
                                        currentMaterialIndex++;
                                    }

                                    if (currentMaterialIndex > -1)
                                    {
                                        currentGroup.indexCount = currentIndex - currentGroup.startIndex;
                                  //      matGroups.Add(currentGroup);
                                        currentObject.DrawGroup.Add(currentGroup);
                                    }

                                    currentMaterialIndex++;

                                    if (matLib.ContainsKey(materialName))
                                    {
                                        currentMaterial = matLib[materialName];


                                        if (textureLib.ContainsKey(materialName))
                                        {
                                            try
                                            {
                                                if (!TextureCache.ContainsKey(textureLib[materialName]))
                                                {
                                                    var path = filename.Substring(0, filename.LastIndexOf('\\') + 1);

                                                    var tex = LoadTexture(path + textureLib[materialName]);
                                                    if (tex != null)
                                                    {
                                                        meshFilenames.Add(textureLib[materialName]);
                                                        TextureCache.Add(textureLib[materialName], tex);
                                                    }
                                                }
                                                meshTextures.Add(TextureCache[textureLib[materialName]]);
                                            }
                                            catch
                                            {
                                            }
                                        }

                                        addMaterial(currentMaterial);
                                        
                                        currentGroup = new Mesh.Group();
                                        currentGroup.startIndex = currentIndex;
                                        currentGroup.indexCount = 0;
                                        currentGroup.materialIndex = currentMaterialIndex;
                                    }
                                }

                                break;
                            case "v":
                                vertexCount++;
                                if (FlipHandedness)
                                {
                                    vertList.Add(new Vector3(-float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                                }
                                else
                                {
                                    vertList.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                                }
                                break;
                            case "vn":
                                if (FlipHandedness)
                                {
                                    normList.Add(new Vector3(-float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                                }
                                else
                                {
                                    normList.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                                }
                                break;
                            case "vt":
                                uvList.Add(new Vector2(float.Parse(parts[1]), FlipV ? (1 - float.Parse(parts[2])) : float.Parse(parts[2])));
                                break;
                            case "g":
                            case "o":
                                if (objectFound)
                                {
                                    if (currentMaterialIndex > -1)
                                    {
                                        currentGroup.indexCount = currentIndex - currentGroup.startIndex;
                       //                 matGroups.Add(currentGroup);
                                        currentObject.DrawGroup.Add(currentGroup);
                                        currentGroup = new Mesh.Group();
                                        currentGroup.startIndex = currentIndex;
                                        currentGroup.indexCount = 0;
                                        currentGroup.materialIndex = currentMaterialIndex;
                                    }
                                    currentObject = new ObjectNode();
                                }
                                
                                objectFound = true;
                                if (parts.Length > 1)
                                {
                                    currentObject.Name = parts[1];
                                }
                                else
                                {
                                    currentObject.Name = "Unnamed";
                                }
                                objects.Add(currentObject);
                                //if (!objectTable.ContainsKey(currentObject.Name))
                                //{
                                //    objectTable.Add(currentObject.Name, currentObject);
                                //}
                                break;
                            case "f":
                                var indexiesA = GetIndexies(parts[1]);
                                var indexiesB = GetIndexies(parts[2]);
                                var indexiesC = GetIndexies(parts[3]);

                                vertexList.Add(new PositionNormalTextured(vertList[indexiesA[0]], normList[indexiesA[2]], uvList[indexiesA[1]]));
                                vertexList.Add(new PositionNormalTextured(vertList[indexiesB[0]], normList[indexiesB[2]], uvList[indexiesB[1]]));
                                vertexList.Add(new PositionNormalTextured(vertList[indexiesC[0]], normList[indexiesC[2]], uvList[indexiesC[1]]));

                                if (FlipHandedness)
                                {
                                    indexList.Add(currentIndex);
                                    indexList.Add(currentIndex + 2);
                                    indexList.Add(currentIndex + 1);
                                }
                                else
                                {
                                    indexList.Add(currentIndex);
                                    indexList.Add(currentIndex + 1);
                                    indexList.Add(currentIndex + 2);  

                                }

                                triangleCount++;
                                currentIndex += 3;
                                //bool flip = true;
                                if (parts.Length > 4)
                                {
                                    var partIndex = 4;

                                    while (partIndex < (parts.Length))
                                    {
                                        if (FlipHandedness)
                                        {
                                            indexiesA = GetIndexies(parts[1]);
                                            indexiesC = GetIndexies(parts[partIndex]);          
                                            indexiesB = GetIndexies(parts[partIndex - 1]);
                                        }
                                        else
                                        {
                                            indexiesA = GetIndexies(parts[1]);
                                            indexiesB = GetIndexies(parts[partIndex - 1]);
                                            indexiesC = GetIndexies(parts[partIndex]);
                                        }
                                        vertexList.Add(new PositionNormalTextured(vertList[indexiesA[0]], normList[indexiesA[2]], uvList[indexiesA[1]]));
                                        vertexList.Add(new PositionNormalTextured(vertList[indexiesB[0]], normList[indexiesB[2]], uvList[indexiesB[1]]));
                                        vertexList.Add(new PositionNormalTextured(vertList[indexiesC[0]], normList[indexiesC[2]], uvList[indexiesC[1]]));


                                        indexList.Add(currentIndex);
                                        indexList.Add(currentIndex + 1);
                                        indexList.Add(currentIndex + 2);
                                        triangleCount++;

                                        currentIndex += 3;
                                        partIndex++;
                                    }
                                }
                                break;
                        }
                    }
                }
            }

            if (!objectFound)
            {
                // add the default object
                objects.Add(currentObject);
            }

            if (currentMaterialIndex == -1 && currentIndex > 0)
            {
                addMaterial(currentMaterial);
                currentMaterialIndex++;
            }

            if (currentMaterialIndex > -1)
            {
                currentGroup.indexCount = currentIndex - currentGroup.startIndex;
                currentObject.DrawGroup.Add(currentGroup);
            }

            if (normList.Count < 2)
            {

                var creaseAngleRad = MathUtil.DegreesToRadians(Smooth ? 170.0f : 45.0f);

                var vertexNormals = CalculateVertexNormalsMerged(vertexList, indexList, creaseAngleRad);
                var newVertexList = new List<PositionNormalTextured>();
                var newVertexCount = indexList.Count;

                for (var vertexIndex = 0; vertexIndex < newVertexCount; ++vertexIndex)
                {
                    var v = vertexList[indexList[vertexIndex]];
                    v.Normal = vertexNormals[vertexIndex];
                    newVertexList.Add(v);
                }

                vertexList = newVertexList;
            }




            mesh = new Mesh(vertexList.ToArray(), indexList.ToArray());
            var rootDummy = new ObjectNode();
            rootDummy.Name = "Root";
            rootDummy.Parent = null;
            rootDummy.Level = -1;
            rootDummy.DrawGroup = null;
            rootDummy.Children = objects;

            Objects = new List<ObjectNode>();
            Objects.Add(rootDummy);


            mesh.setObjects(Objects);

            //List<ObjectNode> objects = new List<ObjectNode>();

            //ObjectNode node = new ObjectNode();
            //node.Name = "Default";
            //node.DrawGroup = matGroups;
            //objects.Add(node);
            //mesh.setObjects(objects);
            //Objects = objects;


            mesh.commitToDevice(RenderContext11.PrepDevice);

            dirty = false;

            GC.Collect();
        }

        private Texture11 LoadTexture(string filename)
        {
            if (!File.Exists(filename))
            {
                var newfilename = FindFile(filename);
                if (string.IsNullOrEmpty(newfilename))
                {
                    newfilename = FindFileFuzzy(filename);
                }

                filename = newfilename;
            }

            if (string.IsNullOrEmpty(filename))
            {
                return null;
            }

            return Texture11.FromFile(RenderContext11.PrepDevice, filename);
        }

        public static string FindFileFuzzy(string filename)
        {
            filename = filename.ToLower();
            var path = filename.Substring(0, filename.LastIndexOf('\\') + 1);
            var file = filename.Substring(filename.LastIndexOf("\\") + 1);
            if (file.Contains("."))
            {
                file = file.Substring(0, file.LastIndexOf('.') );
            }

            var ext = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();

            foreach (var f in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
            {
                var fb = f.Substring(f.LastIndexOf("\\") + 1).ToLower();
                var fe = ""; 
                var ff = "";
                if (f.Contains("."))
                {
                    fe = fb.Substring(fb.LastIndexOf(".") + 1);
                    ff = fb.Substring(0, fb.LastIndexOf("."));
                }
                if (string.Compare(file, 0, ff, 0, file.Length) == 0 && ((fe == ext) || (fe == "png") || (fe == "tif") || (fe == "tiff") || (fe == "bmp") || (fe == "jpg") || (fe == "jpeg") || (fe == "tga") || (fe == "jfif") || (fe == "rla")))
                {
                    return f;
                }
            }


            //foreach (string dir in Directory.GetDirectories(path))
            //{
            //    string child = dir + @"\" + file;
            //    if (File.Exists(child))
            //    {
            //        return child;
            //    }
            //    FindFileFuzzy(child);
            //}

            return null;
        }

        public static string FindFile(string filename)
        {
            var path = filename.Substring(0, filename.LastIndexOf('\\') + 1);
            var file = filename.Substring(filename.LastIndexOf("\\") + 1);

            foreach (var dir in Directory.GetDirectories(path))
            {
                var child = dir + @"\" + file;
                if (File.Exists(child))
                {
                    return child;
                }
                FindFile(child);
            }

            return null;
        }

        public List<ObjectNode> Objects = new List<ObjectNode>();

        Dictionary<string, Material> matLib = new Dictionary<string, Material>();
        Dictionary<string, string> textureLib = new Dictionary<string, string>();
        void LoadMatLib(string filename)
        {
            if (!File.Exists(filename))
            {
                return;
            }

            meshFilenames.Add(filename.Substring(filename.LastIndexOf("\\") + 1));

            try
            {
                var currentMaterial = new Material();
                var materialName = "";

                matLib = new Dictionary<string, Material>();
                textureLib = new Dictionary<string, string>();


                using (Stream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    var sr = new StreamReader(fs);

                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();

                        var parts = line.Trim().Split(new[] { ' ' });

                        if (parts.Length > 0)
                        {
                            switch (parts[0])
                            {
                                case "newmtl":
                                    if (!string.IsNullOrEmpty(materialName))
                                    {
                                        matLib.Add(materialName, currentMaterial);
                                    }

                                    currentMaterial = new Material();
                                    currentMaterial.Diffuse = Color.White;
                                    currentMaterial.Ambient = Color.White;
                                    currentMaterial.Specular = Color.Black;
                                    currentMaterial.SpecularSharpness = 30.0f;
                                    currentMaterial.Opacity = 1.0f;
                                    materialName = parts[1];
                                    break;
                                case "Ka":
                                    currentMaterial.Ambient = Color.FromArgb((int)(Math.Min(float.Parse(parts[1]) * 255, 255)), (int)(Math.Min(float.Parse(parts[2]) * 255, 255)), (int)(Math.Min(float.Parse(parts[3]) * 255, 255)));
                                    break;
                                case "map_Kd":
                                    //ENDURE TEXTURES ARE NOT BLACK!    
                                    currentMaterial.Diffuse = Color.White;

                                    var textureFilename = parts[1];
                                    for (var i = 2; i < parts.Length; i++)
                                    {
                                        textureFilename += " " + parts[i];
                                    }
                                    var path = filename.Substring(0, filename.LastIndexOf('\\') + 1);

                                    textureFilename = textureFilename.Replace("/", "\\");
                                    if (textureFilename.Contains("\\"))
                                    {
                                        textureFilename = textureFilename.Substring(textureFilename.LastIndexOf("\\") + 1);
                                    }

                                    //if (File.Exists(path + "\\" + textureFilename))
                                    {
                                        //textureLib.Add(materialName, path + "\\" + textureFilename);
                                        textureLib.Add(materialName, textureFilename);
                                    }
                                    break;
                                case "Kd":
                                    currentMaterial.Diffuse = Color.FromArgb((int)(Math.Min(float.Parse(parts[1]) * 255, 255)), (int)(Math.Min(float.Parse(parts[2]) * 255, 255)), (int)(Math.Min(float.Parse(parts[3]) * 255, 255)));
                                    break;
                                case "Ks":
                                    currentMaterial.Specular = Color.FromArgb((int)(Math.Min(float.Parse(parts[1]) * 255,255)), (int)(Math.Min(float.Parse(parts[2]) * 255,255)), (int)(Math.Min(float.Parse(parts[3]) * 255,255)));
                                    break;
                                case "d":
                                    // Where does this map?
                                    currentMaterial.Opacity = float.Parse(parts[1]);
                                    break;
                                case "Tr":
                                    // Where does this map?
                                    currentMaterial.Opacity = 1-float.Parse(parts[1]);
                                    break;

                                case "illum":
                                    // Where does this map?
                                    var illuminationMode = int.Parse(parts[1]);
                                    break;

                                case "sharpness":
                                    currentMaterial.SpecularSharpness = float.Parse(parts[1]);
                                    break;
                                case "Ns":
                                    currentMaterial.SpecularSharpness = 1.0f + 2 * float.Parse(parts[1]);
                                    currentMaterial.SpecularSharpness = Math.Max(10.0f, currentMaterial.SpecularSharpness);
                                    break;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(materialName))
                {
                    matLib.Add(materialName, currentMaterial);
                }
            }
            catch
            {
            }
        }

        int[] GetIndexies(string data)
        {
            var parts = data.Trim().Split(new[] { '/' });
            var indecies = new int[3];

            if (string.IsNullOrEmpty(data))
            {
                return indecies;
            }

            if (parts.Length > 0)
            {
                indecies[0] = int.Parse(parts[0]);
            }
            if (parts.Length > 1)
            {
                if (string.IsNullOrEmpty(parts[1]))
                {
                    indecies[1] = 0;
                }
                else
                {
                    indecies[1] = int.Parse(parts[1]);
                }
            }
            if (parts.Length > 2)
            {
                indecies[2] = int.Parse(parts[2]);
            }

            return indecies;
        }

        private void LoadMeshFrom3ds(string filename, float scale)
        {
            // Force garbage collection to ensure that unmanaged resources are released.
            // Temporary workaround until unmanaged resource leak is identified
            GC.Collect();

            int i;

            ushort sectionID;
            uint sectionLength;

            var name = "";
            var material = "";
            var triangleCount = 0;
            var vertexCount = 0;
            var vertexList = new List<PositionNormalTextured>();
            var indexList = new List<int>();
            var attribList = new List<int>();
            //List<int[]> applyLists = new List<int[]>();
            //List<int> applyListsIndex = new List<int>();
            var materialNames = new List<string>();
            var currentMaterialIndex = -1;
            var currentMaterial = new Material();
            var attributeID = 0;

            var count = 0;
            UInt16 lastID = 0;
            var exit = false;
            var normalMapFound = false;

            float offsetX = 0;
            float offsetY = 0;
            float offsetZ = 0;
            var objects = new List<ObjectNode>();
            ObjectNode currentObject = null;
            var objHierarchy = new List<int>();
            var objNames = new List<string>();
            var objectTable = new Dictionary<string, ObjectNode>();

            var dummyCount = 0;

            using (Stream fs = new FileStream(filename, FileMode.Open))
            {
                var br = new BinaryReader(fs);
                var length = fs.Length - 1;
                var startMapIndex = 0;
                var startTriangleIndex = 0;
                while (br.BaseStream.Position < length && !exit) //Loop to scan the whole file
                {
                    sectionID = br.ReadUInt16();
                    sectionLength = br.ReadUInt32();


                    switch (sectionID)
                    {

                        //This section the begining of the file
                        case 0x4d4d:
                            break;

                        // This section marks the edit section containing the 3d models (3d3d get it? very punny!)
                        case 0x3d3d:
                            break;

                        // Object section contains meshes, etc.
                        case 0x4000:
                            name = "";
                            Byte b;
                            do
                            {
                                b = br.ReadByte();
                                if (b > 0)
                                {
                                    name += (char)b;
                                }

                            } while (b != '\0');

                            currentObject = new ObjectNode();
                            currentObject.Name = name;
                            objects.Add(currentObject);
                            if (!objectTable.ContainsKey(currentObject.Name))
                            {
                                objectTable.Add(currentObject.Name, currentObject);
                            }
 
                            break;

                        // Marks the start of a mesh section
                        case 0x4100:
                            startMapIndex = vertexList.Count;
                            startTriangleIndex = indexList.Count / 3;
                            break;

                        // This section has the vertex list.. Maps to Vertext buffer in Direct3d
                        case 0x4110:
                            vertexCount = br.ReadUInt16();

                            for (i = 0; i < vertexCount; i++)
                            {
                                var x = br.ReadSingle() - offsetX;
                                var y = br.ReadSingle() - offsetY;
                                var z = br.ReadSingle() - offsetZ;

                                var vert = new PositionNormalTextured(x * scale, z * scale, y * scale, 0, 0, 0, 0, 0);
                                vertexList.Add(vert);
                            }
                            break;

                        // This section is a tiangle index list. Maps to Index Buffer in Direct3d
                        case 0x4120:
                            {
                                int triCount = br.ReadUInt16();
                                triangleCount += triCount;

                                for (i = 0; i < triCount; i++)
                                {
                                    var aa = br.ReadUInt16() + startMapIndex;
                                    var bb = br.ReadUInt16() + startMapIndex;
                                    var cc = br.ReadUInt16() + startMapIndex;
                                    indexList.Add(cc);
                                    indexList.Add(bb);
                                    indexList.Add(aa);
                                    var flags = br.ReadUInt16();
                                }
                            }
                            break;

                        // Material for face from start face to triCount
                        case 0x4130:
                            {
                                material = "";
                                i = 0;
                                byte b1;
                                do
                                {
                                    b1 = br.ReadByte();
                                    if (b1 > 0)
                                    {
                                        material += (char)b1;
                                    }

                                    i++;
                                } while (b1 != '\0');
                                int triCount = br.ReadUInt16();
                                var applyList = new int[triCount];

                                attributeID = GetMaterialID(material, materialNames);

                                for (i = 0; i < triCount; i++)
                                {
                                    applyList[i] = br.ReadUInt16() + startTriangleIndex;
                                }
                                currentObject.ApplyLists.Add(applyList);
                                currentObject.ApplyListsIndex.Add(attributeID);

                            }
                            break;

                        // Section for UV texture maps
                        case 0x4140:
                            count = br.ReadUInt16();
                            for (i = 0; i < count; i++)
                            {
                                var vert = vertexList[startMapIndex + i];
                                var texCoord = new Vector2(br.ReadSingle(), FlipV ? (1.0f - br.ReadSingle()) : (br.ReadSingle()));
                                vertexList[startMapIndex + i] = new PositionNormalTextured(vert.Position, Vector3.Zero, texCoord);
                            }
                            break;


                        // Section for Smoothing Groups
                        //case 0x4150:
                        //    count = br.ReadUInt16();
                        //    for (i = 0; i < count; i++)
                        //    {
                        //        CustomVertex.PositionNormalTextured vert = vertexList[startMapIndex + i];
                        //        vertexList[startMapIndex + i] = new CustomVertex.PositionNormalTextured(vert.Position, new Vector3(0,0,0), br.ReadSingle(), FlipV ? (1.0f -  br.ReadSingle() ) : (br.ReadSingle()));
                        //    }
                        //    break;
                        case 0x4160:
                            var mat = new float[12];
                            for (i = 0; i < 12; i++)
                            {
                                mat[i] = br.ReadSingle();
                            }
                            //offsetX = mat[9];
                            //offsetY = mat[11];
                            //offsetZ = mat[10];

                            if (objectTable.ContainsKey(name))
                            {
                                objectTable[name].LocalMat = new Matrix3d(
                                    mat[0], mat[1], mat[2], 0,
                                    mat[3], mat[4], mat[5], 0,
                                    mat[6], mat[7], mat[8], 0,
                                    mat[9], mat[10], mat[11], 1);

                                objectTable[name].LocalMat.Invert();

                                //objectTable[name].PivotPoint = new Vector3(mat[9]*mat[0],mat[10]*mat[1],mat[11]*mat[2]);
                            }

                            break;
                        // Materials library section
                        case 0xAFFF:
                            break;
                        // Material Name
                        case 0xA000:
                            {
                                var matName = "";
                                i = 0;
                                byte b2;
                                do
                                {
                                    b2 = br.ReadByte();
                                    if (b2 > 0)
                                    {
                                        matName += (char)b2;
                                    }

                                    i++;
                                } while (b2 != '\0');
                                materialNames.Add(matName);

                                if (currentMaterialIndex > -1)
                                {
                                    addMaterial(currentMaterial);
                                }

                                currentMaterialIndex++;

                                currentMaterial = new Material();
                                currentMaterial.Diffuse = Color.White;
                                currentMaterial.Ambient = Color.White;
                                currentMaterial.Specular = Color.Black;
                                currentMaterial.SpecularSharpness = 30.0f;
                                currentMaterial.Opacity = 1.0f;
                            }
                            break;

                        // Ambient color
                        case 0xA010:
                            currentMaterial.Ambient = LoadColorChunk(br);
                            break;

                        // Diffuse color
                        case 0xA020:
                            currentMaterial.Diffuse = LoadColorChunk(br);
                            break;

                        // Specular color
                        case 0xA030:
                            currentMaterial.Specular = LoadColorChunk(br);
                            break;

                        // Specular power
                        case 0xA040:
                            // This is just a reasonable guess at the mapping from percentage to 
                            // specular exponent used by 3D Studio.
                            currentMaterial.SpecularSharpness = 1.0f + 2 * LoadPercentageChunk(br);

                            // Minimum sharpness of 10 enforced here because of bad specular exponents
                            // in ISS model.
                            // TODO: Fix ISS and permit lower specular exponents here
                            currentMaterial.SpecularSharpness = Math.Max(10.0f, currentMaterial.SpecularSharpness);
                            break;

                        //Texture map file 
                        case 0xA200:
                            break;

                        // Texture file name
                        case 0xA300:
                            {
                                var textureFilename = "";
                                i = 0;
                                byte b2;
                                do
                                {
                                    b2 = br.ReadByte();
                                    if (b2 > 0)
                                    {
                                        textureFilename += (char)b2;
                                    }

                                    i++;
                                } while (b2 != '\0');
                                var path = filename.Substring(0, filename.LastIndexOf('\\') + 1);
                                try
                                {
                                    var tex = LoadTexture(path + textureFilename);

                                    if (tex != null)
                                    {
                                        meshTextures.Add(tex);
                                        meshFilenames.Add(textureFilename);

                                        // The ISS model has black for the diffuse color; to work around this
                                        // we'll set the diffuse color to white when there's a texture present.
                                        // The correct fix is to modify the 3ds model of ISS.
                                        currentMaterial.Diffuse = Color.White;
                                    }
                                    else
                                    {
                                        meshTextures.Add(null);
                                    }
                                }
                                catch
                                {
                                    meshTextures.Add(null);
                                }
                            }
                            break;


                        // Bump map
                        case 0xA230:
                            {
                                var percentage = LoadPercentageChunk(br);

                                int nameId = br.ReadUInt16();
                                var nameBlockLength = br.ReadUInt32();
                                var textureFilename = "";
                                i = 0;
                                byte b2;
                                do
                                {
                                    b2 = br.ReadByte();
                                    if (b2 > 0)
                                    {
                                        textureFilename += (char)b2;
                                    }

                                    i++;
                                } while (b2 != '\0');

                                var path = filename.Substring(0, filename.LastIndexOf('\\') + 1);
                                try
                                {
                                    var tex = LoadTexture(path + textureFilename);

                                    if (tex != null)
                                    {
                                        meshNormalMaps.Add(tex);
                                        meshFilenames.Add(textureFilename);

                                        // Indicate that we have a normal map so that we know to generate tangent vectors for the mesh
                                        normalMapFound = true;
                                    }
                                    else
                                    {
                                        meshNormalMaps.Add(null);
                                    }

                                }
                                catch
                                {
                                    meshNormalMaps.Add(null);
                                }
                            }

                            break;

                        // Specular map
                        case 0xA204:
                            {
                                var strength = LoadPercentageChunk(br);

                                int nameId = br.ReadUInt16();
                                var nameBlockLength = br.ReadUInt32();
                                var textureFilename = "";
                                i = 0;
                                byte b2;
                                do
                                {
                                    b2 = br.ReadByte();
                                    if (b2 > 0)
                                    {
                                        textureFilename += (char)b2;
                                    }

                                    i++;
                                } while (b2 != '\0');

                                var path = filename.Substring(0, filename.LastIndexOf('\\') + 1);
                                try
                                {
                                    var tex = LoadTexture(path + textureFilename);
                                    if (tex != null)
                                    {
                                        meshSpecularTextures.Add(tex);
                                        meshFilenames.Add(textureFilename);

                                        // Set the current specular color from the specular texture strength
                                        var gray = (int)(255.99f * strength / 100.0f);
                                        currentMaterial.Specular = Color.FromArgb(255, gray, gray, gray);
                                    }
                                    else
                                    {
                                        meshSpecularTextures.Add(null);
                                    }
                                }
                                catch
                                {
                                    meshSpecularTextures.Add(null);
                                }
                            }

                            break;
                        case 0xB000:
                            break;
                        case 0xB002:
                            break;
                        case 0xB010:
                            {
                                name = "";
                                i = 0;
                                byte b1;
                                do
                                {
                                    b1 = br.ReadByte();
                                    if (b1 > 0)
                                    {
                                        name += (char)b1;
                                    }

                                    i++;
                                } while (b1 != '\0');
                                var dum1 = (int)br.ReadUInt16();
                                var dum2 = (int)br.ReadUInt16();
                                var level = (int)br.ReadUInt16();

                                if (level == 65535)
                                {
                                    level = -1;
                                }
                                if (name.StartsWith("$"))
                                {
                                    dummyCount++;

                                }
                                else
                                {
                                    objNames.Add(name);
                                }
                                objHierarchy.Add(level);

                                if (objectTable.ContainsKey(name))
                                {

                                    objectTable[name].Level = level;
                                }
                            }
                            break;
                        case 0xB011:
                            {
                                name = "";
                                i = 0;
                                byte b1;
                                do
                                {
                                    b1 = br.ReadByte();
                                    if (b1 > 0)
                                    {
                                        name += (char)b1;
                                    }

                                    i++;
                                } while (b1 != '\0');
                                objNames.Add( "$$$" + name);
                            }
                            break;
                        case 0xB013:
                            //pivot point
                            var points = new float[3];
                            for (i = 0; i < 3; i++)
                            {
                                points[i] = br.ReadSingle();
                            }


                            if (objectTable.ContainsKey(name))
                            {
                                   objectTable[name].PivotPoint = -new Vector3(points[0], points[1], points[2]);
                            }
                            break;
                        case 0xB020:
                            {
                                var pos = new float[8];
                                for (i = 0; i < 8; i++)
                                {
                                    pos[i] = br.ReadSingle();
                                }

                            }
                            break;

                        // If we don't recognize a section then jump over it. Subract the header from the section length
                        default:
                            br.BaseStream.Seek(sectionLength - 6, SeekOrigin.Current);
                            break;
                    }
                    lastID = sectionID;
                }
                br.Close();
                if (currentMaterialIndex > -1)
                {
                    addMaterial(currentMaterial);
                }
            }

            ////debug

            //for ( i = 0; i < 99; i++)
            //{
            //    System.Diagnostics.Debug.WriteLine(objNames[i]);
            //}

            //foreach (ObjectNode node in objects)
            //{
            //    System.Diagnostics.Debug.WriteLine(node.Name);
            //}

            ////debug




            // Generate vertex normals

            // Vertex normals are computed by averaging the normals of all faces
            // with an angle between them less than the crease angle. By setting
            // the crease angle to 0 degrees, the model will have a faceted appearance.
            // Right now, the smooth flag selects between one of two crease angles,
            // but some smoothing is always applied.
            var creaseAngleRad = MathUtil.DegreesToRadians(Smooth ? 70.0f : 45.0f);

            var vertexNormals = CalculateVertexNormalsMerged(vertexList, indexList, creaseAngleRad);
            var newVertexList = new List<PositionNormalTextured>();
            var newVertexCount = triangleCount * 3;

            for (var vertexIndex = 0; vertexIndex < newVertexCount; ++vertexIndex)
            {
                var v = vertexList[indexList[vertexIndex]];
                v.Normal = vertexNormals[vertexIndex];
                newVertexList.Add(v);
            }


            // Use the triangle mesh and material assignments to create a single
            // index list for the mesh.
            var newIndexList = new List<uint>();

            foreach (var node in objects)
            {


                var materialGroups = new List<Mesh.Group>();
                for (i = 0; i < node.ApplyLists.Count; i++)
                {
                    var matId = node.ApplyListsIndex[i];
                    var startIndex = newIndexList.Count;
                    foreach (var triangleIndex in node.ApplyLists[i])
                    {
                        newIndexList.Add((uint)(triangleIndex * 3));
                        newIndexList.Add((uint)(triangleIndex * 3 + 1));
                        newIndexList.Add((uint)(triangleIndex * 3 + 2));
                    }

                    var group = new Mesh.Group();
                    group.startIndex = startIndex;
                    group.indexCount = node.ApplyLists[i].Length * 3;
                    group.materialIndex = matId;
                    materialGroups.Add(group);
                }
                node.DrawGroup = materialGroups;
            }

            // Turn objects into tree
            var nodeStack = new Stack<ObjectNode>();

            var nodeTreeRoot = new List<ObjectNode>();

            var rootDummy = new ObjectNode();
            rootDummy.Name = "Root";
            rootDummy.Parent = null;
            rootDummy.Level = -1;
            rootDummy.DrawGroup = null;

            var currentLevel = -1;

            nodeStack.Push(rootDummy);
            nodeTreeRoot.Add(rootDummy);

            for (i = 0; i < objHierarchy.Count; i++)
            {
                var level = objHierarchy[i];

                if (level <= currentLevel)
                {
                    // pop out all the nodes to intended parent
                    while (level <= nodeStack.Peek().Level && nodeStack.Count > 1)
                    {
                        nodeStack.Pop();
                    }
                    currentLevel = level;

                }

                if (objNames[i].StartsWith("$$$"))
                {
                    var dummy = new ObjectNode();
                    dummy.Name = objNames[i].Replace("$$$", "");
                    dummy.Parent = nodeStack.Peek();
                    dummy.Parent.Children.Add(dummy);
                    dummy.Level = currentLevel = level;
                    dummy.DrawGroup = null;
                    nodeStack.Push(dummy);
                }
                else
                {
                    objectTable[objNames[i]].Level = currentLevel = level;
                    objectTable[objNames[i]].Parent = nodeStack.Peek();
                    objectTable[objNames[i]].Parent.Children.Add(objectTable[objNames[i]]);
                    nodeStack.Push(objectTable[objNames[i]]);
                }
            }

            if (objHierarchy.Count == 0)
            {
                foreach (var node in objects)
                {
                    rootDummy.Children.Add(node);
                    node.Parent = rootDummy;
                }
            }


            if (normalMapFound)
            {
                // If we've got a normal map, we want to generate tangent vectors for the mesh

                // Mapping of vertices to geometry is extremely straightforward now, but this could
                // change when a mesh optimization step is introduced.
                var tangentIndexList = new List<uint>();
                for (uint tangentIndex = 0; tangentIndex < newVertexCount; ++tangentIndex)
                {
                    tangentIndexList.Add(tangentIndex);
                }

                var tangents = CalculateVertexTangents(newVertexList, tangentIndexList, creaseAngleRad);

                // Copy the tangents in the vertex data list
                var vertices = new PositionNormalTexturedTangent[newVertexList.Count];
                var vertexIndex = 0;
                foreach (var v in newVertexList)
                {
                    var tvertex = new PositionNormalTexturedTangent(v.Position, v.Normal, new Vector2(v.Tu, v.Tv), tangents[vertexIndex]);
                    vertices[vertexIndex] = tvertex;
                    ++vertexIndex;
                }
                mesh = new Mesh(vertices, newIndexList.ToArray());
            }
            else
            {
                mesh = new Mesh(newVertexList.ToArray(), newIndexList.ToArray());
            }

            Objects = nodeTreeRoot;
            mesh.setObjects(nodeTreeRoot);
            mesh.commitToDevice(RenderContext11.PrepDevice);

            dirty = false;

            GC.Collect();
        }

        private void OffsetObjects(List<PositionNormalTextured> vertList, List<ObjectNode> objects, Matrix3d offsetMat, Vector3 offsetPoint)
        {
            foreach (var node in objects)
            {
                var matLoc = node.LocalMat; //offsetMat *;

                OffsetObjects(vertList, node.Children, matLoc, node.PivotPoint + offsetPoint);

                foreach (var group in node.DrawGroup)
                {
                    var end = group.startIndex + group.indexCount;
                    for (var i = group.startIndex; i < end; i++)
                    {
                        var vert = vertList[i];
                        vert.Pos += (node.PivotPoint + offsetPoint);
                        vertList[i] = vert;
                    }
                }

            }
        }


        private static int GetMaterialID(string material, List<string> materialNames)
        {
            return materialNames.FindIndex(delegate(String target) { return target == material; });
        }


        // Set up lighting state to account for:
        //   - Light reflected from a nearby planet
        //   - Shadows cast by nearby planets
        public void SetupLighting(RenderContext11 renderContext)
        {
            var objPosition = new Vector3d(renderContext.World.OffsetX, renderContext.World.OffsetY, renderContext.World.OffsetZ);
            var objToLight = objPosition - renderContext.ReflectedLightPosition;
            var sunPosition = renderContext.SunPosition - renderContext.ReflectedLightPosition;
            var cosPhaseAngle = sunPosition.Length() <= 0.0 ? 1.0 : Vector3d.Dot(objToLight, sunPosition) / (objToLight.Length() * sunPosition.Length());
            var reflectedLightFactor = (float)Math.Max(0.0, cosPhaseAngle);
            reflectedLightFactor = (float)Math.Sqrt(reflectedLightFactor); // Tweak falloff of reflected light
            var hemiLightFactor = 0.0f;

            // 1. Reduce the amount of sunlight when the object is in the shadow of a planet
            // 2. Introduce some lighting due to scattering by the planet's atmosphere if it's
            //    close to the surface.
            var sunlightFactor = 1.0;
            if (renderContext.OccludingPlanetRadius > 0.0)
            {
                var objAltitude = (objPosition - renderContext.OccludingPlanetPosition).Length() - renderContext.OccludingPlanetRadius;
                hemiLightFactor = (float)Math.Max(0.0, Math.Min(1.0, 1.0 - (objAltitude / renderContext.OccludingPlanetRadius) * 300));
                reflectedLightFactor *= (1.0f - hemiLightFactor);

                // Compute the distance from the center of the object to the line between the sun and occluding planet
                // We're assuming that the radius of the object is very small relative to Earth;
                // for large objects the amount of shadow will vary, and we should use circular
                // eclipse shadows.
                var sunToPlanet = renderContext.OccludingPlanetPosition - renderContext.SunPosition;
                var objToPlanet = renderContext.OccludingPlanetPosition - objPosition;

                var hemiLightDirection = -objToPlanet;
                hemiLightDirection.Normalize();
                renderContext.HemisphereLightUp = hemiLightDirection;

                var objToSun = renderContext.SunPosition - objPosition;
                var sunPlanetDistance = sunToPlanet.Length();
                var t = -Vector3d.Dot(objToSun, sunToPlanet) / (sunPlanetDistance * sunPlanetDistance);
                if (t > 1.0)
                {
                    // Object is on the side of the planet opposite the sun, so a shadow is possible

                    // Compute the position of the object projected onto the shadow axis
                    var shadowAxisPoint = Vector3d.Add(renderContext.SunPosition, Vector3d.Multiply(sunToPlanet, t));

                    // d is the distance to the shadow axis
                    var d = (shadowAxisPoint - objPosition).Length();

                    // s is the distance from the sun along the shadow axis
                    var s = (shadowAxisPoint - renderContext.SunPosition).Length();

                    // Use the sun's radius to accurately compute the penumbra and umbra cones
                    const double solarRadius = 0.004645784; // AU
                    var penumbraRadius = renderContext.OccludingPlanetRadius + (t - 1.0) * (renderContext.OccludingPlanetRadius + solarRadius);
                    var umbraRadius = renderContext.OccludingPlanetRadius + (t - 1.0) * (renderContext.OccludingPlanetRadius - solarRadius);

                    if (d < penumbraRadius)
                    {
                        // The object is inside the penumbra, so it is at least partly shadowed
                        var minimumShadow = 0.0;
                        if (umbraRadius < 0.0)
                        {
                            // No umbra at this point; degree of shadowing is limited because the
                            // planet doesn't completely cover the sun even when the object is positioned
                            // exactly on the shadow axis.
                            var occlusion = Math.Pow(1.0 / (1.0 - umbraRadius), 2.0);
                            umbraRadius = 0.0;
                            minimumShadow = 1.0 - occlusion;
                        }

                        // Approximate the amount of shadow with linear interpolation. The accurate
                        // calculation involves computing the area of the intersection of two circles.
                        var u = Math.Max(0.0, umbraRadius);
                        sunlightFactor = Math.Max(minimumShadow, (d - u) / (penumbraRadius - u));

                        var gray = (int)(255.99f * sunlightFactor);
                        renderContext.SunlightColor = Color.FromArgb(gray, gray, gray);

                        // Reduce sky-scattered light as well
                        hemiLightFactor *= (float)sunlightFactor;
                    }
                }
            }

            renderContext.ReflectedLightColor = Color.FromArgb((int)(renderContext.ReflectedLightColor.R * reflectedLightFactor),
                                                                               (int)(renderContext.ReflectedLightColor.G * reflectedLightFactor),
                                                                               (int)(renderContext.ReflectedLightColor.B * reflectedLightFactor));
            renderContext.HemisphereLightColor = Color.FromArgb((int)(renderContext.HemisphereLightColor.R * hemiLightFactor),
                                                                               (int)(renderContext.HemisphereLightColor.G * hemiLightFactor),
                                                                               (int)(renderContext.HemisphereLightColor.B * hemiLightFactor));
        }

        public const int MAX_VERTICES = 8000;
        public const int MAX_POLYGONS = 8000;



        public bool ISSLayer = false;

        public void Render(RenderContext11 renderContext, float opacity)
        {
            if (dirty && !(ISSLayer))
            {
                Reload();
            }
            var oldWorld = renderContext.World;

            var offset = mesh.BoundingSphere.Center;
            var unitScale = 1.0f;
            if (mesh.BoundingSphere.Radius > 0.0f)
            {
                unitScale = 1.0f / mesh.BoundingSphere.Radius;
            }
            renderContext.World = Matrix3d.Translation(-offset.X, -offset.Y, -offset.Z) * Matrix3d.Scaling(unitScale, unitScale, unitScale) * oldWorld;

            var worldView = renderContext.World * renderContext.View;
            var v = worldView.Transform(Vector3d.Empty);
            var scaleFactor = Math.Sqrt(worldView.M11 * worldView.M11 + worldView.M22 * worldView.M22 + worldView.M33 * worldView.M33) / unitScale;
            var dist = v.Length();
            var radius = scaleFactor;

            // Calculate pixelsPerUnit which is the number of pixels covered
            // by an object 1 AU at the distance of the planet center from
            // the camera. This calculation works regardless of the projection
            // type.
            var viewportHeight = (int)renderContext.ViewPort.Height;
            var p11 = renderContext.Projection.M11;
            var p34 = renderContext.Projection.M34;
            var p44 = renderContext.Projection.M44;
            var w = Math.Abs(p34) * dist + p44;
            var pixelsPerUnit = (float)(p11 / w) * viewportHeight;
            var radiusInPixels = (float)(radius * pixelsPerUnit);
            if (radiusInPixels < 0.5f)
            {
                // Too small to be visible; skip rendering
                return;
            }

            // These colors can be modified by shadows, distance from planet, etc. Restore
            // the original values after rendering.
            var savedSunlightColor = renderContext.SunlightColor;
            var savedReflectedColor = renderContext.ReflectedLightColor;
            var savedHemiColor = renderContext.HemisphereLightColor;

            if (Properties.Settings.Default.SolarSystemLighting)
            {
                SetupLighting(renderContext);
                renderContext.AmbientLightColor = Color.FromArgb(11, 11, 11);
            }
            else
            {
                // No lighting: set ambient light to white and turn off all other light sources
                renderContext.SunlightColor = Color.Black;
                renderContext.ReflectedLightColor = Color.Black;
                renderContext.HemisphereLightColor = Color.Black;
                renderContext.AmbientLightColor = Color.White;
            }

            var device = renderContext.Device;


            if (mesh == null)
            {
                return;
            }


            //Object3dLayer.sketch.DrawLines(renderContext, 1.0f, System.Drawing.Color.Red);

            renderContext.DepthStencilMode = DepthStencilMode.ZReadWrite;
            renderContext.BlendMode = BlendMode.Alpha;

            var count = meshMaterials.Count;

            mesh.beginDrawing(renderContext);
            if (count > 0)
            {
                for (var i = 0; i < meshMaterials.Count; i++)
                {
                    if (meshMaterials[i].Default)
                    {
                        var mat = meshMaterials[i];
                        mat.Diffuse = Color;
                        mat.Ambient = Color;
                        meshMaterials[i] = mat;
                    }
                    // Set the material and texture for this subset
                    renderContext.SetMaterial(meshMaterials[i], meshTextures[i], meshSpecularTextures[i], meshNormalMaps[i], opacity);
                    renderContext.PreDraw();
                    renderContext.setSamplerState(0, renderContext.WrapSampler);
                    mesh.drawSubset(renderContext, i);
                }
            }
            else
            {
                renderContext.PreDraw();
                for (var i = 0; i < meshTextures.Count; i++)
                {
                    var key = new PlanetShaderKey(PlanetSurfaceStyle.Diffuse, false, 0);
                    renderContext.SetupPlanetSurfaceEffect(key, 1.0f);
                    if (meshTextures[i] != null)
                    {
                        renderContext.MainTexture = meshTextures[i];
                    }
                    renderContext.PreDraw();
                    mesh.drawSubset(renderContext, i);
                }
            }





            renderContext.setSamplerState(0, renderContext.ClampSampler);

            renderContext.setRasterizerState(TriangleCullMode.CullClockwise);
            renderContext.World = oldWorld;



            renderContext.SunlightColor = savedSunlightColor;
            renderContext.ReflectedLightColor = savedReflectedColor;
            renderContext.HemisphereLightColor = savedHemiColor;
            renderContext.AmbientLightColor = Color.Black;




        }

        bool dirty = true;

        static void DisposeTextureList(List<Texture11> textures)
        {
            if (textures != null)
            {
                for (var i = 0; i < textures.Count; ++i)
                {
                    if (textures[i] != null)
                    {
                        textures[i].Dispose();
                        GC.SuppressFinalize(textures[i]);
                        textures[i] = null;
                    }
                }

                textures.Clear();
            }
        }

        internal void Dispose()
        {
            if (mesh != null)
            {
                mesh.Dispose();
                GC.SuppressFinalize(mesh);
                mesh = null;
            }

            foreach (var tex in TextureCache.Values)
            {
                if (tex != null)
                {
                    tex.Dispose();
                }
            }
            TextureCache.Clear();

            DisposeTextureList(meshTextures);
            DisposeTextureList(meshSpecularTextures);
            DisposeTextureList(meshNormalMaps);

            meshMaterials.Clear();
            dirty = true;
        }
    }

    public class ObjectNode
    {
        public string Name;
        public int Level = -1;
        public List<ObjectNode> Children = new List<ObjectNode>();
        public ObjectNode Parent;
        public bool Enabled = true;
        public Vector3 PivotPoint;
        public Matrix3d LocalMat;
        public List<Mesh.Group> DrawGroup = new List<Mesh.Group>();
        public List<int[]> ApplyLists = new List<int[]>();
        public List<int> ApplyListsIndex = new List<int>();
    }


    public class Object3dLayerUI : LayerUI
    {
        readonly Object3dLayer layer;
        bool opened = true;

        public Object3dLayerUI(Object3dLayer layer)
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
            if (layer.object3d.Objects.Count > 0 && layer.object3d.Objects[0].Children != null)
            {
                LoadTree(nodes, layer.object3d.Objects[0].Children);
            }
            return nodes;
        }

        void LoadTree(List<LayerUITreeNode> nodes, List<ObjectNode> children)
        {
            foreach (var child in children)
            {
                var node = new LayerUITreeNode();
                node.Name = child.Name;
                node.Tag = child;
                node.Checked = child.Enabled;
                node.NodeSelected += node_NodeSelected;
                node.NodeChecked += node_NodeChecked;
                nodes.Add(node);
                LoadTree(node.Nodes, child.Children);
            }
        }


        void node_NodeChecked(LayerUITreeNode node, bool newState)
        {
            var child = (ObjectNode)node.Tag;

            if (child != null)
            {
                child.Enabled = newState;
            }
        }



        void node_NodeSelected(LayerUITreeNode node)
        {
            if (callbacks != null)
            {
                var child = (ObjectNode)node.Tag;

                var rowData = new Dictionary<string, string>();

                rowData.Add("Name", child.Name);
                rowData.Add("Pivot.X", child.PivotPoint.X.ToString());
                rowData.Add("Pivot.Y", child.PivotPoint.Y.ToString());
                rowData.Add("Pivot.Z", child.PivotPoint.Z.ToString());
                callbacks.ShowRowData(rowData);

                //Object3dLayer.sketch.Clear();
                //Object3dLayer.sketch.AddLine(new Vector3d(0, 0, 0), new Vector3d(child.PivotPoint.X,-child.PivotPoint.Z,child.PivotPoint.Y));
            }
        }

        public override List<LayerUIMenuItem> GetNodeContextMenu(LayerUITreeNode node)
        {
            return base.GetNodeContextMenu(node);
        }
    }
}
