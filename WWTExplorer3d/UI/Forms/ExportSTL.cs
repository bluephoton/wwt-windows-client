﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace TerraViewer
{
    public partial class ExportSTL : Form, IUiController
    {
        public ExportSTL()
        {
            InitializeComponent();
            SetUiStrings();
        }
        private void SetUiStrings()
        {
            label1.Text = Language.GetLocalizedText(241, "North");
            label2.Text = Language.GetLocalizedText(246, "South");
            label3.Text = Language.GetLocalizedText(248, "West");
            label4.Text = Language.GetLocalizedText(243, "East");
            label5.Text = "Density";
            Text = "Export STL File for 3D Printing";
        }

       
        bool initialized;

        private void ExportSTL_Load(object sender, EventArgs e)
        {
            Earth3d.MainWindow.UiController = this;
            lines = new LineList();
            lines.DepthBuffered = false;
            UpdateTextBoxes();
            initialized = true;
            UpdateLines();
        }

        double top;
        double left;
        double right;
        double bottom;
        int density = 100;

        private void UpdateTextBoxes()
        {
            North.Text = Rect.North.ToString();
            South.Text = Rect.South.ToString();
            West.Text = Rect.West.ToString();
            East.Text = Rect.East.ToString();
            Density.Text = density.ToString();
        }

        private void UpdateLines()
        {
            if (!initialized)
            {
                return;
            }
            lines.Clear();

            var width = Rect.East - Rect.West;
            var height = Rect.North - Rect.South;

            var altitude = 1 + Earth3d.MainWindow.GetScaledAltitudeForLatLong((Rect.North + Rect.South) / 2, (Rect.East + Rect.West) / 2);

            var topLeftA = Coordinates.GeoTo3dDouble(Rect.North - height / 20, Rect.West, altitude);
            var topLeftB = Coordinates.GeoTo3dDouble(Rect.North, Rect.West, altitude);
            var topLeftC = Coordinates.GeoTo3dDouble(Rect.North, Rect.West + width / 20, altitude);
            var topMiddleA = Coordinates.GeoTo3dDouble(Rect.North, ((Rect.East + Rect.West) / 2) - width / 20, altitude);
            var topMiddleB = Coordinates.GeoTo3dDouble(Rect.North, ((Rect.East + Rect.West) / 2) + width / 20, altitude);
            var topRightA = Coordinates.GeoTo3dDouble(Rect.North - height / 20, Rect.East, altitude);
            var topRightB = Coordinates.GeoTo3dDouble(Rect.North, Rect.East, altitude);
            var topRightC = Coordinates.GeoTo3dDouble(Rect.North, Rect.East - width / 20, altitude);

            var middleRightA = Coordinates.GeoTo3dDouble((Rect.North + Rect.South) / 2 - height / 20, Rect.West, altitude);
            var middleRightB = Coordinates.GeoTo3dDouble((Rect.North + Rect.South) / 2 + height / 20, Rect.West, altitude);
            var centerA = Coordinates.GeoTo3dDouble((Rect.North + Rect.South) / 2, ((Rect.East + Rect.West) / 2) - width / 20, altitude);
            var centerB = Coordinates.GeoTo3dDouble((Rect.North + Rect.South) / 2, ((Rect.East + Rect.West) / 2) + width / 20, altitude);
            var centerC = Coordinates.GeoTo3dDouble((Rect.North + Rect.South) / 2 - height / 20, ((Rect.East + Rect.West) / 2), altitude);
            var centerD = Coordinates.GeoTo3dDouble((Rect.North + Rect.South) / 2 + height / 20, ((Rect.East + Rect.West) / 2), altitude);
            var middleLeftA = Coordinates.GeoTo3dDouble((Rect.North + Rect.South) / 2 - height / 20, Rect.East, altitude);
            var middleLeftB = Coordinates.GeoTo3dDouble((Rect.North + Rect.South) / 2 + height / 20, Rect.East, altitude);



            var bottomLeftA = Coordinates.GeoTo3dDouble(Rect.South + height / 20, Rect.West, altitude);
            var bottomLeftB = Coordinates.GeoTo3dDouble(Rect.South, Rect.West, altitude);
            var bottomLeftC = Coordinates.GeoTo3dDouble(Rect.South, Rect.West + width / 20, altitude);
            var bottomMiddleA = Coordinates.GeoTo3dDouble(Rect.South, ((Rect.East + Rect.West) / 2) - width / 20, altitude);
            var bottomMiddleB = Coordinates.GeoTo3dDouble(Rect.South, ((Rect.East + Rect.West) / 2) + width / 20, altitude);
            var bottomRightA = Coordinates.GeoTo3dDouble(Rect.South + height / 20, Rect.East, altitude);
            var bottomRightB = Coordinates.GeoTo3dDouble(Rect.South, Rect.East, altitude);
            var bottomRightC = Coordinates.GeoTo3dDouble(Rect.South, Rect.East - width / 20, altitude);
            var lineColor = Color.Yellow;

            lines.AddLine(topLeftA, topLeftB, lineColor, new Dates());
            lines.AddLine(topLeftB, topLeftC, lineColor, new Dates());
            lines.AddLine(topMiddleA, topMiddleB, lineColor, new Dates());
            lines.AddLine(topRightA, topRightB, lineColor, new Dates());
            lines.AddLine(topRightB, topRightC, lineColor, new Dates());

            lines.AddLine(middleRightA, middleRightB, lineColor, new Dates());
            lines.AddLine(centerA, centerB, lineColor, new Dates());
            lines.AddLine(centerC, centerD, lineColor, new Dates());
            lines.AddLine(middleLeftA, middleLeftB, lineColor, new Dates());


            lines.AddLine(bottomLeftA, bottomLeftB, lineColor, new Dates());
            lines.AddLine(bottomLeftB, bottomLeftC, lineColor, new Dates());
            lines.AddLine(bottomMiddleA, bottomMiddleB, lineColor, new Dates());
            lines.AddLine(bottomRightA, bottomRightB, lineColor, new Dates());
            lines.AddLine(bottomRightB, bottomRightC, lineColor, new Dates());


        }

        private void North_TextChanged(object sender, EventArgs e)
        {
            double.TryParse(North.Text, out top);
            UpdateLines();
        }

        private void South_TextChanged(object sender, EventArgs e)
        {
            double.TryParse(South.Text, out bottom);
            UpdateLines();

        }

        private void West_TextChanged(object sender, EventArgs e)
        {

            double.TryParse(West.Text, out left);
            UpdateLines();
        }

        private void East_TextChanged(object sender, EventArgs e)
        {
            double.TryParse(East.Text, out right);
            UpdateLines();

        }

        LineList lines;

 
        void IUiController.Render(Earth3d window)
        {
            lines.DrawLines(window.RenderContext11, 1);

            return;
        }
        enum DragCorners { None, NW, N, NE, W, C, E, SW, S, SE };

        bool drag;

        DragCorners dragCorner = DragCorners.None;

        Coordinates mouseDown;
        bool IUiController.MouseDown(object sender, MouseEventArgs e)
        {

            var width = Rect.East - Rect.West;
            var height = Rect.North - Rect.South;

            var range = Math.Max(width / 40, height / 40);
            var cursor = Earth3d.MainWindow.GetCoordinatesForScreenPoint(e.X, e.Y);

            if (cursor.Distance(Coordinates.FromLatLng(Rect.North, Rect.West)) < range)
            {
                dragCorner = DragCorners.NW;
                drag = true;
            }
            if (cursor.Distance(Coordinates.FromLatLng(Rect.North, (Rect.West + Rect.East) / 2)) < range)
            {
                dragCorner = DragCorners.N;
                drag = true;

            }
            if (cursor.Distance(Coordinates.FromLatLng(Rect.North, Rect.East)) < range)
            {
                dragCorner = DragCorners.NE;
                drag = true;

            }

            if (cursor.Distance(Coordinates.FromLatLng((Rect.North + Rect.South) / 2, Rect.West)) < range)
            {
                dragCorner = DragCorners.W;
                drag = true;


            }

            if (cursor.Distance(Coordinates.FromLatLng((Rect.North + Rect.South) / 2, (Rect.West + Rect.East) / 2)) < range)
            {
                dragCorner = DragCorners.C;
                drag = true;

            }

            if (cursor.Distance(Coordinates.FromLatLng((Rect.North + Rect.South) / 2, Rect.East)) < range)
            {
                dragCorner = DragCorners.E;
                drag = true;

            }

            if (cursor.Distance(Coordinates.FromLatLng(Rect.South, Rect.West)) < range)
            {
                dragCorner = DragCorners.SW;
                drag = true;
            }

            if (cursor.Distance(Coordinates.FromLatLng(Rect.South, (Rect.West + Rect.East) / 2)) < range)
            {
                dragCorner = DragCorners.S;
                drag = true;

            }

            if (cursor.Distance(Coordinates.FromLatLng(Rect.South, Rect.East)) < range)
            {
                dragCorner = DragCorners.SE;
                drag = true;

            }

            if (drag)
            {
                mouseDown = cursor;
            }
            return drag;
        }

        bool IUiController.MouseUp(object sender, MouseEventArgs e)
        {
            if (!drag)
            {
                return false;
            }
            drag = false;
            return true;
        }

        bool IUiController.MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {

                var cursor = Earth3d.MainWindow.GetCoordinatesForScreenPoint(e.X, e.Y);

                switch (dragCorner)
                {
                    case DragCorners.NW:
                        Rect.North = cursor.Lat;
                        Rect.West = cursor.Lng;
                        break;
                    case DragCorners.N:
                        Rect.North = cursor.Lat;
                        break;
                    case DragCorners.NE:
                        Rect.North = cursor.Lat;
                        Rect.East = cursor.Lng;
                        break;
                    case DragCorners.W:
                        Rect.West = cursor.Lng;
                        break;
                    case DragCorners.C:
                        Rect.North -= (mouseDown.Lat - cursor.Lat);
                        Rect.West -= (mouseDown.Lng - cursor.Lng);
                        Rect.South -= (mouseDown.Lat - cursor.Lat);
                        Rect.East -= (mouseDown.Lng - cursor.Lng);

                        break;
                    case DragCorners.E:
                        Rect.East = cursor.Lng;
                        break;
                    case DragCorners.SW:
                        Rect.South = cursor.Lat;
                        Rect.West = cursor.Lng;
                        break;
                    case DragCorners.S:
                        Rect.South = cursor.Lat;
                        break;
                    case DragCorners.SE:
                        Rect.South = cursor.Lat;
                        Rect.East = cursor.Lng;
                        break;
                    default:
                        break;
                }
                mouseDown = cursor;
                UpdateTextBoxes();
                UpdateLines();
                return true;
            }
            else
            {
                var wnd = (Control)sender;
                var width = Rect.East - Rect.West;
                var height = Rect.North - Rect.South;

                var range = Math.Max(width / 40, height / 40);
                var cursor = Earth3d.MainWindow.GetCoordinatesForScreenPoint(e.X, e.Y);
                var dragCorner = DragCorners.None;
                if (cursor.Distance(Coordinates.FromLatLng(Rect.North, Rect.West)) < range)
                {
                    dragCorner = DragCorners.NW;
                }
                if (cursor.Distance(Coordinates.FromLatLng(Rect.North, (Rect.West + Rect.East) / 2)) < range)
                {
                    dragCorner = DragCorners.N;

                }
                if (cursor.Distance(Coordinates.FromLatLng(Rect.North, Rect.East)) < range)
                {
                    dragCorner = DragCorners.NE;

                }

                if (cursor.Distance(Coordinates.FromLatLng((Rect.North + Rect.South) / 2, Rect.West)) < range)
                {
                    dragCorner = DragCorners.W;

                }

                if (cursor.Distance(Coordinates.FromLatLng((Rect.North + Rect.South) / 2, (Rect.West + Rect.East) / 2)) < range)
                {
                    dragCorner = DragCorners.C;

                }

                if (cursor.Distance(Coordinates.FromLatLng((Rect.North + Rect.South) / 2, Rect.East)) < range)
                {
                    dragCorner = DragCorners.E;
                }

                if (cursor.Distance(Coordinates.FromLatLng(Rect.South, Rect.West)) < range)
                {
                    dragCorner = DragCorners.SW;
                }

                if (cursor.Distance(Coordinates.FromLatLng(Rect.South, (Rect.West + Rect.East) / 2)) < range)
                {
                    dragCorner = DragCorners.S;
                }

                if (cursor.Distance(Coordinates.FromLatLng(Rect.South, Rect.East)) < range)
                {
                    dragCorner = DragCorners.SE;
                }

                switch (dragCorner)
                {
                    case DragCorners.SE:
                    case DragCorners.NW:
                        wnd.Cursor = Cursors.SizeNWSE;
                        break;
                    case DragCorners.N:
                    case DragCorners.S:
                        wnd.Cursor = Cursors.SizeNS;
                        break;
                    case DragCorners.W:
                    case DragCorners.E:
                        wnd.Cursor = Cursors.SizeWE;
                        break;
                    case DragCorners.C:
                        wnd.Cursor = Cursors.Hand;
                        break;
                    case DragCorners.SW:
                    case DragCorners.NE:
                        wnd.Cursor = Cursors.SizeNESW;
                        break;

                    default:
                        wnd.Cursor = Cursors.Default;
                        break;
                }
            }
            return false;
        }

        bool IUiController.MouseClick(object sender, MouseEventArgs e)
        {
            return false;
        }

        bool IUiController.Click(object sender, EventArgs e)
        {
            return false;
        }

        bool IUiController.MouseDoubleClick(object sender, MouseEventArgs e)
        {
            return false;
        }

        bool IUiController.KeyDown(object sender, KeyEventArgs e)
        {
            return false;
        }

        bool IUiController.KeyUp(object sender, KeyEventArgs e)
        {
            return false;
        }

        bool IUiController.Hover(Point pnt)
        {
            return false;
        }
        public GeoRect Rect;


        public void PreRender(Earth3d window)
        {
            
        }

        private void Export_Click(object sender, EventArgs e)
        {
            var filename = "";

            var saveDialog = new SaveFileDialog();
            saveDialog.Filter =  "Standard Tessellation Language" + "|*.stl";
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveDialog.AddExtension = true;
            saveDialog.DefaultExt = ".stl";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                filename = saveDialog.FileName;

                if (string.IsNullOrEmpty(filename))
                {
                    return;
                }
            }
            else
            {
                return;
            }

            ProgressPopup.Show(this, "Export STL File", "Scanning Elevation Map");

            var baseOffset = double.Parse(baseHeight.Text);

            density = int.Parse(Density.Text);

            var xRate = Rect.West - Rect.East;
            var yRate = Rect.North - Rect.South;

            var latCenter = (Rect.North + Rect.South) / 2;

            var ratio = Math.Cos(latCenter / 180 * Math.PI);

            double sizeY = 100;
            var sizeX = Math.Abs(((xRate * ratio) / yRate) * sizeY);

            var stepsX = (int)(sizeX * density / 10);
            var stepsY = (int)(sizeY * density / 10);
     
            
            //Computer relative altitude to latitude scaling for this planet.
            var radius = Earth3d.MainWindow.RenderContext11.NominalRadius;
            var altScaleFactor = ((radius * Math.PI * 2) / 360) * (yRate/sizeY);

            altScaleFactor = 1 / altScaleFactor;

            

            xRate /= stepsX;
            yRate /= stepsY;


            var points = new Vector3d[stepsX, stepsY];
            var altitude = new double[stepsX, stepsY];
            double maxAltitude = -10000000;
            double minAltitude = 100000000;
            var altScale = double.Parse(AltitudeScale.Text) / 100;


            var estimatedTotal = stepsX * stepsY;
            var actualTotal = 0;


            for (var y = 0; y < stepsY; y++)
            {
                for (var x = 0; x < stepsX; x++)
                {
                    var lat = Rect.North - (yRate * y);
                    var lng = Rect.East + (xRate * x);

                    var alt = Earth3d.MainWindow.GetAltitudeForLatLongNow(lat, lng);
                    altitude[x, y] = alt;
                    maxAltitude = Math.Max(alt, maxAltitude);
                    minAltitude = Math.Min(minAltitude, alt);
                    actualTotal++;
                }

                if (!ProgressPopup.SetProgress(((actualTotal * 100) / estimatedTotal), "Scanning Elevation Map"))
                {
                    ProgressPopup.Done();
                    return;
                }

                
            }

            var altRange = maxAltitude - minAltitude;

            // old altScaleFactor = (10 / altRange) * altScale;
            altScaleFactor *= altScale;

            var stepScaleX = sizeX / stepsX;
            var stepScaleY = sizeY / stepsY;

            // make the verticies
            for (var y = 0; y < stepsY; y++)
            {
                for (var x = 0; x < stepsX; x++)
                {
                    altitude[x, y] = ((altitude[x, y] - minAltitude) * altScaleFactor) + baseOffset;

                    points[x, y] = new Vector3d(x * stepScaleX, y * stepScaleY, altitude[x, y]);


                }
            }

           


            ProgressPopup.SetProgress(0, "Writing File");

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            var fs = File.OpenWrite(filename);
            var bw = new BinaryWriter(fs);

            // Write File Header
            bw.Write(new byte[80]);

            // x-1*y-1*2
            var count = ((stepsX - 1) * (stepsY - 1) + (stepsY - 1) + (stepsY - 1) + (stepsX - 1) + (stepsX - 1) + (stepsX - 1) * (stepsY - 1)) * 2;

 
            // Write Triangle Count
            bw.Write(count);


            // Loop thru and create triangles for all quads..

            var writeCount = 0;

            for (var y = 0; y < stepsY - 1; y++)
            {
                for (var x = 0; x < stepsX - 1; x++)
                {
                    // Write dummy Normal
                    bw.Write(0f);
                    bw.Write(0f);
                    bw.Write(0f);


                    // Vertexes - triangle 1
                    WriteVertex(bw, points[x, y]);
                    WriteVertex(bw, points[x + 1, y]);
                    WriteVertex(bw, points[x + 1, y + 1]);
                    bw.Write((UInt16)(0));
                    writeCount++;


                    // Write dummy Normal
                    bw.Write(0f);
                    bw.Write(0f);
                    bw.Write(0f);
                    // Vertexes - triangle 2
                    WriteVertex(bw, points[x, y]);
                    WriteVertex(bw, points[x + 1, y + 1]);
                    WriteVertex(bw, points[x, y + 1]);
                    bw.Write((UInt16)(0));
                    writeCount++;
                }
            }
            ProgressPopup.SetProgress(35, "Writing File");

            var pnt = new Vector3d();

            // Make side Skirts
            for (var y = 0; y < stepsY - 1; y++)
            {
                var x = 0;
                // Write dummy Normal
                bw.Write(0f);
                bw.Write(0f);
                bw.Write(0f);


                // Vertexes - triangle 1
                WriteVertex(bw, points[x, y]);
                WriteVertex(bw, points[x, y + 1]);
                pnt = points[x, y];
                pnt.Z = 0;
                WriteVertex(bw, pnt);
                bw.Write((UInt16)(0));
                writeCount++;


                // Write dummy Normal
                bw.Write(0f);
                bw.Write(0f);
                bw.Write(0f);
                // Vertexes - triangle 2
                WriteVertex(bw, points[x, y + 1]);

                pnt = points[x, y + 1];
                pnt.Z = 0;
                WriteVertex(bw, pnt);

                pnt = points[x, y];
                pnt.Z = 0;
                WriteVertex(bw, pnt);
                bw.Write((UInt16)(0));
                writeCount++;

            }

            ProgressPopup.SetProgress(45, "Writing File");

            for (var y = 0; y < stepsY - 1; y++)
            {
                var x = stepsX - 1;
                // Write dummy Normal
                bw.Write(0f);
                bw.Write(0f);
                bw.Write(0f);


                // Vertexes - triangle 1
                WriteVertex(bw, points[x, y + 1]);
                WriteVertex(bw, points[x, y]);
                pnt = points[x, y];
                pnt.Z = 0;
                WriteVertex(bw, pnt);
                bw.Write((UInt16)(0));
                writeCount++;


                // Write dummy Normal
                bw.Write(0f);
                bw.Write(0f);
                bw.Write(0f);
                // Vertexes - triangle 2

                pnt = points[x, y + 1];
                pnt.Z = 0;
                WriteVertex(bw, pnt);
                WriteVertex(bw, points[x, y + 1]);

                pnt = points[x, y];
                pnt.Z = 0;
                WriteVertex(bw, pnt);
                bw.Write((UInt16)(0));
                writeCount++;

            }

            ProgressPopup.SetProgress(50, "Writing File");

            for (var x = 0; x < stepsX - 1; x++)
            {
                var y = 0;
                // Write dummy Normal
                bw.Write(0f);
                bw.Write(0f);
                bw.Write(0f);


                // Vertexes - triangle 1
                WriteVertex(bw, points[x+ 1, y ]);
                WriteVertex(bw, points[x, y]);
                pnt = points[x, y];
                pnt.Z = 0;
                WriteVertex(bw, pnt);
                bw.Write((UInt16)(0));
                writeCount++;


                // Write dummy Normal
                bw.Write(0f);
                bw.Write(0f);
                bw.Write(0f);
                // Vertexes - triangle 2

                pnt = points[x+ 1, y ];
                pnt.Z = 0;
                WriteVertex(bw, pnt);
                WriteVertex(bw, points[x + 1, y]);

                pnt = points[x, y];
                pnt.Z = 0;
                WriteVertex(bw, pnt);
                bw.Write((UInt16)(0));
                writeCount++;

            }

            ProgressPopup.SetProgress(55, "Writing File");

            for (var x = 0; x < stepsX - 1; x++)
            {
                var y = stepsY - 1;
                // Write dummy Normal
                bw.Write(0f);
                bw.Write(0f);
                bw.Write(0f);


                // Vertexes - triangle 1
                WriteVertex(bw, points[x, y]);
                WriteVertex(bw, points[x + 1, y]);
                pnt = points[x, y];
                pnt.Z = 0;
                WriteVertex(bw, pnt);
                bw.Write((UInt16)(0));
                writeCount++;


                // Write dummy Normal
                bw.Write(0f);
                bw.Write(0f);
                bw.Write(0f);
                // Vertexes - triangle 2
                WriteVertex(bw, points[x + 1, y]);

                pnt = points[x + 1, y];
                pnt.Z = 0;
                WriteVertex(bw, pnt);

                pnt = points[x, y];
                pnt.Z = 0;
                WriteVertex(bw, pnt);
                bw.Write((UInt16)(0));
                writeCount++;

            }

            ProgressPopup.SetProgress(65, "Writing File");


            ProgressPopup.SetProgress(75, "Writing File");

            for (var y = 0; y < stepsY - 1; y++)
            {
                for (var x = 0; x < stepsX - 1; x++)
                {
                    // Write dummy Normal
                    bw.Write(0f);
                    bw.Write(0f);
                    bw.Write(0f);


                    // Vertexes - triangle 1
                    pnt = points[x, y];
                    pnt.Z = 0;
                    WriteVertex(bw, pnt);

                    pnt = points[x + 1, y + 1];
                    pnt.Z = 0;
                    WriteVertex(bw, pnt);

                    pnt = points[x + 1, y];
                    pnt.Z = 0;
                    WriteVertex(bw, pnt);

                  

                    bw.Write((UInt16)(0));
                    writeCount++;


                    // Write dummy Normal
                    bw.Write(0f);
                    bw.Write(0f);
                    bw.Write(0f);

                    // Vertexes - triangle 2
                    pnt = points[x, y];
                    pnt.Z = 0;
                    WriteVertex(bw, pnt);

                    pnt = points[x, y + 1];
                    pnt.Z = 0;
                    WriteVertex(bw, pnt);  
                    
                    pnt = points[x + 1, y + 1];
                    pnt.Z = 0;
                    WriteVertex(bw, pnt);

                    bw.Write((UInt16)(0));
                    writeCount++;
                }
            }



            // Make Bottom

            bw.Close();

            ProgressPopup.Done();

        }

        static void WriteVertex(BinaryWriter bw, Vector3d point)
        {
            bw.Write((float)point.X);
            bw.Write((float)point.Y);
            bw.Write((float)point.Z);
        }

        private void ExportSTL_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Earth3d.MainWindow.UiController == this)
            {
                Earth3d.MainWindow.UiController = null;
            }
        }
    }

    public struct GeoRect
    {
        public double North;
        public double South;
        public double East;
        public double West;
    }
}
