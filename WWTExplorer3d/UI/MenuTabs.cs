using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TerraViewer.Properties;

namespace TerraViewer
{
    public enum ApplicationMode { Explore, Tours, Search, Community, Telescope, View, Settings, Tour1, Tour2, Tour3, Tour4, Tour5 };
    public enum ControlAction { Close, Minimize, Maximize, Restore, AppMenu, CloseTour, SignOut, SignIn };

    public delegate void TabClickedEventHandler(object sender, ApplicationMode e);
    public delegate void MenuClickedEventHandler(object sender, ApplicationMode e);
    public delegate void ControlEventHandler(object sender, ControlAction e);

    public partial class MenuTabs : UserControl
    {
        readonly Bitmap tabHoverTab = Resources.tabHoverTab;
        readonly Bitmap tabHoverMenu = Resources.tabHoverMenu;
        readonly Bitmap selectedTab = Resources.tabSelected;
        readonly Bitmap selectedHoverTab = Resources.tabSelectedHover;
        Bitmap hoverTab = Resources.tabHover;
        readonly Bitmap tourSelectedTab = Resources.tourSelected;
        readonly Bitmap tourHoverTab = Resources.tourHover;
        readonly Bitmap tourSelectedHoverTab = Resources.tourSelectedHover;
        readonly Bitmap closeRest = Resources.CloseRest;
        readonly Bitmap closeHover = Resources.CloseHover;
        readonly Bitmap closePush = Resources.ClosePush;
        readonly Bitmap menuArrow = Resources.menuArrow;

        public event TabClickedEventHandler TabClicked;
        public event MenuClickedEventHandler MenuClicked;
        public event ControlEventHandler ControlEvent;
        public MenuTabs()
        {
            tabs = new List<string>();
            tabs.Add(Language.GetLocalizedText(134, "Explore"));
            tabs.Add(Language.GetLocalizedText(135, "Guided Tours"));
            tabs.Add(Language.GetLocalizedText(137, "Search"));
            tabs.Add(Language.GetLocalizedText(138, "Community"));
            tabs.Add(Language.GetLocalizedText(139, "Telescope"));
            tabs.Add(Language.GetLocalizedText(140, "View"));
            tabs.Add(Language.GetLocalizedText(141, "Settings"));
            tours = new List<TourDocument>();

            InitializeComponent();

        }
        int freezeCount;

        public bool Frozen
        {
            get { return freezeCount > 0; }
        }
        public  void Freeze()
        {
            freezeCount++;            
        }

        public void Thaw()
        {
            freezeCount--;
            if (freezeCount == 0)
            {
                hoverTabIndex = -1;
                onCloseButton = false;
                onMenuArrow = false;
                Refresh();
            }
        }

        public void ShowTabMenu(ApplicationMode appMode)
        {
            hoverTabIndex = (int)appMode;
            Refresh();
            MenuClicked.Invoke(this, appMode);
        }
        public void ShowTabMenu(int relativeIndex)
        {
            hoverTabIndex += relativeIndex;
            if (hoverTabIndex >= tabs.Count)
            {
                hoverTabIndex = 0;
            }

            if (hoverTabIndex < 0)
            {
                hoverTabIndex += tabs.Count;
            }

            Refresh();
            MenuClicked.Invoke(this, (ApplicationMode)hoverTabIndex);
        }
        private void MenuTabs_Load(object sender, EventArgs e)
        {

        }

        public virtual String this[int i]
        {
            get
            {
                return "";
            }
            set
            {

            }
        }
        private readonly List<string> tabs;
        private readonly List<TourDocument> tours;

        public List<TourDocument> Tours
        {
            get { return tours; }
        }

        TourDocument currentTour;

        public TourDocument CurrentTour
        {
            get { return currentTour; }
            set { currentTour = value; }
        }

        int selectedTabIndex;

        public int SelectedTabIndex
        {
            get { return selectedTabIndex; }
            set { selectedTabIndex = value; }
        }
        int hoverTabIndex = -1;
        int startX;

        public int StartX
        {
            get { return startX; }
            set { startX = value; }
        }
        bool onCloseButton;
        bool onMenuArrow;
        bool onSignOut;
        public void AddTour(TourDocument tour)
        {
            tours.Add(tour);
            Refresh();
        }

        public void RemoveTour(TourDocument targetTour)
        {
            tours.Remove(targetTour);
            if (currentTour == targetTour)
            {
                currentTour = null;
                // todo shift to another tour?

            }
        }

        public void FocusTour(TourDocument targetTour)
        {
            var index = 0;
            var maxIndex = (tabs.Count);

            foreach (var tour in Tours)
            {
                if (tour == targetTour)
                {
                    CurrentTour = targetTour;
                    SetSelectedIndex(maxIndex + index, false);
                    Refresh();
                    return;
                }
                index++;
            }
        }

        private void MenuTabs_Paint(object sender, PaintEventArgs e)
        {
            if (Width > 1124)
            {
                startX = 140;
            }
            else
            {
                startX = 0;
            }
            var g = e.Graphics;
            var font = new Font("Segoe UI", 10, FontStyle.Regular);
            Brush brush = new SolidBrush(Color.White);
            var tabIndex = 0;
            var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.Trimming = StringTrimming.EllipsisCharacter;
            format.FormatFlags = StringFormatFlags.NoWrap;


            foreach (var tab in tabs)
            {
                var selectedOrHovered = (tabIndex == selectedTabIndex && (hoverTabIndex == -1)) || tabIndex == hoverTabIndex;
                if (tabIndex == selectedTabIndex)
                {
                    if (tabIndex == hoverTabIndex)
                    {
                        g.DrawImage(selectedHoverTab, (tabIndex * 100) + startX, 0f);
                    }
                    else
                    {
                        g.DrawImage(selectedTab, (tabIndex * 100) + startX, 0f);
                    }
                    if (selectedOrHovered)
                    {
                        g.DrawImage(menuArrow, (tabIndex * 100) + startX + 47, 25); 
                    }

                }
                else
                {
                    if (tabIndex == hoverTabIndex)
                    {
                        if (onMenuArrow)
                        {
                            g.DrawImage(tabHoverMenu, (tabIndex * 100) + startX, 0f);
                        }
                        else
                        {
                            g.DrawImage(tabHoverTab, (tabIndex * 100) + startX, 0f);
                        }

                    }        
                }


                var rect = new RectangleF(((tabIndex*100)+startX),6,95,27);

                g.DrawString(tab, font, brush, rect,format);
                tabIndex++;
            }
            var j = 0;
            foreach (var tour in tours)
            {
                var drawPositionX = (tabIndex * 100) + startX + j * 100;

                if (tabIndex == selectedTabIndex)
                {
                    if (tabIndex == hoverTabIndex)
                    {
                        g.DrawImage(tourSelectedHoverTab, drawPositionX, 0f);
                        if (onCloseButton && mouseDown)
                        {
                            g.DrawImage(closePush, drawPositionX + 182, 5f);
                        }
                        else if (onCloseButton)
                        {
                            g.DrawImage(closeHover, drawPositionX + 182, 5f);
                        }
                        else
                        {
                            g.DrawImage(closeRest, drawPositionX + 182, 5f);
                        }
                    }
                    else
                    {
                        g.DrawImage(tourSelectedTab, drawPositionX, 0f);
                        g.DrawImage(closeRest, drawPositionX + 182, 5f);
                    }

                }
                else
                {
                    if (tabIndex == hoverTabIndex)
                    {
                        g.DrawImage(tourHoverTab, drawPositionX, 0f);
                        if (onCloseButton && mouseDown)
                        {
                            g.DrawImage(closePush, drawPositionX + 182, 5f);
                        }
                        else if (onCloseButton)
                        {
                            g.DrawImage(closeHover, drawPositionX + 182, 5f);
                        }
                        else
                        {
                            g.DrawImage(closeRest, drawPositionX + 182, 5f);
                        }
                    }
                }
                j++;

                var rect = new RectangleF(drawPositionX, 6, 195, 27);

                g.DrawString(tour.Title, font, brush, rect, format);
                tabIndex++;

            }

            if (Earth3d.IsLoggedIn)
            {
                var rect2 = new RectangleF((Width - 80), 6, 80, 27);

                g.DrawString(Language.GetLocalizedText(1025, "Sign Out"), font, brush, rect2, format);

                g.DrawLine(Pens.LightBlue, Width - 80, 0, Width - 80, 30);
            }
            else
            {
                var rect2 = new RectangleF((Width - 80), 6, 80, 27);

                g.DrawString(Language.GetLocalizedText(962, "Sign In"), font, brush, rect2, format);

                g.DrawLine(Pens.LightBlue, Width - 80, 0, Width - 80, 30);
            }
        }

        public bool IsVisible 
        {
            get
            {
                return Visible;
            }
            set
            {
                Visible = value;
            }
        }

        static public bool MouseInTabs = false;
        private void MenuTabs_MouseLeave(object sender, EventArgs e)
        {
            MouseInTabs = false;
            if (!Frozen)
            {
                hoverTabIndex = -1;
                Refresh();
            }
        }

        private void MenuTabs_MouseEnter(object sender, EventArgs e)
        {
            MouseInTabs = true;
            if (!Frozen)
            {
                var mouse = PointToClient(Cursor.Position);

                hoverTabIndex = GetTabIndexFromPoint(mouse, out onCloseButton, out onMenuArrow, out onSignOut);
                Refresh();
            }
        }

        private int GetTabIndexFromPoint(Point pnt, out bool onClose, out bool onMenu, out bool onSignOut)
        {
            var index = (pnt.X - startX) / 100;
            var maxIndex = (tabs.Count - 1);

            onClose = false;
            onMenu = false;
            onSignOut = false;

            if (pnt.X > (Width -80))
            {
                onSignOut = true;
                return -1;
            }


            // Adjusts for tours being double wide...
            if (index > maxIndex)
            {
                var tourIndex = (((index - maxIndex)+1) / 2)-1;
                index = tourIndex + maxIndex+1;

                if (tourIndex < tours.Count)
                {
                    currentTour = tours[tourIndex];
                    var closeStart = startX + (tabs.Count * 100 + tourIndex * 200) + 182;

                    if (pnt.Y > 4 && pnt.Y < 18 && pnt.X > closeStart && pnt.X < (closeStart + 13))
                    {
                        onClose = true;
                    }

                }
            }
            if (pnt.Y > 23 && ! onClose)
            {
                onMenu = true;
            }
            return index;
        }

        private void MenuTabs_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Frozen)
            {
                hoverTabIndex = GetTabIndexFromPoint(e.Location, out onCloseButton, out onMenuArrow, out onSignOut);
                Refresh();
            }
        }

        private void MenuTabs_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var tempIndex = GetTabIndexFromPoint(e.Location, out onCloseButton, out onMenuArrow, out onSignOut);

                if (onCloseButton)
                {
                    ControlEvent.Invoke(this, ControlAction.CloseTour);
                    return;
                }

                if (onSignOut)
                {
                    if (Earth3d.IsLoggedIn)
                    {
                        ControlEvent.Invoke(this, ControlAction.SignOut);
                    }
                    else
                    {
                        ControlEvent.Invoke(this, ControlAction.SignIn);
                    }
                    return;
                }
                SetSelectedIndex(tempIndex, onMenuArrow);
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (e.X < startX)
                {

                    ControlEvent.Invoke(this, ControlAction.AppMenu);

                }

            }
        }

        public void SetSelectedIndex(int targetIndex, bool menuClicked)
        {
      
            if (menuClicked || targetIndex == selectedTabIndex)
            {
                if (MenuClicked != null)
                {
                    MenuClicked.Invoke(this, (ApplicationMode)targetIndex);

                }
            }
            else
            {
                selectedTabIndex = targetIndex;

                if (TabClicked != null)
                {
                    TabClicked.Invoke(this, (ApplicationMode)selectedTabIndex);
                    Refresh();
                }
            }
        }
        public bool Maximized = true;

        private void MenuTabs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var index = (e.X - startX) / 100;

            if (ControlEvent == null)
            {
                return;
            }
            if (e.X < startX)
            {
                if (Maximized)
                {
                    ControlEvent.Invoke(this, ControlAction.Restore);
                }
                else
                {
                    ControlEvent.Invoke(this, ControlAction.Maximize);
                }
            }

        }
        bool mouseDown;
        private void MenuTabs_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            Refresh();
        }

        private void MenuTabs_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            Refresh();

        }
    }
}