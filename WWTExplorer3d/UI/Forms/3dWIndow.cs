using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SpaceNavigator;
using TerraViewer.Authentication;
using TerraViewer.Callibration;
using TerraViewer.org.worldwidetelescope.www;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;
using System.Text;
using TerraViewer.edu.stsci.masthla;
using Microsoft.XInput;
using TerraViewer.Properties;
using WwtDataUtils;
using System.Net.NetworkInformation;
using Registry = Microsoft.Win32.Registry;
using System.Threading.Tasks;
using System.Security;
using Color = System.Drawing.Color;
using Matrix = SharpDX.Matrix;
using Message = System.Windows.Forms.Message;
using Rectangle = System.Drawing.Rectangle;
using Timer = System.Windows.Forms.Timer;

namespace TerraViewer
{
    #region navigation enums

    internal enum NavigationProperties
    {
        Ra,
        Declination,
        Latitude,
        Longitude,
        Zoom,
        Angle,
        Rotation,
        ZoomRate,
        PanUpDownRate,
        PanLeftRightRate,
        RotationRate,
        NavigationHold,
        ImageCrossfade,
        FadeToBlack,
        SystemVolume,
        DomeAlt,
        DomeAz,
        DomeTilt,
        DomeAngle,
        FisheyeAngle,
        ScreenFOV
    };

    internal enum NavigationActions
    {
        MoveUp,
        MoveDown,
        MoveRight,
        MoveLeft,
        ZoomIn,
        ZoomOut,
        TiltUp,
        TiltDown,
        RotateLeft,
        RotateRight,
        DomeLeft,
        DomeRight,
        DomeUp,
        DomeDown,
        AllStop,
        NextItem,
        LastItem,
        Select,
        Back,
        SetForeground,
        SetBackground,
        NextMode,
        PreviousMode,
        ResetCamera,
        SolarSystemMode,
        SkyMode,
        EarthMode,
        PlanetMode,
        PanoramaMode,
        SandboxMode,
        PlayTour,
        PauseTour,
        StopTour,
        NextSlide,
        PreviousSlide,
        MoveToFirstSlide,
        MoveToEndSlide,
        ShowNextContext,
        ShowPreviousContext,
        ShowNextExplore,
        ShowPreviousExplore,
        ResetRiftView,
        GotoSun,
        GotoMercury,
        GotoVenus,
        GotoEarth,
        GotoMars,
        GotoJupiter,
        GotoSaturn,
        GotoUranus,
        GotoNeptune,
        GotoPluto,
        SolarSystemOverview,
        GotoMilkyWay,
        GotoSDSSGalaxies,
        GotoSlide
    };
    #endregion

    public class Earth3d : Form, IScriptable
    {
        #region private fields
        private ToolStripMenuItem toggleFullScreenModeF11ToolStripMenuItem;
        private ToolStripMenuItem nEDSearchToolStripMenuItem;
        private ToolStripMenuItem sDSSSearchToolStripMenuItem;
        private ToolStripMenuItem vOTableToolStripMenuItem;
        private ToolStripMenuItem vORegistryToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem stereoToolStripMenuItem;
        private ToolStripMenuItem enabledToolStripMenuItem;
        private ToolStripMenuItem anaglyphToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator20;
        private ToolStripMenuItem sideBySideProjectionToolStripMenuItem;
        private ToolStripMenuItem sideBySideCrossEyedToolStripMenuItem;
        private ToolStripMenuItem expermentalToolStripMenuItem;
        private ToolStripMenuItem fullDomeToolStripMenuItem;
        private ToolStripMenuItem lookUpOnNEDToolStripMenuItem;
        private ToolStripMenuItem domeSetupToolStripMenuItem;
        private ToolStripMenuItem anaglyphYellowBlueToolStripMenuItem;
        private ToolStripMenuItem listenUpBoysToolStripMenuItem;
        private ToolStripMenuItem sAMPToolStripMenuItem;
        private ToolStripMenuItem sendImageToToolStripMenuItem;
        private ToolStripMenuItem broadcastToolStripMenuItem;
        private ToolStripMenuItem sendTableToToolStripMenuItem;
        private ToolStripMenuItem broadcastToolStripMenuItem1;
        private ToolStripMenuItem imageStackToolStripMenuItem;
        private ToolStripMenuItem addToImageStackToolStripMenuItem;
        private ToolStripMenuItem startupToolStripMenuItem;
        private ToolStripMenuItem earthToolStripMenuItem;
        private ToolStripMenuItem skyToolStripMenuItem;
        private ToolStripMenuItem planetToolStripMenuItem;
        private ToolStripMenuItem solarSystemToolStripMenuItem;
        private ToolStripMenuItem panoramaToolStripMenuItem;
        private ToolStripMenuItem lastToolStripMenuItem;
        private ToolStripMenuItem randomToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripMenuItem musicAndOtherTourResourceToolStripMenuItem;
        private ToolStripMenuItem lookUpOnSDSSToolStripMenuItem;
        private ToolStripMenuItem detachMainViewToSecondMonitor;
        private ToolStripMenuItem shapeFileToolStripMenuItem;
        private ToolStripMenuItem showLayerManagerToolStripMenuItem;
        private ToolStripMenuItem regionalDataCacheToolStripMenuItem;
        private ToolStripMenuItem addAsNewLayerToolStripMenuItem;
        private ToolStripMenuItem addCollectionAsTourStopsToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripMenuItem multiChanelCalibrationToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripMenuItem sendLayersToProjectorServersToolStripMenuItem;
        private ToolStripMenuItem renderToVideoToolStripMenuItem;
        private ToolStripMenuItem showTouchControlsToolStripMenuItem;
        private KioskTitleBar kioskTitleBar;
        private ToolStripMenuItem saveCurrentViewImageToFileToolStripMenuItem;
        private ToolStripMenuItem remoteAccessControlToolStripMenuItem;
        private ToolStripMenuItem layerManagerToolStripMenuItem;
        private ToolStripMenuItem screenBroadcastToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator21;
        private ToolStripMenuItem monochromeStyleToolStripMenuItem;
        private ToolStripMenuItem layersToolStripMenuItem;
        private ToolStripMenuItem publishTourToCommunityToolStripMenuItem;
        private ToolStripMenuItem findEarthBasedLocationToolStripMenuItem;
        private ToolStripMenuItem fullDomePreviewToolStripMenuItem;
        private Timer DeviceHeartbeat;
        private ToolStripMenuItem mIDIControllerSetupToolStripMenuItem;
        private ToolStripMenuItem multiSampleAntialiasingToolStripMenuItem;
        private ToolStripMenuItem noneToolStripMenuItem;
        private ToolStripMenuItem fourSamplesToolStripMenuItem;
        private ToolStripMenuItem eightSamplesToolStripMenuItem;
        private ToolStripMenuItem sendTourToProjectorServersToolStripMenuItem;
        private ToolStripMenuItem clientNodeListToolStripMenuItem;
        private ToolStripMenuItem restoreCacheFromCabinetFileToolStripMenuItem;
        private ToolStripMenuItem saveCacheAsCabinetFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator22;
        private ToolStripMenuItem cacheManagementToolStripMenuItem1;
        private ToolStripMenuItem cacheImageryTilePyramidToolStripMenuItem;
        private ToolStripMenuItem showCacheSpaceUsedToolStripMenuItem;
        private ToolStripMenuItem removeFromImageCacheToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator23;
        private ToolStripMenuItem automaticTourSyncWithProjectorServersToolStripMenuItem;
        private ToolStripMenuItem alternatingLinesOddToolStripMenuItem;
        private ToolStripMenuItem alternatingLinesEvenToolStripMenuItem;
        private ToolStripMenuItem lockVerticalSyncToolStripMenuItem;
        private ToolStripMenuItem targetFrameRateToolStripMenuItem;
        private ToolStripMenuItem fPSToolStripMenuItem60;
        private ToolStripMenuItem fPSToolStripMenuItem30;
        private ToolStripMenuItem fpsToolStripMenuItemUnlimited;
        private ToolStripMenuItem fPSToolStripMenuItem24;
        private ToolStripMenuItem showOverlayListToolStripMenuItem;
        private ToolStripMenuItem tileLoadingThrottlingToolStripMenuItem;
        private ToolStripMenuItem tpsToolStripMenuItem15;
        private ToolStripMenuItem tpsToolStripMenuItem30;
        private ToolStripMenuItem tpsToolStripMenuItem60;
        private ToolStripMenuItem tpsToolStripMenuItem120;
        private ToolStripMenuItem tpsToolStripMenuItemUnlimited;
        private ToolStripMenuItem oculusRiftToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem11;
        private ToolStripMenuItem detachMainViewToThirdMonitorToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem10;
        private ToolStripMenuItem allowUnconstrainedTiltToolStripMenuItem;
        private ToolStripMenuItem showSlideNumbersToolStripMenuItem;
        private ToolStripMenuItem showKeyframerToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem12;
        private ToolStripSeparator toolStripMenuItem13;
        private ToolStripMenuItem xBoxControllerSetupToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem14;
        private ToolStripMenuItem ShowWelcomeTips;
        private ToolStripMenuItem customGalaxyFileToolStripMenuItem;
        private ToolStripMenuItem newFullDomeViewInstanceToolStripMenuItem;
        private ToolStripMenuItem monitorOneToolStripMenuItem;
        private ToolStripMenuItem monitorTwoToolStripMenuItem;
        private ToolStripMenuItem monitorThreeToolStripMenuItem;
        private ToolStripMenuItem monitorFourToolStripMenuItem;
        private ToolStripMenuItem monitorFiveToolStripMenuItem;
        private ToolStripMenuItem monitorSixToolStripMenuItem;
        private ToolStripMenuItem monitorSevenToolStripMenuItem;
        private ToolStripMenuItem monitorEightToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem15;
        private ToolStripMenuItem exportCurrentViewAsSTLFileFor3DPrintingToolStripMenuItem;

        private bool findingTargetGeo;
        private bool zoomingUp;
        private bool smoothZoom = true;
        private Timer InputTimer;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem nameToolStripMenuItem;
        private Timer HoverTimer;
        private ContextMenuStrip communitiesMenu;
        private ContextMenuStrip searchMenu;
        private ContextMenuStrip toursMenu;
        private ContextMenuStrip telescopeMenu;
        private ContextMenuStrip exploreMenu;
        private ContextMenuStrip settingsMenu;
        private ContextMenuStrip viewMenu;
        private ToolStripMenuItem openFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem exitMenuItem;
        private ToolStripMenuItem createNewObservingListToolStripMenuItem;
        private ToolStripMenuItem newObservingListpMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem newSimpleTourMenuItem;
        private ToolStripMenuItem openTourMenuItem;
        private ToolStripMenuItem openObservingListMenuItem;
        private ToolStripMenuItem openImageMenuItem;
        private ToolStripMenuItem openKMLMenuItem;
        private ToolStripMenuItem tourHomeMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem tourSearchWebPageMenuItem;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem homepageMenuItem;
        private ToolStripMenuItem aboutMenuItem;
        private ToolStripMenuItem publishTourMenuItem;
        private ToolStripMenuItem joinCoomunityMenuItem;
        private ToolStripMenuItem updateLoginCredentialsMenuItem;
        private ToolStripMenuItem logoutMenuItem;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem uploadObservingListToCommunityMenuItem;
        private ToolStripMenuItem uploadImageToCommunityMenuItem;
        private ToolStripMenuItem resetCameraMenuItem;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripMenuItem slewTelescopeMenuItem;
        private ToolStripMenuItem centerTelescopeMenuItem;
        private ToolStripMenuItem SyncTelescopeMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem connectTelescopeMenuItem;
        private ToolStripMenuItem trackScopeMenuItem;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripMenuItem parkTelescopeMenuItem;
        private ToolStripSeparator toolStripSeparator13;
        private ToolStripMenuItem ASCOMPlatformHomePage;
        private ToolStripMenuItem chooseTelescopeMenuItem;
        private ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator14;
        private ToolStripMenuItem sIMBADSearchToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem feedbackToolStripMenuItem;
        private ToolStripMenuItem createANewTourToolStripMenuItem;
        private ToolStripMenuItem editTourToolStripMenuItem;
        private ToolStripMenuItem copyCurrentViewToClipboardToolStripMenuItem;
        private ToolStripMenuItem showFinderToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator15;
        private ToolStripMenuItem informationToolStripMenuItem;
        private ToolStripMenuItem lookupOnSimbadToolStripMenuItem;
        private ToolStripMenuItem propertiesToolStripMenuItem;
        private ToolStripMenuItem lookupOnSEDSToolStripMenuItem;
        private ToolStripMenuItem lookupOnWikipediaToolStripMenuItem;
        private ToolStripMenuItem publicationsToolStripMenuItem;
        private ToolStripMenuItem imageryToolStripMenuItem;
        private ToolStripMenuItem getDSSImageToolStripMenuItem;
        private ToolStripMenuItem getSDSSImageToolStripMenuItem;
        private ToolStripMenuItem getDSSFITSToolStripMenuItem;
        private ToolStripMenuItem virtualObservatorySearchesToolStripMenuItem;
        private ToolStripMenuItem uSNONVOConeSearchToolStripMenuItem;
        private ToolStripMenuItem restoreDefaultsToolStripMenuItem;
        private ToolStripMenuItem advancedToolStripMenuItem;
        private ToolStripMenuItem downloadQueueToolStripMenuItem;
        private ToolStripMenuItem startQueueToolStripMenuItem;
        private ToolStripMenuItem stopQueueToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator17;
        private ToolStripSeparator toolStripSeparator16;
        private ToolStripMenuItem showPerformanceDataToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripSeparator toolStripSeparator18;
        private ToolStripSeparator toolStripSeparator19;
        private ToolStripMenuItem flushCacheToolStripMenuItem;
        private RenderTarget renderWindow;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem autoRepeatToolStripMenuItem;
        private ToolStripMenuItem playCollectionAsSlideShowToolStripMenuItem;
        private Timer SlideAdvanceTimer;
        private ToolStripMenuItem oneToolStripMenuItem;
        private ToolStripMenuItem allToolStripMenuItem;
        private ToolStripMenuItem offToolStripMenuItem;
        private ToolStripMenuItem addToCollectionsToolStripMenuItem;
        private ToolStripMenuItem newCollectionToolStripMenuItem;
        private ToolStripMenuItem removeFromCollectionToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private Timer TourEndCheck;
        private ToolStripMenuItem gettingStarteMenuItem;
        private ToolStripMenuItem copyShortcutToolStripMenuItem;
        private Timer autoSaveTimer;
        private ToolStripMenuItem hLAFootprintsToolStripMenuItem;
        private ToolStripMenuItem copyShortCutToThisViewToClipboardToolStripMenuItem;

        private ToolStripMenuItem setCurrentViewAsWindowsDesktopBackgroundToolStripMenuItem;
        private ToolStripMenuItem selectLanguageToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem saveTourAsToolStripMenuItem;
        private ToolStripMenuItem setAsForegroundImageryToolStripMenuItem;
        private ToolStripMenuItem setAsBackgroundImageryToolStripMenuItem;
        private ToolStripSeparator ImagerySeperator;

        private int viewTileLevel;
        private int tileSizeX = 256;
        private int tileSizeY = 256;
        private double baseTileDegrees = 90;
        private int MaxLevels = 2;
        #endregion

        const float FOVMULT = 343.774f;
        public enum ZoomSpeeds { SLOW = 0, MEDIUM, FAST };

        private Timer timer;
        private ToolStripMenuItem viewOverlayTopo;
        private ToolStripMenuItem menuItem7;

        DataSetManager dsm;

        public RenderContext11 RenderContext11 = null;


        public static bool NoStealFocus = false;


        public bool SandboxMode
        {
            get
            {
                return CurrentImageSet != null && CurrentImageSet.DataSetType == ImageSetType.Sandbox;
            }
        }

        public bool SolarSystemMode
        {
            get
            {
                return CurrentImageSet != null && CurrentImageSet.DataSetType == ImageSetType.SolarSystem;
            }
        }

        private IContainer components;
        static bool pause;

        public event EventHandler ImageSetChanged;

        IImageSet currentImageSetfield;

        public IImageSet CurrentImageSet
        {
            get { return currentImageSetfield; }
            set
            {
                if (currentImageSetfield != value)
                {
                    var solarSytemOld = (currentImageSetfield != null && currentImageSetfield.DataSetType == ImageSetType.SolarSystem);
                    currentImageSetfield = value;

                    if (currentImageSetfield.DataSetType == ImageSetType.SolarSystem && !solarSytemOld)
                    {
                        if (contextPanel != null)
                        {
                            contextPanel.Constellation = "Error";
                        }
                    }
                    if (ImageSetChanged != null)
                    {
                        ImageSetChanged.Invoke(this, new EventArgs());
                        if (imageStackVisible)
                        {
                            stack.UpdateList();
                        }
                    }
                    if (value != null)
                    {
                        var hash = value.GetHash();
                        AddImageSetToTable(hash, value);
                    }
                }
            }
        }

        public void StartFadeTransition(double milliseconds)
        {
            Render();
            if (milliseconds > 0)
            {
                fadeImageSet.DelayTime = milliseconds;
            }
            fadeImageSet.State = true;
            fadeImageSet.TargetState = false;
        }

        public double ViewLat
        {
            get { return viewCamera.Lat; }
            set { viewCamera.Lat = value; }
        }

        public double GetEarthAltitude()
        {
            if (SolarSystemMode)
            {
                var pnt = Coordinates.GeoTo3dDouble(ViewLat, ViewLong + 90);

                var EarthMat = Planets.EarthMatrixInv;

                pnt = Vector3d.TransformCoordinate(pnt, EarthMat);
                pnt.Normalize();

                var point = Coordinates.CartesianToLatLng(pnt);

                return GetAltitudeForLatLongForPlanet((int)viewCamera.Target, point.Y, point.X);
            }
            if (CurrentImageSet.DataSetType == ImageSetType.Earth)
            {
                return TargetAltitude;
            }
            if (CurrentImageSet.DataSetType == ImageSetType.Planet)
            {
                return GetAltitudeForLatLong(ViewLat, ViewLong);
            }
            return 0;
        }

        public Vector2d GetEarthCoordinates()
        {
            if (SolarSystemMode)
            {

                var pnt = Coordinates.GeoTo3dDouble(ViewLat, ViewLong + 90);
                var EarthMat = Planets.EarthMatrixInv;
                pnt = Vector3d.TransformCoordinate(pnt, EarthMat);
                pnt.Normalize();

                return Coordinates.CartesianToLatLng(pnt);
            }
            if (CurrentImageSet.DataSetType == ImageSetType.Earth || CurrentImageSet.DataSetType == ImageSetType.Planet)
            {
                return new Vector2d(viewCamera.Lng, viewCamera.Lat);
            }
            return new Vector2d();
        }


        public enum ViewTypes { Equatorial, AltAz, Galactic, Ecliptic, Planet };

        private ViewTypes viewType = ViewTypes.Equatorial;

        public ViewTypes ViewType
        {
            get { return viewType; }
            set { viewType = value; }
        }


        public bool Space
        {
            get
            {
                if (CurrentImageSet != null)
                {
                    return CurrentImageSet.DataSetType == ImageSetType.Sky;
                }
                return true;
            }
        }

        public bool PlanetLike
        {
            get
            {
                if (CurrentImageSet != null)
                {
                    return CurrentImageSet.DataSetType == ImageSetType.Earth || CurrentImageSet.DataSetType == ImageSetType.Planet;
                }
                return true;
            }
        }

        public double RA
        {
            get
            {
                return ((((180 - (ViewLong - 180)) / 360) * 24.0) % 24);
            }
            set
            {
                if (double.IsNaN(value))
                {
                    // Break Here
                    value = 0;
                }
                var temp = 180 - ((value) / 24.0 * 360) - 180;
                if (temp != TargetLong)
                {
                    TargetLong = temp;
                }
            }
        }


        public double Dec
        {
            get
            {
                return ViewLat;
            }
            set
            {
                if (TargetLat != value)
                {
                    if (double.IsNaN(value))
                    {
                        // Break Here
                        value = 0;
                    }
                    TargetLat = value;
                }
            }
        }



        public double ViewLong
        {
            get { return viewCamera.Lng; }
            set
            {
                if (double.IsNaN(value))
                {
                    // Break Here
                    value = 0;
                }
                viewCamera.Lng = value;
            }
        }


        public double ZoomFactor
        {
            get { return viewCamera.Zoom; }
            set { viewCamera.Zoom = value; }
        }


        public double TargetZoom
        {
            get { return targetViewCamera.Zoom; }
            set { targetViewCamera.Zoom = value; }
        }

        private const double finalZoom = 360;
        private const double targetLat = 0;

        public double TargetLat
        {
            get { return targetViewCamera.Lat; }
            set
            {
                if (double.IsNaN(value))
                {
                    // Break Here
                    value = 0;
                }
                targetViewCamera.Lat = value;
            }
        }

        public double TargetLong
        {
            get { return targetViewCamera.Lng; }
            set
            {
                if (double.IsNaN(value))
                {
                    // Break Here
                    value = 0;
                }
                targetViewCamera.Lng = value;
            }
        }

        public static bool IsLoggedIn
        {
            get
            {
                return !String.IsNullOrEmpty(Properties.Settings.Default.LiveIdToken);
            }
        }



        
        public Timer StatupTimer;
        public MenuTabs menuTabs;
        
        public RenderTarget RenderWindow
        {
            get { return renderWindow; }
            set { renderWindow = value; }
        }


        public bool ControllerConnected()
        {
            var state = new XInputState();
            if (XInputMethods.GetState(0, out state))
            {
                return true;
            }

            return false;

        }



        public int ViewWidth
        {
            get
            {
                if ((!Space || rift) && (StereoMode == StereoModes.CrossEyed || StereoMode == StereoModes.SideBySide || StereoMode == StereoModes.OculusRift))
                {
                    return renderWindow.Width / 2;
                }
                return renderWindow.Width;
            }
        }
        public int ViewHeight
        {
            get
            {
                return renderWindow.Height;
            }

        }


        bool JoyInMotion;

        bool rSholderDown;
        bool lSholderDown;
        bool startDown;
        bool backDown;
        bool aDown;
        bool bDown;
        bool xDown;
        bool yDown;
        bool rightThumbDown;
        bool leftThumbDown;
        bool dPadUpDown;
        bool dPadDownDown;
        bool dPadLeftDown;
        bool dPadRightDown;
        bool slowRates = true;

        bool reticleControl;
        int retId = 0;
        public void UpdateXInputState()
        {

            if (Properties.Settings.Default.XboxCustomMapping)
            {
                ProcessCustomXboxMapping();
                return;
            }

            // lastFrameTime gives us fraction of seconds for update of motion factor for zooms
            var factor = lastFrameTime / (1.0 / 60.0);
            JoyInMotion = false;
            XInputState state;
            try
            {
                if (!XInputMethods.GetState(0, out state))
                {
                    return;
                }
            }
            catch
            {
                return;
            }
            double trigger = 0;

            if (state.Gamepad.RightTrigger > 0)
            {
                trigger = -state.Gamepad.RightTrigger;
            }
            else
            {
                trigger = +state.Gamepad.LeftTrigger;
            }

            if (Math.Abs(trigger) > 4)
            {
                ZoomFactor = TargetZoom = ZoomFactor * (1 + (trigger / 16000) * factor);

                if (ZoomFactor > ZoomMax)
                {
                    ZoomFactor = TargetZoom = ZoomMax;
                }

                if (ZoomFactor < ZoomMin)
                {
                    ZoomFactor = TargetZoom = ZoomMin;
                }
                JoyInMotion = true;
            }

            if (state.Gamepad.IsDPadRightButtonDown)
            {
                if (SpaceTimeController.TimeRate > .9)
                {
                    SpaceTimeController.TimeRate *= 1.1;
                }
                else if (SpaceTimeController.TimeRate < -1)
                {
                    SpaceTimeController.TimeRate /= 1.1;
                }
                else
                {
                    SpaceTimeController.TimeRate = 1.0;
                }

                if (SpaceTimeController.TimeRate > 1000000000)
                {
                    SpaceTimeController.TimeRate = 1000000000;
                }
                JoyInMotion = true;
            }

            if (state.Gamepad.IsDPadLeftButtonDown)
            {
                if (SpaceTimeController.TimeRate < -.9)
                {
                    SpaceTimeController.TimeRate *= 1.1;
                }
                else if (SpaceTimeController.TimeRate > 1)
                {
                    SpaceTimeController.TimeRate /= 1.1;
                }
                else
                {
                    SpaceTimeController.TimeRate = -1.0;
                }

                if (SpaceTimeController.TimeRate < -1000000000)
                {
                    SpaceTimeController.TimeRate = -1000000000;
                }
            }

            if ((state.Gamepad.Buttons & XInputButtons.LeftThumb) == XInputButtons.LeftThumb)
            {
                if (!leftThumbDown)
                {
                    leftThumbDown = true;
                    var camParams = new CameraParameters(0, 0, 360, 0, 0, 100);
                    GotoTarget(camParams, false, false);

                }
            }
            else
            {
                leftThumbDown = false;
            }



            if ((state.Gamepad.Buttons & XInputButtons.RightThumb) == XInputButtons.RightThumb)
            {
                if (!rightThumbDown)
                {
                    rightThumbDown = true;
                    reticleControl = !reticleControl;

                    if (reticleControl)
                    {
                        Reticle.Show(retId, false);
                    }
                    else
                    {
                        Reticle.Hide(retId, false);
                    }

                    Properties.Settings.Default.ShowReticle.TargetState = reticleControl;
                }
            }
            else
            {
                rightThumbDown = false;
            }



            if (state.Gamepad.IsStartButtonDown)
            {
                if (!startDown)
                {
                    NextMode();
                    startDown = true;

                }
            }
            else
            {
                startDown = false;
            }

            if (state.Gamepad.IsBackButtonDown)
            {
                if (!backDown)
                {
                    PreviousMode();
                    backDown = true;

                }
            }
            else
            {
                backDown = false;
            }

            if (state.Gamepad.IsDPadUpButtonDown)
            {
                SpaceTimeController.TimeRate = 0;
            }

            if (state.Gamepad.IsDPadDownButtonDown)
            {
                SpaceTimeController.TimeRate = 1;
                SpaceTimeController.SyncTime();
                SpaceTimeController.SyncToClock = true;
            }

            double zoomRate = slowRates ? 16000 : 8000;
            double moveRate = slowRates ? 8000 : 2000;

            if (!reticleControl)
            {
                if (Math.Abs((double)state.Gamepad.RightThumbX) > 8000)
                {
                    CameraRotateTarget = (CameraRotateTarget + (((state.Gamepad.RightThumbX / (zoomRate * 100)) * factor)));
                }

                if (Math.Abs((double)state.Gamepad.RightThumbY) > 8000)
                {
                    CameraAngleTarget = (CameraAngleTarget + (((state.Gamepad.RightThumbY / (zoomRate * 100)) * factor)));
                }
            }
            else
            {
                if (!Reticle.Reticles.ContainsKey(retId))
                {
                    Reticle.Set(retId, 0, 0, Color.Red);
                }


                if (Math.Abs((double)state.Gamepad.RightThumbX) > 7000)
                {
                    var reticle = Reticle.Reticles[retId];
                    reticle.Az = (reticle.Az - (((state.Gamepad.RightThumbX / (zoomRate * 3)) * factor)) / Math.Cos(reticle.Alt * Math.PI / 180));
                }

                if (Math.Abs((double)state.Gamepad.RightThumbY) > 7000)
                {
                    var reticle = Reticle.Reticles[retId];
                    reticle.Alt = (reticle.Alt + (((state.Gamepad.RightThumbY / (zoomRate * 3)) * factor)));
                }
            }



            if (CameraAngleTarget < TiltMin)
            {
                CameraAngleTarget = TiltMin;
            }

            if (CameraAngleTarget > 0)
            {
                CameraAngleTarget = 0;
            }

            if (Math.Abs((double)state.Gamepad.LeftThumbX) > 8000 || Math.Abs((double)state.Gamepad.LeftThumbY) > 8000)
            {
                MoveView((state.Gamepad.LeftThumbX / moveRate) * factor, (state.Gamepad.LeftThumbY / moveRate) * factor, false);
                JoyInMotion = true;
            }

            if (state.Gamepad.IsRightShoulderButtonDown)
            {
                // Edge trigger
                if (!rSholderDown)
                {
                    contextPanel.ShowNextObject();
                    rSholderDown = true;
                }
            }
            else
            {
                rSholderDown = false;
            }

            if (state.Gamepad.IsLeftShoulderButtonDown)
            {
                // Edge trigger
                if (!lSholderDown)
                {
                    contextPanel.ShowPreviousObject();
                    lSholderDown = true;
                }
            }
            else
            {
                lSholderDown = false;
            }

            if (state.Gamepad.IsXButtonDown)
            {
                if (!xDown)
                {
                    xDown = true;
                    switch (CurrentImageSet.DataSetType)
                    {
                        case ImageSetType.Earth:
                            Properties.Settings.Default.ShowClouds.TargetState = !Properties.Settings.Default.ShowClouds.TargetState;
                            break;
                        case ImageSetType.Planet:
                            break;
                        case ImageSetType.Sky:
                            Properties.Settings.Default.ShowEcliptic.TargetState = !Properties.Settings.Default.ShowEcliptic.TargetState;
                            break;
                        case ImageSetType.Panorama:
                            break;
                        case ImageSetType.SolarSystem:
                            {
                                if (Properties.Settings.Default.SolarSystemOrbits.TargetState)
                                {
                                    if (Properties.Settings.Default.SolarSystemMinorOrbits.TargetState)
                                    {
                                        Properties.Settings.Default.SolarSystemOrbits.TargetState = false;
                                        Properties.Settings.Default.SolarSystemMinorOrbits.TargetState = false;
                                    }
                                    else
                                    {
                                        Properties.Settings.Default.SolarSystemMinorOrbits.TargetState = true;
                                    }
                                }
                                else
                                {
                                    Properties.Settings.Default.SolarSystemOrbits.TargetState = true;
                                    Properties.Settings.Default.SolarSystemMinorOrbits.TargetState = false;
                                }

                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                xDown = false;
            }

            if (state.Gamepad.IsYButtonDown)
            {
                if (!yDown)
                {
                    yDown = true;
                    switch (CurrentImageSet.DataSetType)
                    {
                        case ImageSetType.Earth:
                            Properties.Settings.Default.ShowClouds.TargetState = !Properties.Settings.Default.ShowClouds.TargetState;
                            break;
                        case ImageSetType.Planet:
                            break;
                        case ImageSetType.Sky:
                            Properties.Settings.Default.ShowConstellationFigures.TargetState = !Properties.Settings.Default.ShowConstellationFigures.TargetState;
                            break;
                        case ImageSetType.Panorama:
                            break;
                        case ImageSetType.SolarSystem:
                            Properties.Settings.Default.SolarSystemStars.TargetState = !Properties.Settings.Default.SolarSystemStars.TargetState;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                yDown = false;
            }


            if (state.Gamepad.IsAButtonDown)
            {
                if (!aDown)
                {
                    aDown = true;
                    switch (CurrentImageSet.DataSetType)
                    {
                        case ImageSetType.Earth:
                            break;
                        case ImageSetType.Planet:
                            break;
                        case ImageSetType.Sky:
                            Properties.Settings.Default.ShowGrid.TargetState = !Properties.Settings.Default.ShowGrid.TargetState;
                            break;
                        case ImageSetType.Panorama:
                            break;
                        case ImageSetType.SolarSystem:
                            Properties.Settings.Default.SolarSystemMinorPlanets.TargetState = !Properties.Settings.Default.SolarSystemMinorPlanets.TargetState;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                aDown = false;
            }

            if (state.Gamepad.IsBButtonDown)
            {
                if (!bDown)
                {
                    bDown = true;
                    switch (CurrentImageSet.DataSetType)
                    {
                        case ImageSetType.Earth:
                            break;
                        case ImageSetType.Planet:
                            break;
                        case ImageSetType.Sky:
                            Properties.Settings.Default.ShowConstellationBoundries.TargetState = !Properties.Settings.Default.ShowConstellationBoundries.TargetState;
                            Properties.Settings.Default.ShowConstellationSelection.TargetState = false;
                            break;
                        case ImageSetType.Panorama:
                            break;
                        case ImageSetType.SolarSystem:
                            Properties.Settings.Default.SolarSystemMilkyWay.TargetState = !Properties.Settings.Default.SolarSystemMilkyWay.TargetState;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                bDown = false;
            }

            return;

        }

        private void NextMode()
        {
            switch (CurrentImageSet.DataSetType)
            {
                case ImageSetType.Earth:
                    MainWindow.contextPanel.SetLookAtTarget(ImageSetType.Planet);
                    break;
                case ImageSetType.Planet:
                    MainWindow.contextPanel.SetLookAtTarget(ImageSetType.Sky);
                    break;
                case ImageSetType.Sky:
                    if (!Properties.Settings.Default.LocalHorizonMode)
                    {
                        Properties.Settings.Default.LocalHorizonMode = true;
                    }
                    else
                    {
                        Properties.Settings.Default.LocalHorizonMode = false;
                        MainWindow.contextPanel.SetLookAtTarget(ImageSetType.Panorama);

                    }
                    break;
                case ImageSetType.Panorama:
                    MainWindow.contextPanel.SetLookAtTarget(ImageSetType.SolarSystem);
                    break;
                case ImageSetType.SolarSystem:
                    MainWindow.contextPanel.SetLookAtTarget(ImageSetType.Earth);
                    break;
                case ImageSetType.Sandbox:
                    MainWindow.contextPanel.SetLookAtTarget(ImageSetType.Earth);
                    break;
                default:
                    break;
            }
        }

        private void PreviousMode()
        {
            switch (CurrentImageSet.DataSetType)
            {
                case ImageSetType.Earth:
                    MainWindow.contextPanel.SetLookAtTarget(ImageSetType.Sandbox);
                    break;
                case ImageSetType.Planet:
                    MainWindow.contextPanel.SetLookAtTarget(ImageSetType.Earth);
                    break;
                case ImageSetType.Sky:
                    if (Properties.Settings.Default.LocalHorizonMode)
                    {
                        Properties.Settings.Default.LocalHorizonMode = false;
                    }
                    else
                    {
                        Properties.Settings.Default.LocalHorizonMode = false;
                        MainWindow.contextPanel.SetLookAtTarget(ImageSetType.Planet);
                    }
                    break;
                case ImageSetType.Panorama:
                    Properties.Settings.Default.LocalHorizonMode = true;
                    MainWindow.contextPanel.SetLookAtTarget(ImageSetType.Sky);
                    break;
                case ImageSetType.SolarSystem:
                    MainWindow.contextPanel.SetLookAtTarget(ImageSetType.Panorama);
                    break;
                case ImageSetType.Sandbox:
                    MainWindow.contextPanel.SetLookAtTarget(ImageSetType.SolarSystem);
                    break;
                default:
                    break;
            }
        }

        public void ProcessCustomXboxMapping()
        {
            // lastFrameTime gives us fraction of seconds for update of motion factor for zooms
            var factor = lastFrameTime / (1.0 / 60.0);
            JoyInMotion = false;
            XInputState state;
            try
            {
                if (!XInputMethods.GetState(0, out state))
                {
                    return;
                }
            }
            catch
            {
                return;
            }

            NetZoomRate = 0;

            if (state.Gamepad.RightTrigger > 4)
            {
                XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.RightTrigger, state.Gamepad.RightTrigger / 255.0);
            }

            if (state.Gamepad.LeftTrigger > 4)
            {
                XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.LeftTrigger, state.Gamepad.LeftTrigger / 255.0);
            }


            if (state.Gamepad.IsDPadRightButtonDown)
            {
                if (!dPadRightDown)
                {
                    dPadRightDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.DirectionPadRight, 1);
                }

            }
            else
            {
                dPadRightDown = false;
            }

            if (state.Gamepad.IsDPadLeftButtonDown)
            {
                if (!dPadLeftDown)
                {
                    dPadLeftDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.DirectionPadLeft, 1);
                }

            }
            else
            {
                dPadLeftDown = false;
            }

            if (state.Gamepad.IsLeftThumbClick)
            {
                if (!leftThumbDown)
                {
                    leftThumbDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.LeftThumbClick, 1);
                }
            }
            else
            {
                leftThumbDown = false;
            }



            if (state.Gamepad.IsRightThumbClick)
            {
                if (!rightThumbDown)
                {
                    rightThumbDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.RightThumbClick, 1);
                }
            }
            else
            {
                rightThumbDown = false;
            }



            if (state.Gamepad.IsStartButtonDown)
            {
                if (!startDown)
                {
                    startDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.Start, 1);
                }
            }
            else
            {
                startDown = false;
            }

            if (state.Gamepad.IsBackButtonDown)
            {
                if (!backDown)
                {
                    backDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.Back, 1);
                }
            }
            else
            {
                backDown = false;
            }

            if (state.Gamepad.IsDPadUpButtonDown)
            {
                if (!dPadUpDown)
                {
                    dPadUpDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.DirectionPadUp, 1);
                }
            }
            else
            {
                dPadUpDown = false;
            }

            if (state.Gamepad.IsDPadDownButtonDown)
            {
                if (!dPadDownDown)
                {
                    dPadDownDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.DirectionPadDown, 1);
                }
            }
            else
            {
                dPadDownDown = false;
            }



            if (Math.Abs((double)state.Gamepad.RightThumbX) > 8000)
            {
                XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.RightThumbX, (state.Gamepad.RightThumbX - 8000) / (32767.0 - 8000));
            }

            if (Math.Abs((double)state.Gamepad.RightThumbY) > 8000)
            {
                XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.RightThumbY, (state.Gamepad.RightThumbY - 8000) / (32767.0 - 8000));
            }

            if (Math.Abs((double)state.Gamepad.LeftThumbX) > 8000)
            {
                XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.LeftThumbX, (state.Gamepad.LeftThumbX - 8000) / (32767.0 - 8000));
            }

            if (Math.Abs((double)state.Gamepad.LeftThumbY) > 8000)
            {
                XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.LeftThumbY, (state.Gamepad.LeftThumbY - 8000) / (32767.0 - 8000));
            }

            if (state.Gamepad.IsRightShoulderButtonDown)
            {
                // Edge trigger
                if (!rSholderDown)
                {
                    rSholderDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.RightShoulder, 1);
                }
            }
            else
            {
                rSholderDown = false;
            }

            if (state.Gamepad.IsLeftShoulderButtonDown)
            {
                // Edge trigger
                if (!lSholderDown)
                {
                    lSholderDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.LeftShoulder, 1);
                }
            }
            else
            {
                lSholderDown = false;
            }

            if (state.Gamepad.IsXButtonDown)
            {
                if (!xDown)
                {
                    xDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.X, 1);
                }
            }
            else
            {
                xDown = false;
            }

            if (state.Gamepad.IsYButtonDown)
            {
                if (!yDown)
                {
                    yDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.Y, 1);
                }
            }
            else
            {
                yDown = false;
            }


            if (state.Gamepad.IsAButtonDown)
            {
                if (!aDown)
                {
                    aDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.A, 1);

                }
            }
            else
            {
                aDown = false;
            }

            if (state.Gamepad.IsBButtonDown)
            {
                if (!bDown)
                {
                    bDown = XBoxConfig.DispatchXboxEvent(XBoxConfig.XboxButtons.B, 1);

                }
            }
            else
            {
                bDown = false;
            }
        }


        Config config;

        public Config Config
        {
            get { return config; }
            set { config = value; }
        }
        MainMenu holder;
        string mainWindowText = Language.GetLocalizedText(3, "Microsoft WorldWide Telescope");

        public static bool HideSplash = false;
        public Earth3d()
        {
            AudioPlayer.Initialize();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            if (!HideSplash)
            {
                Splash.ShowSplashScreen();
            }

            // Set the initial size of our form
            ClientSize = new Size(400, 300);
            // And its caption



            config = new Config();

            MonitorX = config.MonitorX;
            MonitorY = config.MonitorY;
            MonitorCountX = config.MonitorCountX;
            MonitorCountY = config.MonitorCountY;
            monitorHeight = config.Height;
            monitorWidth = config.Width;
            bezelSpacing = (float)config.Bezel;


            ProjectorServer = !config.Master;
            if (DomeViewer)
            {
                ProjectorServer = true;
            }
            multiMonClient = !config.Master && (MonitorCountX > 1 || MonitorCountY > 1);

            InitializeComponent();
            SetUiStrings();

            Text = Language.GetLocalizedText(3, "Microsoft WorldWide Telescope");

            if (!InitializeGraphics())
            {
                UiTools.ShowMessageBox(Language.GetLocalizedText(4, "You need 3d Graphics and DirectX installed to run this application"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
            }


            // This code is used for dumping shader code when porting to Windows RT/phone where compiling shaders is not possible at runtime
            if (DumpShaders)
            {
                //ShaderLibrary.DumpShaderLibrary();
            }




            if (!InitializeImageSets())
            {
                Close();
            }


            BackInitDelegate initBackground = BackgroundInit;


            initBackground.BeginInvoke(null, null);


            if (TargetScreenId != -1)
            {
                try
                {
                    StartPosition = FormStartPosition.Manual;
                    Screen screenTarget;
                    screenTarget = Screen.AllScreens[TargetScreenId];

                    Top = screenTarget.WorkingArea.Y;
                    Left = screenTarget.WorkingArea.X;
                }
                catch
                {
                }
            }


        }
        PositionColorTexturedVertexBuffer11 distortVertexBuffer;
        IndexBuffer11 distortIndexBuffer;
        int distortVertexCount;
        int distortTriangleCount;

        public bool refreshWarp = true;
        private void RenderDistort()
        {
            SetupMatricesDistort();

            if (distortVertexBuffer == null || refreshWarp)
            {
                MakeDistortionGrid();
                refreshWarp = false;

            }

            RenderContext11.SetDisplayRenderTargets();
            RenderContext11.ClearRenderTarget(SharpDX.Color.Black);

            RenderContext11.SetVertexBuffer(distortVertexBuffer);
            RenderContext11.SetIndexBuffer(distortIndexBuffer);

            RenderContext11.devContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            RenderContext11.BlendMode = BlendMode.Alpha;
            RenderContext11.setRasterizerState(TriangleCullMode.Off);

            var mat = (RenderContext11.World * RenderContext11.View * RenderContext11.Projection).Matrix11;
            mat.Transpose();

            WarpOutputShader.MatWVP = mat;

            WarpOutputShader.Use(RenderContext11.devContext, true);

            RenderContext11.devContext.PixelShader.SetShaderResource(0, undistorted.RenderTexture.ResourceView);
            RenderContext11.devContext.DrawIndexed(distortIndexBuffer.Count, 0, 0);
            PresentFrame11(false);

        }



        private void MakeDistortionGrid()
        {
            var bmpBlend = new Bitmap(config.BlendFile);
            var fastBlend = new FastBitmap(bmpBlend);
            var bmpDistort = new Bitmap(config.DistortionGrid);
            var fastDistort = new FastBitmap(bmpDistort);


            fastBlend.LockBitmapRgb();
            fastDistort.LockBitmapRgb();
            var subX = bmpBlend.Width - 1;
            var subY = subX;

            if (distortIndexBuffer != null)
            {
                distortIndexBuffer.Dispose();
                GC.SuppressFinalize(distortIndexBuffer);
            }

            if (distortVertexBuffer != null)
            {
                distortVertexBuffer.Dispose();
                GC.SuppressFinalize(distortVertexBuffer);
            }


            distortIndexBuffer = new IndexBuffer11(typeof(int), (subX * subY * 6), RenderContext11.PrepDevice);
            distortVertexBuffer = new PositionColorTexturedVertexBuffer11(((subX + 1) * (subY + 1)), RenderContext11.PrepDevice);

            distortVertexCount = (subX + 1) * (subY + 1);


            var index = 0;


            // Create a vertex buffer 
            var verts = (PositionColoredTextured[])distortVertexBuffer.Lock(0, 0); // Lock the buffer (which will return our structs)
            int x1, y1;

            unsafe
            {
                double maxU = 0;
                double maxV = 0;
                double textureStepX = 1.0f / subX;
                double textureStepY = 1.0f / subY;
                for (y1 = 0; y1 <= subY; y1++)
                {
                    double tv;
                    for (x1 = 0; x1 <= subX; x1++)
                    {
                        double tu;


                        index = y1 * (subX + 1) + x1;
                        var pdata = fastDistort.GetRgbPixel(x1, y1);

                        tu = (pdata->blue + ((uint)pdata->red % 16) * 256) / 4095f;
                        tv = (pdata->green + ((uint)pdata->red / 16) * 256) / 4095f;

                        //tu = (tu - .5f) * 1.7777778 + .5f;

                        if (tu > maxU)
                        {
                            maxU = tu;
                        }
                        if (tv > maxV)
                        {
                            maxV = tv;
                        }

                        verts[index].Position = new Vector4(((float)x1 / subX) - .5f, (1f - ((float)y1 / subY)) - .5f, .9f, 1f);
                        verts[index].Tu = (float)tu;
                        verts[index].Tv = (float)tv;
                        var pPixel = fastBlend.GetRgbPixel(x1, y1);

                        verts[index].Color = Color.FromArgb(255, pPixel->red, pPixel->green, pPixel->blue);

                    }
                }
                distortVertexBuffer.Unlock();
                distortTriangleCount = (subX) * (subY) * 2;
                var indexArray = (uint[])distortIndexBuffer.Lock();
                index = 0;
                for (y1 = 0; y1 < subY; y1++)
                {
                    for (x1 = 0; x1 < subX; x1++)
                    {
                        // First triangle in quad
                        indexArray[index] = (uint)(y1 * (subX + 1) + x1);
                        indexArray[index + 1] = (uint)((y1 + 1) * (subX + 1) + x1);
                        indexArray[index + 2] = (uint)(y1 * (subX + 1) + (x1 + 1));

                        // Second triangle in quad
                        indexArray[index + 3] = (uint)(y1 * (subX + 1) + (x1 + 1));
                        indexArray[index + 4] = (uint)((y1 + 1) * (subX + 1) + x1);
                        indexArray[index + 5] = (uint)((y1 + 1) * (subX + 1) + (x1 + 1));
                        index += 6;
                    }
                }
                distortIndexBuffer.Unlock();
            }
            fastDistort.UnlockBitmap();
            fastBlend.UnlockBitmap();
            fastDistort.Dispose();
            GC.SuppressFinalize(fastDistort);
            fastBlend.Dispose();
            GC.SuppressFinalize(fastBlend);
        }

        public static void BackgroundInit()
        {
            Grids.InitStarVertexBuffer(RenderContext11.PrepDevice);
            Grids.MakeMilkyWay(RenderContext11.PrepDevice);
            Grids.InitCosmosVertexBuffer();
            Planets.InitPlanetResources();

        }

        public static void SearchInit()
        {
            Search.InitSearchTable();
        }


        _3DxMouse myMouse;

        int spaceDeviceType = 0;

        bool InitSpaceNavigator()
        {
            try
            {
                myMouse = new _3DxMouse(Handle);
                var NumberOf3DxMice = myMouse.EnumerateDevices();

                // Setup event handlers to be called when something happens
                myMouse.MotionEvent += myMouse_MotionEvent;
                myMouse.ButtonEvent += myMouse_ButtonEvent;
                return true;

            }
            catch
            {
                myMouse = null;
                return false;
            }
        }

        Vector3d SensorTranslation;
        Vector3d SensorRotation;

        void myMouse_ButtonEvent(object sender, _3DxMouse.ButtonEventArgs e)
        {
            CameraParameters cameraParams;
            if ((e.ButtonMask.Pressed & 1) == 1)
            {
                CameraRotateTarget = 0;
            }
            if ((e.ButtonMask.Pressed & 2) == 2)
            {
                cameraParams = new CameraParameters(0, 0, 360, 0, 0, 100);
                GotoTarget(cameraParams, false, false);

            }
        }

        void myMouse_MotionEvent(object sender, _3DxMouse.MotionEventArgs e)
        {
            if (e.TranslationVector != null)
            {

                // Swap axes from HID orientation to a right handed coordinate system that matches WPF model space
                SensorTranslation.X = e.TranslationVector.X;
                SensorTranslation.Y = -e.TranslationVector.Z;
                SensorTranslation.Z = e.TranslationVector.Y;
            }

            // Rotation Vector?
            if (e.RotationVector != null)
            {
                // Swap axes from HID orientation to a right handed coordinate system that matches WPF model space
                SensorRotation.X = e.RotationVector.X;
                SensorRotation.Y = -e.RotationVector.Z;
                SensorRotation.Z = e.RotationVector.Y;
            }
        }


        double sensitivity = 3000;

        public double TiltMin
        {
            get
            {
                if (Properties.Settings.Default.UnconstrainedTilt || ModifierKeys == (Keys.Shift | Keys.Control))
                {
                    return -1.52 * 2;
                }
                return -1.52;
            }
        }

        public void UpdateSpaceNavigator()
        {
            var interupt = false;

            var factor = lastFrameTime / (1.0 / 15.0);
            var units = .15;
            try
            {
                if (Math.Abs(SensorTranslation.Y) > 0)
                {
                    ZoomFactor = TargetZoom = ZoomFactor * (1 + ((SensorTranslation.Y / sensitivity) * factor));

                    if (ZoomFactor > ZoomMax)
                    {
                        ZoomFactor = ZoomMax;
                    }

                    if (ZoomFactor < ZoomMin)
                    {
                        ZoomFactor = ZoomMin;
                    }
                    interupt = true;
                }

                if (Math.Abs(SensorRotation.Y) > 0)
                {
                    var angle = (((SensorRotation.Y / sensitivity) * factor));
                    if (!PlanetLike)
                    {
                        angle = -angle;
                    }
                    CameraRotateTarget = (CameraRotateTarget + angle);
                    interupt = true;
                }

                if (Math.Abs(SensorRotation.X) > 0)
                {
                    var angle = (((SensorRotation.X / sensitivity) * factor));

                    CameraAngleTarget = (CameraAngleTarget + angle);
                    if (CameraAngleTarget < TiltMin)
                    {
                        CameraAngleTarget = TiltMin;
                    }
                    if (CameraAngleTarget > 0)
                    {
                        CameraAngleTarget = 0;
                    }
                    interupt = true;
                }

                if (Math.Abs(SensorTranslation.X) > 0 || Math.Abs(SensorTranslation.Z) > 0)
                {
                    MoveView(SensorTranslation.X * factor * units, -SensorTranslation.Z * factor * units, false);
                    if (SolarSystemMode)
                    {
                        if (TargetLat > 87)
                        {
                            TargetLat = 87;
                        }
                        if (TargetLat < -87)
                        {
                            TargetLat = -87;
                        }

                    }
                    interupt = true;
                }
            }
            catch
            {
            }
            if (interupt)
            {
                UserInterupt();
            }
        }
        // User is taking control...stop automated moves
        void UserInterupt()
        {
            if (playingSlideShow)
            {
                SlideAdvanceTimer.Enabled = false;
                SlideAdvanceTimer.Enabled = true;
            }
            if (mover != null)
            {
                var newCam = mover.CurrentPosition;

                viewCamera = targetViewCamera = newCam;

                mover = null;
            }
        }

        void keyboard_KeyDown(int keyCode)
        {
            CameraParameters cameraParams;
            switch (keyCode)
            {
                case 1:
                    CameraRotateTarget = 0;
                    break;
                case 2:
                    cameraParams = new CameraParameters(0, 0, 360, 0, 0, 100);
                    GotoTarget(cameraParams, false, false);
                    break;
            }
        }


        static bool readyToRender;

        static public bool ReadyToRender
        {
            get { return readyToRender && Initialized; }
            set { readyToRender = value; }
        }
        public static bool Initialized = false;

        public static Earth3d MainWindow = null;

        public void CheckOSVersion()
        {
            var os = Environment.OSVersion;

            // Get the version information
            var vs = os.Version;

            if (vs.Major < 6 && !Properties.Settings.Default.CheckedForFlashingVideo)
            {
                Properties.Settings.Default.TranparentWindows = false;
            }
            Properties.Settings.Default.CheckedForFlashingVideo = true;

        }

        //MSScriptControl.ScriptControlClass script = null;
        private static void UnitTestWCS()
        {
            var fitter = new WcsFitter(686, 1024);
            fitter.AddPoint(Coordinates.FromRaDec(5.533958, -0.30028), new Vector2d(400, 254));
            fitter.AddPoint(Coordinates.FromRaDec(5.59, -5.89722), new Vector2d(258, 836));
            fitter.Solve();
        }

        private void Earth3d_Load(object sender, EventArgs e)
        {
            CheckOSVersion();
            var path = Properties.Settings.Default.ImageSetUrl;

            if (Properties.Settings.Default.ImageSetUrl.ToLower().Contains("imagesetsnew"))
            {
                Properties.Settings.Default.ImageSetUrl = "http://www.worldwidetelescope.org/wwtweb/catalog.aspx?X=ImageSets5";
            }
             
            MainWindow = this;
            dsm = new DataSetManager();
            Constellations.Containment = constellationCheck;

            ContextSearch.InitializeDatabase(true);

            LoadExploreRoot();
            if (explorerRoot != null)
            {
                ContextSearch.AddFolderToSearch(explorerRoot, true);
            }
            ContextSearch.AddCatalogs(true);

            BackInitDelegate initBackground = SearchInit;

            initBackground.BeginInvoke(null, null);

            WindowState = FormWindowState.Maximized;



            FormBorderStyle = TouchKiosk ? FormBorderStyle.None : FormBorderStyle.Sizable;
            TileCache.StartQueue();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            MainWindow.Config.DomeTilt = (float)Properties.Settings.Default.DomeTilt;
            if (ProjectorServer)
            {
                ShowFullScreen(true);
                timer.Interval = 1000;
                InputTimer.Enabled = false;
                Cursor.Hide();
                Properties.Settings.Default.ShowCrosshairs = false;
                Properties.Settings.Default.SolarSystemMultiRes = true;
                NetControl.Start();

            }
            else
            {
                if (Properties.Settings.Default.ListenMode || Settings.DomeView)
                {
                    NetControl.Start();
                }
            }
            if (Settings.MasterController)
            {
                NetControl.StartStatusListner();
            }

            if (Settings.MasterController)
            {
                NetControl.LoadNodeList();
            }

            if (TouchKiosk)
            {
                menuTabs.IsVisible = false;
                kioskTitleBar.Visible = true;
                Properties.Settings.Default.ShowTouchControls = true;
                ShowFullScreen(true);
            }

            if (NoUi)
            {
                menuTabs.IsVisible = false;
                Properties.Settings.Default.ShowTouchControls = true;
                ShowFullScreen(true);
            }

            Tile.GrayscaleStyle = Properties.Settings.Default.MonochromeImageStyle;



            // This forces a init at startup does not do anything but force the static contstuctor to fire now
            LayerManager.LoadTree();

            listenUpBoysToolStripMenuItem.Checked = Properties.Settings.Default.ListenMode;
            var id = Properties.Settings.Default.StartUpLookAt;
            if (Properties.Settings.Default.StartUpLookAt == 5)
            {
                id = Properties.Settings.Default.LastLookAtMode;
            }

            if (Properties.Settings.Default.StartUpLookAt == 6)
            {
                var rnd = new Random();
                id = rnd.Next(-1, 5);
                Properties.Settings.Default.LastLookAtMode = id;
            }

            CurrentImageSet = GetDefaultImageset((ImageSetType)id, BandPass.Visible);

            Properties.Settings.Default.SettingChanging += Default_SettingChanging;
            Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;

            if (Properties.Settings.Default.LocalHorizonMode)
            {
                viewType = ViewTypes.AltAz;
            }
            else
            {
                viewType = ViewTypes.Equatorial;
            }
            InitSpaceNavigator();
            ReadyToRender = true;
            Refresh();

            try
            {
                fov = new FieldOfView(Properties.Settings.Default.FovTelescope, Properties.Settings.Default.FovCamera, Properties.Settings.Default.FovEyepiece);
            }
            catch
            {
            }

            SpaceTimeController.Altitude = Properties.Settings.Default.LocationAltitude;
            SpaceTimeController.Location = Coordinates.FromLatLng(Properties.Settings.Default.LocationLat, Properties.Settings.Default.LocationLng);

            TourPlayer.TourEnded += TourPlayer_TourEnded;
            if (KmlMarkers == null)
            {
                KmlMarkers = new KmlLabels();
            }
            ReadyToRender = true;
            Initialized = true;
            Activate();
            fadeImageSet.State = false;
            fadeImageSet.State = true;
            fadeImageSet.TargetState = false;

            // Force settings 
            Properties.Settings.Default.ActualPlanetScale = true;
            Properties.Settings.Default.HighPercitionPlanets = true;
            Properties.Settings.Default.ShowMoonsAsPointSource = false;
            Properties.Settings.Default.ShowSolarSystem.TargetState = true;

            toolStripMenuItem2.Checked = Settings.MasterController;

            viewCamera.Target = SolarSystemObjects.Sun;

            if (!ProjectorServer)
            {
                webServer.Startup();

                sampConnection = new Samp();

                // Register goto
                SampMessageHandler.RegiseterMessage(new SampCoordPointAtSky(SampGoto));
                SampMessageHandler.RegiseterMessage(new SampTableLoadVoTable(SampLoadTable));
                SampMessageHandler.RegiseterMessage(new SampImageLoadFits(SampLoadFitsImage));
                SampMessageHandler.RegiseterMessage(new SampTableHighlightRow(SampHighlightRow));

                NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;

                MidiMapManager.Startup();

            }

            Fader.TargetState = false;

            hold = new Text3dBatch(80);
            hold.Add(new Text3d(new Vector3d(0, 0, 1), new Vector3d(0, 1, 0), " 0hr123456789-+", 80, .0001f));
            hold.Add(new Text3d(new Vector3d(0, 0, 1), new Vector3d(0, 1, 0), "JanuyFebMcApilg", 80, .0001f));
            hold.Add(new Text3d(new Vector3d(0, 0, 1), new Vector3d(0, 1, 0), "stSmOoNvDBCEdqV", 80, .0001f));
            hold.Add(new Text3d(new Vector3d(0, 0, 1), new Vector3d(0, 1, 0), "jxGHILPRTU", 80, .0001f));
            hold.PrepareBatch();

            Constellations.InitializeConstellationNames();

            if (Properties.Settings.Default.ShowClientNodeList && !ProjectorServer)
            {

                ClientNodeList.ShowNodeList();
            }

            if (DetachScreenId > -1)
            {
                FreeFloatRenderWindow(DetachScreenId);
            }

            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.RefreshToken))
            {
                WindowsLiveSignIn();
            }
        }
        Text3dBatch hold;
        void SampHighlightRow(string url, string id, int row)
        {
            if (Samp.sampKnownTableIds.ContainsKey(id))
            {
                var layer = Samp.sampKnownTableIds[id];

                if (layer.Viewer != null)
                {
                    MethodInvoker doIt = delegate
                    {
                        layer.Viewer.HighlightRow(row);
                    };

                    if (InvokeRequired)
                    {
                        try
                        {
                            Invoke(doIt);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        doIt();
                    }
                }



            }
            else if (Samp.sampKnownTableUrls.ContainsKey(url))
            {

            }

        }

        void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            webServer.Shutdown();
            webServer.Startup();
        }

        void SampLoadFitsImage(string url, string id, string name)
        {
            MethodInvoker doIt = delegate
            {
                DownloadFitsImage(url);
            };

            if (InvokeRequired)
            {
                try
                {
                    Invoke(doIt);
                }
                catch
                {
                }
            }
            else
            {
                doIt();
            }

        }

        public void DownloadFitsImage(string url)
        {
            var filename = Path.GetTempFileName() + ".FITS";
            Cursor.Current = Cursors.WaitCursor;

            if (!FileDownload.DownloadFile(url, filename, true))
            {
                return;
            }
            try
            {
                LoadImage(filename);
            }
            catch
            {
                UiTools.ShowMessageBox(Language.GetLocalizedText(1003, "The image file did not download or is invalid."), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"));
            }
        }


        void SampLoadTable(string url, string id, string name)
        {
            MethodInvoker doIt = delegate
            {
                RunVoSearch(url, id);
            };

            if (InvokeRequired)
            {
                try
                {
                    Invoke(doIt);
                }
                catch
                {
                }
            }
            else
            {
                doIt();
            }

        }
        void SampGoto(double ra, double dec)
        {
            MethodInvoker doIt = delegate
            {
                GotoTargetRADec(ra, dec, true, false);
            };

            if (InvokeRequired)
            {
                try
                {
                    Invoke(doIt);
                }
                catch
                {
                }
            }
            else
            {
                doIt();
            }
        }

        public static MyWebServer webServer = new MyWebServer();

        public Samp sampConnection = null;
        public static int TargetScreenId = -1;
        public static int DetachScreenId = -1;
        public void ShowFullScreen(bool showFull)
        {
            SuspendLayout();
            menuTabs.IsVisible = !showFull && !TouchKiosk;
            if (showFull)
            {
                var doubleWide = (StereoMode == StereoModes.SideBySide || StereoMode == StereoModes.CrossEyed) && !rift;
                FormBorderStyle = FormBorderStyle.None;
                if (doubleWide || Properties.Settings.Default.FullScreenHeight != 0)
                {
                    WindowState = FormWindowState.Normal;
                    TopMost = true;
                }
                else
                {
                    WindowState = FormWindowState.Maximized;
                }
                UiTools.ShowFullScreen(this, doubleWide, TargetScreenId);
            }
            else
            {
                FormBorderStyle = TouchKiosk ? FormBorderStyle.None : FormBorderStyle.Sizable;
            }
            if (showFull)
            {
                holder = Menu;
                Menu = null;
            }
            else
            {
                if (holder != null)
                {
                    Menu = holder;
                }
            }
            fullScreen = showFull;
            ResumeLayout();
            RenderContext11.Resize(renderWindow);
        }




        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hwnd, StringBuilder text, int length);


        [DllImport("kernel32.dll")]
        static extern uint RegisterApplicationRecoveryCallback(IntPtr pRecoveryCallback, IntPtr pvParameter, int dwPingInterval, int dwFlags);

        [DllImport("kernel32.dll")]
        static extern uint ApplicationRecoveryInProgress(out bool pbCancelled);

        [DllImport("kernel32.dll")]
        static extern uint ApplicationRecoveryFinished(bool bSuccess);

        delegate int ApplicationRecoveryCallback(IntPtr pvParameter);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern uint RegisterApplicationRestart(string pszCommandline, int dwFlags);

        public bool IsWindowOrChildFocused()
        {
            var hwndFG = GetForegroundWindow();
            var bFocused = hwndFG == Handle || hwndFG == renderWindow.Handle || ((currentTab != null && currentTab.Handle == hwndFG) || (contextPanel != null && contextPanel.Handle == hwndFG));

            return bFocused;
        }

        private void ShowContextPanel()
        {
            try
            {
                if (ProjectorServer || NoUi)
                {
                    return;
                }
                if (contextPanel == null)
                {
                    contextPanel = new ContextPanel();
                }
                if (Properties.Settings.Default.TranparentWindows)
                {
                    contextPanel.SetOpacity();
                    contextPanel.Parent = null;
                    contextPanel.TopLevel = true;
                    contextPanel.Owner = this;
                    contextPanel.Dock = DockStyle.None;
                    contextPanel.Show();
                    contextPanel.Location = PointToScreen(new Point(0, ClientRectangle.Bottom - contextPanel.Height));
                    contextPanel.Width = ClientRectangle.Width;
                }
                else
                {
                    contextPanel.SetOpacity();
                    contextPanel.TopLevel = false;
                    contextPanel.Owner = null;
                    contextPanel.Parent = this;
                    contextPanel.Dock = DockStyle.Bottom;
                    contextPanel.Show();
                }
            }
            catch
            {
            }

        }

        public bool ShowKmlMarkers = true;
        public bool KmlAutoRefresh = false;

        public ConstellationFigureEditor figureEditor = null;


        public LayerManager layerManager = null;
        private void ShowLayersWindow()
        {
            if (layerManager == null)
            {
                layerManager = new LayerManager();
                layerManager.Owner = this;
            }

            var rectContext = contextPanel.Bounds;
            var rectCurrentTab = currentTab.Bounds;

            var pnt = PointToScreen(new Point(0, (ClientRectangle.Bottom - contextPanel.Height)));
            var pntTab = rectCurrentTab.Location;

            if (!Properties.Settings.Default.TranparentWindows)
            {
                pntTab = PointToScreen(pntTab);
            }
            layerManager.Location = new Point(pnt.X, pntTab.Y + rectCurrentTab.Height + 1);
            layerManager.Height = pnt.Y - layerManager.Top;
            layerManager.Show();
        }

        public void ShowFigureEditorWindow(Constellations figures)
        {
            if (figureEditor == null)
            {
                figureEditor = new ConstellationFigureEditor();
                figureEditor.Owner = this;
            }
            Properties.Settings.Default.ShowConstellationFigures.TargetState = true;

            figureEditor.Figures = figures;



            ShowFiguresEditorWindow();

        }

        private void ShowFiguresEditorWindow()
        {
            if (figureEditor != null)
            {
                var rectContext = contextPanel.Bounds;
                var rectCurrentTab = currentTab.Bounds;


                var pnt = PointToScreen(new Point(ClientRectangle.Right, (ClientRectangle.Bottom - contextPanel.Height)));
                var pntTab = rectCurrentTab.Location;

                if (!Properties.Settings.Default.TranparentWindows)
                {
                    pntTab = PointToScreen(pntTab);
                }
                figureEditor.Location = new Point(pnt.X - figureEditor.Width, pntTab.Y + rectCurrentTab.Height + 1);
                figureEditor.Height = pnt.Y - figureEditor.Top;
                figureEditor.Show();
            }
        }
        ImageStack stack;

        public ImageStack Stack
        {
            get { return stack; }
            set { stack = value; }
        }
        bool imageStackVisible;

        public bool ImageStackVisible
        {
            get { return imageStackVisible; }
            set
            {
                imageStackVisible = value;
                ShowImageStack();
            }
        }
        public void ShowImageStack()
        {
            if (stack == null)
            {
                stack = new ImageStack();
                stack.Owner = this;
                stack.UpdateList();
            }

            if (!imageStackVisible)
            {
                stack.Visible = false;
            }
            else if (!ProjectorServer)
            {
                var rectContext = contextPanel.Bounds;
                var rectCurrentTab = currentTab.Bounds;

                stack.Show();
                var pnt = PointToScreen(new Point(ClientRectangle.Right - stack.Width, ClientRectangle.Bottom - contextPanel.Height));
                stack.Location = new Point(pnt.X, rectCurrentTab.Top + rectCurrentTab.Height + 1);
                stack.Height = pnt.Y - stack.Top;
            }
        }

        Search searchPane;
        FolderBrowser toursTab;
   
        FolderBrowser explorePane;
        FolderBrowser communitiesPane;
        View viewPane;
        SettingsTab settingsPane;
        TelescopeTab telescopePane;
        public ContextPanel contextPanel = null;
        TourEditTab tourEdit;

        public TourEditTab TourEdit
        {
            get { return tourEdit; }
            set { tourEdit = value; }
        }
        IUiController uiController;

        public IUiController UiController
        {
            get { return uiController; }
            set { uiController = value; }
        }

        void menuTabs_MenuClicked(object sender, ApplicationMode e)
        {
            var menuPoint = new Point((int)e * 100 + menuTabs.StartX + 2, 36);

            switch (e)
            {
                case ApplicationMode.Explore:
                    {
                        menuTabs.Freeze();
                        exploreMenu.Show(menuTabs.PointToScreen(menuPoint));
                    }
                    break;
                case ApplicationMode.Tours:
                    {
                        menuTabs.Freeze();
                        toursMenu.Show(menuTabs.PointToScreen(menuPoint));
                    }
                    break;
                case ApplicationMode.Search:
                    {
                        menuTabs.Freeze();
                        searchMenu.Show(menuTabs.PointToScreen(menuPoint));
                    }
                    break;
                case ApplicationMode.Community:
                    {
                        menuTabs.Freeze();
                        communitiesMenu.Show(menuTabs.PointToScreen(menuPoint));
                    }
                    break;

                case ApplicationMode.Telescope:
                    {
                        menuTabs.Freeze();
                        telescopeMenu.Show(menuTabs.PointToScreen(menuPoint));
                    }
                    break;
                case ApplicationMode.View:
                    {
                        menuTabs.Freeze();
                        viewMenu.Show(menuTabs.PointToScreen(menuPoint));
                    }
                    break;
                case ApplicationMode.Settings:
                    {
                        menuTabs.Freeze();
                        settingsMenu.Show(menuTabs.PointToScreen(menuPoint));
                    }
                    break;
            }
        }

        private void menuTabs_TabClicked(object sender, ApplicationMode e)
        {
            if (currentMode == e)
            {
                //Show Menus
            }
            else
            {
                //switch modes
                SetAppMode(e);
            }
        }

        private async void  menuTabs_ControlEvent(object sender, ControlAction e)
        {
            switch (e)
            {
                case ControlAction.AppMenu:
                    break;
                case ControlAction.Close:
                    CloseOpenToursOrAbort(false);
                    TourPopup.CloseTourPopups();
                    Close();
                    break;
                case ControlAction.Maximize:
                case ControlAction.Restore:
                    if (WindowState == FormWindowState.Maximized)
                    {
                        WindowState = FormWindowState.Normal;
                    }
                    else
                    {
                        WindowState = FormWindowState.Maximized;
                    }
                    break;
                case ControlAction.CloseTour:
                    if (tourEdit != null)
                    {
                        CloseOpenToursOrAbort(false);
                    }
                    break;
                case ControlAction.SignOut:
                    WindowsLiveSignOut();
                    if (communitiesPane != null)
                    {
                        communitiesPane.SetCommunitiesMode();
                        communitiesPane.Refresh();
                        communitiesPane.LoadCommunities();
                    }
                    break;
                case ControlAction.SignIn:
                    await WindowsLiveSignIn();
                    menuTabs.SetSelectedIndex((int)ApplicationMode.Community, false);
                    break;
            }
        }

        public void CloseTour(bool silent)
        {
            Undo.Clear();
            FreezeView();
            if (tourEdit != null)
            {
                TourPopup.CloseTourPopups();
                mover = null;
                TourPlayer.Playing = false;
                KeyFramer.HideTimeline();
                if (LayerManager.TourLayers == false && !silent)
                {
                    if (LayerManager.CheckForTourLoadedLayers())
                    {
                        if (UiTools.ShowMessageBox(Language.GetLocalizedText(1004, "Close layers loaded with the tour as well?"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            LayerManager.CloseAllTourLoadedLayers();
                        }
                        else
                        {
                            tourEdit.Tour.DontCleanUpTempFiles = true;
                            LayerManager.CleanAllTourLoadedLayers();
                        }


                    }
                }
                tourEdit.Tour.CurrentTourstopIndex = -1;
                tourEdit.Tour.CleanUp();
                tourEdit.Tour.ClearTempFiles();
                menuTabs.RemoveTour(tourEdit.Tour);
                tourEdit.TourEditorUI.Close();
                tourEdit.Close();
                tourEdit = null;
                uiController = null;
                Settings.TourSettings = null;
                if (toursTab == null)
                {
                    menuTabs.SetSelectedIndex(0, false);
                }
                else
                {
                    menuTabs.SetSelectedIndex(1, false);
                }

                LayerManager.TourLayers = false;
            }
        }

        ApplicationMode currentMode = ApplicationMode.Tour1;

        public void SetAppMode()
        {
            SetAppMode(currentMode);
        }

        void SetAppMode(ApplicationMode mode)
        {
            if (ProjectorServer || NoUi)
            {
                return;
            }

            SuspendLayout();

            if (mode != currentMode)
            {
                switch (currentMode)
                {
                    case ApplicationMode.Search:
                        {
                            if (searchPane != null)
                            {
                                searchPane.Hide();
                            }
                        }
                        break;
                    case ApplicationMode.Tours:
                        {
                            if (toursTab != null)
                            {
                                toursTab.Hide();
                            }
                        }
                        break;
                    case ApplicationMode.Explore:
                        {
                            if (explorePane != null)
                            {
                                explorePane.Hide();
                            }
                        }
                        break;
                    case ApplicationMode.Community:
                        {
                            if (communitiesPane != null)
                            {
                                communitiesPane.Hide();
                            }
                        }
                        break;
                    case ApplicationMode.View:
                        {
                            if (viewPane != null)
                            {
                                viewPane.Hide();
                            }
                        }
                        break;
                    case ApplicationMode.Settings:
                        {
                            if (settingsPane != null)
                            {
                                settingsPane.Hide();
                            }
                        }
                        break;
                    case ApplicationMode.Telescope:
                        {
                            if (telescopePane != null)
                            {
                                telescopePane.Hide();
                            }
                        }
                        break;
                    case ApplicationMode.Tour1:
                    case ApplicationMode.Tour2:
                    case ApplicationMode.Tour3:
                    case ApplicationMode.Tour4:
                    case ApplicationMode.Tour5:
                        {
                            if (tourEdit != null)
                            {
                                tourEdit.Hide();
                            }
                        }

                        break;
                }
            }

            currentMode = mode;
            var loadTours = false;

            switch (mode)
            {
                case ApplicationMode.Tours:
                    {
                        if (toursTab == null)
                        {
                            toursTab = new FolderBrowser();
                            toursTab.Owner = this;
                            loadTours = true;
                        }
                        ShowPane(toursTab);

                    }
                    break;
                case ApplicationMode.Community:
                    {
                        if (communitiesPane == null)
                        {
                            communitiesPane = new FolderBrowser();
                            communitiesPane.SetCommunitiesMode();
                            communitiesPane.Owner = this;
                            ShowPane(communitiesPane);
                            communitiesPane.Refresh();
                            communitiesPane.LoadCommunities();
                        }
                        else
                        {
                            ShowPane(communitiesPane);
                        }
                    }
                    break;
                case ApplicationMode.View:
                    {
                        if (viewPane == null)
                        {
                            viewPane = new View();
                            viewPane.Owner = this;
                        }
                        ShowPane(viewPane);
                    }
                    break;
                case ApplicationMode.Settings:
                    {
                        if (settingsPane == null)
                        {
                            settingsPane = new SettingsTab();
                            settingsPane.Owner = this;
                        }
                        ShowPane(settingsPane);
                    }
                    break;
                case ApplicationMode.Telescope:
                    {
                        if (telescopePane == null)
                        {
                            telescopePane = new TelescopeTab();
                            telescopePane.Owner = this;
                        }
                        ShowPane(telescopePane);
                    }
                    break;
                case ApplicationMode.Explore:
                    {
                        if (explorePane == null)
                        {
                            explorePane = new FolderBrowser();
                            explorePane.ShowMyFolders = true;
                            explorePane.SetExploreMode();
                            explorePane.LoadRootFoder(explorerRoot);
                            explorePane.Owner = this;
                        }
                        ShowPane(explorePane);
                    }
                    break;
                case ApplicationMode.Search:
                    {
                        if (searchPane == null)
                        {
                            searchPane = new Search();
                            searchPane.Owner = this;
                        }
                        ShowPane(searchPane);
                    }

                    break;
                case ApplicationMode.Tour1:
                case ApplicationMode.Tour2:
                case ApplicationMode.Tour3:
                case ApplicationMode.Tour4:
                case ApplicationMode.Tour5:
                    {
                        if (figureEditor != null)
                        {
                            figureEditor.SaveAndClose();
                        }

                        if (menuTabs.CurrentTour != null)
                        {
                            if (tourEdit == null)
                            {
                                tourEdit = new TourEditTab();
                                tourEdit.Owner = this;
                            }

                            if (tourEdit.Tour != menuTabs.CurrentTour)
                            {
                                tourEdit.Tour = menuTabs.CurrentTour;
                            }
                            ShowPane(tourEdit);

                            if (tourEdit.Tour.EditMode && !TourPlayer.Playing)
                            {
                                uiController = tourEdit.TourEditorUI;
                            }
                            TimeLine.SetTour(tourEdit.Tour);
                        }
                    }
                    break;
            }

            ShowContextPanel();


            if (imageStackVisible)
            {
                ShowImageStack();
            }

            ResumeLayout(true);

            if (Properties.Settings.Default.ShowLayerManager)
            {
                ShowLayersWindow();
            }

            if (figureEditor != null)
            {
                ShowFiguresEditorWindow();
            }


            if (currentTab != null)
            {
                currentTab.SetOpacity();
            }

            if (loadTours)
            {
                toursTab.LoadTours();
            }

            ClearClientArea = ClientRectangle;

            if (Properties.Settings.Default.TranparentWindows)
            {
                var widthUsed = 0;


                if (Properties.Settings.Default.ShowLayerManager)
                {
                    widthUsed += layerManager.Width;
                }

                ClearClientArea.Height -= (currentTab.Height + contextPanel.Height);
                ClearClientArea.Width -= widthUsed;
                ClearClientArea.Location = new Point(ClearClientArea.Location.X + widthUsed, ClearClientArea.Location.Y + currentTab.Height);
            }

            if (WindowState != FormWindowState.Minimized)
            {
                KeyFramer.ShowZOrder();
            }

        }
        public Rectangle ClearClientArea;
        Folder explorerRoot;

        public Folder ExplorerRoot
        {
            get { return explorerRoot; }
            set { explorerRoot = value; }
        }

        private void LoadExploreRoot()
        {
            var url = Properties.Settings.Default.ExploreRootUrl;
            var filename = string.Format(@"{0}data\exploreRoot_{1}.wtml", Properties.Settings.Default.CahceDirectory, Math.Abs(url.GetHashCode32()));
            DataSetManager.DownloadFile(url, filename, false, true);
            explorerRoot = Folder.LoadFromFile(filename, true);
        }


        public bool ShowLayersWindows
        {
            get
            {
                return Properties.Settings.Default.ShowLayerManager;
            }
            set
            {
                if (Properties.Settings.Default.ShowLayerManager != value)
                {
                    Properties.Settings.Default.ShowLayerManager = value;
                    if (Properties.Settings.Default.ShowLayerManager)
                    {
                        SetAppMode(currentMode);
                    }
                    else
                    {
                        layerManager.Close();
                        layerManager.Dispose();
                        layerManager = null;
                    }
                    showLayerManagerToolStripMenuItem.Checked = value;
                }

            }
        }
        TabForm currentTab;

        private void ShowPane(TabForm pane)
        {
            try
            {
                currentTab = pane;
                TabForm.CurrentForm = pane;

                if (Properties.Settings.Default.TranparentWindows)
                {
                    pane.Parent = null;
                    pane.TopLevel = true;
                    pane.Owner = this;
                    pane.Dock = DockStyle.None;
                    pane.Show();
                    pane.SetOpacity();
                    pane.Location = PointToScreen(new Point(0, 34));
                    pane.Width = ClientRectangle.Width;
                }
                else
                {
                    menuTabs.Parent = null;
                    pane.SetOpacity();
                    pane.TopLevel = false;
                    pane.Owner = null;
                    pane.Parent = this;
                    pane.Dock = DockStyle.Top;
                    pane.Show();
                    menuTabs.Parent = this;
                }
            }
            catch
            {
            }
        }
        int changeCount;
        public static bool ignoreChanges = false;

        void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ignoreChanges)
            {
                return;
            }
            changeCount++;

            if (e.PropertyName.Contains("Color") || e.PropertyName == "ShowEarthSky" || e.PropertyName == "CloudMap8k" || e.PropertyName == "PulseMeForUpdate")
            {
                Properties.Settings.Default.ColSettingsVersion++;
            }

            if (e.PropertyName.Contains("Elevation") || e.PropertyName.Contains("Show3dCities"))
            {
                TileCache.PurgeQueue();
                TileCache.ClearCache();
            }

            ProcessChanged();
            if (e.PropertyName == "TranparentWindows")
            {
                SetAppMode(currentMode);
            }

            CheckDefaultProperties(false);
            CacheProxy.BaseUrl = Properties.Settings.Default.SharedCacheServer;
        }

        public void SuspendChanges()
        {
            ignoreChanges = true;
        }

        public void ProcessChanged()
        {
            ignoreChanges = false;


            Planets.ShowActualSize = Settings.Active.ActualPlanetScale;

            if (Properties.Settings.Default.LocalHorizonMode)
            {
                viewType = ViewTypes.AltAz;
            }
            else
            {
                viewType = ViewTypes.Equatorial;
            }

        }



        void Default_SettingChanging(object sender, SettingChangingEventArgs e)
        {
            settingsDirty = true;
        }

        DateTime lastMessage = DateTime.Now;
        public void SetLocation(double lat, double lng, double zoom, double cameraRotate, double cameraAngle, int foregroundImageSetHash,
                                            int backgroundImageSetHash, float blendOpacity, bool runSetup, bool flush, SolarSystemObjects target, Vector3d targetPoint, int solarSystemScale, string targetReferenceFrame)
        {
            if (!Settings.MasterController)
            {
                var resetViewmode = false;
                if (!imageStackVisible)
                {
                    if (CurrentImageSet.GetHash() != backgroundImageSetHash && CurrentImageSet.ThumbnailUrl.GetHashCode32() != backgroundImageSetHash)
                    {
                        SetImageSetByHash(backgroundImageSetHash);
                        resetViewmode = true;
                    }

                    if (StudyImageset == null || (StudyImageset.GetHash() != foregroundImageSetHash && StudyImageset.ThumbnailUrl.GetHashCode32() != foregroundImageSetHash))
                    {
                        if (foregroundImageSetHash != 0)
                        {
                            SetStudyImagesetByHash(foregroundImageSetHash);
                        }
                    }
                }

                if (resetViewmode)
                {
                    SetViewMode();
                }
                TrackingFrame = targetReferenceFrame;
                TargetLat = ViewLat = lat;
                TargetLong = ViewLong = lng;
                ZoomFactor = TargetZoom = zoom;
                if (Space && Settings.Active.GalacticMode)
                {
                    var gPoint = Coordinates.J2000toGalactic(viewCamera.RA * 15, viewCamera.Dec);
                    targetAlt = alt = gPoint[1];
                    targetAz = az = gPoint[0];
                }
                else if (Space && Settings.Active.LocalHorizonMode)
                {
                    var currentAltAz = Coordinates.EquitorialToHorizon(Coordinates.FromRaDec(viewCamera.RA, viewCamera.Dec), SpaceTimeController.Location, SpaceTimeController.Now);

                    targetAlt = alt = currentAltAz.Alt;
                    targetAz = az = currentAltAz.Az;
                }
                CameraAngle = cameraAngle;
                CameraRotate = cameraRotate;
                StudyOpacity = blendOpacity;
                SolarSystemTrack = target;
                viewCamera.ViewTarget = targetPoint;
                if (Properties.Settings.Default.SolarSystemScale != solarSystemScale)
                {
                    Properties.Settings.Default.SolarSystemScale = solarSystemScale;
                }
                var ts = DateTime.Now.Subtract(lastMessage);

                lastMessage = DateTime.Now;
            }

            if (runSetup)
            {
                runUpdate();
            }
        }
    

        static public Dictionary<int, IImageSet> ImagesetHashTable = new Dictionary<int, IImageSet>();

        public static void AddImageSetToTable(int hash, IImageSet set)
        {
            if (!ImagesetHashTable.ContainsKey(hash))
            {
                if (set != null)
                {
                    ImagesetHashTable.Add(hash, set);
                }
            }
        }

        public static int bgImagesetGets = 0;
        public static int bgImagesetFails = 0;

        private void SetImageSetByHash(int backgroundImageSetHash)
        {
            if (ImagesetHashTable.ContainsKey(backgroundImageSetHash))
            {
                CurrentImageSet = ImagesetHashTable[backgroundImageSetHash];
            }
            else
            {
                try
                {
                    if (backgroundImageSetHash != 0)
                    {
                        bgImagesetGets++;
                        if (Logging) { WriteLogMessage("Get Background Imageset from Server"); }
                        var client = new WebClient();
                        var url = string.Format("http://{0}:5050/imagesetwtml?id={1}", NetControl.MasterAddress, backgroundImageSetHash);
                        var wtml = client.DownloadString(url);
                        var doc = new XmlDocument();
                        doc.LoadXml(wtml);
                        XmlNode folder = doc["Folder"];
                        var imageSetXml = folder.FirstChild;
                        var ish = ImageSetHelper.FromXMLNode(imageSetXml);
                        CurrentImageSet = ish;
                    }
                }
                catch
                {
                    bgImagesetFails++;
                }

                if (CurrentImageSet == null)
                {
                    CurrentImageSet = ImageSets[0];
                }
            }
        }

        public static int fgImagesetGets = 0;
        public static int fgImagesetFails = 0;

        private void SetStudyImagesetByHash(int foregroundImageSetHash)
        {
            if (ImagesetHashTable.ContainsKey(foregroundImageSetHash))
            {
                StudyImageset = ImagesetHashTable[foregroundImageSetHash];
            }
            else
            {
                try
                {
                    if (foregroundImageSetHash != 0)
                    {
                        if (Logging) { WriteLogMessage("Get Background Imageset from Server"); }
                        fgImagesetGets++;
                        var client = new WebClient();
                        var url = string.Format("http://{0}:5050/imagesetwtml?id={1}", NetControl.MasterAddress, foregroundImageSetHash);
                        var wtml = client.DownloadString(url);
                        var doc = new XmlDocument();
                        doc.LoadXml(wtml);
                        XmlNode folder = doc["Folder"];
                        var imageSetXml = folder.FirstChild;
                        var ish = ImageSetHelper.FromXMLNode(imageSetXml);
                        StudyImageset = ish;
                    }
                }
                catch
                {
                    fgImagesetFails++;
                    StudyImageset = null;
                }

            }
        }

        private IPlace getContextMenuTargetObject()
        {
            return contextMenuTargetObject;
        }

        private void SetUiStrings()
        {
            // Set UI strings
            nameToolStripMenuItem.Text = Language.GetLocalizedText(7, "Name:");
            informationToolStripMenuItem.Text = Language.GetLocalizedText(8, "Information");
            lookupOnSimbadToolStripMenuItem.Text = Language.GetLocalizedText(9, "Look up on SIMBAD");
            lookupOnSEDSToolStripMenuItem.Text = Language.GetLocalizedText(10, "Look up on SEDS");
            lookupOnWikipediaToolStripMenuItem.Text = Language.GetLocalizedText(11, "Look up on Wikipedia");
            publicationsToolStripMenuItem.Text = Language.GetLocalizedText(12, "Look up publications on ADS");
            imageryToolStripMenuItem.Text = Language.GetLocalizedText(13, "Imagery");
            getDSSImageToolStripMenuItem.Text = Language.GetLocalizedText(14, "Get DSS image");
            getSDSSImageToolStripMenuItem.Text = Language.GetLocalizedText(15, "Get SDSS image");
            getDSSFITSToolStripMenuItem.Text = Language.GetLocalizedText(16, "Get DSS FITS");
            virtualObservatorySearchesToolStripMenuItem.Text = Language.GetLocalizedText(17, "Virtual Observatory Searches");
            hLAFootprintsToolStripMenuItem.Text = Language.GetLocalizedText(1, "HLA Footprints");
            propertiesToolStripMenuItem.Text = Language.GetLocalizedText(20, "Properties");
            copyShortcutToolStripMenuItem.Text = Language.GetLocalizedText(21, "Copy Shortcut");
            addToCollectionsToolStripMenuItem.Text = Language.GetLocalizedText(22, "Add to Collection");
            newCollectionToolStripMenuItem.Text = Language.GetLocalizedText(23, "New Collection...");
            removeFromCollectionToolStripMenuItem.Text = Language.GetLocalizedText(24, "Remove from Collection");
            editToolStripMenuItem.Text = Language.GetLocalizedText(25, "Edit...");
            joinCoomunityMenuItem.Text = Language.GetLocalizedText(26, "Join a Community...");
            updateLoginCredentialsMenuItem.Text = Language.GetLocalizedText(27, "Update Login Credentials...");
            logoutMenuItem.Text = Language.GetLocalizedText(28, "Logout");
            uploadObservingListToCommunityMenuItem.Text = Language.GetLocalizedText(30, "Upload Observing List to Community...");
            uploadImageToCommunityMenuItem.Text = Language.GetLocalizedText(31, "Upload Image to Community...");

            sIMBADSearchToolStripMenuItem.Text = Language.GetLocalizedText(34, "SIMBAD Search...");
            tourHomeMenuItem.Text = Language.GetLocalizedText(35, "Tour Home");
            tourSearchWebPageMenuItem.Text = Language.GetLocalizedText(36, "Tour Search Web Page");
            createANewTourToolStripMenuItem.Text = Language.GetLocalizedText(37, "Create a New Tour...");
            publishTourMenuItem.Text = Language.GetLocalizedText(38, "Submit Tour for Publication...");
            saveTourAsToolStripMenuItem.Text = Language.GetLocalizedText(554, "Save Tour As...");
            autoRepeatToolStripMenuItem.Text = Language.GetLocalizedText(39, "Auto Repeat");
            oneToolStripMenuItem.Text = Language.GetLocalizedText(40, "One");
            allToolStripMenuItem.Text = Language.GetLocalizedText(41, "All");
            offToolStripMenuItem.Text = Language.GetLocalizedText(42, "Off");
            editTourToolStripMenuItem.Text = Language.GetLocalizedText(43, "Edit Tour");
            slewTelescopeMenuItem.Text = Language.GetLocalizedText(44, "Slew To Object");
            centerTelescopeMenuItem.Text = Language.GetLocalizedText(45, "Center on Scope");
            uSNONVOConeSearchToolStripMenuItem.Text = Language.GetLocalizedText(18, "USNO NVO cone search");
            SyncTelescopeMenuItem.Text = Language.GetLocalizedText(46, "Sync Scope to Current Location");
            chooseTelescopeMenuItem.Text = Language.GetLocalizedText(47, "Choose Telescope");
            connectTelescopeMenuItem.Text = Language.GetLocalizedText(48, "Connect");
            trackScopeMenuItem.Text = Language.GetLocalizedText(49, "Track Telescope");

            parkTelescopeMenuItem.Text = Language.GetLocalizedText(50, "Park");
            ASCOMPlatformHomePage.Text = Language.GetLocalizedText(51, "ASCOM Platform");
            createNewObservingListToolStripMenuItem.Text = Language.GetLocalizedText(52, "New");
            newObservingListpMenuItem.Text = Language.GetLocalizedText(53, "Collection...");
            newSimpleTourMenuItem.Text = Language.GetLocalizedText(54, "Slide-Based Tour...");
            openFileToolStripMenuItem.Text = Language.GetLocalizedText(55, "&Open");
            openTourMenuItem.Text = Language.GetLocalizedText(56, "Tour...");
            openObservingListMenuItem.Text = Language.GetLocalizedText(57, "Collection...");
            // openImageMenuItem.Text = Language.GetLocalizedText(58, "Image...");
            openImageMenuItem.Text = Language.GetLocalizedText(948, "Astronomical Image...");
            remoteAccessControlToolStripMenuItem.Text = Language.GetLocalizedText(1024, "Remote Access Control...");
            screenBroadcastToolStripMenuItem.Text = Language.GetLocalizedText(1023, "Screen Broadcast...");
            publishTourToCommunityToolStripMenuItem.Text = Language.GetLocalizedText(1022, "Publish Tour to Community...");
            layersToolStripMenuItem.Text = Language.GetLocalizedText(1021, "Layers...");
            showFinderToolStripMenuItem.Text = Language.GetLocalizedText(59, "Show Finder");
            playCollectionAsSlideShowToolStripMenuItem.Text = Language.GetLocalizedText(60, "Play Collection as Slide Show");
            aboutMenuItem.Text = Language.GetLocalizedText(61, "About WorldWide Telescope");
            homepageMenuItem.Text = Language.GetLocalizedText(63, "WorldWide Telescope Home Page");
            exitMenuItem.Text = Language.GetLocalizedText(64, "E&xit");
            checkForUpdatesToolStripMenuItem.Text = Language.GetLocalizedText(65, "Check for Updates...");
            feedbackToolStripMenuItem.Text = Language.GetLocalizedText(66, "Product Support...");
            restoreDefaultsToolStripMenuItem.Text = Language.GetLocalizedText(67, "Restore Defaults");
            advancedToolStripMenuItem.Text = Language.GetLocalizedText(68, "Advanced");
            downloadQueueToolStripMenuItem.Text = Language.GetLocalizedText(69, "Show Download Queue");
            startQueueToolStripMenuItem.Text = Language.GetLocalizedText(70, "Start Queue");
            stopQueueToolStripMenuItem.Text = Language.GetLocalizedText(71, "Stop Queue");
            showPerformanceDataToolStripMenuItem.Text = Language.GetLocalizedText(72, "Show Performance Data");
            toolStripMenuItem2.Text = Language.GetLocalizedText(73, "Master Controller");
            flushCacheToolStripMenuItem.Text = Language.GetLocalizedText(74, "Flush Cache");
            gettingStarteMenuItem.Text = Language.GetLocalizedText(62, "Getting Started (Help)");
            resetCameraMenuItem.Text = Language.GetLocalizedText(75, "Reset Camera");
            copyCurrentViewToClipboardToolStripMenuItem.Text = Language.GetLocalizedText(76, "Copy Current View Image");
            copyShortCutToThisViewToClipboardToolStripMenuItem.Text = Language.GetLocalizedText(77, "Copy Shortcut to this View");
            removeFromImageCacheToolStripMenuItem.Text = Language.GetLocalizedText(82, "Remove from Image Cache");
            setAsForegroundImageryToolStripMenuItem.Text = Language.GetLocalizedText(552, "Set as Foreground Imagery");
            setAsBackgroundImageryToolStripMenuItem.Text = Language.GetLocalizedText(553, "Set as Background Imagery");

            setCurrentViewAsWindowsDesktopBackgroundToolStripMenuItem.Text = Language.GetLocalizedText(83, "Set Current View as Windows Desktop Background");
            Text = Language.GetLocalizedText(3, "Microsoft WorldWide Telescope");
            selectLanguageToolStripMenuItem.Text = Language.GetLocalizedText(1, "Select Your Language") + "...";
            toggleFullScreenModeF11ToolStripMenuItem.Text = Language.GetLocalizedText(565, "Toggle Full Screen Mode");
            vOTableToolStripMenuItem.Text = Language.GetLocalizedText(566, "VOTable...");
            stereoToolStripMenuItem.Text = Language.GetLocalizedText(567, "Stereo");
            enabledToolStripMenuItem.Text = Language.GetLocalizedText(568, "Disabled");
            anaglyphToolStripMenuItem.Text = Language.GetLocalizedText(569, "Anaglyph (Red-Cyan)");
            anaglyphYellowBlueToolStripMenuItem.Text = Language.GetLocalizedText(570, "Anaglyph (Yellow-Blue)");
            sideBySideProjectionToolStripMenuItem.Text = Language.GetLocalizedText(571, "Side by Side Projection");
            sideBySideCrossEyedToolStripMenuItem.Text = Language.GetLocalizedText(572, "Side by Side Cross-Eyed");
            expermentalToolStripMenuItem.Text = Language.GetLocalizedText(574, "Full Dome");
            fullDomeToolStripMenuItem.Text = Language.GetLocalizedText(574, "Full Dome");
            domeSetupToolStripMenuItem.Text = Language.GetLocalizedText(575, "Dome Setup");
            toolStripMenuItem3.Text = Language.GetLocalizedText(576, "VO Cone Search / Registry Lookup...");

            earthToolStripMenuItem.Text = Language.GetLocalizedText(581, "Earth");
            planetToolStripMenuItem.Text = Language.GetLocalizedText(582, "Planet");
            skyToolStripMenuItem.Text = Language.GetLocalizedText(583, "Sky");
            panoramaToolStripMenuItem.Text = Language.GetLocalizedText(286, "Panorama");
            solarSystemToolStripMenuItem.Text = Language.GetLocalizedText(373, "Solar System");
            lastToolStripMenuItem.Text = Language.GetLocalizedText(584, "Last");
            randomToolStripMenuItem.Text = Language.GetLocalizedText(585, "Random XX");
            musicAndOtherTourResourceToolStripMenuItem.Text = Language.GetLocalizedText(586, "Music and other Tour Resource");

            startupToolStripMenuItem.Text = Language.GetLocalizedText(591, "Startup Look At");
            lookUpOnNEDToolStripMenuItem.Text = Language.GetLocalizedText(593, "Look up on NED");
            nEDSearchToolStripMenuItem.Text = Language.GetLocalizedText(594, "NED Search");
            sDSSSearchToolStripMenuItem.Text = Language.GetLocalizedText(595, "SDSS Search");
            sendImageToToolStripMenuItem.Text = Language.GetLocalizedText(596, "Send Image To");
            broadcastToolStripMenuItem.Text = Language.GetLocalizedText(597, "Broadcast");
            broadcastToolStripMenuItem1.Text = Language.GetLocalizedText(597, "Broadcast");
            sendTableToToolStripMenuItem.Text = Language.GetLocalizedText(598, "Send Table To");
            imageStackToolStripMenuItem.Text = Language.GetLocalizedText(622, "Image Stack");
            addToImageStackToolStripMenuItem.Text = Language.GetLocalizedText(623, "Add to Image Stack");


            vORegistryToolStripMenuItem.Text = Language.GetLocalizedText(576, "VO Cone Search / Registry Lookup...");
            shapeFileToolStripMenuItem.Text = Language.GetLocalizedText(631, "Shape File...");

            lookUpOnSDSSToolStripMenuItem.Text = Language.GetLocalizedText(632, "Look up on SDSS");

            addCollectionAsTourStopsToolStripMenuItem.Text = Language.GetLocalizedText(645, "Add Collection as Tour Stops");
            showLayerManagerToolStripMenuItem.Text = Language.GetLocalizedText(655, "Show Layer Manager");
            listenUpBoysToolStripMenuItem.Text = Language.GetLocalizedText(656, "Start Listener");
            detachMainViewToSecondMonitor.Text = Language.GetLocalizedText(657, "Detach Main View to Second Monitor");
            regionalDataCacheToolStripMenuItem.Text = Language.GetLocalizedText(658, "Regional Data Cache...");
            multiChanelCalibrationToolStripMenuItem.Text = Language.GetLocalizedText(669, "Multi-Channel Calibration");
            sendLayersToProjectorServersToolStripMenuItem.Text = Language.GetLocalizedText(670, "Send Layers to Projector Servers");
            showTouchControlsToolStripMenuItem.Text = Language.GetLocalizedText(671, "Show On-Screen Controls");
            saveCurrentViewImageToFileToolStripMenuItem.Text = Language.GetLocalizedText(672, "Save Current View Image to File...");
            renderToVideoToolStripMenuItem.Text = Language.GetLocalizedText(673, "Render to Video...");
            layerManagerToolStripMenuItem.Text = Language.GetLocalizedText(949, "Layer Manager");
            showOverlayListToolStripMenuItem.Text = Language.GetLocalizedText(1057, "Show Overlay List");
            sendTourToProjectorServersToolStripMenuItem.Text = Language.GetLocalizedText(1058, "Send Tour to Projector Servers");
            automaticTourSyncWithProjectorServersToolStripMenuItem.Text = Language.GetLocalizedText(1059, "Automatic Tour Sync with Projector Servers");
            findEarthBasedLocationToolStripMenuItem.Text = Language.GetLocalizedText(1060, "Find Earth Based Location...");
            multiSampleAntialiasingToolStripMenuItem.Text = Language.GetLocalizedText(1061, "Multi-Sample Antialiasing");
            noneToolStripMenuItem.Text = Language.GetLocalizedText(832, "None");
            fourSamplesToolStripMenuItem.Text = Language.GetLocalizedText(1062, "Four Samples");
            eightSamplesToolStripMenuItem.Text = Language.GetLocalizedText(1063, "Eight Samples");
            lockVerticalSyncToolStripMenuItem.Text = Language.GetLocalizedText(1064, "Lock Vertical Sync");
            targetFrameRateToolStripMenuItem.Text = Language.GetLocalizedText(1065, "Target Frame Rate");
            fpsToolStripMenuItemUnlimited.Text = Language.GetLocalizedText(1066, "Unlimited");
            fPSToolStripMenuItem60.Text = Language.GetLocalizedText(1067, "60 FPS");
            fPSToolStripMenuItem30.Text = Language.GetLocalizedText(1068, "30 FPS");
            fPSToolStripMenuItem24.Text = Language.GetLocalizedText(1069, "24 FPS");
            monochromeStyleToolStripMenuItem.Text = Language.GetLocalizedText(1070, "Monochrome Style");
            mIDIControllerSetupToolStripMenuItem.Text = Language.GetLocalizedText(1071, "Controller Setup...");
            tileLoadingThrottlingToolStripMenuItem.Text = Language.GetLocalizedText(1072, "Tile Loading Throttling");
            tpsToolStripMenuItem15.Text = Language.GetLocalizedText(1073, "15 tps");
            tpsToolStripMenuItem30.Text = Language.GetLocalizedText(1074, "30 tps");
            tpsToolStripMenuItem60.Text = Language.GetLocalizedText(1075, "60 tps");
            tpsToolStripMenuItem120.Text = Language.GetLocalizedText(1076, "120 tps");
            tpsToolStripMenuItemUnlimited.Text = Language.GetLocalizedText(1066, "Unlimited");
            saveCacheAsCabinetFileToolStripMenuItem.Text = Language.GetLocalizedText(1077, "Save Cache as Cabinet File...");
            restoreCacheFromCabinetFileToolStripMenuItem.Text = Language.GetLocalizedText(1078, "Restore Cache from Cabinet File...");
            clientNodeListToolStripMenuItem.Text = Language.GetLocalizedText(1079, "Projector Server List");
            allowUnconstrainedTiltToolStripMenuItem.Name = Language.GetLocalizedText(1270, "allowUnconstrainedTiltToolStripMenuItem");
            ShowWelcomeTips.Text = Language.GetLocalizedText(1305, "Show Welcome Tips");
            allowUnconstrainedTiltToolStripMenuItem.Text = Language.GetLocalizedText(1306, "Allow Unconstrained Tilt");
            alternatingLinesEvenToolStripMenuItem.Text = Language.GetLocalizedText(1307, "Alternating Lines Even");
            alternatingLinesOddToolStripMenuItem.Text = Language.GetLocalizedText(1308, "Alternating Lines Odd");
            oculusRiftToolStripMenuItem.Text = Language.GetLocalizedText(1309, "Oculus Rift");
            detachMainViewToThirdMonitorToolStripMenuItem.Text = Language.GetLocalizedText(1310, "Detach Main View to Third Monitor");
            fullDomePreviewToolStripMenuItem.Text = Language.GetLocalizedText(1311, "Full Dome Preview");
            xBoxControllerSetupToolStripMenuItem.Text = Language.GetLocalizedText(1312, "Xbox Controller Setup...");
            showKeyframerToolStripMenuItem.Text = Language.GetLocalizedText(1343, "Show Timeline Editor");
            showSlideNumbersToolStripMenuItem.Text = Language.GetLocalizedText(1344, "Show Slide Numbers");
            newFullDomeViewInstanceToolStripMenuItem.Text = Language.GetLocalizedText(1376, "New Full Dome View Instance");
            customGalaxyFileToolStripMenuItem.Text = Language.GetLocalizedText(1377, "Custom Galaxy File...");
            monitorOneToolStripMenuItem.Text = Language.GetLocalizedText(1378, "Monitor One");
            monitorTwoToolStripMenuItem.Text = Language.GetLocalizedText(1379, "Monitor Two");
            monitorThreeToolStripMenuItem.Text = Language.GetLocalizedText(1380, "Monitor Three");
            monitorFourToolStripMenuItem.Text = Language.GetLocalizedText(1381, "Monitor Four");
            monitorFiveToolStripMenuItem.Text = Language.GetLocalizedText(1382, "Monitor Five");
            monitorSixToolStripMenuItem.Text = Language.GetLocalizedText(1383, "Monitor Six");
            monitorSevenToolStripMenuItem.Text = Language.GetLocalizedText(1384, "Monitor Seven");
            monitorEightToolStripMenuItem.Text = Language.GetLocalizedText(1385, "Monitor Eight");
            exportCurrentViewAsSTLFileFor3DPrintingToolStripMenuItem.Text = Language.GetLocalizedText(1386, "Export Current View as STL File for 3D Printing...");
        }

        private void InitializeComponent()
        {
            components = new Container();
            var resources = new ComponentResourceManager(typeof(Earth3d));
            timer = new Timer(components);
            menuItem7 = new ToolStripMenuItem();
            viewOverlayTopo = new ToolStripMenuItem();
            InputTimer = new Timer(components);
            contextMenu = new ContextMenuStrip(components);
            nameToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator11 = new ToolStripSeparator();
            informationToolStripMenuItem = new ToolStripMenuItem();
            lookupOnSimbadToolStripMenuItem = new ToolStripMenuItem();
            lookupOnSEDSToolStripMenuItem = new ToolStripMenuItem();
            lookupOnWikipediaToolStripMenuItem = new ToolStripMenuItem();
            publicationsToolStripMenuItem = new ToolStripMenuItem();
            lookUpOnNEDToolStripMenuItem = new ToolStripMenuItem();
            lookUpOnSDSSToolStripMenuItem = new ToolStripMenuItem();
            imageryToolStripMenuItem = new ToolStripMenuItem();
            getDSSImageToolStripMenuItem = new ToolStripMenuItem();
            getSDSSImageToolStripMenuItem = new ToolStripMenuItem();
            getDSSFITSToolStripMenuItem = new ToolStripMenuItem();
            virtualObservatorySearchesToolStripMenuItem = new ToolStripMenuItem();
            uSNONVOConeSearchToolStripMenuItem = new ToolStripMenuItem();
            hLAFootprintsToolStripMenuItem = new ToolStripMenuItem();
            nEDSearchToolStripMenuItem = new ToolStripMenuItem();
            sDSSSearchToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            toolStripSeparator15 = new ToolStripSeparator();
            setAsForegroundImageryToolStripMenuItem = new ToolStripMenuItem();
            setAsBackgroundImageryToolStripMenuItem = new ToolStripMenuItem();
            addToImageStackToolStripMenuItem = new ToolStripMenuItem();
            addAsNewLayerToolStripMenuItem = new ToolStripMenuItem();
            cacheManagementToolStripMenuItem1 = new ToolStripMenuItem();
            cacheImageryTilePyramidToolStripMenuItem = new ToolStripMenuItem();
            showCacheSpaceUsedToolStripMenuItem = new ToolStripMenuItem();
            removeFromImageCacheToolStripMenuItem = new ToolStripMenuItem();
            ImagerySeperator = new ToolStripSeparator();
            propertiesToolStripMenuItem = new ToolStripMenuItem();
            copyShortcutToolStripMenuItem = new ToolStripMenuItem();
            addToCollectionsToolStripMenuItem = new ToolStripMenuItem();
            newCollectionToolStripMenuItem = new ToolStripMenuItem();
            removeFromCollectionToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            sAMPToolStripMenuItem = new ToolStripMenuItem();
            sendImageToToolStripMenuItem = new ToolStripMenuItem();
            broadcastToolStripMenuItem = new ToolStripMenuItem();
            sendTableToToolStripMenuItem = new ToolStripMenuItem();
            broadcastToolStripMenuItem1 = new ToolStripMenuItem();
            HoverTimer = new Timer(components);
            communitiesMenu = new ContextMenuStrip(components);
            joinCoomunityMenuItem = new ToolStripMenuItem();
            updateLoginCredentialsMenuItem = new ToolStripMenuItem();
            logoutMenuItem = new ToolStripMenuItem();
            toolStripSeparator8 = new ToolStripSeparator();
            uploadObservingListToCommunityMenuItem = new ToolStripMenuItem();
            uploadImageToCommunityMenuItem = new ToolStripMenuItem();
            searchMenu = new ContextMenuStrip(components);
            sIMBADSearchToolStripMenuItem = new ToolStripMenuItem();
            vORegistryToolStripMenuItem = new ToolStripMenuItem();
            findEarthBasedLocationToolStripMenuItem = new ToolStripMenuItem();
            toursMenu = new ContextMenuStrip(components);
            tourHomeMenuItem = new ToolStripMenuItem();
            tourSearchWebPageMenuItem = new ToolStripMenuItem();
            musicAndOtherTourResourceToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            createANewTourToolStripMenuItem = new ToolStripMenuItem();
            saveTourAsToolStripMenuItem = new ToolStripMenuItem();
            publishTourMenuItem = new ToolStripMenuItem();
            renderToVideoToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            autoRepeatToolStripMenuItem = new ToolStripMenuItem();
            oneToolStripMenuItem = new ToolStripMenuItem();
            allToolStripMenuItem = new ToolStripMenuItem();
            offToolStripMenuItem = new ToolStripMenuItem();
            editTourToolStripMenuItem = new ToolStripMenuItem();
            showOverlayListToolStripMenuItem = new ToolStripMenuItem();
            showKeyframerToolStripMenuItem = new ToolStripMenuItem();
            showSlideNumbersToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem12 = new ToolStripSeparator();
            undoToolStripMenuItem = new ToolStripMenuItem();
            redoToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem13 = new ToolStripSeparator();
            publishTourToCommunityToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator23 = new ToolStripSeparator();
            sendTourToProjectorServersToolStripMenuItem = new ToolStripMenuItem();
            automaticTourSyncWithProjectorServersToolStripMenuItem = new ToolStripMenuItem();
            telescopeMenu = new ContextMenuStrip(components);
            slewTelescopeMenuItem = new ToolStripMenuItem();
            centerTelescopeMenuItem = new ToolStripMenuItem();
            SyncTelescopeMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            chooseTelescopeMenuItem = new ToolStripMenuItem();
            connectTelescopeMenuItem = new ToolStripMenuItem();
            trackScopeMenuItem = new ToolStripMenuItem();
            toolStripSeparator12 = new ToolStripSeparator();
            parkTelescopeMenuItem = new ToolStripMenuItem();
            toolStripSeparator13 = new ToolStripSeparator();
            ASCOMPlatformHomePage = new ToolStripMenuItem();
            exploreMenu = new ContextMenuStrip(components);
            createNewObservingListToolStripMenuItem = new ToolStripMenuItem();
            newObservingListpMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            newSimpleTourMenuItem = new ToolStripMenuItem();
            openFileToolStripMenuItem = new ToolStripMenuItem();
            openTourMenuItem = new ToolStripMenuItem();
            openObservingListMenuItem = new ToolStripMenuItem();
            layersToolStripMenuItem = new ToolStripMenuItem();
            openImageMenuItem = new ToolStripMenuItem();
            openKMLMenuItem = new ToolStripMenuItem();
            vOTableToolStripMenuItem = new ToolStripMenuItem();
            shapeFileToolStripMenuItem = new ToolStripMenuItem();
            layerManagerToolStripMenuItem = new ToolStripMenuItem();
            customGalaxyFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            showFinderToolStripMenuItem = new ToolStripMenuItem();
            playCollectionAsSlideShowToolStripMenuItem = new ToolStripMenuItem();
            addCollectionAsTourStopsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            ShowWelcomeTips = new ToolStripMenuItem();
            aboutMenuItem = new ToolStripMenuItem();
            gettingStarteMenuItem = new ToolStripMenuItem();
            homepageMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            exitMenuItem = new ToolStripMenuItem();
            settingsMenu = new ContextMenuStrip(components);
            checkForUpdatesToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            feedbackToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator17 = new ToolStripSeparator();
            restoreDefaultsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator16 = new ToolStripSeparator();
            advancedToolStripMenuItem = new ToolStripMenuItem();
            downloadQueueToolStripMenuItem = new ToolStripMenuItem();
            startQueueToolStripMenuItem = new ToolStripMenuItem();
            stopQueueToolStripMenuItem = new ToolStripMenuItem();
            tileLoadingThrottlingToolStripMenuItem = new ToolStripMenuItem();
            tpsToolStripMenuItem15 = new ToolStripMenuItem();
            tpsToolStripMenuItem30 = new ToolStripMenuItem();
            tpsToolStripMenuItem60 = new ToolStripMenuItem();
            tpsToolStripMenuItem120 = new ToolStripMenuItem();
            tpsToolStripMenuItemUnlimited = new ToolStripMenuItem();
            toolStripMenuItem8 = new ToolStripSeparator();
            saveCacheAsCabinetFileToolStripMenuItem = new ToolStripMenuItem();
            restoreCacheFromCabinetFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator22 = new ToolStripSeparator();
            flushCacheToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator18 = new ToolStripSeparator();
            showPerformanceDataToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator19 = new ToolStripSeparator();
            toolStripMenuItem2 = new ToolStripMenuItem();
            multiChanelCalibrationToolStripMenuItem = new ToolStripMenuItem();
            clientNodeListToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem6 = new ToolStripSeparator();
            sendLayersToProjectorServersToolStripMenuItem = new ToolStripMenuItem();
            mIDIControllerSetupToolStripMenuItem = new ToolStripMenuItem();
            xBoxControllerSetupToolStripMenuItem = new ToolStripMenuItem();
            remoteAccessControlToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem14 = new ToolStripSeparator();
            selectLanguageToolStripMenuItem = new ToolStripMenuItem();
            regionalDataCacheToolStripMenuItem = new ToolStripMenuItem();
            viewMenu = new ContextMenuStrip(components);
            resetCameraMenuItem = new ToolStripMenuItem();
            showTouchControlsToolStripMenuItem = new ToolStripMenuItem();
            monochromeStyleToolStripMenuItem = new ToolStripMenuItem();
            allowUnconstrainedTiltToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator9 = new ToolStripSeparator();
            startupToolStripMenuItem = new ToolStripMenuItem();
            earthToolStripMenuItem = new ToolStripMenuItem();
            planetToolStripMenuItem = new ToolStripMenuItem();
            skyToolStripMenuItem = new ToolStripMenuItem();
            panoramaToolStripMenuItem = new ToolStripMenuItem();
            solarSystemToolStripMenuItem = new ToolStripMenuItem();
            lastToolStripMenuItem = new ToolStripMenuItem();
            randomToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripSeparator();
            copyCurrentViewToClipboardToolStripMenuItem = new ToolStripMenuItem();
            copyShortCutToThisViewToClipboardToolStripMenuItem = new ToolStripMenuItem();
            saveCurrentViewImageToFileToolStripMenuItem = new ToolStripMenuItem();
            setCurrentViewAsWindowsDesktopBackgroundToolStripMenuItem = new ToolStripMenuItem();
            exportCurrentViewAsSTLFileFor3DPrintingToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator21 = new ToolStripSeparator();
            screenBroadcastToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator14 = new ToolStripSeparator();
            imageStackToolStripMenuItem = new ToolStripMenuItem();
            showLayerManagerToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator20 = new ToolStripSeparator();
            stereoToolStripMenuItem = new ToolStripMenuItem();
            enabledToolStripMenuItem = new ToolStripMenuItem();
            anaglyphToolStripMenuItem = new ToolStripMenuItem();
            anaglyphYellowBlueToolStripMenuItem = new ToolStripMenuItem();
            sideBySideProjectionToolStripMenuItem = new ToolStripMenuItem();
            sideBySideCrossEyedToolStripMenuItem = new ToolStripMenuItem();
            alternatingLinesOddToolStripMenuItem = new ToolStripMenuItem();
            alternatingLinesEvenToolStripMenuItem = new ToolStripMenuItem();
            oculusRiftToolStripMenuItem = new ToolStripMenuItem();
            expermentalToolStripMenuItem = new ToolStripMenuItem();
            fullDomeToolStripMenuItem = new ToolStripMenuItem();
            newFullDomeViewInstanceToolStripMenuItem = new ToolStripMenuItem();
            monitorOneToolStripMenuItem = new ToolStripMenuItem();
            monitorTwoToolStripMenuItem = new ToolStripMenuItem();
            monitorThreeToolStripMenuItem = new ToolStripMenuItem();
            monitorFourToolStripMenuItem = new ToolStripMenuItem();
            monitorFiveToolStripMenuItem = new ToolStripMenuItem();
            monitorSixToolStripMenuItem = new ToolStripMenuItem();
            monitorSevenToolStripMenuItem = new ToolStripMenuItem();
            monitorEightToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem15 = new ToolStripSeparator();
            domeSetupToolStripMenuItem = new ToolStripMenuItem();
            listenUpBoysToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem11 = new ToolStripSeparator();
            detachMainViewToSecondMonitor = new ToolStripMenuItem();
            detachMainViewToThirdMonitorToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem10 = new ToolStripSeparator();
            fullDomePreviewToolStripMenuItem = new ToolStripMenuItem();
            toggleFullScreenModeF11ToolStripMenuItem = new ToolStripMenuItem();
            multiSampleAntialiasingToolStripMenuItem = new ToolStripMenuItem();
            noneToolStripMenuItem = new ToolStripMenuItem();
            fourSamplesToolStripMenuItem = new ToolStripMenuItem();
            eightSamplesToolStripMenuItem = new ToolStripMenuItem();
            lockVerticalSyncToolStripMenuItem = new ToolStripMenuItem();
            targetFrameRateToolStripMenuItem = new ToolStripMenuItem();
            fpsToolStripMenuItemUnlimited = new ToolStripMenuItem();
            fPSToolStripMenuItem60 = new ToolStripMenuItem();
            fPSToolStripMenuItem30 = new ToolStripMenuItem();
            fPSToolStripMenuItem24 = new ToolStripMenuItem();
            StatupTimer = new Timer(components);
            SlideAdvanceTimer = new Timer(components);
            TourEndCheck = new Timer(components);
            autoSaveTimer = new Timer(components);
            DeviceHeartbeat = new Timer(components);
            kioskTitleBar = new KioskTitleBar();
            renderWindow = new RenderTarget();
            menuTabs = new MenuTabs();
            contextMenu.SuspendLayout();
            communitiesMenu.SuspendLayout();
            searchMenu.SuspendLayout();
            toursMenu.SuspendLayout();
            telescopeMenu.SuspendLayout();
            exploreMenu.SuspendLayout();
            settingsMenu.SuspendLayout();
            viewMenu.SuspendLayout();
            SuspendLayout();
            // 
            // timer
            // 
            timer.Tick += timer1_Tick;
            // 
            // Seperator
            // 
            menuItem7.Name = "menuItem7";
            menuItem7.Size = new Size(32, 19);
            menuItem7.Text = "-";
            // 
            // viewOverlayTopo
            // 
            viewOverlayTopo.Name = "viewOverlayTopo";
            viewOverlayTopo.Size = new Size(32, 19);
            // 
            // InputTimer
            // 
            InputTimer.Enabled = true;
            InputTimer.Interval = 350;
            InputTimer.Tick += timer2_Tick;
            // 
            // contextMenu
            // 
            contextMenu.Items.AddRange(new ToolStripItem[] {
            nameToolStripMenuItem,
            toolStripSeparator11,
            informationToolStripMenuItem,
            imageryToolStripMenuItem,
            virtualObservatorySearchesToolStripMenuItem,
            toolStripSeparator15,
            setAsForegroundImageryToolStripMenuItem,
            setAsBackgroundImageryToolStripMenuItem,
            addToImageStackToolStripMenuItem,
            addAsNewLayerToolStripMenuItem,
            cacheManagementToolStripMenuItem1,
            ImagerySeperator,
            propertiesToolStripMenuItem,
            copyShortcutToolStripMenuItem,
            addToCollectionsToolStripMenuItem,
            removeFromCollectionToolStripMenuItem,
            editToolStripMenuItem,
            sAMPToolStripMenuItem});
            contextMenu.Name = "contextMenu";
            contextMenu.Size = new Size(225, 352);
            contextMenu.Closing += contextMenu_Closing;
            contextMenu.Opening += contextMenu_Opening;
            // 
            // nameToolStripMenuItem
            // 
            nameToolStripMenuItem.Name = "nameToolStripMenuItem";
            nameToolStripMenuItem.Size = new Size(224, 22);
            nameToolStripMenuItem.Text = "Name:";
            nameToolStripMenuItem.Click += nameToolStripMenuItem_Click;
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new Size(221, 6);
            // 
            // informationToolStripMenuItem
            // 
            informationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            lookupOnSimbadToolStripMenuItem,
            lookupOnSEDSToolStripMenuItem,
            lookupOnWikipediaToolStripMenuItem,
            publicationsToolStripMenuItem,
            lookUpOnNEDToolStripMenuItem,
            lookUpOnSDSSToolStripMenuItem});
            informationToolStripMenuItem.Name = "informationToolStripMenuItem";
            informationToolStripMenuItem.Size = new Size(224, 22);
            informationToolStripMenuItem.Text = "Information";
            // 
            // lookupOnSimbadToolStripMenuItem
            // 
            lookupOnSimbadToolStripMenuItem.Name = "lookupOnSimbadToolStripMenuItem";
            lookupOnSimbadToolStripMenuItem.Size = new Size(227, 22);
            lookupOnSimbadToolStripMenuItem.Text = "Look up on SIMBAD";
            lookupOnSimbadToolStripMenuItem.Click += lookupOnSimbadToolStripMenuItem_Click;
            // 
            // lookupOnSEDSToolStripMenuItem
            // 
            lookupOnSEDSToolStripMenuItem.Name = "lookupOnSEDSToolStripMenuItem";
            lookupOnSEDSToolStripMenuItem.Size = new Size(227, 22);
            lookupOnSEDSToolStripMenuItem.Text = "Look up on SEDS";
            lookupOnSEDSToolStripMenuItem.Click += lookupOnSEDSToolStripMenuItem_Click;
            // 
            // lookupOnWikipediaToolStripMenuItem
            // 
            lookupOnWikipediaToolStripMenuItem.Name = "lookupOnWikipediaToolStripMenuItem";
            lookupOnWikipediaToolStripMenuItem.Size = new Size(227, 22);
            lookupOnWikipediaToolStripMenuItem.Text = "Look up on Wikipedia";
            lookupOnWikipediaToolStripMenuItem.Click += lookupOnWikipediaToolStripMenuItem_Click;
            // 
            // publicationsToolStripMenuItem
            // 
            publicationsToolStripMenuItem.Name = "publicationsToolStripMenuItem";
            publicationsToolStripMenuItem.Size = new Size(227, 22);
            publicationsToolStripMenuItem.Text = "Look up publications on ADS";
            publicationsToolStripMenuItem.Click += publicationsToolStripMenuItem_Click;
            // 
            // lookUpOnNEDToolStripMenuItem
            // 
            lookUpOnNEDToolStripMenuItem.Name = "lookUpOnNEDToolStripMenuItem";
            lookUpOnNEDToolStripMenuItem.Size = new Size(227, 22);
            lookUpOnNEDToolStripMenuItem.Text = "Look up on NED";
            lookUpOnNEDToolStripMenuItem.Click += lookUpOnNEDToolStripMenuItem_Click;
            // 
            // lookUpOnSDSSToolStripMenuItem
            // 
            lookUpOnSDSSToolStripMenuItem.Name = "lookUpOnSDSSToolStripMenuItem";
            lookUpOnSDSSToolStripMenuItem.Size = new Size(227, 22);
            lookUpOnSDSSToolStripMenuItem.Text = "Look up on SDSS";
            lookUpOnSDSSToolStripMenuItem.Click += lookUpOnSDSSToolStripMenuItem_Click;
            // 
            // imageryToolStripMenuItem
            // 
            imageryToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            getDSSImageToolStripMenuItem,
            getSDSSImageToolStripMenuItem,
            getDSSFITSToolStripMenuItem});
            imageryToolStripMenuItem.Name = "imageryToolStripMenuItem";
            imageryToolStripMenuItem.Size = new Size(224, 22);
            imageryToolStripMenuItem.Text = "Imagery";
            // 
            // getDSSImageToolStripMenuItem
            // 
            getDSSImageToolStripMenuItem.Name = "getDSSImageToolStripMenuItem";
            getDSSImageToolStripMenuItem.Size = new Size(157, 22);
            getDSSImageToolStripMenuItem.Text = "Get DSS image";
            getDSSImageToolStripMenuItem.Click += getDSSImageToolStripMenuItem_Click;
            // 
            // getSDSSImageToolStripMenuItem
            // 
            getSDSSImageToolStripMenuItem.Name = "getSDSSImageToolStripMenuItem";
            getSDSSImageToolStripMenuItem.Size = new Size(157, 22);
            getSDSSImageToolStripMenuItem.Text = "Get SDSS image";
            getSDSSImageToolStripMenuItem.Click += getSDSSImageToolStripMenuItem_Click;
            // 
            // getDSSFITSToolStripMenuItem
            // 
            getDSSFITSToolStripMenuItem.Name = "getDSSFITSToolStripMenuItem";
            getDSSFITSToolStripMenuItem.Size = new Size(157, 22);
            getDSSFITSToolStripMenuItem.Text = "Get DSS FITS";
            getDSSFITSToolStripMenuItem.Click += getDSSFITSToolStripMenuItem_Click;
            // 
            // virtualObservatorySearchesToolStripMenuItem
            // 
            virtualObservatorySearchesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            uSNONVOConeSearchToolStripMenuItem,
            hLAFootprintsToolStripMenuItem,
            nEDSearchToolStripMenuItem,
            sDSSSearchToolStripMenuItem,
            toolStripMenuItem3});
            virtualObservatorySearchesToolStripMenuItem.Name = "virtualObservatorySearchesToolStripMenuItem";
            virtualObservatorySearchesToolStripMenuItem.Size = new Size(224, 22);
            virtualObservatorySearchesToolStripMenuItem.Text = "Virtual Observatory Searches";
            virtualObservatorySearchesToolStripMenuItem.Click += virtualObservatorySearchesToolStripMenuItem_Click;
            // 
            // uSNONVOConeSearchToolStripMenuItem
            // 
            uSNONVOConeSearchToolStripMenuItem.Name = "uSNONVOConeSearchToolStripMenuItem";
            uSNONVOConeSearchToolStripMenuItem.Size = new Size(264, 22);
            uSNONVOConeSearchToolStripMenuItem.Text = "USNO NVO cone search";
            uSNONVOConeSearchToolStripMenuItem.Visible = false;
            uSNONVOConeSearchToolStripMenuItem.Click += uSNONVOConeSearchToolStripMenuItem_Click;
            // 
            // hLAFootprintsToolStripMenuItem
            // 
            hLAFootprintsToolStripMenuItem.MergeAction = MergeAction.Insert;
            hLAFootprintsToolStripMenuItem.Name = "hLAFootprintsToolStripMenuItem";
            hLAFootprintsToolStripMenuItem.Size = new Size(264, 22);
            hLAFootprintsToolStripMenuItem.Text = "HLA Footprints";
            hLAFootprintsToolStripMenuItem.Visible = false;
            hLAFootprintsToolStripMenuItem.Click += hLAFootprintsToolStripMenuItem_Click;
            // 
            // nEDSearchToolStripMenuItem
            // 
            nEDSearchToolStripMenuItem.Name = "nEDSearchToolStripMenuItem";
            nEDSearchToolStripMenuItem.Size = new Size(264, 22);
            nEDSearchToolStripMenuItem.Text = "NED Search";
            nEDSearchToolStripMenuItem.Click += NEDSearchToolStripMenuItem_Click;
            // 
            // sDSSSearchToolStripMenuItem
            // 
            sDSSSearchToolStripMenuItem.Name = "sDSSSearchToolStripMenuItem";
            sDSSSearchToolStripMenuItem.Size = new Size(264, 22);
            sDSSSearchToolStripMenuItem.Text = "SDSS Search";
            sDSSSearchToolStripMenuItem.Click += sDSSSearchToolStripMenuItem_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(264, 22);
            toolStripMenuItem3.Text = "VO Cone Search / Registry Lookup...";
            toolStripMenuItem3.Click += vORegistryToolStripMenuItem_Click;
            // 
            // toolStripSeparator15
            // 
            toolStripSeparator15.Name = "toolStripSeparator15";
            toolStripSeparator15.Size = new Size(221, 6);
            // 
            // setAsForegroundImageryToolStripMenuItem
            // 
            setAsForegroundImageryToolStripMenuItem.Name = "setAsForegroundImageryToolStripMenuItem";
            setAsForegroundImageryToolStripMenuItem.Size = new Size(224, 22);
            setAsForegroundImageryToolStripMenuItem.Text = "Set as Forground Imagery";
            setAsForegroundImageryToolStripMenuItem.Click += setAsForegroundImageryToolStripMenuItem_Click;
            // 
            // setAsBackgroundImageryToolStripMenuItem
            // 
            setAsBackgroundImageryToolStripMenuItem.Name = "setAsBackgroundImageryToolStripMenuItem";
            setAsBackgroundImageryToolStripMenuItem.Size = new Size(224, 22);
            setAsBackgroundImageryToolStripMenuItem.Text = "Set as Background Imagery";
            setAsBackgroundImageryToolStripMenuItem.Click += setAsBackgroundImageryToolStripMenuItem_Click;
            // 
            // addToImageStackToolStripMenuItem
            // 
            addToImageStackToolStripMenuItem.Name = "addToImageStackToolStripMenuItem";
            addToImageStackToolStripMenuItem.Size = new Size(224, 22);
            addToImageStackToolStripMenuItem.Text = "Add to Image Stack";
            addToImageStackToolStripMenuItem.Click += addToImageStackToolStripMenuItem_Click;
            // 
            // addAsNewLayerToolStripMenuItem
            // 
            addAsNewLayerToolStripMenuItem.Name = "addAsNewLayerToolStripMenuItem";
            addAsNewLayerToolStripMenuItem.Size = new Size(224, 22);
            addAsNewLayerToolStripMenuItem.Text = "Add as New Layer";
            addAsNewLayerToolStripMenuItem.Click += addAsNewLayerToolStripMenuItem_Click;
            // 
            // cacheManagementToolStripMenuItem1
            // 
            cacheManagementToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] {
            cacheImageryTilePyramidToolStripMenuItem,
            showCacheSpaceUsedToolStripMenuItem,
            removeFromImageCacheToolStripMenuItem});
            cacheManagementToolStripMenuItem1.Name = "cacheManagementToolStripMenuItem1";
            cacheManagementToolStripMenuItem1.Size = new Size(224, 22);
            cacheManagementToolStripMenuItem1.Text = "Cache Management";
            // 
            // cacheImageryTilePyramidToolStripMenuItem
            // 
            cacheImageryTilePyramidToolStripMenuItem.Name = "cacheImageryTilePyramidToolStripMenuItem";
            cacheImageryTilePyramidToolStripMenuItem.Size = new Size(231, 22);
            cacheImageryTilePyramidToolStripMenuItem.Text = "Cache Imagery Tile Pyramid...";
            cacheImageryTilePyramidToolStripMenuItem.Click += cacheImageryTilePyramidToolStripMenuItem_Click;
            // 
            // showCacheSpaceUsedToolStripMenuItem
            // 
            showCacheSpaceUsedToolStripMenuItem.Name = "showCacheSpaceUsedToolStripMenuItem";
            showCacheSpaceUsedToolStripMenuItem.Size = new Size(231, 22);
            showCacheSpaceUsedToolStripMenuItem.Text = "Show Cache Space Used...";
            showCacheSpaceUsedToolStripMenuItem.Click += showCacheSpaceUsedToolStripMenuItem_Click;
            // 
            // removeFromImageCacheToolStripMenuItem
            // 
            removeFromImageCacheToolStripMenuItem.Name = "removeFromImageCacheToolStripMenuItem";
            removeFromImageCacheToolStripMenuItem.Size = new Size(231, 22);
            removeFromImageCacheToolStripMenuItem.Text = "Remove from Image Cache";
            removeFromImageCacheToolStripMenuItem.Click += removeFromImageCacheToolStripMenuItem_Click;
            // 
            // ImagerySeperator
            // 
            ImagerySeperator.Name = "ImagerySeperator";
            ImagerySeperator.Size = new Size(221, 6);
            // 
            // propertiesToolStripMenuItem
            // 
            propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            propertiesToolStripMenuItem.Size = new Size(224, 22);
            propertiesToolStripMenuItem.Text = "Properties";
            propertiesToolStripMenuItem.Click += propertiesToolStripMenuItem_Click;
            // 
            // copyShortcutToolStripMenuItem
            // 
            copyShortcutToolStripMenuItem.Name = "copyShortcutToolStripMenuItem";
            copyShortcutToolStripMenuItem.Size = new Size(224, 22);
            copyShortcutToolStripMenuItem.Text = "Copy Shortcut";
            copyShortcutToolStripMenuItem.Click += copyShortcutToolStripMenuItem_Click;
            // 
            // addToCollectionsToolStripMenuItem
            // 
            addToCollectionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            newCollectionToolStripMenuItem});
            addToCollectionsToolStripMenuItem.Name = "addToCollectionsToolStripMenuItem";
            addToCollectionsToolStripMenuItem.Size = new Size(224, 22);
            addToCollectionsToolStripMenuItem.Text = "Add to Collection";
            addToCollectionsToolStripMenuItem.DropDownOpening += addToCollectionsToolStripMenuItem_DropDownOpening;
            addToCollectionsToolStripMenuItem.Click += addToCollectionsToolStripMenuItem_Click;
            // 
            // newCollectionToolStripMenuItem
            // 
            newCollectionToolStripMenuItem.Name = "newCollectionToolStripMenuItem";
            newCollectionToolStripMenuItem.Size = new Size(164, 22);
            newCollectionToolStripMenuItem.Text = "New Collection...";
            // 
            // removeFromCollectionToolStripMenuItem
            // 
            removeFromCollectionToolStripMenuItem.Name = "removeFromCollectionToolStripMenuItem";
            removeFromCollectionToolStripMenuItem.Size = new Size(224, 22);
            removeFromCollectionToolStripMenuItem.Text = "Remove from Collection";
            removeFromCollectionToolStripMenuItem.Click += removeFromCollectionToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(224, 22);
            editToolStripMenuItem.Text = "Edit...";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // sAMPToolStripMenuItem
            // 
            sAMPToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            sendImageToToolStripMenuItem,
            sendTableToToolStripMenuItem});
            sAMPToolStripMenuItem.Name = "sAMPToolStripMenuItem";
            sAMPToolStripMenuItem.Size = new Size(224, 22);
            sAMPToolStripMenuItem.Text = "SAMP";
            // 
            // sendImageToToolStripMenuItem
            // 
            sendImageToToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            broadcastToolStripMenuItem});
            sendImageToToolStripMenuItem.Name = "sendImageToToolStripMenuItem";
            sendImageToToolStripMenuItem.Size = new Size(153, 22);
            sendImageToToolStripMenuItem.Text = "Send Image To";
            // 
            // broadcastToolStripMenuItem
            // 
            broadcastToolStripMenuItem.Name = "broadcastToolStripMenuItem";
            broadcastToolStripMenuItem.Size = new Size(126, 22);
            broadcastToolStripMenuItem.Text = "Broadcast";
            broadcastToolStripMenuItem.Click += broadcastToolStripMenuItem_Click;
            // 
            // sendTableToToolStripMenuItem
            // 
            sendTableToToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            broadcastToolStripMenuItem1});
            sendTableToToolStripMenuItem.Name = "sendTableToToolStripMenuItem";
            sendTableToToolStripMenuItem.Size = new Size(153, 22);
            sendTableToToolStripMenuItem.Text = "Send Table To";
            // 
            // broadcastToolStripMenuItem1
            // 
            broadcastToolStripMenuItem1.Name = "broadcastToolStripMenuItem1";
            broadcastToolStripMenuItem1.Size = new Size(126, 22);
            broadcastToolStripMenuItem1.Text = "Broadcast";
            // 
            // HoverTimer
            // 
            HoverTimer.Enabled = true;
            HoverTimer.Interval = 500;
            HoverTimer.Tick += HoverTimer_Tick;
            // 
            // communitiesMenu
            // 
            communitiesMenu.Items.AddRange(new ToolStripItem[] {
            joinCoomunityMenuItem,
            updateLoginCredentialsMenuItem,
            logoutMenuItem,
            toolStripSeparator8,
            uploadObservingListToCommunityMenuItem,
            uploadImageToCommunityMenuItem});
            communitiesMenu.Name = "communitiesMenu";
            communitiesMenu.Size = new Size(281, 120);
            communitiesMenu.Closed += PopupClosed;
            communitiesMenu.Opening += communitiesMenu_Opening;
            communitiesMenu.PreviewKeyDown += exploreMenu_PreviewKeyDown;
            // 
            // joinCoomunityMenuItem
            // 
            joinCoomunityMenuItem.Name = "joinCoomunityMenuItem";
            joinCoomunityMenuItem.Size = new Size(280, 22);
            joinCoomunityMenuItem.Text = "Join a Community...";
            joinCoomunityMenuItem.Click += joinCoomunityMenuItem_Click;
            // 
            // updateLoginCredentialsMenuItem
            // 
            updateLoginCredentialsMenuItem.Name = "updateLoginCredentialsMenuItem";
            updateLoginCredentialsMenuItem.Size = new Size(280, 22);
            updateLoginCredentialsMenuItem.Text = "Update Login Credentials...";
            updateLoginCredentialsMenuItem.Click += associateLiveIDToolStripMenuItem_Click;
            // 
            // logoutMenuItem
            // 
            logoutMenuItem.Enabled = false;
            logoutMenuItem.Name = "logoutMenuItem";
            logoutMenuItem.Size = new Size(280, 22);
            logoutMenuItem.Text = "Logout";
            logoutMenuItem.Visible = false;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new Size(277, 6);
            toolStripSeparator8.Visible = false;
            // 
            // uploadObservingListToCommunityMenuItem
            // 
            uploadObservingListToCommunityMenuItem.Name = "uploadObservingListToCommunityMenuItem";
            uploadObservingListToCommunityMenuItem.Size = new Size(280, 22);
            uploadObservingListToCommunityMenuItem.Text = "Upload Observing List to Community...";
            uploadObservingListToCommunityMenuItem.Visible = false;
            // 
            // uploadImageToCommunityMenuItem
            // 
            uploadImageToCommunityMenuItem.Name = "uploadImageToCommunityMenuItem";
            uploadImageToCommunityMenuItem.Size = new Size(280, 22);
            uploadImageToCommunityMenuItem.Text = "Upload Image to Community... ";
            uploadImageToCommunityMenuItem.Visible = false;
            // 
            // searchMenu
            // 
            searchMenu.Items.AddRange(new ToolStripItem[] {
            sIMBADSearchToolStripMenuItem,
            vORegistryToolStripMenuItem,
            findEarthBasedLocationToolStripMenuItem});
            searchMenu.Name = "contextMenuStrip1";
            searchMenu.Size = new Size(265, 70);
            searchMenu.Closed += PopupClosed;
            searchMenu.Opening += searchMenu_Opening;
            searchMenu.PreviewKeyDown += exploreMenu_PreviewKeyDown;
            // 
            // sIMBADSearchToolStripMenuItem
            // 
            sIMBADSearchToolStripMenuItem.Name = "sIMBADSearchToolStripMenuItem";
            sIMBADSearchToolStripMenuItem.Size = new Size(264, 22);
            sIMBADSearchToolStripMenuItem.Text = "SIMBAD Search...";
            sIMBADSearchToolStripMenuItem.Click += sIMBADSearchToolStripMenuItem_Click;
            // 
            // vORegistryToolStripMenuItem
            // 
            vORegistryToolStripMenuItem.Name = "vORegistryToolStripMenuItem";
            vORegistryToolStripMenuItem.Size = new Size(264, 22);
            vORegistryToolStripMenuItem.Text = "VO Cone Search / Registry Lookup...";
            vORegistryToolStripMenuItem.Click += vORegistryToolStripMenuItem_Click;
            // 
            // findEarthBasedLocationToolStripMenuItem
            // 
            findEarthBasedLocationToolStripMenuItem.Name = "findEarthBasedLocationToolStripMenuItem";
            findEarthBasedLocationToolStripMenuItem.Size = new Size(264, 22);
            findEarthBasedLocationToolStripMenuItem.Text = "Find Earth Based Location...";
            findEarthBasedLocationToolStripMenuItem.Click += findEarthBasedLocationToolStripMenuItem_Click;
            // 
            // toursMenu
            // 
            toursMenu.Items.AddRange(new ToolStripItem[] {
            tourHomeMenuItem,
            tourSearchWebPageMenuItem,
            musicAndOtherTourResourceToolStripMenuItem,
            toolStripSeparator6,
            createANewTourToolStripMenuItem,
            saveTourAsToolStripMenuItem,
            publishTourMenuItem,
            renderToVideoToolStripMenuItem,
            toolStripSeparator1,
            autoRepeatToolStripMenuItem,
            editTourToolStripMenuItem,
            showOverlayListToolStripMenuItem,
            showKeyframerToolStripMenuItem,
            showSlideNumbersToolStripMenuItem,
            toolStripMenuItem12,
            undoToolStripMenuItem,
            redoToolStripMenuItem,
            toolStripMenuItem13,
            publishTourToCommunityToolStripMenuItem,
            toolStripSeparator23,
            sendTourToProjectorServersToolStripMenuItem,
            automaticTourSyncWithProjectorServersToolStripMenuItem});
            toursMenu.Name = "contextMenuStrip1";
            toursMenu.Size = new Size(304, 408);
            toursMenu.Closed += PopupClosed;
            toursMenu.Opening += toursMenu_Opening;
            toursMenu.PreviewKeyDown += exploreMenu_PreviewKeyDown;
            // 
            // tourHomeMenuItem
            // 
            tourHomeMenuItem.Name = "tourHomeMenuItem";
            tourHomeMenuItem.Size = new Size(303, 22);
            tourHomeMenuItem.Text = "Tour Home";
            tourHomeMenuItem.Click += tourHomeMenuItem_Click;
            // 
            // tourSearchWebPageMenuItem
            // 
            tourSearchWebPageMenuItem.Name = "tourSearchWebPageMenuItem";
            tourSearchWebPageMenuItem.Size = new Size(303, 22);
            tourSearchWebPageMenuItem.Text = "Tour Search Web Page";
            tourSearchWebPageMenuItem.Visible = false;
            tourSearchWebPageMenuItem.Click += tourSearchWebPageMenuItem_Click;
            // 
            // musicAndOtherTourResourceToolStripMenuItem
            // 
            musicAndOtherTourResourceToolStripMenuItem.Name = "musicAndOtherTourResourceToolStripMenuItem";
            musicAndOtherTourResourceToolStripMenuItem.Size = new Size(303, 22);
            musicAndOtherTourResourceToolStripMenuItem.Text = "Music and other Tour Resource";
            musicAndOtherTourResourceToolStripMenuItem.Click += musicAndOtherTourResourceToolStripMenuItem_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(300, 6);
            // 
            // createANewTourToolStripMenuItem
            // 
            createANewTourToolStripMenuItem.Name = "createANewTourToolStripMenuItem";
            createANewTourToolStripMenuItem.Size = new Size(303, 22);
            createANewTourToolStripMenuItem.Text = "Create a New Tour...";
            createANewTourToolStripMenuItem.Click += newSlideBasedTour;
            // 
            // saveTourAsToolStripMenuItem
            // 
            saveTourAsToolStripMenuItem.Name = "saveTourAsToolStripMenuItem";
            saveTourAsToolStripMenuItem.Size = new Size(303, 22);
            saveTourAsToolStripMenuItem.Text = "Save Tour As...";
            saveTourAsToolStripMenuItem.Click += saveTourAsToolStripMenuItem_Click;
            // 
            // publishTourMenuItem
            // 
            publishTourMenuItem.Name = "publishTourMenuItem";
            publishTourMenuItem.Size = new Size(303, 22);
            publishTourMenuItem.Text = "Submit Tour for Publication...";
            publishTourMenuItem.Click += publishTourMenuItem_Click;
            // 
            // renderToVideoToolStripMenuItem
            // 
            renderToVideoToolStripMenuItem.Name = "renderToVideoToolStripMenuItem";
            renderToVideoToolStripMenuItem.Size = new Size(303, 22);
            renderToVideoToolStripMenuItem.Text = "Render to Video...";
            renderToVideoToolStripMenuItem.Click += renderToVideoToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(300, 6);
            // 
            // autoRepeatToolStripMenuItem
            // 
            autoRepeatToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            oneToolStripMenuItem,
            allToolStripMenuItem,
            offToolStripMenuItem});
            autoRepeatToolStripMenuItem.Name = "autoRepeatToolStripMenuItem";
            autoRepeatToolStripMenuItem.Size = new Size(303, 22);
            autoRepeatToolStripMenuItem.Text = "Auto Repeat";
            // 
            // oneToolStripMenuItem
            // 
            oneToolStripMenuItem.Name = "oneToolStripMenuItem";
            oneToolStripMenuItem.Size = new Size(96, 22);
            oneToolStripMenuItem.Text = "One";
            oneToolStripMenuItem.Click += oneToolStripMenuItem_Click;
            // 
            // allToolStripMenuItem
            // 
            allToolStripMenuItem.Name = "allToolStripMenuItem";
            allToolStripMenuItem.Size = new Size(96, 22);
            allToolStripMenuItem.Text = "All";
            allToolStripMenuItem.Click += allToolStripMenuItem_Click;
            // 
            // offToolStripMenuItem
            // 
            offToolStripMenuItem.Name = "offToolStripMenuItem";
            offToolStripMenuItem.Size = new Size(96, 22);
            offToolStripMenuItem.Text = "Off";
            offToolStripMenuItem.Click += offToolStripMenuItem_Click;
            // 
            // editTourToolStripMenuItem
            // 
            editTourToolStripMenuItem.Name = "editTourToolStripMenuItem";
            editTourToolStripMenuItem.Size = new Size(303, 22);
            editTourToolStripMenuItem.Text = "Edit Tour";
            editTourToolStripMenuItem.Click += editTourToolStripMenuItem_Click;
            // 
            // showOverlayListToolStripMenuItem
            // 
            showOverlayListToolStripMenuItem.Name = "showOverlayListToolStripMenuItem";
            showOverlayListToolStripMenuItem.Size = new Size(303, 22);
            showOverlayListToolStripMenuItem.Text = "Show Overlay List";
            showOverlayListToolStripMenuItem.Click += showOverlayListToolStripMenuItem_Click;
            // 
            // showKeyframerToolStripMenuItem
            // 
            showKeyframerToolStripMenuItem.Name = "showKeyframerToolStripMenuItem";
            showKeyframerToolStripMenuItem.Size = new Size(303, 22);
            showKeyframerToolStripMenuItem.Text = "Show Timeline Editor";
            showKeyframerToolStripMenuItem.Click += showKeyframerToolStripMenuItem_Click;
            // 
            // showSlideNumbersToolStripMenuItem
            // 
            showSlideNumbersToolStripMenuItem.Name = "showSlideNumbersToolStripMenuItem";
            showSlideNumbersToolStripMenuItem.Size = new Size(303, 22);
            showSlideNumbersToolStripMenuItem.Text = "Show Slide Numbers";
            showSlideNumbersToolStripMenuItem.Click += showSlideNumbersToolStripMenuItem_Click;
            // 
            // toolStripMenuItem12
            // 
            toolStripMenuItem12.Name = "toolStripMenuItem12";
            toolStripMenuItem12.Size = new Size(300, 6);
            // 
            // undoToolStripMenuItem
            // 
            undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            undoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
            undoToolStripMenuItem.Size = new Size(303, 22);
            undoToolStripMenuItem.Text = "&Undo:";
            undoToolStripMenuItem.Click += undoToolStripMenuItem_Click;
            // 
            // redoToolStripMenuItem
            // 
            redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            redoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Y;
            redoToolStripMenuItem.Size = new Size(303, 22);
            redoToolStripMenuItem.Text = "Redo:";
            redoToolStripMenuItem.Click += redoToolStripMenuItem_Click;
            // 
            // toolStripMenuItem13
            // 
            toolStripMenuItem13.Name = "toolStripMenuItem13";
            toolStripMenuItem13.Size = new Size(300, 6);
            // 
            // publishTourToCommunityToolStripMenuItem
            // 
            publishTourToCommunityToolStripMenuItem.Name = "publishTourToCommunityToolStripMenuItem";
            publishTourToCommunityToolStripMenuItem.Size = new Size(303, 22);
            publishTourToCommunityToolStripMenuItem.Text = "Publish Tour to Community...";
            publishTourToCommunityToolStripMenuItem.Click += publishTourToCommunityToolStripMenuItem_Click;
            // 
            // toolStripSeparator23
            // 
            toolStripSeparator23.Name = "toolStripSeparator23";
            toolStripSeparator23.Size = new Size(300, 6);
            // 
            // sendTourToProjectorServersToolStripMenuItem
            // 
            sendTourToProjectorServersToolStripMenuItem.Name = "sendTourToProjectorServersToolStripMenuItem";
            sendTourToProjectorServersToolStripMenuItem.Size = new Size(303, 22);
            sendTourToProjectorServersToolStripMenuItem.Text = "Send Tour to Projector Servers";
            sendTourToProjectorServersToolStripMenuItem.Click += sendTourToProjectorServersToolStripMenuItem_Click;
            // 
            // automaticTourSyncWithProjectorServersToolStripMenuItem
            // 
            automaticTourSyncWithProjectorServersToolStripMenuItem.Name = "automaticTourSyncWithProjectorServersToolStripMenuItem";
            automaticTourSyncWithProjectorServersToolStripMenuItem.Size = new Size(303, 22);
            automaticTourSyncWithProjectorServersToolStripMenuItem.Text = "Automatic Tour Sync with Projector Servers";
            automaticTourSyncWithProjectorServersToolStripMenuItem.Click += automaticTourSyncWithProjectorServersToolStripMenuItem_Click;
            // 
            // telescopeMenu
            // 
            telescopeMenu.Items.AddRange(new ToolStripItem[] {
            slewTelescopeMenuItem,
            centerTelescopeMenuItem,
            SyncTelescopeMenuItem,
            toolStripSeparator3,
            chooseTelescopeMenuItem,
            connectTelescopeMenuItem,
            trackScopeMenuItem,
            toolStripSeparator12,
            parkTelescopeMenuItem,
            toolStripSeparator13,
            ASCOMPlatformHomePage});
            telescopeMenu.Name = "contextMenuStrip1";
            telescopeMenu.Size = new Size(241, 198);
            telescopeMenu.Closed += PopupClosed;
            telescopeMenu.Opening += telescopeMenu_Opening;
            telescopeMenu.PreviewKeyDown += exploreMenu_PreviewKeyDown;
            // 
            // slewTelescopeMenuItem
            // 
            slewTelescopeMenuItem.MergeIndex = 0;
            slewTelescopeMenuItem.Name = "slewTelescopeMenuItem";
            slewTelescopeMenuItem.Size = new Size(240, 22);
            slewTelescopeMenuItem.Text = "Slew To Object";
            slewTelescopeMenuItem.Click += slewTelescopeMenuItem_Click;
            // 
            // centerTelescopeMenuItem
            // 
            centerTelescopeMenuItem.MergeIndex = 1;
            centerTelescopeMenuItem.Name = "centerTelescopeMenuItem";
            centerTelescopeMenuItem.Size = new Size(240, 22);
            centerTelescopeMenuItem.Text = "Center on Scope";
            centerTelescopeMenuItem.Click += centerTelescopeMenuItem_Click;
            // 
            // SyncTelescopeMenuItem
            // 
            SyncTelescopeMenuItem.MergeIndex = 2;
            SyncTelescopeMenuItem.Name = "SyncTelescopeMenuItem";
            SyncTelescopeMenuItem.Size = new Size(240, 22);
            SyncTelescopeMenuItem.Text = "Sync Scope to Current Location";
            SyncTelescopeMenuItem.Click += SyncTelescopeMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.MergeIndex = 3;
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(237, 6);
            // 
            // chooseTelescopeMenuItem
            // 
            chooseTelescopeMenuItem.MergeIndex = 4;
            chooseTelescopeMenuItem.Name = "chooseTelescopeMenuItem";
            chooseTelescopeMenuItem.Size = new Size(240, 22);
            chooseTelescopeMenuItem.Text = "Choose Telescope";
            chooseTelescopeMenuItem.Click += chooseTelescopeMenuItem_Click;
            // 
            // connectTelescopeMenuItem
            // 
            connectTelescopeMenuItem.AccessibleName = "";
            connectTelescopeMenuItem.MergeIndex = 5;
            connectTelescopeMenuItem.Name = "connectTelescopeMenuItem";
            connectTelescopeMenuItem.Size = new Size(240, 22);
            connectTelescopeMenuItem.Text = "Connect";
            connectTelescopeMenuItem.Click += connectTelescopeMenuItem_Click;
            // 
            // trackScopeMenuItem
            // 
            trackScopeMenuItem.MergeIndex = 6;
            trackScopeMenuItem.Name = "trackScopeMenuItem";
            trackScopeMenuItem.Size = new Size(240, 22);
            trackScopeMenuItem.Text = "Track Telescope";
            trackScopeMenuItem.Click += trackScopeMenuItem_Click;
            // 
            // toolStripSeparator12
            // 
            toolStripSeparator12.MergeIndex = 7;
            toolStripSeparator12.Name = "toolStripSeparator12";
            toolStripSeparator12.Size = new Size(237, 6);
            // 
            // parkTelescopeMenuItem
            // 
            parkTelescopeMenuItem.MergeIndex = 8;
            parkTelescopeMenuItem.Name = "parkTelescopeMenuItem";
            parkTelescopeMenuItem.Size = new Size(240, 22);
            parkTelescopeMenuItem.Text = "Park";
            parkTelescopeMenuItem.Click += parkTelescopeMenuItem_Click;
            // 
            // toolStripSeparator13
            // 
            toolStripSeparator13.Name = "toolStripSeparator13";
            toolStripSeparator13.Size = new Size(237, 6);
            // 
            // ASCOMPlatformHomePage
            // 
            ASCOMPlatformHomePage.Name = "ASCOMPlatformHomePage";
            ASCOMPlatformHomePage.Size = new Size(240, 22);
            ASCOMPlatformHomePage.Text = "ASCOM Platform";
            ASCOMPlatformHomePage.Click += AscomPlatformMenuItem_Click;
            // 
            // exploreMenu
            // 
            exploreMenu.Items.AddRange(new ToolStripItem[] {
            createNewObservingListToolStripMenuItem,
            openFileToolStripMenuItem,
            toolStripSeparator7,
            showFinderToolStripMenuItem,
            playCollectionAsSlideShowToolStripMenuItem,
            addCollectionAsTourStopsToolStripMenuItem,
            toolStripSeparator2,
            ShowWelcomeTips,
            aboutMenuItem,
            gettingStarteMenuItem,
            homepageMenuItem,
            toolStripSeparator4,
            exitMenuItem});
            exploreMenu.Name = "contextMenuStrip1";
            exploreMenu.Size = new Size(255, 242);
            exploreMenu.Closed += PopupClosed;
            exploreMenu.Opening += exploreMenu_Opening;
            exploreMenu.PreviewKeyDown += exploreMenu_PreviewKeyDown;
            // 
            // createNewObservingListToolStripMenuItem
            // 
            createNewObservingListToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            newObservingListpMenuItem,
            toolStripSeparator5,
            newSimpleTourMenuItem});
            createNewObservingListToolStripMenuItem.Name = "createNewObservingListToolStripMenuItem";
            createNewObservingListToolStripMenuItem.Size = new Size(254, 22);
            createNewObservingListToolStripMenuItem.Text = "New";
            // 
            // newObservingListpMenuItem
            // 
            newObservingListpMenuItem.Name = "newObservingListpMenuItem";
            newObservingListpMenuItem.Size = new Size(172, 22);
            newObservingListpMenuItem.Text = "Collection...";
            newObservingListpMenuItem.Click += newObservingListpMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(169, 6);
            // 
            // newSimpleTourMenuItem
            // 
            newSimpleTourMenuItem.Name = "newSimpleTourMenuItem";
            newSimpleTourMenuItem.Size = new Size(172, 22);
            newSimpleTourMenuItem.Text = "Slide-Based Tour...";
            newSimpleTourMenuItem.Click += newSlideBasedTour;
            // 
            // openFileToolStripMenuItem
            // 
            openFileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            openTourMenuItem,
            openObservingListMenuItem,
            layersToolStripMenuItem,
            openImageMenuItem,
            openKMLMenuItem,
            vOTableToolStripMenuItem,
            shapeFileToolStripMenuItem,
            layerManagerToolStripMenuItem,
            customGalaxyFileToolStripMenuItem});
            openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            openFileToolStripMenuItem.Size = new Size(254, 22);
            openFileToolStripMenuItem.Text = "&Open";
            // 
            // openTourMenuItem
            // 
            openTourMenuItem.Name = "openTourMenuItem";
            openTourMenuItem.Size = new Size(190, 22);
            openTourMenuItem.Text = "Tour...";
            openTourMenuItem.Click += openTourMenuItem_Click;
            // 
            // openObservingListMenuItem
            // 
            openObservingListMenuItem.Name = "openObservingListMenuItem";
            openObservingListMenuItem.Size = new Size(190, 22);
            openObservingListMenuItem.Text = "Collection...";
            openObservingListMenuItem.Click += openObservingListMenuItem_Click;
            // 
            // layersToolStripMenuItem
            // 
            layersToolStripMenuItem.Name = "layersToolStripMenuItem";
            layersToolStripMenuItem.Size = new Size(190, 22);
            layersToolStripMenuItem.Text = "Layers...";
            layersToolStripMenuItem.Click += layersToolStripMenuItem_Click;
            // 
            // openImageMenuItem
            // 
            openImageMenuItem.Name = "openImageMenuItem";
            openImageMenuItem.Size = new Size(190, 22);
            openImageMenuItem.Text = "Astronomical Image...";
            openImageMenuItem.Click += openImageMenuItem_Click;
            // 
            // openKMLMenuItem
            // 
            openKMLMenuItem.Name = "openKMLMenuItem";
            openKMLMenuItem.Size = new Size(190, 22);
            openKMLMenuItem.Text = "KML...";
            openKMLMenuItem.Visible = false;
            openKMLMenuItem.Click += openKMLMenuItem_Click;
            // 
            // vOTableToolStripMenuItem
            // 
            vOTableToolStripMenuItem.Name = "vOTableToolStripMenuItem";
            vOTableToolStripMenuItem.Size = new Size(190, 22);
            vOTableToolStripMenuItem.Text = "VO Table...";
            vOTableToolStripMenuItem.Click += vOTableToolStripMenuItem_Click;
            // 
            // shapeFileToolStripMenuItem
            // 
            shapeFileToolStripMenuItem.Name = "shapeFileToolStripMenuItem";
            shapeFileToolStripMenuItem.Size = new Size(190, 22);
            shapeFileToolStripMenuItem.Text = "Shape File...";
            shapeFileToolStripMenuItem.Visible = false;
            shapeFileToolStripMenuItem.Click += shapeFileToolStripMenuItem_Click;
            // 
            // layerManagerToolStripMenuItem
            // 
            layerManagerToolStripMenuItem.Name = "layerManagerToolStripMenuItem";
            layerManagerToolStripMenuItem.Size = new Size(190, 22);
            layerManagerToolStripMenuItem.Text = "Layer Manager";
            layerManagerToolStripMenuItem.Click += layerManagerToolStripMenuItem_Click;
            // 
            // customGalaxyFileToolStripMenuItem
            // 
            customGalaxyFileToolStripMenuItem.Name = "customGalaxyFileToolStripMenuItem";
            customGalaxyFileToolStripMenuItem.Size = new Size(190, 22);
            customGalaxyFileToolStripMenuItem.Text = "Custom Galaxy File...";
            customGalaxyFileToolStripMenuItem.Click += customGalaxyFileToolStripMenuItem_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(251, 6);
            // 
            // showFinderToolStripMenuItem
            // 
            showFinderToolStripMenuItem.Name = "showFinderToolStripMenuItem";
            showFinderToolStripMenuItem.Size = new Size(254, 22);
            showFinderToolStripMenuItem.Text = "Show Finder";
            showFinderToolStripMenuItem.Click += showFinderToolStripMenuItem_Click;
            // 
            // playCollectionAsSlideShowToolStripMenuItem
            // 
            playCollectionAsSlideShowToolStripMenuItem.Name = "playCollectionAsSlideShowToolStripMenuItem";
            playCollectionAsSlideShowToolStripMenuItem.Size = new Size(254, 22);
            playCollectionAsSlideShowToolStripMenuItem.Text = "Play Collection as Slide Show";
            playCollectionAsSlideShowToolStripMenuItem.Click += playCollectionAsSlideShowToolStripMenuItem_Click;
            // 
            // addCollectionAsTourStopsToolStripMenuItem
            // 
            addCollectionAsTourStopsToolStripMenuItem.Name = "addCollectionAsTourStopsToolStripMenuItem";
            addCollectionAsTourStopsToolStripMenuItem.Size = new Size(254, 22);
            addCollectionAsTourStopsToolStripMenuItem.Text = "Add Collection as Tour Stops";
            addCollectionAsTourStopsToolStripMenuItem.Click += addCollectionAsTourStopsToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(251, 6);
            // 
            // ShowWelcomeTips
            // 
            ShowWelcomeTips.Name = "ShowWelcomeTips";
            ShowWelcomeTips.Size = new Size(254, 22);
            ShowWelcomeTips.Text = "Show Welcome Tips";
            ShowWelcomeTips.Click += ShowWelcomeTips_Click;
            // 
            // aboutMenuItem
            // 
            aboutMenuItem.Name = "aboutMenuItem";
            aboutMenuItem.Size = new Size(254, 22);
            aboutMenuItem.Text = "About WorldWide Telescope";
            aboutMenuItem.Click += aboutMenuItem_Click;
            // 
            // gettingStarteMenuItem
            // 
            gettingStarteMenuItem.Name = "gettingStarteMenuItem";
            gettingStarteMenuItem.Size = new Size(254, 22);
            gettingStarteMenuItem.Text = "Getting Started (Help)";
            gettingStarteMenuItem.Click += gettingStarteMenuItem_Click;
            // 
            // homepageMenuItem
            // 
            homepageMenuItem.Name = "homepageMenuItem";
            homepageMenuItem.Size = new Size(254, 22);
            homepageMenuItem.Text = "WorldWide Telescope Home Page";
            homepageMenuItem.Click += homepageMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(251, 6);
            // 
            // exitMenuItem
            // 
            exitMenuItem.Name = "exitMenuItem";
            exitMenuItem.Size = new Size(254, 22);
            exitMenuItem.Text = "E&xit";
            exitMenuItem.Click += exitMenuItem_Click;
            // 
            // settingsMenu
            // 
            settingsMenu.Items.AddRange(new ToolStripItem[] {
            checkForUpdatesToolStripMenuItem,
            toolStripMenuItem1,
            feedbackToolStripMenuItem,
            toolStripSeparator17,
            restoreDefaultsToolStripMenuItem,
            toolStripSeparator16,
            advancedToolStripMenuItem,
            mIDIControllerSetupToolStripMenuItem,
            xBoxControllerSetupToolStripMenuItem,
            remoteAccessControlToolStripMenuItem,
            toolStripMenuItem14,
            selectLanguageToolStripMenuItem,
            regionalDataCacheToolStripMenuItem});
            settingsMenu.Name = "contextMenuStrip1";
            settingsMenu.Size = new Size(207, 248);
            settingsMenu.Closed += PopupClosed;
            settingsMenu.Opening += settingsMenu_Opening;
            settingsMenu.PreviewKeyDown += exploreMenu_PreviewKeyDown;
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            checkForUpdatesToolStripMenuItem.Size = new Size(206, 22);
            checkForUpdatesToolStripMenuItem.Text = "Check for Updates...";
            checkForUpdatesToolStripMenuItem.Click += checkForUpdatesToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(203, 6);
            // 
            // feedbackToolStripMenuItem
            // 
            feedbackToolStripMenuItem.Name = "feedbackToolStripMenuItem";
            feedbackToolStripMenuItem.Size = new Size(206, 22);
            feedbackToolStripMenuItem.Text = "Product Support...";
            feedbackToolStripMenuItem.Click += feedbackToolStripMenuItem_Click;
            // 
            // toolStripSeparator17
            // 
            toolStripSeparator17.Name = "toolStripSeparator17";
            toolStripSeparator17.Size = new Size(203, 6);
            // 
            // restoreDefaultsToolStripMenuItem
            // 
            restoreDefaultsToolStripMenuItem.Name = "restoreDefaultsToolStripMenuItem";
            restoreDefaultsToolStripMenuItem.Size = new Size(206, 22);
            restoreDefaultsToolStripMenuItem.Text = "Restore Defaults";
            restoreDefaultsToolStripMenuItem.Click += restoreDefaultsToolStripMenuItem_Click;
            // 
            // toolStripSeparator16
            // 
            toolStripSeparator16.Name = "toolStripSeparator16";
            toolStripSeparator16.Size = new Size(203, 6);
            // 
            // advancedToolStripMenuItem
            // 
            advancedToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            downloadQueueToolStripMenuItem,
            startQueueToolStripMenuItem,
            stopQueueToolStripMenuItem,
            tileLoadingThrottlingToolStripMenuItem,
            toolStripMenuItem8,
            saveCacheAsCabinetFileToolStripMenuItem,
            restoreCacheFromCabinetFileToolStripMenuItem,
            toolStripSeparator22,
            flushCacheToolStripMenuItem,
            toolStripSeparator18,
            showPerformanceDataToolStripMenuItem,
            toolStripSeparator19,
            toolStripMenuItem2,
            multiChanelCalibrationToolStripMenuItem,
            clientNodeListToolStripMenuItem,
            toolStripMenuItem6,
            sendLayersToProjectorServersToolStripMenuItem});
            advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            advancedToolStripMenuItem.Size = new Size(206, 22);
            advancedToolStripMenuItem.Text = "Advanced";
            advancedToolStripMenuItem.DropDownOpening += advancedToolStripMenuItem_DropDownOpening;
            // 
            // downloadQueueToolStripMenuItem
            // 
            downloadQueueToolStripMenuItem.Name = "downloadQueueToolStripMenuItem";
            downloadQueueToolStripMenuItem.Size = new Size(252, 22);
            downloadQueueToolStripMenuItem.Text = "Show Download Queue";
            downloadQueueToolStripMenuItem.Click += showQueue_Click;
            // 
            // startQueueToolStripMenuItem
            // 
            startQueueToolStripMenuItem.Name = "startQueueToolStripMenuItem";
            startQueueToolStripMenuItem.Size = new Size(252, 22);
            startQueueToolStripMenuItem.Text = "Start Queue";
            startQueueToolStripMenuItem.Click += startQueue_Click;
            // 
            // stopQueueToolStripMenuItem
            // 
            stopQueueToolStripMenuItem.Name = "stopQueueToolStripMenuItem";
            stopQueueToolStripMenuItem.Size = new Size(252, 22);
            stopQueueToolStripMenuItem.Text = "Stop Queue";
            stopQueueToolStripMenuItem.Click += stopQueue_Click;
            // 
            // tileLoadingThrottlingToolStripMenuItem
            // 
            tileLoadingThrottlingToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            tpsToolStripMenuItem15,
            tpsToolStripMenuItem30,
            tpsToolStripMenuItem60,
            tpsToolStripMenuItem120,
            tpsToolStripMenuItemUnlimited});
            tileLoadingThrottlingToolStripMenuItem.Name = "tileLoadingThrottlingToolStripMenuItem";
            tileLoadingThrottlingToolStripMenuItem.Size = new Size(252, 22);
            tileLoadingThrottlingToolStripMenuItem.Text = "Tile Loading Throttling";
            tileLoadingThrottlingToolStripMenuItem.DropDownOpening += tileLoadingThrottlingToolStripMenuItem_DropDownOpening;
            // 
            // tpsToolStripMenuItem15
            // 
            tpsToolStripMenuItem15.Name = "tpsToolStripMenuItem15";
            tpsToolStripMenuItem15.Size = new Size(126, 22);
            tpsToolStripMenuItem15.Text = "15 tps";
            tpsToolStripMenuItem15.Click += tpsToolStripMenuItem15_Click;
            // 
            // tpsToolStripMenuItem30
            // 
            tpsToolStripMenuItem30.Name = "tpsToolStripMenuItem30";
            tpsToolStripMenuItem30.Size = new Size(126, 22);
            tpsToolStripMenuItem30.Text = "30 tps";
            tpsToolStripMenuItem30.Click += tpsToolStripMenuItem30_Click;
            // 
            // tpsToolStripMenuItem60
            // 
            tpsToolStripMenuItem60.Name = "tpsToolStripMenuItem60";
            tpsToolStripMenuItem60.Size = new Size(126, 22);
            tpsToolStripMenuItem60.Text = "60 tps";
            tpsToolStripMenuItem60.Click += tpsToolStripMenuItem60_Click;
            // 
            // tpsToolStripMenuItem120
            // 
            tpsToolStripMenuItem120.Name = "tpsToolStripMenuItem120";
            tpsToolStripMenuItem120.Size = new Size(126, 22);
            tpsToolStripMenuItem120.Text = "120 tps";
            tpsToolStripMenuItem120.Click += tpsToolStripMenuItem120_Click;
            // 
            // tpsToolStripMenuItemUnlimited
            // 
            tpsToolStripMenuItemUnlimited.Name = "tpsToolStripMenuItemUnlimited";
            tpsToolStripMenuItemUnlimited.Size = new Size(126, 22);
            tpsToolStripMenuItemUnlimited.Text = "Unlimited";
            tpsToolStripMenuItemUnlimited.Click += tpsToolStripMenuItemUnlimited_Click;
            // 
            // toolStripMenuItem8
            // 
            toolStripMenuItem8.Name = "toolStripMenuItem8";
            toolStripMenuItem8.Size = new Size(249, 6);
            // 
            // saveCacheAsCabinetFileToolStripMenuItem
            // 
            saveCacheAsCabinetFileToolStripMenuItem.Name = "saveCacheAsCabinetFileToolStripMenuItem";
            saveCacheAsCabinetFileToolStripMenuItem.Size = new Size(252, 22);
            saveCacheAsCabinetFileToolStripMenuItem.Text = "Save Cache as Cabinet File...";
            saveCacheAsCabinetFileToolStripMenuItem.Click += saveCacheAsCabinetFileToolStripMenuItem_Click;
            // 
            // restoreCacheFromCabinetFileToolStripMenuItem
            // 
            restoreCacheFromCabinetFileToolStripMenuItem.Name = "restoreCacheFromCabinetFileToolStripMenuItem";
            restoreCacheFromCabinetFileToolStripMenuItem.Size = new Size(252, 22);
            restoreCacheFromCabinetFileToolStripMenuItem.Text = "Restore Cache from Cabinet File...";
            restoreCacheFromCabinetFileToolStripMenuItem.Click += restoreCacheFromCabinetFileToolStripMenuItem_Click;
            // 
            // toolStripSeparator22
            // 
            toolStripSeparator22.Name = "toolStripSeparator22";
            toolStripSeparator22.Size = new Size(249, 6);
            // 
            // flushCacheToolStripMenuItem
            // 
            flushCacheToolStripMenuItem.Name = "flushCacheToolStripMenuItem";
            flushCacheToolStripMenuItem.Size = new Size(252, 22);
            flushCacheToolStripMenuItem.Text = "Flush Cache";
            flushCacheToolStripMenuItem.Click += helpFlush_Click;
            // 
            // toolStripSeparator18
            // 
            toolStripSeparator18.Name = "toolStripSeparator18";
            toolStripSeparator18.Size = new Size(249, 6);
            // 
            // showPerformanceDataToolStripMenuItem
            // 
            showPerformanceDataToolStripMenuItem.CheckOnClick = true;
            showPerformanceDataToolStripMenuItem.Name = "showPerformanceDataToolStripMenuItem";
            showPerformanceDataToolStripMenuItem.Size = new Size(252, 22);
            showPerformanceDataToolStripMenuItem.Text = "Show Performance Data";
            showPerformanceDataToolStripMenuItem.Click += showPerformanceDataToolStripMenuItem_Click;
            // 
            // toolStripSeparator19
            // 
            toolStripSeparator19.Name = "toolStripSeparator19";
            toolStripSeparator19.Size = new Size(249, 6);
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.MergeIndex = 1;
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(252, 22);
            toolStripMenuItem2.Text = "Master Controller";
            toolStripMenuItem2.Click += menuMasterControler_Click;
            // 
            // multiChanelCalibrationToolStripMenuItem
            // 
            multiChanelCalibrationToolStripMenuItem.Name = "multiChanelCalibrationToolStripMenuItem";
            multiChanelCalibrationToolStripMenuItem.Size = new Size(252, 22);
            multiChanelCalibrationToolStripMenuItem.Text = "Multi-Channel Calibration";
            multiChanelCalibrationToolStripMenuItem.Click += multiChanelCalibrationToolStripMenuItem_Click;
            // 
            // clientNodeListToolStripMenuItem
            // 
            clientNodeListToolStripMenuItem.Name = "clientNodeListToolStripMenuItem";
            clientNodeListToolStripMenuItem.Size = new Size(252, 22);
            clientNodeListToolStripMenuItem.Text = "Projector Server List";
            clientNodeListToolStripMenuItem.Click += clientNodeListToolStripMenuItem_Click;
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new Size(249, 6);
            // 
            // sendLayersToProjectorServersToolStripMenuItem
            // 
            sendLayersToProjectorServersToolStripMenuItem.Name = "sendLayersToProjectorServersToolStripMenuItem";
            sendLayersToProjectorServersToolStripMenuItem.Size = new Size(252, 22);
            sendLayersToProjectorServersToolStripMenuItem.Text = "Send Layers to Projector Servers";
            sendLayersToProjectorServersToolStripMenuItem.Click += sendLayersToProjectorServersToolStripMenuItem_Click;
            // 
            // mIDIControllerSetupToolStripMenuItem
            // 
            mIDIControllerSetupToolStripMenuItem.Name = "mIDIControllerSetupToolStripMenuItem";
            mIDIControllerSetupToolStripMenuItem.Size = new Size(206, 22);
            mIDIControllerSetupToolStripMenuItem.Text = "Controller Setup...";
            mIDIControllerSetupToolStripMenuItem.Click += mIDIControllerSetupToolStripMenuItem_Click;
            // 
            // xBoxControllerSetupToolStripMenuItem
            // 
            xBoxControllerSetupToolStripMenuItem.Name = "xBoxControllerSetupToolStripMenuItem";
            xBoxControllerSetupToolStripMenuItem.Size = new Size(206, 22);
            xBoxControllerSetupToolStripMenuItem.Text = "Xbox Controller Setup...";
            xBoxControllerSetupToolStripMenuItem.Click += xBoxControllerSetupToolStripMenuItem_Click;
            // 
            // remoteAccessControlToolStripMenuItem
            // 
            remoteAccessControlToolStripMenuItem.Name = "remoteAccessControlToolStripMenuItem";
            remoteAccessControlToolStripMenuItem.Size = new Size(206, 22);
            remoteAccessControlToolStripMenuItem.Text = "Remote Access Control...";
            remoteAccessControlToolStripMenuItem.Click += remoteAccessControlToolStripMenuItem_Click;
            // 
            // toolStripMenuItem14
            // 
            toolStripMenuItem14.Name = "toolStripMenuItem14";
            toolStripMenuItem14.Size = new Size(203, 6);
            // 
            // selectLanguageToolStripMenuItem
            // 
            selectLanguageToolStripMenuItem.Name = "selectLanguageToolStripMenuItem";
            selectLanguageToolStripMenuItem.Size = new Size(206, 22);
            selectLanguageToolStripMenuItem.Text = "Select Language...";
            selectLanguageToolStripMenuItem.Click += selectLanguageToolStripMenuItem_Click;
            // 
            // regionalDataCacheToolStripMenuItem
            // 
            regionalDataCacheToolStripMenuItem.Name = "regionalDataCacheToolStripMenuItem";
            regionalDataCacheToolStripMenuItem.Size = new Size(206, 22);
            regionalDataCacheToolStripMenuItem.Text = "Regional Data Cache...";
            regionalDataCacheToolStripMenuItem.Click += regionalDataCacheToolStripMenuItem_Click;
            // 
            // viewMenu
            // 
            viewMenu.Items.AddRange(new ToolStripItem[] {
            resetCameraMenuItem,
            showTouchControlsToolStripMenuItem,
            monochromeStyleToolStripMenuItem,
            allowUnconstrainedTiltToolStripMenuItem,
            toolStripSeparator9,
            startupToolStripMenuItem,
            toolStripMenuItem5,
            copyCurrentViewToClipboardToolStripMenuItem,
            copyShortCutToThisViewToClipboardToolStripMenuItem,
            saveCurrentViewImageToFileToolStripMenuItem,
            setCurrentViewAsWindowsDesktopBackgroundToolStripMenuItem,
            exportCurrentViewAsSTLFileFor3DPrintingToolStripMenuItem,
            toolStripSeparator21,
            screenBroadcastToolStripMenuItem,
            toolStripSeparator14,
            imageStackToolStripMenuItem,
            showLayerManagerToolStripMenuItem,
            toolStripSeparator20,
            stereoToolStripMenuItem,
            expermentalToolStripMenuItem,
            toggleFullScreenModeF11ToolStripMenuItem,
            multiSampleAntialiasingToolStripMenuItem,
            lockVerticalSyncToolStripMenuItem,
            targetFrameRateToolStripMenuItem});
            viewMenu.Name = "contextMenuStrip1";
            viewMenu.Size = new Size(341, 452);
            viewMenu.Closed += PopupClosed;
            viewMenu.Opening += viewMenu_Opening;
            viewMenu.PreviewKeyDown += exploreMenu_PreviewKeyDown;
            // 
            // resetCameraMenuItem
            // 
            resetCameraMenuItem.Name = "resetCameraMenuItem";
            resetCameraMenuItem.Size = new Size(340, 22);
            resetCameraMenuItem.Text = "Reset Camera";
            resetCameraMenuItem.Click += resetCameraToolStripMenuItem_Click;
            // 
            // showTouchControlsToolStripMenuItem
            // 
            showTouchControlsToolStripMenuItem.Name = "showTouchControlsToolStripMenuItem";
            showTouchControlsToolStripMenuItem.Size = new Size(340, 22);
            showTouchControlsToolStripMenuItem.Text = "Show On-Screen Controls";
            showTouchControlsToolStripMenuItem.Click += showTouchControlsToolStripMenuItem_Click;
            // 
            // monochromeStyleToolStripMenuItem
            // 
            monochromeStyleToolStripMenuItem.Name = "monochromeStyleToolStripMenuItem";
            monochromeStyleToolStripMenuItem.Size = new Size(340, 22);
            monochromeStyleToolStripMenuItem.Text = "Monochrome Style";
            monochromeStyleToolStripMenuItem.Click += monochromeStyleToolStripMenuItem_Click;
            // 
            // allowUnconstrainedTiltToolStripMenuItem
            // 
            allowUnconstrainedTiltToolStripMenuItem.Name = "allowUnconstrainedTiltToolStripMenuItem";
            allowUnconstrainedTiltToolStripMenuItem.Size = new Size(340, 22);
            allowUnconstrainedTiltToolStripMenuItem.Text = "Allow Unconstrained Tilt";
            allowUnconstrainedTiltToolStripMenuItem.Click += allowUnconstrainedTiltToolStripMenuItem_Click;
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new Size(337, 6);
            // 
            // startupToolStripMenuItem
            // 
            startupToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            earthToolStripMenuItem,
            planetToolStripMenuItem,
            skyToolStripMenuItem,
            panoramaToolStripMenuItem,
            solarSystemToolStripMenuItem,
            lastToolStripMenuItem,
            randomToolStripMenuItem});
            startupToolStripMenuItem.Name = "startupToolStripMenuItem";
            startupToolStripMenuItem.Size = new Size(340, 22);
            startupToolStripMenuItem.Text = "Startup Look At";
            startupToolStripMenuItem.DropDownOpening += startupToolStripMenuItem_DropDownOpening;
            // 
            // earthToolStripMenuItem
            // 
            earthToolStripMenuItem.Name = "earthToolStripMenuItem";
            earthToolStripMenuItem.Size = new Size(141, 22);
            earthToolStripMenuItem.Text = "Earth";
            earthToolStripMenuItem.Click += earthToolStripMenuItem_Click;
            // 
            // planetToolStripMenuItem
            // 
            planetToolStripMenuItem.Name = "planetToolStripMenuItem";
            planetToolStripMenuItem.Size = new Size(141, 22);
            planetToolStripMenuItem.Text = "Planet";
            planetToolStripMenuItem.Click += planetToolStripMenuItem_Click;
            // 
            // skyToolStripMenuItem
            // 
            skyToolStripMenuItem.Name = "skyToolStripMenuItem";
            skyToolStripMenuItem.Size = new Size(141, 22);
            skyToolStripMenuItem.Text = "Sky";
            skyToolStripMenuItem.Click += skyToolStripMenuItem_Click;
            // 
            // panoramaToolStripMenuItem
            // 
            panoramaToolStripMenuItem.Name = "panoramaToolStripMenuItem";
            panoramaToolStripMenuItem.Size = new Size(141, 22);
            panoramaToolStripMenuItem.Text = "Panorama";
            panoramaToolStripMenuItem.Click += panoramaToolStripMenuItem_Click;
            // 
            // solarSystemToolStripMenuItem
            // 
            solarSystemToolStripMenuItem.Name = "solarSystemToolStripMenuItem";
            solarSystemToolStripMenuItem.Size = new Size(141, 22);
            solarSystemToolStripMenuItem.Text = "Solar System";
            solarSystemToolStripMenuItem.Click += solarSystemToolStripMenuItem_Click;
            // 
            // lastToolStripMenuItem
            // 
            lastToolStripMenuItem.Name = "lastToolStripMenuItem";
            lastToolStripMenuItem.Size = new Size(141, 22);
            lastToolStripMenuItem.Text = "Last";
            lastToolStripMenuItem.Click += lastToolStripMenuItem_Click;
            // 
            // randomToolStripMenuItem
            // 
            randomToolStripMenuItem.Name = "randomToolStripMenuItem";
            randomToolStripMenuItem.Size = new Size(141, 22);
            randomToolStripMenuItem.Text = "Random";
            randomToolStripMenuItem.Click += randomToolStripMenuItem_Click;
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new Size(337, 6);
            // 
            // copyCurrentViewToClipboardToolStripMenuItem
            // 
            copyCurrentViewToClipboardToolStripMenuItem.Name = "copyCurrentViewToClipboardToolStripMenuItem";
            copyCurrentViewToClipboardToolStripMenuItem.Size = new Size(340, 22);
            copyCurrentViewToClipboardToolStripMenuItem.Text = "Copy Current View Image";
            copyCurrentViewToClipboardToolStripMenuItem.Click += copyCurrentViewToClipboardToolStripMenuItem_Click;
            // 
            // copyShortCutToThisViewToClipboardToolStripMenuItem
            // 
            copyShortCutToThisViewToClipboardToolStripMenuItem.Name = "copyShortCutToThisViewToClipboardToolStripMenuItem";
            copyShortCutToThisViewToClipboardToolStripMenuItem.Size = new Size(340, 22);
            copyShortCutToThisViewToClipboardToolStripMenuItem.Text = "Copy Shortcut to this View";
            copyShortCutToThisViewToClipboardToolStripMenuItem.Click += copyShortcutMenuItem_Click;
            // 
            // saveCurrentViewImageToFileToolStripMenuItem
            // 
            saveCurrentViewImageToFileToolStripMenuItem.Name = "saveCurrentViewImageToFileToolStripMenuItem";
            saveCurrentViewImageToFileToolStripMenuItem.Size = new Size(340, 22);
            saveCurrentViewImageToFileToolStripMenuItem.Text = "Save Current View Image to File...";
            saveCurrentViewImageToFileToolStripMenuItem.Click += saveCurrentViewImageToFileToolStripMenuItem_Click;
            // 
            // setCurrentViewAsWindowsDesktopBackgroundToolStripMenuItem
            // 
            setCurrentViewAsWindowsDesktopBackgroundToolStripMenuItem.Name = "setCurrentViewAsWindowsDesktopBackgroundToolStripMenuItem";
            setCurrentViewAsWindowsDesktopBackgroundToolStripMenuItem.Size = new Size(340, 22);
            setCurrentViewAsWindowsDesktopBackgroundToolStripMenuItem.Text = "Set Current View as Windows Desktop Background";
            setCurrentViewAsWindowsDesktopBackgroundToolStripMenuItem.Click += setCurrentViewAsWindowsDesktopBackgroundToolStripMenuItem_Click;
            // 
            // exportCurrentViewAsSTLFileFor3DPrintingToolStripMenuItem
            // 
            exportCurrentViewAsSTLFileFor3DPrintingToolStripMenuItem.Name = "exportCurrentViewAsSTLFileFor3DPrintingToolStripMenuItem";
            exportCurrentViewAsSTLFileFor3DPrintingToolStripMenuItem.Size = new Size(340, 22);
            exportCurrentViewAsSTLFileFor3DPrintingToolStripMenuItem.Text = "Export Current View as STL File for 3D Printing...";
            exportCurrentViewAsSTLFileFor3DPrintingToolStripMenuItem.Click += exportCurrentViewAsSTLFileFor3DPrintingToolStripMenuItem_Click;
            // 
            // toolStripSeparator21
            // 
            toolStripSeparator21.Name = "toolStripSeparator21";
            toolStripSeparator21.Size = new Size(337, 6);
            // 
            // screenBroadcastToolStripMenuItem
            // 
            screenBroadcastToolStripMenuItem.Name = "screenBroadcastToolStripMenuItem";
            screenBroadcastToolStripMenuItem.Size = new Size(340, 22);
            screenBroadcastToolStripMenuItem.Text = "Screen Broadcast...";
            screenBroadcastToolStripMenuItem.Click += screenBroadcastToolStripMenuItem_Click;
            // 
            // toolStripSeparator14
            // 
            toolStripSeparator14.Name = "toolStripSeparator14";
            toolStripSeparator14.Size = new Size(337, 6);
            toolStripSeparator14.Visible = false;
            // 
            // imageStackToolStripMenuItem
            // 
            imageStackToolStripMenuItem.Name = "imageStackToolStripMenuItem";
            imageStackToolStripMenuItem.Size = new Size(340, 22);
            imageStackToolStripMenuItem.Text = "Image Stack";
            imageStackToolStripMenuItem.Click += imageStackToolStripMenuItem_Click;
            // 
            // showLayerManagerToolStripMenuItem
            // 
            showLayerManagerToolStripMenuItem.Name = "showLayerManagerToolStripMenuItem";
            showLayerManagerToolStripMenuItem.Size = new Size(340, 22);
            showLayerManagerToolStripMenuItem.Text = "Show Layer Manager";
            showLayerManagerToolStripMenuItem.Click += showLayerManagerToolStripMenuItem_Click;
            // 
            // toolStripSeparator20
            // 
            toolStripSeparator20.Name = "toolStripSeparator20";
            toolStripSeparator20.Size = new Size(337, 6);
            // 
            // stereoToolStripMenuItem
            // 
            stereoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            enabledToolStripMenuItem,
            anaglyphToolStripMenuItem,
            anaglyphYellowBlueToolStripMenuItem,
            sideBySideProjectionToolStripMenuItem,
            sideBySideCrossEyedToolStripMenuItem,
            alternatingLinesOddToolStripMenuItem,
            alternatingLinesEvenToolStripMenuItem,
            oculusRiftToolStripMenuItem});
            stereoToolStripMenuItem.Name = "stereoToolStripMenuItem";
            stereoToolStripMenuItem.Size = new Size(340, 22);
            stereoToolStripMenuItem.Text = "Stereo";
            stereoToolStripMenuItem.DropDownOpening += stereoToolStripMenuItem_DropDownOpening;
            // 
            // enabledToolStripMenuItem
            // 
            enabledToolStripMenuItem.Name = "enabledToolStripMenuItem";
            enabledToolStripMenuItem.Size = new Size(199, 22);
            enabledToolStripMenuItem.Text = "Disabled";
            enabledToolStripMenuItem.Click += enabledToolStripMenuItem_Click;
            // 
            // anaglyphToolStripMenuItem
            // 
            anaglyphToolStripMenuItem.Name = "anaglyphToolStripMenuItem";
            anaglyphToolStripMenuItem.Size = new Size(199, 22);
            anaglyphToolStripMenuItem.Text = "Anaglyph (Red-Cyan)";
            anaglyphToolStripMenuItem.Click += anaglyphToolStripMenuItem_Click;
            // 
            // anaglyphYellowBlueToolStripMenuItem
            // 
            anaglyphYellowBlueToolStripMenuItem.Name = "anaglyphYellowBlueToolStripMenuItem";
            anaglyphYellowBlueToolStripMenuItem.Size = new Size(199, 22);
            anaglyphYellowBlueToolStripMenuItem.Text = "Anaglyph (Yellow-Blue)";
            anaglyphYellowBlueToolStripMenuItem.Click += anaglyphYellowBlueToolStripMenuItem_Click;
            // 
            // sideBySideProjectionToolStripMenuItem
            // 
            sideBySideProjectionToolStripMenuItem.Name = "sideBySideProjectionToolStripMenuItem";
            sideBySideProjectionToolStripMenuItem.Size = new Size(199, 22);
            sideBySideProjectionToolStripMenuItem.Text = "Side by Side Projection";
            sideBySideProjectionToolStripMenuItem.Click += sideBySideProjectionToolStripMenuItem_Click;
            // 
            // sideBySideCrossEyedToolStripMenuItem
            // 
            sideBySideCrossEyedToolStripMenuItem.Name = "sideBySideCrossEyedToolStripMenuItem";
            sideBySideCrossEyedToolStripMenuItem.Size = new Size(199, 22);
            sideBySideCrossEyedToolStripMenuItem.Text = "Side by Side Cross-Eyed";
            sideBySideCrossEyedToolStripMenuItem.Click += sideBySideCrossEyedToolStripMenuItem_Click;
            // 
            // alternatingLinesOddToolStripMenuItem
            // 
            alternatingLinesOddToolStripMenuItem.Name = "alternatingLinesOddToolStripMenuItem";
            alternatingLinesOddToolStripMenuItem.Size = new Size(199, 22);
            alternatingLinesOddToolStripMenuItem.Text = "Alternating Lines Odd";
            alternatingLinesOddToolStripMenuItem.Click += alternatingLinesOddToolStripMenuItem_Click;
            // 
            // alternatingLinesEvenToolStripMenuItem
            // 
            alternatingLinesEvenToolStripMenuItem.Name = "alternatingLinesEvenToolStripMenuItem";
            alternatingLinesEvenToolStripMenuItem.Size = new Size(199, 22);
            alternatingLinesEvenToolStripMenuItem.Text = "Alternating Lines Even";
            alternatingLinesEvenToolStripMenuItem.Click += alternatingLinesEvenToolStripMenuItem_Click;
            // 
            // oculusRiftToolStripMenuItem
            // 
            oculusRiftToolStripMenuItem.Name = "oculusRiftToolStripMenuItem";
            oculusRiftToolStripMenuItem.Size = new Size(199, 22);
            oculusRiftToolStripMenuItem.Text = "Oculus Rift";
            oculusRiftToolStripMenuItem.Click += oculusRiftToolStripMenuItem_Click;
            // 
            // expermentalToolStripMenuItem
            // 
            expermentalToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            fullDomeToolStripMenuItem,
            newFullDomeViewInstanceToolStripMenuItem,
            toolStripMenuItem15,
            domeSetupToolStripMenuItem,
            listenUpBoysToolStripMenuItem,
            toolStripMenuItem11,
            detachMainViewToSecondMonitor,
            detachMainViewToThirdMonitorToolStripMenuItem,
            toolStripMenuItem10,
            fullDomePreviewToolStripMenuItem});
            expermentalToolStripMenuItem.Name = "expermentalToolStripMenuItem";
            expermentalToolStripMenuItem.Size = new Size(340, 22);
            expermentalToolStripMenuItem.Text = "Single Channel Full Dome";
            // 
            // fullDomeToolStripMenuItem
            // 
            fullDomeToolStripMenuItem.Name = "fullDomeToolStripMenuItem";
            fullDomeToolStripMenuItem.Size = new Size(271, 22);
            fullDomeToolStripMenuItem.Text = "Dome View";
            fullDomeToolStripMenuItem.Click += fullDomeToolStripMenuItem_Click;
            // 
            // newFullDomeViewInstanceToolStripMenuItem
            // 
            newFullDomeViewInstanceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            monitorOneToolStripMenuItem,
            monitorTwoToolStripMenuItem,
            monitorThreeToolStripMenuItem,
            monitorFourToolStripMenuItem,
            monitorFiveToolStripMenuItem,
            monitorSixToolStripMenuItem,
            monitorSevenToolStripMenuItem,
            monitorEightToolStripMenuItem});
            newFullDomeViewInstanceToolStripMenuItem.Name = "newFullDomeViewInstanceToolStripMenuItem";
            newFullDomeViewInstanceToolStripMenuItem.Size = new Size(271, 22);
            newFullDomeViewInstanceToolStripMenuItem.Text = "New Full Dome View Instance";
            newFullDomeViewInstanceToolStripMenuItem.DropDownOpening += newFullDomeViewInstanceToolStripMenuItem_DropDownOpening;
            // 
            // monitorOneToolStripMenuItem
            // 
            monitorOneToolStripMenuItem.Name = "monitorOneToolStripMenuItem";
            monitorOneToolStripMenuItem.Size = new Size(151, 22);
            monitorOneToolStripMenuItem.Tag = "1";
            monitorOneToolStripMenuItem.Text = "Monitor One";
            monitorOneToolStripMenuItem.Click += CreateDomeInstanceToolStripMenuItem_Click;
            // 
            // monitorTwoToolStripMenuItem
            // 
            monitorTwoToolStripMenuItem.Name = "monitorTwoToolStripMenuItem";
            monitorTwoToolStripMenuItem.Size = new Size(151, 22);
            monitorTwoToolStripMenuItem.Tag = "2";
            monitorTwoToolStripMenuItem.Text = "Monitor Two";
            monitorTwoToolStripMenuItem.Click += CreateDomeInstanceToolStripMenuItem_Click;
            // 
            // monitorThreeToolStripMenuItem
            // 
            monitorThreeToolStripMenuItem.Name = "monitorThreeToolStripMenuItem";
            monitorThreeToolStripMenuItem.Size = new Size(151, 22);
            monitorThreeToolStripMenuItem.Tag = "3";
            monitorThreeToolStripMenuItem.Text = "Monitor Three";
            monitorThreeToolStripMenuItem.Click += CreateDomeInstanceToolStripMenuItem_Click;
            // 
            // monitorFourToolStripMenuItem
            // 
            monitorFourToolStripMenuItem.Name = "monitorFourToolStripMenuItem";
            monitorFourToolStripMenuItem.Size = new Size(151, 22);
            monitorFourToolStripMenuItem.Tag = "4";
            monitorFourToolStripMenuItem.Text = "Monitor Four";
            monitorFourToolStripMenuItem.Click += CreateDomeInstanceToolStripMenuItem_Click;
            // 
            // monitorFiveToolStripMenuItem
            // 
            monitorFiveToolStripMenuItem.Name = "monitorFiveToolStripMenuItem";
            monitorFiveToolStripMenuItem.Size = new Size(151, 22);
            monitorFiveToolStripMenuItem.Tag = "5";
            monitorFiveToolStripMenuItem.Text = "Monitor Five";
            monitorFiveToolStripMenuItem.Click += CreateDomeInstanceToolStripMenuItem_Click;
            // 
            // monitorSixToolStripMenuItem
            // 
            monitorSixToolStripMenuItem.Name = "monitorSixToolStripMenuItem";
            monitorSixToolStripMenuItem.Size = new Size(151, 22);
            monitorSixToolStripMenuItem.Tag = "6";
            monitorSixToolStripMenuItem.Text = "Monitor Six";
            monitorSixToolStripMenuItem.Click += CreateDomeInstanceToolStripMenuItem_Click;
            // 
            // monitorSevenToolStripMenuItem
            // 
            monitorSevenToolStripMenuItem.Name = "monitorSevenToolStripMenuItem";
            monitorSevenToolStripMenuItem.Size = new Size(151, 22);
            monitorSevenToolStripMenuItem.Tag = "7";
            monitorSevenToolStripMenuItem.Text = "Monitor Seven";
            monitorSevenToolStripMenuItem.Click += CreateDomeInstanceToolStripMenuItem_Click;
            // 
            // monitorEightToolStripMenuItem
            // 
            monitorEightToolStripMenuItem.Name = "monitorEightToolStripMenuItem";
            monitorEightToolStripMenuItem.Size = new Size(151, 22);
            monitorEightToolStripMenuItem.Tag = "8";
            monitorEightToolStripMenuItem.Text = "Monitor Eight";
            monitorEightToolStripMenuItem.Click += CreateDomeInstanceToolStripMenuItem_Click;
            // 
            // toolStripMenuItem15
            // 
            toolStripMenuItem15.Name = "toolStripMenuItem15";
            toolStripMenuItem15.Size = new Size(268, 6);
            // 
            // domeSetupToolStripMenuItem
            // 
            domeSetupToolStripMenuItem.Name = "domeSetupToolStripMenuItem";
            domeSetupToolStripMenuItem.Size = new Size(271, 22);
            domeSetupToolStripMenuItem.Text = "Dome Setup";
            domeSetupToolStripMenuItem.Click += domeSetupToolStripMenuItem_Click;
            // 
            // listenUpBoysToolStripMenuItem
            // 
            listenUpBoysToolStripMenuItem.Name = "listenUpBoysToolStripMenuItem";
            listenUpBoysToolStripMenuItem.Size = new Size(271, 22);
            listenUpBoysToolStripMenuItem.Text = "Start Listener";
            listenUpBoysToolStripMenuItem.Click += listenUpBoysToolStripMenuItem_Click;
            // 
            // toolStripMenuItem11
            // 
            toolStripMenuItem11.Name = "toolStripMenuItem11";
            toolStripMenuItem11.Size = new Size(268, 6);
            // 
            // detachMainViewToSecondMonitor
            // 
            detachMainViewToSecondMonitor.Name = "detachMainViewToSecondMonitor";
            detachMainViewToSecondMonitor.Size = new Size(271, 22);
            detachMainViewToSecondMonitor.Text = "Detach Main View to Second Monitor";
            detachMainViewToSecondMonitor.Click += detatchMainViewMenuItem_Click;
            // 
            // detachMainViewToThirdMonitorToolStripMenuItem
            // 
            detachMainViewToThirdMonitorToolStripMenuItem.Name = "detachMainViewToThirdMonitorToolStripMenuItem";
            detachMainViewToThirdMonitorToolStripMenuItem.Size = new Size(271, 22);
            detachMainViewToThirdMonitorToolStripMenuItem.Text = "Detach Main View to Third Monitor";
            detachMainViewToThirdMonitorToolStripMenuItem.Click += detachMainViewToThirdMonitorToolStripMenuItem_Click;
            // 
            // toolStripMenuItem10
            // 
            toolStripMenuItem10.Name = "toolStripMenuItem10";
            toolStripMenuItem10.Size = new Size(268, 6);
            // 
            // fullDomePreviewToolStripMenuItem
            // 
            fullDomePreviewToolStripMenuItem.Name = "fullDomePreviewToolStripMenuItem";
            fullDomePreviewToolStripMenuItem.Size = new Size(271, 22);
            fullDomePreviewToolStripMenuItem.Text = "Full Dome Preview";
            fullDomePreviewToolStripMenuItem.Click += fullDomePreviewToolStripMenuItem_Click;
            // 
            // toggleFullScreenModeF11ToolStripMenuItem
            // 
            toggleFullScreenModeF11ToolStripMenuItem.Name = "toggleFullScreenModeF11ToolStripMenuItem";
            toggleFullScreenModeF11ToolStripMenuItem.ShortcutKeyDisplayString = "F11";
            toggleFullScreenModeF11ToolStripMenuItem.ShortcutKeys = Keys.F11;
            toggleFullScreenModeF11ToolStripMenuItem.Size = new Size(340, 22);
            toggleFullScreenModeF11ToolStripMenuItem.Text = "Toggle Full Screen Mode";
            toggleFullScreenModeF11ToolStripMenuItem.Click += toggleFullScreenModeF11ToolStripMenuItem_Click;
            // 
            // multiSampleAntialiasingToolStripMenuItem
            // 
            multiSampleAntialiasingToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            noneToolStripMenuItem,
            fourSamplesToolStripMenuItem,
            eightSamplesToolStripMenuItem});
            multiSampleAntialiasingToolStripMenuItem.Name = "multiSampleAntialiasingToolStripMenuItem";
            multiSampleAntialiasingToolStripMenuItem.Size = new Size(340, 22);
            multiSampleAntialiasingToolStripMenuItem.Text = "Multi-Sample Antialiasing";
            multiSampleAntialiasingToolStripMenuItem.DropDownOpening += multiSampleAntialiasingToolStripMenuItem_DropDownOpening;
            // 
            // noneToolStripMenuItem
            // 
            noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            noneToolStripMenuItem.Size = new Size(148, 22);
            noneToolStripMenuItem.Text = "None";
            noneToolStripMenuItem.Click += noneToolStripMenuItem_Click;
            // 
            // fourSamplesToolStripMenuItem
            // 
            fourSamplesToolStripMenuItem.Name = "fourSamplesToolStripMenuItem";
            fourSamplesToolStripMenuItem.Size = new Size(148, 22);
            fourSamplesToolStripMenuItem.Text = "Four Samples";
            fourSamplesToolStripMenuItem.Click += fourSamplesToolStripMenuItem_Click;
            // 
            // eightSamplesToolStripMenuItem
            // 
            eightSamplesToolStripMenuItem.Name = "eightSamplesToolStripMenuItem";
            eightSamplesToolStripMenuItem.Size = new Size(148, 22);
            eightSamplesToolStripMenuItem.Text = "Eight Samples";
            eightSamplesToolStripMenuItem.Click += eightSamplesToolStripMenuItem_Click;
            // 
            // lockVerticalSyncToolStripMenuItem
            // 
            lockVerticalSyncToolStripMenuItem.Name = "lockVerticalSyncToolStripMenuItem";
            lockVerticalSyncToolStripMenuItem.Size = new Size(340, 22);
            lockVerticalSyncToolStripMenuItem.Text = "Lock Vertical Sync";
            lockVerticalSyncToolStripMenuItem.Click += lockVerticalSyncToolStripMenuItem_Click;
            // 
            // targetFrameRateToolStripMenuItem
            // 
            targetFrameRateToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            fpsToolStripMenuItemUnlimited,
            fPSToolStripMenuItem60,
            fPSToolStripMenuItem30,
            fPSToolStripMenuItem24});
            targetFrameRateToolStripMenuItem.Name = "targetFrameRateToolStripMenuItem";
            targetFrameRateToolStripMenuItem.Size = new Size(340, 22);
            targetFrameRateToolStripMenuItem.Text = "Target Frame Rate";
            targetFrameRateToolStripMenuItem.DropDownOpening += targetFrameRateToolStripMenuItem_DropDownOpening;
            // 
            // fpsToolStripMenuItemUnlimited
            // 
            fpsToolStripMenuItemUnlimited.Name = "fpsToolStripMenuItemUnlimited";
            fpsToolStripMenuItemUnlimited.Size = new Size(126, 22);
            fpsToolStripMenuItemUnlimited.Text = "Unlimited";
            fpsToolStripMenuItemUnlimited.Click += fpsToolStripMenuItemUnlimited_Click;
            // 
            // fPSToolStripMenuItem60
            // 
            fPSToolStripMenuItem60.Name = "fPSToolStripMenuItem60";
            fPSToolStripMenuItem60.Size = new Size(126, 22);
            fPSToolStripMenuItem60.Text = "60 FPS";
            fPSToolStripMenuItem60.Click += fPSToolStripMenuItem60_Click;
            // 
            // fPSToolStripMenuItem30
            // 
            fPSToolStripMenuItem30.Name = "fPSToolStripMenuItem30";
            fPSToolStripMenuItem30.Size = new Size(126, 22);
            fPSToolStripMenuItem30.Text = "30 FPS";
            fPSToolStripMenuItem30.Click += fPSToolStripMenuItem30_Click;
            // 
            // fPSToolStripMenuItem24
            // 
            fPSToolStripMenuItem24.Name = "fPSToolStripMenuItem24";
            fPSToolStripMenuItem24.Size = new Size(126, 22);
            fPSToolStripMenuItem24.Text = "24 FPS";
            fPSToolStripMenuItem24.Click += fPSToolStripMenuItem24_Click;
            // 
            // StatupTimer
            // 
            StatupTimer.Enabled = true;
            StatupTimer.Interval = 1000;
            StatupTimer.Tick += StatupTimer_Tick;
            // 
            // SlideAdvanceTimer
            // 
            SlideAdvanceTimer.Interval = 10000;
            SlideAdvanceTimer.Tick += SlideAdvanceTimer_Tick;
            // 
            // TourEndCheck
            // 
            TourEndCheck.Enabled = true;
            TourEndCheck.Interval = 1000;
            TourEndCheck.Tick += TourEndCheck_Tick;
            // 
            // autoSaveTimer
            // 
            autoSaveTimer.Enabled = true;
            autoSaveTimer.Interval = 60000;
            autoSaveTimer.Tick += autoSaveTimer_Tick;
            // 
            // DeviceHeartbeat
            // 
            DeviceHeartbeat.Enabled = true;
            DeviceHeartbeat.Tick += DeviceHeartbeat_Tick;
            // 
            // kioskTitleBar
            // 
            kioskTitleBar.BackgroundImage = ((Image)(resources.GetObject("kioskTitleBar.BackgroundImage")));
            kioskTitleBar.Dock = DockStyle.Top;
            kioskTitleBar.Location = new Point(0, 34);
            kioskTitleBar.Name = "kioskTitleBar";
            kioskTitleBar.Size = new Size(863, 34);
            kioskTitleBar.TabIndex = 9;
            kioskTitleBar.Visible = false;
            // 
            // renderWindow
            // 
            renderWindow.BackColor = Color.Black;
            renderWindow.Dock = DockStyle.Fill;
            renderWindow.Location = new Point(0, 34);
            renderWindow.Name = "renderWindow";
            renderWindow.Size = new Size(863, 328);
            renderWindow.TabIndex = 8;
            renderWindow.TabStop = false;
            renderWindow.Click += renderWindow_Click;
            renderWindow.Paint += renderWindow_Paint;
            renderWindow.MouseClick += renderWindow_MouseClick;
            renderWindow.MouseDoubleClick += renderWindow_MouseDoubleClick;
            renderWindow.MouseDown += renderWindow_MouseDown;
            renderWindow.MouseEnter += renderWindow_MouseEnter;
            renderWindow.MouseLeave += renderWindow_MouseLeave;
            renderWindow.MouseHover += renderWindow_MouseHover;
            renderWindow.MouseMove += renderWindow_MouseMove;
            renderWindow.MouseUp += renderWindow_MouseUp;
            renderWindow.Resize += renderWindow_Resize;
            // 
            // menuTabs
            // 
            menuTabs.BackColor = Color.Black;
            menuTabs.BackgroundImage = ((Image)(resources.GetObject("menuTabs.BackgroundImage")));
            menuTabs.CurrentTour = null;
            menuTabs.Dock = DockStyle.Top;
            menuTabs.IsVisible = true;
            menuTabs.Location = new Point(0, 0);
            menuTabs.Name = "menuTabs";
            menuTabs.SelectedTabIndex = 0;
            menuTabs.Size = new Size(863, 34);
            menuTabs.StartX = 0;
            menuTabs.TabIndex = 4;
            menuTabs.TabClicked += menuTabs_TabClicked;
            menuTabs.MenuClicked += menuTabs_MenuClicked;
            menuTabs.ControlEvent += menuTabs_ControlEvent;
            menuTabs.Load += menuTabs_Load;
            // 
            // Earth3d
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = Color.Black;
            ClientSize = new Size(863, 362);
            Controls.Add(kioskTitleBar);
            Controls.Add(renderWindow);
            Controls.Add(menuTabs);
            DoubleBuffered = true;
            Icon = ((Icon)(resources.GetObject("$this.Icon")));
            KeyPreview = true;
            MinimumSize = new Size(800, 400);
            Name = "Earth3d";
            Text = "Microsoft WorldWide Telescope";
            FormClosing += Earth3d_FormClosing;
            FormClosed += Earth3d_FormClosed;
            Load += Earth3d_Load;
            Shown += Earth3d_Shown;
            ResizeBegin += Earth3d_ResizeBegin;
            ResizeEnd += Earth3d_ResizeEnd;
            Click += Earth3d_Click;
            Paint += Earth3d_Paint;
            KeyDown += MainWndow_KeyDown;
            KeyUp += Earth3d_KeyUp;
            MouseDoubleClick += Earth3d_MouseDoubleClick;
            MouseDown += MainWndow_MouseDown;
            MouseEnter += Earth3d_MouseEnter;
            MouseLeave += Earth3d_MouseLeave;
            MouseMove += MainWndow_MouseMove;
            MouseUp += MainWndow_MouseUp;
            MouseWheel += MainWndow_MouseWheel;
            Move += Earth3d_Move;
            Resize += Earth3d_Resize;
            contextMenu.ResumeLayout(false);
            communitiesMenu.ResumeLayout(false);
            searchMenu.ResumeLayout(false);
            toursMenu.ResumeLayout(false);
            telescopeMenu.ResumeLayout(false);
            exploreMenu.ResumeLayout(false);
            settingsMenu.ResumeLayout(false);
            viewMenu.ResumeLayout(false);
            ResumeLayout(false);

        }


        RenderHost renderHost;
        public void FreeFloatRenderWindow(int targetMonitor)
        {

            if (renderHost == null)
            {
                Controls.Remove(renderWindow);
                renderHost = new RenderHost();
                renderHost.Show();
                renderHost.Controls.Add(renderWindow);
                var id = 0;
                if (Screen.FromControl(this).DeviceName == Screen.AllScreens[0].DeviceName)
                {
                    id = targetMonitor;
                }

                if (id == 0 && targetMonitor > 1)
                {
                    if (Screen.FromControl(this).DeviceName == Screen.AllScreens[1].DeviceName)
                    {
                        id = targetMonitor;
                    }
                }


                UiTools.ShowFullScreen(renderHost, false, id);
                RenderContext11.Resize(renderWindow);
            }
        }

        public void FreeFloatRenderWindow(string deviceName)
        {

            if (renderHost == null)
            {
                Controls.Remove(renderWindow);
                var id = 0;
                var foundId = -1;
                foreach (var screen in Screen.AllScreens)
                {
                    if (deviceName.StartsWith(screen.DeviceName))
                    {
                        foundId = id;
                        break;
                    }
                    id++;
                }
                if (foundId > -1)
                {
                    renderHost = new RenderHost();
                    renderHost.Show();
                    renderHost.Controls.Add(renderWindow);
                    UiTools.ShowFullScreen(renderHost, false, id);
                    RenderContext11.Resize(renderWindow);
                }
            }
        }

        public void AttachRenderWindow()
        {
            if (renderHost != null)
            {
                renderHost.Controls.Remove(renderWindow);
                Controls.Add(renderWindow);
                renderHost.Hide();
                renderHost.Close();
                renderHost = null;
            }
        }

        private void detatchMainViewMenuItem_Click(object sender, EventArgs e)
        {
            if (detachMainViewToSecondMonitor.Checked || detachMainViewToThirdMonitorToolStripMenuItem.Checked)
            {
                AttachRenderWindow();
                detachMainViewToThirdMonitorToolStripMenuItem.Checked = false;
                detachMainViewToSecondMonitor.Checked = false;
            }
            else
            {
                FreeFloatRenderWindow(1);
                detachMainViewToThirdMonitorToolStripMenuItem.Checked = false;
                detachMainViewToSecondMonitor.Checked = true;
            }
        }

        private void detachMainViewToThirdMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (detachMainViewToSecondMonitor.Checked || detachMainViewToThirdMonitorToolStripMenuItem.Checked)
            {
                AttachRenderWindow();
                detachMainViewToThirdMonitorToolStripMenuItem.Checked = false;
            }
            else
            {
                FreeFloatRenderWindow(2);
                detachMainViewToThirdMonitorToolStripMenuItem.Checked = true;
                detachMainViewToSecondMonitor.Checked = false;
            }
        }

        SkyLabel label;
        public KmlLabels KmlMarkers = null;
        public bool InitializeGraphics()
        {
            if (ReadyToRender)
            {
                return true;
            }

            try
            {
                if (Properties.Settings.Default.MultiSampling == 0)
                {
                    Properties.Settings.Default.MultiSampling = 1;
                }
                RenderContext11.MultiSampleCount = Math.Max(1, Properties.Settings.Default.MultiSampling);
                RenderContext11 = new RenderContext11(renderWindow);

                ReadyToRender = true;
                pause = false;
                return true;
            }
            catch
            {
                // Catch any errors and return a failure
                return false;
            }
        }


        PositionColorTexturedVertexBuffer11[] domeVertexBuffer;
        IndexBuffer11[] domeIndexBuffer;
        int domeVertexCount;
 
        int domeTriangleCount;


        void CreateDomeFaceVertexBuffer(int face)
        {
            var domeSubX = 50;
            var domeSubY = 50;

            CleanupDomeVertexBuffer(face);

            var fea = Math.Min(250, Properties.Settings.Default.FisheyeAngle) / 180;
            var fa = Math.Min(250, Properties.Settings.Default.FisheyeAngle);

            domeIndexBuffer[face] = new IndexBuffer11(typeof(short), (domeSubX * domeSubY * 6), RenderContext11.PrepDevice);
            domeVertexBuffer[face] = new PositionColorTexturedVertexBuffer11(((domeSubX + 1) * (domeSubY + 1)), RenderContext11.PrepDevice);

            domeVertexCount = domeSubX * domeSubY * 6;


            var index = 0;

            var vb = domeVertexBuffer[face];

            var verts = (PositionColoredTextured[])vb.Lock(0, 0);
            int x1, y1;



            var topLeft = new Vector3d();
            var topRight = new Vector3d();
            var bottomLeft = new Vector3d();
            var bottomRight = new Vector3d();

            var faceType = (RenderTypes)face;

            switch (faceType)
            {
                case RenderTypes.DomeFront:
                    topLeft = new Vector3d(-1, 1, 1);
                    topRight = new Vector3d(1, 1, 1);
                    bottomLeft = new Vector3d(-1, -1, 1);
                    bottomRight = new Vector3d(1, -1, 1);
                    break;
                case RenderTypes.DomeRight:
                    topLeft = new Vector3d(1, 1, 1);
                    topRight = new Vector3d(1, 1, -1);
                    bottomLeft = new Vector3d(1, -1, 1);
                    bottomRight = new Vector3d(1, -1, -1);
                    break;
                case RenderTypes.DomeUp:
                    topLeft = new Vector3d(-1, 1, -1);
                    topRight = new Vector3d(1, 1, -1);
                    bottomLeft = new Vector3d(-1, 1, 1);
                    bottomRight = new Vector3d(1, 1, 1);
                    break;
                case RenderTypes.DomeLeft:
                    topLeft = new Vector3d(-1, 1, -1);
                    topRight = new Vector3d(-1, 1, 1);
                    bottomLeft = new Vector3d(-1, -1, -1);
                    bottomRight = new Vector3d(-1, -1, 1);
                    break;
                case RenderTypes.DomeBack:
                    topLeft = new Vector3d(1, 1, -1);
                    topRight = new Vector3d(-1, 1, -1);
                    bottomLeft = new Vector3d(1, -1, -1);
                    bottomRight = new Vector3d(-1, -1, -1);
                    break;
            }

            double textureStepX = 1.0f / domeSubX;
            double textureStepY = 1.0f / domeSubY;
            for (y1 = 0; y1 <= domeSubY; y1++)
            {
                double tv;
                if (y1 != domeSubY)
                {
                    tv = textureStepY * y1;
                }
                else
                {
                    tv = 1;
                }

                for (x1 = 0; x1 <= domeSubX; x1++)
                {
                    double tu;
                    if (x1 != domeSubX)
                    {
                        tu = textureStepX * x1;
                    }
                    else
                    {
                        tu = 1;
                    }

                    var top = Vector3d.Lerp(topLeft, topRight, tu);
                    var bottom = Vector3d.Lerp(bottomLeft, bottomRight, tu);
                    var net = Vector3d.Lerp(top, bottom, tv);
                    net.Normalize();
                    var netNet = Coordinates.CartesianToSpherical2(net.Vector3);
                    var dist = (180 - (netNet.Lat + 90)) / (180 * fea);
                    dist = Math.Min(.5, dist);

                    var x = Math.Sin((netNet.Lng + 90) / 180 * Math.PI) * dist;
                    var y = Math.Cos((netNet.Lng + 90) / 180 * Math.PI) * dist;

                    index = y1 * (domeSubX + 1) + x1;
                    verts[index].Position = new Vector4((float)x, (float)y, .9f, 1);
                    verts[index].Tu = (float)tu;
                    verts[index].Tv = (float)tv;
                    verts[index].Color = Color.White;
                }
            }
            vb.Unlock();
            domeTriangleCount = (domeSubX) * (domeSubY) * 2;
            var indexArray = (short[])domeIndexBuffer[face].Lock();
            index = 0;
            for (y1 = 0; y1 < domeSubY; y1++)
            {
                for (x1 = 0; x1 < domeSubX; x1++)
                {
                    //index = (y1 * domeSubX * 6) + 6 * x1;
                    // First triangle in quad
                    indexArray[index] = (short)(y1 * (domeSubX + 1) + x1);
                    indexArray[index + 1] = (short)((y1 + 1) * (domeSubX + 1) + x1);
                    indexArray[index + 2] = (short)(y1 * (domeSubX + 1) + (x1 + 1));

                    // Second triangle in quad
                    indexArray[index + 3] = (short)(y1 * (domeSubX + 1) + (x1 + 1));
                    indexArray[index + 4] = (short)((y1 + 1) * (domeSubX + 1) + x1);
                    indexArray[index + 5] = (short)((y1 + 1) * (domeSubX + 1) + (x1 + 1));
                    index += 6;
                }
            }
            domeIndexBuffer[face].Unlock();
        }

        private void CleanupDomeVertexBuffers()
        {
            if (domeIndexBuffer != null)
            {
                for (var face = 0; face < 5; face++)
                {
                    CleanupDomeVertexBuffer(face);
                }
            }
        }

        private void CleanupDomeVertexBuffer(int face)
        {
            if (domeIndexBuffer[face] != null)
            {
                domeIndexBuffer[face].Dispose();
                GC.SuppressFinalize(domeIndexBuffer[face]);
            }

            if (domeVertexBuffer[face] != null)
            {
                domeVertexBuffer[face].Dispose();
                GC.SuppressFinalize(domeVertexBuffer[face]);
            }
        }

        PositionColorTexturedVertexBuffer11 warpVertexBuffer;
        IndexBuffer11 warpIndexBuffer;
        int warpVertexCount;
        int warpIndexCount;
        int warpTriangleCount;

        public void CreateWarpVertexBuffer()
        {
            ReadWarpMeshFile();
            var warpSubX = meshX - 1;
            var warpSubY = meshY - 1;

            CleanUpWarpBuffers();


            warpIndexBuffer = new IndexBuffer11(typeof(short), (warpSubX * warpSubY * 6), RenderContext11.PrepDevice);
            warpVertexBuffer = new PositionColorTexturedVertexBuffer11(((warpSubX + 1) * (warpSubY + 1)), RenderContext11.PrepDevice);

            warpVertexCount = ((warpSubX + 1) * (warpSubY + 1));


            var index = 0;

            var vb = warpVertexBuffer;
            // Create a vertex buffer 
            var verts = (PositionColoredTextured[])vb.Lock(0, 0); // Lock the buffer (which will return our structs)
            int x1, y1;



            double textureStepX = 1.0f / warpSubX;
            double textureStepY = 1.0f / warpSubY;
            for (y1 = 0; y1 <= warpSubY; y1++)
            {

                for (x1 = 0; x1 <= warpSubX; x1++)
                {

                    index = y1 * (warpSubX + 1) + x1;
                    verts[index].Position = mesh[x1, y1].Position;
                    verts[index].Tu = mesh[x1, y1].Tu;
                    verts[index].Tv = mesh[x1, y1].Tv;
                    verts[index].Color = mesh[x1, y1].Color;
                }
            }
            vb.Unlock();
            warpTriangleCount = (warpSubX) * (warpSubY) * 2;
            var indexArray = (short[])warpIndexBuffer.Lock();
            index = 0;
            for (y1 = 0; y1 < warpSubY; y1++)
            {
                for (x1 = 0; x1 < warpSubX; x1++)
                {
                    // First triangle in quad
                    indexArray[index] = (short)(y1 * (warpSubX + 1) + x1);
                    indexArray[index + 1] = (short)((y1 + 1) * (warpSubX + 1) + x1);
                    indexArray[index + 2] = (short)(y1 * (warpSubX + 1) + (x1 + 1));

                    // Second triangle in quad
                    indexArray[index + 3] = (short)(y1 * (warpSubX + 1) + (x1 + 1));
                    indexArray[index + 4] = (short)((y1 + 1) * (warpSubX + 1) + x1);
                    indexArray[index + 5] = (short)((y1 + 1) * (warpSubX + 1) + (x1 + 1));
                    index += 6;
                }
            }
            warpIndexBuffer.Unlock();
        }

        private void CleanUpWarpBuffers()
        {
            if (warpIndexBuffer != null)
            {
                warpIndexBuffer.Dispose();
                GC.SuppressFinalize(warpIndexBuffer);
            }

            if (warpVertexBuffer != null)
            {
                warpVertexBuffer.Dispose();
                GC.SuppressFinalize(warpVertexBuffer);
            }
        }

        PositionColoredTextured[,] mesh;
        // bool WarpedDome = true;
        int meshX;
        int meshY;

        public void ReadWarpMeshFile()
        {

            var filename = Properties.Settings.Default.CahceDirectory + "meshwarp.txt";
            var appdir = Path.GetDirectoryName(Application.ExecutablePath);
            if (Properties.Settings.Default.DomeTypeIndex == 3 && String.IsNullOrEmpty(Properties.Settings.Default.CustomWarpFilename))
            {
                Properties.Settings.Default.DomeTypeIndex = 1;
            }


            switch (Properties.Settings.Default.DomeTypeIndex)
            {
                default:
                case 1:
                    filename = appdir + "\\MeshWarps\\warp_mirror_16x9.data";
                    break;
                case 2:
                    filename = appdir + "\\MeshWarps\\warp_mirror_4x3.data";
                    break;
                case 3:
                    filename = Properties.Settings.Default.CustomWarpFilename;
                    break;
            }

            if (!File.Exists(filename))
            {
                return;
            }


            var sr = new StreamReader(filename, Encoding.ASCII);
            var buffer = sr.ReadLine();
            buffer = sr.ReadLine();
            var parts = buffer.Split(new[] { ' ' });

            meshX = Convert.ToInt32(parts[0]);
            meshY = Convert.ToInt32(parts[1]);
            mesh = new PositionColoredTextured[meshX, meshY];

            for (var y = 0; y < meshY; y++)
            {
                for (var x = 0; x < meshX; x++)
                {
                    buffer = sr.ReadLine();
                    parts = buffer.Split(new[] { ' ', '\t' });
                    mesh[x, y].Position = new Vector4((Convert.ToSingle(parts[0])) / 2, Convert.ToSingle(parts[1]) / 2, .9f, 1);
                    mesh[x, y].Tu = Convert.ToSingle(parts[2]);
                    mesh[x, y].Tv = 1.0f - Convert.ToSingle(parts[3]);
                    var col = (Byte)(Convert.ToSingle(parts[4]) * 255);
                    mesh[x, y].Color = Color.FromArgb(255, col, col, col);
                }
            }

            sr.Close();
        }

        private void CleanupStereoAndDomeBuffers()
        {


            if (leftEye != null)
            {
                leftEye.Dispose();
                GC.SuppressFinalize(leftEye);
                leftEye = null;
            }

            if (rightEye != null)
            {
                rightEye.Dispose();
                GC.SuppressFinalize(rightEye);
                rightEye = null;
            }

            if (stereoRenderTexture != null)
            {
                stereoRenderTexture.Dispose();
                GC.SuppressFinalize(stereoRenderTexture);
                stereoRenderTexture = null;
            }

            if (domeZbuffer != null)
            {
                domeZbuffer.Dispose();
                GC.SuppressFinalize(domeZbuffer);
                domeZbuffer = null;
            }

            for (var face = 0; face < 5; face++)
            {
                if (domeCube[face] != null)
                {
                    domeCube[face].Dispose();
                    GC.SuppressFinalize(domeCube[face]);
                    domeCube[face] = null;
                }
            }

            if (domeCubeFaceMultisampled != null)
            {
                domeCubeFaceMultisampled.Dispose();
                GC.SuppressFinalize(domeCubeFaceMultisampled);
                domeCubeFaceMultisampled = null;
            }

            if (undistorted != null)
            {
                undistorted.Dispose();
                GC.SuppressFinalize(undistorted);
                undistorted = null;
            }
        }

        void device_DeviceResizing(object sender, CancelEventArgs e)
        {
            if (renderWindow.Height == 0 | renderWindow.Width == 0)
            {
                e.Cancel = true;
            }
        }
        const double RC = 3.1415927 / 180;
        const int subDivisionsX = 48 * 4;
        const int subDivisionsY = 24 * 4;
        bool showWireFrame;

        Color FogColor = Color.LightBlue;
        Color SkyColor = Color.Black;
        public static PlaneD[] frustum = new PlaneD[6];
        public static double front = -1;
        public static double back = 0;
        public static Vector3d cameraTarget = new Vector3d(0f, 0f, 1f);
        double colorBlend;
        static public Matrix3d WorldMatrix;
        static public Matrix3d ViewMatrix;
        static public Matrix3d ProjMatrix;
        double m_nearPlane;

        readonly int MonitorX;
        readonly int MonitorY;
        readonly int MonitorCountX = 3;
        readonly int MonitorCountY = 3;

        public static bool multiMonClient = false;
        public static bool ProjectorServer = false;

        KmlViewInformation kmlViewInfo = new KmlViewInformation();

        public KmlViewInformation KmlViewInfo
        {
            get { return kmlViewInfo; }
            set { kmlViewInfo = value; }
        }

        readonly int monitorWidth = 1920;
        readonly int monitorHeight = 1200;

        double alt;
        double targetAlt;

        public double Alt
        {
            get { return alt; }
            set { alt = value; }
        }
        double az;
        double targetAz;
        public double Az
        {
            get { return az; }
            set { az = value; }
        }

        readonly float bezelSpacing = 1.07f;
        static Vector3d viewPoint;

        static public Vector3d ViewPoint
        {
            get { return viewPoint; }
            set { viewPoint = value; }
        }

        private void SetupMatricesFisheye()
        {

            RenderContext11.World = Matrix3d.Identity;

            var view = Matrix3d.Identity;
            var ProjMatrix = Matrix3d.Identity;
            RenderContext11.View = view;



            m_nearPlane = 0f;
            if (ViewWidth > ViewHeight)
            {
                ProjMatrix.Matrix11 = Matrix.OrthoLH((ViewWidth / (float)renderWindow.ClientRectangle.Height) * 1f, 1f, 1, -1);
            }
            else
            {
                ProjMatrix.Matrix11 = Matrix.OrthoLH(1f, (renderWindow.ClientRectangle.Height / (float)ViewWidth) * 1f, 1, -1);
            }
            RenderContext11.Projection = ProjMatrix;

        }

        private void SetupMatricesWarpFisheye(float width)
        {

            RenderContext11.World = Matrix3d.Identity;

            var view = Matrix3d.Identity;

            RenderContext11.View = view;

            m_nearPlane = 0f;



            if (ViewWidth > ViewHeight)
            {
                ProjMatrix.Matrix11 = Matrix.OrthoLH(width, 1f, 1, -1);
            }
            else
            {
                ProjMatrix.Matrix11 = Matrix.OrthoLH(width, 1f, 1, -1);
            }

            RenderContext11.Projection = ProjMatrix;

        }

        private void SetupMatricesDistort()
        {
            RenderContext11.World = Matrix3d.Identity;

            var view = Matrix3d.Identity;

            RenderContext11.View = view;

            m_nearPlane = 0f;
            ProjMatrix.Matrix11 = Matrix.OrthoLH(1f, 1f, 1, -1);

            RenderContext11.Projection = ProjMatrix;

        }
        Matrix3d domeMatrix;
        bool domeMatrixFresh;
        bool domeAngleMatrixFresh;

        public bool DomeMatrixFresh
        {
            get { return domeMatrixFresh; }
            set
            {
                domeMatrixFresh = value;
                domeAngleMatrixFresh = value;
            }
        }


        Matrix3d DomeMatrix
        {
            get
            {
                if (!domeMatrixFresh)
                {
                    domeMatrix = Matrix3d.RotationX(((-(config.TotalDomeTilt + viewCamera.DomeAlt)) / 180 * Math.PI)) * Matrix3d.RotationY((config.DomeAngle + viewCamera.DomeAz) / 180 * Math.PI);
                    domeMatrixFresh = true;
                }
                return domeMatrix;
            }
        }

        Matrix3d DomeAngleMatrix
        {
            get
            {
                if (!domeAngleMatrixFresh)
                {
                    domeMatrix = Matrix3d.RotationX((-viewCamera.DomeAlt / 180 * Math.PI)) * Matrix3d.RotationY((config.DomeAngle + viewCamera.DomeAz) / 180 * Math.PI);
                    domeAngleMatrixFresh = true;
                }
                return domeMatrix;
            }
        }

        public void SetupMatricesOverlays()
        {

            RenderContext11.World = Matrix3d.Identity;

            var lookAtAdjust = Matrix3d.Identity;

            var lookFrom = new Vector3d(0, 0, 0);
            var lookAt = new Vector3d(0, 0, 1);
            var lookUp = new Vector3d(0, 1, 0);

            var dome = false;

            Matrix3d view;

            switch (CurrentRenderType)
            {
                case RenderTypes.DomeUp:
                    dome = true;
                    lookAtAdjust.Multiply(Matrix3d.RotationX(Math.PI / 2));
                    break;
                case RenderTypes.DomeLeft:
                    dome = true;
                    lookAtAdjust.Multiply(Matrix3d.RotationY(Math.PI / 2));
                    break;
                case RenderTypes.DomeRight:
                    dome = true;
                    lookAtAdjust.Multiply(Matrix3d.RotationY(-Math.PI / 2));
                    break;
                case RenderTypes.DomeFront:
                    dome = true;
                    break;
                case RenderTypes.DomeBack:
                    lookAtAdjust.Multiply(Matrix3d.RotationY(Math.PI));
                    dome = true;
                    break;
                default:
                    break;
            }

            if (config.MultiChannelDome1)
            {
                var matHeadingPitchRoll =
                    Matrix3d.RotationZ((config.Roll / 180 * Math.PI)) *
                    Matrix3d.RotationY((config.Heading / 180 * Math.PI)) *
                    Matrix3d.RotationX(((config.Pitch) / 180 * Math.PI));

                view = Matrix3d.LookAtLH(lookFrom, lookAt, lookUp) * DomeMatrix * matHeadingPitchRoll;
            }
            else
            {
                if (Settings.DomeView)
                {
                    view = Matrix3d.LookAtLH(lookFrom, lookAt, lookUp) * DomeMatrix * lookAtAdjust;

                }
                else
                {
                    if (DomePreviewPopup.Active && !dome)
                    {
                        var matDomePreview =
                             Matrix3d.RotationY((DomePreviewPopup.Az / 180 * Math.PI)) *
                             Matrix3d.RotationX((DomePreviewPopup.Alt / 180 * Math.PI));
                        view = Matrix3d.LookAtLH(lookFrom, lookAt, lookUp) * DomeMatrix * matDomePreview;
                    }
                    else if (rift)
                    {
                        var matRiftView = Matrix3d.RotationY(GetHeading()) * Matrix3d.RotationX(GetPitch()) * Matrix3d.RotationZ(-GetRoll());
                        view = Matrix3d.LookAtLH(lookFrom, lookAt, lookUp) * matRiftView;
                    }

                    else
                    {
                        view = Matrix3d.LookAtLH(lookFrom, lookAt, lookUp) * lookAtAdjust;
                    }

                }

                if (multiMonClient)
                {
                    RenderContext11.View = RenderContext11.View * Matrix3d.RotationY((config.Heading / 180 * Math.PI));
                }
            }

            var viewXform = Matrix3d.Scaling(1, -1, 1);

            view = viewXform * view;

            RenderContext11.View = view;

            double back = 10000;
            m_nearPlane = .1f;

            if (config.MultiChannelDome1)
            {
                double aspect = config.Aspect;
                var top = m_nearPlane * 2 / ((1 / Math.Tan(config.UpFov / 180 * Math.PI))) / 2;
                var bottom = m_nearPlane * 2 / -(1 / Math.Tan(config.DownFov / 180 * Math.PI)) / 2;
                var right = m_nearPlane * 2 / (1 / Math.Tan((config.UpFov + config.DownFov) / 2 / 180 * Math.PI)) * aspect / 2;
                var left = -right;



                ProjMatrix = Matrix3d.PerspectiveOffCenterLH(
                    left,
                    right,
                    bottom,
                    top,
                    m_nearPlane,
                    back);

            }
            else if (config.MultiProjector)
            {
                RenderContext11.View = RenderContext11.View * config.ViewMatrix;
                ProjMatrix = Matrix3d.PerspectiveFovLH((75f / 180f) * Math.PI, 1.777778, m_nearPlane, back);
                RenderContext11.ViewBase = RenderContext11.View;
            }
            else if (multiMonClient)
            {
                var fov = (((config.UpFov + config.DownFov) / 2 / 180 * Math.PI));
                if (fov == 0)
                {
                    fov = (Math.PI / 4.0);
                }
                ProjMatrix = Matrix3d.PerspectiveFovLH(fov, monitorWidth * MonitorCountX / (monitorHeight * (double)MonitorCountY), m_nearPlane, back);
            }
            else if (dome)
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH((Math.PI / 2.0), 1.0f, m_nearPlane, back);
            }
            else if (rift)
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH(riftFov, ViewWidth / (double)renderWindow.ClientRectangle.Height, m_nearPlane, back);
                RenderContext11.PerspectiveFov = riftFov;

            }
            else
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH(fovLocal, ViewWidth / (double)renderWindow.ClientRectangle.Height, m_nearPlane, back);
            }

            if (multiMonClient && !config.MultiChannelDome1 && !config.MultiProjector && !config.MultiChannelGlobe)
            {
                ProjMatrix.M11 *= MonitorCountX * bezelSpacing;
                ProjMatrix.M22 *= MonitorCountY * bezelSpacing;
                ProjMatrix.M31 = (MonitorCountX - 1) - (MonitorX * bezelSpacing * 2);
                ProjMatrix.M32 = -((MonitorCountY - 1) - (MonitorY * bezelSpacing * 2));
            }

            if (rift)
            {
                if (CurrentRenderType == RenderTypes.LeftEye)
                {

                    ProjMatrix.M31 += iod;
                }
                else
                {
                    ProjMatrix.M31 -= iod;
                }
            }

            RenderContext11.Projection = ProjMatrix;


        }




        public void SetupMatricesAltAz()
        {
            RenderContext11.World = Matrix3d.Identity;

            var lookAtAdjust = Matrix3d.Identity;

            var lookFrom = new Vector3d(0, 0, 0);
            var lookAt = new Vector3d(0, 0, 1);
            var lookUp = new Vector3d(0, 1, 0);

            var dome = false;

            Matrix3d view;
            Matrix3d ProjMatrix;

            switch (CurrentRenderType)
            {
                case RenderTypes.DomeUp:
                    dome = true;
                    lookAtAdjust.Multiply(Matrix3d.RotationX(Math.PI / 2));
                    break;
                case RenderTypes.DomeLeft:
                    dome = true;
                    lookAtAdjust.Multiply(Matrix3d.RotationY(Math.PI / 2));
                    break;
                case RenderTypes.DomeRight:
                    dome = true;
                    lookAtAdjust.Multiply(Matrix3d.RotationY(-Math.PI / 2));
                    break;
                case RenderTypes.DomeFront:
                    dome = true;
                    break;
                case RenderTypes.DomeBack:
                    lookAtAdjust.Multiply(Matrix3d.RotationY(Math.PI));
                    dome = true;
                    break;
                default:
                    break;
            }

            if (config.MultiChannelDome1)
            {
                var matHeadingPitchRoll =
                    Matrix3d.RotationZ((config.Roll / 180 * Math.PI)) *
                    Matrix3d.RotationY((config.Heading / 180 * Math.PI)) *
                    Matrix3d.RotationX(((config.Pitch) / 180 * Math.PI));

                view = Matrix3d.LookAtLH(lookFrom, lookAt, lookUp) * matHeadingPitchRoll;
            }
            else
            {
                if (Settings.DomeView)
                {
                    view = Matrix3d.LookAtLH(lookFrom, lookAt, lookUp) * lookAtAdjust;

                }
                else
                {
                    if (DomePreviewPopup.Active && !dome)
                    {
                        var matDomePreview =
                             Matrix3d.RotationY((DomePreviewPopup.Az / 180 * Math.PI)) *
                             Matrix3d.RotationX((DomePreviewPopup.Alt / 180 * Math.PI));
                        view = Matrix3d.LookAtLH(lookFrom, lookAt, lookUp) * matDomePreview;
                    }
                    else if (rift)
                    {
                        var matRiftView = Matrix3d.RotationY(GetHeading()) * Matrix3d.RotationX(GetPitch()) * Matrix3d.RotationZ(-GetRoll());
                        view = Matrix3d.LookAtLH(lookFrom, lookAt, lookUp) * matRiftView;
                    }
                    else
                    {
                        view = Matrix3d.LookAtLH(lookFrom, lookAt, lookUp) * DomeMatrix * lookAtAdjust;
                    }

                    if (multiMonClient)
                    {
                        RenderContext11.View = RenderContext11.View * Matrix3d.RotationY((config.Heading / 180 * Math.PI));
                    }
                }
            }

            var viewXform = Matrix3d.Scaling(1, 1, 1);

            view = viewXform * view;

            RenderContext11.View = view;

            double back = 10000;
            m_nearPlane = .1f;

            if (config.MultiChannelDome1)
            {
                double aspect = config.Aspect;
                var top = m_nearPlane * 2 / ((1 / Math.Tan(config.UpFov / 180 * Math.PI))) / 2;
                var bottom = m_nearPlane * 2 / -(1 / Math.Tan(config.DownFov / 180 * Math.PI)) / 2;
                var right = m_nearPlane * 2 / (1 / Math.Tan((config.UpFov + config.DownFov) / 2 / 180 * Math.PI)) * aspect / 2;
                var left = -right;



                ProjMatrix = Matrix3d.PerspectiveOffCenterLH(
                    left,
                    right,
                    bottom,
                    top,
                    m_nearPlane,
                    back);

            }
            else if (config.MultiProjector)
            {
                RenderContext11.View = RenderContext11.View * config.ViewMatrix;
                ProjMatrix = Matrix3d.PerspectiveFovLH((75f / 180f) * Math.PI, 1.777778, m_nearPlane, back);
                RenderContext11.ViewBase = RenderContext11.View;
            }
            else if (multiMonClient)
            {
                var fov = (((config.UpFov + config.DownFov) / 2 / 180 * Math.PI));
                if (fov == 0)
                {
                    fov = (Math.PI / 4.0);
                }
                ProjMatrix = Matrix3d.PerspectiveFovLH(fov, monitorWidth * MonitorCountX / (monitorHeight * (double)MonitorCountY), m_nearPlane, back);
            }
            else if (dome)
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH((Math.PI / 2.0), 1.0f, m_nearPlane, back);
            }
            else if (rift)
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH(riftFov, ViewWidth / (double)renderWindow.ClientRectangle.Height, m_nearPlane, back);
            }
            else
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH(RenderContext11.PerspectiveFov, ViewWidth / (double)renderWindow.ClientRectangle.Height, m_nearPlane, back);
            }

            if (multiMonClient && !config.MultiChannelDome1 && !config.MultiProjector)
            {
                ProjMatrix.M11 *= MonitorCountX * bezelSpacing;
                ProjMatrix.M22 *= MonitorCountY * bezelSpacing;
                ProjMatrix.M31 = (MonitorCountX - 1) - (MonitorX * bezelSpacing * 2);
                ProjMatrix.M32 = -((MonitorCountY - 1) - (MonitorY * bezelSpacing * 2));
            }



            RenderContext11.Projection = ProjMatrix;
        }

        bool galMatInit;
        Matrix3d galacticMatrix = Matrix3d.Identity;

        private void SetupMatricesSpace11(double localZoomFactor, RenderTypes renderType)
        {
            if (config.MultiChannelDome1 || config.MultiProjector || DomePreviewPopup.Active || rift)
            {
                SetupMatricesSpaceMultiChannel(localZoomFactor, renderType);
                return;
            }

            if ((Settings.Active.GalacticMode && !Settings.Active.LocalHorizonMode) && CurrentImageSet.DataSetType == ImageSetType.Sky)
            {
                // Show in galactic coordinates
                if (!galMatInit)
                {
                    galacticMatrix = Matrix3d.Identity;
                    galacticMatrix.Multiply(Matrix3d.RotationY(-(90 - (17.7603329867975 * 15)) / 180.0 * Math.PI));
                    galacticMatrix.Multiply(Matrix3d.RotationX(-((-28.9361739586894)) / 180.0 * Math.PI));
                    galacticMatrix.Multiply(Matrix3d.RotationZ(((31.422052860102041270114993238783) - 90) / 180.0 * Math.PI));
                    galMatInit = true;
                }

                WorldMatrix = galacticMatrix;
                WorldMatrix.Multiply(Matrix3d.RotationY(((az)) / 180.0 * Math.PI));
                WorldMatrix.Multiply(Matrix3d.RotationX(-((alt)) / 180.0 * Math.PI));


                var gPoint = Coordinates.GalactictoJ2000(az, alt);

                RA = gPoint[0] / 15;
                Dec = gPoint[1];
                viewCamera.Lat = targetViewCamera.Lat;
                viewCamera.Lng = targetViewCamera.Lng;

            }
            else
            {
                // Show in Ecliptic

                WorldMatrix = Matrix3d.RotationY(-((ViewLong + 90.0) / 180.0 * Math.PI));
                WorldMatrix.Multiply(Matrix3d.RotationX(((-ViewLat) / 180.0 * Math.PI)));
            }
            var camLocal = CameraRotate;

            // altaz
            if ((Settings.Active.LocalHorizonMode && !Settings.Active.GalacticMode) && CurrentImageSet.DataSetType == ImageSetType.Sky)
            {
                var zenithAltAz = new Coordinates(0, 0);

                zenithAltAz.Az = 0;

                zenithAltAz.Alt = 0;



                if (!config.Master)
                {
                    alt = 0;
                    az = 0;
                    config.DomeTilt = 0;
                    if (Properties.Settings.Default.DomeTilt != 0)
                    {
                        Properties.Settings.Default.DomeTilt = 0;
                    }
                }

                var zenith = Coordinates.HorizonToEquitorial(zenithAltAz, SpaceTimeController.Location, SpaceTimeController.Now);

                var raPart = -((zenith.RA - 6) / 24.0 * (Math.PI * 2));
                var decPart = -(((zenith.Dec)) / 360.0 * (Math.PI * 2));
                var raText = Coordinates.FormatDMS(zenith.RA);
                WorldMatrix = Matrix3d.RotationY(-raPart);
                WorldMatrix.Multiply(Matrix3d.RotationX(decPart));

                if (SpaceTimeController.Location.Lat < 0)
                {
                    WorldMatrix.Multiply(Matrix3d.RotationY(((az) / 180.0 * Math.PI)));

                    WorldMatrix.Multiply(Matrix3d.RotationX(((alt) / 180.0 * Math.PI)));
                    camLocal += Math.PI;
                }
                else
                {
                    WorldMatrix.Multiply(Matrix3d.RotationY(((-az) / 180.0 * Math.PI)));

                    WorldMatrix.Multiply(Matrix3d.RotationX(((-alt) / 180.0 * Math.PI)));
                }

                var currentRaDec = Coordinates.HorizonToEquitorial(Coordinates.FromLatLng(alt, az), SpaceTimeController.Location, SpaceTimeController.Now);

                TargetLat = ViewLat = currentRaDec.Dec;
                TargetLong = ViewLong = RAtoViewLng(currentRaDec.RA);
            }

            RenderContext11.World = WorldMatrix;
            RenderContext11.WorldBase = WorldMatrix;
            // altaz

            ViewPoint = Coordinates.RADecTo3d(RA, -Dec, 1.0);



            var distance = (4.0 * (localZoomFactor / 180)) + 0.000001;

            FovAngle = ((localZoomFactor/**16*/) / FOVMULT) / Math.PI * 180;
            RenderContext11.CameraPosition = new Vector3d(0.0, 0.0, 0.0);
            // This is for distance Calculation. For space everything is the same distance, so camera target is key.

            RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, new Vector3d(0.0, 0.0, -1.0), new Vector3d(Math.Sin(camLocal), Math.Cos(camLocal), 0.0));

            if (config.MultiChannelGlobe)
            {
                var globeCameraRotation =
                    Matrix3d.RotationZ((config.Roll / 180 * Math.PI)) *
                    Matrix3d.RotationY((config.Heading / 180 * Math.PI)) *
                    Matrix3d.RotationX(((config.Pitch) / 180 * Math.PI));
                RenderContext11.View = RenderContext11.View * globeCameraRotation;
            }



            if (multiMonClient)
            {
                RenderContext11.View = RenderContext11.View * Matrix3d.RotationY((config.Heading / 180 * Math.PI));
            }


            RenderContext11.ViewBase = RenderContext11.View;

            m_nearPlane = 0f;
            if (multiMonClient)
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH((localZoomFactor/**16*/) / FOVMULT, monitorWidth * MonitorCountX / (double)(monitorHeight * MonitorCountY), .1, -2.0);

            }
            else
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH((localZoomFactor/**16*/) / FOVMULT, ViewWidth / (double)renderWindow.ClientRectangle.Height, .1, -2.0);

            }
            RenderContext11.PerspectiveFov = (localZoomFactor) / FOVMULT;


            if (multiMonClient)
            {
                ProjMatrix.M11 *= MonitorCountX * bezelSpacing;
                ProjMatrix.M22 *= MonitorCountY * bezelSpacing;
                ProjMatrix.M31 = (MonitorCountX - 1) - (MonitorX * bezelSpacing * 2);
                ProjMatrix.M32 = -((MonitorCountY - 1) - (MonitorY * bezelSpacing * 2));
            }

            if (config.MultiChannelGlobe)
            {
                ProjMatrix = Matrix3d.OrthoLH(config.Aspect * 2.0, 2.0, 0.0, 2.0);
            }



            RenderContext11.Projection = ProjMatrix;

            ViewMatrix = RenderContext11.View;

            MakeFrustum();

        }


        private void SetupMatricesSpaceMultiChannel(double localZoomFactor, RenderTypes renderType)
        {
            var faceSouth = false;

            if ((Settings.Active.LocalHorizonMode && !Settings.Active.GalacticMode) && CurrentImageSet.DataSetType == ImageSetType.Sky)
            {
                faceSouth = !Properties.Settings.Default.FaceNorth;
                var currentRaDec = Coordinates.HorizonToEquitorial(Coordinates.FromLatLng(0, 0), SpaceTimeController.Location, SpaceTimeController.Now);

                alt = 0;
                az = 0;
                config.DomeTilt = 0;
                if (Properties.Settings.Default.DomeTilt != 0)
                {
                    Properties.Settings.Default.DomeTilt = 0;
                }

                TargetLat = ViewLat = currentRaDec.Dec;
                TargetLong = ViewLong = RAtoViewLng(currentRaDec.RA);

            }

            if (SolarSystemTrack != SolarSystemObjects.Custom && SolarSystemTrack != SolarSystemObjects.Undefined)
            {
                viewCamera.ViewTarget = Planets.GetPlanetTargetPoint(SolarSystemTrack, ViewLat, ViewLong, 0);
            }

            RenderContext11.LightingEnabled = false;

            var localZoom = ZoomFactor * 20;
            var lookAt = new Vector3d(0, 0, -1);
            FovAngle = ((ZoomFactor/**16*/) / FOVMULT) / Math.PI * 180;

            // for constellations
            ViewPoint = Coordinates.RADecTo3d(RA, -Dec, 1.0);


            var distance = (Math.Min(1, (.5 * (ZoomFactor / 180)))) - 1 + 0.0001;

            RenderContext11.CameraPosition = new Vector3d(0, 0, distance);
            var lookUp = new Vector3d(Math.Sin(CameraRotate), Math.Cos(CameraRotate), 0.0001f);

            var lookAtAdjust = Matrix3d.Identity;

            if ((Settings.Active.GalacticMode && !Settings.Active.LocalHorizonMode) && CurrentImageSet.DataSetType == ImageSetType.Sky)
            {
                if (!galMatInit)
                {
                    galacticMatrix = Matrix3d.Identity;
                    galacticMatrix.Multiply(Matrix3d.RotationY(-(90 - (17.7603329867975 * 15)) / 180.0 * Math.PI));
                    galacticMatrix.Multiply(Matrix3d.RotationX(-((-28.9361739586894)) / 180.0 * Math.PI));
                    galacticMatrix.Multiply(Matrix3d.RotationZ(((31.422052860102041270114993238783) - 90) / 180.0 * Math.PI));
                    galMatInit = true;
                }

                WorldMatrix = galacticMatrix;
                WorldMatrix.Multiply(Matrix3d.RotationY(((az)) / 180.0 * Math.PI));
                WorldMatrix.Multiply(Matrix3d.RotationX(-((alt)) / 180.0 * Math.PI));


                var gPoint = Coordinates.GalactictoJ2000(az, alt);

                RA = gPoint[0] / 15;
                Dec = gPoint[1];
                targetViewCamera.Lat = viewCamera.Lat;
                targetViewCamera.Lng = viewCamera.Lng;
            }
            else
            {
                // Show in Ecliptic

                WorldMatrix = Matrix3d.RotationY(-((ViewLong + 90) / 180.0 * Math.PI));
                WorldMatrix.Multiply(Matrix3d.RotationX(((-ViewLat) / 180.0 * Math.PI)));
            }


            RenderContext11.World = WorldMatrix;
            RenderContext11.WorldBase = WorldMatrix;



            lookAt.TransformCoordinate(lookAtAdjust);
            Matrix3d matHeadingPitchRoll;

            if (DomePreviewPopup.Active)
            {
                matHeadingPitchRoll =

                      Matrix3d.RotationY((DomePreviewPopup.Az / 180 * Math.PI)) *
                      Matrix3d.RotationX((DomePreviewPopup.Alt / 180 * Math.PI));
            }
            else
            {
                matHeadingPitchRoll =
                      Matrix3d.RotationZ((config.Roll / 180 * Math.PI)) *
                      Matrix3d.RotationY((config.Heading / 180 * Math.PI)) *
                      Matrix3d.RotationX(((config.Pitch) / 180 * Math.PI));
            }

            if (rift)
            {
                var matRiftView = Matrix3d.RotationY(GetHeading()) * Matrix3d.RotationX(GetPitch()) * Matrix3d.RotationZ(-GetRoll());
                RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp) * matRiftView;
            }
            else
            {
                var matNorth = Matrix3d.RotationY(faceSouth ? Math.PI : 0);

                RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp) * matNorth * DomeMatrix * matHeadingPitchRoll;
            }

            var temp = lookAt - RenderContext11.CameraPosition;
            temp.Normalize();
            ViewPoint = temp;

            // Set the near clip plane close enough that the sky dome isn't clipped
            var cameraZ = (Math.Min(1, (.5 * (ZoomFactor / 180)))) - 1 + 0.0001;
            m_nearPlane = (float)(1.0 + cameraZ) * 0.5f;

            back = 12;
            double aspect = config.Aspect;
            var top = m_nearPlane * 2 / ((1 / Math.Tan(config.UpFov / 180 * Math.PI))) / 2;
            var bottom = m_nearPlane * 2 / -(1 / Math.Tan(config.DownFov / 180 * Math.PI)) / 2;
            var right = m_nearPlane * 2 / (1 / Math.Tan((config.UpFov + config.DownFov) / 2 / 180 * Math.PI)) * aspect / 2;
            var left = -right;


            if (config.MultiChannelDome1)
            {
                ProjMatrix = Matrix3d.PerspectiveOffCenterLH(
                    left,
                    right,
                    bottom,
                    top,
                    m_nearPlane,
                    back);
            }
            else if (rift)
            {
                RenderContext11.View = RenderContext11.View * config.ViewMatrix;
                ProjMatrix = Matrix3d.PerspectiveFovLH(riftFov, ViewWidth / (double)renderWindow.ClientRectangle.Height, m_nearPlane, back);
                RenderContext11.PerspectiveFov = riftFov;

            }
            else
            {
                RenderContext11.View = RenderContext11.View * config.ViewMatrix;
                ProjMatrix = Matrix3d.PerspectiveFovLH((75f / 180f) * Math.PI, 1.777778, m_nearPlane, back);

            }

            if (rift)
            {
                if (renderType == RenderTypes.LeftEye)
                {

                    ProjMatrix.M31 += iod;
                }
                else
                {
                    ProjMatrix.M31 -= iod;
                }
            }


            RenderContext11.Projection = ProjMatrix;

            ViewMatrix = RenderContext11.View;
            RenderContext11.ViewBase = RenderContext11.View;
            MakeFrustum();
        }

        // video
        private void SetupMatricesVideoOverlay(double localZoomFactor)
        {
            if (config.MultiChannelDome1 || config.MultiProjector || DomePreviewPopup.Active)
            {
                SetupMatricesVideoOverlayMultiChannel(localZoomFactor);
                return;
            }

            WorldMatrix = Matrix3d.RotationY(-((0 + 90) / 180.0 * Math.PI));
            WorldMatrix.Multiply(Matrix3d.RotationX(((-0) / 180.0 * Math.PI)));

            double camLocal = 0;


            RenderContext11.World = WorldMatrix;
            RenderContext11.WorldBase = WorldMatrix;
            // altaz

            ViewPoint = Coordinates.RADecTo3d(0, 0, 1.0);

            
            FovAngle = ((360) / FOVMULT) / Math.PI * 180;
            RenderContext11.CameraPosition = new Vector3d(0.0, 0.0, 0.0);
            // This is for distance Calculation. For space everything is the same distance, so camera target is key.

            RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, new Vector3d(0.0, 0.0, -1.0), new Vector3d(Math.Sin(camLocal), Math.Cos(camLocal), 0.0));
            RenderContext11.ViewBase = RenderContext11.View;

            m_nearPlane = 0f;
            if (multiMonClient)
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH((360/**16*/) / FOVMULT, monitorWidth * MonitorCountX / (double)(monitorHeight * MonitorCountY), 0, -2.0);

            }
            else if (rift)
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH(riftFov, ViewWidth / (double)renderWindow.ClientRectangle.Height, m_nearPlane, back);
                RenderContext11.PerspectiveFov = riftFov;

            }
            else
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH((360/**16*/) / FOVMULT, ViewWidth / (double)renderWindow.ClientRectangle.Height, 0, -2.0);

            }



            if (multiMonClient)
            {
                ProjMatrix.M11 *= MonitorCountX * bezelSpacing;
                ProjMatrix.M22 *= MonitorCountY * bezelSpacing;
                ProjMatrix.M31 = (MonitorCountX - 1) - (MonitorX * bezelSpacing * 2);
                ProjMatrix.M32 = -((MonitorCountY - 1) - (MonitorY * bezelSpacing * 2));
            }

            if (rift)
            {
                if (CurrentRenderType == RenderTypes.LeftEye)
                {

                    ProjMatrix.M31 += iod;
                }
                else
                {
                    ProjMatrix.M31 -= iod;
                }
            }

            RenderContext11.Projection = ProjMatrix;

            ViewMatrix = RenderContext11.View;

            MakeFrustum();

        }
        private void SetupMatricesVideoOverlayMultiChannel(double localZoomFactor)
        {

            RenderContext11.LightingEnabled = false;

            var lookAt = new Vector3d(-1, 0, 0);

            RenderContext11.CameraPosition = new Vector3d(0, 0, 0);
            var lookUp = new Vector3d(0, 1, 0);

            var lookAtAdjust = Matrix3d.Identity;

            WorldMatrix = Matrix3d.Identity;

            RenderContext11.World = WorldMatrix;
            RenderContext11.WorldBase = WorldMatrix;



            lookAt.TransformCoordinate(lookAtAdjust);
            Matrix3d matHeadingPitchRoll;

            if (DomePreviewPopup.Active)
            {
                matHeadingPitchRoll =

                      Matrix3d.RotationY((DomePreviewPopup.Az / 180 * Math.PI)) *
                      Matrix3d.RotationX((DomePreviewPopup.Alt / 180 * Math.PI));
            }
            else
            {
                matHeadingPitchRoll =
                      Matrix3d.RotationZ((config.Roll / 180 * Math.PI)) *
                      Matrix3d.RotationY((config.Heading / 180 * Math.PI)) *
                      Matrix3d.RotationX(((config.Pitch) / 180 * Math.PI));
            }


            RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp) * DomeMatrix * matHeadingPitchRoll;


            m_nearPlane = .000000001;
            back = 12;
            double aspect = config.Aspect;
            var top = m_nearPlane * 2 / ((1 / Math.Tan(config.UpFov / 180 * Math.PI))) / 2;
            var bottom = m_nearPlane * 2 / -(1 / Math.Tan(config.DownFov / 180 * Math.PI)) / 2;
            var right = m_nearPlane * 2 / (1 / Math.Tan((config.UpFov + config.DownFov) / 2 / 180 * Math.PI)) * aspect / 2;
            var left = -right;


            if (config.MultiChannelDome1)
            {
                ProjMatrix = Matrix3d.PerspectiveOffCenterLH(
                    left,
                    right,
                    bottom,
                    top,
                    m_nearPlane,
                    back);
            }
            else
            {
                RenderContext11.View = RenderContext11.View * config.ViewMatrix;
 
                ProjMatrix = Matrix3d.PerspectiveFovLH((75f / 180f) * Math.PI, 1.777778, m_nearPlane, back);

            }

            RenderContext11.Projection = ProjMatrix;

            ViewMatrix = RenderContext11.View;
            RenderContext11.ViewBase = RenderContext11.View;
            MakeFrustum();
        }

      
        public static Matrix3d inverseWorld;

        public void MakeFrustum()
        {
            var viewProjection = (RenderContext11.World * RenderContext11.View * RenderContext11.Projection);

            inverseWorld = RenderContext11.World;
            inverseWorld.Invert();

            // Left plane 
            frustum[0].A = (float)(viewProjection.M14 + viewProjection.M11);
            frustum[0].B = (float)(viewProjection.M24 + viewProjection.M21);
            frustum[0].C = (float)(viewProjection.M34 + viewProjection.M31);
            frustum[0].D = (float)(viewProjection.M44 + viewProjection.M41);

            // Right plane 
            frustum[1].A = (float)(viewProjection.M14 - viewProjection.M11);
            frustum[1].B = (float)(viewProjection.M24 - viewProjection.M21);
            frustum[1].C = (float)(viewProjection.M34 - viewProjection.M31);
            frustum[1].D = (float)(viewProjection.M44 - viewProjection.M41);

            // Top plane 
            frustum[2].A = (float)(viewProjection.M14 - viewProjection.M12);
            frustum[2].B = (float)(viewProjection.M24 - viewProjection.M22);
            frustum[2].C = (float)(viewProjection.M34 - viewProjection.M32);
            frustum[2].D = (float)(viewProjection.M44 - viewProjection.M42);

            // Bottom plane 
            frustum[3].A = (float)(viewProjection.M14 + viewProjection.M12);
            frustum[3].B = (float)(viewProjection.M24 + viewProjection.M22);
            frustum[3].C = (float)(viewProjection.M34 + viewProjection.M32);
            frustum[3].D = (float)(viewProjection.M44 + viewProjection.M42);

            // Near plane 
            frustum[4].A = (float)(viewProjection.M13);
            frustum[4].B = (float)(viewProjection.M23);
            frustum[4].C = (float)(viewProjection.M33);
            frustum[4].D = (float)(viewProjection.M43);

            // Far plane 
            frustum[5].A = (float)(viewProjection.M14 - viewProjection.M13);
            frustum[5].B = (float)(viewProjection.M24 - viewProjection.M23);
            frustum[5].C = (float)(viewProjection.M34 - viewProjection.M33);
            frustum[5].D = (float)(viewProjection.M44 - viewProjection.M43);

            // Normalize planes 
            for (var i = 0; i < 6; i++)
            {
                frustum[i].Normalize();
            }
            RenderContext11.MakeFrustum();
        }


        double targetHeight = 1;
        double targetAltitude;

        public double TargetAltitude
        {
            get { return targetAltitude; }
            set { targetAltitude = value; }
        }


        double fovLocal = (Math.PI / 4.0);

        private void SetupMatricesLand11(RenderTypes renderType)
        {
            WorldMatrix = Matrix3d.RotationY(((ViewLong + 90f) / 180f * Math.PI));
            WorldMatrix.Multiply(Matrix3d.RotationX(((-ViewLat) / 180f * Math.PI)));
            RenderContext11.World = WorldMatrix;
            RenderContext11.WorldBase = WorldMatrix;


            double distance = 0;
            if (CurrentImageSet.IsMandelbrot)
            {

                distance = (4.0 * (ZoomFactor / 180)) + 0.00000000000000000000000000000000000000001;
            }
            else
            {

                distance = (4.0 * (ZoomFactor / 180)) + 0.000001;
            }

            if (Settings.Active.ShowElevationModel)
            {
                targetAltitude = GetScaledAltitudeForLatLong(ViewLat, ViewLong);
                var heightNow = 1 + targetAltitude;
                targetAltitude *= RenderContext11.NominalRadius;
                if ((double.IsNaN(heightNow)))
                {
                    heightNow = 0;
                }

                if (targetHeight < heightNow)
                {
                    targetHeight = (((targetHeight * 2) + heightNow) / 3);
                }
                else
                {
                    targetHeight = (((targetHeight * 9) + heightNow) / 10);
                }
                if (double.IsNaN(targetHeight))
                {
                    targetHeight = 0;
                }
                if (config.MultiChannelDome1 || config.MultiProjector)
                {
                    targetHeight = heightNow = NetControl.focusAltitude;
                }

            }
            else
            {
                targetAltitude = 0;
                targetHeight = 1;
            }
            var rotLocal = CameraRotate;
            if (!rift)
            {
                if (renderType == RenderTypes.RightEye)
                {
                    rotLocal -= .008;
                }
                if (renderType == RenderTypes.LeftEye)
                {
                    rotLocal += .008;
                }
            }

            RenderContext11.CameraPosition = new Vector3d(
                (Math.Sin(rotLocal) * Math.Sin(CameraAngle) * distance),
                (Math.Cos(rotLocal) * Math.Sin(CameraAngle) * distance),
                (-targetHeight - (Math.Cos(CameraAngle) * distance)));
            cameraTarget = new Vector3d(0.0f, 0.0f, -targetHeight);

            var camHeight = RenderContext11.CameraPosition.Length();
            if (Tile.GrayscaleStyle)
            {
                if (CurrentImageSet.Projection == ProjectionType.Toast && (CurrentImageSet.MeanRadius > 0 && CurrentImageSet.MeanRadius < 4000000))
                {
                    var val = (int)Math.Max(0, Math.Min(255, 255 - Math.Min(255, (camHeight - 1) * 5000)));
                    SkyColor = Color.FromArgb(213 * val / 255, 165 * val / 255, 118 * val / 255);
                }
                else if (CurrentImageSet.DataSetType == ImageSetType.Earth)
                {
                    SkyColor = Color.FromArgb(255, 184, 184, 184);
                }
                else
                {
                    SkyColor = Color.Black;
                }
            }
            else
            {
                if (CurrentImageSet.ReferenceFrame == "Mars" && Settings.Active.ShowEarthSky)
                {
                    var val = (int)Math.Max(0, Math.Min(255, 255 - Math.Min(255, (camHeight - 1) * 5000)));
                    SkyColor = Color.FromArgb(213 * val / 255, 165 * val / 255, 118 * val / 255);
                }
                else if (CurrentImageSet.DataSetType == ImageSetType.Earth && Settings.Active.ShowEarthSky)
                {
                    var val = (int)Math.Max(0, Math.Min(255, 255 - Math.Min(255, (camHeight - 1) * 5000)));
                    SkyColor = Color.FromArgb(255, val / 3, val / 3, val);
                }
                else
                {
                    SkyColor = Color.Black;
                }
            }
 
            if (config.MultiChannelGlobe)
            {
                // Move the camera to some fixed distance from the globe
                RenderContext11.CameraPosition *= 50.0 / RenderContext11.CameraPosition.Length();

                // Modify camera position in globe mode
                var globeCameraRotation =
                    Matrix3d.RotationZ((config.Roll / 180 * Math.PI)) *
                    Matrix3d.RotationY((config.Heading / 180 * Math.PI)) *
                    Matrix3d.RotationX(((config.Pitch) / 180 * Math.PI));
                RenderContext11.CameraPosition = globeCameraRotation.Transform(RenderContext11.CameraPosition);
                cameraTarget = globeCameraRotation.Transform(cameraTarget);
                RenderContext11.View = Matrix3d.LookAtLH(
                    RenderContext11.CameraPosition,
                    cameraTarget,
                    new Vector3d(Math.Sin(rotLocal) * Math.Cos(CameraAngle), Math.Cos(rotLocal) * Math.Cos(CameraAngle), Math.Sin(CameraAngle)));
            }
            else if (config.MultiChannelDome1)
            {
                var matHeadingPitchRoll =
                    Matrix3d.RotationZ((config.Roll / 180 * Math.PI)) *
                    Matrix3d.RotationY((config.Heading / 180 * Math.PI)) *
                    Matrix3d.RotationX(((config.Pitch) / 180 * Math.PI));
                RenderContext11.View = Matrix3d.LookAtLH(
                            RenderContext11.CameraPosition,
                            cameraTarget,
                            new Vector3d(Math.Sin(rotLocal) * Math.Cos(CameraAngle), Math.Cos(rotLocal) * Math.Cos(CameraAngle), Math.Sin(CameraAngle)))
                            * DomeMatrix
                            * matHeadingPitchRoll;
                RenderContext11.ViewBase = RenderContext11.View;
            }
            else
            {

                var lookUp = new Vector3d(Math.Sin(rotLocal) * Math.Cos(CameraAngle), Math.Cos(rotLocal) * Math.Cos(CameraAngle), Math.Sin(CameraAngle));

                if (DomePreviewPopup.Active)
                {
                    var matDomePreview =
                         Matrix3d.RotationY((DomePreviewPopup.Az / 180 * Math.PI)) *
                         Matrix3d.RotationX((DomePreviewPopup.Alt / 180 * Math.PI));
                    RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, cameraTarget, lookUp) * Matrix3d.RotationX(((-config.TotalDomeTilt) / 180 * Math.PI)) * matDomePreview;
                }
                else if (rift)
                {
                    var amount = distance / 100;
                    var stereoTranslate = Matrix3d.Translation(renderType == RenderTypes.LeftEye ? amount : -amount, 0, 0);
                    var matRiftView = Matrix3d.RotationY(GetHeading()) * Matrix3d.RotationX(GetPitch()) * Matrix3d.RotationZ(-GetRoll());
                    RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, cameraTarget, lookUp) * Matrix3d.Translation(HeadPosition) * matRiftView * stereoTranslate;
                }
                else
                {
                    RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, cameraTarget, lookUp) * Matrix3d.Translation(HeadPosition);

                }

                if (multiMonClient)
                {
                    RenderContext11.View = RenderContext11.View * Matrix3d.RotationY((config.Heading / 180 * Math.PI));
                }


                RenderContext11.ViewBase = RenderContext11.View;


            }

            back = Math.Sqrt((distance + 1f) * (distance + 1f) - 1);
            back = Math.Max(.5, back);

            if (Properties.Settings.Default.EarthCutawayView.State)
            {
                back = 20;
            }
            m_nearPlane = distance * .05f;
            if (config.MultiChannelGlobe)
            {
                m_nearPlane = RenderContext11.CameraPosition.Length() - 2.0;
                back = m_nearPlane + 4.0;
                ProjMatrix = Matrix3d.OrthoLH(config.Aspect * 2.0, 2.0, m_nearPlane, back);
            }
            else if (config.MultiChannelDome1)
            {
                double aspect = config.Aspect;
                var top = m_nearPlane * 2 / ((1 / Math.Tan(config.UpFov / 180 * Math.PI))) / 2;
                var bottom = m_nearPlane * 2 / -(1 / Math.Tan(config.DownFov / 180 * Math.PI)) / 2;
                var right = m_nearPlane * 2 / (1 / Math.Tan((config.UpFov + config.DownFov) / 2 / 180 * Math.PI)) * aspect / 2;
                var left = -right;

                ProjMatrix = Matrix3d.PerspectiveOffCenterLH(
                    left,
                    right,
                    bottom,
                    top,
                    m_nearPlane,
                    back);


            }
            else if (config.MultiProjector)
            {
                RenderContext11.View = RenderContext11.View * config.ViewMatrix;
                ProjMatrix = Matrix3d.PerspectiveFovLH((75f / 180f) * Math.PI, 1.777778, m_nearPlane, back);
                RenderContext11.ViewBase = RenderContext11.View;

            }
            else if (multiMonClient)
            {
                var fov = (((config.UpFov + config.DownFov) / 2 / 180 * Math.PI));
                if (fov == 0)
                {
                    fov = (Math.PI / 4.0);
                }

                m_nearPlane = distance * .05f;
                ProjMatrix = Matrix3d.PerspectiveFovLH(fov, (monitorWidth * MonitorCountX) / (monitorHeight * MonitorCountY), m_nearPlane, back);
            }
            else if (rift)
            {
                m_nearPlane = distance * .05f;
                ProjMatrix = Matrix3d.PerspectiveFovLH(riftFov, ViewWidth / (double)renderWindow.ClientRectangle.Height, m_nearPlane, back);
                RenderContext11.PerspectiveFov = riftFov;

            }
            else
            {

                m_nearPlane = distance * .05f;
                ProjMatrix = Matrix3d.PerspectiveFovLH(fovLocal, ViewWidth / (double)renderWindow.ClientRectangle.Height, m_nearPlane, back);
                RenderContext11.PerspectiveFov = fovLocal;
            }


            if (multiMonClient && !config.MultiChannelDome1 && !Config.MultiProjector && !Config.MultiChannelGlobe)
            {
                ProjMatrix.M11 *= MonitorCountX * bezelSpacing;
                ProjMatrix.M22 *= MonitorCountY * bezelSpacing;
                ProjMatrix.M31 = (MonitorCountX - 1) - (MonitorX * bezelSpacing * 2);
                ProjMatrix.M32 = -((MonitorCountY - 1) - (MonitorY * bezelSpacing * 2));
            }

            if (rift)
            {
                if (renderType == RenderTypes.LeftEye)
                {

                    ProjMatrix.M31 += iod;
                }
                else
                {
                    ProjMatrix.M31 -= iod;
                }
            }


            RenderContext11.Projection = ProjMatrix;

            colorBlend = 1 / distance;

            ViewMatrix = RenderContext11.View;


            MakeFrustum();
        }


        public double GetScaledAltitudeForLatLong(double viewLat, double viewLong)
        {
            var layer = CurrentImageSet;

            if (layer == null)
            {
                return 0;
            }

            var maxX = GetTilesXForLevel(layer, layer.BaseLevel);
            var maxY = GetTilesYForLevel(layer, layer.BaseLevel);

            for (var x = 0; x < maxX; x++)
            {
                for (var y = 0; y < maxY; y++)
                {
                    var tile = TileCache.GetTile(layer.BaseLevel, x, y, layer, null);
                    if (tile != null)
                    {
                        if (tile.IsPointInTile(viewLat, viewLong))
                        {
                            return tile.GetSurfacePointAltitude(viewLat, viewLong, false);
                        }
                    }
                }
            }
            return 0;
        }

        public double GetAltitudeForLatLong(double viewLat, double viewLong)
        {
            var layer = CurrentImageSet;

            if (layer == null)
            {
                return 0;
            }

            var maxX = GetTilesXForLevel(layer, layer.BaseLevel);
            var maxY = GetTilesYForLevel(layer, layer.BaseLevel);

            for (var x = 0; x < maxX; x++)
            {
                for (var y = 0; y < maxY; y++)
                {
                    var tile = TileCache.GetTile(layer.BaseLevel, x, y, layer, null);
                    if (tile != null)
                    {
                        if (tile.IsPointInTile(viewLat, viewLong))
                        {
                            return tile.GetSurfacePointAltitude(viewLat, viewLong, true);
                        }
                    }
                }
            }
            return 0;
        }

        public double GetAltitudeForLatLongNow(double viewLat, double viewLong)
        {
            var layer = CurrentImageSet;

            if (layer == null)
            {
                return 0;
            }

            var maxX = GetTilesXForLevel(layer, layer.BaseLevel);
            var maxY = GetTilesYForLevel(layer, layer.BaseLevel);

            for (var x = 0; x < maxX; x++)
            {
                for (var y = 0; y < maxY; y++)
                {
                    var tile = TileCache.GetTile(layer.BaseLevel, x, y, layer, null);
                    if (tile != null)
                    {
                        if (tile.IsPointInTile(viewLat, viewLong))
                        {
                            return tile.GetSurfacePointAltitudeNow(viewLat, viewLong, true, Tile.lastDeepestLevel+1);
                        }
                    }
                }
            }
            return 0;
        }

        public double GetAltitudeForLatLongForPlanet(int planetID, double viewLat, double viewLong)
        {

            var layer = GetImagesetByName(Planets.GetNameFrom3dId(planetID));

            if (layer == null)
            {
                return 0;
            }

            var maxX = GetTilesXForLevel(layer, layer.BaseLevel);
            var maxY = GetTilesYForLevel(layer, layer.BaseLevel);

            for (var x = 0; x < maxX; x++)
            {
                for (var y = 0; y < maxY; y++)
                {
                    var tile = TileCache.GetTile(layer.BaseLevel, x, y, layer, null);
                    if (tile != null)
                    {
                        if (tile.IsPointInTile(viewLat, viewLong))
                        {
                            return tile.GetSurfacePointAltitude(viewLat, viewLong, true);
                        }
                    }
                }
            }
            return 0;
        }

        private void SetupMatricesLandDome(RenderTypes renderType)
        {
            FovAngle = 60;

            RenderContext11.LightingEnabled = false;

            var localZoom = ZoomFactor * 20;
            var distance = (4.0 * (ZoomFactor / 180)) + 0.000001;


            var lookAt = new Vector3d(0.0f, 0.0f, -targetHeight);

            if (Settings.Active.ShowElevationModel)
            {
                var heightNow = 1 + GetScaledAltitudeForLatLong(ViewLat, ViewLong);
                if (targetHeight < heightNow)
                {
                    targetHeight = (((targetHeight * 2) + heightNow) / 3);
                }
                else
                {
                    targetHeight = (((targetHeight * 9) + heightNow) / 10);
                }

            }
            else
            {
                targetHeight = 1;
            }

            var rotLocal = CameraRotate;

            if (renderType == RenderTypes.RightEye)
            {
                rotLocal -= .008;
            }
            if (renderType == RenderTypes.LeftEye)
            {
                rotLocal += .008;
            }

            RenderContext11.CameraPosition = new Vector3d(
                (Math.Sin(rotLocal) * Math.Sin(CameraAngle) * distance),
                (Math.Cos(rotLocal) * Math.Sin(CameraAngle) * distance),
                (-targetHeight - (Math.Cos(CameraAngle) * distance)));

            var lookAtAdjust = Matrix3d.Identity;

            var lookUp = new Vector3d(Math.Sin(rotLocal) * Math.Cos(CameraAngle), Math.Cos(rotLocal) * Math.Cos(CameraAngle), Math.Sin(CameraAngle));

            var cubeMat = Matrix3d.Identity;

            switch (renderType)
            {
                case RenderTypes.DomeUp:
                    cubeMat = Matrix3d.RotationX((Math.PI / 2));
                    break;
                case RenderTypes.DomeLeft:
                    cubeMat = Matrix3d.RotationY((Math.PI / 2));
                    break;
                case RenderTypes.DomeRight:
                    cubeMat = Matrix3d.RotationY(-(Math.PI / 2));
                    break;
                case RenderTypes.DomeFront:
                    break;
                case RenderTypes.DomeBack:
                    cubeMat = Matrix3d.RotationY((Math.PI));
                    break;
                default:
                    break;
            }
            var camHeight = RenderContext11.CameraPosition.Length();

            if (CurrentImageSet.Projection == ProjectionType.Toast && (CurrentImageSet.MeanRadius > 0 && CurrentImageSet.MeanRadius < 4000000))
            {
                var val = (int)Math.Max(0, Math.Min(255, 255 - Math.Min(255, (camHeight - 1) * 5000)));
                SkyColor = Color.FromArgb(213 * val / 255, 165 * val / 255, 118 * val / 255);
            }
            else if (CurrentImageSet.DataSetType == ImageSetType.Earth && Settings.Active.ShowEarthSky)
            {
                var val = (int)Math.Max(0, Math.Min(255, 255 - Math.Min(255, (camHeight - 1) * 5000)));
                SkyColor = Color.FromArgb(255, val / 3, val / 3, val);
            }
            else
            {
                SkyColor = Color.Black;
            }

            WorldMatrix = Matrix3d.RotationY(((ViewLong + 90f) / 180f * Math.PI));
            WorldMatrix.Multiply(Matrix3d.RotationX(((-ViewLat) / 180f * Math.PI)));
            RenderContext11.World = WorldMatrix;
            RenderContext11.WorldBase = WorldMatrix;

            if (config.MultiChannelDome1)
            {
                var matHeadingPitchRoll =
                    Matrix3d.RotationZ((config.Roll / 180 * Math.PI)) *
                    Matrix3d.RotationX((config.Pitch / 180 * Math.PI)) *
                    Matrix3d.RotationY((config.Heading / 180 * Math.PI));

                RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp)
                    * DomeMatrix
                    * matHeadingPitchRoll;
            }
            else
            {
                RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp) * DomeMatrix * cubeMat;
            }

            var temp = lookAt - RenderContext11.CameraPosition;
            temp.Normalize();
            ViewPoint = temp;


            back = Math.Sqrt((distance + 1f) * (distance + 1f) - 1);
            m_nearPlane = distance * .1f;


            ProjMatrix = Matrix3d.PerspectiveFovLH((Math.PI / 2.0), 1.0f, m_nearPlane, back);
            if (config.MultiChannelDome1)
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH(((config.UpFov + config.DownFov) / 180 * Math.PI), ViewWidth / (double)renderWindow.ClientRectangle.Height, m_nearPlane, back);
            }

            else if (multiMonClient)
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH((Math.PI / 2.0), 1.0f, m_nearPlane, back);
                RenderContext11.PerspectiveFov = (Math.PI / 2.0);
            }

            RenderContext11.Projection = ProjMatrix;


            ViewMatrix = RenderContext11.View;
            RenderContext11.ViewBase = RenderContext11.View;

            MakeFrustum();
        }


        public SolarSystemObjects SolarSystemTrack
        {
            get
            {
                return viewCamera.Target;
            }
            set
            {
                viewCamera.Target = value;
            }
        }
        public double SolarSystemCameraDistance
        {
            get
            {
                return (4.0 * (ZoomFactor / 9)) + 0.000001;
            }

        }



        public string TrackingFrame
        {
            get { return viewCamera.TargetReferenceFrame; }
            set { viewCamera.TargetReferenceFrame = value; }
        }





        bool useSolarSystemTilt = true;

        CameraParameters CustomTrackingParams;

        Vector3d cameraOffset;

        private void SetupMatricesSolarSystem11(bool forStars, RenderTypes renderType)
        {
            if (SandboxMode)
            {
                if (SolarSystemTrack != SolarSystemObjects.Custom && SolarSystemTrack != SolarSystemObjects.Undefined)
                {
                    viewCamera.ViewTarget = new Vector3d();
                }
            }
            else
            {
                if (SolarSystemTrack != SolarSystemObjects.Custom && SolarSystemTrack != SolarSystemObjects.Undefined)
                {
                    viewCamera.ViewTarget = Planets.GetPlanetTargetPoint(SolarSystemTrack, ViewLat, ViewLong, 0);
                }
            }



            var cameraDistance = SolarSystemCameraDistance;

            var trackingMatrix = Matrix3d.Identity;
            cameraDistance -= 0.000001;

            var activeTrackingFrame = false;
            if (SolarSystemTrack == SolarSystemObjects.Custom && !string.IsNullOrEmpty(TrackingFrame))
            {
                activeTrackingFrame = true;
                viewCamera.ViewTarget = LayerManager.GetFrameTarget(RenderContext11, TrackingFrame, out trackingMatrix);
            }
            else if (!string.IsNullOrEmpty(TrackingFrame))
            {
                TrackingFrame = "";
            }


            var center = viewCamera.ViewTarget;
            var lightPosition = -center;

            var localZoom = ZoomFactor * 20;
            var lookAt = new Vector3d(0, 0, 0);

            var viewAdjust = Matrix3d.Identity;
            viewAdjust.Multiply(Matrix3d.RotationX(((-ViewLat) / 180f * Math.PI)));
            viewAdjust.Multiply(Matrix3d.RotationY(((-ViewLong) / 180f * Math.PI)));

            var lookAtAdjust = Matrix3d.Identity;


            var dome = false;

            Vector3d lookUp;





            if (useSolarSystemTilt && !SandboxMode)
            {
                var angle = CameraAngle;
                if (cameraDistance > 0.0008)
                {
                    angle = 0;
                }
                else if (cameraDistance > 0.00001)
                {
                    var val = Math.Min(1.903089987, Math.Log(cameraDistance, 10) + 5) / 1.903089987;

                    angle = angle * Math.Max(0, 1 - val);
                }



                RenderContext11.CameraPosition = new Vector3d(
                (Math.Sin(-CameraRotate) * Math.Sin(angle) * cameraDistance),
                (Math.Cos(-CameraRotate) * Math.Sin(angle) * cameraDistance),
                ((Math.Cos(angle) * cameraDistance)));
                lookUp = new Vector3d(Math.Sin(-CameraRotate), Math.Cos(-CameraRotate), 0.00001f);
            }
            else
            {
                RenderContext11.CameraPosition = new Vector3d(0, 0, ((cameraDistance)));

                lookUp = new Vector3d(Math.Sin(-CameraRotate), Math.Cos(-CameraRotate), 0.0001f);
            }


            RenderContext11.CameraPosition.TransformCoordinate(viewAdjust);

            cameraOffset = RenderContext11.CameraPosition;

            cameraOffset.TransformCoordinate(Matrix3d.Invert(trackingMatrix));



            lookUp.TransformCoordinate(viewAdjust);



            switch (renderType)
            {
                case RenderTypes.DomeUp:
                    dome = true;
                    lookAtAdjust.Multiply(Matrix3d.RotationX(Math.PI / 2));
                    break;
                case RenderTypes.DomeLeft:
                    dome = true;
                    lookAtAdjust.Multiply(Matrix3d.RotationY(Math.PI / 2));
                    break;
                case RenderTypes.DomeRight:
                    dome = true;
                    lookAtAdjust.Multiply(Matrix3d.RotationY(-Math.PI / 2));
                    break;
                case RenderTypes.DomeFront:
                    dome = true;
                    break;
                case RenderTypes.DomeBack:
                    lookAtAdjust.Multiply(Matrix3d.RotationY(Math.PI));
                    dome = true;
                    break;
                default:
                    break;
            }
            WorldMatrix = Matrix3d.Identity;
            RenderContext11.World = WorldMatrix;
            RenderContext11.WorldBase = RenderContext11.World;

            if (config.MultiChannelDome1)
            {
                var matHeadingPitchRoll =
                    Matrix3d.RotationZ((config.Roll / 180 * Math.PI)) *
                    Matrix3d.RotationY((config.Heading / 180 * Math.PI)) *
                    Matrix3d.RotationX(((config.Pitch) / 180 * Math.PI));

                RenderContext11.View = trackingMatrix * Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp) * DomeMatrix * matHeadingPitchRoll;
            }
            else
            {
                if (Settings.DomeView)
                {
                    RenderContext11.View = trackingMatrix * Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp) * DomeMatrix * lookAtAdjust;

                }
                else
                {
                    if (DomePreviewPopup.Active && !dome)
                    {
                        var matDomePreview =
                             Matrix3d.RotationY((DomePreviewPopup.Az / 180 * Math.PI)) *
                             Matrix3d.RotationX((DomePreviewPopup.Alt / 180 * Math.PI));
                        RenderContext11.View = trackingMatrix * Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp) * DomeMatrix * matDomePreview;
                    }
                    else if (rift || renderType == RenderTypes.RightEye || renderType == RenderTypes.LeftEye)
                    {
                        var amount = cameraDistance / 100;
                        var stereoTranslate = Matrix3d.Translation(renderType == RenderTypes.LeftEye ? amount : -amount, 0, 0);
                        var matRiftView = Matrix3d.Identity;
                        if (rift)
                        {
                            matRiftView = Matrix3d.RotationY(GetHeading()) * Matrix3d.RotationX(GetPitch()) * Matrix3d.RotationZ(-GetRoll());
                        }

                        RenderContext11.View = trackingMatrix * Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp) * lookAtAdjust * matRiftView * stereoTranslate;
                    }
                    else
                    {
                        RenderContext11.View = trackingMatrix * Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp) * lookAtAdjust;
                    }

                    if (multiMonClient)
                    {
                        RenderContext11.View = RenderContext11.View * Matrix3d.RotationY((config.Heading / 180 * Math.PI));
                    }

                }
            }

            RenderContext11.ViewBase = RenderContext11.View;


            var temp = lookAt - RenderContext11.CameraPosition;
            temp.Normalize();
            temp = Vector3d.TransformCoordinate(temp, trackingMatrix);
            temp.Normalize();
            ViewPoint = temp;



            if (activeTrackingFrame)
            {
                var atfCamPos = RenderContext11.CameraPosition;
                var atfLookAt = lookAt;
                var atfLookUp = lookUp;
                var mat = trackingMatrix;
                mat.Invert();

                atfCamPos.TransformCoordinate(mat);
                atfLookAt.TransformCoordinate(mat);
                atfLookUp.TransformCoordinate(mat);
                atfLookAt.Normalize();
                atfLookUp.Normalize();

                CustomTrackingParams.Angle = 0;
                CustomTrackingParams.Rotation = 0;
                CustomTrackingParams.DomeAlt = viewCamera.DomeAlt;
                CustomTrackingParams.DomeAz = viewCamera.DomeAz;
                CustomTrackingParams.TargetReferenceFrame = "";
                CustomTrackingParams.ViewTarget = viewCamera.ViewTarget;
                CustomTrackingParams.Zoom = viewCamera.Zoom;
                CustomTrackingParams.Target = SolarSystemObjects.Custom;


                var atfLook = atfCamPos - atfLookAt;
                atfLook.Normalize();



                var latlng = Coordinates.CartesianToSpherical2(atfLook);
                CustomTrackingParams.Lat = latlng.Lat;
                CustomTrackingParams.Lng = latlng.Lng - 90;

                var up = Coordinates.GeoTo3dDouble(latlng.Lat + 90, latlng.Lng - 90);
                var left = Vector3d.Cross(atfLook, up);

                var dotU = Math.Acos(Vector3d.Dot(atfLookUp, up));
                var dotL = Math.Acos(Vector3d.Dot(atfLookUp, left));

                CustomTrackingParams.Rotation = dotU;// -Math.PI / 2;
            }


            var radius = Planets.GetAdjustedPlanetRadius((int)SolarSystemTrack);


            if (cameraDistance < radius * 2.0 && !forStars)
            {
                m_nearPlane = cameraDistance * 0.03;

                m_nearPlane = Math.Max(m_nearPlane, .00000000001);
                back = 1900;
            }
            else
            {
                if (forStars)
                {
                    back = 900056;
                    back = cameraDistance > 900056 ? cameraDistance * 3 : 900056;
                    m_nearPlane = .00003f;

                }
                else
                {
                    back = cameraDistance > 1900 ? cameraDistance + 200 : 1900;

                    if (Settings.Active.SolarSystemScale < 13)
                    {
                        m_nearPlane = (float)Math.Min(cameraDistance * 0.03, 0.01);
                    }
                    else
                    {
                        m_nearPlane = .001f;
                    }
                }
            }
            if (config.MultiChannelDome1)
            {
                double aspect = config.Aspect;
                var top = m_nearPlane * 2 / ((1 / Math.Tan(config.UpFov / 180 * Math.PI))) / 2;
                var bottom = m_nearPlane * 2 / -(1 / Math.Tan(config.DownFov / 180 * Math.PI)) / 2;
                var right = m_nearPlane * 2 / (1 / Math.Tan((config.UpFov + config.DownFov) / 2 / 180 * Math.PI)) * aspect / 2;
                var left = -right;

                ProjMatrix = Matrix3d.PerspectiveOffCenterLH(
                    left,
                    right,
                    bottom,
                    top,
                    m_nearPlane,
                    back);
            }
            else if (config.MultiProjector)
            {

                RenderContext11.View = RenderContext11.View * config.ViewMatrix;

                ProjMatrix = Matrix3d.PerspectiveFovLH((75f / 180f) * Math.PI, 1.777778, m_nearPlane, back);

                RenderContext11.ViewBase = RenderContext11.View;
            }
            else if (multiMonClient && !dome)
            {
                var fov = (((config.UpFov + config.DownFov) / 2 / 180 * Math.PI));
                if (fov == 0)
                {
                    fov = (Math.PI / 4.0);
                }
                ProjMatrix = Matrix3d.PerspectiveFovLH((Math.PI / 4.0), monitorWidth * MonitorCountX / (monitorHeight * (double)MonitorCountY), m_nearPlane, back);
            }
            else if (dome)
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH((Math.PI / 2.0), 1.0f, m_nearPlane, back);
            }
            else if (rift)
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH(riftFov, ViewWidth / (double)renderWindow.ClientRectangle.Height, m_nearPlane, back);
                RenderContext11.PerspectiveFov = riftFov;

            }
            else
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH((fovLocal), ViewWidth / (double)renderWindow.ClientRectangle.Height, m_nearPlane, back);
                RenderContext11.PerspectiveFov = fovLocal;
            }

            if (multiMonClient && !config.MultiChannelDome1 && !config.MultiProjector)
            {
                ProjMatrix.M11 *= MonitorCountX * bezelSpacing;
                ProjMatrix.M22 *= MonitorCountY * bezelSpacing;
                ProjMatrix.M31 = (MonitorCountX - 1) - (MonitorX * bezelSpacing * 2);
                ProjMatrix.M32 = -((MonitorCountY - 1) - (MonitorY * bezelSpacing * 2));
            }

            if (rift)
            {
                if (renderType == RenderTypes.LeftEye)
                {

                    ProjMatrix.M31 += iod;
                }
                else
                {
                    ProjMatrix.M31 -= iod;
                }
            }

            RenderContext11.Projection = ProjMatrix;

            ViewMatrix = RenderContext11.View;
            RenderContext11.ViewBase = RenderContext11.View;

            MakeFrustum();
        }

        float iod = .181f;
        private void SetupMatricesSpaceDome(bool forStars, RenderTypes renderType)
        {

            if (SolarSystemTrack != SolarSystemObjects.Custom && SolarSystemTrack != SolarSystemObjects.Undefined)
            {
                viewCamera.ViewTarget = Planets.GetPlanetTargetPoint(SolarSystemTrack, ViewLat, ViewLong, 0);
            }


            var camLocal = CameraRotate;
            if ((Settings.Active.LocalHorizonMode && !Settings.Active.GalacticMode) && CurrentImageSet.DataSetType == ImageSetType.Sky)
            {
                if (Properties.Settings.Default.ShowHorizon != false)
                {
                    Properties.Settings.Default.ShowHorizon = false;
                }
                var zenithAltAz = new Coordinates(0, 0);

                zenithAltAz.Az = 0;

                zenithAltAz.Alt = 0;

                ZoomFactor = TargetZoom = ZoomMax;
                alt = 0;
                az = 0;

                var zenith = Coordinates.HorizonToEquitorial(zenithAltAz, SpaceTimeController.Location, SpaceTimeController.Now);

                var raPart = -((zenith.RA - 6) / 24.0 * (Math.PI * 2));
                var decPart = -(((zenith.Dec)) / 360.0 * (Math.PI * 2));
                var raText = Coordinates.FormatDMS(zenith.RA);
                WorldMatrix = Matrix3d.RotationY(-raPart);
                WorldMatrix.Multiply(Matrix3d.RotationX(decPart));

                if (SpaceTimeController.Location.Lat < 0)
                {
                    WorldMatrix.Multiply(Matrix3d.RotationY(((az) / 180.0 * Math.PI)));

                    WorldMatrix.Multiply(Matrix3d.RotationX(((alt) / 180.0 * Math.PI)));
                    camLocal += Math.PI;
                }
                else
                {
                    WorldMatrix.Multiply(Matrix3d.RotationY(((-az) / 180.0 * Math.PI)));

                    WorldMatrix.Multiply(Matrix3d.RotationX(((-alt) / 180.0 * Math.PI)));
                }

                var currentRaDec = Coordinates.HorizonToEquitorial(Coordinates.FromLatLng(alt, az), SpaceTimeController.Location, SpaceTimeController.Now);

                TargetLat = ViewLat = currentRaDec.Dec;
                TargetLong = ViewLong = RAtoViewLng(currentRaDec.RA);

            }

            var center = viewCamera.ViewTarget;
            RenderContext11.LightingEnabled = false;

            var localZoom = ZoomFactor * 20;
            var lookAt = new Vector3d(0, 0, -1);
            FovAngle = ((ZoomFactor/**16*/) / FOVMULT) / Math.PI * 180;


             var distance = (Math.Min(1, (.5 * (ZoomFactor / 180)))) - 1 + 0.0001;

            RenderContext11.CameraPosition = new Vector3d(0, 0, distance);
            var lookUp = new Vector3d(Math.Sin(-CameraRotate), Math.Cos(-CameraRotate), 0.0001f);

            var lookAtAdjust = Matrix3d.Identity;

            switch (renderType)
            {
                case RenderTypes.DomeUp:
                    lookAtAdjust.Multiply(Matrix3d.RotationX(Math.PI / 2));
                    break;
                case RenderTypes.DomeLeft:
                    lookAtAdjust.Multiply(Matrix3d.RotationY(Math.PI / 2));
                    break;
                case RenderTypes.DomeRight:
                    lookAtAdjust.Multiply(Matrix3d.RotationY(-Math.PI / 2));
                    break;
                case RenderTypes.DomeFront:
                    break;
                case RenderTypes.DomeBack:
                    lookAtAdjust.Multiply(Matrix3d.RotationY(Math.PI));
                    break;
                default:
                    break;
            }


            if ((Settings.Active.GalacticMode && !Settings.Active.LocalHorizonMode) && CurrentImageSet.DataSetType == ImageSetType.Sky)
            {
                if (!galMatInit)
                {
                    galacticMatrix = Matrix3d.Identity;
                    galacticMatrix.Multiply(Matrix3d.RotationY(-(90 - (17.7603329867975 * 15)) / 180.0 * Math.PI));
                    galacticMatrix.Multiply(Matrix3d.RotationX(-((-28.9361739586894)) / 180.0 * Math.PI));
                    galacticMatrix.Multiply(Matrix3d.RotationZ(((31.422052860102041270114993238783) - 90) / 180.0 * Math.PI));
                    galMatInit = true;
                }

                WorldMatrix = galacticMatrix;
                WorldMatrix.Multiply(Matrix3d.RotationY(((az)) / 180.0 * Math.PI));
                WorldMatrix.Multiply(Matrix3d.RotationX(-((alt)) / 180.0 * Math.PI));


                var gPoint = Coordinates.GalactictoJ2000(az, alt);

                RA = gPoint[0] / 15;
                Dec = gPoint[1];
                targetViewCamera.Lat = viewCamera.Lat;
                targetViewCamera.Lng = viewCamera.Lng;
            }
            else
            {
                // Show in Ecliptic

                WorldMatrix = Matrix3d.RotationY(-((ViewLong + 90.0) / 180.0 * Math.PI));
                WorldMatrix.Multiply(Matrix3d.RotationX(((-ViewLat) / 180.0 * Math.PI)));
            }

            RenderContext11.World = WorldMatrix;
            RenderContext11.WorldBase = WorldMatrix;


            if (Settings.Active.LocalHorizonMode)
            {
                var matNorth = Matrix3d.RotationY(Properties.Settings.Default.FaceNorth ? 0 : Math.PI);
                RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp) * matNorth * DomeAngleMatrix * lookAtAdjust;
            }
            else
            {
                RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp) * DomeMatrix * lookAtAdjust;
            }
            var temp = lookAt - RenderContext11.CameraPosition;
            temp.Normalize();
            ViewPoint = temp;

            // Set the near clip plane close enough that the sky dome isn't clipped
            var cameraZ = (Math.Min(1, (.5 * (ZoomFactor / 180)))) - 1 + 0.0001;
            m_nearPlane = (float)(1.0 + cameraZ) * 0.5f;

            ProjMatrix = Matrix3d.PerspectiveFovLH((Math.PI / 2.0), 1.0f, m_nearPlane, -1f);
            RenderContext11.PerspectiveFov = (Math.PI / 2.0);
            if (multiMonClient)
            {
                ProjMatrix.M11 *= MonitorCountX * bezelSpacing;
                ProjMatrix.M22 *= MonitorCountY * bezelSpacing;
                ProjMatrix.M31 = (MonitorCountX - 1) - (MonitorX * bezelSpacing * 2);
                ProjMatrix.M32 = -((MonitorCountY - 1) - (MonitorY * bezelSpacing * 2));
            }

            RenderContext11.Projection = ProjMatrix;

            ViewMatrix = RenderContext11.View;
            RenderContext11.ViewBase = RenderContext11.View;
            MakeFrustum();
        }

        private void SetupMatricesVideoOverlayDome(bool forStars, RenderTypes renderType)
        {


            var center = viewCamera.ViewTarget;
            RenderContext11.LightingEnabled = false;

            var localZoom = ZoomFactor * 20;
            var lookAt = new Vector3d(0, 0, -1);
            FovAngle = ((360) / FOVMULT) / Math.PI * 180;

            double distance = 1;

            RenderContext11.CameraPosition = new Vector3d(0, 0, distance);
            var lookUp = new Vector3d(Math.Sin(-0), Math.Cos(-0), 0.0001f);

            var lookAtAdjust = Matrix3d.Identity;

            switch (renderType)
            {
                case RenderTypes.DomeUp:
                    lookAtAdjust.Multiply(Matrix3d.RotationX(Math.PI / 2));
                    break;
                case RenderTypes.DomeLeft:
                    lookAtAdjust.Multiply(Matrix3d.RotationY(Math.PI / 2));
                    break;
                case RenderTypes.DomeRight:
                    lookAtAdjust.Multiply(Matrix3d.RotationY(-Math.PI / 2));
                    break;
                case RenderTypes.DomeFront:
                    break;
                case RenderTypes.DomeBack:
                    lookAtAdjust.Multiply(Matrix3d.RotationY(Math.PI));
                    break;
                default:
                    break;
            }

            WorldMatrix = Matrix3d.RotationY(-((0 + 90) / 180f * Math.PI));
            WorldMatrix.Multiply(Matrix3d.RotationX(((0) / 180f * Math.PI)));
            RenderContext11.World = WorldMatrix;
            RenderContext11.WorldBase = WorldMatrix;

            if (rift)
            {
                double amount = 0;
                var stereoTranslate = Matrix3d.Translation(renderType == RenderTypes.LeftEye ? amount : -amount, 0, 0);
                var matRiftView = Matrix3d.RotationY(GetHeading()) * Matrix3d.RotationX(GetPitch()) * Matrix3d.RotationZ(-GetRoll());
                RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp) * DomeMatrix * lookAtAdjust * matRiftView * stereoTranslate;
            }
            else
            {
                RenderContext11.View = Matrix3d.LookAtLH(RenderContext11.CameraPosition, lookAt, lookUp) * DomeMatrix * lookAtAdjust;
            }

            var temp = lookAt - RenderContext11.CameraPosition;
            temp.Normalize();
            ViewPoint = temp;


            m_nearPlane = ((.000000001));

            if (rift)
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH(riftFov, 1.0f, m_nearPlane, -1f);
            }
            else
            {
                ProjMatrix = Matrix3d.PerspectiveFovLH((Math.PI / 2.0), 1.0f, m_nearPlane, -1f);
            }

            if (multiMonClient)
            {
                ProjMatrix.M11 *= MonitorCountX * bezelSpacing;
                ProjMatrix.M22 *= MonitorCountY * bezelSpacing;
                ProjMatrix.M31 = (MonitorCountX - 1) - (MonitorX * bezelSpacing * 2);
                ProjMatrix.M32 = -((MonitorCountY - 1) - (MonitorY * bezelSpacing * 2));
            }

            if (rift)
            {
                if (renderType == RenderTypes.LeftEye)
                {

                    ProjMatrix.M31 += iod;
                }
                else
                {
                    ProjMatrix.M31 -= iod;
                }
            }

            RenderContext11.Projection = ProjMatrix;

            ViewMatrix = RenderContext11.View;
            RenderContext11.ViewBase = RenderContext11.View;
            MakeFrustum();
        }



        public bool IsSphereInViewFrustum(Vector3 center, float radius)
        {
            var centerV4 = new Vector4d(center.X, center.Y, center.Z, 1f);
            for (var i = 0; i < 6; i++)
            {
                if (frustum[i].Dot(centerV4) + radius < 0)
                {
                    return false;
                }
            }
            return true;
        }


        public void SetLabelText(IPlace place, bool showText)
        {
            if (label != null)
            {
                label.Dispose();
                GC.SuppressFinalize(label);
                label = null;
            }
            if (place != null)
            {

                if (SolarSystemMode || PlanetLike)
                {
                    if (PlanetLike)
                    {
                        label = new SkyLabel(RenderContext11, ((place.Lng / 15)) + 180, place.Lat, showText ? place.Name : "", LabelSytle.Telrad, 1.0);
                    }
                    else if (place.Classification == Classification.SolarSystem)
                    {
                        var temp = Planets.GetPlanet3dLocation((SolarSystemObjects)Planets.GetPlanetIDFromName(place.Name));

                        temp.Subtract(MainWindow.viewCamera.ViewTarget);
                        temp.Subtract(MainWindow.viewCamera.ViewTarget);

                        label = new SkyLabel(RenderContext11, temp, showText ? place.Name : "", LabelSytle.Telrad);

                    }
                    else
                    {
                        label = new SkyLabel(RenderContext11, place.RA, -place.Dec, showText ? place.Name : "", LabelSytle.Telrad, place.Distance != 0 ? place.Distance : 100000000.0);
                    }
                }
                else
                {
                    label = new SkyLabel(RenderContext11, place.RA, place.Dec, showText ? place.Name : "", LabelSytle.Telrad, place.Distance != 0 ? place.Distance : 1.0);
                }
            }
        }



        string constellation;

        public string Constellation
        {
            get { return constellation; }
        }

        Coordinates[] currentViewCorners;

        public Coordinates[] CurrentViewCorners
        {
            get { return currentViewCorners; }
            set { currentViewCorners = value; }
        }

        bool hemisphereView = false;

        int frameCount;

        long lastSampleTime;
        public static int masterSyncFrameNumber = 0;

        static bool logging;

        public static bool Logging
        {
            get { return logging; }
            set
            {
                if (logging != value)
                {
                    logging = value;

                    if (logFilestream != null)
                    {
                        logFilestream.Close();
                        logFilestream = null;
                    }

                    if (logging)
                    {
                        FrameNumber = masterSyncFrameNumber;
                        logFilestream = new StreamWriter("C:\\wwtconfig\\wwtrenderlog.txt");
                        logFilestream.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}", "Frame Number", "Master Frame", "Render Time", "Tiles Loaded", "Textures", "Garbage Collections", "Memory", "Status Report");
                    }
                }
            }
        }
        private static readonly Mutex logMutex = new Mutex();
        public static void WriteLogMessage(string message)
        {
            if (logging)
            {
                var ticks = HiResTimer.TickCount - lastRender;

                var ms = (int)((ticks * 1000) / HiResTimer.Frequency);

                logMutex.WaitOne();
                logFilestream.WriteLine("{0}\t{1}\t{2}\t{3}", frameNumber, masterSyncFrameNumber, ms, message);
                logMutex.ReleaseMutex();
            }
        }

        static StreamWriter logFilestream;
        static int frameNumber;

        public static int FrameNumber
        {
            get { return frameNumber; }
            set { frameNumber = value; }
        }
        static int lastGcCount;
        public static float LastFPS = 0;
        static DateTime lastPing = DateTime.Now;

        public static int statusReport = 0;

        private void UpdateStats()
        {
            frameCount++;
            var ticks = HiResTimer.TickCount - lastSampleTime;

            var seconds = (double)(ticks / HiResTimer.Frequency);


            if (seconds > 1.0)
            {
                var frameRate = frameCount / seconds;
                sendMoveCount = 0;
                lastSampleTime = HiResTimer.TickCount;
                LastFPS = (float)frameRate;
                frameCount = 0;

                if (showPerfData)
                {
                    Text = Language.GetLocalizedText(3, "Microsoft WorldWide Telescope") + " - FPS:" + frameRate.ToString("0.0") + Language.GetLocalizedText(84, "  Tiles in View:") + Tile.TilesInView + Language.GetLocalizedText(85, " Triangles Rendered:") + Tile.TrianglesRendered + " Tiled Ready" + TileCache.GetReadyCount() + "Total Tiles:" + TileCache.GetTotalCount();
                }
            }

            if (config.Master == false)
            {
                var ts = DateTime.Now - lastPing;
                if (ts.TotalSeconds > 20)
                {
                    NetControl.PingStatus();
                    statusReport++;
                    lastPing = DateTime.Now.AddMilliseconds(pingRandom.NextDouble() * 5000);
                }

            }


            frameNumber++;
            if (logging)
            {
                if (logFilestream != null)
                {
                    ticks = HiResTimer.TickCount - lastRender;

                    var ms = (int)((ticks * 1000) / HiResTimer.Frequency);
                    var gcCount = GC.CollectionCount(2);
                    var thisCount = gcCount - lastGcCount;
                    lastGcCount = gcCount;


                    var memNow = GC.GetTotalMemory(false);
                    if (lastMem == 0)
                    {
                        lastMem = memNow;
                    }
                    var mem = memNow - lastMem;
                    lastMem = memNow;
                    logFilestream.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}", frameNumber, masterSyncFrameNumber, ms, TileCache.tilesLoadedThisFrame, Tile.TexturesLoaded, thisCount, mem, statusReport);
                    TileCache.tilesLoadedThisFrame = 0;
                    Tile.TexturesLoaded = 0;
                    bgImagesetGets = 0;
                    bgImagesetFails = 0;
                    fgImagesetGets = 0;
                    fgImagesetFails = 0;
                    statusReport = 0;
                }
            }
        }

        long lastMem;
        readonly Random pingRandom = new Random();
        BlendState panoramaBlend = new BlendState();
        readonly BlendState fovBlend = new BlendState();
        readonly BlendState fadeImageSet = new BlendState(true, 2000);
        IViewMover mover;

        internal IViewMover Mover
        {
            get { return mover; }
            set { mover = value; }
        }


        public CameraParameters viewCamera = new CameraParameters(0, 0, 360, 0, 0, 100);
        public CameraParameters targetViewCamera = new CameraParameters(0, 0, 360, 0, 0, 100);

        public bool IsMoving
        {
            get
            {
                return (mover != null || zooming == true || targetViewCamera != viewCamera) || JoyInMotion;
            }
        }

        double fovAngle;

        public double FovAngle
        {
            get { return fovAngle; }
            set
            {
                fovAngle = value;
                degreesPerPixel = fovAngle / renderWindow.ClientRectangle.Height;
            }
        }

        private double degreesPerPixel;

        public double DegreesPerPixel
        {
            get { return degreesPerPixel; }
            set { degreesPerPixel = value; }
        }
        delegate void RenderDelegate();
        delegate void BackInitDelegate();
        public void RenderCrossThread()
        {
            Invoke(new RenderDelegate(Render));
        }

        bool blink;
        DateTime lastBlink = DateTime.Now;

        public static int LoadTileBudget = 1;


        bool refreshDomeTextures = true;
        bool usingLargeTextures = true;
        int currentCubeFaceSize;
        public bool SyncLayerNeeded = false;
        public bool SyncTourNeeded = false;
        public bool ChronoZoomOpen = false;

        public void Render()
        {
            if (!readyToRender)
            {
                return;
            }


            if (SyncLayerNeeded)
            {
                NetControl.SyncLayersUiThread();
            }

            if (SyncTourNeeded)
            {
                NetControl.SyncTourUiThread();
            }

            if (Tile.fastLoad)
            {
                Tile.fastLoadAutoReset = true;
            }

            if (!TourPlayer.Playing)
            {
                MainWindow.CrossFadeFrame = false;
            }

            

            var ticks = HiResTimer.TickCount;

            var elapsedSeconds = ((double)(ticks - lastRenderTickCount)) / HiResTimer.Frequency;

            if (Properties.Settings.Default.TargetFrameRate != 0 && !(Properties.Settings.Default.FrameSync && Properties.Settings.Default.TargetFrameRate == 60))
            {
                var frameRate = Properties.Settings.Default.TargetFrameRate;


                if (elapsedSeconds < (1.0 / frameRate))
                {
                    return;
                }
            }

            lastRenderTickCount = ticks;

            lastFrameTime = (Math.Min(.1, elapsedSeconds));

            //Update MetaNow to current realtime for entire frame to render exactly on time
            SpaceTimeController.MetaNow = DateTime.Now;


            LoadTileBudget = 1;

            if (IsPaused() || !Initialized)
            {
                Thread.Sleep(100);
                return;
            }



            if (ProjectorServer)
            {
                UpdateNetworkStatus();
            }


            //oculus rift support
            rift = StereoMode == StereoModes.OculusRift;
            if (rift)
            {
                GetSensorSample();
            }




            TileCache.PurgeLRU();
            TileCache.DecimateQueue();

            Tile.imageQuality = Properties.Settings.Default.ImageQuality;

            Tile.CurrentRenderGeneration++;
            IconCacheEntry.CurrentFrame = Tile.CurrentRenderGeneration;

            Tile.lastDeepestLevel = Tile.deepestLevel;
            Tile.TilesInView = 0;
            Tile.TrianglesRendered = 0;
            Tile.TilesTouched = 0;
 

            if (ZoomFactor == 0 || TargetZoom == 0 || double.IsNaN(ZoomFactor) || double.IsNaN(TargetZoom))
            {
                ZoomFactor = TargetZoom = 360;
            }

            if (contextPanel != null)
            {
                contextPanel.QueueProgress = TileCache.QueuePercent;
            }

            TileCache.InitNextWaitingTile();

            // reset dome matrix Cache
            DomeMatrixFresh = false;


            if (mover != null)
            {
                SpaceTimeController.Now = mover.CurrentDateTime;
            }
            else
            {
                SpaceTimeController.UpdateClock();
                LayerManager.UpdateLayerTime();
            }

            if (uiController != null)
            {
                {
                    uiController.PreRender(this);
                }
            }

            if (Space)
            {
                Planets.UpdatePlanetLocations(false);
            }
            else if (CurrentImageSet.DataSetType == ImageSetType.SolarSystem)
            {
                // todo allow update of focus planet
                Planets.UpdatePlanetLocations(true);
                Planets.UpdateOrbits(0);
            }

            UpdateSpaceNavigator();
            UpdateXInputState();
            UpdateNetControlState();
            if (mover != null)
            {
                UpdateMover(mover);
            }
            else
            {
                if (!SandboxMode)
                {
                    if (SolarSystemTrack == SolarSystemObjects.Undefined | (SolarSystemTrack == SolarSystemObjects.Custom && viewCamera.ViewTarget == Vector3d.Empty))
                    {
                        SolarSystemTrack = SolarSystemObjects.Sun;
                    }
                }
            }

            UpdateViewParameters();

            if (SolarSystemMode)
            {
                if (SolarSystemTrack != SolarSystemObjects.Custom)
                {
                    viewCamera.ViewTarget = Planets.GetPlanet3dLocation(SolarSystemTrack);
                }
            }

            ClampZoomValues();

            if (blink)
            {
                var ts = DateTime.Now - lastBlink;
                if (ts.TotalMilliseconds > 500)
                {
                    if (StudyOpacity > 0)
                    {
                        StudyOpacity = 0;
                    }
                    else
                    {
                        StudyOpacity = 100;
                    }
                    lastBlink = DateTime.Now;
                }
            }

            LayerManager.PrepTourLayers();

            if (Settings.MasterController)
            {
                SendMove();
            }



            if (contextPanel != null)
            {

                contextPanel.QueueProgress = TileCache.QueuePercent;

                if (Space)
                {
                    contextPanel.ViewLevel = fovAngle;
                    contextPanel.RA = RA;
                    contextPanel.Dec = Dec;

                    if (constellationCheck != null)
                    {
                        constellation = constellationCheck.FindConstellationForPoint(RA, Dec);
                        contextPanel.Constellation = Constellations.FullName(Constellation);
                    }
                }
                else if (SolarSystemMode || SandboxMode)
                {
                    if (SandboxMode)
                    {
                        contextPanel.Sandbox = true;
                        contextPanel.Distance = SolarSystemCameraDistance;
                    }
                    else
                    {
                        contextPanel.Sandbox = false;
                        contextPanel.Distance = SolarSystemCameraDistance;
                    }


                    if (!SandboxMode && (viewCamera.Target != SolarSystemObjects.Custom && viewCamera.Target != SolarSystemObjects.Undefined)) 
                    {
                        var pnt = Coordinates.GeoTo3dDouble(ViewLat, ViewLong + 90);

                        var EarthMat = Planets.EarthMatrixInv;


                        pnt = Vector3d.TransformCoordinate(pnt, EarthMat);
                        pnt.Normalize();


                        var radec = Coordinates.CartesianToLatLng(pnt);

                        if (viewCamera.Target != SolarSystemObjects.Earth)
                        {
                            if (radec.X < 0)
                            {
                                radec.X += 360;
                            }
                        }

                        contextPanel.RA = radec.X;
                        contextPanel.Dec = radec.Y;
                    }
                    else
                    {
                        contextPanel.RA = ViewLong;
                        contextPanel.Dec = ViewLat;
                    }
                    contextPanel.Constellation = null;
                }
                else if (PlanetLike)
                {
                    contextPanel.Sandbox = false;
                    contextPanel.Distance = SolarSystemCameraDistance / UiTools.KilometersPerAu * 370;
                    contextPanel.RA = ViewLong;
                    contextPanel.Dec = ViewLat;
                    contextPanel.Constellation = null;
                }
                else
                {
                    contextPanel.Sandbox = false;
                    contextPanel.ViewLevel = fovAngle;
                    contextPanel.RA = ViewLong;
                    contextPanel.Dec = ViewLat;
                    contextPanel.Constellation = null;
                }
            }

            if (StereoMode != StereoModes.Off && (!Space || rift))
            {
                RenderContext11.ViewPort = new Viewport(0, 0, ViewWidth, renderWindow.Height, 0.0f, 1.0f);

                // Ensure that the dome depth/stencil buffer matches our requirements
                if (domeZbuffer != null)
                {
                    if (domeZbuffer.Width != ViewWidth || domeZbuffer.Height != renderWindow.Height)
                    {
                        domeZbuffer.Dispose();
                        GC.SuppressFinalize(domeZbuffer);
                        domeZbuffer = null;
                    }
                }

                if (leftEye != null)
                {
                    if (leftEye.RenderTexture.Height != renderWindow.Height || leftEye.RenderTexture.Width != ViewWidth)
                    {
                        leftEye.Dispose();
                        GC.SuppressFinalize(leftEye);
                        leftEye = null;
                    }
                }

                if (rightEye != null)
                {
                    if (rightEye.RenderTexture.Height != renderWindow.Height || rightEye.RenderTexture.Width != ViewWidth)
                    {
                        rightEye.Dispose();
                        GC.SuppressFinalize(rightEye);
                        rightEye = null;
                    }
                }

                if (stereoRenderTexture != null)
                {
                    if (stereoRenderTexture.RenderTexture.Height != renderWindow.Height || stereoRenderTexture.RenderTexture.Width != ViewWidth)
                    {
                        stereoRenderTexture.Dispose();
                        GC.SuppressFinalize(stereoRenderTexture);
                        stereoRenderTexture = null;
                    }
                }

                if (domeZbuffer == null)
                {
                    domeZbuffer = new DepthBuffer(ViewWidth, renderWindow.Height);
                }

                if (leftEye == null)
                {
                    leftEye = new RenderTargetTexture(ViewWidth, renderWindow.Height, 1);
                }

                if (rightEye == null)
                {
                    rightEye = new RenderTargetTexture(ViewWidth, renderWindow.Height, 1);
                }

                if (RenderContext11.MultiSampleCount > 1)
                {
                    // When multisample anti-aliasing is enabled, render to an offscreen buffer and then
                    // resolve to the left and then the right eye textures. 
                    if (stereoRenderTexture == null)
                    {
                        stereoRenderTexture = new RenderTargetTexture(ViewWidth, renderWindow.Height);
                    }

                    RenderFrame(stereoRenderTexture, domeZbuffer, RenderTypes.LeftEye);
                    RenderContext11.PrepDevice.ImmediateContext.ResolveSubresource(stereoRenderTexture.RenderTexture.Texture, 0,
                                                                                   leftEye.RenderTexture.Texture, 0,
                                                                                   RenderContext11.DefaultColorFormat);
                    RenderFrame(stereoRenderTexture, domeZbuffer, RenderTypes.RightEye);
                    RenderContext11.PrepDevice.ImmediateContext.ResolveSubresource(stereoRenderTexture.RenderTexture.Texture, 0,
                                                                                   rightEye.RenderTexture.Texture, 0,
                                                                                   RenderContext11.DefaultColorFormat);
                }
                else
                {
                    // When anti-aliasing is not enabled, render directly to the left and right eye textures.
                    RenderFrame(leftEye, domeZbuffer, RenderTypes.LeftEye);
                    RenderFrame(rightEye, domeZbuffer, RenderTypes.RightEye);
                }

                if (StereoMode == StereoModes.InterlineEven || StereoMode == StereoModes.InterlineOdd)
                {
                    RenderSteroPairInterline(leftEye, rightEye);
                }
                else if (StereoMode == StereoModes.AnaglyphMagentaGreen || StereoMode == StereoModes.AnaglyphRedCyan || StereoMode == StereoModes.AnaglyphYellowBlue)
                {
                    RenderSteroPairAnaglyph(leftEye, rightEye);
                }
                else if (StereoMode == StereoModes.OculusRift)
                {
                    RenderSteroOculurRift(leftEye, rightEye);
                }
                else
                {
                    if (StereoMode == StereoModes.CrossEyed)
                    {

                        RenderSteroPairSideBySide(rightEye, leftEye);
                    }
                    else
                    {
                        RenderSteroPairSideBySide(leftEye, rightEye);
                    }
                }
            }
            else if (Settings.DomeView)
            {
                var cubeFaceSize = 512;
                if (usingLargeTextures)
                {
                    cubeFaceSize = 1024;
                }

                if (CaptureVideo && dumpFrameParams.Dome)
                {
                    cubeFaceSize = 2048;
                }



                if (usingLargeTextures != Properties.Settings.Default.LargeDomeTextures)
                {
                    refreshDomeTextures = true;
                }

                if (currentCubeFaceSize != cubeFaceSize)
                {
                    refreshDomeTextures = true;
                }

                if (refreshDomeTextures)
                {
                    usingLargeTextures = Properties.Settings.Default.LargeDomeTextures;
                    for (var face = 0; face < 5; face++)
                    {
                        if (domeCube[face] != null)
                        {
                            domeCube[face].Dispose();
                            GC.SuppressFinalize(domeCube[face]);
                            domeCube[face] = null;
                        }
                    }
                    if (domeZbuffer != null)
                    {
                        domeZbuffer.Dispose();
                        GC.SuppressFinalize(domeZbuffer);
                        domeZbuffer = null;
                    }
                    if (domeCubeFaceMultisampled != null)
                    {
                        domeCubeFaceMultisampled.Dispose();
                        GC.SuppressFinalize(domeCubeFaceMultisampled);
                        domeCubeFaceMultisampled = null;
                    }
                }


                // Ensure that the dome depth/stencil buffer matches our requirements
                if (domeZbuffer != null)
                {
                    if (domeZbuffer.Width != cubeFaceSize || domeZbuffer.Height != cubeFaceSize)
                    {
                        domeZbuffer.Dispose();
                        GC.SuppressFinalize(domeZbuffer);
                        domeZbuffer = null;
                    }
                }

                if (domeZbuffer == null)
                {
                    domeZbuffer = new DepthBuffer(cubeFaceSize, cubeFaceSize);
                }

                if (domeCubeFaceMultisampled == null && RenderContext11.MultiSampleCount > 1)
                {
                    domeCubeFaceMultisampled = new RenderTargetTexture(cubeFaceSize, cubeFaceSize);
                }

                for (var face = 0; face < 5; face++)
                {
                    if (domeCube[face] == null)
                    {
                        domeCube[face] = new RenderTargetTexture(cubeFaceSize, cubeFaceSize, 1);
                        currentCubeFaceSize = cubeFaceSize;
                        refreshDomeTextures = false;
                    }

                    if (RenderContext11.MultiSampleCount > 1)
                    {
                        // When MSAA is enabled, we render each face to the same multisampled render target,
                        // then resolve to a different texture for each face. This saves memory and works around
                        // the fact that multisample textures are not permitted to have mipmaps.
                        RenderFrame(domeCubeFaceMultisampled, domeZbuffer, (RenderTypes)face);
                        RenderContext11.PrepDevice.ImmediateContext.ResolveSubresource(domeCubeFaceMultisampled.RenderTexture.Texture, 0,
                                                                                       domeCube[face].RenderTexture.Texture, 0,
                                                                                       RenderContext11.DefaultColorFormat);
                    }
                    else
                    {
                        RenderFrame(domeCube[face], domeZbuffer, (RenderTypes)face);
                    }
                    RenderContext11.PrepDevice.ImmediateContext.GenerateMips(domeCube[face].RenderTexture.ResourceView);
                }

                if (Properties.Settings.Default.DomeTypeIndex > 0)
                {
                    RenderWarpedFisheye();
                }
                else
                {
                    if (CaptureVideo && dumpFrameParams.Dome)
                    {
                        if (!dumpFrameParams.WaitDownload || TileCache.QueuePercent == 100)
                        {
                            RenderDomeMaster();
                        }
                    }
                    RenderFisheye(false);
                }

            }
            else if (config.UseDistrotionAndBlend)
            {
                if (undistorted == null)
                {
                    undistorted = new RenderTargetTexture(config.Width, config.Height);
                }


                // Ensure that the dome depth/stencil buffer matches our requirements
                if (domeZbuffer != null)
                {
                    if (domeZbuffer.Width != config.Width || domeZbuffer.Height != config.Height)
                    {
                        domeZbuffer.Dispose();
                        GC.SuppressFinalize(domeZbuffer);
                        domeZbuffer = null;
                    }
                }

                if (domeZbuffer == null)
                {
                    domeZbuffer = new DepthBuffer(config.Width, config.Height);

                }


                // * If there's no multisampling, draw directly into the undistorted texture
                // * When multisampling is on, draw into an intermediate buffer, then resolve 
                //   it into the undistorted texture
                RenderFrame(undistorted, domeZbuffer, RenderTypes.Normal);

                RenderDistort();
            }
            else if (Properties.Settings.Default.FlatScreenWarp)
            {
                if (undistorted == null)
                {
                    undistorted = new RenderTargetTexture(ViewWidth, renderWindow.ClientRectangle.Height);
                }

                if (domeZbuffer != null)
                {
                    if (domeZbuffer.Width != ViewWidth || domeZbuffer.Height != renderWindow.ClientRectangle.Height)
                    {
                        domeZbuffer.Dispose();
                        GC.SuppressFinalize(domeZbuffer);
                        domeZbuffer = null;
                    }
                }

                if (domeZbuffer == null)
                {
                    domeZbuffer = new DepthBuffer(ViewWidth, renderWindow.ClientRectangle.Height);

                }


                RenderFrame(undistorted, domeZbuffer, RenderTypes.Normal);
                RenderFlatDistort();

            }
            else
            {
                if (renderWindow.ClientSize.Height != RenderContext11.DisplayViewport.Height ||
                            renderWindow.ClientSize.Width != RenderContext11.DisplayViewport.Width)
                {
                    RenderContext11.Resize(renderWindow);
                }
                RenderFrame(null, null, RenderTypes.Normal);
            }

            UpdateStats();

            lastRender = HiResTimer.TickCount;

            if (CaptureVideo)
            {
                if (!dumpFrameParams.WaitDownload || TileCache.QueuePercent == 100)
                {
                    if (!dumpFrameParams.Dome)
                    {
                        var ticksa = HiResTimer.TickCount;
                        SaveFrame();
                    }
                    SpaceTimeController.NextFrame();
                }
                if (SpaceTimeController.DoneDumping())
                {
                    SpaceTimeController.CancelFrameDump = false;
                    DomeFrameDumping();
                }
            }

            if (Tile.fastLoadAutoReset)
            {
                Tile.fastLoad = false;
                Tile.fastLoadAutoReset = false;
            }

        }

        public void UpdateMover(IViewMover mover)
        {
            var newCam = mover.CurrentPosition;

            if (viewCamera.Opacity != newCam.Opacity)
            {
                if (contextPanel != null)
                {
                    contextPanel.studyOpacity.Value = (int)newCam.Opacity;
                }

            }
            viewCamera = targetViewCamera = newCam;



            if (Space && Settings.Active.GalacticMode)
            {
                var gPoint = Coordinates.J2000toGalactic(newCam.RA * 15, newCam.Dec);

                targetAlt = alt = gPoint[1];
                targetAz = az = gPoint[0];
            }
            else if (Space && Settings.Active.LocalHorizonMode)
            {
                var currentAltAz = Coordinates.EquitorialToHorizon(Coordinates.FromRaDec(newCam.RA, newCam.Dec), SpaceTimeController.Location, SpaceTimeController.Now);

                targetAlt = alt = currentAltAz.Alt;
                targetAz = az = currentAltAz.Az;
            }

            if (mover.Complete)
            {
                targetViewCamera = viewCamera = newCam;
                MainWindow.Mover = null;
                //Todo Notify interested parties that move is complete

                NotifyMoveComplete();
            }
        }


        private void RenderFlatDistort()
        {
            if (warpTexture == null)
            {
                warpTexture = new RenderTargetTexture(2048, 2048);
            }

            SetupMatricesFisheye();

            if (warpIndexBuffer == null)
            {
                CreateWarpVertexBuffer();
            }

            RenderContext11.SetDisplayRenderTargets();

            RenderContext11.ClearRenderTarget(SharpDX.Color.Black);
            RenderContext11.devContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            RenderContext11.BlendMode = BlendMode.None;
            RenderContext11.setRasterizerState(TriangleCullMode.Off);
            var mat = (RenderContext11.World * RenderContext11.View * RenderContext11.Projection).Matrix11;
            mat.Transpose();

            WarpOutputShader.MatWVP = mat;
            WarpOutputShader.Use(RenderContext11.devContext, true);

            RenderContext11.SetIndexBuffer(warpIndexBuffer);
            RenderContext11.SetVertexBuffer(warpVertexBuffer);

            RenderContext11.devContext.PixelShader.SetShaderResource(0, undistorted.RenderTexture.ResourceView);
            RenderContext11.devContext.DrawIndexed(warpIndexBuffer.Count, 0, 0);

            PresentFrame11(false);

        }

        private void DomeFrameDumping()
        {
            SpaceTimeController.FrameDumping = false;
            CaptureVideo = false;
            if (domeMasterTexture != null)
            {
                domeMasterTexture.Dispose();
                GC.SuppressFinalize(domeMasterTexture);
                domeMasterTexture = null;
            }
            TourEdit.PauseTour();
        }

        private void SaveFrame()
        {
            RenderContext11.SaveBackBuffer(dumpFrameParams.Name.Replace(".", string.Format("_{0:0000}.", SpaceTimeController.CurrentFrameNumber)), ImageFileFormat.Png);
        }



        double NetZoomRate;
        private void UpdateNetControlState()
        {
            var factor = lastFrameTime / (1.0 / 60.0);



            if (Math.Abs(NetZoomRate) > 4)
            {
                ZoomFactor = TargetZoom = ZoomFactor * (1 + (NetZoomRate / 8000) * factor);

                if (ZoomFactor > ZoomMax)
                {
                    ZoomFactor = TargetZoom = ZoomMax;
                }

                if (ZoomFactor < ZoomMin)
                {
                    ZoomFactor = TargetZoom = ZoomMin;
                }
            }


            return;


        }

        double lastFisheyAngle;

        private void RenderFisheye(bool forTexture)
        {

            if (!forTexture)
            {
                SetupMatricesFisheye();
                RenderContext11.SetDisplayRenderTargets();
            }


            RenderContext11.ClearRenderTarget(SharpDX.Color.Black);

            if (domeVertexBuffer == null || lastFisheyAngle != Properties.Settings.Default.FisheyeAngle)
            {
                lastFisheyAngle = Properties.Settings.Default.FisheyeAngle;

                domeVertexBuffer = new PositionColorTexturedVertexBuffer11[5];
                domeIndexBuffer = new IndexBuffer11[5];

                for (var face = 0; face < 5; face++)
                {
                    CreateDomeFaceVertexBuffer(face);
                }
            }


            RenderContext11.devContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            RenderContext11.BlendMode = BlendMode.None;
            RenderContext11.setRasterizerState(TriangleCullMode.Off);
            var mat = (RenderContext11.World * RenderContext11.View * RenderContext11.Projection).Matrix11;
            mat.Transpose();

            WarpOutputShader.MatWVP = mat;
            WarpOutputShader.Use(RenderContext11.devContext, true);


            for (var face = 0; face < 5; face++)
            {
                RenderContext11.SetIndexBuffer(domeIndexBuffer[face]);
                RenderContext11.SetVertexBuffer(domeVertexBuffer[face]);
                RenderContext11.devContext.PixelShader.SetShaderResource(0, domeCube[face].RenderTexture.ResourceView);
                RenderContext11.devContext.DrawIndexed(domeIndexBuffer[face].Count, 0, 0);
            }

            PresentFrame11(forTexture);

        }

        RenderTargetTexture domeMasterTexture;
        private void RenderDomeMaster()
        {

            if (domeMasterTexture == null)
            {
                domeMasterTexture = new RenderTargetTexture(dumpFrameParams.Width, dumpFrameParams.Height, 1);
            }

            RenderContext11.DepthStencilMode = DepthStencilMode.Off;

            RenderContext11.SetOffscreenRenderTargets(domeMasterTexture, null);

            RenderContext11.ClearRenderTarget(SharpDX.Color.Black);

            SetupMatricesWarpFisheye(1f);

            if (domeVertexBuffer == null)
            {
                domeVertexBuffer = new PositionColorTexturedVertexBuffer11[5];
                domeIndexBuffer = new IndexBuffer11[5];

                for (var face = 0; face < 5; face++)
                {
                    CreateDomeFaceVertexBuffer(face);
                }
            }

            RenderContext11.devContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            RenderContext11.BlendMode = BlendMode.None;
            RenderContext11.setRasterizerState(TriangleCullMode.Off);
            RenderContext11.DepthStencilMode = DepthStencilMode.Off;

            var mat = (RenderContext11.World * RenderContext11.View * RenderContext11.Projection).Matrix11;
            mat.Transpose();

            WarpOutputShader.MatWVP = mat;
            WarpOutputShader.Use(RenderContext11.devContext, true);



            for (var face = 0; face < 5; face++)
            {
                RenderContext11.SetIndexBuffer(domeIndexBuffer[face]);
                RenderContext11.SetVertexBuffer(domeVertexBuffer[face]);
                RenderContext11.devContext.PixelShader.SetShaderResource(0, domeCube[face].RenderTexture.ResourceView);
                RenderContext11.devContext.DrawIndexed(domeIndexBuffer[face].Count, 0, 0);
            }

            Texture2D.ToFile(RenderContext11.devContext, domeMasterTexture.RenderTexture.Texture, ImageFileFormat.Png, dumpFrameParams.Name.Replace(".", string.Format("_{0:0000}.", SpaceTimeController.CurrentFrameNumber)));

        }

        RenderTargetTexture warpTexture;
        RenderTargetTexture warpTextureMSAA;
        private void RenderWarpedFisheye()
        {
            if (warpTexture == null)
            {
                warpTexture = new RenderTargetTexture(2048, 2048, 1);
                if (RenderContext11.MultiSampleCount > 1)
                {
                    warpTextureMSAA = new RenderTargetTexture(2048, 2048);
                }
            }

            // If MSAA is enabled, render to an MSAA target and perform a resolve to a non-MSAA texture.
            // Otherwise, render directly to the non-MSAA texture
            var warpRenderTarget = warpTextureMSAA != null ? warpTextureMSAA : warpTexture;

            SetupMatricesWarpFisheye(1);
            RenderContext11.SetOffscreenRenderTargets(warpRenderTarget, null);
            RenderContext11.DepthStencilMode = DepthStencilMode.Off;
            RenderFisheye(true);

            if (warpTextureMSAA != null)
            {
                RenderContext11.Device.ImmediateContext.ResolveSubresource(warpTextureMSAA.RenderTexture.Texture, 0, warpTexture.RenderTexture.Texture, 0, RenderContext11.DefaultColorFormat);
            }
            RenderContext11.Device.ImmediateContext.GenerateMips(warpTexture.RenderTexture.ResourceView);

            SetupMatricesWarpFisheye(ViewWidth / (float)renderWindow.ClientRectangle.Height);

            if (warpIndexBuffer == null)
            {
                CreateWarpVertexBuffer();
            }


            RenderContext11.SetDisplayRenderTargets();

            RenderContext11.ClearRenderTarget(SharpDX.Color.Black);
            RenderContext11.devContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            RenderContext11.BlendMode = BlendMode.None;
            RenderContext11.setRasterizerState(TriangleCullMode.Off);
            var mat = (RenderContext11.World * RenderContext11.View * RenderContext11.Projection).Matrix11;

            mat.Transpose();

            WarpOutputShader.MatWVP = mat;
            WarpOutputShader.Use(RenderContext11.devContext, true);

            RenderContext11.SetIndexBuffer(warpIndexBuffer);
            RenderContext11.SetVertexBuffer(warpVertexBuffer);
            RenderContext11.devContext.PixelShader.SetShaderResource(0, warpTexture.RenderTexture.ResourceView);
            RenderContext11.devContext.DrawIndexed(warpIndexBuffer.Count, 0, 0);

            PresentFrame11(false);

        }


        // Stereo buffers
        RenderTargetTexture leftEye;
        RenderTargetTexture rightEye;
        RenderTargetTexture stereoRenderTexture;

        // Distortion buffers
        RenderTargetTexture undistorted;

        // Full-dome buffers
        RenderTargetTexture domeCubeFaceMultisampled;
        readonly RenderTargetTexture[] domeCube = new RenderTargetTexture[5];
        DepthBuffer domeZbuffer;

        public enum StereoModes { Off, AnaglyphRedCyan, AnaglyphYellowBlue, AnaglyphMagentaGreen, CrossEyed, SideBySide, InterlineEven, InterlineOdd, OculusRift, Right, Left };

        public StereoModes StereoMode = StereoModes.Off;

        enum RenderTypes { DomeFront, DomeRight, DomeUp, DomeLeft, DomeBack, Normal, RightEye, LeftEye };
        static RenderTypes CurrentRenderType = RenderTypes.Normal;

        SphereTest sphere = null;

        IImageSet milkyWayBackground;
        IImageSet cmbBackground;

        private void RenderFrame(RenderTargetTexture targetTexture, DepthBuffer depthBuffer, RenderTypes renderType)
        {
            CurrentRenderType = renderType;

            var offscreenRender = targetTexture != null;

            Tile.deepestLevel = 0;
 
            try
            {
                if (offscreenRender)
                {
                    RenderContext11.SetOffscreenRenderTargets(targetTexture, depthBuffer);
                }
                else
                {
                    RenderContext11.SetDisplayRenderTargets();
                }

                //Clear the backbuffer to a black color 

                RenderContext11.ClearRenderTarget(new SharpDX.Color(SkyColor.R, SkyColor.G, SkyColor.B, SkyColor.A));



                RenderContext11.RenderType = CurrentImageSet.DataSetType;

                RenderContext11.BlendMode = BlendMode.Alpha;
                if (CurrentImageSet.DataSetType == ImageSetType.Sandbox)
                {
                    // Start Sandbox mode
                    RenderContext11.SunPosition = LayerManager.GetPrimarySandboxLight();
                    RenderContext11.SunlightColor = LayerManager.GetPrimarySandboxLightColor();

                    RenderContext11.ReflectedLightColor = Color.Black;
                    RenderContext11.HemisphereLightColor = Color.Black;

                    SkyColor = Color.Black;
                    if ((int)SolarSystemTrack < (int)SolarSystemObjects.Custom)
                    {
                        var radius = Planets.GetAdjustedPlanetRadius((int)SolarSystemTrack);
                        var distance = SolarSystemCameraDistance;
                        var camAngle = fovLocal;
                        var distrad = distance / (radius * Math.Tan(.5 * camAngle));
                        if (distrad < 1)
                        {
                            planetFovWidth = Math.Asin(distrad);
                        }
                        else
                        {
                            planetFovWidth = Math.PI;
                        }
                    }
                    else
                    {
                        planetFovWidth = Math.PI;
                    }


                    SetupMatricesSolarSystem11(false, renderType);

 
                    var matLocal = RenderContext11.World;
                    matLocal.Multiply(Matrix3d.Translation(-viewCamera.ViewTarget));
                    RenderContext11.World = matLocal;

                    RenderContext11.WorldBase = RenderContext11.World;
                    RenderContext11.WorldBaseNonRotating = RenderContext11.World;
                    RenderContext11.NominalRadius = 1;

                    MainWindow.MakeFrustum();

                    var zoom = MainWindow.ZoomFactor;

                    LayerManager.Draw(RenderContext11, 1.0f, false, "Sandbox", true, false);

                    if ((SolarSystemMode) && label != null && !TourPlayer.Playing)
                    {
                        label.Draw(RenderContext11, true);
                    }

                    RenderContext11.setRasterizerState(TriangleCullMode.Off);
                    // end Sandbox Mode
                }
                else if (CurrentImageSet.DataSetType == ImageSetType.SolarSystem)
                {



                    {
                        SkyColor = Color.Black;
                        if ((int)SolarSystemTrack < (int)SolarSystemObjects.Custom)
                        {
                            var radius = Planets.GetAdjustedPlanetRadius((int)SolarSystemTrack);
                            var distance = SolarSystemCameraDistance;
                            var camAngle = fovLocal;
                            var distrad = distance / (radius * Math.Tan(.5 * camAngle));
                            if (distrad < 1)
                            {
                                planetFovWidth = Math.Asin(distrad);
                            }
                            else
                            {
                                planetFovWidth = Math.PI;
                            }
                        }
                        else
                        {
                            planetFovWidth = Math.PI;
                        }


                        if (trackingObject == null)
                        {
                            trackingObject = Search.FindCatalogObjectExact("Sun");
                        }

                        SetupMatricesSolarSystem11(true, renderType);



                        var skyOpacity = 1.0f - Planets.CalculateSkyBrightnessFactor(RenderContext11.View, viewCamera.ViewTarget);
                        if (float.IsNaN(skyOpacity))
                        {
                            skyOpacity = 0f;
                        }

                        var zoom = MainWindow.ZoomFactor;
                        var milkyWayBlend = (float)Math.Min(1, Math.Max(0, (Math.Log(zoom) - 8.4)) / 4.2);
                        var milkyWayBlendIn = (float)Math.Min(1, Math.Max(0, (Math.Log(zoom) - 17.9)) / 2.3);


                        if (Properties.Settings.Default.SolarSystemMilkyWay.State)
                        {
                            if (milkyWayBlend < 1) // Solar System mode Milky Way background
                            {
                                if (milkyWayBackground == null)
                                {
                                    milkyWayBackground = GetImagesetByName("Digitized Sky Survey (Color)");
                                }

                                if (milkyWayBackground != null)
                                {
                                    var c = ((1 - milkyWayBlend)) / 4;
                                    var matOldMW = RenderContext11.World;
                                    var matLocalMW = RenderContext11.World;
                                    matLocalMW.Multiply(Matrix3d.Scaling(100000, 100000, 100000));
                                    matLocalMW.Multiply(Matrix3d.RotationX(-23.5 / 180 * Math.PI));
                                    matLocalMW.Multiply(Matrix3d.RotationY(Math.PI));
                                    matLocalMW.Multiply(Matrix3d.Translation(cameraOffset));
                                    RenderContext11.World = matLocalMW;
                                    RenderContext11.WorldBase = matLocalMW;
                                    MainWindow.MakeFrustum();

                                    RenderContext11.SetupBasicEffect(BasicEffect.TextureColorOpacity, 1, Color.White);
                                    RenderContext11.DepthStencilMode = DepthStencilMode.Off;
                                    DrawTiledSphere(milkyWayBackground, c * Properties.Settings.Default.SolarSystemMilkyWay.Opacity, Color.FromArgb(255, 255, 255, 255));
                                    RenderContext11.World = matOldMW;
                                    RenderContext11.WorldBase = matOldMW;
                                    RenderContext11.DepthStencilMode = DepthStencilMode.ZReadWrite;
                                }
                            }
                        }

                        // CMB

                        var cmbBlend = (float)Math.Min(1, Math.Max(0, (Math.Log(zoom) - 33)) / 2.3);


                        var cmbLog = Math.Log(zoom);

                        if (Properties.Settings.Default.SolarSystemCMB.State)
                        {
                            if (cmbBlend > 0) // Solar System mode Milky Way background
                            {
                                if (cmbBackground == null)
                                {
                                    cmbBackground = GetImagesetByName("Planck CMB");
                                }

                                if (cmbBackground != null)
                                {
                                    var c = ((cmbBlend)) / 16;
                                    var matOldMW = RenderContext11.World;
                                    var matLocalMW = RenderContext11.World;
  
                                    matLocalMW.Multiply(Matrix3d.Scaling(2.9090248982E+15, 2.9090248982E+15, 2.9090248982E+15));
                                    matLocalMW.Multiply(Matrix3d.RotationX(-23.5 / 180 * Math.PI));
                                    matLocalMW.Multiply(Matrix3d.RotationY(Math.PI));

                                    RenderContext11.World = matLocalMW;
                                    RenderContext11.WorldBase = matLocalMW;
                                    MainWindow.MakeFrustum();

                                    RenderContext11.SetupBasicEffect(BasicEffect.TextureColorOpacity, 1, Color.White);

                                    RenderContext11.DepthStencilMode = DepthStencilMode.Off;
                                    DrawTiledSphere(cmbBackground, c * Properties.Settings.Default.SolarSystemCMB.Opacity, Color.FromArgb(255, 255, 255, 255));
                                    RenderContext11.World = matOldMW;
                                    RenderContext11.WorldBase = matOldMW;
                                    RenderContext11.DepthStencilMode = DepthStencilMode.ZReadWrite;
                                }
                            }
                        }




                        {
                            var matOld = RenderContext11.World;

                            var matLocal = RenderContext11.World;
                            matLocal.Multiply(Matrix3d.Translation(viewCamera.ViewTarget));
                            RenderContext11.World = matLocal;
                            MainWindow.MakeFrustum();

                            if (Properties.Settings.Default.SolarSystemCosmos.State)
                            {
                                RenderContext11.DepthStencilMode = DepthStencilMode.Off;
                                Grids.DrawCosmos3D(RenderContext11, Properties.Settings.Default.SolarSystemCosmos.Opacity * skyOpacity);
                                RenderContext11.DepthStencilMode = DepthStencilMode.ZReadWrite;
                            }

                            if (true)
                            {
                                RenderContext11.DepthStencilMode = DepthStencilMode.Off;

                                Grids.DrawCustomCosmos3D(RenderContext11, skyOpacity);

                                RenderContext11.DepthStencilMode = DepthStencilMode.ZReadWrite;
                            }


                            if (Properties.Settings.Default.SolarSystemMilkyWay.State && milkyWayBlendIn > 0)
                            {
                                Grids.DrawGalaxy3D(RenderContext11, Properties.Settings.Default.SolarSystemMilkyWay.Opacity * skyOpacity * milkyWayBlendIn);
                            }


                            if (Properties.Settings.Default.SolarSystemStars.State)
                            {
                                Grids.DrawStars3D(RenderContext11, Properties.Settings.Default.SolarSystemStars.Opacity * skyOpacity);
                            }

                                         
                            LayerManager.Draw(RenderContext11, 1.0f, true, "Sky", true, false);

                            RenderContext11.World = matOld;
                            MainWindow.MakeFrustum();
                        }


                        if (SolarSystemCameraDistance < 15000)
                        {
                            SetupMatricesSolarSystem11(false, renderType);


                            if (Properties.Settings.Default.SolarSystemMinorPlanets.State)
                            {
                                MinorPlanets.DrawMPC3D(RenderContext11, Properties.Settings.Default.SolarSystemMinorPlanets.Opacity, viewCamera.ViewTarget);
                            }

                            Planets.DrawPlanets3D(RenderContext11, Properties.Settings.Default.SolarSystemPlanets.Opacity, viewCamera.ViewTarget);
                        }

                        var p = Math.Log(zoom);
                        var d = (180 / SolarSystemCameraDistance) * 100; 

                        var sunAtDistance = (float)Math.Min(1, Math.Max(0, (Math.Log(zoom) - 7.5)) / 3);

                        if (sunAtDistance > 0)
                        {
                            Planets.DrawPointPlanet(RenderContext11, new Vector3d(0, 0, 0), (float)d * sunAtDistance, Color.FromArgb(192, 191, 128), false, 1);
                        }

                        if ((SolarSystemMode) && label != null && !TourPlayer.Playing)
                        {
                            label.Draw(RenderContext11, true);
                        }
                    }

                    RenderContext11.setRasterizerState(TriangleCullMode.Off);
                }
                else
                {

                    if (CurrentImageSet.DataSetType == ImageSetType.Panorama || CurrentImageSet.DataSetType == ImageSetType.Sky)
                    {
                        SkyColor = Color.Black;

                        if ((int)renderType < 5)
                        {
                            SetupMatricesSpaceDome(false, renderType);
                        }
                        else
                        {
                            SetupMatricesSpace11(ZoomFactor, renderType);
                        }
                        RenderContext11.DepthStencilMode = DepthStencilMode.Off;
                    }
                    else
                    {

                        if (Settings.DomeView)
                        {
                            SetupMatricesLandDome(renderType);
                        }
                        else
                        {
                            SetupMatricesLand11(renderType);
                        }
                        RenderContext11.DepthStencilMode = DepthStencilMode.ZReadWrite;
                    }

                    ComputeViewParameters(CurrentImageSet);

                    // Update Context pane
                    CurrentViewCorners = new Coordinates[] 
                    {
                        GetCoordinatesForScreenPoint(0, 0),
                        GetCoordinatesForScreenPoint(ViewWidth, 0),
                        GetCoordinatesForScreenPoint(ViewWidth, renderWindow.ClientRectangle.Height),
                        GetCoordinatesForScreenPoint(0, renderWindow.ClientRectangle.Height) 
                    };

                    var temp = GetCoordinatesForScreenPoint(ViewWidth / 2, renderWindow.ClientRectangle.Height / 2);

                    if (contextPanel != null && ((int)renderType > 4 || renderType == RenderTypes.DomeFront))
                    {
                        contextPanel.SetViewRect(CurrentViewCorners);
                    }
                    UpdateKmlViewInfo();

                    if (KmlMarkers != null)
                    {
                        KmlMarkers.ClearGroundOverlays();
                    }

                    var referenceFrame = GetCurrentReferenceFrame();


                    if (PlanetLike || Space)
                    {
                        LayerManager.PreDraw(RenderContext11, 1.0f, Space, referenceFrame, true);
                    }

                    if (Properties.Settings.Default.EarthCutawayView.State && !Space && CurrentImageSet.DataSetType == ImageSetType.Earth)
                    {
                        Grids.DrawEarthStructure(RenderContext11, 1f);
                    }

                    RenderContext11.SetupBasicEffect(BasicEffect.TextureColorOpacity, 1, Color.White);

                    if (KmlMarkers != null)
                    {
                        KmlMarkers.SetupGroundOverlays(RenderContext11);
                    }

                    if (PlanetLike)
                    {
                        RenderContext11.setRasterizerState(TriangleCullMode.Off);
                    }

                    // Call DrawTiledSphere instead of PaintLayerFull, because PaintLayerFull
                    // will reset ground layer state
                    DrawTiledSphere(CurrentImageSet, 1.0f, Color.White);


                    if (imageStackVisible)
                    {
                        foreach (ImageSet set in ImageStackList)
                        {
                            PaintLayerFull11(set, StudyOpacity);
                        }
                    }

                    if (studyImageset != null)
                    {
                        if (studyImageset.DataSetType != CurrentImageSet.DataSetType)
                        {
                            StudyImageset = null;
                        }
                        else
                        {
                            PaintLayerFull11(studyImageset, StudyOpacity);
                        }
                    }


                    if (previewImageset != null && PreviewBlend.State)
                    {
                        if (previewImageset.DataSetType != CurrentImageSet.DataSetType)
                        {
                            previewImageset = null;
                        }
                        else
                        {
                            PaintLayerFull11(previewImageset, PreviewBlend.Opacity * 100.0f);
                        }
                    }
                    else
                    {
                        PreviewBlend.State = false;
                        previewImageset = null;
                    }


                    if (Space && (CurrentImageSet.Name == "Plotted Sky"))
                    {

                        Grids.DrawStars(RenderContext11, 1f);
                    }

                    if (Space && Properties.Settings.Default.ShowSolarSystem.State)
                    {
                        Planets.DrawPlanets(RenderContext11, Properties.Settings.Default.ShowSolarSystem.Opacity);
                    }


                    if (PlanetLike || Space)
                    {
                        if (!Space)
                        {
                            //todo fix this for other planets..
                            var angle = Coordinates.MstFromUTC2(SpaceTimeController.Now, 0) / 180.0 * Math.PI;
                            RenderContext11.WorldBaseNonRotating = Matrix3d.RotationY(angle) * RenderContext11.WorldBase;
                            RenderContext11.NominalRadius = CurrentImageSet.MeanRadius;
                        }
                        else
                        {
                            RenderContext11.WorldBaseNonRotating = RenderContext11.World;
                            RenderContext11.NominalRadius = CurrentImageSet.MeanRadius;
                            RenderContext11.DepthStencilMode = DepthStencilMode.Off;
                        }

                        LayerManager.Draw(RenderContext11, 1.0f, Space, referenceFrame, true, Space);
                    }

                    if (Space && !hemisphereView && Settings.Active.LocalHorizonMode && !Settings.DomeView && !ProjectorServer)
                    {
                        Grids.DrawHorizon(RenderContext11, 1f);
                    }

                    if (Settings.Active.ShowClouds && !Space && CurrentImageSet.DataSetType == ImageSetType.Earth)
                    {
                        DrawClouds();
                    }


                    // Draw Field of view indicator

                    if (Settings.Active.ShowFieldOfView)
                    {
                        fovBlend.TargetState = true;
                    }
                    else
                    {
                        fovBlend.TargetState = false;
                    }

                    if (fovBlend.State)
                    {
                        if (fov != null && Space)
                        {
                            fov.Draw3D(RenderContext11, fovBlend.Opacity, RA, Dec);
                        }
                    }

                    if (label != null && !TourPlayer.Playing)
                    {
                        label.Draw(RenderContext11, PlanetLike);
                    }

                    if (ShowKmlMarkers && KmlMarkers != null)
                    {
                        KmlMarkers.DrawLabels(RenderContext11);
                    }



                    // End Planet & space
                }

                if (uiController != null)
                {
                    {
                        uiController.Render(this);
                    }
                }

                if (videoOverlay != null)
                {
                    if ((int)renderType < 5)
                    {
                        SetupMatricesVideoOverlayDome(false, renderType);
                    }
                    else
                    {
                        SetupMatricesVideoOverlay(ZoomFactor);
                    }
                    var mode = RenderContext11.DepthStencilMode = DepthStencilMode.Off;
                    PaintLayerFull11(videoOverlay, 100f);
                    RenderContext11.DepthStencilMode = mode;
                }

                if (measuringDrag && measureLines != null)
                {
                    measureLines.DrawLines(RenderContext11, 1.0f, Color.Yellow);

                }

                if (Properties.Settings.Default.ShowCrosshairs && !TourPlayer.Playing && renderType == RenderTypes.Normal)
                {
                    var aspect = RenderContext11.ViewPort.Height / RenderContext11.ViewPort.Width;


                    crossHairPoints[0].X = .01f * aspect;
                    crossHairPoints[1].X = -.01f * aspect;
                    crossHairPoints[0].Y = 0;
                    crossHairPoints[1].Y = 0;
                    crossHairPoints[0].Z = .9f;
                    crossHairPoints[1].Z = .9f;
                    crossHairPoints[0].W = 1f;
                    crossHairPoints[1].W = 1f;
                    crossHairPoints[0].Color = Color.White;
                    crossHairPoints[1].Color = Color.White;

                    crossHairPoints[2].X = 0;
                    crossHairPoints[3].X = 0;
                    crossHairPoints[2].Y = -.01f;
                    crossHairPoints[3].Y = .01f;
                    crossHairPoints[2].Z = .9f;
                    crossHairPoints[3].Z = .9f;
                    crossHairPoints[2].W = 1f;
                    crossHairPoints[3].W = 1f;
                    crossHairPoints[2].Color = Color.White;
                    crossHairPoints[3].Color = Color.White;

                    Sprite2d.DrawLines(RenderContext11, crossHairPoints, 4, Matrix.OrthoLH(1f, 1f, 1, -1), false);

                }


                if (Properties.Settings.Default.ShowTouchControls && (!TourPlayer.Playing || mover == null) && ( renderType == RenderTypes.Normal || renderType == RenderTypes.LeftEye || renderType == RenderTypes.RightEye) && !rift )
                {
                    DrawTouchControls();
                }


                DrawKinectUI();

                SetupMatricesAltAz();
                Reticle.DrawAll(RenderContext11);


            }
            catch (Exception e)
            {
                if (Logging) { WriteLogMessage("RenderFrame: Exception"); }
                if (offscreenRender)
                {
                    throw e;
                }
            }
            finally
            {
                if (offscreenRender)
                {

                    RenderContext11.SetDisplayRenderTargets();
                }
            }

            PresentFrame11(offscreenRender);
        }

        readonly PositionColoredTextured[] crossHairPoints = new PositionColoredTextured[4];

        private string GetCurrentReferenceFrame()
        {
            if (!string.IsNullOrEmpty(CurrentImageSet.ReferenceFrame))
            {
                return CurrentImageSet.ReferenceFrame;
            }
            if (CurrentImageSet.DataSetType == ImageSetType.Earth)
            {
                return "Earth";
            }
            if (CurrentImageSet.Name == "Visible Imagery" && CurrentImageSet.Url.ToLower().Contains("mars"))
            {

                CurrentImageSet.ReferenceFrame = "Mars";
                return CurrentImageSet.ReferenceFrame;
            }

            if (CurrentImageSet.DataSetType == ImageSetType.Planet)
            {
                foreach (var name in Enum.GetNames(typeof(SolarSystemObjects)))
                {
                    if (CurrentImageSet.Name.ToLower().Contains(name.ToLower()))
                    {
                        CurrentImageSet.ReferenceFrame = name;
                        return name;
                    }
                }
            }
            if (CurrentImageSet.DataSetType == ImageSetType.Sky)
            {
                return "Sky";
            }
            return "";
        }

        private static Matrix3d bias = Matrix3d.Scaling(.5f, -.5f, .5f) * Matrix3d.Translation(.5f, .5f, .5f);

        bool flush = true;

        Query query;

        private void PresentFrame11(bool renderToTexture)
        {
            // Update the screen
            if (!renderToTexture)
            {
                FadeFrame();
                NetControl.WaitForNetworkSync();
                RenderContext11.Present(Properties.Settings.Default.FrameSync);

                if (flush)
                {
                    RenderContext11.devContext.Flush();
                    var qd = new QueryDescription();

                    qd.Type = QueryType.Event;


                    query = new Query(RenderContext11.Device, qd);

                    RenderContext11.devContext.End(query);

                    var result = false;
                    var retVal = false;
                    while (!result && !retVal)
                    {
                        var ds = RenderContext11.devContext.GetData(query); 

                        result = ds.ReadBoolean();
                        ds.Close();
                        ds.Dispose();

                    }
                    query.Dispose();
                }
            }

        }

        readonly PositionColoredTextured[] fadePoints = new PositionColoredTextured[4];
        public BlendState Fader = new BlendState(true, 2000);

        private bool crossFadeFrame;

        private Texture11 crossFadeTexture;
        public bool CrossFadeFrame
        {
            set
            {
                if (value && crossFadeFrame != value)
                {
                    if (crossFadeTexture != null)
                    {
                        crossFadeTexture.Dispose();
                    }
                    crossFadeTexture = RenderContext11.GetScreenTexture();

                }
                crossFadeFrame = value;

                if (!value)
                {
                    if (crossFadeTexture != null)
                    {
                        crossFadeTexture.Dispose();
                        crossFadeTexture = null;
                    }
                }
            }
            get
            {
                return crossFadeFrame;
            }
        }

        private void FadeFrame()
        {

            var sp = Settings.Active.GetSetting(StockSkyOverlayTypes.FadeToBlack);

            

            if ((sp.Opacity > 0) && !(Settings.MasterController && Properties.Settings.Default.FadeRemoteOnly))
            {
                var color = Color.FromArgb(255 - UiTools.Gamma(255 - (int)(sp.Opacity * 255), 1 / 2.2f), Color.Black);

                if (!(sp.Opacity > 0))
                {
                    color = Color.FromArgb(255 - UiTools.Gamma(255 - (int)(sp.Opacity * 255), 1 / 2.2f), Color.Black);
                }


                if (crossFadeFrame)
                {
                    color = Color.FromArgb(UiTools.Gamma((int)((sp.Opacity) * 255), 1 / 2.2f), Color.White);
                }
                else
                {
                    if (crossFadeTexture != null)
                    {
                        crossFadeTexture.Dispose();
                        crossFadeTexture = null;
                    }
                }

                fadePoints[0].X = 0;
                fadePoints[0].Y = renderWindow.Height;
                fadePoints[0].Z = 0;
                fadePoints[0].Tu = 0;
                fadePoints[0].Tv = 1;
                fadePoints[0].W = 1;
                fadePoints[0].Color = color;
                fadePoints[1].X = 0;
                fadePoints[1].Y = 0;
                fadePoints[1].Z = 0;
                fadePoints[1].Tu = 0;
                fadePoints[1].Tv = 0;
                fadePoints[1].W = 1;
                fadePoints[1].Color = color;
                fadePoints[2].X = renderWindow.Width;
                fadePoints[2].Y = renderWindow.Height;
                fadePoints[2].Z = 0;
                fadePoints[2].Tu = 1;
                fadePoints[2].Tv = 1;
                fadePoints[2].W = 1;
                fadePoints[2].Color = color;
                fadePoints[3].X = renderWindow.Width;
                fadePoints[3].Y = 0;
                fadePoints[3].Z = 0;
                fadePoints[3].Tu = 1;
                fadePoints[3].Tv = 0;
                fadePoints[3].W = 1;
                fadePoints[3].Color = color;

                Sprite2d.DrawForScreen(RenderContext11, fadePoints, 4, crossFadeTexture, PrimitiveTopology.TriangleStrip);
            }
        }

        TansformedPositionTexturedVertexBuffer11 ScreenVertexBuffer;

        void RenderSteroPairAnaglyph(RenderTargetTexture left, RenderTargetTexture right)
        {

            RenderContext11.SetDisplayRenderTargets();
            RenderContext11.ClearRenderTarget(SharpDX.Color.Black);


            if (ScreenVertexBuffer == null)
            {
                if (ScreenVertexBuffer != null)
                {
                    ScreenVertexBuffer.Dispose();
                    GC.SuppressFinalize(ScreenVertexBuffer);
                    ScreenVertexBuffer = null;
                }

                ScreenVertexBuffer = new TansformedPositionTexturedVertexBuffer11(6, RenderContext11.PrepDevice);

                //PreTransformed
                var quad = (TansformedPositionTextured[])ScreenVertexBuffer.Lock(0, 0);


                quad[0].Position = new Vector4(-1, 1, .9f, 1);
                quad[0].Tu = 0;
                quad[0].Tv = 0;

                quad[1].Position = new Vector4(1, 1, .9f, 1);
                quad[1].Tu = 1;
                quad[1].Tv = 0;

                quad[2].Position = new Vector4(-1, -1, .9f, 1);
                quad[2].Tu = 0;
                quad[2].Tv = 1;

                quad[3].Position = new Vector4(-1, -1, .9f, 1);
                quad[3].Tu = 0;
                quad[3].Tv = 1;

                quad[4].Position = new Vector4(1, 1, .9f, 1);
                quad[4].Tu = 1;
                quad[4].Tv = 0;

                quad[5].Position = new Vector4(1, -1, .9f, 1);
                quad[5].Tu = 1;
                quad[5].Tv = 1;

                ScreenVertexBuffer.Unlock();

            }
            var leftEyeColor = Color.Red;
            var rightEyeColor = Color.Cyan;



            if (StereoMode == StereoModes.AnaglyphYellowBlue)
            {
                leftEyeColor = Color.Yellow;
                rightEyeColor = Color.Blue;

            }

            if (StereoMode == StereoModes.AnaglyphMagentaGreen)
            {
                leftEyeColor = Color.Yellow;
                rightEyeColor = Color.Blue;

            }


            RenderContext11.SetVertexBuffer(ScreenVertexBuffer);

            RenderContext11.devContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            RenderContext11.BlendMode = BlendMode.Additive;

            RenderContext11.setRasterizerState(TriangleCullMode.Off);


            //Left Eye

            AnaglyphStereoShader.Color = new Color4(leftEyeColor.R / 255f, leftEyeColor.G / 255f, leftEyeColor.B / 255f, leftEyeColor.A / 255f);
            AnaglyphStereoShader.Use(RenderContext11.devContext);


            RenderContext11.devContext.PixelShader.SetShaderResource(0, leftEye.RenderTexture.ResourceView);


            RenderContext11.devContext.Draw(ScreenVertexBuffer.Count, 0);

            //Right Eye
            RenderContext11.devContext.PixelShader.SetShaderResource(0, rightEye.RenderTexture.ResourceView);
            AnaglyphStereoShader.Color = new Color4(rightEyeColor.R / 255f, rightEyeColor.G / 255f, rightEyeColor.B / 255f, rightEyeColor.A / 255f);
            AnaglyphStereoShader.Use(RenderContext11.devContext);
            RenderContext11.devContext.Draw(ScreenVertexBuffer.Count, 0);

            RenderContext11.BlendMode = BlendMode.Alpha;

            PresentFrame11(false);
        }

        void RenderSteroPairInterline(RenderTargetTexture left, RenderTargetTexture right)
        {
            if (renderWindow.ClientSize.Height != RenderContext11.DisplayViewport.Height ||
                renderWindow.ClientSize.Width != RenderContext11.DisplayViewport.Width)
            {
                RenderContext11.Resize(renderWindow);
            }

            RenderContext11.SetDisplayRenderTargets();
            RenderContext11.ClearRenderTarget(SharpDX.Color.Black);


            if (ScreenVertexBuffer == null)
            {
                if (ScreenVertexBuffer != null)
                {
                    ScreenVertexBuffer.Dispose();
                    GC.SuppressFinalize(ScreenVertexBuffer);
                    ScreenVertexBuffer = null;
                }

                ScreenVertexBuffer = new TansformedPositionTexturedVertexBuffer11(6, RenderContext11.PrepDevice);

                //PreTransformed
                var quad = (TansformedPositionTextured[])ScreenVertexBuffer.Lock(0, 0);


                quad[0].Position = new Vector4(-1, 1, .9f, 1);
                quad[0].Tu = 0;
                quad[0].Tv = 0;

                quad[1].Position = new Vector4(1, 1, .9f, 1);
                quad[1].Tu = 1;
                quad[1].Tv = 0;

                quad[2].Position = new Vector4(-1, -1, .9f, 1);
                quad[2].Tu = 0;
                quad[2].Tv = 1;

                quad[3].Position = new Vector4(-1, -1, .9f, 1);
                quad[3].Tu = 0;
                quad[3].Tv = 1;

                quad[4].Position = new Vector4(1, 1, .9f, 1);
                quad[4].Tu = 1;
                quad[4].Tv = 0;

                quad[5].Position = new Vector4(1, -1, .9f, 1);
                quad[5].Tu = 1;
                quad[5].Tv = 1;

                ScreenVertexBuffer.Unlock();

            }


            RenderContext11.SetVertexBuffer(ScreenVertexBuffer);

            RenderContext11.devContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            RenderContext11.BlendMode = BlendMode.Additive;

            RenderContext11.setRasterizerState(TriangleCullMode.Off);


            InterlineStereoShader.Lines = renderWindow.Height;
            InterlineStereoShader.Odd = StereoMode == StereoModes.InterlineOdd ? 1.0f : 0.0f;

            InterlineStereoShader.Use(RenderContext11.devContext);

            RenderContext11.devContext.PixelShader.SetShaderResource(0, right.RenderTexture.ResourceView);
            RenderContext11.devContext.PixelShader.SetShaderResource(1, left.RenderTexture.ResourceView);

            RenderContext11.devContext.Draw(ScreenVertexBuffer.Count, 0);

            RenderContext11.BlendMode = BlendMode.Alpha;

            PresentFrame11(false);
        }
        void RenderSteroPairSideBySide(RenderTargetTexture left, RenderTargetTexture right)
        {
            RenderContext11.SetDisplayRenderTargets();
            RenderContext11.ClearRenderTarget(SharpDX.Color.Black);


            if (ScreenVertexBuffer == null)
            {
                if (ScreenVertexBuffer != null)
                {
                    ScreenVertexBuffer.Dispose();
                    GC.SuppressFinalize(ScreenVertexBuffer);
                    ScreenVertexBuffer = null;
                }

                ScreenVertexBuffer = new TansformedPositionTexturedVertexBuffer11(6, RenderContext11.PrepDevice);

                //PreTransformed
                var quad = (TansformedPositionTextured[])ScreenVertexBuffer.Lock(0, 0);


                quad[0].Position = new Vector4(-1, 1, .9f, 1);
                quad[0].Tu = 0;
                quad[0].Tv = 0;

                quad[1].Position = new Vector4(1, 1, .9f, 1);
                quad[1].Tu = 1;
                quad[1].Tv = 0;

                quad[2].Position = new Vector4(-1, -1, .9f, 1);
                quad[2].Tu = 0;
                quad[2].Tv = 1;

                quad[3].Position = new Vector4(-1, -1, .9f, 1);
                quad[3].Tu = 0;
                quad[3].Tv = 1;

                quad[4].Position = new Vector4(1, 1, .9f, 1);
                quad[4].Tu = 1;
                quad[4].Tv = 0;

                quad[5].Position = new Vector4(1, -1, .9f, 1);
                quad[5].Tu = 1;
                quad[5].Tv = 1;

                ScreenVertexBuffer.Unlock();

            }


            RenderContext11.SetVertexBuffer(ScreenVertexBuffer);

            RenderContext11.devContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            RenderContext11.BlendMode = BlendMode.Additive;

            RenderContext11.setRasterizerState(TriangleCullMode.Off);

            SideBySideStereoShader.Use(RenderContext11.devContext);


            RenderContext11.devContext.PixelShader.SetShaderResource(0, right.RenderTexture.ResourceView);
            RenderContext11.devContext.PixelShader.SetShaderResource(1, left.RenderTexture.ResourceView);


            RenderContext11.devContext.Draw(ScreenVertexBuffer.Count, 0);

            RenderContext11.BlendMode = BlendMode.Alpha;

            PresentFrame11(false);
        }

        void RenderSteroOculurRift(RenderTargetTexture left, RenderTargetTexture right)
        {
            RenderContext11.SetDisplayRenderTargets();
            RenderContext11.ClearRenderTarget(SharpDX.Color.Black);


            if (ScreenVertexBuffer == null)
            {
                if (ScreenVertexBuffer != null)
                {
                    ScreenVertexBuffer.Dispose();
                    GC.SuppressFinalize(ScreenVertexBuffer);
                    ScreenVertexBuffer = null;
                }

                ScreenVertexBuffer = new TansformedPositionTexturedVertexBuffer11(6, RenderContext11.PrepDevice);

                //PreTransformed
                var quad = (TansformedPositionTextured[])ScreenVertexBuffer.Lock(0, 0);


                quad[0].Position = new Vector4(-1, 1, .9f, 1);
                quad[0].Tu = 0;
                quad[0].Tv = 0;

                quad[1].Position = new Vector4(1, 1, .9f, 1);
                quad[1].Tu = 1;
                quad[1].Tv = 0;

                quad[2].Position = new Vector4(-1, -1, .9f, 1);
                quad[2].Tu = 0;
                quad[2].Tv = 1;

                quad[3].Position = new Vector4(-1, -1, .9f, 1);
                quad[3].Tu = 0;
                quad[3].Tv = 1;

                quad[4].Position = new Vector4(1, 1, .9f, 1);
                quad[4].Tu = 1;
                quad[4].Tv = 0;

                quad[5].Position = new Vector4(1, -1, .9f, 1);
                quad[5].Tu = 1;
                quad[5].Tv = 1;

                ScreenVertexBuffer.Unlock();

            }


            RenderContext11.SetVertexBuffer(ScreenVertexBuffer);

            RenderContext11.devContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            RenderContext11.BlendMode = BlendMode.Additive;

            RenderContext11.setRasterizerState(TriangleCullMode.Off);


            var lensOffset = riftInfo.LensSeparationDistance * 0.5f;
            var lensShift = riftInfo.HScreenSize * 0.25f - lensOffset;
            var lensViewportShift = 4.0f * lensShift / riftInfo.HScreenSize;
            var XCenterOffset = lensViewportShift;


            // Convert fit value to distortion-centered coordinates before fit radius
            // calculation.
            var stereoAspect = ViewWidth / (float)ViewHeight;
            var dx = -1 - XCenterOffset;
            var dy = 0 / stereoAspect;
            var fitRadius = (float)Math.Sqrt(dx * dx + dy * dy);
            var Scale = DistortionFn(fitRadius) / fitRadius;
            Scale = .5f;
            RiftStereoShader.constants.Scale = new Vector2(Scale * 1f, Scale * stereoAspect);
            RiftStereoShader.constants.ScaleIn = new Vector2(2.0f, 2f * (1f / stereoAspect));
            RiftStereoShader.constants.LensCenterLeft = new Vector2(.5f + (iod / 2), .5f);
            RiftStereoShader.constants.LensCenterRight = new Vector2(.5f - (iod / 2), .5f);
            RiftStereoShader.constants.HmdWarpParam = new Vector4(riftInfo.DistortionK0, riftInfo.DistortionK1, riftInfo.DistortionK2, riftInfo.DistortionK3);
            RiftStereoShader.Use(RenderContext11.devContext);


            RenderContext11.devContext.PixelShader.SetShaderResource(0, left.RenderTexture.ResourceView);
            RenderContext11.devContext.PixelShader.SetShaderResource(1, right.RenderTexture.ResourceView);


            RenderContext11.devContext.Draw(ScreenVertexBuffer.Count, 0);

            RenderContext11.BlendMode = BlendMode.Alpha;

            PresentFrame11(false);
        }

        float DistortionFn(float r)
        {
            var rsq = r * r;
            var scale = r * (riftInfo.DistortionK0 + riftInfo.DistortionK1 * rsq + riftInfo.DistortionK2 * rsq * rsq + riftInfo.DistortionK3 * rsq * rsq * rsq);
            return scale;
        }

        public void DrawClouds()
        {
            var cloudTexture = Planets.CloudTexture;
            if (cloudTexture != null)
            {
                RenderContext11.SetupBasicEffect(BasicEffect.TextureColorOpacity, 1.0f, Color.White);

                RenderContext11.MainTexture = cloudTexture;

                var savedWorld = RenderContext11.World;
                var cloudScale = 1.0 + Planets.EarthCloudHeightMeters / 6378100.0;
                RenderContext11.World = Matrix3d.Scaling(cloudScale, cloudScale, cloudScale) * RenderContext11.World;

                RenderContext11.setRasterizerState(TriangleCullMode.CullCounterClockwise);
                RenderContext11.DepthStencilMode = DepthStencilMode.ZReadOnly;
                RenderContext11.BlendMode = BlendMode.Alpha;

                Planets.DrawFixedResolutionSphere(RenderContext11, 0);

                RenderContext11.World = savedWorld;
                RenderContext11.setRasterizerState(TriangleCullMode.CullClockwise);
                RenderContext11.DepthStencilMode = DepthStencilMode.ZReadWrite;
                RenderContext11.BlendMode = BlendMode.None;
            }
        }

        private void ClampZoomValues()
        {
            if (ZoomFactor > ZoomMax)
            {
                ZoomFactor = ZoomMax;
            }
            if (ZoomFactor < ZoomMin)
            {
                ZoomFactor = ZoomMin;
            }
            if (TargetZoom > ZoomMax)
            {
                TargetZoom = ZoomMax;
            }
            if (TargetZoom < ZoomMin)
            {
                TargetZoom = ZoomMin;
            }
        }
        double lastFrameTime = .1;
        Int64 lastRenderTickCount;

        bool CaptureVideo;
        public bool ScreenShot = false;
        public VideoOut videoOut = null;
        Bitmap bmpVideoOut = null;

        int lastTimeSyncFrame;

        private void UpdateNetworkStatus()
        {
            ignoreChanges = true;
            NetControl.SetSettingsBelndStates();
            NetControl.SetSolarSystemsSettingsBlendStates();

            if (lastTimeSyncFrame != NetControl.currnetSyncFrame)
            {
                SpaceTimeController.Now = NetControl.now;
            }

            if (Properties.Settings.Default.DomeTilt != NetControl.domeTilt)
            {
                Properties.Settings.Default.DomeTilt = NetControl.domeTilt;
                MainWindow.Config.DomeTilt = (float)NetControl.domeTilt;
            }

            if (Properties.Settings.Default.DomeAngle != NetControl.domeAngle)
            {
                Properties.Settings.Default.DomeAngle = (float)NetControl.domeAngle;
                MainWindow.Config.DomeAngle = (float)NetControl.domeAngle;
            }

            lastTimeSyncFrame = NetControl.currnetSyncFrame;
            SpaceTimeController.TimeRate = NetControl.timeRate;
            SpaceTimeController.SyncToClock = NetControl.timeRate != 0;
            SpaceTimeController.Altitude = NetControl.altitude;
            SpaceTimeController.Location = Coordinates.FromLatLng(NetControl.loclat, NetControl.loclng);
            SetLocation(NetControl.lat, NetControl.lng, NetControl.zoom, NetControl.cameraRotate, NetControl.cameraAngle, NetControl.foregroundImageSetHash,
                NetControl.backgroundImageSetHash, NetControl.blendOpacity, NetControl.runSetup, NetControl.flush, NetControl.target, NetControl.targetPoint, NetControl.solarSystemScale, NetControl.TrackingFrame);

            viewCamera.DomeAlt = NetControl.domeAlt;
            viewCamera.DomeAz = NetControl.domeAz;

            var currentVersion = Properties.Settings.Default.ColSettingsVersion;

            Properties.Settings.Default.ReticleAlt = NetControl.reticleAlt;
            Properties.Settings.Default.ReticleAz = NetControl.reticleAz;

            Properties.Settings.Default.ConstellationFiguresFilter.SetBits(NetControl.figuresFilter);
            Properties.Settings.Default.ConstellationNamesFilter.SetBits(NetControl.namesFilter);
            Properties.Settings.Default.ConstellationBoundariesFilter.SetBits(NetControl.bounderiesFilter);
            Properties.Settings.Default.ConstellationArtFilter.SetBits(NetControl.artFilter);

            if (NetControl.ColorVersionNumber != currentVersion)
            {
                NetControl.GetColorSettings();
            }
            ignoreChanges = false;
        }

        private void LoadCurrentFigures()
        {
            constellationsFigures = new Constellations("Default Figures", "http://www.worldwidetelescope.org/data/figures.txt", false, false);
        }

        private void NotifyMoveComplete()
        {
            if (KmlAutoRefresh)
            {
                MyPlaces.UpdateNetworkLinks(KmlViewInfo);
            }
            SendMoveComplete();
        }

        private void UpdateKmlViewInfo()
        {
            KmlViewInfo.bboxNorth = Math.Max(CurrentViewCorners[0].Dec, CurrentViewCorners[1].Dec);
            KmlViewInfo.bboxSouth = Math.Min(CurrentViewCorners[2].Dec, CurrentViewCorners[3].Dec);
            KmlViewInfo.bboxEast = Math.Max(CurrentViewCorners[1].Lng, CurrentViewCorners[2].Lng);
            KmlViewInfo.bboxWest = Math.Min(CurrentViewCorners[0].Lng, CurrentViewCorners[2].Lng);
            KmlViewInfo.viewMoving = false;
            KmlViewInfo.viewJustStopped = true;
            //todo Fill in completely from camera parameters..
            if (KmlMarkers == null)
            {
                KmlMarkers = new KmlLabels();
            }
        }


        private InputLayout layout = null;
        public void PaintLayerFull11(IImageSet layer, float opacityPercentage)
        {
            var opacity = opacityPercentage / 100.0f;
            RenderContext11.SetupBasicEffect(BasicEffect.TextureColorOpacity, opacity, Color.White);
            DrawTiledSphere(layer, opacity, Color.White);
        }

        public void PaintLayerFullTint11(IImageSet layer, float opacityPercentage, Color color)
        {
            var opacity = opacityPercentage / 100.0f;
            RenderContext11.SetupBasicEffect(BasicEffect.TextureColorOpacity, opacity, color);
            DrawTiledSphere(layer, opacity, color);
        }

        float brightness = .5f;
        float contrast = .5f;

        public void DrawTiledSphere(IImageSet layer, float opacity, Color color)
        {
            var maxX = GetTilesXForLevel(layer, layer.BaseLevel);
            var maxY = GetTilesYForLevel(layer, layer.BaseLevel);

            // Set up the input assembler; match the layout of the current shader
            RenderContext11.Device.ImmediateContext.InputAssembler.InputLayout = RenderContext11.Shader.inputLayout(PlanetShader11.StandardVertexLayout.PositionNormalTex2);
            RenderContext11.devContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            Tile.Viewport = RenderContext11.ViewPort;
            Tile.wvp = (RenderContext11.WorldBase * RenderContext11.ViewBase * RenderContext11.Projection).Matrix11;

            RenderContext11.PreDraw();

            if (layer.DataSetType == ImageSetType.Sky)
            {
                HDRPixelShader.constants.a = brightness;
                HDRPixelShader.constants.b = contrast;
                HDRPixelShader.constants.opacity = opacity;
                HDRPixelShader.constants.tint = new Color4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);

                HDRPixelShader.Use(RenderContext11.devContext);
            }

            if (Properties.Settings.Default.EarthCutawayView.State && !SolarSystemMode && !Space && layer.DataSetType == ImageSetType.Earth)
            {

                RenderContext11.SetupBasicEffect(BasicEffect.TextureColorOpacity, opacity, Color.White);
                if (layer.Projection == ProjectionType.Toast)
                {

                    var tile = TileCache.GetTile(layer.BaseLevel + 1, 1, 0, layer, null);
                    if (tile != null && tile.IsTileInFrustum(RenderContext11.Frustum))
                    {
                        tile.Draw3D(RenderContext11, opacity, null);
                    }
                    tile = TileCache.GetTile(layer.BaseLevel + 1, 1, 1, layer, null);
                    if (tile != null && tile.IsTileInFrustum(RenderContext11.Frustum))
                    {
                        tile.Draw3D(RenderContext11, opacity, null);
                    }
                    tile = TileCache.GetTile(layer.BaseLevel + 1, 0, 0, layer, null);
                    if (tile != null && tile.IsTileInFrustum(RenderContext11.Frustum))
                    {
                        tile.Draw3D(RenderContext11, opacity, null);
                    }
                    //Show if partially transparent
                    if (Properties.Settings.Default.EarthCutawayView.Opacity != 1.0)
                    {
                        tile = TileCache.GetTile(layer.BaseLevel + 1, 0, 1, layer, null);
                        if (tile != null && tile.IsTileInFrustum(RenderContext11.Frustum))
                        {
                            RenderContext11.SetupBasicEffect(BasicEffect.TextureColorOpacity, 1.0f - Properties.Settings.Default.EarthCutawayView.Opacity, Color.White);
                            tile.Draw3D(RenderContext11, opacity, null);
                        }
                    }
                }
                else
                {
                    for (var x = 0; x < maxX; x++)
                    {
                        for (var y = 0; y < maxY; y++)
                        {

                            if (!(x == 1))
                            {
                                var tile = TileCache.GetTile(layer.BaseLevel, x, y, layer, null);
                                if (tile != null && tile.IsTileInFrustum(RenderContext11.Frustum))
                                {
                                    tile.Draw3D(RenderContext11, opacity, null);
                                }
                            }
                            else
                            {
                                var tile = TileCache.GetTile(layer.BaseLevel + 1, x * 2 + 1, y * 2, layer, null);
                                if (tile != null && tile.IsTileInFrustum(RenderContext11.Frustum))
                                {
                                    tile.Draw3D(RenderContext11, opacity, null);
                                }
                                tile = TileCache.GetTile(layer.BaseLevel + 1, x * 2 + 1, y * 2 + 1, layer, null);
                                if (tile != null && tile.IsTileInFrustum(RenderContext11.Frustum))
                                {
                                    tile.Draw3D(RenderContext11, opacity, null);
                                }
                                if (Properties.Settings.Default.EarthCutawayView.Opacity != 1.0 && !Space)
                                {
                                    RenderContext11.SetupBasicEffect(BasicEffect.TextureColorOpacity, 1.0f - Properties.Settings.Default.EarthCutawayView.Opacity, Color.White);

                                    tile = TileCache.GetTile(layer.BaseLevel + 1, x * 2, y * 2, layer, null);
                                    if (tile != null && tile.IsTileInFrustum(RenderContext11.Frustum))
                                    {
                                        tile.Draw3D(RenderContext11, 1, null);
                                    }
                                    tile = TileCache.GetTile(layer.BaseLevel + 1, x * 2, y * 2 + 1, layer, null);
                                    if (tile != null && tile.IsTileInFrustum(RenderContext11.Frustum))
                                    {
                                        tile.Draw3D(RenderContext11, 1, null);
                                    }
                                    RenderContext11.SetupBasicEffect(BasicEffect.TextureColorOpacity, 1.0f, Color.White);

                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (var x = 0; x < maxX; x++)
                {
                    for (var y = 0; y < maxY; y++)
                    {
                        var tile = TileCache.GetTile(layer.BaseLevel, x, y, layer, null);
                        if (tile != null && tile.IsTileInFrustum(RenderContext11.Frustum))
                        {
                            tile.Draw3D(RenderContext11, opacity, null);
                        }
                    }
                }
            }

            RenderContext11.DisableEffect();

            RenderContext11.LocalCenter = Vector3d.Empty;
        }


        internal static int GetTilesYForLevel(IImageSet layer, int level)
        {
            int maxY;

            switch (layer.Projection)
            {
                case ProjectionType.Mercator:
                    maxY = (int)Math.Pow(2, level);
                    break;
                case ProjectionType.Equirectangular:
                    maxY = (int)(Math.Pow(2, level) * (180 / layer.BaseTileDegrees) + .9);
                    break;
                case ProjectionType.Tangent:
                    maxY = (int)Math.Pow(2, level);

                    break;
                case ProjectionType.SkyImage:
                case ProjectionType.Spherical:
                    maxY = 1;
                    break;
                default:
                    maxY = (int)Math.Pow(2, level);
                    break;
            }
            return maxY;
        }

        internal static int GetTilesXForLevel(IImageSet layer, int level)
        {
            int maxX;
            switch (layer.Projection)
            {
                case ProjectionType.Plotted:
                case ProjectionType.Toast:
                    maxX = (int)Math.Pow(2, level);
                    break;
                case ProjectionType.Mercator:
                    maxX = (int)Math.Pow(2, level) * (int)(layer.BaseTileDegrees / 360.0);
                    break;
                case ProjectionType.Equirectangular:
                    maxX = (int)(Math.Pow(2, level) * (layer.BaseTileDegrees / 90.0));
                    maxX = (int)(Math.Pow(2, level) * (360 / layer.BaseTileDegrees) + .9);

                    break;

                case ProjectionType.Tangent:
                    if (layer.WidthFactor == 1)
                    {
                        maxX = (int)Math.Pow(2, level) * 2;
                    }
                    else
                    {
                        maxX = (int)Math.Pow(2, level);
                    }
                    break;
                case ProjectionType.SkyImage:
                    maxX = 1;
                    break;
                case ProjectionType.Spherical:
                    maxX = 1;
                    break;
                default:
                    maxX = (int)Math.Pow(2, level) * 2;
                    break;
            }


            return maxX;
        }

        protected override void OnPaint(PaintEventArgs e)
        {

        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {

        }

        private void Earth3d_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                if (tourEdit != null)
                {
                    tourEdit.PauseTour();
                }

                TileCache.PurgeQueue();
                TileCache.ClearCache();
            }
            if (RenderContext11 != null)
            {
                RenderContext11.Resize(renderWindow);
            }

            if (ReadyToRender)
            {

                SetAppMode(currentMode);
            }
        }

        private bool IsPaused()
        {
            return ((WindowState == FormWindowState.Minimized) || !Visible || pause);
        }

        public static bool renderingVideo = false;


        static private void RegisterKnownFileTypes()
        {
            RegisterFileType(".wtt", Language.GetLocalizedText(87, "WorldWide Telescope Tour"));
            RegisterFileType(".wtml", Language.GetLocalizedText(88, "WorldWide Telescope Media List"));
  
        }

        static private void RegisterFileType(string extension, string friendlyName)
        {
            var root = Registry.ClassesRoot;

            var extensionKey = root.OpenSubKey(extension);
            if (extensionKey != null)
            {
                return;
            }
            var runningAssembly = Assembly.GetExecutingAssembly();

            extensionKey = root.CreateSubKey(extension);

            extensionKey.SetValue("", "WorldWideTelescope" + extension);

            var typeInfoKey = root.CreateSubKey("WorldWideTelescope" + extension);
            typeInfoKey.SetValue("", friendlyName);
            var icon = typeInfoKey.CreateSubKey("DefaultIcon");
            icon.SetValue("", runningAssembly.Location + ",0");

            var shellSubkey = typeInfoKey.CreateSubKey("shell");

            // Create a subkey for the "Open" verb
            var openSubKey = shellSubkey.CreateSubKey("Open");

            openSubKey.SetValue("", "&Play Tour");


            var cmdSubkey = openSubKey.CreateSubKey("command");
            cmdSubkey.SetValue("", runningAssembly.Location + " %1");

        }


        static private bool ShouldAutoUpdate()
        {
            var root = Registry.CurrentUser;

            var wwtKey = root.OpenSubKey("Software\\Microsoft\\WorldWide Telescope");
            if (wwtKey == null)
            {
                return true;
            }

            return ((int)wwtKey.GetValue("AutoUpdates", 1)) != 0;
        }

        static private string GetIDCrlPath()
        {
            var root = Registry.LocalMachine;

            var wwtKey = root.OpenSubKey(@"Software\Microsoft\IdentityCRL");
            if (wwtKey == null)
            {
                return "";
            }

            return (string)wwtKey.GetValue("TargetDir", "");
        }

        /*
         * Cross process block
         * 
         * 
         * 
         */

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, IntPtr lParam);

        [DllImport("user32")]
        public static extern uint RegisterWindowMessage(string message);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, IntPtr lParam);

        public static uint AlertMessage;

        private static void SendWMData(IntPtr hwnd, string argData)
        {

            File.WriteAllText(Properties.Settings.Default.CahceDirectory + @"\launchfile.txt", argData);
            PostMessage(hwnd, AlertMessage, 0, IntPtr.Zero);

        }
        public static bool closeWelcome = false;

        public bool CloseWelcome
        {
            get { return closeWelcome; }
            set { closeWelcome = value; }
        }

        Message message;
        protected override void WndProc(ref Message m)
        {

            if (myMouse != null)
            {
                message = m;
                myMouse.ProcessMessage(message);
            }


            base.WndProc(ref m);
        }
        // end

        protected override void OnResize(EventArgs e)
        {
            //if (RenderContext.Device != null)
            //{

            //    RestoreDevice();
            //}
            try
            {
                base.OnResize(e);
            }
            catch
            {
            }
        }


        public static string launchTourFile = "";

        [DllImport("User32.dll")]

        public static extern int ShowWindowAsync(IntPtr hWnd, int swCommand);
        enum ShowWindowConstants
        {
            SW_HIDE = 0, SW_SHOWNORMAL = 1, SW_NORMAL = 1, SW_SHOWMINIMIZED = 2, SW_SHOWMAXIMIZED = 3, SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4, SW_SHOW = 5, SW_MINIMIZE = 6, SW_SHOWMINNOACTIVE = 7, SW_SHOWNA = 8, SW_RESTORE = 9, SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11, SW_MAX = 11
        };

      

        public static bool TouchKiosk = false;
        public static bool NoUi = false;

        public static bool DomeViewer = false;
        static bool DumpShaders;
        public static bool RestartedWithoutTour = false;

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, UInt32 dwFlags);

        [DllImport("user32")]
        public static extern bool ChangeWindowMessageFilter(uint msg, uint flags);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetProcessDPIAware();


        public static void PulseForUpdate()
        {
            Properties.Settings.Default.PulseMeForUpdate = !Properties.Settings.Default.PulseMeForUpdate;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main(string[] args)
        {
 


            var culture = new CultureInfo("en-US", false);
            Thread.CurrentThread.CurrentCulture = culture;
            Application.CurrentCulture = culture;
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
 
            var now = DateTime.Now;
            var singleInstance = true;


            foreach (var arg in args)
            {
             
                if (arg == "-logging")
                {
                    Logging = true;
                }


                if (arg == "-kiosk")
                {
                    TouchKiosk = true;
                }

                if (arg == "-domeviewer")
                {
                    DomeViewer = true;
                    singleInstance = false;
                    HideSplash = true;
                    TileCache.NodeID++;
                }

                if (arg == "-noui")
                {
                    NoUi = true;
                }

                if (arg == "-dumpshaders")
                {
                    DumpShaders = true;
                }

                if (arg.StartsWith("-screen:"))
                {
                    try
                    {
                        TargetScreenId = int.Parse(arg.Substring(arg.LastIndexOf(":") + 1));
                    }
                    catch
                    {
                    }
                }

                if (arg.StartsWith("-detach:"))
                {
                    try
                    {
                        DetachScreenId = int.Parse(arg.Substring(arg.LastIndexOf(":") + 1));
                    }
                    catch
                    {
                    }
                }
            }

            AlertMessage = RegisterWindowMessage("WWT Launch Tour");
            ChangeWindowMessageFilter(AlertMessage, 1);

            if (args.Length > 0)
            {
                if (args[0] == "launch")
                {
                    Process.Start("wwtexplorer.exe");
                    return;
                }
                if (args[0] == "restart")
                {
                    singleInstance = false;
                    HideSplash = true;
                    RestartedWithoutTour = true;
                }

                if (args[0].ToLower().EndsWith(".kml") || args[0].ToLower().EndsWith(".kmz") || args[0].ToLower().EndsWith(".wtt") || args[0].ToLower().EndsWith(".wtml") || args[0].ToLower().EndsWith(".wwtfig") || args[0].ToLower().EndsWith(".wwtl"))
                {
                    if (File.Exists(args[0]))
                    {
                        launchTourFile = args[0];
                    }
                }
            }

            if (singleInstance)
            {
                var RunningProcesses = Process.GetProcessesByName("WWTExplorer");


                if (RunningProcesses.Length > 1)
                {
                    foreach (var p in RunningProcesses)
                    {
                        if (p.Id != Process.GetCurrentProcess().Id)
                        {
                            SendWMData(p.MainWindowHandle, launchTourFile);
                        }
                    }
                    return;
                }
            }

            PulseMe.PulseForUpdate = PulseForUpdate;

            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            CheckDefaultProperties(true);
            defaultWebProxy = WebRequest.DefaultWebProxy;
            UpdateProxySettings();


            Language.CurrentLanguage =
                new Language(Properties.Settings.Default.LanguageName,
                             Properties.Settings.Default.LanguageUrl,
                             Properties.Settings.Default.LanguageCode,
                             Properties.Settings.Default.ExploreRootUrl,
                             Properties.Settings.Default.ImageSetUrl,
                             Properties.Settings.Default.SharedCacheServer);

            CacheProxy.BaseUrl = Properties.Settings.Default.SharedCacheServer;

            try
            {
                if (!CheckForUpdates(false))
                {
                    return;
                }
            }
            catch
            {
            }

            try
            {
                RegisterKnownFileTypes();
            }
            catch
            {
            }


            // Check for failed Startup
            if (CheckStartFlag())
            {
                if (UiTools.ShowMessageBox(Language.GetLocalizedText(688, "WorldWide Telescope failed to complete the last startup attempt. Would you like WorldWide Telescope to attempt an auto reset of the data directory?"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    ResetDataDirectory();
                }
            }
            SetStartFlag();



            using (var frm = new Earth3d())
            {
                //Stopwatch sw = new Stopwatch();
                try
                {
                    frm.Show();
                    frm.Refresh();
                    if (!frm.Created)
                    {
                        return;
                    }
                }
                catch
                {
                    return;
                }


                MainLoop(frm);

            }
            if (LanguageReboot)
            {
                var path = Assembly.GetExecutingAssembly().Location;

                Process.Start(path, "restart");
            }
        }


        private static ApplicationRecoveryCallback crashCallback;

        private static void SetupRecovery()
        {
            crashCallback = CrashCallback;

            var cb = Marshal.GetFunctionPointerForDelegate(crashCallback);

            RegisterApplicationRecoveryCallback(cb, IntPtr.Zero, 6000, 0);
        }


        private static int CrashCallback(IntPtr pvParameter)
        {
            // if (ProjectorServer)
            {

                File.WriteAllText(@"c:\wwtconfig\crashdump.txt", "Unhandled Exception in Current Domain");

                LanguageReboot = true;
                CloseNow = true;
                //     return 0;
            }

            ApplicationRecoveryFinished(true);
            return 0;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (ProjectorServer)
            {

                File.WriteAllText(@"c:\wwtconfig\crashdump.txt", "Unhandled Exception in Current Domain");

                LanguageReboot = true;
                CloseNow = true;
                return;
            }
        }

        public static void MainLoop(Earth3d frm)
        {
            // Hook the application's idle event     
            Application.AddMessageFilter(new DataMessageFilter());
            Application.Idle += OnApplicationIdle;
            Application.Run(frm);

        }

        private static void OnApplicationIdle(object sender, EventArgs e)
        {
            while (AppStillIdle)
            {
                if (CloseNow)
                {
                    MainWindow.Close();
                    CloseNow = false;
                }
                else
                {
                    if (MainWindow != null)
                    {
                        MainWindow.Render();
                    }
                }
            }
        }

        private static bool AppStillIdle
        {
            get
            {
                MessageS msg;
                return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }


        //
        [StructLayout(LayoutKind.Sequential)]
        public struct MessageS
        {
            public IntPtr hWnd;
            public int msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public Point p;
        }

        [SuppressUnmanagedCodeSecurity] // We won't use this maliciously
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out MessageS msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);


        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (ProjectorServer)
            {
                LanguageReboot = true;
                CloseNow = true;

                File.WriteAllText(@"c:\wwtconfig\crashdump.txt", e.Exception.Message + e.Exception.Source + e.Exception.StackTrace);

                return;
            }

            if (starting)
            {
                if (UiTools.ShowMessageBox(Language.GetLocalizedText(689, "WorldWide Telescope encountered an error starting up. Would you like WorldWide Telescope to attempt a reset of the data directory and restart?"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    ResetDataDirectory();
                    ResetStartFlag();
                    Process.Start("wwtexplorer.exe", "restart");
                    Process.GetCurrentProcess().Kill();

                }
            }
            else
            {
                if (UiTools.ShowMessageBox(Language.GetLocalizedText(690, "WorldWide Telescope has encountered an error. Click OK to restart or Cancel to attempt to ignore the error and continue"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Process.Start("wwtexplorer.exe", "restart");
                    Process.GetCurrentProcess().Kill();

                }
            }
        }

        static bool starting = true;
        private static void SetStartFlag()
        {
            var startFlagFilename = string.Format(@"{0}\wwtstartup.flag", Path.GetTempPath());
            File.WriteAllText(startFlagFilename, "Starting");
            starting = true;
        }

        private static void ResetDataDirectory()
        {
            ExtractDataCabinet(true);
        }

        private static void ResetStartFlag()
        {
            var startFlagFilename = string.Format(@"{0}\wwtstartup.flag", Path.GetTempPath());
            File.Delete(startFlagFilename);
            starting = false;
        }

        private static bool CheckStartFlag()
        {
            var startFlagFilename = string.Format(@"{0}\wwtstartup.flag", Path.GetTempPath());

            return File.Exists(startFlagFilename);

        }

        static IWebProxy defaultWebProxy;

        public static void UpdateProxySettings()
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.ProxyServer))
            {
                try
                {
                    var proxyURI = new Uri(String.Format("http://{0}:{1}", Properties.Settings.Default.ProxyServer, Properties.Settings.Default.ProxyPort));
                    var proxy = new WebProxy(proxyURI);
                    proxy.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy;
                    return;
                }
                catch
                {
                }

            }

            WebRequest.DefaultWebProxy = defaultWebProxy;

        }

        static bool resetProperties;
        private static void CheckDefaultProperties(bool checkDataCabinet)
        {
            AppSettings.SettingsBase = Properties.Settings.Default;

            if (Properties.Settings.Default.UpgradeNeeded && !resetProperties)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.SolarSystemCosmos.TargetState = true;
                Properties.Settings.Default.SolarSystemOverlays.TargetState = false;
                Properties.Settings.Default.ImageQuality = 100;
                Properties.Settings.Default.ImageSetUrl = "http://www.worldwidetelescope.org/wwtweb/catalog.aspx?X=ImageSets5";
                Properties.Settings.Default.UpgradeNeeded = false;
            }

            if (string.IsNullOrEmpty(Properties.Settings.Default.CahceDirectory))
            {
                Properties.Settings.Default.CahceDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\WorldWideTelescope\\";
                var tempString = Properties.Settings.Default.CahceDirectory;
            }

            if (Properties.Settings.Default.UserRatingGUID == Guid.Empty)
            {
                Properties.Settings.Default.UserRatingGUID = Guid.NewGuid();
            }
            var extractData = false;

            if (!Directory.Exists(Properties.Settings.Default.CahceDirectory + "data"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.CahceDirectory + "data");
                extractData = true;
            }

            if (!Directory.Exists(Properties.Settings.Default.CahceDirectory + "data\\figures"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.CahceDirectory + "data\\figures");
            }

            if (!Directory.Exists(Properties.Settings.Default.CahceDirectory + "data\\wms"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.CahceDirectory + "data\\wms");
            }

            if (!Directory.Exists(Properties.Settings.Default.CahceDirectory + "data\\figures"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.CahceDirectory + "data\\figures");
            }

            if (!Directory.Exists(Properties.Settings.Default.CahceDirectory + "buttons"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.CahceDirectory + "buttons");
            }

            if (!Directory.Exists(Properties.Settings.Default.CahceDirectory + "thumbnails"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.CahceDirectory + "thumbnails");
            }

            //TODO We need to update this via a flag to force upgrades past v5.1 to work
            if (!File.Exists(Properties.Settings.Default.CahceDirectory + "data\\wwtv5.2.7.txt"))
            {
                extractData = true;
            }

            if (extractData && checkDataCabinet)
            {
                ExtractDataCabinet(false);
            }

            AppSettings.SettingsBase = Properties.Settings.Default;
        }

        private static void ExtractDataCabinet(bool eraseFirst)
        {

            var appdir = Path.GetDirectoryName(Application.ExecutablePath);
            var dataDir = Properties.Settings.Default.CahceDirectory + "data";
            if (eraseFirst)
            {
                if (Directory.Exists(dataDir))
                {
                    Directory.Delete(dataDir, true);
                }
            }
            var filename = appdir + "\\datafiles.cabinet";
            var cab = new FileCabinet(filename, dataDir);
            cab.Extract();
            File.WriteAllText(Properties.Settings.Default.CahceDirectory + "data\\wwtv5.2.7.txt", "WWT Version 5.5.7 installed");
        }

        int mouseDownX;
        int mouseDownY;


        public double CameraRotateTarget
        {
            get { return targetViewCamera.Rotation; }
            set { targetViewCamera.Rotation = value; }
        }

        public double CameraRotate
        {
            get { return viewCamera.Rotation; }
            set { viewCamera.Rotation = value; }
        }

        public double CameraAngle
        {
            get { return viewCamera.Angle; }
            set { viewCamera.Angle = value; }
        }

        public double CameraAngleTarget
        {
            get { return targetViewCamera.Angle; }
            set { targetViewCamera.Angle = value; }
        }

        private int GetLevelForImageSet(IImageSet imageSet)
        {
            return (int)(Math.Log(imageSet.BaseTileDegrees / ZoomFactor, 2) + .01);
        }

        double planetFovWidth = Math.PI;

        private void ComputeViewParameters(IImageSet imageSet)
        {

            MaxLevels = imageSet.Levels - 1;
            baseTileDegrees = imageSet.BaseTileDegrees;


            var level = Math.Log(baseTileDegrees / ZoomFactor, 2) + 2.01F;

            if ((int)level > MaxLevels)
            {
                viewTileLevel = MaxLevels;
            }
            else if (level < 0)
            {
                viewTileLevel = 0;
            }
            else
            {
                viewTileLevel = (int)level;
            }



            tileSizeY = (int)(256.0 * (1.0 * (((baseTileDegrees / ZoomFactor) / Math.Pow(2, viewTileLevel)))));

            tileSizeX = tileSizeY;


            if (TileCache.CurrentLevel != viewTileLevel)
            {
                TileCache.CurrentLevel = viewTileLevel;
            }

            return;
        }

        private int GetTileXFromLng(double lng)
        {
            var tile = ((lng + 180.0F) / (baseTileDegrees / Math.Pow(2, viewTileLevel)));
            if (tile < 0)
            {
                tile = -1;
            }
            return (int)tile;
        }

        private int GetTileYFromLat(double lat)
        {

            return (int)((lat + 90.0) / (baseTileDegrees / (Math.Pow(2, viewTileLevel)))) - 1;
        }

       

        public double GetPixelScaleX(bool mouseRelative)
        {
            var lat = ViewLat;

            if (mouseRelative)
            {
                if (Space && Settings.Active.GalacticMode)
                {
                    var cursor = renderWindow.PointToClient(Cursor.Position);
                    var result = GetCoordinatesForScreenPoint(cursor.X, cursor.Y);

                    var gPoint = Coordinates.J2000toGalactic(result.RA * 15, result.Dec);

                    lat = gPoint[1];
                }
                else if (Space && Settings.Active.LocalHorizonMode)
                {
                    var cursor = renderWindow.PointToClient(Cursor.Position);
                    var currentAltAz = Coordinates.EquitorialToHorizon(GetCoordinatesForScreenPoint(cursor.X, cursor.Y), SpaceTimeController.Location, SpaceTimeController.Now);

                    lat = currentAltAz.Alt;
                }
                else
                {
                    var cursor = renderWindow.PointToClient(Cursor.Position);
                    var result = GetCoordinatesForScreenPoint(cursor.X, cursor.Y);
                    lat = result.Lat;
                }
            }

            if (CurrentImageSet != null && (CurrentImageSet.DataSetType == ImageSetType.Sky || CurrentImageSet.DataSetType == ImageSetType.Panorama || SandboxMode || SolarSystemMode || CurrentImageSet.DataSetType == ImageSetType.Earth || CurrentImageSet.DataSetType == ImageSetType.Planet))
            {
                double cosLat = 1;
                if (ViewLat > 89.9999)
                {
                    cosLat = Math.Cos(89.9999 * RC);
                }
                else
                {
                    cosLat = Math.Cos(lat * RC);

                }

                var zz = (90 - ZoomFactor / 6);
                var zcos = Math.Cos(zz * RC);

                return GetPixelScaleY() / Math.Max(zcos, cosLat);
            }
            return (((baseTileDegrees / Math.Pow(2, viewTileLevel)) / tileSizeX) / 5) / Math.Max(.2, Math.Cos(targetLat));
        }

        public double GetPixelScaleY()
        {
            if (SolarSystemMode)
            {
                if ((int)SolarSystemTrack < (int)SolarSystemObjects.Custom)
                {
                    return Math.Min(.06, 545000 * Math.Tan(Math.PI / 4) * ZoomFactor / renderWindow.ClientRectangle.Height);
                }
                return .06;
            }
            if (CurrentImageSet != null && (CurrentImageSet.DataSetType == ImageSetType.Sky || CurrentImageSet.DataSetType == ImageSetType.Panorama))
            {
                var val = fovAngle / renderWindow.ClientRectangle.Height;

                return val;
            }
            if (SandboxMode)
            {
                return .06;
            }
            return ((baseTileDegrees / Math.Pow(2, viewTileLevel)) / tileSizeY) / 5;
        }

        SimpleLineList11 measureLines;
        Coordinates measureStart;
        Coordinates measureEnd;

        private bool measuringDrag;
        private bool measuring;
        public bool Measuring
        {
            get { return measuring; }
            set
            {
                if (measureLines != null)
                {
                    measureLines.Clear();
                }
                measuring = value;
            }
        }
        private bool dragging;
        private bool spinning;
        private bool angle;
        private bool moved;


        private void MainWndow_MouseDown(object sender, MouseEventArgs e)
        {


        }
        IPlace contextMenuTargetObject;
        private void MainWndow_MouseUp(object sender, MouseEventArgs e)
        {

        }

        void ShowPropertiesForPoint(Point pntCenter)
        {
            if (contextPanel != null)
            {
                // TODO fix this for earth, plantes, panoramas
                var result = GetCoordinatesForScreenPoint(pntCenter.X, pntCenter.Y);
                var constellation = constellationCheck.FindConstellationForPoint(result.RA, result.Dec);
                contextPanel.Constellation = Constellations.FullName(constellation);
                var closetPlace = ContextSearch.FindClosestMatch(constellation, result.RA, result.Dec, ZoomFactor / 1300);

                if (closetPlace == null)
                {
                    closetPlace = new TourPlace(Language.GetLocalizedText(90, "No Object"), result.Dec, result.RA, Classification.Unidentified, constellation, ImageSetType.Sky, -1);
                }

                ShowPropertiesMenu(closetPlace, pntCenter);
            }
        }


        private void ShowObjectResolveMenu(IPlace[] resultList, Point pntShow)
        {
            pntShow = new Point(pntShow.X + 100, pntShow.Y + 100);
            var ResolveMenu = new ContextMenuStrip();

            foreach (var place in resultList)
            {
                var menuItem = new ToolStripMenuItem(place.Name);
                menuItem.Tag = place;
                menuItem.Click += ResolveAmbiguityMenu_Click;
                menuItem.MouseEnter += ResolveAmbiguityMenu_MouseEnter;
                ResolveMenu.Items.Add(menuItem);

            }
            ResolveMenu.Show(pntShow);
        }

        void ResolveAmbiguityMenu_MouseEnter(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;

            var placePicked = (IPlace)menuItem.Tag;

            SetLabelText(placePicked, true);

        }

        void ResolveAmbiguityMenu_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;

            var placePicked = (IPlace)menuItem.Tag;

            ShowPropertiesMenu(placePicked, Cursor.Position);
        }
        bool starRegAdded = false;
        public void ShowContextMenu(IPlace place, Point pntShow, bool forCollection, bool readOnly)
        {
            getSDSSImageToolStripMenuItem.Enabled = IsInSDSSFootprint(place.RA, place.Dec);

            if (place.Name == Language.GetLocalizedText(90, "No Object"))
            {
                lookupOnSEDSToolStripMenuItem.Enabled = false;
                lookupOnWikipediaToolStripMenuItem.Enabled = false;
                publicationsToolStripMenuItem.Enabled = false;
                lookupOnSimbadToolStripMenuItem.Enabled = true;
                lookUpOnSDSSToolStripMenuItem.Enabled = getSDSSImageToolStripMenuItem.Enabled;
            }
            else
            {
                if (place.Name.ToLower().StartsWith("ngc") || place.Name.ToLower().StartsWith("ic") || place.Name.ToLower().StartsWith("m"))
                {
                    lookupOnSEDSToolStripMenuItem.Enabled = true;
                }
                else
                {
                    lookupOnSEDSToolStripMenuItem.Enabled = false;
                }

                getSDSSImageToolStripMenuItem.Enabled = getSDSSImageToolStripMenuItem.Enabled;
                lookupOnWikipediaToolStripMenuItem.Enabled = true;
                publicationsToolStripMenuItem.Enabled = true;
                lookupOnSimbadToolStripMenuItem.Enabled = place.Classification != Classification.SolarSystem;

            }

            // TODO replace this with another way of knowing whats near Maybe popup data near object
            nameToolStripMenuItem.Text = Language.GetLocalizedText(7, "Name:") + " " + place.Name;
            contextMenuTargetObject = place;

            if (AddFigurePointMenuItem == null)
            {
                AddFigurePointMenuItem = new ToolStripMenuItem(Language.GetLocalizedText(91, "Add Point to Constellation Figure"));
                AddFigurePointMenuItem.Click += AddFigurePointMenuItem_Click;
            }

            if (contextMenu.Items.Contains(AddFigurePointMenuItem))
            {
                contextMenu.Items.Remove(AddFigurePointMenuItem);
            }

            if (figureEditor != null)
            {
                contextMenu.Items.Add(AddFigurePointMenuItem);
            }


            removeFromCollectionToolStripMenuItem.Visible = forCollection && !readOnly;
            editToolStripMenuItem.Visible = forCollection && !readOnly;
            removeFromImageCacheToolStripMenuItem.Visible = place.IsImage;
            setAsBackgroundImageryToolStripMenuItem.Visible = place.IsImage;
            setAsForegroundImageryToolStripMenuItem.Visible = place.IsImage;
            addToImageStackToolStripMenuItem.Visible = place.IsImage && imageStackVisible;
            ImagerySeperator.Visible = place.IsImage;

            // override items for communities
            var communitiesPlace = contextMenuTargetObject as Place;
            if (communitiesPlace != null)
            {
                if (communitiesPlace.MSRComponentId > 0)
                {
                    removeFromCollectionToolStripMenuItem.Visible = true;
                }
            }

            contextMenu.Show(this, pntShow.X, pntShow.Y);
            return;
        }

        void starReg_Click(object sender, EventArgs e)
        {
            var url = String.Format("http://www.worldwidetelescope.org/wwtweb/starReg.aspx?ra={0}&dec={1}", Coordinates.FormatDMS(contextMenuTargetObject.RA), Coordinates.FormatDMS(contextMenuTargetObject.Dec));
            WebWindow.OpenUrl(url, false);

        }

        private void ShowPropertiesMenu(IPlace place, Point pntShow)
        {
            ObjectProperties.ShowAt(place, renderWindow.PointToScreen(pntShow));

            return;


        }


        ToolStripMenuItem AddFigurePointMenuItem;

        void AddFigurePointMenuItem_Click(object sender, EventArgs e)
        {
            figureEditor.AddFigurePoint(contextMenuTargetObject);
        }

        private void Earth3d_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        public double RAtoViewLng(double ra)
        {
            return (((180 - ((ra) / 24.0 * 360) - 180) + 540) % 360) - 180;
        }

        Point lastMousePosition = new Point(-1, -1);
        private void MainWndow_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Earth3d_Click(object sender, EventArgs e)
        {

        }

        public Coordinates GetCoordinatesForScreenPoint(int x, int y)
        {
            var result = new Coordinates(0, 0);
            var rect = renderWindow.ClientRectangle;
            Vector3d PickRayOrig;
            Vector3d PickRayDir;
            var pt = new Point(x, y);
            TransformPickPointToWorldSpace(pt, rect.Width, rect.Height, out PickRayOrig, out PickRayDir);
            if (Space)
            {
                result = Coordinates.CartesianToSpherical(PickRayDir);

            }
            else if (PlanetLike)
            {
                var inThere = SphereIntersectRay(PickRayOrig, PickRayDir, out result);

            }

            return result;
        }

        public Coordinates GetCoordinatesForReticle(int id)
        {
            var result = new Coordinates(0, 0);

            if (!Reticle.Reticles.ContainsKey(id))
            {
                return result;
            }
            var ret = Reticle.Reticles[id];


            var pick = Coordinates.RADecTo3d(ret.Az / 15 - 6, ret.Alt, 1);

            var distance = (Math.Min(1, (.5 * (ZoomFactor / 180)))) - 1 + 0.0001;

            var PickRayOrig = new Vector3d(0, 0, distance);

            var mat = WorldMatrix * Matrix3d.RotationX(((config.TotalDomeTilt) / 180 * Math.PI));
            var mat2 = WorldMatrix * Matrix3d.RotationZ(((config.TotalDomeTilt) / 180 * Math.PI));

            mat.Invert();
            mat2.Invert();
            mat.MultiplyVector(ref pick);
            mat2.MultiplyVector(ref PickRayOrig);
            var PickRayDir = pick;
            SphereIntersectRay(PickRayOrig.Vector3, PickRayDir.Vector3, out result);
            return result;
        }


        public void GotoReticlePoint(int id)
        {
            var result = new Coordinates(0, 0);

            if (!Reticle.Reticles.ContainsKey(id))
            {
                return;
            }
            var ret = Reticle.Reticles[id];


            var pick = Coordinates.RADecTo3d(ret.Az / 15 - 6, ret.Alt, 1);

            var distance = (Math.Min(1, (.5 * (ZoomFactor / 180)))) - 1 + 0.0001;

            var PickRayOrig = new Vector3d(0, -distance, 0);

            var mat = WorldMatrix * Matrix3d.RotationX(((config.TotalDomeTilt) / 180 * Math.PI));

            mat.Invert();

            mat.MultiplyVector(ref pick);
            mat.MultiplyVector(ref PickRayOrig);
            var PickRayDir = pick;
            var temp = new Vector3d(PickRayOrig);
            temp.Subtract(MainWindow.viewCamera.ViewTarget);

            var closetPlace = Grids.FindClosestObject(temp, new Vector3d(PickRayDir));

            if (closetPlace != null)
            {
                GotoTarget(closetPlace, false, false, true);
            }
        }

        public bool SphereIntersectRay(Vector3d pickRayOrig, Vector3d pickRayDir, out Coordinates pointCoordinate)
        {
            pointCoordinate = new Coordinates(0, 0);
            double r = 1;
            //Compute A, B and C coefficients
            var a = Vector3d.Dot(pickRayDir, pickRayDir);
            var b = 2 * Vector3d.Dot(pickRayDir, pickRayOrig);
            var c = Vector3d.Dot(pickRayOrig, pickRayOrig) - (r * r);

            //Find discriminant
            var disc = b * b - 4 * a * c;

            // if discriminant is negative there are no real roots, so return 
            // false as ray misses sphere
            if (disc < 0)
            {
                return false;
            }

            // compute q as described above
            var distSqrt = Math.Sqrt(disc);
            double q;
            if (b < 0)
            {
                q = (-b - distSqrt) / 2.0f;
            }
            else
            {
                q = (-b + distSqrt) / 2.0f;
            }

            // compute t0 and t1
            var t0 = q / a;
            var t1 = c / q;

            // make sure t0 is smaller than t1
            if (t0 > t1)
            {
                // if t0 is bigger than t1 swap them around
                var temp = t0;
                t0 = t1;
                t1 = temp;
            }

            // if t1 is less than zero, the object is in the ray's negative direction
            // and consequently the ray misses the sphere
            if (t1 < 0)
            {
                return false;
            }
            double t = 0;
            // if t0 is less than zero, the intersection point is at t1
            if (t0 < 0)
            {
                t = t1;
            }
            // else the intersection point is at t0
            else
            {
                t = t0;
            }

            var point = pickRayDir * t;

            point = pickRayOrig + point;

            pointCoordinate = Coordinates.CartesianToSpherical2(point);

            return true;
        }


        public bool SphereIntersectRay(Vector3 pickRayOrig, Vector3 pickRayDir, out Coordinates pointCoordinate)
        {
            pointCoordinate = new Coordinates(0, 0);
            float r = 1;
            //Compute A, B and C coefficients
            var a = Vector3.Dot(pickRayDir, pickRayDir);
            var b = 2 * Vector3.Dot(pickRayDir, pickRayOrig);
            var c = Vector3.Dot(pickRayOrig, pickRayOrig) - (r * r);

            //Find discriminant
            var disc = b * b - 4 * a * c;

            // if discriminant is negative there are no real roots, so return 
            // false as ray misses sphere
            if (disc < 0)
            {
                return false;
            }

            // compute q as described above
            var distSqrt = (float)Math.Sqrt(disc);
            float q;
            if (b < 0)
            {
                q = (-b - distSqrt) / 2.0f;
            }
            else
            {
                q = (-b + distSqrt) / 2.0f;
            }

            // compute t0 and t1
            var t0 = q / a;
            var t1 = c / q;

            // make sure t0 is smaller than t1
            if (t0 > t1)
            {
                // if t0 is bigger than t1 swap them around
                var temp = t0;
                t0 = t1;
                t1 = temp;
            }

            // if t1 is less than zero, the object is in the ray's negative direction
            // and consequently the ray misses the sphere
            if (t1 < 0)
            {
                return false;
            }
            float t = 0;
            // if t0 is less than zero, the intersection point is at t1
            if (t0 < 0)
            {
                t = t1;
            }
            // else the intersection point is at t0
            else
            {
                t = t0;
            }

            var point = pickRayDir * t;

            point = pickRayOrig + point;

            pointCoordinate = Coordinates.CartesianToSpherical2(point);

            return true;
        }


        public void TransformPickPointToWorldSpace(Point ptCursor, int backBufferWidth, int backBufferHeight, out Vector3d vPickRayOrig, out Vector3d vPickRayDir)
        {
            // Credit due to the DirectX 9 C++ Pick sample and MVP Robert Dunlop
            // Get the pick ray from the mouse position

            // Compute the vector of the pick ray in screen space
            Vector3d v;
            v.X = (((2.0 * ptCursor.X) / backBufferWidth) - 1) / ProjMatrix.M11;
            v.Y = -(((2.0 * ptCursor.Y) / backBufferHeight) - 1) / ProjMatrix.M22;
            v.Z = 1.0;

            //Matrix3d mInit = WorldMatrix * ViewMatrix;
            var mInit = RenderContext11.WorldBase * ViewMatrix;

            var m = Matrix3d.Invert(mInit);

            // Transform the screen space pick ray into 3D space
            vPickRayDir.X = v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31;
            vPickRayDir.Y = v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32;
            vPickRayDir.Z = v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33;


            vPickRayDir.Normalize();

            vPickRayOrig.X = m.M41;
            vPickRayOrig.Y = m.M42;
            vPickRayOrig.Z = m.M43;
        }

        public void TransformStarPickPointToWorldSpace(Point ptCursor, int backBufferWidth, int backBufferHeight, out Vector3d vPickRayOrig, out Vector3d vPickRayDir)
        {

            Vector3d v;
            v.X = (((2.0f * ptCursor.X) / backBufferWidth) - 1) / ProjMatrix.M11;
            v.Y = -(((2.0f * ptCursor.Y) / backBufferHeight) - 1) / ProjMatrix.M22;
            v.Z = 1.0f;

            var mInit = WorldMatrix * ViewMatrix;

            var m = Matrix3d.Invert(mInit);

            // Transform the screen space pick ray into 3D space
            vPickRayDir.X = v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31;
            vPickRayDir.Y = v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32;
            vPickRayDir.Z = v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33;


            vPickRayDir.Normalize();

            // Transform the screen space pick ray into 3D space
            vPickRayDir.X = v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31;
            vPickRayDir.Y = v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32;
            vPickRayDir.Z = v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33;


            vPickRayDir.Normalize();

            vPickRayOrig.X = m.M41;
            vPickRayOrig.Y = m.M42;
            vPickRayOrig.Z = m.M43;

            // Calculate the origin as intersection with near frustum

            vPickRayOrig.X += vPickRayDir.X * m_nearPlane;
            vPickRayOrig.Y += vPickRayDir.Y * m_nearPlane;
            vPickRayOrig.Z += vPickRayDir.Z * m_nearPlane;
        }

        double deltaLat;
        double deltaLong;
        private void MainWndow_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                if (Properties.Settings.Default.FollowMouseOnZoom && !PlanetLike)
                {
                    var point = GetCoordinatesForScreenPoint(e.X, e.Y);
                    if (Space && Settings.Active.LocalHorizonMode && !tracking)
                    {
                        var currentAltAz = Coordinates.EquitorialToHorizon(point, SpaceTimeController.Location, SpaceTimeController.Now);

                        targetAlt = currentAltAz.Alt;
                        targetAz = currentAltAz.Az;

                    }
                    else
                    {
                        TargetLong = RAtoViewLng(point.RA);
                        TargetLat = point.Lat;
                    }
                }
                zoomSpeed = (ZoomSpeeds)Properties.Settings.Default.ZoomSpeed;

                if (Math.Abs(e.Delta) == 120)
                {
                    if (e.Delta < 0)
                    {
                        ZoomOut();
                    }
                    else
                    {
                        ZoomIn();
                    }
                }
                else
                {
                    if (e.Delta < 0)
                    {
                        ZoomOut(Math.Abs(e.Delta) / 120.0);
                    }
                    else
                    {
                        ZoomIn(Math.Abs(e.Delta) / 120.0);
                    }
                }

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            MoveView(-moveVector.X, moveVector.Y, false);
            ZoomView(ZoomVector / 400);
            RotateView(0, OrbitVector / 1000);





            switch (activeTouch)
            {
                case TouchControls.ZoomIn:
                    zoomSpeed = (ZoomSpeeds)Properties.Settings.Default.ZoomSpeed;
                    ZoomIn();
                    break;
                case TouchControls.ZoomOut:
                    zoomSpeed = (ZoomSpeeds)Properties.Settings.Default.ZoomSpeed;
                    ZoomOut();
                    break;
                case TouchControls.Up:
                    MoveUp();
                    break;
                case TouchControls.Down:
                    MoveDown();
                    break;
                case TouchControls.Left:
                    MoveLeft();
                    break;
                case TouchControls.Right:
                    MoveRight();
                    break;
                case TouchControls.Clockwise:
                    RotateView(0, .05);
                    break;
                case TouchControls.CounterClockwise:
                    RotateView(0, -.05);
                    break;
                case TouchControls.TiltUp:
                    RotateView(-.04, 0);
                    break;
                case TouchControls.TiltDown:
                    RotateView(.04, 0);
                    break;
                case TouchControls.TrackBall:
                    //Earth3d.MainWindow.MoveView(-moveVector.X, moveVector.Y, false);
                    break;
                case TouchControls.None:

                    break;
            }


        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Tab:
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;

            }
            return base.IsInputKey(keyData);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    return false;
            }
            return base.ProcessDialogKey(keyData);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    return false;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }




        protected override bool ProcessTabKey(bool forward)
        {
            return false;
        }

        public void RotateView(double upDown, double leftRight)
        {
            CameraRotateTarget = (CameraRotateTarget + leftRight);
            CameraAngleTarget = (CameraAngleTarget + upDown);

            if (CameraAngleTarget < TiltMin)
            {
                CameraAngleTarget = TiltMin;
            }

            if (CameraAngleTarget > 0)
            {
                CameraAngleTarget = 0;
            }
        }
        bool useAsymetricProj;

        private void MainWndow_KeyDown(object sender, KeyEventArgs e)
        {


            if (uiController != null)
            {
                if (uiController.KeyDown(sender, e))
                {
                    return;
                }
            }
            if (ModifierKeys == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.OemMinus:
                    case Keys.PageUp:
                    case Keys.Subtract:
                        zoomSpeed = (ZoomSpeeds)Properties.Settings.Default.ZoomSpeed;
                        ZoomOut();
                        break;
                    case Keys.PageDown:
                    case Keys.Oemplus:
                    case Keys.Add:
                        zoomSpeed = (ZoomSpeeds)Properties.Settings.Default.ZoomSpeed;
                        ZoomIn();
                        break;
                    case Keys.F1:
                        LaunchHelp();
                        break;
                    case Keys.F5:
                        TileCache.ClearCache();
                        Tile.fastLoad = true;
                        Tile.iTileBuildCount = 0;
                        break;
                    case Keys.Left:
                        RotateView(0, -.05);
                        break;
                    case Keys.Right:
                        RotateView(0, .05);
                        break;
                    case Keys.Up:
                        RotateView(-.01, 0);
                        break;
                    case Keys.Down:
                        RotateView(.01, 0);
                        break;

                }
            }
            else if (ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.F9:
                        config = new Config();
                        break;
                    case Keys.A:
                        useAsymetricProj = !useAsymetricProj;
                        break;
                    case Keys.F5:
                        Tile.PurgeRefresh = true;
                        Render();
                        Tile.PurgeRefresh = false;
                        TileCache.ClearCache();
                        break;
                    case Keys.E:
                        if (uiController == null)
                        {
                            // We can't really tell where this image came from so dirty everything.
                            FolderBrowser.AllDirty = true;
                            uiController = new ImageAlignmentUI();
                            MainWindow.StudyOpacity = 50;

                        }
                        else
                        {
                            if (uiController != null && uiController is ImageAlignmentUI)
                            {
                                ((ImageAlignmentUI)uiController).Close();
                            }
                            uiController = null;
                        }
                        break;

                    case Keys.L:
                        Logging = !Logging;

                        break;
                    case Keys.T:
                        targetViewCamera = viewCamera = CustomTrackingParams;
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Z:
                        riftFov *= .99f;
                        Text = riftFov.ToString();
                        break;
                    case Keys.X:
                        riftFov *= 1.01f;
                        Text = riftFov.ToString();
                        break;
                    case Keys.C:
                        iod *= .99f;
                        break;
                    case Keys.V:
                        iod *= 1.01f;
                        break;
                    case Keys.F:
                        SpaceTimeController.Faster();
                        break;
                    case Keys.S:
                        SpaceTimeController.Slower();
                        break;
                    case Keys.H:
                        // turn friction on and off
                        Friction = !Friction;
                        break;
                    case Keys.P:
                        SpaceTimeController.PauseTime();
                        break;
                    case Keys.N:
                        SpaceTimeController.SetRealtimeNow();
                        break;
                    case Keys.B:
                        blink = !blink;
                        break;
                    case Keys.L:
                        RenderContext11.SetLatency(3);
                        break;
                    case Keys.K:
                        RenderContext11.SetLatency(1);
                        break;
                    case Keys.F5:
                        TileCache.ClearCache();
                        Tile.iTileBuildCount = 0;
                        break;
                    case Keys.F11:
                        ShowFullScreen(!fullScreen);
                        break;
                    case Keys.OemMinus:
                    case Keys.PageUp:
                    case Keys.Subtract:
                        zoomSpeed = (ZoomSpeeds)Properties.Settings.Default.ZoomSpeed;
                        ZoomOut();
                        break;
                    case Keys.Oemplus:
                    case Keys.PageDown:
                    case Keys.Add:
                        zoomSpeed = (ZoomSpeeds)Properties.Settings.Default.ZoomSpeed;
                        ZoomIn();
                        break;
                    case Keys.F3:

                        break;
                    case Keys.F1:
                        LaunchHelp();
                        break;
                    case Keys.F2:
                        showWireFrame = !showWireFrame;
                        break;
                    case Keys.Left:
                        var c = UiTools.GetFocusControl();
                        MoveLeft();
                        break;
                    case Keys.Right:
                        MoveRight();
                        break;
                    case Keys.Up:
                        MoveUp();
                        break;
                    case Keys.Down:
                        MoveDown();
                        break;
                    case Keys.Space:
                    case Keys.Play:
                        if (TourEdit != null)
                        {
                            TourEdit.PlayNow(false);
                        }
                        break;
                    case Keys.Escape:
                        if (fullScreen)
                        {
                            ShowFullScreen(false);
                        }
                        break;
                }
            }
        }


        private static bool fullScreen;

        public static bool FullScreen
        {
            get { return fullScreen; }
            set { fullScreen = value; }
        }
        public void MoveDown()
        {
            MoveView(0, -50, false);
        }

      
        public void MoveUp()
        {
            MoveView(0, 50, false);

        }

        public void MoveRight()
        {
            MoveView(50, 0, false);

        }

        public void MoveLeft()
        {
            MoveView(-50, 0, false);
        }


        internal void MoveAndZoom(double leftRight, double upDown, double zoom)
        {
            MoveView(leftRight, upDown, false);
            ZoomView(zoom);
        }


        public void MoveView(double amountX, double amountY, bool mouseDrag)
        {
            if (CurrentImageSet == null)
            {
                return;
            }
            Tracking = false;
            var angle = Math.Atan2(amountY, amountX);
            var distance = Math.Sqrt(amountY * amountY + amountX * amountX);
            if (SolarSystemMode)
            {
                amountX = Math.Cos(angle - CameraRotate) * distance;
                amountY = Math.Sin(angle - CameraRotate) * distance;
            }
            else if (!PlanetLike)
            {
                amountX = Math.Cos(angle + CameraRotate) * distance;
                amountY = Math.Sin(angle + CameraRotate) * distance;
            }
            else
            {
                amountX = Math.Cos(angle - CameraRotate) * distance;
                amountY = Math.Sin(angle - CameraRotate) * distance;
            }

            MoveViewNative(amountX, amountY, mouseDrag);
        }



        public void MoveViewNative(double amountX, double amountY, bool mouseDrag)
        {
            var scaleY = GetPixelScaleY();
            var scaleX = GetPixelScaleX(mouseDrag);

       
            if (CurrentImageSet.DataSetType == ImageSetType.SolarSystem || SandboxMode)
            {
                if (scaleY > .05999)
                {
                    scaleX = scaleY;
                }
            }

            if (Space && Settings.Active.GalacticMode)
            {
                amountX = -amountX;
            }

            if (Space && (Settings.Active.LocalHorizonMode || Settings.Active.GalacticMode))
            {
                targetAlt += (amountY) * scaleY;
                if (targetAlt > Properties.Settings.Default.MaxLatLimit)
                {
                    targetAlt = Properties.Settings.Default.MaxLatLimit;
                }
                if (targetAlt < -Properties.Settings.Default.MaxLatLimit)
                {
                    targetAlt = -Properties.Settings.Default.MaxLatLimit;
                }

            }
            else
            {
                TargetLat += (amountY) * scaleY;

                if (TargetLat > Properties.Settings.Default.MaxLatLimit)
                {
                    TargetLat = Properties.Settings.Default.MaxLatLimit;
                }
                if (TargetLat < -Properties.Settings.Default.MaxLatLimit)
                {
                    TargetLat = -Properties.Settings.Default.MaxLatLimit;
                }
            }
            if (Space && (Settings.Active.LocalHorizonMode || Settings.Active.GalacticMode))
            {
                targetAz = ((targetAz + amountX * scaleX) + 720) % 360;
            }
            else
            {
                TargetLong += (amountX) * scaleX;

                TargetLong = ((TargetLong + 900.0) % 360.0) - 180.0;
            }
        }


        //

        [DllImport("hhctrl.ocx", EntryPoint = "HtmlHelp", CharSet = CharSet.Unicode)]
        internal static extern IntPtr HtmlHelp(IntPtr hWndCaller, string helpFile, int command, string topic);

        public static void LaunchHelp()
        {
            WebWindow.OpenUrl("http://www.worldwidetelescope.org/Support/Index", true);
           
        }

        bool settingsDirty;
        public static bool FormIsClosing = false;
        private void Earth3d_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormIsClosing = true;
            if (videoOut != null)
            {
                videoOut.Close();
            }

            if (!CloseOpenToursOrAbort(true))
            {
                e.Cancel = true;
            }

            TourPopup.CloseTourPopups();

            Initialized = false;

            if (renderHost != null)
            {
                renderHost.Close();
            }
        }

        private void Earth3d_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {

                if (sampConnection.Connected)
                {
                    sampConnection.Unregister();
                }
            }
            catch
            {
            }

            try
            {
                webServer.Shutdown();
            }
            catch
            {
            }

            if (Settings.MasterController)
            {
                NetControl.SaveNodeList();
            }

            MidiMapManager.Shutdown();

            if (config.Master == false)
            {
                NetControl.ReportStatus(ClientNodeStatus.Offline, "Shutting Down", "");
            }


            try
            {
                pause = true;
                config.SaveToXml();
            }
            catch
            {
            }
            try
            {

                ShutdownServices();

                MainWindow = null;
            }
            catch
            {
            }

            try
            {
                TourDocument.ClearTempDirectory();
            }
            catch
            {
            }

            if (settingsDirty)
            {
                Properties.Settings.Default.Save();
            }
        }


        private void ShutdownServices()
        {
            TileCache.ShutdownQueue();
            TileCache.PurgeQueue();
            TileCache.ClearCache();
            NetControl.Abort();
            BufferPool11.DisposeBuffers();
            CleanUpWarpBuffers();

            CleanupStereoAndDomeBuffers();
            CleanupDomeVertexBuffers();
            Grids.CleanupGrids();
            GlyphCache.CleanUpAll();
            Constellations.CleanUpAll();
            ScreenBroadcast.Shutdown();

            RenderContext11.Dispose();
            RenderContext11 = null;
            AudioPlayer.Shutdown();
        }

        private void showQueue_Click(object sender, EventArgs e)
        {
            var queueList = new Queue_List();
            queueList.Show();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();

        }
        public double GetNetzoom(double amount)
        {
            double net = 1;
            switch (zoomSpeed)
            {
                case ZoomSpeeds.SLOW:
                    net = .1;
                    break;
                case ZoomSpeeds.MEDIUM:
                    net = .25;
                    break;
                case ZoomSpeeds.FAST:
                    net = 1;
                    break;
                default:
                    break;
            }
            if (ModifierKeys == Keys.Shift)
            {
                net /= 5;
            }

            return 1 + (net * amount / 4);
        }

        public double NetZoomFactor
        {
            get
            {
                double net = 1;
                switch (zoomSpeed)
                {
                    case ZoomSpeeds.SLOW:
                        net = .1;
                        break;
                    case ZoomSpeeds.MEDIUM:
                        net = .25;
                        break;
                    case ZoomSpeeds.FAST:
                        net = 1;
                        break;
                    default:
                        break;
                }
                if (ModifierKeys == Keys.Shift)
                {
                    net /= 5;
                }

                return 1 + net;
            }

        }
        public void ZoomView(double amount)
        {
            if (amount == 99999)
            {
                TargetZoom = ZoomFactor;
                return;
            }
            if (amount > 0)
            {
                if (TargetZoom > ZoomMin)
                {
                    TargetZoom /= (1 + amount);

                    ComputeViewParameters(CurrentImageSet);
                }
                else
                {
                    TargetZoom = ZoomMin;
                }
            }
            if (amount < 0)
            {
                if ((TargetZoom * (1 - amount)) <= ZoomMax)
                {
                    TargetZoom *= (1 - amount);

                    ComputeViewParameters(CurrentImageSet);
                }
                else
                {
                    TargetZoom = ZoomMax;
                }
            }

        }

        public void DomeLeft(double amount)
        {
            MainWindow.viewCamera.DomeAz += (float)amount;
        }

        public void DomeRight(double amount)
        {
            MainWindow.viewCamera.DomeAz -= (float)amount;
        }

        public void DomeUp(double amount)
        {
            MainWindow.viewCamera.DomeAlt += (float)amount;
        }

        public void DomeDown(double amount)
        {
            MainWindow.viewCamera.DomeAlt -= (float)amount;
        }

        public void ZoomRateIn(double amount)
        {
            NetZoomRate = -amount * 500;
        }

        public void ZoomRateOut(double amount)
        {
            NetZoomRate = amount * 500;
        }

        public void RotateRight(double amount)
        {
            RotateView(0, -amount / 100);
        }

        public void RotateLeft(double amount)
        {
            RotateView(0, amount / 100);
        }

        public void TiltUp(double amount)
        {
            RotateView(amount / 100, 0);

        }

        public void TiltDown(double amount)
        {
            RotateView(-amount / 100, 0);
        }

        public void ZoomIn(double amount)
        {
            if (TargetZoom > ZoomFactor)
            {
                TargetZoom = ZoomFactor;
                return;
            }

            if (TargetZoom > ZoomMin)
            {
                TargetZoom /= 1 + GetNetzoom(amount);

                if (!smoothZoom)
                {
                    ZoomFactor = TargetZoom;
                }
                ComputeViewParameters(CurrentImageSet);
            }
            else
            {
                TargetZoom = ZoomMin;
            }

        }

        public void ZoomOut(double amount)
        {
            if (TargetZoom < ZoomFactor)
            {
                TargetZoom = ZoomFactor;
                return;
            }

            if ((TargetZoom * GetNetzoom(amount)) <= ZoomMax)
            {
                TargetZoom *= GetNetzoom(amount);
                if (!smoothZoom)
                {
                    ZoomFactor = TargetZoom;
                }
                ComputeViewParameters(CurrentImageSet);
            }
            else
            {
                TargetZoom = ZoomMax;
            }
        }

        public void ZoomIn()
        {
            if (TargetZoom > ZoomFactor)
            {
                TargetZoom = ZoomFactor;
                return;
            }

            if (TargetZoom > ZoomMin)
            {
                TargetZoom /= NetZoomFactor;

                if (!smoothZoom)
                {
                    ZoomFactor = TargetZoom;
                }
                ComputeViewParameters(CurrentImageSet);
            }
            else
            {
                TargetZoom = ZoomMin;
            }

        }

        public void ZoomOut()
        {
            if (TargetZoom < ZoomFactor)
            {
                TargetZoom = ZoomFactor;
                return;
            }

            if ((TargetZoom * NetZoomFactor) <= ZoomMax)
            {
                TargetZoom *= NetZoomFactor;
                if (!smoothZoom)
                {
                    ZoomFactor = TargetZoom;
                }
                ComputeViewParameters(CurrentImageSet);
            }
            else
            {
                TargetZoom = ZoomMax;
            }
        }
        double zoomMax = 360;

        readonly double zoomMaxSolarSystem = Properties.Settings.Default.MaxZoomLimitSolar;
        double ZoomMax
        {
            get
            {
                if (currentImageSetfield.DataSetType == ImageSetType.SolarSystem)
                {
                    return zoomMaxSolarSystem;
                }
                return zoomMax;
            }
        }
        double zoomMin = 0.001373291015625;
        readonly double zoomMinSolarSystem = Properties.Settings.Default.MinZoonLimitSolar;

        public double ZoomMin
        {
            get
            {
                if (currentImageSetfield.DataSetType == ImageSetType.SolarSystem)
                {
                    return (zoomMinSolarSystem / 10000000000) * Settings.Active.SolarSystemScale;
                }
                if (currentImageSetfield.IsMandelbrot)
                {

                    return 0.00000000000000000000000000000001;
                }
                return zoomMin / 64;
            }
            set { zoomMin = value; }
        }


        bool zooming;
        bool tracking;

        public bool Tracking
        {
            get { return tracking; }
            set { tracking = value; }
        }

        IPlace trackingObject;

        public IPlace TrackingObject
        {
            get { return trackingObject; }
            set { trackingObject = value; }
        }

        private void UpdateViewParameters()
        {
            double speed = 8;
            switch (zoomSpeed)
            {
                case ZoomSpeeds.FAST:
                    speed = 8;
                    break;
                case ZoomSpeeds.MEDIUM:
                    speed = 16;
                    break;
                case ZoomSpeeds.SLOW:
                    speed = 32;
                    break;
            }

            if (Math.Abs(ZoomFactor - TargetZoom) > (ZoomFactor / 2048))
            {
                ZoomFactor += (TargetZoom - ZoomFactor) / speed;
                zooming = true;
            }
            else
            {
                zoomingUp = false;
                ZoomFactor = TargetZoom;
                if (zooming)
                {
                    zooming = false;
                    NotifyMoveComplete();
                }
            }

            if (Math.Abs(CameraRotateTarget - CameraRotate) > (.1 * RC))
            {
                CameraRotate += (CameraRotateTarget - CameraRotate) / 10;
            }
            else
            {
                CameraRotate = CameraRotateTarget;
            }

            if (Math.Abs(CameraAngleTarget - CameraAngle) > (.1 * RC))
            {
                CameraAngle += (CameraAngleTarget - CameraAngle) / 10;
            }
            else
            {
                CameraAngle = CameraAngleTarget;
            }


            if (mover == null)
            {
                if (Space && tracking && trackingObject != null)
                {
                    if (Space && Settings.Active.GalacticMode)
                    {
                        var gPoint = Coordinates.J2000toGalactic(trackingObject.RA * 15, trackingObject.Dec);

                        targetAlt = alt = gPoint[1];
                        targetAz = az = gPoint[0];
                    }
                    else if (Space && Settings.Active.LocalHorizonMode)
                    {
                        var currentAltAz = Coordinates.EquitorialToHorizon(Coordinates.FromRaDec(trackingObject.RA, trackingObject.Dec), SpaceTimeController.Location, SpaceTimeController.Now);

                        targetAlt = alt = currentAltAz.Alt;
                        targetAz = az = currentAltAz.Az;
                    }
                    else
                    {
                        ViewLat = TargetLat = trackingObject.Dec;
                        ViewLong = TargetLong = RAtoViewLng(trackingObject.RA);
                    }
                }
                else if (!SolarSystemMode)
                {

                    //todo dome tilt looks fishey here...
                    if (Space && Settings.Active.LocalHorizonMode && Settings.DomeView)
                    {
                        targetAlt = alt = -config.TotalDomeTilt;
                        targetAz = az = 0;
                    }

                    tracking = false;
                    trackingObject = null;
                }
            }

            if (!zoomingUp && !tracking)
            {
                var minDelta = (ZoomFactor / 4000.0);
                if (ZoomFactor > 360)
                {
                    minDelta = (360.0 / 40000.0);
                }

                if (Space && (Settings.Active.LocalHorizonMode || Settings.Active.GalacticMode))
                {
                    if (((Math.Abs(targetAlt - alt) >= (minDelta)) |
                        ((Math.Abs(targetAz - az) >= (minDelta)))))
                    {
                        alt += (targetAlt - alt) / 10;

                        if (Math.Abs(targetAz - az) > 170)
                        {
                            if (targetAz > az)
                            {
                                az += (targetAz - (360 + az)) / 10;
                            }
                            else
                            {
                                az += ((360 + targetAz) - az) / 10;
                            }
                        }
                        else
                        {
                            az += (targetAz - az) / 10;
                        }

                        az = ((az + 720) % 360);
                    }
                }
                else
                {
                    if (((Math.Abs(TargetLat - ViewLat) >= (minDelta)) |
                        ((Math.Abs(TargetLong - ViewLong) >= (minDelta)))))
                    {
                        if (deltaLat != 0 | deltaLong != 0)
                        {
                            ViewLat += deltaLat;
                            ViewLong += deltaLong;
                        }
                        else
                        {
                            ViewLat += (TargetLat - ViewLat) / 10;

                            if (Math.Abs(TargetLong - ViewLong) > 170)
                            {
                                if (TargetLong > ViewLong)
                                {
                                    ViewLong += (TargetLong - (360 + ViewLong)) / 10;
                                }
                                else
                                {
                                    ViewLong += ((360 + TargetLong) - ViewLong) / 10;
                                }
                            }
                            else
                            {
                                ViewLong += (TargetLong - ViewLong) / 10;
                            }
                        }
                        ViewLong = ((ViewLong + 540) % 360) - 180;
                    }
                    else
                    {
                        if (ViewLat != TargetLat || ViewLong != TargetLong)
                        {
                            ViewLat = TargetLat;
                            ViewLong = TargetLong;


                            NotifyMoveComplete();
                        }
                        deltaLat = 0;
                        deltaLong = 0;
                        if (findingTargetGeo)
                        {
                            TargetZoom = finalZoom;
                            findingTargetGeo = false;
                        }
                    }
                }
            }

        }
        double lastMoveCompleteLat;
        double lastMoveCompleteLng;
        private void SendMoveComplete()
        {
            if (ViewLat != lastMoveCompleteLat || ViewLong != lastMoveCompleteLng)
            {
                lastMoveCompleteLat = ViewLat;
                lastMoveCompleteLng = ViewLong;
                if (Space)
                {

                    UpdateSampClients();
                }
            }
        }

        private UpdateSampClientsDelegate invokeUpdateSampClients;

        private delegate void UpdateSampClientsDelegate();

        private void UpdateSampClients()
        {
            if (invokeUpdateSampClients == null)
            {
                invokeUpdateSampClients = UpdateSampClientsCaller;
            }

            invokeUpdateSampClients.BeginInvoke(null, null);
        }

        private void UpdateSampClientsCaller()
        {
            sampConnection.GotoPoint(RA, Dec);
        }
        int sendMoveCount;

        private void SendMove()
        {
            var fgHash = 0;
            var bgHash = 0;

            sendMoveCount++;

            if (studyImageset != null)
            {
                fgHash = studyImageset.GetHash();
            }
            if (CurrentImageSet != null)
            {
                bgHash = CurrentImageSet.GetHash();
            }

            // Moving to Binary Sync
            NetControl.SendMoveBinary(ViewLat, ViewLong, ZoomFactor, CameraRotate, CameraAngle, fgHash, bgHash, StudyOpacity, autoUpdate, autoFlush, Settings.Active.LocalHorizonMode, (int)SolarSystemTrack, viewCamera.ViewTarget, Settings.Active.SolarSystemScale, targetHeight, TrackingFrame, Properties.Settings.Default.ReticleAlt, Properties.Settings.Default.ReticleAz);
            autoFlush = false;
        }


        public void GotoTarget(IPlace place, bool noZoom, bool instant, bool trackObject)
        {
            if (place == null)
            {
                return;
            }
            if ((trackObject && SolarSystemMode) )
            {
                if ((place.Classification == Classification.SolarSystem && place.Type != ImageSetType.SolarSystem) || (place.Classification == Classification.Star) || (place.Classification == Classification.Galaxy) && place.Distance > 0)
                {
                    var target = SolarSystemObjects.Undefined;

                    if (place.Classification == Classification.Star || place.Classification == Classification.Galaxy)
                    {
                        target = SolarSystemObjects.Custom;
                    }
                    else
                    {
                        try
                        {
                            if (place.Target != SolarSystemObjects.Undefined)
                            {
                                target = place.Target;
                            }
                            else
                            {
                                target = (SolarSystemObjects)Enum.Parse(typeof(SolarSystemObjects), place.Name, true);
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (target != SolarSystemObjects.Undefined)
                    {
                        trackingObject = place;
                        double jumpTime = 4;

                        if (target == SolarSystemObjects.Custom)
                        {
                            jumpTime = 17;
                        }
                        else
                        {
                            jumpTime += 13 * (101 - Settings.Active.SolarSystemScale) / 100;
                        }

                        if (instant)
                        {
                            jumpTime = 1;
                        }

                        var camTo = viewCamera;
                        camTo.TargetReferenceFrame = "";
                        camTo.Target = target;
                        double zoom = 10;
                        if (target == SolarSystemObjects.Custom)
                        {
                            if (place.Classification == Classification.Galaxy)
                            {
                                zoom = 1404946007758;
                            }
                            else
                            {
                                zoom = 63239.6717 * 100;
                            }
                            // Star or something outside of SS
                            var vect = Coordinates.RADecTo3d(place.RA, place.Dec, place.Distance);
                            var ecliptic = Coordinates.MeanObliquityOfEcliptic(SpaceTimeController.JNow) / 180.0 * Math.PI;

                            vect.RotateX(ecliptic);
                            camTo.ViewTarget = -vect;
                        }
                        else
                        {
                            camTo.ViewTarget = Planets.GetPlanet3dLocation(target, SpaceTimeController.GetJNowForFutureTime(jumpTime));
                            switch (target)
                            {
                                case SolarSystemObjects.Sun:
                                    zoom = .6;
                                    break;
                                case SolarSystemObjects.Mercury:
                                    zoom = .0004;
                                    break;
                                case SolarSystemObjects.Venus:
                                    zoom = .0004;
                                    break;
                                case SolarSystemObjects.Mars:
                                    zoom = .0004;
                                    break;
                                case SolarSystemObjects.Jupiter:
                                    zoom = .007;
                                    break;
                                case SolarSystemObjects.Saturn:
                                    zoom = .007;
                                    break;
                                case SolarSystemObjects.Uranus:
                                    zoom = .004;
                                    break;
                                case SolarSystemObjects.Neptune:
                                    zoom = .004;
                                    break;
                                case SolarSystemObjects.Pluto:
                                    zoom = .0004;
                                    break;
                                case SolarSystemObjects.Moon:
                                    zoom = .0004;
                                    break;
                                case SolarSystemObjects.Io:
                                    zoom = .0004;
                                    break;
                                case SolarSystemObjects.Europa:
                                    zoom = .0004;
                                    break;
                                case SolarSystemObjects.Ganymede:
                                    zoom = .0004;
                                    break;
                                case SolarSystemObjects.Callisto:
                                    zoom = .0004;
                                    break;
                                case SolarSystemObjects.Earth:
                                    zoom = .0004;
                                    break;
                                case SolarSystemObjects.Custom:
                                    zoom = 10;
                                    break;

                                default:
                                    break;
                            }

                            zoom = zoom * Settings.Active.SolarSystemScale;

                        }

                        var fromParams = viewCamera;
                        if (SolarSystemTrack == SolarSystemObjects.Custom && !string.IsNullOrEmpty(TrackingFrame))
                        {
                            fromParams = CustomTrackingParams;
                            TrackingFrame = "";
                        }
                        camTo.Zoom = zoom;
                        var toVector = camTo.ViewTarget;
                        toVector.Subtract(fromParams.ViewTarget);

  
                        if (place.Classification == Classification.Star)
                        {
                            toVector = -toVector;
                        }

                        if (toVector.Length() != 0)
                        {

                            var raDec = toVector.ToRaDec();

                            if (target == SolarSystemObjects.Custom)
                            {
                                camTo.Lat = -raDec.Y;
                            }
                            else
                            {
                                camTo.Lat = raDec.Y;
                            }
                            camTo.Lng = raDec.X * 15 - 90;
                        }
                        else
                        {
                            camTo.Lat = viewCamera.Lat;
                            camTo.Lng = viewCamera.Lng;
                        }

                        if (target != SolarSystemObjects.Custom)
                        {
                            // replace with planet surface
                            camTo.ViewTarget = Planets.GetPlanetTargetPoint(target, camTo.Lat, camTo.Lng, SpaceTimeController.GetJNowForFutureTime(jumpTime));

                        }



                        var solarMover = new ViewMoverKenBurnsStyle(fromParams, camTo, jumpTime, SpaceTimeController.Now, SpaceTimeController.GetTimeForFutureTime(jumpTime), InterpolationType.EaseInOut);
                        solarMover.FastDirectionMove = true;
                        mover = solarMover;

                        return;
                    }
                }
            }


            Tracking = false;
            trackingObject = null;
            var camParams = place.CamParams;



            if (place.Type != CurrentImageSet.DataSetType)
            {
                ZoomFactor = TargetZoom = ZoomMax;
                CameraRotateTarget = CameraRotate = 0;
                CameraAngleTarget = CameraAngle = 0;
                viewCamera = place.CamParams;
                if (place.BackgroundImageSet != null)
                {
                    FadeInImageSet(GetRealImagesetFromGeneric(place.BackgroundImageSet));
                }
                else
                {
                    CurrentImageSet = GetDefaultImageset(place.Type, BandPass.Visible);
                }
                instant = true;
            }
            else if (SolarSystemMode && place.Target != SolarSystemTrack)
            {
                ZoomFactor = TargetZoom = ZoomMax;
                CameraRotateTarget = CameraRotate = 0;
                CameraAngleTarget = CameraAngle = 0;
                viewCamera = targetViewCamera = place.CamParams;
                SolarSystemTrack = place.Target;
                instant = true;
            }


            if (place.Classification == Classification.Constellation)
            {
                camParams.Zoom = ZoomMax;
                GotoTarget(false, instant, camParams, null, null);
            }
            else
            {
                SolarSystemTrack = place.Target;
                GotoTarget(noZoom, instant, camParams, place.StudyImageset, place.BackgroundImageSet);
 
                if (trackObject)
                {
                    Tracking = true;
                    TrackingObject = place;
                }
            }

        }

        public void FreezeView()
        {
            targetAlt = alt;
            targetAz = az;
            TargetLat = ViewLat;
            TargetLong = ViewLong;
            TargetZoom = ZoomFactor;
            CameraRotateTarget = CameraRotate;
        }

        public void GotoTarget(CameraParameters camParams, bool noZoom, bool instant)
        {
            tracking = false;
            trackingObject = null;
            GotoTarget(noZoom, instant, camParams, studyImageset, CurrentImageSet);

        }
        public void GotoTargetRADec(double ra, double dec, bool noZoom, bool instant)
        {
            tracking = false;
            trackingObject = null;
            GotoTarget(noZoom, instant, new CameraParameters(dec, RAtoViewLng(ra), -1, viewCamera.Rotation, viewCamera.Angle, (float)viewCamera.Opacity), StudyImageset, CurrentImageSet);
        }

        IImageSet targetStudyImageset;
        IImageSet targetBackgroundImageset;

        public void SetStudyImageset(IImageSet studyImageSet, IImageSet backgroundImageSet)
        {
            targetStudyImageset = studyImageSet;
            targetBackgroundImageset = backgroundImageSet;
            if ((targetStudyImageset != null && StudyImageset == null) || (studyImageset != null && !studyImageset.Equals(targetStudyImageset)))
            {
                StudyImageset = targetStudyImageset;
            }

            if (targetBackgroundImageset != null && !CurrentImageSet.Equals(targetBackgroundImageset))
            {
                if (targetBackgroundImageset != null && targetBackgroundImageset.Generic)
                {

                    FadeInImageSet(GetRealImagesetFromGeneric(targetBackgroundImageset));
                }
                else
                {
                    FadeInImageSet(targetBackgroundImageset);
                }
            }
        }

        public void GotoTarget(bool noZoom, bool instant, CameraParameters cameraParams, IImageSet studyImageSet, IImageSet backgroundImageSet)
        {
            tracking = false;
            trackingObject = null;
            targetStudyImageset = studyImageSet;
            targetBackgroundImageset = backgroundImageSet;


            if (noZoom)
            {
                cameraParams.Zoom = viewCamera.Zoom;
                cameraParams.Angle = viewCamera.Angle;
                cameraParams.Rotation = viewCamera.Rotation;
            }
            else
            {
                if (cameraParams.Zoom == -1)
                {
                    if (Space)
                    {
                        cameraParams.Zoom = 1.40625;
                    }
                    else
                    {
                        cameraParams.Zoom = 0.09F;
                    }
                }
            }

            if (instant || (Math.Abs(ViewLat - cameraParams.Lat) < .000000000001 && Math.Abs(ViewLong - cameraParams.Lng) < .000000000001 && Math.Abs(ZoomFactor - cameraParams.Zoom) < .000000000001))
            {
                mover = null;
                viewCamera = targetViewCamera = cameraParams;

                if (Space && Settings.Active.GalacticMode)
                {
                    var gPoint = Coordinates.J2000toGalactic(viewCamera.RA * 15, viewCamera.Dec);
                    targetAlt = alt = gPoint[1];
                    targetAz = az = gPoint[0];
                }
                else if (Space && Settings.Active.LocalHorizonMode)
                {
                    var currentAltAz = Coordinates.EquitorialToHorizon(Coordinates.FromRaDec(viewCamera.RA, viewCamera.Dec), SpaceTimeController.Location, SpaceTimeController.Now);

                    targetAlt = alt = currentAltAz.Alt;
                    targetAz = az = currentAltAz.Az;
                }
                mover_Midpoint(this, new EventArgs());
            }
            else
            {
                if (TourPlayer.Playing)
                {
                    mover = new ViewMoverSlew(viewCamera, cameraParams);
                }
                else
                {
                    mover = new ViewMoverSlew(viewCamera, cameraParams, 1.2);
                }
                mover.Midpoint += mover_Midpoint;
            }
        }

        void mover_Midpoint(object sender, EventArgs e)
        {
            if ((targetStudyImageset != null && studyImageset == null) || (studyImageset != null && !studyImageset.Equals(targetStudyImageset)))
            {
                StudyImageset = targetStudyImageset;
            }

            if (targetBackgroundImageset != null && !CurrentImageSet.Equals(targetBackgroundImageset))
            {
                if (targetBackgroundImageset != null && targetBackgroundImageset.Generic)
                {

                    FadeInImageSet(GetRealImagesetFromGeneric(targetBackgroundImageset));
                }
                else
                {
                    FadeInImageSet(targetBackgroundImageset);
                }


            }
        }

        public void FadeInImageSet(IImageSet newImageSet)
        {
            if (newImageSet.DataSetType != CurrentImageSet.DataSetType)
            {
                fadeImageSet.State = true;
                fadeImageSet.TargetState = false;
            }
            CurrentImageSet = newImageSet;
        }

        public IImageSet GetImagesetByName(string name)
        {
            foreach (var imageset in ImageSets)
            {
                if (imageset.Name.ToLower() == name.ToLower())
                {
                    return imageset;
                }
            }
            return null;
        }

        public IImageSet GetDefaultImageset(ImageSetType imageSetType, BandPass bandPass)
        {
            foreach (var imageset in ImageSets)
            {
                if (imageset.DefaultSet && imageset.BandPass == bandPass && imageset.DataSetType == imageSetType)
                {
                    return imageset;
                }

            }
            foreach (var imageset in ImageSets)
            {
                if (imageset.BandPass == bandPass && imageset.DataSetType == imageSetType)
                {
                    return imageset;
                }

            }
            foreach (var imageset in ImageSets)
            {
                if (imageset.DataSetType == imageSetType)
                {
                    return imageset;
                }

            }
            return ImageSets[0];
        }

        private IImageSet GetRealImagesetFromGeneric(IImageSet generic)
        {
            foreach (var imageset in ImageSets)
            {
                if (imageset.DefaultSet && imageset.BandPass == generic.BandPass && imageset.DataSetType == generic.DataSetType)
                {
                    return imageset;
                }

            }

            foreach (var imageset in ImageSets)
            {
                if (imageset.BandPass == generic.BandPass && imageset.DataSetType == generic.DataSetType)
                {
                    return imageset;
                }

            }
            return ImageSets[0];
        }



        // Begin Set View Mode

        public void SetViewMode(IImageSet newImageSet)
        {
            if (newImageSet != null && CurrentImageSet != null && CurrentImageSet.DataSetType != newImageSet.DataSetType)
            {
                ZoomFactor = TargetZoom = ZoomMax;
                CameraRotate = CameraRotateTarget = 0;
            }
            CurrentImageSet = newImageSet;
            //TileCache.PurgeQueue();
            //TileCache.ClearCache();
        }


        public void SetViewMode()
        {

        }

        static public List<IImageSet> ImageSets = new List<IImageSet>();
        static public Dictionary<string, IImageSet> ReplacementImageSets = new Dictionary<string, IImageSet>();
        public bool InitializeImageSets()
        {

            var url = Properties.Settings.Default.ImageSetUrl;
            var filename = String.Format(@"{0}data\imagesets_5_{1}.wtml", Properties.Settings.Default.CahceDirectory, Math.Abs(url.GetHashCode32()));

            try
            {
                ImageSets.Clear();
                DataSetManager.DownloadFile(url, filename, false, true);
                var doc = new XmlDocument();

                doc.Load(filename);
                var node = doc.SelectSingleNode("Folder");

                foreach (XmlNode child in node.ChildNodes)
                {
                    var ish = ImageSetHelper.FromXMLNode(child);

                    ImageSets.Add(ish);
                    if (!String.IsNullOrEmpty(ish.AltUrl))
                    {
                        ReplacementImageSets.Add(ish.AltUrl, ish);
                    }
                }

                ImageSets.Add(new ImageSetHelper("SandBox", "", ImageSetType.Sandbox, BandPass.Visible, ProjectionType.Toast, 0, 0, 0, 0, 0, "", false, "", 0, 0, 0, false, "", false, false, 0, 0, 0, "", "", "", "", 1, "SandBox"));
                return true;
            }
            catch
            {
                File.Delete(filename);
                UiTools.ShowMessageBox(Language.GetLocalizedText(93, "The Imagery data file could not be downloaded or has been corrupted. WorldWide Telescope must close. You need a working internet connection to update this file. Try again later"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"));
                return false;
            }


        }

        /*
         * Place Holder             Meaning
         * RA                       Main View Center RA in decimal degrees
         * DEC                      Main View Center DEC in degrees degrees
         * FOV                      Main View Height in Decimal Degrees
         * UL.RA                    Upper Left RA
         * UL.DEC                   Upper Left Dec
         * UL, UR, LL, LR           Corners of view display
         * JD                       Decimal Julian Date
         * ROTATION                 Rotation angle East of North Decimal Degrees
         * OBJECT                   Catalog Name of Selected Object
         * SR                       Search Cone Radius that will Cover
         * LAT                      View From Latitude
         * LNG                      View From Longitude
         * ELEV                     View From Elevation
         * WIDTH                    Screen Width in Pixels
         * HEIGHT                   Screen Height in Pixels
         * CONST                    Current Constellation Abbreviation (Three letter codes)
         * ALT                      Local Altitude
         * AZ                       Local Azimuth
         * l                        Galactic Lat
         * b                        Galactic Long
         * CP1(a:b)Label            CP1, CP2, CP3, CP4 are custome sliders that show up when a folder specifies them
         *                          Slider values are send on each update, and URL refreshes with slider change
         
         * */
        /// <summary>
        /// Substitudes placeholders with live values in URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string PrepareUrl(string url)
        {
            url = url.Replace("{RA}", (RA * 15).ToString());
            url = url.Replace("{DEC}", Dec.ToString());
            url = url.Replace("{FOV}", FovAngle.ToString());
            if (CurrentViewCorners != null)
            {
                url = url.Replace("{UL.RA}", (CurrentViewCorners[0].RA * 15).ToString());
                url = url.Replace("{UL.DEC}", (CurrentViewCorners[0].Dec).ToString());
                url = url.Replace("{UR.RA}", (CurrentViewCorners[1].RA * 15).ToString());
                url = url.Replace("{UR.DEC}", (CurrentViewCorners[1].Dec).ToString());
                url = url.Replace("{LL.RA}", (CurrentViewCorners[2].RA * 15).ToString());
                url = url.Replace("{LL.DEC}", (CurrentViewCorners[2].Dec).ToString());
                url = url.Replace("{LR.RA}", (CurrentViewCorners[3].RA * 15).ToString());
                url = url.Replace("{LR.DEC}", (CurrentViewCorners[3].Dec).ToString());
            }

            url = url.Replace("{JD}", SpaceTimeController.JNow.ToString());
            url = url.Replace("{ROTATION}", CameraRotate.ToString());
            url = url.Replace("{SR}", (fovAngle * 1.5).ToString());
            url = url.Replace("{LAT}", SpaceTimeController.Location.Lat.ToString());
            url = url.Replace("{LNG}", SpaceTimeController.Location.Lng.ToString());
            url = url.Replace("{ELEV}", SpaceTimeController.Altitude.ToString());
            url = url.Replace("{WIDTH}", renderWindow.ClientRectangle.Width.ToString());
            url = url.Replace("{HEIGHT}", renderWindow.ClientRectangle.Height.ToString());
            url = url.Replace("{CONST}", constellation);
            url = url.Replace("{ALT}", Alt.ToString());
            url = url.Replace("{AZ}", Az.ToString());
            //          url = url.Replace("{LiveToken}", CloudCommunities.GetTokenFromId(true));
            var gal = J2000toGalactic(RA * 15, Dec);

            url = url.Replace("{l}", gal[0].ToString());
            url = url.Replace("{b}", gal[1].ToString());

            return url;
        }

        private void startQueue_Click(object sender, EventArgs e)
        {
            TileCache.StartQueue();
        }

        private void stopQueue_Click(object sender, EventArgs e)
        {
            TileCache.ShutdownQueue();
        }

        ZoomSpeeds zoomSpeed = ZoomSpeeds.MEDIUM;

        public ZoomSpeeds ZoomSpeed
        {
            get { return zoomSpeed; }
            set { zoomSpeed = value; }
        }
 
        static long lastRender = HiResTimer.TickCount;

        private void timer2_Tick(object sender, EventArgs e)
        {


            //if (spaceNavigatorDevice == null)
            //{
            //    GetData();
            //}
            // Make sure we render when dialogs are up
            var ticks = HiResTimer.TickCount - lastRender;

            var ms = (int)((ticks * 1000) / HiResTimer.Frequency);

            if (ms > 350 && !pause && Initialized && !SpaceTimeController.FrameDumping)
            {
                Render();
            }
        }

 
        private void menuMasterControler_Click(object sender, EventArgs e)
        {
            Settings.MasterController = !Settings.MasterController;
            toolStripMenuItem2.Checked = Settings.MasterController;
        }


        internal Constellations constellationsBoundries = new Constellations("Constellations", "http://www.worldwidetelescope.org/data/constellations.txt", true, false);
        internal Constellations constellationsFigures = new Constellations("Default Figures", "http://www.worldwidetelescope.org/data/figures.txt", false, false);
        internal Constellations constellationCheck = new Constellations("Constellations", "http://www.worldwidetelescope.org/data/constellations.txt", true, true);

        public Constellations ConstellationCheck
        {
            get { return constellationCheck; }
            set { constellationCheck = value; }
        }


        bool autoUpdate;
        bool autoFlush;
        private void runUpdate()
        {
            helpAutoUpdate_Click(null, null);
        }

        private void helpAutoUpdate_Click(object sender, EventArgs e)
        {
            if (!Settings.MasterController)
            {
                if (!CheckForUpdates(true))
                {
                    Close();
                }
            }
            else
            {
                autoUpdate = true;
                SendMove();
            }


        }

        private static bool CheckForUpdates(bool interactive)
        {
            var versionChecked = true;
            try
            {

                //TODO move this to the real temp
                if (!Directory.Exists(Path.GetTempPath()))
                {
                    Directory.CreateDirectory(Path.GetTempPath());
                }
                var Client = new WebClient();

                var yourVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                var url = String.Format("http://www.worldwidetelescope.org/wwtweb/login.aspx?user={0}&Version={1}&Equinox=true", Properties.Settings.Default.UserRatingGUID.ToString("D"), yourVersion);
                var data = Client.DownloadString(url);

                var lines = data.Split(new[] { '\n' });



                var version = lines[0].Substring(lines[0].IndexOf(':') + 1).Trim();
                var dataVersion = lines[1].Substring(lines[1].IndexOf(':') + 1).Trim();
                var message = lines[2].Substring(lines[2].IndexOf(':') + 1).Trim();
                var updateUrl = "http://www.worldwidetelescope.org/wwtweb/setup.aspx";
                var warnVersion = version;
                if (lines.GetLength(0) > 3)
                {
                    warnVersion = lines[3].Substring(lines[3].IndexOf(':') + 1).Trim();
                }
                var minVersion = version;
                if (lines.GetLength(0) > 4)
                {
                    minVersion = lines[4].Substring(lines[4].IndexOf(':') + 1).Trim();
                }
                if (lines.GetLength(0) > 5)
                {
                    updateUrl = lines[5].Substring(lines[5].IndexOf(':') + 1).Trim();
                }

                if (!lines[0].StartsWith("ClientVersion"))
                {
                    throw new Exception();
                }
                if (!lines[1].StartsWith("DataVersion"))
                {
                    throw new Exception();
                }
                var myDataDir = Properties.Settings.Default.CahceDirectory + "\\data";
                if (!Directory.Exists(myDataDir))
                {
                    Directory.CreateDirectory(myDataDir);
                }

                if (!String.IsNullOrEmpty(message))
                {
                    UiTools.ShowMessageBox(message, Language.GetLocalizedText(94, "WorldWide Telescope Notification"));
                }


                if (IsNewerVersion(minVersion, yourVersion))
                {
                    if (UiTools.ShowMessageBox(string.Format(Language.GetLocalizedText(95, "You must Update your client to connect to WorldWide Telescope.\n(Your version: {0}, Update version: {1})"), yourVersion, version), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        var osInfo = Environment.OSVersion;
                        if (osInfo.Version.Major < 6)
                        {
                            WebWindow.OpenUrl(updateUrl, true);
                            return false;
                        }
                        versionChecked = true;
                        pause = true;
                        if (!FileDownload.DownloadFile(updateUrl, string.Format(@"{0}\wwtsetup.msi", Path.GetTempPath()), true))
                        {
                            return false;
                        }
                        pause = false;

                        if (multiMonClient)
                        {
                            Process.Start(@"msiexec.exe", string.Format(@"/i {0}\wwtsetup.msi /q", Path.GetTempPath()));
                        }
                        else
                        {
                            Process.Start(@"msiexec.exe", string.Format(@"/i {0}\wwtsetup.msi", Path.GetTempPath()));
                        }




                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (IsNewerVersion(warnVersion, yourVersion) || (IsNewerVersion(version, yourVersion) && interactive))
                {
                    if (interactive || ShouldAutoUpdate())
                    {
                        if (UiTools.ShowMessageBox(string.Format(Language.GetLocalizedText(96, "There is a new software update available.\n(Your version: {0}, Update version: {1})"), yourVersion, version), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"), MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            var osInfo = Environment.OSVersion;
                            if (osInfo.Version.Major < 6)
                            {
                                WebWindow.OpenUrl(updateUrl, true);
                                return false;
                            }
                            versionChecked = true;
                            pause = true;
                            if (!FileDownload.DownloadFile(updateUrl, string.Format(@"{0}\wwtsetup.msi", Path.GetTempPath()), true))
                            {
                                return true;
                            }
                            pause = false;

                            if (multiMonClient)
                            {
                                Process.Start(@"msiexec.exe", string.Format(@"/i {0}\wwtsetup.msi /q", Path.GetTempPath()));
                            }
                            else
                            {
                                Process.Start(@"msiexec.exe", string.Format(@"/i {0}\wwtsetup.msi", Path.GetTempPath()));
                            }

                            return false;
                        }
                    }
                }
                else
                {
                    if (interactive)
                    {
                        UiTools.ShowMessageBox(string.Format(Language.GetLocalizedText(97, "You have the latest version.\n(Your version: {0}, Server version: {1})"), yourVersion, version), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"), MessageBoxButtons.OK);
                    }
                    versionChecked = true;

                }
                DataSetManager.DataFresh = true;

                var myDataVersionFilename = Properties.Settings.Default.CahceDirectory + "\\data\\dataversion.txt";
                if (File.Exists(myDataVersionFilename))
                {
                    var yourDataVersion = File.ReadAllText(myDataVersionFilename);
                    if (yourDataVersion != dataVersion)
                    {
                        DataSetManager.DataFresh = false;
                    }
                }
                else
                {
                    DataSetManager.DataFresh = false;
                }
                File.WriteAllText(myDataVersionFilename, dataVersion);
                return true;

            }
            catch
            {
                versionChecked = false;
                return true;
            }
            finally
            {
                if (!versionChecked)
                {
                    DataSetManager.DataFresh = true;
                }
            }
        }

        private static bool IsNewerVersion(string newVersion, string oldVersion)
        {
            var partsOld = oldVersion.Split(new[] { '.' });
            var partsNew = newVersion.Split(new[] { '.' });

            var oldNum = Convert.ToInt32(partsOld[0]) * 10000000 + Convert.ToInt32(partsOld[1]) * 10000 + Convert.ToInt32(partsOld[2]) * 10 + Convert.ToInt32(partsOld[3]);
            var newNum = Convert.ToInt32(partsNew[0]) * 10000000 + Convert.ToInt32(partsNew[1]) * 10000 + Convert.ToInt32(partsNew[2]) * 10 + Convert.ToInt32(partsNew[3]);

            return newNum > oldNum;
        }



        void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            contextPanel.QueueProgress = e.ProgressPercentage;

        }

        private void helpFlush_Click(object sender, EventArgs e)
        {
            TileCache.ClearCache();
        }



        public static double[] J2000toGalactic(double J2000RA, double J2000DEC)
        {
            var J2000pos = new[] { Math.Cos(J2000RA / 180.0 * Math.PI) * Math.Cos(J2000DEC / 180.0 * Math.PI), Math.Sin(J2000RA / 180.0 * Math.PI) * Math.Cos(J2000DEC / 180.0 * Math.PI), Math.Sin(J2000DEC / 180.0 * Math.PI) };

            var RotationMatrix = new double[3][];
            RotationMatrix[0] = new[] { -.0548755604, -.8734370902, -.4838350155 };
            RotationMatrix[1] = new[] { .4941094279, -.4448296300, .7469822445 };
            RotationMatrix[2] = new[] { -.8676661490, -.1980763734, .4559837762 };



            var Galacticpos = new double[3];
            for (var i = 0; i < 3; i++)
            {
                Galacticpos[i] = J2000pos[0] * RotationMatrix[i][0] + J2000pos[1] * RotationMatrix[i][1] + J2000pos[2] * RotationMatrix[i][2];
            }

            var GalacticL2 = Math.Atan2(Galacticpos[1], Galacticpos[0]);
            if (GalacticL2 < 0)
            {
                GalacticL2 = GalacticL2 + 2 * Math.PI;
            }
            if (GalacticL2 > 2 * Math.PI)
            {
                GalacticL2 = GalacticL2 - 2 * Math.PI;
            }

            var GalacticB2 = Math.Atan2(Galacticpos[2], Math.Sqrt(Galacticpos[0] * Galacticpos[0] + Galacticpos[1] * Galacticpos[1]));

            return new[] { GalacticL2 / Math.PI * 180.0, GalacticB2 / Math.PI * 180.0 };
        }




        public static double[] GalactictoJ2000(double GalacticL2, double GalacticB2)
        {
            var Galacticpos = new[] { Math.Cos(GalacticL2 / 180.0 * Math.PI) * Math.Cos(GalacticB2 / 180.0 * Math.PI), Math.Sin(GalacticL2 / 180.0 * Math.PI) * Math.Cos(GalacticB2 / 180.0 * Math.PI), Math.Sin(GalacticB2 / 180.0 * Math.PI) };
            var RotationMatrix = new double[3][];
            RotationMatrix[0] = new[] { -.0548755604, -.8734370902, -.4838350155 };
            RotationMatrix[1] = new[] { .4941094279, -.4448296300, .7469822445 };
            RotationMatrix[2] = new[] { -.8676661490, -.1980763734, .4559837762 };

            var J2000pos = new double[3];
            for (var i = 0; i < 3; i++)
            {
                J2000pos[i] = Galacticpos[0] * RotationMatrix[0][i] + Galacticpos[1] * RotationMatrix[1][i] + Galacticpos[2] * RotationMatrix[2][i];
            }

            var J2000RA = Math.Atan2(J2000pos[1], J2000pos[0]);
            if (J2000RA < 0)
            {
                J2000RA = J2000RA + 2 * Math.PI;
            }
            if (J2000RA > 2 * Math.PI)
            {
                J2000RA = J2000RA - 2 * Math.PI;
            }

            var J2000DEC = Math.Atan2(J2000pos[2], Math.Sqrt(J2000pos[0] * J2000pos[0] + J2000pos[1] * J2000pos[1]));

            return new[] { J2000RA / Math.PI * 180.0, J2000DEC / Math.PI * 180.0 };

        }


        private void TelescopeConnect_Click(object sender, EventArgs e)
        {


        }



        private void lookupOnWikipediaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var name = contextMenuTargetObject.Name.Replace("NGC ", "NGC");

            if (name.Length > 1 && name[0] == 'M' && Char.IsNumber(name[1]))
            {
                name = name.Replace("M", "Messier ");
            }

            WebWindow.OpenUrl(String.Format("http://en.wikipedia.org/wiki/{0}", name), false);

        }

        private void copyShortcutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var link = string.Format("http://www.worldwidetelescope.org/wwtweb/goto.aspx?object={0}&ra={1}&dec={2}&zoom={3}", contextMenuTargetObject.Name, contextMenuTargetObject.RA, contextMenuTargetObject.Dec, ZoomFactor);
            Clipboard.SetText(link);
        }

        private void copyShortcutMenuItem_Click(object sender, EventArgs e)
        {

            var constellation = constellationCheck.FindConstellationForPoint(RA, Dec);
            contextPanel.Constellation = Constellations.FullName(constellation);
            contextMenuTargetObject = new TourPlace("ViewShortcut", Dec, RA, Classification.Unidentified, constellation, ImageSetType.Sky, -1);
            var link = string.Format("http://www.worldwidetelescope.org/wwtweb/goto.aspx?object={0}&ra={1}&dec={2}&zoom={3}", contextMenuTargetObject.Name, contextMenuTargetObject.RA, contextMenuTargetObject.Dec, ZoomFactor);
            Clipboard.SetText(link);
        }
        private void lookupOnAladinToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lookupOnSEDSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebWindow.OpenUrl(String.Format("http://seds.org/~spider/ngc/ngc.cgi?{0}", contextMenuTargetObject.Name), false);
        }

        private void lookupOnSimbadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (contextMenuTargetObject.Classification == Classification.Unidentified)
            {
                WebWindow.OpenUrl(String.Format("http://simbad.u-strasbg.fr/simbad/sim-coo?CooEpoch=2000&Coord={0}h{1}d&submit=submit%20query&Radius.unit=arcmin&CooEqui=2000&CooFrame=FK5&Radius=10", contextMenuTargetObject.RA, contextMenuTargetObject.Dec > 0 ? "%2b" + contextMenuTargetObject.Dec : contextMenuTargetObject.Dec.ToString()), false);
            }
            else
            {
                WebWindow.OpenUrl(String.Format("http://simbad.u-strasbg.fr/sim-id.pl?Ident={0}", contextMenuTargetObject.Name), false);
            }

        }

        private void AscomPlatformMenuItem_Click(object sender, EventArgs e)
        {
            WebWindow.OpenUrl("http://www.ascom-standards.org", true);
        }

        private void nameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void getDSSImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebWindow.OpenUrl(String.Format("http://archive.stsci.edu/cgi-bin/dss_search?v=poss2ukstu_red&r={0}&d={1}&e=J2000&h=15.0&w=15.0&f=gif&c=none&fov=NONE&v3=", Coordinates.FormatDMS(contextMenuTargetObject.RA), Coordinates.FormatDMS(contextMenuTargetObject.Dec)), false);
        }

        private void publicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebWindow.OpenUrl(String.Format("http://adsabs.harvard.edu/cgi-bin/abs_connect?db_key=AST&sim_query=YES&object={0}", contextMenuTargetObject.Name), false);

        }


        private void getDSSFITSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var url = String.Format("http://archive.stsci.edu/cgi-bin/dss_search?v=poss2ukstu_red&r={0}&d={1}&e=J2000&h=15.0&w=15.0&f=fits&c=none&fov=NONE&v3=", Coordinates.FormatDMS(contextMenuTargetObject.RA), Coordinates.FormatDMS(contextMenuTargetObject.Dec));

            var filename = Path.GetTempFileName();

            if (!FileDownload.DownloadFile(url, filename, true))
            {
                return;
            }
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = Language.GetLocalizedText(1055, "Fits Image(*.FIT)|*.FIT");
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveDialog.AddExtension = true;
            saveDialog.DefaultExt = ".FIT";
            saveDialog.FileName = contextMenuTargetObject.Name;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.Move(filename, saveDialog.FileName);
                }
                catch
                {
                    UiTools.ShowMessageBox(Language.GetLocalizedText(99, "There was a problem saving the downloaded FITS file. Please make sure you specified a path where you have permisions to save and that has free space."), Language.GetLocalizedText(100, "Download FITS Image File"));
                }
            }
        }

        private void Earth3d_Paint(object sender, PaintEventArgs e)
        {

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {

        }


        FieldOfView fov;

        public FieldOfView Fov
        {
            get { return fov; }
            set { fov = value; }
        }
        private IImageSet previewImageset;

        public IImageSet PreviewImageset
        {
            get { return previewImageset; }
            set { previewImageset = value; }
        }

        public BlendState PreviewBlend = new BlendState(false, 500);

        private IImageSet studyImageset;



        public IImageSet videoOverlay = null;
        public float StudyOpacity
        {
            get { return (float)viewCamera.Opacity; }
            set { viewCamera.Opacity = value; }
        }

        public IImageSet StudyImageset
        {
            get { return studyImageset; }
            set
            {
                studyImageset = value;
                if (contextPanel != null)
                {
                    if ((studyImageset != null) != contextPanel.studyOpacity.Visible)
                    {
                        contextPanel.studyOpacity.Visible = (studyImageset != null);

                        if (imageStackVisible)
                        {
                            stack.UpdateList();
                        }
                    }

                    var showHist = false;
                    if (contextPanel.studyOpacity.Visible && studyImageset != null && studyImageset.WcsImage is FitsImage)
                    {
                        showHist = true;
                    }

                    if (contextPanel.scaleButton.Visible != showHist)
                    {
                        contextPanel.scaleButton.Visible = contextPanel.scaleLabel.Visible = showHist;
                    }
                }
            }
        }

        private void Earth3d_Move(object sender, EventArgs e)
        {
            if (ReadyToRender)
            {
                SetAppMode(currentMode);
            }
        }

        private void Earth3d_ResizeBegin(object sender, EventArgs e)
        {
  
        }

        private void Earth3d_ResizeEnd(object sender, EventArgs e)
        {
            pause = false;
        }

        private void Hover()
        {
            if (pause)
            {
                return;
            }

            if (!ProjectorServer)
            {

                if (CurrentImageSet.ReferenceFrame == null)
                {
                    switch (CurrentImageSet.DataSetType)
                    {
                        case ImageSetType.Earth:
                            CurrentImageSet.ReferenceFrame = "Earth";
                            break;
                        case ImageSetType.Planet:
                            break;
                        case ImageSetType.Sky:
                            CurrentImageSet.ReferenceFrame = "Sky";
                            break;
                        case ImageSetType.Panorama:
                            CurrentImageSet.ReferenceFrame = "Panorama";
                            break;
                        case ImageSetType.SolarSystem:
                            CurrentImageSet.ReferenceFrame = SolarSystemTrack.ToString();
                            break;
                        default:
                            break;
                    }

                }
                var cursor = renderWindow.PointToClient(Cursor.Position);
                if (uiController != null)
                {
                    if (uiController.Hover(cursor))
                    {
                        return;
                    }
                }
                var result = GetCoordinatesForScreenPoint(cursor.X, cursor.Y);
                // todo unify this with findclosest
                LayerManager.HoverCheckScreenSpace(cursor, CurrentImageSet.ReferenceFrame);



                if (Space)
                {
                    if (CurrentImageSet.DataSetType == ImageSetType.Sky)
                    {

                        if (constellationCheck != null)
                        {
                            var closetPlace = LayerManager.FindClosest(result, (float)(ZoomFactor / 18000.00), true, "Sky");

                            if (closetPlace == null)
                            {
                                var constellation = constellationCheck.FindConstellationForPoint(result.RA, result.Dec);
                                closetPlace = ContextSearch.FindClosestMatch(constellation, result.RA, result.Dec, ZoomFactor / 900);
                            }

                            if (ShowKmlMarkers && KmlMarkers != null)
                            {
                                closetPlace = KmlMarkers.HoverCheck(Coordinates.RADecTo3dDouble(result, 1.0).Vector311, closetPlace, (float)(ZoomFactor / 900.0));
                            }

                            if (closetPlace != null)
                            {
                                MainWindow.SetLabelText(closetPlace, true);
                            }
                            else
                            {
                                MainWindow.SetLabelText(null, false);
                            }
                        }
                    }
                }
                else
                {


                    // todo unify this with hover check..
                    var closetPlace = LayerManager.FindClosest(result, (float)(ZoomFactor / 900.00), false, CurrentImageSet.ReferenceFrame);
                    if (closetPlace != null)
                    {
                        MainWindow.SetLabelText(closetPlace, true);
                    }
                    else
                    {
                        MainWindow.SetLabelText(null, false);
                    }
                }
            }
        }

        private void Earth3d_MouseLeave(object sender, EventArgs e)
        {

        }

        private void Earth3d_MouseEnter(object sender, EventArgs e)
        {

        }
        bool CursorVisible = true;

        bool showTourCompleteDialog;

        public bool ShowTourCompleteDialog
        {
            get { return showTourCompleteDialog; }
            set { showTourCompleteDialog = value; }
        }

        private void HoverTimer_Tick(object sender, EventArgs e)
        {
            var ts = DateTime.Now - lastMouseMove;

            if (mouseMoved && ts.TotalMilliseconds > 500)
            {
                if ((TourPlayer.Playing || (fullScreen && kinectHeard)) && (Properties.Settings.Default.FullScreenTours == true && !Settings.MasterController))
                {
                    if (CursorVisible)
                    {
                        Cursor.Hide();
                        CursorVisible = false;
                    }
                }
                else
                {
                    if (!CursorVisible)
                    {
                        Cursor.Show();
                        CursorVisible = true;
                    }
                }

                Hover();
                mouseMoved = false;
            }



        }

        private void getSDSSImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var url = String.Format("http://casjobs.sdss.org/ImgCutoutDR7/getjpeg.aspx?ra={0}&dec={1}&scale=0.79224&width=800&height=800&opt=&query=", Coordinates.FormatDMS(contextMenuTargetObject.RA), Coordinates.FormatDMS(contextMenuTargetObject.Dec));
            WebWindow.OpenUrl(url, false);


        }

        public Bitmap GetScreenThumbnail()
        {

            if (Properties.Settings.Default.ShowSafeArea || Properties.Settings.Default.ShowTouchControls)
            {
                // Draw without safe areas
                TourEditor.Capturing = true;
                Render();
                Thread.Sleep(100);
                Render();
                TourEditor.Capturing = false;
            }

            try
            {
                var imgOrig = RenderContext11.GetScreenBitmap();

                var bmpThumb = new Bitmap(96, 45);

                var g = Graphics.FromImage(bmpThumb);

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                var imageAspect = ((double)imgOrig.Width) / (imgOrig.Height);

                var clientAspect = ((double)bmpThumb.Width) / bmpThumb.Height;

                var cw = bmpThumb.Width;
                var ch = bmpThumb.Height;

                if (imageAspect < clientAspect)
                {
                    ch = (int)(cw / imageAspect);
                }
                else
                {
                    cw = (int)(ch * imageAspect);
                }

                var cx = (bmpThumb.Width - cw) / 2;
                var cy = ((bmpThumb.Height - ch) / 2);// - 1;
                var destRect = new Rectangle(cx, cy, cw, ch);

                var srcRect = new Rectangle(0, 0, imgOrig.Width, imgOrig.Height);
                g.DrawImage(imgOrig, destRect, srcRect, GraphicsUnit.Pixel);
                g.Dispose();
                GC.SuppressFinalize(g);
                imgOrig.Dispose();
                GC.SuppressFinalize(imgOrig);

                return bmpThumb;
            }
            catch
            {
                var bmp = new Bitmap(96, 45);
                var g = Graphics.FromImage(bmp);
                g.Clear(Color.Blue);

                g.DrawString("Can't Capture", UiTools.StandardSmall, UiTools.StadardTextBrush, new PointF(3, 15));
                return bmp;
            }
        }

        internal bool OnTarget(IPlace place)
        {
            var ot = ((Math.Abs(ViewLat - TargetLat) < .0000000001 && Math.Abs(ViewLong - TargetLong) < .0000000001 && Math.Abs(ZoomFactor - TargetZoom) < .000000000001) && mover == null);
            return ot;
         
        }


        private void newSlideBasedTour(object sender, EventArgs e)
        {
            if (!CloseOpenToursOrAbort(false))
            {
                return;
            }
            var tour = new TourDocument();
            var tourProps = new TourProperties();
            tourProps.EditTour = tour;
            if (tourProps.ShowDialog() == DialogResult.OK)
            {
                tour.EditMode = true;
                menuTabs.AddTour(tour);
                menuTabs.FocusTour(tour);
                Undo.Clear();
            }

        }

        private void newObservingListpMenuItem_Click(object sender, EventArgs e)
        {
            explorePane.NewCollection();
        }

        private void newTimelineTour_Click(object sender, EventArgs e)
        {
           

        }

        private void newInteractiveTour_Click(object sender, EventArgs e)
        {
            

        }
        private bool CloseOpenToursOrAbort(bool silent)
        {
            if (menuTabs.CurrentTour != null)
            {
                if (tourEdit != null)
                {
                    KeyFramer.HideTimeline();
                    if (tourEdit.Tour.EditMode && tourEdit.Tour.TourDirty)
                    {
                        var result = MessageBox.Show(Language.GetLocalizedText(5, "Your tour has unsaved changes. Do you want to save the changes before closing?"), Language.GetLocalizedText(6, "Tour Editor"), MessageBoxButtons.YesNoCancel);
                        if (result == DialogResult.Yes)
                        {
                            if (!tourEdit.Save(false))
                            {
                                return false;
                            }
                        }
                        if (result == DialogResult.Cancel)
                        {
                            return false;
                        }
                    }
                    CloseTour(silent);
                    return true;
                }
            }
            return true;


        }

        private void openTourMenuItem_Click(object sender, EventArgs e)
        {
            if (!CloseOpenToursOrAbort(false))
            {
                return;
            }

            var openFile = new OpenFileDialog();
            openFile.Filter = Language.GetLocalizedText(101, "WorldWide Telescope Tours") + "|*.wtt";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var filename = openFile.FileName;
                try
                {
                    LoadTourFromFile(filename, true, "");
                }
                catch
                {
                    MessageBox.Show(Language.GetLocalizedText(102, "This file does not seem to be a valid tour"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"));
                }
            }
        }

        public TourDocument LoadTourFromFile(string filename, bool editMode, string tagId)
        {

            if (!CloseOpenToursOrAbort(false))
            {
                return null;
            }

            if (!File.Exists(filename))
            {
                MessageBox.Show(Language.GetLocalizedText(103, "The tour file could not be downloaded and was not cached. Check you network connection."), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"));
                return null;
            }

            var fi = new FileInfo(filename);
            if (fi.Length == 0)
            {
                File.Delete(filename);
                MessageBox.Show(Language.GetLocalizedText(104, "The tour file could not be downloaded and was not cached. Check you network connection."), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"));
                return null;
            }
            if (fi.Length < 100)
            {
                MessageBox.Show(Language.GetLocalizedText(105, "The tour file is invalid. Check you network connection."), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"));
                File.Delete(filename);
                return null;
            }

            if (Settings.MasterController && Properties.Settings.Default.AutoSyncTours)
            {
                editMode = true;
            }


            Undo.Clear();
            LayerManager.TourLayers = !editMode;
            var tour = TourDocument.FromFile(filename, editMode);



            if (tour != null)
            {
                tour.TagId = tagId;
                tour.EditMode = editMode;
                menuTabs.AddTour(tour);
                menuTabs.FocusTour(tour);

                if (NoUi)
                {
                    if (tourEdit == null)
                    {
                        tourEdit = new TourEditTab();
                        tourEdit.Owner = this;
                    }
                    tourEdit.Tour = tour;

                   
                    Properties.Settings.Default.AutoRepeatTour = true;
                    tourEdit.PlayNow(true);
                   
                }
                if (Settings.MasterController && Properties.Settings.Default.AutoSyncTours)
                {
                    SyncTour();
                }
            }
            else
            {
                MessageBox.Show(Language.GetLocalizedText(106, "Could not load tour"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"));
            }
            return tour;
        }

        private void openObservingListMenuItem_Click(object sender, EventArgs e)
        {
           
            var openFile = new OpenFileDialog();
            openFile.Filter = Language.GetLocalizedText(107, "WorldWide Telescope Collection") + "|*.wtml";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var filename = openFile.FileName;

                try
                {
                    LoadFolder(filename);
                }
                catch
                {
                    MessageBox.Show(Language.GetLocalizedText(109, "This file does not seem to be a valid collection"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"));
                }
            }

        }

        private void LoadFolder(string filename)
        {
            var newFolder = Folder.LoadFromFile(filename, false);

            if (newFolder.Group == FolderGroup.Goto)
            {
                if (newFolder.Children[0] is Place)
                {
                    GotoTarget((IPlace)newFolder.Children[0], false, false, true);
                }
            }
            else if (newFolder.Group == FolderGroup.Community)
            {
                AddCommunity(newFolder);
            }
            else if (newFolder.Group == FolderGroup.ImageStack)
            {
                ImageStackVisible = true;
                LoadImageStack(newFolder, true);
                if (newFolder.Browseable == FolderBrowseable.True && !ProjectorServer)
                {
                    if (!explorePane.IsCollectionLoaded(filename, true))
                    {
                        explorePane.LoadCollection(newFolder);
                    }
                    PlayCollection();
                    ShowFullScreen(true);
                }

            }
            else
            {
                if (!explorePane.IsCollectionLoaded(filename, true))
                {
                    explorePane.LoadCollection(newFolder);
                }
            }
        }
        bool firstImageLoaded = true;
        private void LoadImageStack(Folder newFolder, bool showFirstAsBackground)
        {
            firstImageLoaded = showFirstAsBackground;
            AddClidrenToStack(newFolder, showFirstAsBackground);

            Stack.UpdateList();
        }

        IPlace lastAddedToStack = null;
        public void AddClidrenToStack(Folder folder, bool showFirstAsBackground)
        {

            foreach (var o in folder.Children)
            {
                if (o is Folder)
                {
                    AddClidrenToStack((Folder)o, false);
                }
                else
                {
                    if (o is Place)
                    {
                        if (showFirstAsBackground && firstImageLoaded)
                        {
                            SetCurrentBackgroundForStack((Place)o);
                        }
                        else
                        {
                            AddPlaceToStack((Place)o, false);
                        }
                    }
                    else if (o is IImageSet)
                    {
                        var imageSet = (IImageSet)o;
                        var tp = new TourPlace(imageSet.Name, imageSet.CenterX, imageSet.CenterY, Classification.Unidentified, "", imageSet.DataSetType, 360);
                        if (showFirstAsBackground && firstImageLoaded)
                        {
                            SetCurrentBackgroundForStack(tp);
                        }
                        else
                        {
                            AddPlaceToStack(tp, false);
                        }
                    }
                }
            }
        }

        void SetCurrentBackgroundForStack(IPlace place)
        {
            IImageSet imageSet = null;
            if (place.StudyImageset != null)
            {
                imageSet = place.StudyImageset;
            }
            if (place.BackgroundImageSet != null)
            {
                imageSet = place.BackgroundImageSet;
            }

            CurrentImageSet = imageSet;
            firstImageLoaded = false;
        }


        private void AddCommunity(Folder newFolder)
        {
            var filename = CommuinitiesDirectory + Math.Abs(newFolder.Url.GetHashCode32()) + ".wtml";
            if (!Directory.Exists(CommuinitiesDirectory))
            {
                Directory.CreateDirectory(CommuinitiesDirectory);
            }

            if (File.Exists(filename))
            {
                UiTools.ShowMessageBox(Language.GetLocalizedText(110, "The file opened is a community registration file and this community is already registered."));
                communitiesPane.LoadCommunities();
                menuTabs.SetSelectedIndex((int)ApplicationMode.Explore, false);
                Refresh();
                menuTabs.SetSelectedIndex((int)ApplicationMode.Community, false);
                return;
            }

            if (UiTools.ShowMessageBox(Language.GetLocalizedText(111, "The file opened is a community registration file. Would you like to add this to your communities list?"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"), MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            newFolder.SaveToFile(filename);
            communitiesPane.LoadCommunities();
            menuTabs.SetSelectedIndex((int)ApplicationMode.Explore, false);
            Refresh();
            menuTabs.SetSelectedIndex((int)ApplicationMode.Community, false);
        }


        public static string CommuinitiesDirectory
        {
            get { return Properties.Settings.Default.CahceDirectory + "Communities\\"; }
        }

        private void homepageMenuItem_Click(object sender, EventArgs e)
        {
            WebWindow.OpenUrl("http://www.worldwidetelescope.org/", true);
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.ShowDialog(this);
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        internal KmlCollection MyPlaces = new KmlCollection();
        private void openKMLMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private bool OpenKmlFile(string filename)
        {
            try
            {
                ShowLayersWindows = true;
                LayerManager.LoadLayer(filename, "Earth", true, false);
                return true;
            }
            catch
            {
                return false;
            }


        }
        private void openImageMenuItem_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = Language.GetLocalizedText(979, "Images(*.JPG;*.PNG;*.TIF;*.TIFF;*.FITS;*.FIT)|*.JPG;*.PNG;*.TIF;*.TIFF;*.FITS;*.FIT");
             openFile.RestoreDirectory = true;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFile.FileName))
                {
                    var filename = openFile.FileName;

                    LoadImage(filename);
                }

            }
        }

        private void LoadImage(string filename)
        {
            var wcsImage = WcsImage.FromFile(filename);

            var hasAvm = wcsImage.ValidWcs;
            {
                var bmp = wcsImage.GetBitmap();
                wcsImage.AdjustScale(bmp.Width, bmp.Height);

                ImageSetHelper imageSet = null;
                TourPlace place = null;
                if (hasAvm)
                {
                    imageSet = new ImageSetHelper(wcsImage.Description, filename, ImageSetType.Sky, BandPass.Visible, ProjectionType.SkyImage, Math.Abs(filename.GetHashCode32()), 0, 0, 256, wcsImage.ScaleY, ".tif", wcsImage.ScaleX > 0, "", wcsImage.CenterX, wcsImage.CenterY, wcsImage.Rotation, false, "", false, false, 1, wcsImage.ReferenceX, wcsImage.ReferenceY, wcsImage.Copyright, wcsImage.CreditsUrl, "", "", 0, "");
                    place = new TourPlace(UiTools.GetNamesStringFromArray(wcsImage.Keywords.ToArray()), wcsImage.CenterY, wcsImage.CenterX / 15, Classification.Unidentified, constellationCheck.FindConstellationForPoint(wcsImage.CenterX, wcsImage.CenterY), ImageSetType.Sky, -1);
                }
                else
                {

                    imageSet = new ImageSetHelper(wcsImage.Description, filename, ImageSetType.Sky, BandPass.Visible, ProjectionType.SkyImage, Math.Abs(filename.GetHashCode32()), 0, 0, 256, .001, ".tif", false, "", RA * 15, ViewLat, 0, false, "", false, false, 1, bmp.Width / 2, bmp.Height / 2, wcsImage.Copyright, wcsImage.CreditsUrl, "", "", 0, "");
                    place = new TourPlace(UiTools.GetNamesStringFromArray(wcsImage.Keywords.ToArray()), ViewLat, RA, Classification.Unidentified, constellationCheck.FindConstellationForPoint(wcsImage.CenterX, wcsImage.CenterY), ImageSetType.Sky, -1);
                }
                imageSet.WcsImage = wcsImage;
                place.StudyImageset = imageSet;
                place.Tag = wcsImage;
                var pl = Place.FromIPlace(place);

                pl.ThumbNail = UiTools.MakeThumbnail(bmp);
                StudyImageset = pl.StudyImageset;
                GotoTarget(pl, false, false, true);



                explorePane.OpenImages.AddChildPlace(pl);

                bmp.Dispose();
                GC.SuppressFinalize(bmp);
                bmp = null;

                explorePane.ShowOpenImages();

            }
            if (!hasAvm)
            {
                MessageBox.Show(Language.GetLocalizedText(112, "The image file did not contain recognizable WCS or AVM Metadata to position it in the sky"), Language.GetLocalizedText(113, "Load Image"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }



        private void menuTabs_Load(object sender, EventArgs e)
        {

        }

        private void tourHomeMenuItem_Click(object sender, EventArgs e)
        {
            WebWindow.OpenUrl("http://www.worldwidetelescope.org/Learn/Exploring#guidedtours", true);
        }

        private void tourSearchWebPageMenuItem_Click(object sender, EventArgs e)
        {
            WebWindow.OpenUrl("http://www.worldwidetelescope.org/Community", true);
        }

        private void favoriteMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void gettingStarteMenuItem_Click(object sender, EventArgs e)
        {
            WebWindow.OpenUrl("http://www.worldwidetelescope.org/Support/Index", true);
        }

        private void exportTourToFileMenuItem_Click(object sender, EventArgs e)
        {
            // Export
            if (tourEdit != null)
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = Language.GetLocalizedText(101, "WorldWide Telescope Tours") + "|*.wtt";
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveDialog.AddExtension = true;
                saveDialog.DefaultExt = ".WTT";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    tourEdit.Tour.SaveToFile(saveDialog.FileName);
                }
            }
        }

        private void publishTourMenuItem_Click(object sender, EventArgs e)
        {
            if (tourEdit != null)
            {
                UiTools.ShowMessageBox(Language.GetLocalizedText(114, "Respect Copyright. Please respect the rights of artists and creators. Content such as music is protected by copyright. The music made available to you in WorldWide Telescope is protected by copyright and may be used for the sole purpose of creating tours in WorldWide Telescope. You may not share other people's content unless you own the rights or have permission from the owner."), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"));
                var webService = new WWTWebService();
                webService.Timeout = 1000000;
                var tempFile = Path.GetTempFileName();
                try
                {
                    tourEdit.Tour.SaveToFile(tempFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Language.GetLocalizedText(115, "There was a problem saving the tour.") + ex.Message, Language.GetLocalizedText(116, "Submission Failed"));
                }
                var tourXML = tourEdit.Tour.GetXmlSubmitString();
                var tourBlob = UiTools.LoadBlob(tempFile);
                File.Delete(tempFile);

                var tourThumbBlob = UiTools.LoadBlob(tourEdit.Tour.TourThumbnailFilename);
                byte[] authorThumbBlob = null;
                try
                {

                    authorThumbBlob = UiTools.LoadBlob(tourEdit.Tour.AuthorThumbnailFilename);
                }
                catch
                {
                    MessageBox.Show(Language.GetLocalizedText(117, "There was no author image. Please add one and submit again."), Language.GetLocalizedText(116, "Submission Failed"));
                    return;
                }
                org.worldwidetelescope.www.Tour[] results = null;
                try
                {
                    results = webService.ImportTour(tourXML, tourBlob, tourThumbBlob, authorThumbBlob);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Language.GetLocalizedText(118, "There was a problem submitting the tour.") + ex.Message, Language.GetLocalizedText(116, "Submission Failed"));
                    return;
                }

                if (results != null && results.Length > 0)
                {
                    MessageBox.Show(Language.GetLocalizedText(119, "The tour was successfully submitted for publication. It must undergo technical review and approval before it will appear on the tour directory."), Language.GetLocalizedText(120, "Submit Tour for Publication"));
                }
                else
                {
                    MessageBox.Show(Language.GetLocalizedText(121, "There was a problem submitting the tour. Please review the tour content and make sure all fields are filled in correctly and that the total content size is below in maximum tour size."), Language.GetLocalizedText(120, "Submit Tour for Publication"));
                }

            }

        }

        private void slewTelescopeMenuItem_Click(object sender, EventArgs e)
        {
            if (telescopePane != null)
            {
                telescopePane.SlewScope_Click(sender, e);
            }
        }

        private void centerTelescopeMenuItem_Click(object sender, EventArgs e)
        {
            if (telescopePane != null)
            {
                telescopePane.CenterScope_Click(sender, e);
            }
        }

        private void SyncTelescopeMenuItem_Click(object sender, EventArgs e)
        {
            if (telescopePane != null)
            {
                telescopePane.SyncScope_Click(sender, e);
            }
        }

        private void chooseTelescopeMenuItem_Click(object sender, EventArgs e)
        {
            if (telescopePane != null)
            {
                telescopePane.Choose_Click(sender, e);
            }
        }

        private void connectTelescopeMenuItem_Click(object sender, EventArgs e)
        {
            if (telescopePane != null)
            {
                telescopePane.ConnectScope_Click(sender, e);
            }
        }

        private void trackScopeMenuItem_Click(object sender, EventArgs e)
        {
            if (telescopePane != null)
            {
                telescopePane.TrackScope.Checked = !telescopePane.TrackScope.Checked;
                trackScopeMenuItem.Checked = telescopePane.TrackScope.Checked;
            }
        }

        private void parkTelescopeMenuItem_Click(object sender, EventArgs e)
        {
            if (telescopePane != null)
            {
                telescopePane.Park_Click(sender, e);
            }

        }



        private void telescopeMenu_Opening(object sender, CancelEventArgs e)
        {
            if (telescopePane == null)
            {
                telescopePane = new TelescopeTab();
                telescopePane.Owner = this;
            }
            if (telescopePane != null)
            {
                trackScopeMenuItem.Checked = telescopePane.TrackScope.Checked;
                var state = telescopePane.TelescopeConnected;

                centerTelescopeMenuItem.Enabled = state;
                slewTelescopeMenuItem.Enabled = state;
                trackScopeMenuItem.Enabled = state;
                if (state)
                {
                    SyncTelescopeMenuItem.Enabled = telescopePane.Scope.CanSync && telescopePane.Scope.Tracking;
                    if (telescopePane.Scope.AtPark)
                    {
                        parkTelescopeMenuItem.Text = Language.GetLocalizedText(122, "Unpark");
                        parkTelescopeMenuItem.Enabled = telescopePane.Scope.CanUnpark;
                    }
                    else
                    {
                        parkTelescopeMenuItem.Text = Language.GetLocalizedText(50, "Park");
                        parkTelescopeMenuItem.Enabled = telescopePane.Scope.CanPark;
                    }
                }
                else
                {
                    SyncTelescopeMenuItem.Enabled = state;
                    parkTelescopeMenuItem.Enabled = state;
                }

                if (state)
                {
                    connectTelescopeMenuItem.Text = Language.GetLocalizedText(123, "Disconnect");
                }
                else
                {
                    connectTelescopeMenuItem.Text = Language.GetLocalizedText(48, "Connect");
                }
            }
        }

        private void resetCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetCamera();

        }

        private void ResetCamera()
        {
            var camParams = new CameraParameters(0, 0, 360, 0, 0, 100);
            GotoTarget(camParams, false, true);
        }



        bool showPerfData;
        private void showPerformanceDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPerfData = !showPerfData;
            showPerformanceDataToolStripMenuItem.Checked = showPerfData;
            Text = Language.GetLocalizedText(3, "Microsoft WorldWide Telescope");
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Settings.MasterController)
            {
                try
                {
                    if (!CheckForUpdates(true))
                    {
                        Close();
                    }
                }
                catch
                {
                    MessageBox.Show(Language.GetLocalizedText(124, "Could not connect to the server to check for updates. Try again later."), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"));
                }
            }
            else
            {
                autoUpdate = true;
                SendMove();
            }
        }



        private void StatupTimer_Tick(object sender, EventArgs e)
        {
            if (Initialized)
            {
                StatupTimer.Enabled = false;
                Activate();
                SetAppMode(ApplicationMode.Explore);
                ResetStartFlag();
                try
                {
                    if (!string.IsNullOrEmpty(launchTourFile))
                    {
                        if (Path.GetExtension(launchTourFile) == ".wwtl")
                        {
                            LayerManager.LoadLayerFile(launchTourFile, "Sun", false);
                        }
                        else if (Path.GetExtension(launchTourFile) == ".wtml")
                        {
                            LoadFolder(launchTourFile);
                        }
                        else if (Path.GetExtension(launchTourFile) == ".wwtfig")
                        {
                            SetAppMode(ApplicationMode.View);
                            if (viewPane != null)
                            {
                                settingsPane.InstallNewFigureFile(launchTourFile);
                            }
                        }
                        else if (Path.GetExtension(launchTourFile) == ".wtt")
                        {
                            LoadTourFromFile(launchTourFile, false, "");
                        }
                        else if (Path.GetExtension(launchTourFile) == ".kml" || Path.GetExtension(launchTourFile) == ".kmz")
                        {
                            OpenKmlFile(launchTourFile);
                        }
                        launchTourFile = "";
                    }
                    else
                    {
                        if (config.Master == true)
                        {
                            if (Properties.Settings.Default.ShowNavHelp)
                            {
                                ShowWelcome();
                            }

                            if (Properties.Settings.Default.ShowJoystickHelp && ControllerConnected())
                            {
                                var joystick = new JoystickHelp();
                                joystick.ShowDialog();
                            }
                        }
                    }
                    if (config.Master == false)
                    {
                        NetControl.ReportStatus(ClientNodeStatus.Online, "Ready", "");
                    }

                }
                catch
                {
                }
            }
        }

        private static void ShowWelcome()
        {
            var welcome = new Welcome();
            welcome.ShowDialog();
        }



        private void PopupClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            menuTabs.Thaw();

        }

        static public bool IsInSDSSFootprint(double ra, double dec)
        {
            if (!((ra * 15) > 270 | dec < -3 | (ra * 15) < 105 | dec > 75))
            {
                return true;
            }
            return false;
        }

        private void downloadQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var queueList = new Queue_List();
            queueList.Show();
        }

        private void sIMBADSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SimbadSearch searchDialog;
                var foundOrCanceled = false;
                var notFound = false;
                var targetName = "";

                if (searchPane != null)
                {
                    targetName = searchPane.SearchStringText;
                }
                while (!foundOrCanceled)
                {
                    searchDialog = new SimbadSearch();
                    searchDialog.ObejctName = targetName;

                    if (notFound)
                    {
                        searchDialog.Text = Language.GetLocalizedText(125, "SIMBAD Search - Not Found");
                    }
                    if (searchDialog.ShowDialog() == DialogResult.OK)
                    {
                        targetName = searchDialog.ObejctName;
                        var lookup = new ObjectLookup();

                        AstroObjectResult result = null;

                        if (Space)
                        {
                            result = lookup.SkyLookup(targetName);
                        }
  

                        if (result != null)
                        {
                            if (Space)
                            {
                                GotoTarget(false, false, new CameraParameters(result.Dec, RAtoViewLng(result.RA), -1, 0, 0, 1.0f), null, null);
                            }
                            else
                            {
                                GotoTarget(false, false, new CameraParameters(result.Dec, result.RA, -1, 0, 0, 1.0f), null, null);
                            }
                            foundOrCanceled = true;

                        }
                        else
                        {
                            foundOrCanceled = false;
                            notFound = true;
                        }
                    }
                    else
                    {
                        foundOrCanceled = true;
                    }

                }
            }
            catch
            {
                UiTools.ShowMessageBox(Language.GetLocalizedText(126, "Could not connect to SIMBAD Name Resolution Server. Check you internet connection"));
            }
        }

        private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebWindow.OpenUrl("http://www.worldwidetelescope.org/Support/IssuesAndBugs", true);
        }



        private void editTourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (menuTabs.CurrentTour != null)
            {
                if (!menuTabs.CurrentTour.EditMode)
                {
                    LayerManager.MergeToursLayers();
                }

                menuTabs.CurrentTour.EditMode = true;
                if (tourEdit != null)
                {
                    tourEdit.SetEditMode(true);
                    tourEdit.PauseTour();
                }
                menuTabs.FocusTour(menuTabs.CurrentTour);
            }
        }
        private void hLAFootprintsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var footprint = new HSTFootprint();

            var region = footprint.ACS_ConeFootprintL1((contextMenuTargetObject.RA * 15), contextMenuTargetObject.Dec, fovAngle);

        }
       

        private void uSNONVOConeSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var url = String.Format("http://nedwww.ipac.caltech.edu/cgi-bin/nph-objsearch?search_type=Near+Position+Search&of=xml_main&RA={0}&DEC={1}&SR={2}", (contextMenuTargetObject.RA * 15), contextMenuTargetObject.Dec, fovAngle);
            var client = new WebClient();

            try
            {
                var data = client.DownloadString(url);
                var doc = new XmlDocument();
                doc.LoadXml(data);
                var voTable = new VoTable(doc);
   
                var layer = LayerManager.AddVoTableLayer(voTable, "VO Table");
                var viewer = new VOTableViewer();
                viewer.Layer = layer;

                viewer.Show();
                ShowLayersWindows = true;
            }
            catch
            {
                WebWindow.OpenUrl(url, true);
            }
  
        }

        private void saveCurrentViewImageToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {



            try
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = Language.GetLocalizedText(978, "Portable Network Graphics(*.png)|*.png|JPEG Image(*.jpg)|*.jpg");
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveDialog.AddExtension = true;
                saveDialog.DefaultExt = ".png";
                saveDialog.FileName = "SavedView.png";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    if (Properties.Settings.Default.ShowSafeArea || Properties.Settings.Default.ShowTouchControls)
                    {
                        // Draw without safe areas
                        TourEditor.Capturing = true;
                        Render();
                        Thread.Sleep(100);
                        Render();
                        TourEditor.Capturing = false;
                    }

                    if (saveDialog.FileName.ToLower().EndsWith(".jpg") || saveDialog.FileName.ToLower().EndsWith(".jpeg"))
                    {
                        RenderContext11.SaveBackBuffer(saveDialog.FileName, ImageFileFormat.Jpg);
                    }
                    else
                    {
                        RenderContext11.SaveBackBuffer(saveDialog.FileName, ImageFileFormat.Png);
                    }
                }
            }
            catch
            {
                UiTools.ShowMessageBox(Language.GetLocalizedText(1051, "There was a problem capturing the screen contents"));
            }
        }
        private void copyCurrentViewToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.ShowSafeArea || Properties.Settings.Default.ShowTouchControls)
            {
                // Draw without safe areas
                TourEditor.Capturing = true;
                Render();
                Thread.Sleep(100);
                Render();
                TourEditor.Capturing = false;
            }

            try
            {
                var bmp = RenderContext11.GetScreenBitmap();
                Clipboard.SetImage(bmp);
                bmp.Dispose();
                GC.SuppressFinalize(bmp);
            }
            catch
            {
                using (var bmp = new Bitmap(196, 45))
                {
                    var g = Graphics.FromImage(bmp);
                    g.Clear(Color.Blue);

                    g.DrawString("Can't Capture Screenshot", UiTools.StandardSmall, UiTools.StadardTextBrush, new PointF(3, 15));
                    Clipboard.SetImage(bmp);
                }
            }
        }

        private void setCurrentViewAsWindowsDesktopBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowFullScreen(true);
                var showCrossHairs = Properties.Settings.Default.ShowCrosshairs;
                Properties.Settings.Default.ShowCrosshairs = false;
                Render();
                Render();
                Properties.Settings.Default.ShowCrosshairs = showCrossHairs;
                var path = Properties.Settings.Default.CahceDirectory + "wallpaper.bmp";

                RenderContext11.SaveBackBuffer(path, ImageFileFormat.Bmp);
                UiTools.SetWallpaper(path);
                ShowFullScreen(false);
            }
            catch
            {

            }
        }

        private void viewMenu_Opening(object sender, CancelEventArgs e)
        {
            fullDomeToolStripMenuItem.Checked = Properties.Settings.Default.DomeView;
            detachMainViewToSecondMonitor.Enabled = Screen.AllScreens.Length > 1;
            detachMainViewToThirdMonitorToolStripMenuItem.Enabled = Screen.AllScreens.Length > 2;
            var id = -1;
            var index = 0;
            if (renderHost != null)
            {
                var screen = Screen.FromControl(renderHost);
                foreach (var s in Screen.AllScreens)
                {
                    if (s.DeviceName == screen.DeviceName)
                    {
                        id = index;
                    }
                    index++;
                }
            }

            detachMainViewToSecondMonitor.Checked = (id == 1 || id == 0);
            detachMainViewToThirdMonitorToolStripMenuItem.Checked = (id == 2);

            showTouchControlsToolStripMenuItem.Checked = Properties.Settings.Default.ShowTouchControls;
            monochromeStyleToolStripMenuItem.Checked = Properties.Settings.Default.MonochromeImageStyle;
            monochromeStyleToolStripMenuItem.Visible = false;
            lockVerticalSyncToolStripMenuItem.Checked = Properties.Settings.Default.FrameSync;
            allowUnconstrainedTiltToolStripMenuItem.Checked = Properties.Settings.Default.UnconstrainedTilt;

            exportCurrentViewAsSTLFileFor3DPrintingToolStripMenuItem.Enabled = PlanetLike;
        }

        internal void joinCoomunityMenuItem_Click(object sender, EventArgs e)
        {

            JoinCommunity();

        }

        internal void JoinCommunity()
        {
            //RG: The layerscape home page is now {rootdomain}/Community
            WebWindow.OpenUrl(Properties.Settings.Default.CloudCommunityUrlNew + "/Community", true);
        }

        private void showFinderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFinder();
        }

        private void ShowFinder()
        {
            ShowPropertiesForPoint(new Point(renderWindow.ClientSize.Width / 2, renderWindow.ClientSize.Height / 2));
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var pnt = Cursor.Position;
            Focus();
            ObjectProperties.ShowNofinder(contextMenuTargetObject, pnt);
        }

        private void restoreDefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Language.GetLocalizedText(2, "This will restore user settings to their default values. Are you sure you want to proceed?"), Language.GetLocalizedText(936, "Restore Default Settings"), MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                resetProperties = true;
                Properties.Settings.Default.Reset();
                Properties.Settings.Default.UpgradeNeeded = false;
            }
        }

        private void exploreMenu_Opening(object sender, CancelEventArgs e)
        {
            playCollectionAsSlideShowToolStripMenuItem.Checked = playingSlideShow;
            showFinderToolStripMenuItem.Enabled = CurrentImageSet.DataSetType == ImageSetType.Sky || SolarSystemMode;
        }

        private void renderWindow_Click(object sender, EventArgs e)
        {
            if (uiController != null)
            {
                if (uiController.Click(sender, e))
                {
                    return;
                }
            }
        }

        private void renderWindow_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void renderWindow_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (activeTouch != TouchControls.None)
            {
                return;
            }

            if (uiController != null)
            {
                if (uiController.MouseDoubleClick(sender, e))
                {
                    return;
                }
            }

            var result = GetCoordinatesForScreenPoint(e.X, e.Y);
            if (Space)
            {
                GotoTarget(false, false, new CameraParameters(result.Dec, RAtoViewLng(result.RA), viewCamera.Zoom > ZoomMin ? viewCamera.Zoom / 2 : viewCamera.Zoom, viewCamera.Rotation, viewCamera.Angle, (float)viewCamera.Opacity), studyImageset, CurrentImageSet);
            }
            else
            {
                TargetLong += (e.X - (Width / 2)) * GetPixelScaleX(false);
                TargetLat -= (e.Y - (Height / 2)) * GetPixelScaleY();
                TargetLong = ((TargetLong + 180.0) % 360.0) - 180.0;
                TargetLat = ((TargetLat + 90.0) % 180.0) - 90.0;
                deltaLat = (TargetLat - ViewLat) / 20;
                deltaLong = (TargetLong - ViewLong) / 20;
                zoomSpeed = (ZoomSpeeds)Properties.Settings.Default.ZoomSpeed;
                ZoomIn();
            }
        }




        private void renderWindow_MouseDown(object sender, MouseEventArgs e)
        {
            Focus();
            if (uiController != null)
            {
                if (uiController.MouseDown(sender, new MouseEventArgs(e.Button, e.Clicks, e.X + Properties.Settings.Default.ScreenHitTestOffsetX, e.Y + Properties.Settings.Default.ScreenHitTestOffsetY, e.Delta)))
                {
                    return;
                }
            }



            if (ModifierKeys == (Keys.Control | Keys.Alt | Keys.Shift))
            {
                // Contrast code here
                contrastMode = true;
                return;
            }
            contrastMode = false;



            if (ProcessTouchControls(e))
            {
                return;
            }


            if (e.Button == MouseButtons.Left)
            {
                if (CurrentImageSet != null && CurrentImageSet.ReferenceFrame == null)
                {
                    switch (CurrentImageSet.DataSetType)
                    {
                        case ImageSetType.Earth:
                            CurrentImageSet.ReferenceFrame = "Earth";
                            break;
                        case ImageSetType.Planet:
                            break;
                        case ImageSetType.Sky:
                            CurrentImageSet.ReferenceFrame = "Sky";
                            break;
                        case ImageSetType.Panorama:
                            CurrentImageSet.ReferenceFrame = "Panorama";
                            break;
                        case ImageSetType.SolarSystem:
                            CurrentImageSet.ReferenceFrame = SolarSystemTrack.ToString();
                            break;
                        default:
                            break;
                    }
                }
                if (CurrentImageSet != null)
                {
                    if (LayerManager.ClickCheckScreenSpace(new Point(e.X, e.Y), CurrentImageSet.ReferenceFrame))
                    {
                        return;
                    }
                }


            }


            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle)
            {

                if (ModifierKeys == Keys.Alt || ModifierKeys == Keys.Control || e.Button == MouseButtons.Middle)
                {
                    spinning = true;
                    angle = true;
                }
                else if (ModifierKeys == Keys.Shift || measuring)
                {
                    if (Space)
                    {
                        measuringDrag = true;
                        measureEnd = measureStart = GetCoordinatesForScreenPoint(e.X, e.Y);
                        if (measureLines != null)
                        {
                            measureLines.Clear();
                        }
                    }
                }
                else
                {
                    dragging = true;
                }
                moved = false;
                mouseDownX = e.X;
                mouseDownY = e.Y;
            }


        }

        enum TouchControls { ZoomIn, ZoomOut, Up, Down, Left, Right, Clockwise, CounterClockwise, TiltUp, TiltDown, TrackBall, Finder, Home, None, ZoomTrack, PanTrack, OrbitTrack };
        List<Vector2d> touchPoints = new List<Vector2d>();
        TouchControls activeTouch = TouchControls.None;
        Point touchTrackBallCenter;

        bool contrastMode;

        double zoomTrackerRadius = 76;
        Vector2d zoomTracker = new Vector2d(99, -15);
        Vector2d panTracker = new Vector2d(98, 77);
        readonly Vector2d orbitTracker = new Vector2d(99, 187);

        public bool Friction
        {
            get
            {
                return Properties.Settings.Default.NavigationHold;
            }
            set
            {
                if (Properties.Settings.Default.NavigationHold != value)
                {
                    Properties.Settings.Default.NavigationHold = value;

                    if (Properties.Settings.Default.NavigationHold)
                    {
                         activeTouch = TouchControls.None;
                    }
                }

            }
        }


        private bool ProcessTouchControls(MouseEventArgs e)
        {
            activeTouch = TouchControls.None;
            if (Properties.Settings.Default.ShowTouchControls)
            {
                MakeTouchPoints();

                var tX = e.X + Properties.Settings.Default.ScreenHitTestOffsetX;
                var tY = e.Y + Properties.Settings.Default.ScreenHitTestOffsetY;

                if (tX > (renderWindow.Width - 207) && tX < (renderWindow.Width - 10))
                {
                    if (tY > renderWindow.Height - (234 + 120 + 30) && tY < (renderWindow.Height - 10))
                    {

                        var hit = new Point(tX - (renderWindow.Width - 207), tY - (renderWindow.Height - (234 + 120)));

                        for (var i = touchPoints.Count - 1; i >= 0; i--)
                        {
                            var test = new Vector2d(hit.X, hit.Y);
                            test = test - touchPoints[i];
                            if (test.Length < 15)
                            {
                                activeTouch = (TouchControls)i;
                                if (activeTouch == TouchControls.TrackBall)
                                {
                                    touchTrackBallCenter = new Point(tX, tY);
                                    moveVector = new PointF();
                                    touchPoints[(int)TouchControls.PanTrack] = panTracker;
                                }
                                timer.Enabled = true;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private void MakeTouchPoints()
        {
            if (touchPoints == null || touchPoints.Count == 0)
            {
                touchPoints = new List<Vector2d>();
                touchPoints.Add(new Vector2d(45, 23)); // ZoomIn
                touchPoints.Add(new Vector2d(153, 23));// ZoomOut
                touchPoints.Add(new Vector2d(97, 32));// Up
                touchPoints.Add(new Vector2d(97, 120));// Down
                touchPoints.Add(new Vector2d(52, 77));// Left
                touchPoints.Add(new Vector2d(143, 77));// Right
                touchPoints.Add(new Vector2d(29, 117));// Rotate Left
                touchPoints.Add(new Vector2d(166, 117));// Rotate Right
                touchPoints.Add(new Vector2d(97, 161));// Tilt Up
                touchPoints.Add(new Vector2d(97, 212));// Tilt Down
                touchPoints.Add(new Vector2d(97, 76));// Track Ball
                touchPoints.Add(new Vector2d(172, 199));// Finder
                touchPoints.Add(new Vector2d(25, 199));// Home
                touchPoints.Add(new Vector2d(500, 500));// None
                touchPoints.Add(zoomTracker);// zoomTracker
                touchPoints.Add(panTracker);// panTracker
                touchPoints.Add(orbitTracker);// orbitTracker
            }
        }

        private bool ProcessKioskControls(MouseEventArgs e)
        {
            activeTouch = TouchControls.None;
            timer.Enabled = false;
            if (Properties.Settings.Default.ShowTouchControls)
            {
                MakeTouchPoints();

                var tX = e.X + Properties.Settings.Default.ScreenHitTestOffsetX;
                var tY = e.Y + Properties.Settings.Default.ScreenHitTestOffsetY;

                if (tX > (renderWindow.Width - 207) && tX < (renderWindow.Width - 10))
                {
                    if (tY > renderWindow.Height - (234 + 120) && tY < (renderWindow.Height - 10))
                    {
                        moveVector = new PointF();
                        var hit = PointToTouch(new Point(tX, tY));
                        for (var i = 0; i < touchPoints.Count; i++)
                        {
                            var test = new Vector2d(hit.X, hit.Y);
                            test = test - touchPoints[i];
                            if (test.Length < 11)
                            {
                                activeTouch = (TouchControls)i;
                                if (activeTouch == TouchControls.TrackBall)
                                {
                                    touchTrackBallCenter = new Point(tX, tY);
                                }
                                timer.Enabled = true;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private Point PointToTouch(Point pnt)
        {
            return new Point(pnt.X - (renderWindow.Width - 207), pnt.Y - (renderWindow.Height - (234 + 120)));
        }

        private Point TouchToScreen(Point pnt)
        {
            return new Point(pnt.X + (renderWindow.Width - 207), pnt.Y + (renderWindow.Height - (234 + 120)));
        }

        private Vector2d PointToTouch(Vector2d pnt)
        {
            return new Vector2d(pnt.X - (renderWindow.Width - 207), pnt.Y - (renderWindow.Height - (234 + 120)));
        }

        private Vector2d TouchToScreen(Vector2d pnt)
        {
            return new Vector2d(pnt.X + (renderWindow.Width - 207), pnt.Y + (renderWindow.Height - (234 + 120)));
        }

        private void renderWindow_MouseEnter(object sender, EventArgs e)
        {
            if (!NoStealFocus)
            {
                var hwndFG = GetForegroundWindow();
                if ((currentTab != null && currentTab.Handle == hwndFG) || (contextPanel != null && contextPanel.Handle == hwndFG) || (ObjectProperties.Props != null && ObjectProperties.Props.Handle == hwndFG))
                {
                    renderWindow.Focus();
                }
            }
        }

        private void renderWindow_MouseHover(object sender, EventArgs e)
        {

        }

        private void renderWindow_MouseLeave(object sender, EventArgs e)
        {
        }

        PointF moveVector;
        bool mouseMoved;
        DateTime lastMouseMove = DateTime.Now;
        float ZoomVector;
        float OrbitVector;
        private void renderWindow_MouseMove(object sender, MouseEventArgs e)
        {



            if (contrastMode)
            {
                contrast = (1 - (e.Y / (float)ClientSize.Height));
                brightness = e.X / (float)ClientSize.Width;
                return;
            }




            if (activeTouch != TouchControls.None)
            {
                if (activeTouch == TouchControls.TrackBall)
                {
                    moveVector = new PointF(touchTrackBallCenter.X - (e.X + Properties.Settings.Default.ScreenHitTestOffsetX), touchTrackBallCenter.Y - (e.Y + Properties.Settings.Default.ScreenHitTestOffsetY));
                }

                if (activeTouch == TouchControls.PanTrack)
                {
                    var panTrack = TouchToScreen(panTracker);


                    var mv = new Vector2d(panTrack.X - (e.X + Properties.Settings.Default.ScreenHitTestOffsetX), panTrack.Y - (e.Y + Properties.Settings.Default.ScreenHitTestOffsetY));

                    if (mv.Length > 50)
                    {
                        mv.Normalize();
                        mv.Scale(50);
                    }

                    moveVector = new PointF((float)mv.X, (float)mv.Y);


                }

                if (activeTouch == TouchControls.ZoomTrack)
                {
                    var zoomTrack = TouchToScreen(zoomTracker);
                    var zoomDrag = zoomTrack.X - (e.X + Properties.Settings.Default.ScreenHitTestOffsetX);
                    if (Math.Abs(zoomDrag) > 54)
                    {
                        ZoomVector = 54 * Math.Sign(zoomDrag);
                    }
                    else
                    {
                        ZoomVector = (float)zoomDrag;
                    }
                }

                if (activeTouch == TouchControls.OrbitTrack)
                {
                    var orbitTrack = TouchToScreen(orbitTracker);
                    var orbitDrag = orbitTrack.X - (e.X + Properties.Settings.Default.ScreenHitTestOffsetX);
                    if (Math.Abs(orbitDrag) > 70)
                    {
                        OrbitVector = 70 * Math.Sign(orbitDrag);
                    }
                    else
                    {
                        OrbitVector = (float)orbitDrag;
                    }
                }

                return;
            }


            if (uiController != null)
            {
                if (uiController.MouseMove(sender, new MouseEventArgs(e.Button, e.Clicks, e.X + Properties.Settings.Default.ScreenHitTestOffsetX, e.Y + Properties.Settings.Default.ScreenHitTestOffsetY, e.Delta)))
                {
                    return;
                }
            }

            moved = true;


            if (lastMousePosition == e.Location)
            {
                return;
            }
            mouseMoved = true;
            lastMouseMove = DateTime.Now;

            if (!CursorVisible && !ProjectorServer)
            {
                Cursor.Show();
                CursorVisible = true;
            }

            if (measuringDrag)
            {
                measureEnd = GetCoordinatesForScreenPoint(e.X, e.Y);

                if (measureLines == null)
                {

                    measureLines = new SimpleLineList11();
                    measureLines.DepthBuffered = false;

                }
                measureLines.Clear();
                measureLines.AddLine(Coordinates.RADecTo3d(measureStart.RA + 12, measureStart.Dec, 1), Coordinates.RADecTo3d(measureEnd.RA + 12, measureEnd.Dec, 1));
                var angularSperation = CAAAngularSeparation.Separation(measureStart.RA, measureStart.Dec, measureEnd.RA, measureEnd.Dec);



                var pl = new TourPlace(Language.GetLocalizedText(977, "Seperation: ") + Coordinates.FormatDMS(angularSperation), measureEnd.Dec, measureEnd.RA, Classification.Star, Constellations.Containment.FindConstellationForPoint(measureEnd.RA, measureEnd.Dec), ImageSetType.Sky, -1);
                SetLabelText(pl, true);

            }
            else if (Space && Settings.Active.GalacticMode)
            {
                if (dragging)
                {
                    Tracking = false;

                    MoveView(-(e.X - mouseDownX), (e.Y - mouseDownY), true);
                    if (!Properties.Settings.Default.SmoothPan)
                    {
                        az = targetAz;
                        alt = targetAlt;
                        var gPoint = Coordinates.GalactictoJ2000(az, alt);
                        TargetLat = ViewLat = gPoint[1];
                        TargetLong = ViewLong = RAtoViewLng(gPoint[0] / 15);
                        NotifyMoveComplete();
                    }
                    mouseDownX = e.X;
                    mouseDownY = e.Y;
                }
                else if (spinning || angle)
                {

                    CameraRotateTarget = (CameraRotateTarget + (((double)(e.X - mouseDownX)) / 1000 * Math.PI));

                    CameraAngleTarget = (CameraAngleTarget + (((double)(e.Y - mouseDownY)) / 1000 * Math.PI));

                    if (CameraAngleTarget < TiltMin)
                    {
                        CameraAngleTarget = TiltMin;
                    }

                    if (CameraAngleTarget > 0)
                    {
                        CameraAngleTarget = 0;
                    }

                    if (!Properties.Settings.Default.SmoothPan)
                    {
                        CameraRotate = CameraRotateTarget;
                        CameraAngle = CameraAngleTarget;
                    }

                    mouseDownX = e.X;
                    mouseDownY = e.Y;
                }
                else
                {
                    mouseMoved = true;
                    lastMouseMove = DateTime.Now;
                }
            }
            else if (Space && Settings.Active.LocalHorizonMode)
            {
                if (dragging)
                {
                    if (!SolarSystemMode)
                    {
                        Tracking = false;
                    }

                    MoveView(-(e.X - mouseDownX), (e.Y - mouseDownY), true);
                    if (!Properties.Settings.Default.SmoothPan)
                    {
                        az = targetAz;
                        alt = targetAlt;
                        var currentRaDec = Coordinates.HorizonToEquitorial(Coordinates.FromLatLng(alt, az), SpaceTimeController.Location, SpaceTimeController.Now);

                        TargetLat = ViewLat = currentRaDec.Dec;
                        TargetLong = ViewLong = RAtoViewLng(currentRaDec.RA);
                        NotifyMoveComplete();
                    }
                    mouseDownX = e.X;
                    mouseDownY = e.Y;
                }
                else
                {
                    mouseMoved = true;
                    lastMouseMove = DateTime.Now;
                }
            }
            else
            {
                if (dragging)
                {
                    if (!SolarSystemMode)
                    {
                        Tracking = false;
                    }

                    MoveView(-(e.X - mouseDownX), (e.Y - mouseDownY), true);
                    if (!Properties.Settings.Default.SmoothPan)
                    {
                        ViewLat = TargetLat;
                        ViewLong = TargetLong;
                        NotifyMoveComplete();
                    }
                    mouseDownX = e.X;
                    mouseDownY = e.Y;
                }
                else if (spinning || angle)
                {

                    CameraRotateTarget = (CameraRotateTarget + (((double)(e.X - mouseDownX)) / 1000 * Math.PI));

                    CameraAngleTarget = (CameraAngleTarget + (((double)(e.Y - mouseDownY)) / 1000 * Math.PI));

                    if (CameraAngleTarget < TiltMin)
                    {
                        CameraAngleTarget = TiltMin;
                    }

                    if (CameraAngleTarget > 0)
                    {
                        CameraAngleTarget = 0;
                    }

                    if (!Properties.Settings.Default.SmoothPan)
                    {
                        CameraRotate = CameraRotateTarget;
                        CameraAngle = CameraAngleTarget;
                    }

                    mouseDownX = e.X;
                    mouseDownY = e.Y;
                }
                else
                {
                    mouseMoved = true;
                    lastMouseMove = DateTime.Now;

                }
            }

            lastMousePosition = e.Location;
        }

        private void renderWindow_MouseUp(object sender, MouseEventArgs e)
        {
            if (contrastMode)
            {
                contrastMode = false;
                return;
            }


            if (activeTouch != TouchControls.None)
            {
                if (activeTouch == TouchControls.Finder)
                {
                    if (kioskControl)
                    {
                        if (ObjectProperties.Active)
                        {
                            ObjectProperties.HideProperties();
                        }
                        else
                        {
                            ShowFinder();
                        }
                    }
                    else
                    {
                        Friction = !Friction;
                    }
                }
                if (activeTouch == TouchControls.Home)
                {
                    if (kioskControl)
                    {
                        if (TouchKiosk)
                        {
                            Properties.Settings.Default.SolarSystemScale = 1;
                            FadeInImageSet(GetDefaultImageset(ImageSetType.SolarSystem, BandPass.Visible));
                            var camParams = new CameraParameters(45, 0, 360, 0, 0, 100);
                            GotoTarget(camParams, false, true);
                        }
                        else
                        {
                            var camParams = new CameraParameters(0, 0, 360, 0, 0, 100);
                            GotoTarget(camParams, false, true);
                        }
                    }
                    else
                    {
                        TouchAllStop();
                    }
                }

                activeTouch = TouchControls.None;

                return;
            }


            if (uiController != null)
            {
                if (uiController.MouseUp(sender, new MouseEventArgs(e.Button, e.Clicks, e.X + Properties.Settings.Default.ScreenHitTestOffsetX, e.Y + Properties.Settings.Default.ScreenHitTestOffsetY, e.Delta)))
                {
                    return;
                }
            }

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle)
            {
                dragging = false;
                spinning = false;
                measuringDrag = false;
                measuring = false;
                angle = false;
                if (!moved && ShowKmlMarkers && Space)
                {

                    var cursor = renderWindow.PointToClient(Cursor.Position);

                    var result = GetCoordinatesForScreenPoint(cursor.X, cursor.Y);

                    if (CurrentImageSet.DataSetType == ImageSetType.Sky)
                    {
                        if (!ProjectorServer)
                        {
                            if (constellationCheck != null)
                            {
                                if (ShowKmlMarkers && KmlMarkers != null)
                                {
                                    KmlMarkers.ItemClick(Coordinates.RADecTo3dDouble(result, 1.0).Vector311, (float)(ZoomFactor / 900.0));
                                }
                            }
                        }
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if ((CurrentImageSet.DataSetType == ImageSetType.Sky || CurrentImageSet.DataSetType == ImageSetType.SolarSystem /*|| CurrentImageSet.DataSetType == ImageSetType.Planet || CurrentImageSet.DataSetType == ImageSetType.Earth*/ ) && !TourPlayer.Playing)
                {


                    if (figureEditor != null)
                    {
                        // TODO fix this for earth, plantes, panoramas
                        var result = GetCoordinatesForScreenPoint(e.X, e.Y);
                        var constellation = constellationCheck.FindConstellationForPoint(result.RA, result.Dec);
                        contextPanel.Constellation = Constellations.FullName(constellation);
                        var closetPlace = ContextSearch.FindClosestMatch(constellation, result.RA, result.Dec, MainWindow.DegreesPerPixel * 80);
                        if (closetPlace == null)
                        {
                            closetPlace = new TourPlace(Language.GetLocalizedText(90, "No Object"), result.Dec, result.RA, Classification.Unidentified, constellation, ImageSetType.Sky, -1);
                        }
                        figureEditor.AddFigurePoint(closetPlace);
                    }
                    else
                    {
                        var pntShow = new Point(e.X, e.Y);

                        if (SolarSystemMode)
                        {
                            ObjectProperties.ShowAt(renderWindow.PointToScreen(pntShow));

                        }
                        else
                        {
                            ShowPropertiesForPoint(pntShow);
                        }
                    }
                }
            }
        }

        private void TouchAllStop()
        {
            moveVector = new PointF();
            touchPoints[(int)TouchControls.PanTrack] = panTracker;
            touchPoints[(int)TouchControls.OrbitTrack] = orbitTracker;
            touchPoints[(int)TouchControls.ZoomTrack] = zoomTracker;
            ZoomVector = 0;
            OrbitVector = 0;
        }

        private void renderWindow_Resize(object sender, EventArgs e)
        {
        }

        private void renderWindow_Paint(object sender, PaintEventArgs e)
        {
            if (RenderContext11 != null && RenderContext11.Device != null && ReadyToRender && !pause && !SpaceTimeController.FrameDumping)
            {
                Render();
            }
            else
            {
                e.Graphics.Clear(Color.Black);
            }
        }

        bool playingSlideShow;
        private void playCollectionAsSlideShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (playingSlideShow)
            {
                StopSlideShow();
            }
            else
            {
                PlayCollection();
            }
        }

        private void extractThumb(Folder folder)
        {
            foreach (var child in folder.Children)
            {
                if (child is Folder)
                {
                    var ims = child as Folder;
                    var imt = child as IThumbnail;
                    if (ims != null && imt != null)
                    {
                        if (!string.IsNullOrEmpty(ims.Thumbnail) && ims.Thumbnail.Contains("wwt.nasa"))
                        {
                            var filename = ims.Thumbnail.Substring(ims.Thumbnail.LastIndexOf("/") + 1);
                            if (!File.Exists("c:\\marsthumbs\\" + filename))
                            {
                                var bmp = imt.ThumbNail;
                                bmp.Save("c:\\marsthumbs\\" + filename);
                                bmp.Dispose();
                            }
                        }
                    }


                    extractThumb(child as Folder);
                }
                if (child is IImageSet)
                {
                    var ims = child as IImageSet;
                    var imt = child as IThumbnail;
                    if (ims != null && imt != null)
                    {
                        if (ims.ThumbnailUrl.Contains("wwt.nasa"))
                        {
                            var filename = ims.ThumbnailUrl.Substring(ims.ThumbnailUrl.LastIndexOf("/") + 1);
                            if (!File.Exists("c:\\marsthumbs\\" + filename))
                            {
                                var bmp = imt.ThumbNail;
                                bmp.Save("c:\\marsthumbs\\" + filename);
                                bmp.Dispose();
                            }
                        }
                    }
                }
                if (child is IPlace)
                {
                    var ims = child as IPlace;
                    var imt = child as IThumbnail;
                    if (ims != null && imt != null)
                    {
                        if (!string.IsNullOrEmpty(ims.Thumbnail) && ims.Thumbnail.Contains("wwt.nasa"))
                        {
                            var filename = ims.Thumbnail.Substring(ims.Thumbnail.LastIndexOf("/") + 1);
                            var bmp = imt.ThumbNail;
                            bmp.Save("c:\\marsthumbs\\" + filename);
                            bmp.Dispose();
                        }
                    }
                }
            }
        }

        private void StopSlideShow()
        {
            playingSlideShow = false;
            SlideAdvanceTimer.Enabled = false;
        }

        private void PlayCollection()
        {
            playingSlideShow = true;
            SlideAdvanceTimer.Enabled = true;
            SlideAdvanceTimer.Interval = 500;
            if (!currentTab.AdvanceSlide(true))
            {
                StopSlideShow();
            }
        }

        private void SlideAdvanceTimer_Tick(object sender, EventArgs e)
        {
            if (playingSlideShow)
            {
                if (SlideAdvanceTimer.Interval == 500)
                {
                    if (mover == null)
                    {
                        SlideAdvanceTimer.Interval = 10000;
                    }
                }
                else
                {
                    if (!currentTab.AdvanceSlide(false))
                    {
                        StopSlideShow();
                        if (Properties.Settings.Default.AutoRepeatTour)
                        {
                            PlayCollection();
                        }
                    }
                    else
                    {
                        SlideAdvanceTimer.Interval = 500;
                    }
                }
            }
            else
            {
                SlideAdvanceTimer.Enabled = false;
            }
        }

        void TourPlayer_TourEnded(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.AutoRepeatTourAll && (tourEdit != null && !tourEdit.Tour.EditMode))
            {
                toursTab.PlayNext();
            }
        }

        private void oneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoRepeatTour = true;
            Properties.Settings.Default.AutoRepeatTourAll = false;

        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoRepeatTour = false;
            Properties.Settings.Default.AutoRepeatTourAll = true;

        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoRepeatTour = false;
            Properties.Settings.Default.AutoRepeatTourAll = false;
        }

        private void toursMenu_Opening(object sender, CancelEventArgs e)
        {
            if (ModifierKeys == (Keys.Shift | Keys.Control))
            {
                Properties.Settings.Default.AdvancedCommunities = true;
            }

            if (menuTabs.CurrentTour != null)
            {
                if (ModifierKeys == (Keys.Shift | Keys.Control))
                {
                    publishTourMenuItem.Visible = menuTabs.CurrentTour.EditMode;
                    Properties.Settings.Default.AdvancedCommunities = true;
                }
                else
                {
                    publishTourMenuItem.Visible = false;
                }
                editTourToolStripMenuItem.Visible = !menuTabs.CurrentTour.EditMode;

                publishTourToCommunityToolStripMenuItem.Enabled = IsLoggedIn;

                renderToVideoToolStripMenuItem.Enabled = true;
                showOverlayListToolStripMenuItem.Enabled = true;
                showSlideNumbersToolStripMenuItem.Visible = true;
                showSlideNumbersToolStripMenuItem.Checked = Properties.Settings.Default.ShowSlideNumbers;
                showKeyframerToolStripMenuItem.Visible = menuTabs.CurrentTour.EditMode;
                showKeyframerToolStripMenuItem.Checked = TimeLine.AreOpenTimelines;
            }
            else
            {
                publishTourToCommunityToolStripMenuItem.Enabled = false;
                renderToVideoToolStripMenuItem.Enabled = false;
                publishTourMenuItem.Visible = false;
                editTourToolStripMenuItem.Visible = false;
                showOverlayListToolStripMenuItem.Enabled = false;
                showSlideNumbersToolStripMenuItem.Visible = false;
                showKeyframerToolStripMenuItem.Visible = false;
            }

            undoToolStripMenuItem.Text = "&" + Language.GetLocalizedText(643, "Undo:") + " " + Undo.PeekActionString();
            undoToolStripMenuItem.Enabled = Undo.PeekAction();

            redoToolStripMenuItem.Text = "&" + Language.GetLocalizedText(644, "Redo:") + " " + Undo.PeekRedoActionString();
            redoToolStripMenuItem.Enabled = Undo.PeekRedoAction();

            saveTourAsToolStripMenuItem.Visible = tourEdit != null;

            oneToolStripMenuItem.Checked = Properties.Settings.Default.AutoRepeatTour;
            allToolStripMenuItem.Checked = Properties.Settings.Default.AutoRepeatTourAll;
            offToolStripMenuItem.Checked = !oneToolStripMenuItem.Checked && !allToolStripMenuItem.Checked;

            sendTourToProjectorServersToolStripMenuItem.Enabled = Settings.MasterController;
            automaticTourSyncWithProjectorServersToolStripMenuItem.Enabled = Settings.MasterController;
            automaticTourSyncWithProjectorServersToolStripMenuItem.Checked = Properties.Settings.Default.AutoSyncTours;
        }

        private void Earth3d_KeyUp(object sender, KeyEventArgs e)
        {
            if (ModifierKeys == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.E:
                        menuTabs.ShowTabMenu(ApplicationMode.Explore);
                        break;
                    case Keys.G:
                        menuTabs.ShowTabMenu(ApplicationMode.Tours);
                        break;
                    case Keys.S:
                        menuTabs.ShowTabMenu(ApplicationMode.Search);
                        break;
                    case Keys.C:
                        menuTabs.ShowTabMenu(ApplicationMode.Community);
                        break;
                    case Keys.T:
                        menuTabs.ShowTabMenu(ApplicationMode.Telescope);
                        break;
                    case Keys.V:
                        menuTabs.ShowTabMenu(ApplicationMode.View);
                        break;
                    case Keys.N:
                        menuTabs.ShowTabMenu(ApplicationMode.Settings);
                        break;
                }
            }
        }

        private void exploreMenu_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    menuTabs.ShowTabMenu(+1);
                    break;

                case Keys.Left:
                    menuTabs.ShowTabMenu(-1);
                    break;
            }
        }

        private void addToCollectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        void newCollectionMenu_Click(object sender, EventArgs e)
        {
            var folder = explorePane.NewCollection();
            if (folder != null)
            {
                folder.AddChildPlace(Place.FromIPlace(contextMenuTargetObject));
            }
        }

        void AddTocollectionMenu_Click(object sender, EventArgs e)
        {
            var folder = (Folder)((ToolStripMenuItem)sender).Tag;
            folder.AddChildPlace(Place.FromIPlace(contextMenuTargetObject));

            explorePane.ReloadFolder();

            var item = (ToolStripMenuItem)sender;
            contextMenu.Close();
        }

        private void addToCollectionsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            addToCollectionsToolStripMenuItem.DropDownItems.Clear();

            var newCollectionMenu = new ToolStripMenuItem(Language.GetLocalizedText(23, "New Collection..."));
            newCollectionMenu.Click += newCollectionMenu_Click;
            addToCollectionsToolStripMenuItem.DropDownItems.Add(newCollectionMenu);

            var menuItem = addToCollectionsToolStripMenuItem;
            addToCollectionsToolStripMenuItem.Tag = explorePane.MyCollections;

            CreatePickFolderMenu(menuItem);
        }

        private void CreatePickFolderMenu(ToolStripMenuItem menuItem)
        {
            var collections = (Folder)menuItem.Tag;

            foreach (var f in collections.Folder1)
            {
                var tempMenu = new ToolStripMenuItem(f.Name);
                tempMenu.Click += AddTocollectionMenu_Click;
                tempMenu.Tag = f;
                menuItem.DropDownItems.Add(tempMenu);
                CreatePickFolderMenu(tempMenu);
            }
        }
        private void removeFromCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (explorePane != null)
            {
                explorePane.RemoveSelected();
            }
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            NoStealFocus = true;

        }

        private void contextMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            NoStealFocus = false;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = new PlaceEditor();
            editor.EditTarget = contextMenuTargetObject;
            if (editor.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void TourEndCheck_Tick(object sender, EventArgs e)
        {
            if (showTourCompleteDialog && !NoShowTourEndPage)
            {
                showTourCompleteDialog = false;
                if (tourEdit != null)
                {
                    var dlgResult = TourPopup.ShowEndTourPopupModal(tourEdit.Tour);
                    if (dlgResult == DialogResult.OK)
                    {
                        tourEdit.PlayNow(true);
                    }
                    else if (dlgResult == DialogResult.Cancel)
                    {
                        CloseTour(false);
                    }
                }
            }
        }

        private void autoSaveTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (explorePane != null)
                {
                    explorePane.AutoSave();
                }
            }
            catch
            {
            }

        }

        private void removeFromImageCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var place = contextMenuTargetObject;
                IImageSet imageSet = null;
                if (place.StudyImageset != null)
                {
                    imageSet = place.StudyImageset;
                }
                if (place.BackgroundImageSet != null)
                {
                    imageSet = place.BackgroundImageSet;
                }

                long totalBytes = 0;
                long demBytes = 0;
                var path = Properties.Settings.Default.CahceDirectory + @"Imagery\" + imageSet.ImageSetID;

                var demPath = Properties.Settings.Default.CahceDirectory + @"dem\" + Math.Abs(imageSet.DemUrl != null ? imageSet.DemUrl.GetHashCode32() : 0);

                Cursor.Current = Cursors.WaitCursor;

                if (!Directory.Exists(path) && !Directory.Exists(demPath))
                {
                    UiTools.ShowMessageBox(Language.GetLocalizedText(127, "There is no image data in the disk cache to purge."));
                }

                ClearCache.PurgeDirecotryNoProgress(path, ref totalBytes);


                if (Directory.Exists(demPath))
                {
                    ClearCache.PurgeDirecotryNoProgress(demPath, ref demBytes);
                }

                Cursor.Current = Cursors.Default;
                UiTools.ShowMessageBox(String.Format(Language.GetLocalizedText(128, "The imageset was removed from cache and {0}MB was freed"), ((totalBytes + demBytes) / 1024.0 / 1024.0).ToString("f")));

            }
            catch
            {


            }

        }

        private void selectLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new LanguageSelect();

            dialog.Language = Language.CurrentLanguage;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.Language.Url != Language.CurrentLanguage.Url)
                {
                    if (dialog.Language.Code == "ZZZZ")
                    {
                        var openFile = new OpenFileDialog();
                        openFile.Filter = "WWT Language File|*.tdf";
                        if (openFile.ShowDialog() == DialogResult.OK)
                        {
                            var filename = openFile.FileName;

                            try
                            {
                                var uri = new Uri(filename);
                                if (uri.IsFile)
                                {
                                    filename = uri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped);

                                }
                                dialog.Language.Url = "file://" + filename;
                            }
                            catch
                            {
                                //todo localization
                                UiTools.ShowMessageBox("Could Not Open Language Table File. Please ensure it is valid");
                            }
                        }
                    }
                    var useProxy = false;

                    if (!string.IsNullOrEmpty(dialog.Language.Proxy) && dialog.Language.Proxy != Properties.Settings.Default.SharedCacheServer)
                    {
                        var result1 = UiTools.ShowMessageBox(Language.GetLocalizedText(691, "This Language Pack has an optional regional data cache that will allow improved performance when in that region. Do you wish to use it?"), Language.GetLocalizedText(692, "Use Regional Data Cache"), MessageBoxButtons.YesNo);
                        if (result1 == DialogResult.Yes)
                        {
                            useProxy = true;
                        }
                    }


                    //todo Localize Text
                    var result = UiTools.ShowMessageBox(Language.GetLocalizedText(693, "WorldWide Telescope must restart to load a new language file. Do you want to restart now?"), Language.GetLocalizedText(694, "Restart Now?"), MessageBoxButtons.YesNoCancel);
                    switch (result)
                    {
                        case DialogResult.Cancel:
                            return;
                        case DialogResult.No:
                            Properties.Settings.Default.ImageSetUrl = dialog.Language.ImageSetsUrl;
                            Properties.Settings.Default.ExploreRootUrl = dialog.Language.RootUrl;
                            Properties.Settings.Default.LanguageCode = dialog.Language.Code;
                            Properties.Settings.Default.LanguageName = dialog.Language.Name;
                            Properties.Settings.Default.LanguageUrl = dialog.Language.Url;
                            if (useProxy)
                            {
                                Properties.Settings.Default.SharedCacheServer = dialog.Language.Proxy;
                            }
                            break;

                        case DialogResult.Yes:
                            Properties.Settings.Default.ImageSetUrl = dialog.Language.ImageSetsUrl;
                            Properties.Settings.Default.ExploreRootUrl = dialog.Language.RootUrl;
                            Properties.Settings.Default.LanguageCode = dialog.Language.Code;
                            Properties.Settings.Default.LanguageName = dialog.Language.Name;
                            Properties.Settings.Default.LanguageUrl = dialog.Language.Url;
                            LanguageReboot = true;
                            if (useProxy)
                            {
                                Properties.Settings.Default.SharedCacheServer = dialog.Language.Proxy;
                            }
                            Close();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        static bool LanguageReboot;
        static bool CloseNow;
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tourEdit != null)
            {
                tourEdit.UndoStep();
            }

        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tourEdit != null)
            {
                tourEdit.RedoStep();
            }
        }

        private void saveTourAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tourEdit != null)
            {
                tourEdit.Save(true);
            }
        }

        private void setAsForegroundImageryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var place = contextMenuTargetObject;
                IImageSet imageSet = null;
                if (place.StudyImageset != null)
                {
                    imageSet = place.StudyImageset;
                }
                if (place.BackgroundImageSet != null)
                {
                    imageSet = place.BackgroundImageSet;
                }

                MainWindow.SetStudyImageset(imageSet, null);
            }
            catch
            {


            }
        }

        private void setAsBackgroundImageryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var place = contextMenuTargetObject;
                IImageSet imageSet = null;
                if (place.StudyImageset != null)
                {
                    imageSet = place.StudyImageset;
                }
                if (place.BackgroundImageSet != null)
                {
                    imageSet = place.BackgroundImageSet;
                }

                if (imageSet != null)
                {
                    CurrentImageSet = imageSet;
                }
            }
            catch
            {


            }
        }
        public List<IImageSet> ImageStackList = new List<IImageSet>();
        private void addToImageStackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var place = contextMenuTargetObject;
                AddPlaceToStack(place, true);
                ShowImageStack();
            }
            catch
            {


            }
        }

        public void AddPlaceToStack(IPlace place, bool refresh)
        {
            IImageSet imageSet = null;
            if (place.StudyImageset != null)
            {
                imageSet = place.StudyImageset;
            }
            if (place.BackgroundImageSet != null)
            {
                imageSet = place.BackgroundImageSet;
            }

            if (imageSet != null)
            {
                ImageStackList.Add(ImageSet.FromIImage(imageSet));
                if (refresh)
                {
                    stack.UpdateList();
                }
            }
        }

        public void RemovePlaceFromStack(IPlace place, bool refresh)
        {
            if (place == null)
            {
                return;
            }

            IImageSet imageSet = null;
            if (place.StudyImageset != null)
            {
                imageSet = place.StudyImageset;
            }
            if (place.BackgroundImageSet != null)
            {
                imageSet = place.BackgroundImageSet;
            }

            if (imageSet != null)
            {
                IImageSet itemToRemove = null;
                foreach (var set in ImageStackList)
                {
                    if (set.GetHash() == imageSet.GetHash())
                    {
                        itemToRemove = set;
                    }
                }
                ImageStackList.Remove(itemToRemove);
                if (refresh)
                {
                    stack.UpdateList();
                }
            }
        }
        private void toggleFullScreenModeF11ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFullScreen(!fullScreen);
        }

        private void virtualObservatorySearchesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void NEDSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var url = String.Format("http://nedwww.ipac.caltech.edu/cgi-bin/nph-objsearch?search_type=Near+Position+Search&of=xml_main&RA={0}&DEC={1}&SR={2}", (contextMenuTargetObject.RA * 15), contextMenuTargetObject.Dec, (FovAngle) < (1.0 / 60.0) ? (FovAngle).ToString() : (1.0 / 60.0).ToString());

            RunVoSearch(url, null);
 
        }

        public void RunVoSearch(string url, string ID)
        {
            var filename = Path.GetTempFileName();
            Cursor.Current = Cursors.WaitCursor;

            if (!FileDownload.DownloadFile(url, filename, true))
            {
                return;
            }

            try
            {
                var voTable = new VoTable(filename);
                voTable.SampId = ID;
                if (!voTable.error)
                {
                    var viewer = new VOTableViewer();
                    var layer = LayerManager.AddVoTableLayer(voTable, "VO Table");

                    if (ID != null)
                    {
                        Samp.sampKnownTableIds.Add(ID, layer);

                    }
                    Samp.sampKnownTableUrls.Add(url, layer);
                    viewer.Layer = layer;
                    viewer.Show();
                    ShowLayersWindows = true;

                }
                else
                {
                    UiTools.ShowMessageBox(voTable.errorText);
                }
            }
            catch
            {
                WebWindow.OpenUrl(url, true);
            }
            Cursor.Current = Cursors.Default;
        }

        private void vOTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "VOTable|*.xml";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var filename = openFile.FileName;
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    var voTable = new VoTable(filename);
                    var layer = LayerManager.AddVoTableLayer(voTable, Path.GetFileName(filename));

                    var viewer = new VOTableViewer();
                    viewer.Layer = layer;
                    viewer.Show();
                    ShowLayersWindows = true;
                }
                catch
                {
                    //todo localization
                    UiTools.ShowMessageBox(Language.GetLocalizedText(695, "Could Not Open VO Table File. Please ensure it is valid"));
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void searchMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void vORegistryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var browser = new VORegistryBrowser();
            if (browser.ShowDialog() == DialogResult.OK)
            {
                RunVoSearch(browser.URL, null);
            }
        }

        private void sDSSSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var url = String.Format("http://casjobs.sdss.org/vo/dr5cone/sdssConeSearch.asmx/ConeSearch?ra={0}&dec={1}&sr={2}", (contextMenuTargetObject.RA * 15), contextMenuTargetObject.Dec, fovAngle);

            RunVoSearch(url, null);
        }

        private void stereoToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            oculusRiftToolStripMenuItem.Checked = StereoMode == StereoModes.OculusRift;
            enabledToolStripMenuItem.Checked = StereoMode == StereoModes.Off;
            anaglyphToolStripMenuItem.Checked = StereoMode == StereoModes.AnaglyphRedCyan;
            anaglyphYellowBlueToolStripMenuItem.Checked = StereoMode == StereoModes.AnaglyphYellowBlue;
            sideBySideProjectionToolStripMenuItem.Checked = StereoMode == StereoModes.SideBySide;
            sideBySideCrossEyedToolStripMenuItem.Checked = StereoMode == StereoModes.CrossEyed;
            alternatingLinesEvenToolStripMenuItem.Checked = StereoMode == StereoModes.InterlineEven;
            alternatingLinesOddToolStripMenuItem.Checked = StereoMode == StereoModes.InterlineOdd;
        }

        private void enabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rift)
            {
                rift = false;
                AttachRenderWindow();
            }
            StereoMode = StereoModes.Off;
            Properties.Settings.Default.ColSettingsVersion++;
        }



        private void anaglyphYellowBlueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rift)
            {
                rift = false;
                AttachRenderWindow();
            }
            StereoMode = StereoModes.AnaglyphYellowBlue;
            Properties.Settings.Default.ColSettingsVersion++;
        }
        private void anaglyphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rift)
            {
                rift = false;
                AttachRenderWindow();
            }
            StereoMode = StereoModes.AnaglyphRedCyan;
            Properties.Settings.Default.ColSettingsVersion++;
        }

        private void sideBySideProjectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rift)
            {
                rift = false;
                AttachRenderWindow();
            }
            StereoMode = StereoModes.SideBySide;
            Properties.Settings.Default.ColSettingsVersion++;
        }

        bool rift;
        bool riftInit;

        [DllImport("riftapi.dll")]
        static extern int ResetRift();
        [DllImport("riftapi.dll")]
        static extern int InitRiftApi();
        [DllImport("riftapi.dll")]
        static extern int GetSensorSample();
        [DllImport("riftapi.dll")]
        static extern float GetHeading();
        [DllImport("riftapi.dll")]
        static extern float GetPitch();
        [DllImport("riftapi.dll")]
        static extern float GetRoll();
        [DllImport("riftapi.dll")]
        static extern int CloseRiftApi();
        [DllImport("riftapi.dll")]
        static extern int GetRiftInfo(ref RiftInfo riftInfo);
        [DllImport("riftapi.dll")]
        static extern IntPtr GetDisplayName();
        RiftInfo riftInfo;
        double riftFov = 1.6446; 
        private void oculusRiftToolStripMenuItem_Click(object sender, EventArgs e)
        {


            if (!riftInit)
            {
                if (InitRiftApi() != 1)
                {

                    return;
                }
                riftInit = true;
            }

            GetRiftInfo(ref riftInfo);
            iod = (4 * ((riftInfo.HScreenSize / 4) - (riftInfo.LensSeparationDistance / 2)) / riftInfo.HScreenSize);
            var aspect = riftInfo.HResolution / (2.0 * riftInfo.VResolution);
            riftFov = aspect * 2 * Math.Atan(riftInfo.VScreenSize / (2 * riftInfo.EyeToScreenDistance));
         

            if (riftInfo.VResolution == 800) //dev Kit
            {
                riftFov = 1.6446;
            }


            var ptr = GetDisplayName();
            var name = Marshal.PtrToStringAnsi(ptr);
            FreeFloatRenderWindow(name);

           

            rift = true;
            StereoMode = StereoModes.OculusRift;

            Properties.Settings.Default.ColSettingsVersion++;
        }

        private void sideBySideCrossEyedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rift)
            {
                rift = false;
                AttachRenderWindow();
            }

            StereoMode = StereoModes.CrossEyed;
            Properties.Settings.Default.ColSettingsVersion++;
        }


        private void alternatingLinesOddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rift)
            {
                rift = false;
                AttachRenderWindow();
            }
            StereoMode = StereoModes.InterlineOdd;
            Properties.Settings.Default.ColSettingsVersion++;
        }

        private void alternatingLinesEvenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rift)
            {
                rift = false;
                AttachRenderWindow();
            }
            StereoMode = StereoModes.InterlineEven;
            Properties.Settings.Default.ColSettingsVersion++;
        }

        private void fullDomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DomeView = !Properties.Settings.Default.DomeView;
            Settings.DomeView = false;

            if (Properties.Settings.Default.DomeView)
            {
                NetControl.Start();
            }
        }



        private void lookUpOnNEDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (contextMenuTargetObject.Classification == Classification.Unidentified)
            {
                var url = "http://nedwww.ipac.caltech.edu/cgi-bin/nph-objsearch?in_csys=Equatorial&in_equinox=J2000.0&lon={0}d&lat={1}d&radius={2}&hconst=73&omegam=0.27&omegav=0.73&corr_z=1&search_type=Near+Position+Search&z_constraint=Unconstrained&z_value1=&z_value2=&z_unit=z&ot_include=ANY&nmp_op=ANY&out_csys=Equatorial&out_equinox=J2000.0&obj_sort=Distance+to+search+center&of=pre_text&zv_breaker=30000.0&list_limit=5&img_stamp=YES";
                WebWindow.OpenUrl(String.Format(url, contextMenuTargetObject.RA, contextMenuTargetObject.Dec,
                    (FovAngle * 60) < 1 ? (FovAngle * 60).ToString() : "1.0"), false);
            }
            else
            {
                var url = "http://nedwww.ipac.caltech.edu/cgi-bin/nph-imgdata?objname={0}";
                WebWindow.OpenUrl(String.Format(url, contextMenuTargetObject.Name), false);
            }

        }

        private void domeSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dome = new DomeSetup();
            dome.Show();
        }
        bool menu;
        bool showNextObject;
        bool contact;
        bool kinectHeard;
        internal void MoveAndZoomRate(double leftRight, double upDown, double zoom, string mode)
        {
            kinectHeard = true;
            if (mode == "Menu")
            {
                menu = true;
                kinectPickValue = 0;
            }



            if (!menu)
            {
                MoveView(leftRight, upDown, false);
                NetZoomRate = zoom;
            }
            else
            {
                contact = mode == "Contact";
                if (contact)
                {
                    kinectListOffsetTarget -= leftRight;
                    kinectPickValue += upDown;
                    if (kinectPickValue > 600)
                    {
                        showNextObject = true;
                    }
                    else if (kinectPickValue < -600)
                    {
                        menu = false;
                    }
                    else
                    {
                        kinectPickValue *= .95;
                    }
                }
            }
        }

        private void listenUpBoysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ListenMode = !Properties.Settings.Default.ListenMode;
            listenUpBoysToolStripMenuItem.Checked = Properties.Settings.Default.ListenMode;
        }

        internal void SetBackgroundByName(string name)
        {
            var target = MainWindow.ExplorerRoot;


            foreach (var set in ImageSets)
            {
                if (set.Name.ToLower().Contains(name.ToLower()))
                {
                    CurrentImageSet = set;
                    return;
                }
            }


            foreach (var o in target.Children)
            {
                if (o is Folder)
                {
                    if (SetBackgroundByName(o as Folder, name))
                    {
                        return;
                    }
                }
            }

            target = explorePane.MyCollections;

            foreach (var o in target.Children)
            {
                if (o is Folder)
                {
                    if (SetBackgroundByName(o as Folder, name))
                    {
                        return;
                    }
                }
            }
        }

        private bool SetBackgroundByName(Folder folder, string name)
        {
            foreach (var o in folder.Children)
            {
                if (o is IPlace)
                {

                    var place = (IPlace)o;

                    if (place.Name.ToLower().Contains(name.ToLower()))
                    {
                        if (place.BackgroundImageSet != null && place.BackgroundImageSet.DataSetType != MainWindow.CurrentImageSet.DataSetType)
                        {
                            continue;
                        }
                        if (place.Classification == Classification.SolarSystem && MainWindow.CurrentImageSet.DataSetType == ImageSetType.SolarSystem)
                        {
                            CurrentImageSet = new ImageSetHelper(ImageSetType.SolarSystem, BandPass.Visible);
                        }
                        IImageSet imageSet = null;
                        if (place.StudyImageset != null)
                        {
                            imageSet = place.StudyImageset;
                        }
                        if (place.BackgroundImageSet != null)
                        {
                            imageSet = place.BackgroundImageSet;
                        }

                        if (imageSet != null)
                        {
                            CurrentImageSet = imageSet;
                        }
                        GotoTarget(place, false, false, true);
                        return true;
                    }
                }
            }
            return false;
        }

        private void broadcastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var place = contextMenuTargetObject;

                if (place.Tag is FitsImage)
                {
                    var image = (FitsImage)place.Tag;
                    var path = new Uri(image.Filename);

                    sampConnection.LoadImageFits(path.ToString(), image.Filename, place.Name);
                }
            }
            catch
            {


            }
        }

        private void imageStackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageStackVisible = !imageStackVisible;
            ShowImageStack();
        }

        private void earthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartUpLookAt = (int)ImageSetType.Earth;
        }

        private void planetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartUpLookAt = (int)ImageSetType.Planet;
        }

        private void skyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartUpLookAt = (int)ImageSetType.Sky;
        }

        private void panoramaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartUpLookAt = (int)ImageSetType.Panorama;
        }



        private void solarSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartUpLookAt = (int)ImageSetType.SolarSystem;
        }

        private void lastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartUpLookAt = 5;
        }
        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartUpLookAt = 6;
        }

        private void startupToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            earthToolStripMenuItem.Checked = Properties.Settings.Default.StartUpLookAt == (int)ImageSetType.Earth;
            planetToolStripMenuItem.Checked = Properties.Settings.Default.StartUpLookAt == (int)ImageSetType.Planet;
            skyToolStripMenuItem.Checked = Properties.Settings.Default.StartUpLookAt == (int)ImageSetType.Sky;
            panoramaToolStripMenuItem.Checked = Properties.Settings.Default.StartUpLookAt == (int)ImageSetType.Panorama;
            solarSystemToolStripMenuItem.Checked = Properties.Settings.Default.StartUpLookAt == (int)ImageSetType.SolarSystem;
            lastToolStripMenuItem.Checked = Properties.Settings.Default.StartUpLookAt == 5;
            randomToolStripMenuItem.Checked = Properties.Settings.Default.StartUpLookAt == 6;
        }

        private void musicAndOtherTourResourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebWindow.OpenUrl("http://www.worldwidetelescope.org/Download/TourAssets", true);

        }

        private void lookUpOnSDSSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var url = "http://cas.sdss.org/dr7/en/tools/quicklook/quickobj.asp?id={0}";
            url = String.Format(url, contextMenuTargetObject.Name.Replace("SDSS ", ""));

            if (contextMenuTargetObject.Name == "No Object")
            {
                url = String.Format("http://cas.sdss.org/dr7/en/tools/quicklook/quickobj.asp?ra={0}&dec={1}", contextMenuTargetObject.RA * 15, contextMenuTargetObject.Dec);
            }

            WebWindow.OpenUrl(url, false);

        }

        private void shapeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadLayerFile(false);
            ShowLayersWindows = true;
        }

        public static void LoadLayerFile(bool referenceFrameRightClick)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "All Data Files|*.wwtl;*.txt;*.csv;*.tdf;*.3ds;*.obj;*.shp;*.png;*.jpg|WorldWide Telescope Layer File(*.wwtl)|*.wwtl|Data Table(*.txt;*.csv;*.tdf)|*.txt;*.csv;*.tdf|ESRI Shape File(*.shp)|*.shp|3d Model(*.3ds;*.obj)|*.3ds;*.obj|Image Overlays (*.png;*.jpg)|*.png;*.jpg";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var filename = openFile.FileName;

                try
                {
                    LayerManager.LoadLayer(filename, LayerManager.CurrentMap, true, referenceFrameRightClick);
                }
                catch
                {
                    MessageBox.Show(Language.GetLocalizedText(109, "This file does not seem to be a valid collection"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"));
                }
            }
        }


        public static void LoadODATAFeed()
        {


        }

        private void Earth3d_Shown(object sender, EventArgs e)
        {
            if (config.MultiChannelDome1 | ProjectorServer | config.MultiProjector | NoUi)
            {
                TopMost = true;
                Refresh();
                TopMost = false;
            }
        }

        private void showLayerManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLayersWindows = !ShowLayersWindows;
        }

        private void uploadTourToCommunityToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void associateLiveIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowsLiveSignIn();
        }

        public async Task WindowsLiveSignIn()
        {
 
            await SignIn();


            menuTabs.Refresh();
            if (communitiesPane != null)
            {
                communitiesPane.ReloadFolder();
            }
        }

        private OAuthTicket Connection { get; set; }

        private async Task SignIn()
        {
            Connection = await OAuthAuthenticator.SignInToMicrosoftAccount(this);
            if (null != Connection)
            {
                var wc = new WebClient();
                var profile = "";

                profile = wc.DownloadString("https://apis.live.net/v5.0//me?access_token=" + Connection.AccessToken);
                var user = JsonConvert.DeserializeObject<UserObject>(profile);
  
                //ready
             
                Properties.Settings.Default.LiveIdWWTId = user.Id;
                Properties.Settings.Default.LiveIdUser = user.Name;
                Properties.Settings.Default.LiveIdToken = Connection.AccessToken;

            }

        }

        public void WindowsLiveSignOut()
        {
            Properties.Settings.Default.LiveIdToken = null;
            Properties.Settings.Default.RefreshToken = null;
            menuTabs.Refresh();
            if (communitiesPane != null)
            {
                communitiesPane.ReloadFolder();
            }
        }


        private void OpenTourProperties()
        {
            if (tourEdit == null || tourEdit.Tour == null)
            {
                MessageBox.Show(Language.GetLocalizedText(-1, "There was a problem loading the tour..."));
            }
            else
            {
                if (string.IsNullOrEmpty(tourEdit.Tour.Author) ||
                    string.IsNullOrEmpty(tourEdit.Tour.AuthorEmail) ||
                    string.IsNullOrEmpty(tourEdit.Tour.Description) ||
                    tourEdit.Tour.AuthorImage == null)
                {
                    var tourProps = new TourProperties();
                    tourProps.EditTour = tourEdit.Tour;
                    tourProps.Strict = tourProps.highlightNeeded = tourProps.authorImageNeeded = true;
                    tourProps.highlightReqFields();
                    if (tourProps.ShowDialog() == DialogResult.OK)
                    {
                        MainWindow.Refresh();
                    }
                }
                else
                {
                    if (tourEdit.Tour.TourStops.Count == 0)
                    {
                        MessageBox.Show(Language.GetLocalizedText(-1, "Tour must have stops..."));
                    }
                }
            }
        }

        private Tour getTourFromCurrentTour(string filename)
        {
            if (tourEdit == null || tourEdit.Tour == null)
            {
                return null;
            }
            var editTour = tourEdit.Tour;
            if (string.IsNullOrEmpty(editTour.Author) || string.IsNullOrEmpty(editTour.AuthorEmail) ||
                string.IsNullOrEmpty(editTour.Description) || editTour.AuthorImage == null ||
                editTour.TourStops.Count == 0)
            {
                return null;
            }

            editTour.SaveToFile(filename);
            return new Tour()
            {
                // AttributesAndCredits = editTour.AttributesAndCredits,
                Author = editTour.Author,
                AuthorContactText = editTour.AuthorContactText,
                AuthorEmail = editTour.AuthorEmail,
                // AuthorEmailOther = editTour.AuthorEmailOther,
                AuthorImage = editTour.AuthorImage,
                AuthorImageUrl = "", // editTour.AuthorThumbnailFilename,
                AuthorUrl = editTour.AuthorUrl,
                AuthorURL = editTour.AuthorUrl,
                // AverageRating = editTour.AverageRating,
                // AverageUserRating = editTour.AverageUserRating,
                // Bounds = editTour.Bounds,
                // Children = editTour.Children,
                // Classification = editTour.Classification,
                Description = editTour.Description,
                Id = editTour.Id,
                ID = editTour.Id,
                Keywords = editTour.Keywords,
                LengthInSecs = editTour.RunTime.TotalSeconds,
                LengthInSeconds = editTour.RunTime.TotalSeconds,
                OrganizationName = editTour.OrgName,
                OrganizationUrl = editTour.OrganizationUrl,
                OrgName = editTour.OrgName,
                OrgUrl = editTour.OrgUrl,
                // RelatedTours = editTour.RelatedTours,
                // Taxonomy = editTour.Taxonomy,
                ThumbNail = editTour.TourStops[0].Thumbnail,
                ThumbnailUrl = "", // editTour.TourThumbnailFilename,
                Title = editTour.Title,
                // TourAttributionAndCredits = editTour.TourAttributionAndCredits,
                TourDuration = editTour.RunTime.ToString(),
                TourUrl = filename
                // UserLevel = editTour.UserLevel
            };
        }

        private void addToCommunityToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void communitiesMenu_Opening(object sender, CancelEventArgs e)
        {
            updateLoginCredentialsMenuItem.Visible = Properties.Settings.Default.AdvancedCommunities;
        }

        private void regionalDataCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sharedCache = new SimpleInput(Language.GetLocalizedText(660, "Shared Data Cache Settings"), Language.GetLocalizedText(661, "Shared Data Cache URL (leave empty for none)"), Properties.Settings.Default.SharedCacheServer, 1024);
            sharedCache.MinLength = 0;
            if (sharedCache.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.SharedCacheServer = sharedCache.ResultText;
                CacheProxy.BaseUrl = Properties.Settings.Default.SharedCacheServer;
            }
        }

        private void addAsNewLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                var place = contextMenuTargetObject;
                AddPlaceAsLayer(place, true);
            }
            catch
            {


            }
        }

        public void AddPlaceAsLayer(IPlace place, bool refresh)
        {
            IImageSet imageSet = null;
            if (place.StudyImageset != null)
            {
                imageSet = place.StudyImageset;
            }
            if (place.BackgroundImageSet != null)
            {
                imageSet = place.BackgroundImageSet;
            }

            if (imageSet != null)
            {
                LayerManager.AddImagesetLayer(imageSet);

            }
        }

        private void addCollectionAsTourStopsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void multiChanelCalibrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            var calibrate = new Calibration();

            calibrate.Show();
        }


        public void ShowCalibrationScreen()
        {


            if (InvokeRequired)
            {
                MethodInvoker ShowMeCall = CalibrationScreen.ShowWindow;
                try
                {
                    Invoke(ShowMeCall);
                }
                catch { }
            }
            else
            {
                CalibrationScreen.ShowWindow();
            }
        }

        private void sendLayersToProjectorServersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveLayerSyncFile();
            var command = "SYNCLAYERS," + MainWindow.Config.ClusterID + ",-1";
            NetControl.SendCommand(command);
        }

        private static void SaveLayerSyncFile()
        {
            var layers = new LayerContainer();
            layers.SaveToFile(Properties.Settings.Default.CahceDirectory + "\\layerSync.layers");
            layers.Dispose();
            GC.SuppressFinalize(layers);
        }

        private void sendTourToProjectorServersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SyncTour();
        }

        private void SyncTour()
        {
            if (TourEdit != null)
            {
                if (TourEdit.Tour != null)
                {
                    TourEdit.Tour.SaveToFile(Properties.Settings.Default.CahceDirectory + "\\tourSync.wtt", true, true);

                    var command = "SYNCTOUR," + MainWindow.Config.ClusterID + ",-1";
                    NetControl.SendCommand(command);
                }
            }
        }

        private void automaticTourSyncWithProjectorServersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoSyncTours = !Properties.Settings.Default.AutoSyncTours;
            automaticTourSyncWithProjectorServersToolStripMenuItem.Checked = Properties.Settings.Default.AutoSyncTours;
        }

        public VideoOutputType dumpFrameParams = new VideoOutputType();

        RenderProgress RenderProgress;
        private void renderToVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var videoDialog = new OutputTourToVideo();
            videoDialog.Target = menuTabs.CurrentTour;


            if (TourEdit != null)
            {
                if (videoDialog.ShowDialog() == DialogResult.OK)
                {
                    SpaceTimeController.MetaNow = DateTime.Now;
                    SpaceTimeController.FrameDumping = true;
                    SpaceTimeController.FramesPerSecond = videoDialog.RenderValues.Fps;
                    SpaceTimeController.TotalFrames = videoDialog.RenderValues.TotalFrames;
                    SpaceTimeController.CurrentFrameNumber = videoDialog.RenderValues.StartFrame;
                    dumpFrameParams = videoDialog.RenderValues;
                    CaptureVideo = true;
                    RenderProgress = new RenderProgress();
                    RenderProgress.Owner = this;
                    RenderProgress.Show();
                    SpaceTimeController.CancelFrameDump = false;
                    TourEdit.PlayNow(false);
                }

            }
        }

        Texture11 touchControl;
        Texture11 touchControlNoHold;
        Texture11 trackerButton;
        bool kioskControl = false;
        readonly PositionColoredTextured[] TouchControlPoints = new PositionColoredTextured[4];
        private void DrawTouchControls()
        {

            if (Properties.Settings.Default.ShowTouchControls && !TourEditor.Capturing && !CaptureVideo)
            {

                MakeTouchPoints();

                if (trackerButton == null)
                {
                    trackerButton = Planets.LoadPlanetTexture(Resources.TrackerButton);
                }

                if (touchControl == null)
                {
                    var appdir = Path.GetDirectoryName(Application.ExecutablePath);
                    var customImageFile = appdir + "\\TouchControls.png";
                    var customImageFileNoHold = appdir + "\\TouchControlsNoHold.png";
                    if (File.Exists(customImageFile))
                    {
                        var bmp = new Bitmap(customImageFile);
                        touchControl = Planets.LoadPlanetTexture(bmp); 
                        bmp.Dispose();
                    }
                    else
                    {
                        touchControl = Planets.LoadPlanetTexture(Resources.TouchControls);
                    }

                    if (File.Exists(customImageFileNoHold))
                    {
                        var bmp = new Bitmap(customImageFileNoHold);
                        touchControlNoHold = Planets.LoadPlanetTexture(bmp); 
                        bmp.Dispose();
                    }
                    else
                    {
                        touchControlNoHold = Planets.LoadPlanetTexture(Resources.TouchControlsNoHold);
                    }

                }

                var x = RenderContext11.ViewPort.Width - 207;
                var y = RenderContext11.ViewPort.Height - (234 + 120);
                float w = 197;
                float h = 224;

                TouchControlPoints[0].X = x;
                TouchControlPoints[0].Y = y;
                TouchControlPoints[0].Z = .9f;
                TouchControlPoints[0].W = 1;
                TouchControlPoints[0].Tu = 0;
                TouchControlPoints[0].Tv = 0;
                TouchControlPoints[0].Color = Color.FromArgb(64, 255, 255, 255);

                TouchControlPoints[1].X = x + w;
                TouchControlPoints[1].Y = y;
                TouchControlPoints[1].Tu = 1;
                TouchControlPoints[1].Tv = 0;
                TouchControlPoints[1].Color = Color.FromArgb(64, 255, 255, 255);
                TouchControlPoints[1].Z = .9f;
                TouchControlPoints[1].W = 1;

                TouchControlPoints[2].X = x;
                TouchControlPoints[2].Y = y + h;
                TouchControlPoints[2].Tu = 0;
                TouchControlPoints[2].Tv = 1;
                TouchControlPoints[2].Color = Color.FromArgb(64, 255, 255, 255);
                TouchControlPoints[2].Z = .9f;
                TouchControlPoints[2].W = 1;

                TouchControlPoints[3].X = x + w;
                TouchControlPoints[3].Y = y + h;
                TouchControlPoints[3].Tu = 1;
                TouchControlPoints[3].Tv = 1;
                TouchControlPoints[3].Color = Color.FromArgb(64, 255, 255, 255);
                TouchControlPoints[3].Z = .9f;
                TouchControlPoints[3].W = 1;

                if (Friction)
                {
                    Sprite2d.DrawForScreen(RenderContext11, TouchControlPoints, 4, touchControlNoHold, PrimitiveTopology.TriangleStrip);
                }
                else
                {
                    Sprite2d.DrawForScreen(RenderContext11, TouchControlPoints, 4, touchControl, PrimitiveTopology.TriangleStrip);
                }

                if (!kioskControl)
                {
                    if (Friction && activeTouch == TouchControls.None)
                    {
                        var frictionFactor = (float)(1 - (lastFrameTime / 2));
                        moveVector.X *= frictionFactor;
                        moveVector.Y *= frictionFactor;
                        OrbitVector *= frictionFactor;
                        ZoomVector *= frictionFactor;
                    }


                    // Calculate Arc for Zoom
                    var zoomArc = new Vector2d(zoomTracker.X - ZoomVector, (zoomTracker.Y - Math.Cos(ZoomVector / 54 * Math.PI / 2) * 20) + 39);
                    touchPoints[(int)TouchControls.ZoomTrack] = zoomArc;

                    // Calculate Arc for Orbit
                    var orbitArc = new Vector2d(99 - (Math.Sin(OrbitVector / 70 * Math.PI / 2) * 68), 113 + (Math.Cos(OrbitVector / 70 * Math.PI / 2) * 75));
                    touchPoints[(int)TouchControls.OrbitTrack] = orbitArc;

                    // Calculate Current Pan Position
                    var panPos = new Vector2d(panTracker.X - moveVector.X, panTracker.Y - moveVector.Y);

                    touchPoints[(int)TouchControls.PanTrack] = panPos;

                    Sprite2d.Draw2D(RenderContext11, trackerButton, new SizeF(32, 32), new PointF(15, 15), 0, new PointF((float)touchPoints[(int)TouchControls.PanTrack].X + x, (float)touchPoints[(int)TouchControls.PanTrack].Y + y), Color.FromArgb(128, 255, 255, 255));
                    Sprite2d.Draw2D(RenderContext11, trackerButton, new SizeF(32, 32), new PointF(15, 15), 0, new PointF((float)touchPoints[(int)TouchControls.OrbitTrack].X + x, (float)touchPoints[(int)TouchControls.OrbitTrack].Y + y), Color.FromArgb(128, 255, 255, 255));
                    Sprite2d.Draw2D(RenderContext11, trackerButton, new SizeF(32, 32), new PointF(15, 15), 0, new PointF((float)touchPoints[(int)TouchControls.ZoomTrack].X + x, (float)touchPoints[(int)TouchControls.ZoomTrack].Y + y), Color.FromArgb(128, 255, 255, 255));
                }
            }
        }

        double kinectListOffsetTarget;
        double kinectPickValue;

        Folder kinectUi;

        bool kinectInit;
        private void DrawKinectUI()
        {
            var index = 0;
            var itemStride = 600;

            if (kinectUi == null && !kinectInit)
            {
                kinectInit = true;

                try
                {
                   kinectUi = Folder.LoadFromUrl("http://www.worldwidetelescope.org/wwtweb/catalog.aspx?w=kinect", false);
                }
                catch
                {
                }
            }

            if (kinectUi != null && menu)
            {
                var currentId = Math.Abs((int)((kinectListOffsetTarget - itemStride / 2) / itemStride));
                Place currentPlace = null;
                Place earthPlace = null;
                var gap = ((int)((kinectListOffsetTarget - itemStride / 2) / itemStride) * itemStride) - kinectListOffsetTarget;

                if (Math.Abs(gap) > .5 && !contact)
                {
                    kinectListOffsetTarget += Math.Min(10, Math.Abs(gap)) * Math.Sign(gap);
                }

                foreach (var item in kinectUi.Children)
                {
                    var place = item as Place;

                    if (place != null && place.Classification == Classification.SolarSystem)
                    {
                        var id = Planets.GetPlanetIDFromName(place.Name);
                        if (place.Name == "Solar Eclipse 2017")
                        {
                            id = 18;
                        }

                        if (place.Name == "Earth")
                        {
                            earthPlace = place;
                        }

                        if (id > -1)
                        {
                            if (index == currentId)
                            {
                                currentPlace = place;
                            }
                            var tex = Planets.PlanetTextures[id];

                            if (tex != null)
                            {

                                if (id > 9 && id < 18)
                                {
                                    Sprite2d.Draw2D(
                                      RenderContext11,
                                      tex,
                                      new SizeF(512, 512),
                                      new PointF(.5f, .5f),
                                      0,
                                      new PointF(
                                          (RenderContext11.ViewPort.Width / 2) +
                                          (index * itemStride + (int)kinectListOffsetTarget),
                                          RenderContext11.ViewPort.Height / 2
                                          ),
                                      Color.White
                                      );
                                 }
                                else if (id > 0)// && id != 18)
                                {
                                    Sprite2d.Draw2D(
                                       RenderContext11,
                                       tex,
                                       new SizeF(512, 512),
                                       new PointF(.5f, .5f),
                                       0,
                                       new PointF(
                                           (RenderContext11.ViewPort.Width / 2) +
                                           (index * itemStride + (int)kinectListOffsetTarget),
                                           RenderContext11.ViewPort.Height / 2
                                           ),
                                       Color.White
                                       );

                                }
                                else
                                {
                                    Sprite2d.Draw2D(
                                        RenderContext11,
                                        tex,
                                        new SizeF(1024, 1024),
                                        new PointF(.5f, .5f),
                                        0,
                                        new PointF(
                                            (RenderContext11.ViewPort.Width / 2) +
                                            (index * itemStride + (int)kinectListOffsetTarget),
                                            RenderContext11.ViewPort.Height / 2
                                            ),
                                        Color.White
                                        );
                                }
                            }
                            index++;
                        }
                    }
                }



                if (showNextObject)
                {
                    if (currentPlace != null)
                    {
                        if (currentPlace.Name == "Solar Eclipse 2017")
                        {
                            kinectEclipseMode = true;
                            SpaceTimeController.Now = new DateTime(2017, 08, 21, 16, 00, 00);
                            SpaceTimeController.TimeRate = 200;
                            SpaceTimeController.SyncToClock = true;
                            GotoTarget(earthPlace, false, false, true);
                        }
                        else
                        {
                            kinectEclipseMode = false;
                            GotoTarget(currentPlace, false, false, true);
                        }
                    }
                    showNextObject = false;
                    menu = false;
                }
            }
        }
        bool kinectEclipseMode;
        public bool NoShowTourEndPage = false;


        private void showTouchControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.ShowTouchControls = !Properties.Settings.Default.ShowTouchControls;
        }



        public string FocusReferenceFrame()
        {

            return CurrentImageSet.ReferenceFrame;

        }

        private void remoteAccessControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var wal = new RemoteAccessControl();

            wal.ShowDialog();
        }



        private void layerManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLayersWindows = true;
        }


        private void screenBroadcastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sb = new ScreenBroadcast();
            sb.Show();
        }

        private void monochromeStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.MonochromeImageStyle = !Properties.Settings.Default.MonochromeImageStyle;
            monochromeStyleToolStripMenuItem.Checked = Properties.Settings.Default.MonochromeImageStyle;
            Tile.GrayscaleStyle = Properties.Settings.Default.MonochromeImageStyle;
            TileCache.PurgeQueue();
            TileCache.ClearCache();
        }

        private void layersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = Language.GetLocalizedText(976, "WorldWide Telescope Layer File(*.wwtl)|*.wwtl");
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var filename = openFile.FileName;

                try
                {
                    LayerManager.LoadLayerFile(filename, LayerManager.CurrentMap, false);
                }
                catch
                {
                    MessageBox.Show(Language.GetLocalizedText(109, "This file does not seem to be a valid collection"), Language.GetLocalizedText(3, "Microsoft WorldWide Telescope"));
                }
            }
        }

        private void publishTourToCommunityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsLoggedIn)
            {
                var filename = Path.GetTempFileName();
                TourEdit.Tour.SaveToFile(filename, true, false);
                EOCalls.InvokePublishFile(filename, TourEdit.Tour.Title + ".wtt");
                RefreshCommunityLocal();
            }
        }


        public static void RefreshCommunity()
        {
            MethodInvoker doIt = delegate
            {
                MainWindow.RefreshCommunityLocal();
            };

            if (MainWindow.InvokeRequired)
            {
                try
                {
                    MainWindow.Invoke(doIt);
                }
                catch
                {
                }
            }
            else
            {
                doIt();
            }
        }

        public void RefreshCommunityLocal()
        {
            if (communitiesPane != null)
            {
                communitiesPane.ReloadFolder();
            }
        }

        private void findEarthBasedLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var locationSearch = new LocationSearch();
            if (searchPane != null)
            {
                locationSearch.ObejctName = searchPane.SearchStringText;
            }
            if (locationSearch.ShowDialog() == DialogResult.OK)
            {
                if (SolarSystemMode)
                {
                    var pnt = Coordinates.GeoTo3dDouble(locationSearch.Result.Lat, locationSearch.Result.Lng);

                    pnt = Vector3d.TransformCoordinate(pnt, Planets.EarthMatrix);
                    pnt.Normalize();
                    var radec = Coordinates.CartesianToLatLng(pnt);


                    TargetLat = radec.Y;
                    TargetLong = radec.X - 90;
                }
                else
                {
                    GotoTarget(locationSearch.Result, false, false, false);
                }
            }
        }

        private void fullDomePreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var domePopup = new DomePreviewPopup();
            domePopup.Show();
        }

        private void xBoxControllerSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XBoxConfig.ShowSetupWindow();
        }

        private void DeviceHeartbeat_Tick(object sender, EventArgs e)
        {
            MidiMapManager.Heartbeat();
        }

        private void mIDIControllerSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MidiSetup.ShowSetupWindow();
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.MultiSampling != 1)
            {
                Properties.Settings.Default.MultiSampling = 1;
                RestartNow();
            }
        }

        private void fourSamplesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.MultiSampling != 4)
            {
                Properties.Settings.Default.MultiSampling = 4;
                RestartNow();
            }
        }

        private void eightSamplesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.MultiSampling != 8)
            {
                Properties.Settings.Default.MultiSampling = 8;
                RestartNow();
            }
        }

        public void RestartNow()
        {
            if (UiTools.ShowMessageBox(Language.GetLocalizedText(1043, "WorldWide Telescope must restart for new settings to take effect. Do you want to restart now?"), Language.GetLocalizedText(694, "Restart Now?"), MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                LanguageReboot = true;
                Close();
            }
        }

        private void multiSampleAntialiasingToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            switch (Properties.Settings.Default.MultiSampling)
            {
                case 4:
                    noneToolStripMenuItem.Checked = false;
                    fourSamplesToolStripMenuItem.Checked = true;
                    eightSamplesToolStripMenuItem.Checked = false;
                    break;
                case 8:
                    noneToolStripMenuItem.Checked = false;
                    fourSamplesToolStripMenuItem.Checked = false;
                    eightSamplesToolStripMenuItem.Checked = true;
                    break;
                default:
                    noneToolStripMenuItem.Checked = true;
                    fourSamplesToolStripMenuItem.Checked = false;
                    eightSamplesToolStripMenuItem.Checked = false;
                    break;
            }
        }



        #region IScriptable Members
        // Ra, Declinations, Latitude, Longitude, Zoom, Angle, Rotation, ImageCrossfade, FadeToBlack
        ScriptableProperty[] IScriptable.GetProperties()
        {
            var props = new List<ScriptableProperty>();

            props.Add(new ScriptableProperty("Ra", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, 0, 24, false));
            props.Add(new ScriptableProperty("declination", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, -90, +90, false));
            props.Add(new ScriptableProperty("Latitude", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, -90, 90, false));
            props.Add(new ScriptableProperty("Longitude", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, -180, 180, false));
            props.Add(new ScriptableProperty("Zoom", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Log, ZoomMin, ZoomMax, false));
            props.Add(new ScriptableProperty("Angle", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, -Math.PI / 2, 0, false));
            props.Add(new ScriptableProperty("Rotation", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, -3.14, 3.14, false));
            props.Add(new ScriptableProperty("ZoomRate", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, -54, 54, false));
            props.Add(new ScriptableProperty("PanUpDownRate", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, -50, 50, false));
            props.Add(new ScriptableProperty("PanLeftRightRate", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, -50, 50, false));
            props.Add(new ScriptableProperty("RotationRate", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, -70, 70, false));
            props.Add(new ScriptableProperty("ImageCrossfade", ScriptablePropertyTypes.BlendState, ScriptablePropertyScale.Linear, 0, 100, true));
            props.Add(new ScriptableProperty("FadeToBlack", ScriptablePropertyTypes.BlendState, ScriptablePropertyScale.Linear, 0, 1, true));
            props.Add(new ScriptableProperty("SystemVolume", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, 0, 1, false));
            props.Add(new ScriptableProperty("DomeTilt", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, 0, 90, false));
            props.Add(new ScriptableProperty("DomeAngle", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, -180, 180, false));
            props.Add(new ScriptableProperty("DomeAlt", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, 0, 90, false));
            props.Add(new ScriptableProperty("DomeAz", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, -180, 180, false));
            props.Add(new ScriptableProperty("FisheyeAngle", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, 0, 270, false));
            props.Add(new ScriptableProperty("NavigationHold", ScriptablePropertyTypes.Bool, ScriptablePropertyScale.Linear, 0, 1, true));
            props.Add(new ScriptableProperty("ScreenFOV", ScriptablePropertyTypes.Double, ScriptablePropertyScale.Linear, 1, 89, false));
            return props.ToArray();
        }

        string[] IScriptable.GetActions()
        {
            return Enum.GetNames(typeof(NavigationActions));
        }

        void IScriptable.InvokeAction(string name, string value)
        {
            MethodInvoker updatePlace = delegate
            {
                if (string.IsNullOrEmpty(name))
                {
                    return;
                }
                try
                {
                    var action = (NavigationActions)Enum.Parse(typeof(NavigationActions), name, true);

                    switch (action)
                    {
                        case NavigationActions.ResetRiftView:
                            if (rift)
                            {
                                ResetRift();
                            }
                            break;
                        case NavigationActions.AllStop:
                            TouchAllStop();
                            break;
                        case NavigationActions.MoveUp:
                            if (!string.IsNullOrEmpty(value))
                            {
                                MoveView(0, double.Parse(value) * 10, false);
                            }
                            else
                            {
                                MoveUp();
                            }
                            break;
                        case NavigationActions.MoveDown:
                            if (!string.IsNullOrEmpty(value))
                            {
                                MoveView(0, -double.Parse(value) * 10, false);
                            }
                            else
                            {
                                MoveDown();
                            }

                            break;
                        case NavigationActions.MoveRight:
                            if (!string.IsNullOrEmpty(value))
                            {
                                MoveView(double.Parse(value) * 10, 0, false);
                            }
                            else
                            {
                                MoveRight();
                            }
                            break;
                        case NavigationActions.MoveLeft:
                            if (!string.IsNullOrEmpty(value))
                            {
                                MoveView(-double.Parse(value) * 10, 0, false);
                            }
                            else
                            {
                                MoveLeft();
                            }
                            break;
                        case NavigationActions.ZoomIn:
                            if (!string.IsNullOrEmpty(value))
                            {
                                ZoomIn(double.Parse(value));
                            }
                            else
                            {
                                ZoomIn();
                            }
                            break;
                        case NavigationActions.ZoomOut:
                            if (!string.IsNullOrEmpty(value))
                            {
                                ZoomOut(double.Parse(value));
                             }
                            else
                            {
                                ZoomOut();
                            }
                            break;
                        case NavigationActions.RotateLeft:
                            if (!string.IsNullOrEmpty(value))
                            {
                                RotateLeft(double.Parse(value));
                            }
                            else
                            {
                                RotateLeft(1);
                            }
                            break;
                        case NavigationActions.RotateRight:
                            if (!string.IsNullOrEmpty(value))
                            {
                                RotateRight(double.Parse(value));
                            }
                            else
                            {
                                RotateRight(1);
                            }
                            break;
                        case NavigationActions.DomeLeft:
                            if (!string.IsNullOrEmpty(value))
                            {
                                DomeLeft(double.Parse(value));
                            }
                            else
                            {
                                DomeLeft(1);
                            }
                            break;

                        case NavigationActions.DomeRight:
                            if (!string.IsNullOrEmpty(value))
                            {
                                DomeRight(double.Parse(value));
                            }
                            else
                            {
                                DomeRight(1);
                            }
                            break;

                        case NavigationActions.DomeUp:
                            if (!string.IsNullOrEmpty(value))
                            {
                                DomeUp(double.Parse(value));
                            }
                            else
                            {
                                DomeUp(1);
                            }
                            break;

                        case NavigationActions.DomeDown:
                            if (!string.IsNullOrEmpty(value))
                            {
                                DomeUp(double.Parse(value));
                            }
                            else
                            {
                                DomeUp(1);
                            }
                            break;


                        case NavigationActions.TiltUp:
                            if (!string.IsNullOrEmpty(value))
                            {
                                TiltUp(double.Parse(value));
                            }
                            else
                            {
                                TiltUp(1);
                            }
                            break;
                        case NavigationActions.TiltDown:
                            if (!string.IsNullOrEmpty(value))
                            {
                                TiltDown(double.Parse(value));
                            }
                            else
                            {
                                TiltDown(1);
                            }
                            break;
                        case NavigationActions.ResetCamera:
                            ResetCamera();
                            break;
                        case NavigationActions.NextItem:
                            explorePane.MoveNext();
                            break;
                        case NavigationActions.LastItem:
                            explorePane.MovePrevious();
                            break;
                        case NavigationActions.Select:
                            explorePane.SelectItem();
                            break;
                        case NavigationActions.Back:
                            explorePane.Back();
                            break;
                        case NavigationActions.ShowNextContext:
                            contextPanel.ShowNextObject();
                            break;
                        case NavigationActions.ShowPreviousContext:
                            contextPanel.ShowPreviousObject();
                            break;
                        case NavigationActions.ShowNextExplore:
                            explorePane.MoveNext();
                            explorePane.ShowCurrent();
                            break;
                        case NavigationActions.ShowPreviousExplore:
                            explorePane.MovePrevious();
                            explorePane.ShowCurrent();
                            break;
                        case NavigationActions.SetForeground:
                            explorePane.SelectForeground();
                            break;
                        case NavigationActions.SetBackground:
                            explorePane.SelectBackground();
                            break;
                        case NavigationActions.PreviousMode:
                            PreviousMode();
                            break;
                        case NavigationActions.NextMode:
                            NextMode();
                            break;
                        case NavigationActions.SolarSystemMode:
                            {
                                var lookAt = ImageSetType.SolarSystem;
                                contextPanel.SetLookAtTarget(lookAt);
                            }
                            break;
                        case NavigationActions.SkyMode:
                            {
                                var lookAt = ImageSetType.Sky;
                                contextPanel.SetLookAtTarget(lookAt);
                            }
                            break;
                        case NavigationActions.EarthMode:
                            {
                                var lookAt = ImageSetType.Earth;
                                contextPanel.SetLookAtTarget(lookAt);
                            }
                            break;
                        case NavigationActions.PlanetMode:
                            {
                                var lookAt = ImageSetType.Planet;
                                contextPanel.SetLookAtTarget(lookAt);
                            }
                            break;
                        case NavigationActions.PanoramaMode:
                            {
                                var lookAt = ImageSetType.Panorama;
                                contextPanel.SetLookAtTarget(lookAt);
                            }
                            break;
                        case NavigationActions.SandboxMode:
                            {
                                var lookAt = ImageSetType.Sandbox;
                                contextPanel.SetLookAtTarget(lookAt);
                            }
                            break;
                        case NavigationActions.PlayTour:
                            {
                                if (tourEdit != null)
                                {
                                    tourEdit.PlayNow(false);
                                }
                            }
                            break;
                        case NavigationActions.PauseTour:
                            {
                                if (tourEdit != null)
                                {
                                    tourEdit.PauseTour();
                                }
                            }
                            break;
                        case NavigationActions.StopTour:
                            {
                                if (tourEdit != null)
                                {
                                    tourEdit.PauseTour();
                                    CloseTour(false);
                                }
                            }
                            break;
                        case NavigationActions.NextSlide:
                            {
                                if (TourPlayer.Playing)
                                {
                                    var player = uiController as TourPlayer;
                                    if (player != null)
                                    {
                                        player.MoveNextSlide();
                                    }

                                }
                            }
                            break;
                        case NavigationActions.GotoSlide:
                            if (!string.IsNullOrEmpty(value))
                            {
                                var slideID = -1;
                                int.TryParse(value, out slideID);

                                if (TourPlayer.Playing && slideID > -1)
                                {
                                    var player = uiController as TourPlayer;
                                    if (player != null)
                                    {
                                        player.MoveToSlide(slideID);
                                    }

                                }
                            }
                            break;
                        case NavigationActions.PreviousSlide:
                            {
                                if (TourPlayer.Playing)
                                {
                                    var player = uiController as TourPlayer;
                                    if (player != null)
                                    {
                                        player.MovePreviousSlide();
                                    }

                                }
                            }
                            break;
                        case NavigationActions.MoveToEndSlide:
                            {
                                if (TourPlayer.Playing)
                                {
                                    var player = uiController as TourPlayer;
                                    if (player != null)
                                    {
                                        player.MoveToEndSlide();
                                    }

                                }
                            }
                            break;
                        case NavigationActions.MoveToFirstSlide:
                            {
                                if (TourPlayer.Playing)
                                {
                                    var player = uiController as TourPlayer;
                                    if (player != null)
                                    {
                                        player.MoveToEndSlide();
                                    }

                                }
                            }
                            break;
                        case NavigationActions.GotoSun:
                            {
                                var place = Search.FindCatalogObjectExact("Sun");
                                GotoTarget(place, false, false, true);

                            }
                            break;
                        case NavigationActions.GotoMercury:
                            {
                                var place = Search.FindCatalogObjectExact("Mercury");
                                GotoTarget(place, false, false, true);

                            }
                            break;
                        case NavigationActions.GotoVenus:
                            {
                                var place = Search.FindCatalogObjectExact("Venus");
                                GotoTarget(place, false, false, true);

                            }
                            break;
                        case NavigationActions.GotoEarth:
                            {
                                var place = Search.FindCatalogObjectExact("Earth");
                                GotoTarget(place, false, false, true);

                            }
                            break;
                        case NavigationActions.GotoMars:
                            {
                                var place = Search.FindCatalogObjectExact("Mars");
                                GotoTarget(place, false, false, true);

                            }
                            break;
                        case NavigationActions.GotoJupiter:
                            {
                                var place = Search.FindCatalogObjectExact("Jupiter");
                                GotoTarget(place, false, false, true);

                            }
                            break;
                        case NavigationActions.GotoSaturn:
                            {
                                var place = Search.FindCatalogObjectExact("Saturn");
                                GotoTarget(place, false, false, true);

                            }
                            break;
                        case NavigationActions.GotoUranus:
                            {
                                var place = Search.FindCatalogObjectExact("Uranus");
                                GotoTarget(place, false, false, true);

                            }
                            break;
                        case NavigationActions.GotoNeptune:
                            {
                                var place = Search.FindCatalogObjectExact("Neptune");
                                GotoTarget(place, false, false, true);

                            }
                            break;
                        case NavigationActions.GotoPluto:
                            {
                                var place = Search.FindCatalogObjectExact("Pluto");
                                GotoTarget(place, false, false, true);

                            }
                            break;
                        case NavigationActions.SolarSystemOverview:
                            {
                                var cameraParams = new CameraParameters(45, 45, 300, 0, 0, 100);

                                MainWindow.GotoTarget(cameraParams, false, false);

                            }
                            break;
                        case NavigationActions.GotoMilkyWay:
                            {
                                var cameraParams = new CameraParameters(45, 45, 10000000000, 0, 0, 100);

                                MainWindow.GotoTarget(cameraParams, false, false);

                            }
                            break;
                        case NavigationActions.GotoSDSSGalaxies:
                            {
                                var cameraParams = new CameraParameters(45, 45, 300000000000000, 0, 0, 100);

                                MainWindow.GotoTarget(cameraParams, false, false);

                            }
                            break;

                        default:
                            break;

                    }
                }
                catch
                {
                }
            };
            try
            {
                Invoke(updatePlace);
            }
            catch
            {
            }
        }

        void IScriptable.SetProperty(string name, string value)
        {
            try
            {
                var prop = (NavigationProperties)Enum.Parse(typeof(NavigationProperties), name, true);

                switch (prop)
                {
                    case NavigationProperties.Ra:
                        TargetLong = RAtoViewLng(double.Parse(value));
                        break;
                    case NavigationProperties.Declination:
                        TargetLat = double.Parse(value);
                        break;
                    case NavigationProperties.Latitude:
                        TargetLat = double.Parse(value);
                        break;
                    case NavigationProperties.Longitude:
                        TargetLong = double.Parse(value);
                        break;
                    case NavigationProperties.Zoom:
                        break;
                    case NavigationProperties.Angle:
                        {
                            var val = double.Parse(value);
                            targetViewCamera.Angle = val;
                        }
                        break;
                    case NavigationProperties.Rotation:
                        {
                            var val = double.Parse(value);
                            targetViewCamera.Rotation = val;
                        }
                        break;
                    case NavigationProperties.ZoomRate:
                        {
                            var val = double.Parse(value);
                            ZoomVector = (float)val;
                            timer.Enabled = true;
                        }
                        break;
                    case NavigationProperties.PanUpDownRate:
                        {
                            var val = double.Parse(value);
                            moveVector.Y = (float)val;
                            timer.Enabled = true;
                        }
                        break;
                    case NavigationProperties.PanLeftRightRate:
                        {
                            var val = double.Parse(value);
                            moveVector.X = (float)val;
                            timer.Enabled = true;
                        }
                        break;
                    case NavigationProperties.RotationRate:
                        {
                            var val = double.Parse(value);
                            OrbitVector = (float)val;
                            timer.Enabled = true;
                        }
                        break;


                    case NavigationProperties.DomeAlt:
                        {
                            var val = double.Parse(value);
                            viewCamera.DomeAlt = val;
                        }
                        break;

                    case NavigationProperties.DomeAz:
                        {
                            var val = double.Parse(value);
                            viewCamera.DomeAz = val;
                        }
                        break;

                    case NavigationProperties.DomeTilt:
                        {
                            var val = double.Parse(value);
                            MainWindow.Config.DomeTilt = (float)val;
                        }
                        break;

                    case NavigationProperties.DomeAngle:
                        {
                            var val = double.Parse(value);
                            MainWindow.Config.DomeAngle = (float)val;
                        }
                        break;

                    case NavigationProperties.FisheyeAngle:
                        {
                            var val = double.Parse(value);
                            Properties.Settings.Default.FisheyeAngle = (float)val;
                        }
                        break;
                    case NavigationProperties.ImageCrossfade:
                        {
                            var val = double.Parse(value);
                            StudyOpacity = (float)(val);

                        }
                        break;
                    case NavigationProperties.FadeToBlack:
                        {
                            var val = double.Parse(value);
                            Fader.Opacity = 1f - (float)(val);
                        }
                        break;
                    case NavigationProperties.SystemVolume:
                        {
                            var val = double.Parse(value);
                            UiTools.SetSystemVolume((float)val);
                        }
                        break;

                    case NavigationProperties.NavigationHold:
                        {
                            var val = double.Parse(value);
                            Friction = val != 0;
                        }
                        break;
                    case NavigationProperties.ScreenFOV:
                        {
                            var val = double.Parse(value);
                            fovLocal = (float)val / 180 * Math.PI;
                        }
                        break;

                    default:
                        break;
                }
            }
            catch
            {
            }
        }

        string IScriptable.GetProperty(string name)
        {
            try
            {
                var prop = (NavigationProperties)Enum.Parse(typeof(NavigationProperties), name, true);
                switch (prop)
                {
                    //todo Fix maps for Get properties to match set
                    case NavigationProperties.ImageCrossfade:
                        {
                            if (StudyOpacity > 0)
                            {
                                StudyOpacity = 100;
                                return true.ToString();
                            }
                            StudyOpacity = 0;
                            return false.ToString();
                        }

                    case NavigationProperties.FadeToBlack:
                        {

                            return Fader.TargetState.ToString();
                        }
                    case NavigationProperties.SystemVolume:
                        {
                            return UiTools.GetSystemVolume().ToString();
                        }

                    case NavigationProperties.NavigationHold:
                        {
                            return Friction ? "1" : "0";
                        }

                    case NavigationProperties.ScreenFOV:
                        return (fovLocal / Math.PI * 180).ToString();

                    case NavigationProperties.Ra:
                        return RA.ToString();

                    case NavigationProperties.Declination:
                        return TargetLat.ToString();

                    case NavigationProperties.Latitude:
                        return TargetLat.ToString();

                    case NavigationProperties.Longitude:
                        return TargetLong.ToString();

                    case NavigationProperties.Zoom:
                        return ZoomFactor.ToString();

                    case NavigationProperties.Angle:
                        {
                            return targetViewCamera.Angle.ToString();
                        }

                    case NavigationProperties.Rotation:
                        {
                            return targetViewCamera.Rotation.ToString();
                        }


                    case NavigationProperties.DomeAlt:
                        {
                            return viewCamera.DomeAlt.ToString();
                        }


                    case NavigationProperties.DomeAz:
                        {
                            return viewCamera.DomeAz.ToString();
                        }

                    case NavigationProperties.DomeTilt:
                        {
                            return Config.DomeTilt.ToString();
                        }


                    case NavigationProperties.DomeAngle:
                        {
                            return Config.DomeAngle.ToString();
                        }


                    case NavigationProperties.FisheyeAngle:
                        {
                            return Properties.Settings.Default.FisheyeAngle.ToString();
                        }


                    default:
                        return "0";

                }
            }
            catch
            {
                return "0";
            }
        }

        bool IScriptable.ToggleProperty(string name)
        {
            var prop = (NavigationProperties)Enum.Parse(typeof(NavigationProperties), name);

            switch (prop)
            {

                case NavigationProperties.ImageCrossfade:
                    {
                        if (StudyOpacity > 0)
                        {
                            StudyOpacity = 100;
                            return true;
                        }
                        StudyOpacity = 0;
                        return false;
                    }

                case NavigationProperties.FadeToBlack:
                    {
                        Fader.TargetState = !Fader.TargetState;
                        return Fader.TargetState;
                    }


                case NavigationProperties.NavigationHold:
                    {
                        Friction = !Friction;
                        return Friction;
                    }
                default:
                    break;
            }
            return false;
        }

        #endregion

        public void GotoCatalogObject(string name)
        {
            MethodInvoker updatePlace = delegate
            {
                if (string.IsNullOrEmpty(name))
                {
                    return;
                }
                try
                {
                    var place = Search.FindCatalogObjectExact(name);
                    MainWindow.GotoTarget(place, false, false, true);
                }
                catch
                {
                }
            };
            try
            {
                Invoke(updatePlace);
            }
            catch
            {
            }
        }

        private void settingsMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void clientNodeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClientNodeList.IsNodeListVisible())
            {
                ClientNodeList.HideNodeList();
            }
            else
            {
                ClientNodeList.ShowNodeList();
            }
        }

        private void cacheImageryTilePyramidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var place = contextMenuTargetObject;
                IImageSet imageSet = null;
                if (place.StudyImageset != null)
                {
                    imageSet = place.StudyImageset;
                }
                if (place.BackgroundImageSet != null)
                {
                    imageSet = place.BackgroundImageSet;
                }
                if (imageSet != null)
                {
                    var ctp = new CacheTilePyramid();
                    ctp.imageSet = imageSet;
                    ctp.ShowDialog();
                }
            }
            catch
            {


            }
        }

        private void restoreCacheFromCabinetFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreCache();
        }

        public static void RestoreCache()
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = Language.GetLocalizedText(1056, "WWT Cabinet File(*.cabinet)|*.cabinet");
            openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            openFile.RestoreDirectory = true;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFile.FileName))
                {
                    ExtractCacheCabinet(openFile.FileName, false);
                }
            }
        }

        private static void ExtractCacheCabinet(string filename, bool eraseFirst)
        {
            var dataDir = Properties.Settings.Default.CahceDirectory;
            if (eraseFirst)
            {
                if (Directory.Exists(dataDir))
                {
                    Directory.Delete(dataDir, true);
                }
            }

            var cab = new FileCabinet(filename, dataDir);
            cab.Extract();

        }

        private void saveCacheAsCabinetFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtractCache();
        }

        public static void ExtractCache()
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = Language.GetLocalizedText(1056, "WWT Cabinet File(*.cabinet)|*.cabinet");
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveDialog.AddExtension = true;
            saveDialog.DefaultExt = ".cabinet";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\WorldWideTelescope";

                    var cab = new FileCabinet(saveDialog.FileName, Properties.Settings.Default.CahceDirectory);

                    InjestDirectory(cab, path);
                    cab.Package();
                }
                catch (Exception ex)
                {
                    UiTools.ShowMessageBox(ex.Message, "Error Saving cache");
                }
            }
        }

        private static void InjestDirectory(FileCabinet cab, string path)
        {
            foreach (var dir in Directory.GetDirectories(path))
            {
                InjestDirectory(cab, dir);

            }
            foreach (var file in Directory.GetFiles(path))
            {
                cab.AddFile(file);

            }
        }

        private void showCacheSpaceUsedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var place = contextMenuTargetObject;
                IImageSet imageSet = null;
                if (place.StudyImageset != null)
                {
                    imageSet = place.StudyImageset;
                }
                if (place.BackgroundImageSet != null)
                {
                    imageSet = place.BackgroundImageSet;
                }

                long totalBytes = 0;
                long demBytes = 0;
                var path = Properties.Settings.Default.CahceDirectory + @"Imagery\" + imageSet.ImageSetID;

                var demPath = Properties.Settings.Default.CahceDirectory + @"dem\" + Math.Abs(imageSet.DemUrl != null ? imageSet.DemUrl.GetHashCode32() : 0);

                Cursor.Current = Cursors.WaitCursor;

                if (!Directory.Exists(path) && !Directory.Exists(demPath))
                {
                    UiTools.ShowMessageBox(Language.GetLocalizedText(127, "There is no image data in the disk cache to purge."));
                    return;
                }

                ClearCache.TotalDir(path, ref totalBytes, 0);


                if (Directory.Exists(demPath))
                {
                    ClearCache.TotalDir(demPath, ref demBytes, 0);
                }

                Cursor.Current = Cursors.Default;
                UiTools.ShowMessageBox(String.Format(Language.GetLocalizedText(1048, "The imageset uses {0}MB of disk space."), ((totalBytes + demBytes) / 1024.0 / 1024.0).ToString("f")));

            }
            catch
            {


            }

        }

        public void ShowDebugBitmap(Bitmap bmp)
        {

            MethodInvoker ShowMeCall = delegate
            {
                DBmp.ShowBitmap(bmp);
            };
            try
            {
                Invoke(ShowMeCall);
            }
            catch
            {
            }

        }

        private void lockVerticalSyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FrameSync = !Properties.Settings.Default.FrameSync;
            lockVerticalSyncToolStripMenuItem.Checked = Properties.Settings.Default.FrameSync;
        }

        private void fpsToolStripMenuItemUnlimited_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TargetFrameRate = 0;
        }

        private void fPSToolStripMenuItem60_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TargetFrameRate = 60;
        }
        private void fPSToolStripMenuItem30_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TargetFrameRate = 30;
        }

        private void fPSToolStripMenuItem24_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TargetFrameRate = 24;
        }

        private void targetFrameRateToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            fpsToolStripMenuItemUnlimited.Checked = Properties.Settings.Default.TargetFrameRate == 0;
            fPSToolStripMenuItem60.Checked = Properties.Settings.Default.TargetFrameRate == 60;
            fPSToolStripMenuItem30.Checked = Properties.Settings.Default.TargetFrameRate == 30;
            fPSToolStripMenuItem24.Checked = Properties.Settings.Default.TargetFrameRate == 24;
        }

        private void showOverlayListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OverlayList.ShowOverlayList();
        }

        private void tileLoadingThrottlingToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            tpsToolStripMenuItem15.Checked = Properties.Settings.Default.TileThrottling == 15;
            tpsToolStripMenuItem30.Checked = Properties.Settings.Default.TileThrottling == 30;
            tpsToolStripMenuItem60.Checked = Properties.Settings.Default.TileThrottling == 60;
            tpsToolStripMenuItem120.Checked = Properties.Settings.Default.TileThrottling == 120;
            tpsToolStripMenuItemUnlimited.Checked = Properties.Settings.Default.TileThrottling == 12000;
        }

        private void tpsToolStripMenuItem15_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TileThrottling = 15;
        }

        private void tpsToolStripMenuItem30_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TileThrottling = 30;
        }

        private void tpsToolStripMenuItem60_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TileThrottling = 60;
        }

        private void tpsToolStripMenuItem120_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TileThrottling = 120;
        }

        private void tpsToolStripMenuItemUnlimited_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TileThrottling = 12000;
        }

        private void allowUnconstrainedTiltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UnconstrainedTilt = !Properties.Settings.Default.UnconstrainedTilt;
        }

        private void showSlideNumbersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowSlideNumbers = !Properties.Settings.Default.ShowSlideNumbers;

            if (tourEdit != null)
            {
                tourEdit.Refresh();
            }
        }

        private void showKeyframerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TimeLine.AreOpenTimelines)
            {
                KeyFramer.HideTimeline();
            }
            else
            {
                KeyFramer.ShowTimeline();
            }
        }

        private void ShowWelcomeTips_Click(object sender, EventArgs e)
        {
            ShowWelcome();
        }

        private void customGalaxyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "Delimeted Text(*.csv;*.tdf;*.txt)|*.csv;*.tdf;*.txt";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var filename = openFile.FileName;
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    Grids.InitializeCustomCosmos(filename);
                }
                catch
                {
                    //todo localization
                    UiTools.ShowMessageBox("Could not load file");
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void newFullDomeViewInstanceToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            monitorEightToolStripMenuItem.Visible = Screen.AllScreens.Length > 7;
            monitorSevenToolStripMenuItem.Visible = Screen.AllScreens.Length > 6;
            monitorSixToolStripMenuItem.Visible = Screen.AllScreens.Length > 5;
            monitorFiveToolStripMenuItem.Visible = Screen.AllScreens.Length > 4;
            monitorFourToolStripMenuItem.Visible = Screen.AllScreens.Length > 3;
            monitorThreeToolStripMenuItem.Visible = Screen.AllScreens.Length > 2;
            monitorTwoToolStripMenuItem.Visible = Screen.AllScreens.Length > 1;
            monitorOneToolStripMenuItem.Visible = Screen.AllScreens.Length > 0;
        }

        private void CreateDomeInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);
            CreateDomeInstance(id);
        }

        private static void CreateDomeInstance(int id)
        {
            Process.Start("wwtexplorer.exe", string.Format("-screen:{0} -domeviewer", id));
        }



        internal void SetHeadPosition(Vector3d head)
        {
            // Need to filter this for noise and jitter;
            HeadPosition = head;

        }
        Vector3d HeadPosition;

        private void exportCurrentViewAsSTLFileFor3DPrintingToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var rect = new GeoRect();

            var amount = ZoomFactor/10;

            rect.North = MainWindow.viewCamera.Lat + amount;
            rect.South = MainWindow.viewCamera.Lat - amount;
            rect.West = MainWindow.viewCamera.Lng - amount;
            rect.East = MainWindow.viewCamera.Lng + amount;

            var props = new ExportSTL();
            props.Rect = rect;
            props.Owner = MainWindow;
            props.Show();
        }

        private void advancedToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            clientNodeListToolStripMenuItem.Checked = ClientNodeList.IsNodeListVisible();
        }
    }
}
