using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Xml;
using SharpDX.Direct3D;
using TerraViewer.Properties;


namespace TerraViewer
{
    public enum OverlayAnchor { Sky, Screen, Dome };


    public abstract class Overlay : IAnimatable
    {
        public static OverlayAnchor DefaultAnchor = OverlayAnchor.Screen;
        public bool isDynamic = false;
        public const string ClipboardFormat = "WorldWideTelescope.Overlay";
        public bool TriangleStrip = true;
        protected bool isDesignTimeOnly = false;

        string name;

        public int Version = 0;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Id = Guid.NewGuid().ToString();

        TourStop owner;

        public TourStop Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public int ZOrder
        {
            get
            {
                var index = owner.Overlays.FindIndex(delegate(Overlay overlay) { return this == overlay; });
                return index;
            }
        }

        private string url="";

        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        private string linkID="";

        public string LinkID
        {
            get { return linkID; }
            set { linkID = value; }
        }

       
        virtual public void Play()
        {
        }

       
        virtual public void Pause()
        {
        }

       
        virtual public void Stop()
        {
        }

       
        virtual public void Seek(double time)
        {
        }



        public AnimationTarget AnimationTarget = null;



        /// <summary>
        /// Creates a vertex position based on the anchor type
        /// </summary>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <returns></returns>

        Matrix3d domeMatrix = Matrix3d.Identity;
        float domeMatX;
        float domeMatY;
        float domeAngle = 0;
        public Vector3d MakePosition(float centerX, float centerY, float offsetX, float offsetY, float angle)
        {
            centerX -= 960;
            centerY -= 558;

            if (Anchor == OverlayAnchor.Screen)
            {
                var point = new Vector3d(centerX + offsetX, centerY + offsetY, 1347);

                if (domeMatX != 0 || domeMatY != 0 || domeAngle != angle)
                {
                    domeMatX = centerX;
                    domeMatY = centerY;
                    domeMatrix = Matrix3d.Translation(new Vector3d(-centerX, -centerY, 0)) * Matrix3d.RotationZ((float)(angle / 180 * Math.PI)) * Matrix3d.Translation(new Vector3d(centerX, centerY, 0));
                }
                point.TransformCoordinate(domeMatrix);

                return point;
            }
            else
            {
                centerX /=1350;
                centerY /=1350;
                var point = new Vector3d(offsetX, offsetY, 1347);

                if (domeMatX != centerX || domeMatY != centerY || domeAngle != angle) 
                {
                    domeMatX = centerX;
                    domeMatY = centerY;
                    domeMatrix =  Matrix3d.RotationZ((float)(angle/180*Math.PI)) * Matrix3d.RotationX(-centerY) * Matrix3d.RotationY(centerX);
                }
                point.TransformCoordinate(domeMatrix);

                return point;

            }
        }

        virtual public void Draw3D(RenderContext11 renderContext, float transparancy, bool designTime)
        {
            if (texture == null || isDynamic)
            {
                InitializeTexture();
            }

            if (!isDesignTimeOnly || designTime) 
            {          
                InitiaizeGeometry();

                UpdateRotation(); ;

                Sprite2d.Draw(renderContext, points, points.Length, texture, TriangleStrip ? PrimitiveTopology.TriangleStrip : PrimitiveTopology.TriangleList);
            }
        }

        virtual public void CleanUp()
        {
            if (texture != null)
            {
                texture.Dispose();
                GC.SuppressFinalize(texture);
                texture = null;
            }
        }

        virtual public void InitializeTexture()
        {
        }

        protected PositionColoredTextured[] points = null;
        virtual public void CleanUpGeometry()
        {
            points = null;
            currentRotation = 0;
        }

        virtual public void InitiaizeGeometry()
        {
            if (points == null)
            {
            
                currentRotation = 0;
                points = new PositionColoredTextured[4];

                points[0].Position = MakePosition(X, Y, -Width / 2, -Height / 2, RotationAngle).Vector4;
                points[0].Tu = 0;
                points[0].Tv = 0;
                points[0].Color = Color;


                points[1].Position = MakePosition(X, Y, Width / 2, -Height / 2, RotationAngle).Vector4;
                points[1].Tu = 1;
                points[1].Tv = 0;
                points[1].Color = Color;


                points[2].Position = MakePosition(X, Y, -Width / 2, Height / 2, RotationAngle).Vector4;
                points[2].Tu = 0;
                points[2].Tv = 1;
                points[2].Color = Color;


                points[3].Position = MakePosition(X, Y, Width / 2, Height / 2, RotationAngle).Vector4;
                points[3].Tu = 1;
                points[3].Tv = 1;
                points[3].Color = Color;
            }
        }

        virtual public void UpdateRotation()
        {
            // Make position replaced all this.
            //if (currentRotation == RotationAngle || points == null)
            //{
            //    return;
            //}
            
            //System.Drawing.Drawing2D.Matrix mat = new System.Drawing.Drawing2D.Matrix();


            //PointF[] tempPoints = new PointF[points.Length];
            //for (int i = 0; i < points.Length; i++)
            //{
            //    tempPoints[i].X = points[i].X;
            //    tempPoints[i].Y = points[i].Y;
            //}
            //mat.RotateAt(RotationAngle - currentRotation,new PointF((float)(X),(float)(Y)));

            //mat.TransformPoints(tempPoints);

            //for (int i = 0; i < points.Length; i++)
            //{
            //    points[i].X = (float)tempPoints[i].X;
            //    points[i].Y = (float)tempPoints[i].Y;
            //}
            //currentRotation = RotationAngle;

        }
        // Animation Support
        bool animate;

        public bool Animate
        {
            get { return animate; }
            set
            {
                if (animate != value)
                {
 
                    if (value)
                    {
                        endX = x;
                        endY = y;
                        endRotationAngle = rotationAngle;
                        endColor = color;
                        endWidth = width;
                        endHeight = height;
                        CleanUpGeometry();
                    }
                    else
                    {
                        endX = x = X;
                        endY = y = Y;
                        endRotationAngle = rotationAngle = RotationAngle;
                        endColor = color = Color;
                        endWidth = width = Width;
                        endHeight = height = Height;
                        CleanUpGeometry();
                        tweenFactor = 0;
                    }
                    animate = value;
              }
            }
        }
        float tweenFactor;

        public float TweenFactor
        {
            get
            {
                return tweenFactor;
            }
            set
            {
                if (!animate)
                {
                    tweenFactor = 0;
                }
                else
                {
                    if (tweenFactor != value)
                    {
                        tweenFactor = value;
                        CleanUpGeometry();
                    }
                }
            }
        }
        
        float endX;
        float endY;
        float endOpacity;
        Color endColor;
        float endWidth;
        float endHeight;
        float endRotationAngle;



        // End Animation Support
        public virtual double[] GetParams()
        {
            if (animate && tweenFactor > .5f)
            {
                return new[] { endX, endY, endWidth, endHeight, endRotationAngle, endColor.R / 255.0, endColor.G / 255.0, endColor.B / 255.0, endColor.A / 255.0 };
            }
            return new[] { x, y, width, height, rotationAngle, color.R / 255.0, color.G / 255.0, color.B / 255.0, color.A / 255.0 };
        }

        public virtual string[] GetParamNames()
        {

            return new[] { "Translate.X", "Translate.Y", "Size.Width", "Size.Height", "Rotation", "Color.Red", "Color.Green", "Color.Blue", "Color.Alpha" };
        }

        public BaseTweenType[] GetParamTypes()
        {
            return new[] { BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Power, BaseTweenType.Power, BaseTweenType.Linear, BaseTweenType.Linear, BaseTweenType.Linear , BaseTweenType.Linear, BaseTweenType.Linear};
        }

        public string GetIndentifier()
        {
            return Id;
        }

        public string GetName()
        {
            return Name;
        }

        public virtual void SetParams(double[] paramList)
        {
            animate = false;
            x = (float) paramList[0];
            y =(float) paramList[1];
            width = (float) paramList[2];
            height = (float) paramList[3];
            rotationAngle = (float) paramList[4];

            Color = Color.FromArgb(Math.Min(255, (int)(paramList[8] * 255)), Math.Min(255, (int)(paramList[5] * 255)), Math.Min(255, (int)(paramList[6] * 255)), Math.Min(255, (int)(paramList[7] * 255)));
            Version++;
            CleanUpGeometry();
        }


        public IUiController GetEditUI()
        {
            return null;
        }

        OverlayAnchor anchor = DefaultAnchor;

        public OverlayAnchor Anchor
        {
            get { return anchor; }
            set { anchor = value; }
        }
       
        public PointF Position
        {
            get
            {
                return new PointF(X,Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        private  float x;


       
        public float X
        {
            get
            {
                if (animate)
                {
                    return (x*(1-tweenFactor))+(endX*tweenFactor); 
                }
                return x;
            }
            set
            {
                if (tweenFactor < .5f || !animate)
                {
                    if (x != value)
                    {
                        Version++;
                        x = value;
                        CleanUpGeometry();
                    }
                }
                else
                {
                    if (endX != value)
                    {
                        endX = value;
                        Version++;
                        CleanUpGeometry();
                    }
                }
            }
        }
        private float y;

       
        public float Y
        {
            get
            {
                if (animate)
                {
                    return (y * (1 - tweenFactor)) + (endY * tweenFactor);
                }
                return y;
            }
            set
            {
                if (tweenFactor < .5f || !animate)
                {
                    if (y != value)
                    {
                        y = value;
                        Version++;
                        CleanUpGeometry();
                    }
                }
                else
                {
                    if (endY != value)
                    {
                        endY = value;
                        Version++;
                        CleanUpGeometry();
                    }
                }
            }
        }
        private float width;

       
        public float Width
        {
            get
            {
                if (animate)
                {
                    return (width * (1 - tweenFactor)) + (endWidth * tweenFactor);
                }
                return width;
            }
            set
            {
                if (value < 5 && value != 0 )
                {
                    value = 5;
                }

                if (tweenFactor < .5f || !animate)
                {
                    if (width != value)
                    {
                        width = value;
                        Version++;
                        CleanUpGeometry();
                    }
                }
                else
                {
                    if (endWidth != value)
                    {
                        endWidth = value;
                        Version++;
                        CleanUpGeometry();
                    }
                }
            }
        }


        private float height;

       
        public float Height
        {
            get
            {
                if (animate)
                {
                    return (height * (1 - tweenFactor)) + (endHeight * tweenFactor);
                }
                return height;
            }
            set
            {
                if (value < 5 && value != 0)
                {
                    value = 5;
                }

                if (tweenFactor < .5f || !animate)
                {
                    if (height != value)
                    {
                        height = value;
                        Version++;
                        CleanUpGeometry();
                    }
                }
                else
                {
                    if (endHeight != value)
                    {
                        endHeight = value;
                        Version++;
                        CleanUpGeometry();
                    }
                }
            }
        }

        private Color color = Color.White;

       
        public virtual Color Color
        {
            get
            {
                if (animate)
                {
                    var red = (int)((color.R * (1f - tweenFactor)) + (endColor.R * tweenFactor));
                    var green = (int)((color.G * (1f - tweenFactor)) + (endColor.G * tweenFactor));
                    var blue = (int)((color.B * (1f - tweenFactor)) + (endColor.B * tweenFactor));
                    var alpha = (int)((color.A * (1f - tweenFactor)) + (endColor.A * tweenFactor));
                    return Color.FromArgb(Math.Max(0, Math.Min(255, alpha)), Math.Max(0, Math.Min(255, red)), Math.Max(0, Math.Min(255, green)), Math.Max(0, Math.Min(255, blue)));
                }
                return color;
            }
            set
            {
                if (tweenFactor < .5f | !animate)
                {
                    if (color != value)
                    {
                        color = value;
                        Version++;
                        CleanUpGeometry();
                    }
                }
                else
                {
                    if (endColor != value)
                    {
                        endColor = value;
                        Version++;
                        CleanUpGeometry();
                    }
                }
            }
        }

        private float opacity = .5f;

       
        public float Opacity
        {
            get
            {
                return Color.A/255.0f;
            }
            set
            {
                var col = Color;
                Color = Color.FromArgb(Math.Min(255,(int)(value * 255f)), col.R, col.G, col.B);
                opacity = value;
                Version++;
            }
        }

        float rotationAngle;
        protected float currentRotation = 0;
       

        public float RotationAngle
        {
            get
            {
                if (animate)
                {
                    return (rotationAngle * (1 - tweenFactor)) + (endRotationAngle * tweenFactor);
                }
                return rotationAngle;
            }
            set
            {
                if (tweenFactor < .5f || !animate)
                {
                    if (rotationAngle != value)
                    {
                        Version++;
                        rotationAngle = value;
                        CleanUpGeometry();
                    }
                }
                else
                {
                    if (endRotationAngle != value)
                    {
                        Version++;
                        endRotationAngle = value;
                        CleanUpGeometry();
                    }
                }
            }
        }

        protected Texture11 texture = null;

        virtual public bool HitTest(PointF pntTest)
        {
            var mat = new Matrix();
            mat.RotateAt(-RotationAngle, new PointF(X , Y ));

            var tempPoints = new PointF[1];

            tempPoints[0].X = pntTest.X;
            tempPoints[0].Y = pntTest.Y;

            mat.TransformPoints( tempPoints);

            var rect = new RectangleF((X-(Width/2)), (Y-(Height/2)), Width, Height);
            if (rect.Contains(tempPoints[0]))
            {
                return true;
            }

            return false;

        }


        Rectangle bounds;

        public Rectangle Bounds
        {
            get
            {
                return bounds;
            }
            set
            {
                bounds = value;
            }
        }

        private InterpolationType interpolationType = InterpolationType.Default;

        public InterpolationType InterpolationType
        {
            get { return interpolationType; }
            set { interpolationType = value; }
        }


        public virtual void SaveToXml(XmlTextWriter xmlWriter, bool saveKeys)
        {
            xmlWriter.WriteStartElement("Overlay");
            xmlWriter.WriteAttributeString("Id", Id);
            xmlWriter.WriteAttributeString("Type", GetType().FullName);
            xmlWriter.WriteAttributeString("Name", Name);
            xmlWriter.WriteAttributeString("X", x.ToString());
            xmlWriter.WriteAttributeString("Y", y.ToString());
            xmlWriter.WriteAttributeString("Width", width.ToString());
            xmlWriter.WriteAttributeString("Height", height.ToString());
            xmlWriter.WriteAttributeString("Rotation", rotationAngle.ToString());
            xmlWriter.WriteAttributeString("Color", SavedColor.Save(color));
            xmlWriter.WriteAttributeString("Url", url);
            xmlWriter.WriteAttributeString("LinkID", linkID);
            xmlWriter.WriteAttributeString("Animate", animate.ToString());
            if (animate)
            {
                xmlWriter.WriteAttributeString("EndX", endX.ToString());
                xmlWriter.WriteAttributeString("EndY", endY.ToString());
                xmlWriter.WriteAttributeString("EndWidth", endWidth.ToString());
                xmlWriter.WriteAttributeString("EndHeight", endHeight.ToString());
                xmlWriter.WriteAttributeString("EndRotation", endRotationAngle.ToString());
                xmlWriter.WriteAttributeString("EndColor", SavedColor.Save(endColor));
                xmlWriter.WriteAttributeString("InterpolationType", interpolationType.ToString());
            }
            xmlWriter.WriteAttributeString("Anchor", anchor.ToString());
           
            
            WriteOverlayProperties(xmlWriter);


            if (AnimationTarget != null && saveKeys)
            {
                AnimationTarget.SaveToXml(xmlWriter);
            }

            xmlWriter.WriteEndElement();
        }

        public virtual void AddFilesToCabinet(FileCabinet fc)
        {
             throw new Exception("The method or operation is not implemented.");
        }

        public virtual void WriteOverlayProperties(XmlTextWriter xmlWriter)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        internal static Overlay FromXml(TourStop owner, XmlNode overlay)
        {
            var overlayClassName = overlay.Attributes["Type"].Value;

            var overLayType = Type.GetType(overlayClassName);

            var newOverlay = (Overlay)Activator.CreateInstance(overLayType);
            newOverlay.owner = owner;
            newOverlay.FromXml(overlay);
            return newOverlay;
        }

        private void FromXml(XmlNode node)
        {
            
            Id = node.Attributes["Id"].Value;
            Name = node.Attributes["Name"].Value;
            x = Convert.ToSingle(node.Attributes["X"].Value);
            y = Convert.ToSingle(node.Attributes["Y"].Value);
            width = Convert.ToSingle(node.Attributes["Width"].Value);
            height = Convert.ToSingle(node.Attributes["Height"].Value);
            rotationAngle = Convert.ToSingle(node.Attributes["Rotation"].Value);
            color = SavedColor.Load(node.Attributes["Color"].Value);
            if (node.Attributes["Url"] != null)
            {
                Url = node.Attributes["Url"].Value;
            }

            if (node.Attributes["LinkID"] != null)
            {
                LinkID = node.Attributes["LinkID"].Value;
            }

            if (node.Attributes["Anchor"] != null)
            {
                anchor = (OverlayAnchor)Enum.Parse(typeof(OverlayAnchor), node.Attributes["Anchor"].Value);
            }
            else
            {
                anchor = OverlayAnchor.Screen;
            }

            if (node.Attributes["Animate"] != null)
            {
                animate = Convert.ToBoolean(node.Attributes["Animate"].Value);
                if (animate)
                {
                    endX = Convert.ToSingle(node.Attributes["EndX"].Value);
                    endY = Convert.ToSingle(node.Attributes["EndY"].Value);
                    endColor = SavedColor.Load(node.Attributes["EndColor"].Value);
                    endWidth = Convert.ToSingle(node.Attributes["EndWidth"].Value);
                    endHeight = Convert.ToSingle(node.Attributes["EndHeight"].Value);
                    endRotationAngle = Convert.ToSingle(node.Attributes["EndRotation"].Value);
                    if (node.Attributes["InterpolationType"] != null)
                    {
                        InterpolationType = (InterpolationType)Enum.Parse(typeof(InterpolationType), node.Attributes["InterpolationType"].Value);
                    } 
                }
            }

            if (node["KeyFrames"] != null)
            {
                AnimationTarget = new AnimationTarget(owner);
                AnimationTarget.FromXml(node["KeyFrames"]);
                AnimationTarget.Target = this;
                AnimationTarget.TargetID = GetIndentifier();
            }

            InitializeFromXml(node);
        }

        public virtual void InitializeFromXml(XmlNode node)
        {

        }

        public override string ToString()
        {
            return Name;
        }   
    }

    
    public class BitmapOverlay : Overlay
    {
        string filename;

        public BitmapOverlay()
        {

        }
        
        public BitmapOverlay(TourStop owner, string filename)
        {
            Owner = owner;
            this.filename = Guid.NewGuid() + ".png";

            Name = filename.Substring(filename.LastIndexOf('\\') + 1);
            File.Copy(filename, Owner.Owner.WorkingDirectory + this.filename);

            var bmp = new Bitmap(Owner.Owner.WorkingDirectory + this.filename);
            Width = bmp.Width;
            Height = bmp.Height;
            bmp.Dispose();
            GC.SuppressFinalize(bmp);
            bmp = null;
            X = 0;
            Y = 0;          
        }


        public BitmapOverlay( TourStop owner, Image image)
        {
            Owner = owner;
            // to make directory and guid filename in tour temp dir.
            filename = Guid.NewGuid()+".png";

            Name = owner.GetNextDefaultName("Image");
            X = 0;
            Y = 0;
            image.Save(Owner.Owner.WorkingDirectory + filename, ImageFormat.Png);
            Width = image.Width;
            Height = image.Height;
        }

        public BitmapOverlay Copy(TourStop owner)
        {
            var newBmpOverlay = new BitmapOverlay();
            newBmpOverlay.Owner = owner;
            newBmpOverlay.filename = filename;
            newBmpOverlay.X = X;
            newBmpOverlay.Y = Y;
            newBmpOverlay.Width = Width;
            newBmpOverlay.Height = Height;
            newBmpOverlay.Color = Color;
            newBmpOverlay.Opacity = Opacity;
            newBmpOverlay.RotationAngle = RotationAngle;
            newBmpOverlay.Name = Name + " - Copy";

            return newBmpOverlay;
        }

        public override void CleanUp()
        {
            texture = null;
        }

        public override void AddFilesToCabinet(FileCabinet fc)
        {
            fc.AddFile(Owner.Owner.WorkingDirectory + filename);
        }

        public override void InitializeTexture()
        {
            try
            {
                texture = Owner.Owner.GetCachedTexture(Owner.Owner.WorkingDirectory + filename, false);

                if (Width == 0 && Height == 0)
                {
                    Width = texture.Width;
                    Height = texture.Height;

                }
            }
            catch
            {
                texture = Texture11.FromBitmap(Resources.BadImage);

                Width = texture.Width;
                Height = texture.Height;

            }
        }

        public override void WriteOverlayProperties(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Bitmap");
            xmlWriter.WriteAttributeString("Filename", filename);
            xmlWriter.WriteEndElement();
        }

        public override void InitializeFromXml(XmlNode node)
        {
            XmlNode bitmap = node["Bitmap"];
            filename = bitmap.Attributes["Filename"].Value;
        }
    } 
    public class TextOverlay : Overlay
    {
        public TextObject TextObject;
        public override Color Color
        {
            get
            {
                return base.Color;

            }
            set
            {
                if (TextObject.ForegroundColor != value || base.Color != value)
                {
                    TextObject.ForegroundColor = value;
                    base.Color = value;
                    CleanUp();
                }
            }
        }
        public TextOverlay()
        {
        }
        public TextOverlay(TextObject textObject)
        {
            TextObject = textObject;
            Name = textObject.Text.Split(new[] {'\r','\n'})[0];
            X = 0;
            Y = 0;
            
        }

        public override void InitializeTexture()
        {
            if (texture != null)
            {
                texture.Dispose();
                GC.SuppressFinalize(texture);
            }
            var font = TextObject.Font;
            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;

            var bmp = new Bitmap(20, 20);
            var g = Graphics.FromImage(bmp);

            var text = TextObject.Text;

            if (text.Contains("{$"))
            {
                isDynamic = true;
                text = text.Replace("{$DATE}", SpaceTimeController.Now.ToString("yyyy/MM/dd"));
                text = text.Replace("{$TIME}", SpaceTimeController.Now.ToString("HH:mm:ss"));
                text = text.Replace("{$DIST}", UiTools.FormatDistance(Earth3d.MainWindow.SolarSystemCameraDistance));
                text = text.Replace("{$LAT}", Coordinates.FormatDMS(Earth3d.MainWindow.ViewLat));
                text = text.Replace("{$LNG}", Coordinates.FormatDMS(Earth3d.MainWindow.ViewLong));
                text = text.Replace("{$RA}", Coordinates.FormatDMS(Earth3d.MainWindow.RA));
                text = text.Replace("{$DEC}", Coordinates.FormatDMS(Earth3d.MainWindow.Dec));
                text = text.Replace("{$FOV}", Coordinates.FormatDMS(Earth3d.MainWindow.FovAngle));
            }
            else
            {
                isDynamic = false;
            }
            

            var size = g.MeasureString(text, font);
            g.Dispose();
            GC.SuppressFinalize(g);
            bmp.Dispose();

            float border =0;

            switch (TextObject.BorderStyle)
            {
                case TextBorderStyle.None:
                case TextBorderStyle.Tight:
                    border = 0;
                    break;
                case TextBorderStyle.Small:
                    border = 10;
                    break;
                case TextBorderStyle.Medium:
                    border = 15;
                    break;
                case TextBorderStyle.Large:
                    border = 20;
                    break;
                default:
                    break;
            }
            if (size.Width == 0 || size.Height == 0)
            {
                size = new SizeF(1, 1);
            }
            bmp = new Bitmap((int)(size.Width + (border * 2)), (int)(size.Height + (border * 2)));
            g = Graphics.FromImage(bmp);
            if (TextObject.BorderStyle != TextBorderStyle.None)
            {
                g.Clear(TextObject.BackgroundColor);
            }

            g.SmoothingMode = SmoothingMode.HighQuality;
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            Brush textBrush = new SolidBrush(Color);

            g.DrawString(text, font, textBrush, border, border, sf);
            textBrush.Dispose();
            GC.SuppressFinalize(textBrush);
            g.Dispose();
            GC.SuppressFinalize(g);
            texture = Texture11.FromBitmap( bmp);
            bmp.Dispose();
            font.Dispose();
            if (Width == 0 && Height == 0)
            {
                Width = size.Width + (border * 2);
                Height = size.Height + (border * 2);
            }
        }

        public override void WriteOverlayProperties(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Text");
            TextObject.SaveToXml(xmlWriter);
            xmlWriter.WriteEndElement();
        }

        public override void InitializeFromXml(XmlNode node)
        {
            XmlNode text = node["Text"];

            TextObject = TextObject.FromXml(text["TextObject"]);
        
        }
        public override void AddFilesToCabinet(FileCabinet fc)
        {
            return;
        }

        override public void InitiaizeGeometry()
        {
            currentRotation = 0;
            if (points == null)
            {
                points = new PositionColoredTextured[4];
            }

            points[0].Position = MakePosition(X, Y, -Width / 2, -Height / 2, RotationAngle).Vector4;
            points[0].Tu = 0;
            points[0].Tv = 0;
            points[0].Color = Color.White;


            points[1].Position = MakePosition(X, Y, Width / 2, -Height / 2, RotationAngle).Vector4;
            points[1].Tu = 1;
            points[1].Tv = 0;
            points[1].Color = Color.White;


            points[2].Position = MakePosition(X, Y, -Width / 2, Height / 2, RotationAngle).Vector4;
            points[2].Tu = 0;
            points[2].Tv = 1;
            points[2].Color = Color.White;

            points[3].Position = MakePosition(X, Y, Width / 2, Height / 2, RotationAngle).Vector4;
            points[3].Tu = 1;
            points[3].Tv = 1;
            points[3].Color = Color.White;
        }
    }
    
    public enum ShapeType { Circle, Rectagle, Star, Donut, Arrow, Line, OpenRectagle };
    public class ShapeOverlay : Overlay
    {
        ShapeType shapeType = ShapeType.Rectagle;

        public ShapeOverlay()
        {
        }


        public ShapeType ShapeType
        {
          get { return shapeType; }
          set
          {
              shapeType = value;
              CleanUpGeometry();
          }
        }

        public ShapeOverlay( TourStop owner, ShapeType shapeType)
        {
            ShapeType = shapeType;
            Owner = owner;
            Name = owner.GetNextDefaultName(shapeType.ToString());
        }

        public override void AddFilesToCabinet(FileCabinet fc)
        {
            return;
        }

        public override void InitiaizeGeometry()
        {
            if (points == null)
            {
                switch (shapeType)
                {
                    case ShapeType.Circle:
                        {
                            CreateCircleGeometry();

                        }
                        break;
                    case ShapeType.Rectagle:
                        base.InitiaizeGeometry();
                        break;
                    case ShapeType.OpenRectagle:
                        CreateOpenRectGeometry();
                        break;
                    case ShapeType.Star:
                        CreateStarGeometry();
                        break;
                    case ShapeType.Donut:
                        CreateDonutGeometry();
                        break;
                    case ShapeType.Arrow:
                        CreateArrowGeometry();
                        break;
                    case ShapeType.Line:
                        CreateLineGeometry();
                        break;
                    default:
                        break;
                }
            }
        }
        private void CreateLineGeometry()
        {
            var centerX = X;
            var centerY = Y;
            var radius = Width / 2;

            //float length = (float)Math.Sqrt(Width * Width + Height * Height);
            var length = Width;
            var segments = (int)(length / 12f) + 1;
            var radiansPerSegment = ((float)Math.PI * 2) / segments;
            if (points == null)
            {
                points = new PositionColoredTextured[segments * 2 + 2];
            }

            for (var j = 0; j <= segments; j++)
            {
                var i = j * 2;
                points[i].Position = MakePosition(X, Y, (float)((j / (double)segments) * (Width) - (Width / 2)), 6f, RotationAngle).Vector4;
                points[i].Tu = ((j) % 2);
                points[i].Tv = 0;
                points[i].Color = Color;

                points[i + 1].Position = MakePosition(X, Y, (float)((j / (double)segments) * (Width) - (Width / 2)), -6f, RotationAngle).Vector4;
                points[i + 1].Tu = (j % 2);
                points[i + 1].Tv = 1;
                points[i + 1].Color = Color;

            }
        }
        private void CreateOpenRectGeometry()
        {
            var centerX = X;
            var centerY = Y;
            var radius = Width / 2;

            var length = Width;
            var segments = (int)(length / 12f) + 1;
            var segmentsHigh = (int)(Height / 12f) + 1;

            var totalPoints = (((segments+1) * 2 )+((segmentsHigh+1)*2 ))*2;
            if (points == null)
            {
                points = new PositionColoredTextured[totalPoints];
            }
            for (var j = 0; j <= segments; j++)
            {
                var i = j * 2;

                points[i].Position = MakePosition(centerX, centerY,
                    (float)(j / (double)segments) * (Width) - (Width / 2),
                    Height / 2, RotationAngle).Vector4;
                points[i].Tu = ((j) % 2);
                points[i].Tv = 0;
                points[i].Color = Color;


                points[i + 1].Position = MakePosition(centerX, centerY,
                    (float)(j / (double)segments) * (Width) - (Width / 2),
                    (Height / 2) - 12f, RotationAngle).Vector4;
                points[i + 1].Tu = (j % 2);
                points[i + 1].Tv = 1;
                points[i + 1].Color = Color;

                var k = (((segments+1) * 4)+((segmentsHigh+1)*2)-2)-i;


                points[k].Position = MakePosition(centerX, centerY,
                    (float)(j / (double)segments) * (Width) - (Width / 2),
                    -(Height / 2) + 12f, RotationAngle).Vector4;
 
                points[k].Tu = ((j) % 2);
                points[k].Tv = 0;
                points[k].Color = Color;


                points[k+1].Position = MakePosition(centerX, centerY,
                    (float)(j / (double)segments) * (Width) - (Width / 2),
                    -(Height / 2), RotationAngle).Vector4;
 
                points[k + 1].Tu = (j % 2);
                points[k + 1].Tv = 1;
                points[k + 1].Color = Color;

            }
            var offset = ((segments+1) * 2);
            for (var j = 0; j <= segmentsHigh; j++)
            {
                var top = ((segmentsHigh+1) * 2)+offset-2;
                var i = j * 2 ;

                points[top - i].Position = MakePosition(centerX, centerY, Width / 2, (float)((j / (double)segmentsHigh) * (Height) - (Height / 2)), RotationAngle).Vector4;
 
                points[top-i].Tu = ((j) % 2);
                points[top-i].Tv = 0;
                points[top-i].Color = Color;


                points[top - i + 1].Position = MakePosition(centerX, centerY,
                    (Width / 2) - 12f,
                    (float)((j / (double)segmentsHigh) * Height - ((Height / 2))), RotationAngle).Vector4;

                points[top-i + 1].Tu = (j % 2);
                points[top-i + 1].Tv = 1;
                points[top-i + 1].Color = Color;

                var k = i + ((segments + 1) * 4) + ((segmentsHigh + 1) * 2);

                points[k].Position = MakePosition(centerX, centerY,
                                -(Width / 2) + 12,
                                (float)((j / (double)segmentsHigh) * (Height) - (Height / 2)), RotationAngle).Vector4;
                points[k].Tu = ((j) % 2);
                points[k].Tv = 0;
                points[k].Color = Color;


                points[k + 1].Position = MakePosition(centerX, centerY,
                               - (Width / 2),
                               (float)((j / (double)segmentsHigh) * Height - ((Height / 2))), RotationAngle).Vector4;
                points[k + 1].Tu = (j % 2);
                points[k + 1].Tv = 1;
                points[k + 1].Color = Color;

            }
        }
        PositionColoredTextured[] pnts;
        private void CreateStarGeometry()
        {
            var centerX = X;
            var centerY = Y;
            var radius = Width / 2;

            var radiansPerSegment = ((float)Math.PI * 2) / 5;
            if (points == null)
            {
                points = new PositionColoredTextured[12];
            }

            if (pnts == null)
            {
                pnts = new PositionColoredTextured[10];
            }

            for (var i = 0; i < 5; i++)
            {
                var rads = i * radiansPerSegment - (Math.PI/2) ;
                pnts[i].Position = MakePosition(centerX, centerY, (float)(Math.Cos(rads) * (Width / 2)), (float)(Math.Sin(rads) * (Height / 2)), RotationAngle).Vector4;
                pnts[i].Tu = 0;
                pnts[i].Tv = 0;
                pnts[i].Color = Color;
            }

            for (var i = 5; i < 10; i++)
            {
                var rads = i * radiansPerSegment + (radiansPerSegment / 2) - (Math.PI/2);
                pnts[i].Position = MakePosition(centerX, centerY, (float)(Math.Cos(rads) * (Width / 5.3)), (float)(Math.Sin(rads) * (Height / 5.3)), RotationAngle).Vector4;

                pnts[i].Tu = 0;
                pnts[i].Tv = 0;
                pnts[i].Color = Color;
            }
            points[0] = pnts[0];
            points[1] = pnts[5];
            points[2] = pnts[9];
            points[3] = pnts[1];
            points[4] = pnts[7];
            points[5] = pnts[4];
            points[6] = pnts[6];
            points[7] = pnts[2];
            points[8] = pnts[7];
            points[9] = pnts[7];
            points[10] = pnts[3];
            points[11] = pnts[8];
            TriangleStrip = false;
        }
        private void CreateArrowGeometry()
        {
            if (points == null)
            {
                points = new PositionColoredTextured[9];
            }

            points[0].Position = MakePosition(X, Y, -Width / 2, -Height / 4, RotationAngle).Vector4;
            points[0].Tu = 0;
            points[0].Tv = 0;
            points[0].Color = Color;

            points[1].Position = MakePosition(X, Y, Width / 4, -Height / 4, RotationAngle).Vector4;
            points[1].Tu = 1;
            points[1].Tv = 0;
            points[1].Color = Color;


            points[2].Position = MakePosition(X, Y, -Width / 2, Height / 4, RotationAngle).Vector4;
            points[2].Tu = 0;
            points[2].Tv = 1;
            points[2].Color = Color;

            points[3].Position = MakePosition(X, Y, Width / 4, -Height / 4, RotationAngle).Vector4;
            points[3].Tu = 1;
            points[3].Tv = 0;
            points[3].Color = Color;


            points[4].Position = MakePosition(X, Y, -Width / 2, Height / 4, RotationAngle).Vector4;
            points[4].Tu = 0;
            points[4].Tv = 1;
            points[4].Color = Color;


            points[5].Position = MakePosition(X, Y, Width / 4, Height / 4, RotationAngle).Vector4;
            points[5].Tu = 1;
            points[5].Tv = 1;
            points[5].Color = Color;
        
            // Point

            points[6].Position = MakePosition(X, Y, Width / 4, -Height / 2, RotationAngle).Vector4;
            points[6].Tu = 1;
            points[6].Tv = 1;
            points[6].Color = Color;



            points[7].Position = MakePosition(X, Y, Width / 2, 0, RotationAngle).Vector4;
            points[7].Tu = 1;
            points[7].Tv = .5f;
            points[7].Color = Color;

            points[8].Position = MakePosition(X, Y, Width / 4, Height / 2, RotationAngle).Vector4;
            points[8].Tu = 1;
            points[8].Tv = 1;
            points[8].Color = Color;

            TriangleStrip = false;
        }
        private void CreateDonutGeometry()
        {
            var centerX = X;
            var centerY = Y;
            var radius = Width / 2;

            var circumference = (float)Math.PI * 2.0f * radius;
            var segments = (int)(circumference / 12) + 1;
            var radiansPerSegment = ((float)Math.PI * 2) / segments;
            if (points == null)
            {
                points = new PositionColoredTextured[segments * 2 + 2];
            }

            for (var j = 0; j <= segments; j++)
            {
                var i = j * 2;

                points[i].Position = MakePosition(centerX, centerY, (float)(Math.Cos(j * radiansPerSegment) * (Width / 2)), (float)(Math.Sin(j * radiansPerSegment) * (Height / 2)), RotationAngle).Vector4;
                points[i].Tu = ((j) % 2);
                points[i].Tv = 0;
                points[i].Color = Color;

                points[i + 1].Position = MakePosition(centerX, centerY, (float)(Math.Cos(j * radiansPerSegment) * ((Width / 2) - 10)), (float)(Math.Sin(j * radiansPerSegment) * ((Height / 2) - 10)), RotationAngle).Vector4;
                points[i + 1].Tu = (j % 2);
                points[i + 1].Tv = 1;
                points[i + 1].Color = Color;

            }
        } 
        private void CreateCircleGeometry()
        {
            var centerX = X;
            var centerY = Y;
            var radius = Width / 2;

            var circumference = (float)Math.PI * 2.0f * radius;
            var segments = (int)(circumference / 12)+1;
            var radiansPerSegment = ((float)Math.PI * 2) / segments;
            if (points == null)
            {
                points = new PositionColoredTextured[segments * 2 + 2];
            }
            for (var j = 0; j <= segments; j++)
            {
                var i = j * 2;

                points[i].Position = MakePosition(centerX, centerY, (float)(Math.Cos(j * radiansPerSegment) * (Width / 2)), (float)(Math.Sin(j * radiansPerSegment) * (Height / 2)), RotationAngle).Vector4;
                points[i].Tu = ((j) % 2);
                points[i].Tv = 0;
                points[i].Color = Color;

                points[i + 1].Position = MakePosition(centerX, centerY, 0, 0, RotationAngle).Vector4;
                points[i + 1].Tu = (j % 2);
                points[i + 1].Tv = 1;
                points[i + 1].Color = Color;

            }
        }
        public override void InitializeTexture()
        {
            switch (ShapeType)
            {
                case ShapeType.Line:
                case ShapeType.Donut:
                case ShapeType.OpenRectagle:
                    {
                        var bmp = new Bitmap(13, 10);
                        var g = Graphics.FromImage(bmp);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        Brush brush = new SolidBrush(Color);
                        g.FillEllipse(brush, 1, 0, 10, 9);
                        g.Dispose();
                        texture = Texture11.FromBitmap(bmp);
                        bmp.Dispose();
                    }       
                    break;         
                case ShapeType.Circle:

                case ShapeType.Rectagle:
                case ShapeType.Star:

                case ShapeType.Arrow:
                default:
                    {
                        texture = null;
                    }  
                    break;
            }
        }

        public override void CleanUpGeometry()
        {
            base.CleanUpGeometry();
            CleanUp();
        }

        public override void WriteOverlayProperties(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Shape");
            xmlWriter.WriteAttributeString("ShapeType", shapeType.ToString());
            xmlWriter.WriteEndElement();
        }

        public override void InitializeFromXml(XmlNode node)
        {
            XmlNode shape = node["Shape"];
            shapeType = (ShapeType)Enum.Parse(typeof(ShapeType), shape.Attributes["ShapeType"].Value);
        }
    }
    public class AudioOverlay : Overlay
    {
        string filename;


        AudioPlayer audio;

        public AudioPlayer Audio
        {
            get { return audio; }
            set { audio = value; }
        }
        int volume = 100;

        bool mute;

        public bool Mute
        {
            get { return mute; }
            set
            {
                mute = value;
                Volume = Volume;
            }
        }

        public int Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                if (audio != null)
                {
                    if (mute)
                    {
                        //todo11 check volume levels

                        audio.Volume = 0;
                    }
                    else
                    {
                        audio.Volume = volume /100f;
                    }
                }
            }
        }

        double begin;

        public double Begin
        {
            get { return begin; }
            set { begin = value; }
        }
        double end;

        public double End
        {
            get { return end; }
            set { end = value; }
        }
        double fadeIn;

        public double FadeIn
        {
            get { return fadeIn; }
            set { fadeIn = value; }
        }
        double fadeOut;

        public double FadeOut
        {
            get { return fadeOut; }
            set { fadeOut = value; }
        }

        private bool loop;

        public bool Loop
        {
            get { return loop; }
            set { loop = value; }
        }

        public AudioOverlay()
        {
            isDesignTimeOnly = true;
        }

        public override void AddFilesToCabinet(FileCabinet fc)
        {
            fc.AddFile(Owner.Owner.WorkingDirectory + filename);
        }

       
        public override void Play()
        {
            if (audio == null)
            {
                InitializeTexture();
            }
            if (audio != null)
            {
                if (mute)
                {
                    audio.Volume = 0f;
                }
                else
                {
                    audio.Volume = volume / 100f;
                }
                Seek(0);

                audio.Play();
            }
        }

        public override void Pause()
        {   
            if (audio == null)
            {
                InitializeTexture();
            }
            if (audio != null)
            {
                audio.Pause();
            }
        }

        public override void Stop()
        {    
            if (audio == null)
            {
                InitializeTexture();
            }
            if (audio != null)
            {
                audio.Stop();
            }
        }

       
        public override void Seek(double time)
        {

            time += begin;
            if (audio == null)
            {
                InitializeTexture();
            }
            if (audio != null)
            {
                if (audio.Duration > time )
                {
                    audio.Seek(0);
                    audio.Seek(time);
                    var d = audio.CurrentPosition;
                    var b = d;
                }
                else
                {
                    audio.Stop();
                }
            }
        }

        public AudioOverlay( TourStop owner, string filename)
        {
            isDesignTimeOnly = true;
            X = 0;
            Y = 0;
            this.filename = Guid.NewGuid() + filename.Substring(filename.LastIndexOf("."));
            Owner = owner;
            Name = owner.GetNextDefaultName("Audio");
            File.Copy(filename, Owner.Owner.WorkingDirectory + this.filename);
        }

        public override void InitializeTexture()
        {
            try
            {

                audio = new AudioPlayer(Owner.Owner.WorkingDirectory + filename);
                audio.PlaybackComplete += audio_Ending;
            }
            catch
            {
            }
 
        }

        void audio_Ending(object sender, EventArgs e)
        {
            if (loop)
            {
                Play();
            }
        }

        public override void CleanUp()
        {
            base.CleanUp();

            if (audio != null)
            {
                audio.Stop();
                audio.PlaybackComplete -= audio_Ending;
                audio.Dispose();
                audio = null;
            }
        
        }

        AudioType trackType = AudioType.Music;

        public AudioType TrackType
        {
            get { return trackType; }
            set { trackType = value; }
        }

        public override void WriteOverlayProperties(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Audio");
            xmlWriter.WriteAttributeString("Filename", filename);
            xmlWriter.WriteAttributeString("Volume", volume.ToString());
            xmlWriter.WriteAttributeString("Mute", mute.ToString());
            xmlWriter.WriteAttributeString("TrackType", trackType.ToString());
            xmlWriter.WriteAttributeString("Begin", begin.ToString());
            xmlWriter.WriteAttributeString("End", end.ToString());
            xmlWriter.WriteAttributeString("FadeIn", fadeIn.ToString());
            xmlWriter.WriteAttributeString("FadeOut", fadeOut.ToString());
            xmlWriter.WriteAttributeString("Loop", loop.ToString());
            xmlWriter.WriteEndElement();
        }

        public override void InitializeFromXml(XmlNode node)
        {
            XmlNode audio = node["Audio"];
            filename = audio.Attributes["Filename"].Value;
            if (audio.Attributes["Volume"] != null)
            {
                volume = Convert.ToInt32(audio.Attributes["Volume"].Value);
            }

            if (audio.Attributes["Mute"] != null)
            {
                mute = Convert.ToBoolean(audio.Attributes["Mute"].Value);
            }

            if (audio.Attributes["TrackType"] != null)
            {
                trackType = (AudioType)Enum.Parse(typeof(AudioType), audio.Attributes["TrackType"].Value);
            }

            if (audio.Attributes["Begin"] != null)
            {
                begin = double.Parse(audio.Attributes["Begin"].Value);
            }

            if (audio.Attributes["End"] != null)
            {
                end = double.Parse(audio.Attributes["End"].Value);
            }

            if (audio.Attributes["FadeIn"] != null)
            {
                fadeIn = double.Parse(audio.Attributes["FadeIn"].Value);
            }

            if (audio.Attributes["FadeOut"] != null)
            {
                fadeOut = double.Parse(audio.Attributes["FadeOut"].Value);
            }

            if (audio.Attributes["Loop"] != null)
            {
                loop = bool.Parse(audio.Attributes["Loop"].Value);
            }
        }
    }
    
    public enum LoopTypes { Loop, UpDown, Down, UpDownOnce, Once, Begin, End };

    public class FlipbookOverlay : Overlay
    {
        string filename;

        LoopTypes loopType = LoopTypes.UpDown;

        public LoopTypes LoopType
        {
            get { return loopType; }
            set { loopType = value; }
        }

        int startFrame;

        public int StartFrame
        {
            get { return startFrame; }
            set { startFrame = value; }
        }
        List<int> framesList = new List<int>();
        string frameSequence;

        public string FrameSequence
        {
            get { return frameSequence; }
            set 
            {
                if (frameSequence != value)
                {
                    frameSequence = value;
                    framesList = new List<int>();
                    if (!string.IsNullOrEmpty(frameSequence))
                    {
                        try
                        {
                            var parts = frameSequence.Split(new[] { ',' });
                            foreach (var part in parts)
                            {
                                var x = Convert.ToInt32(part.Trim());
                                framesList.Add(x);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        int frames = 1;

        public int Frames
        {
            get { return frames; }
            set
            {
                frames = value;
            }
        }

        int framesX = 8;

        public int FramesX
        {
            get { return framesX; }
            set { framesX = value; }
        }
        int framesY = 8;

        public int FramesY
        {
            get { return framesY; }
            set { framesY = value; }
        }



        public FlipbookOverlay()
        {

        }

        public FlipbookOverlay( TourStop owner, string filename)
        {
            Owner = owner;


            var extension = filename.Substring(filename.LastIndexOf("."));

            this.filename = Guid.NewGuid() + extension;

            Name = filename.Substring(filename.LastIndexOf('\\') +1);
            File.Copy(filename, Owner.Owner.WorkingDirectory + this.filename);

            var bmp = new Bitmap(Owner.Owner.WorkingDirectory + this.filename);
            Width = 256;
            Height = 256;
            bmp.Dispose();
            bmp = null;
            X = 0;
            Y = 0;
        }


        public FlipbookOverlay( TourStop owner, Image image)
        {
            Owner = owner;
            // to make directory and guid filename in tour temp dir.
            filename = Guid.NewGuid() + ".png";

            Name = owner.GetNextDefaultName("Image");
            X = 0;
            Y = 0;
            image.Save(Owner.Owner.WorkingDirectory + filename, ImageFormat.Png);
            Width = 256;
            Height = 256;
        }

        public FlipbookOverlay Copy(TourStop owner)
        {
            //todo fix this
            var newFlipbookOverlay = new FlipbookOverlay();
            newFlipbookOverlay.Owner = owner;
            newFlipbookOverlay.filename = filename;
            newFlipbookOverlay.X = X;
            newFlipbookOverlay.Y = Y;
            newFlipbookOverlay.Width = Width;
            newFlipbookOverlay.Height = Height;
            newFlipbookOverlay.Color = Color;
            newFlipbookOverlay.Opacity = Opacity;
            newFlipbookOverlay.RotationAngle = RotationAngle;
            newFlipbookOverlay.Name = Name + " - Copy";
            newFlipbookOverlay.StartFrame = StartFrame;
            newFlipbookOverlay.Frames = Frames;
            newFlipbookOverlay.LoopType = LoopType;
            newFlipbookOverlay.FrameSequence = FrameSequence;
            newFlipbookOverlay.FramesX = FramesX;
            newFlipbookOverlay.FramesY = FramesY;

            return newFlipbookOverlay;
        }

        public override void CleanUp()
        {
            texture = null;
        }

        public override void AddFilesToCabinet(FileCabinet fc)
        {
            fc.AddFile(Owner.Owner.WorkingDirectory + filename);
        }

        public override void InitializeTexture()
        {
            try
            {
                var colorKey = filename.ToLower().EndsWith(".jpg");
                texture = Owner.Owner.GetCachedTexture( Owner.Owner.WorkingDirectory + filename, colorKey);
                
                
                if (Width == 0 && Height == 0)
                {
                    Width = texture.Width;
                    Height = texture.Height;

                }
            }
            catch
            {
                texture = Texture11.FromBitmap(Resources.BadImage);
                {
                    Width = texture.Width;
                    Height = texture.Height;

                }
            }
        }

        public override void WriteOverlayProperties(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Flipbook");
            xmlWriter.WriteAttributeString("Filename", filename);
            xmlWriter.WriteAttributeString("Frames", frames.ToString());
            xmlWriter.WriteAttributeString("Loop", loopType.ToString());
            xmlWriter.WriteAttributeString("FramesX", framesX.ToString());
            xmlWriter.WriteAttributeString("FramesY", framesY.ToString());
            xmlWriter.WriteAttributeString("StartFrame", startFrame.ToString());
            if (!string.IsNullOrEmpty(frameSequence))
            {
                xmlWriter.WriteAttributeString("FrameSequence", frameSequence);
            }
            xmlWriter.WriteEndElement();
        }

        public override void InitializeFromXml(XmlNode node)
        {
            XmlNode flipbook = node["Flipbook"];
            filename = flipbook.Attributes["Filename"].Value;
            frames = Convert.ToInt32(flipbook.Attributes["Frames"].Value);
            loopType = (LoopTypes)Enum.Parse(typeof(LoopTypes), flipbook.Attributes["Loop"].Value);

            if (flipbook.Attributes["FramesX"] != null)
            {
                FramesX = Convert.ToInt32(flipbook.Attributes["FramesX"].Value);
            }
            if (flipbook.Attributes["FramesY"] != null)
            {
                FramesY = Convert.ToInt32(flipbook.Attributes["FramesY"].Value);
            }
            if (flipbook.Attributes["StartFrame"] != null)
            {
                StartFrame = Convert.ToInt32(flipbook.Attributes["StartFrame"].Value);
            }
            if (flipbook.Attributes["FrameSequence"] != null)
            {
                FrameSequence = flipbook.Attributes["FrameSequence"].Value;
            }
        }

        int currentFrame;
        //int widthCount = 8;
        //int heightCount = 8;
        int cellHeight = 256;
        int cellWidth = 256;
        DateTime timeStart = DateTime.Now;
        bool playing = true;
        public override void Play()
        {
            playing = true;
            timeStart = SpaceTimeController.MetaNow;
        }
        public override void Pause()
        {
            playing = false;
        }
        public override void Stop()
        {
            playing = false;
            currentFrame = 0;
        }

        override public void InitiaizeGeometry()
        {
            var frameCount = frames;
            if (!String.IsNullOrEmpty(frameSequence))
            {
                frameCount = framesList.Count;
            }

            if (playing)
            {
                // todo allow play backwards loop to point.
                var ts = SpaceTimeController.MetaNow - timeStart;
                switch (loopType)
                {
                    case LoopTypes.Loop:
                        currentFrame = (int)((ts.TotalSeconds * 24.0) % frameCount) +startFrame;
                        break;
                    case LoopTypes.UpDown:
                        currentFrame = Math.Abs((int)((ts.TotalSeconds * 24.0 + frameCount) % (frameCount * 2 - 1)) - (frameCount - 1)) + startFrame;
                        break;
                    case LoopTypes.Down:
                        currentFrame = Math.Max(0, frameCount - (int)((ts.TotalSeconds * 24.0) % frameCount)) + startFrame;
                        break;
                    case LoopTypes.UpDownOnce:
                        var temp = (int)Math.Min(ts.TotalSeconds * 24.0,frameCount *2 +1) + frameCount;
                        currentFrame = Math.Abs((temp) % (frameCount * 2 - 1) - (frameCount - 1)) + startFrame;
                        break;
                    case LoopTypes.Once:
                        currentFrame = Math.Min(frameCount - 1, (int)((ts.TotalSeconds * 24.0)));
                        break;
                    case LoopTypes.Begin:
                        currentFrame = startFrame;
                        break;
                    case LoopTypes.End:
                        currentFrame = (frameCount - 1) + startFrame;
                        break;
                    default:
                        currentFrame = startFrame;
                        break;
                }
                
            }
            if (!String.IsNullOrEmpty(frameSequence))
            {
                if (currentFrame < framesList.Count && currentFrame > -1)
                {
                    currentFrame = framesList[currentFrame];
                }
                else
                {
                    currentFrame = 0;
                }
            }

            currentRotation = 0;
            if (points == null)
            {
                points = new PositionColoredTextured[4];
            }
            var cellHeight = 1f / framesY;
            var cellWidth = 1f / framesX;

            var indexX = currentFrame % framesX;
            var indexY = currentFrame / framesX;

            points[0].Position = MakePosition(X, Y, -(Width / 2), -(Height / 2), RotationAngle).Vector4;
            points[0].Tu = indexX * cellWidth;
            points[0].Tv = indexY * cellHeight;
            points[0].Color = Color;


            points[1].Position = MakePosition(X, Y, (Width / 2), -(Height / 2), RotationAngle).Vector4;
            points[1].Tu = indexX * cellWidth + cellWidth;
            points[1].Tv = indexY * cellHeight;
            points[1].Color = Color;


            points[2].Position = MakePosition(X, Y, -(Width / 2), (Height / 2), RotationAngle).Vector4;
            points[2].Tu = indexX * cellWidth;
            points[2].Tv = indexY * cellHeight + cellHeight;
            points[2].Color = Color;

            points[3].Position = MakePosition(X, Y, (Width / 2), (Height / 2), RotationAngle).Vector4;
            points[3].Tu = indexX * cellWidth + cellWidth;
            points[3].Tv = indexY * cellHeight + cellHeight;
            points[3].Color = Color;
        }
    } 
}
