using System;
using kOS;
using kOS.AddOns;
using kOS.Module;
using kOS.Safe;
using kOS.Safe.Binding;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Utilities;
using kOS.Screen;
using kOS.Suffixed;
using kOS.Suffixed.PartModuleField;
using kOS_WPKSP;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace kOS_WPKSP
{
    [kOSAddon("WPKSP")]
    [KOSNomenclature("WPKSPAddon")]
    public class WPKSPAddon : kOS.Suffixed.Addon
    {
        public static readonly String version = "1.2.1.0";
        static WPKSPAddon()
        {
        }

        public WPKSPAddon(SharedObjects shared) : base(shared)
        {
            InitializeSuffixes();
        }

        public override BooleanValue Available()
        {
            return true;
        }

        new public void InitializeSuffixes()
        {
            AddSuffix("HIDETOOLBAR", new NoArgsVoidSuffix(HideToolbar));
            AddSuffix("SHOWTOOLBAR", new NoArgsVoidSuffix(ShowToolbar));
            AddSuffix(new[]{"SHOWINFO", "INFO"}, new OneArgsSuffix<StringValue>(SelectInfo));
            AddSuffix("RESOURCES", new SetSuffix<BooleanValue>(new SuffixGetDlg<BooleanValue>(IsResourcesShowing), new SuffixSetDlg<BooleanValue>(ShowResources)));
            AddSuffix(new[]{"SHOWTIME", "TIME"}, new OneArgsSuffix<StringValue>(SelectTime));
            AddSuffix("TOGGLETIME", new NoArgsVoidSuffix(ToggleTimeMode));
            AddSuffix(new[]{"TOGGLECUTOUTS", "CUTOUTS", "CUTOUT"}, new NoArgsVoidSuffix(ToggleIVACutouts));
            AddSuffix(new[]{"TOGGLECOM", "COM", "MASS"}, new NoArgsVoidSuffix(ToggleCOM));
            AddSuffix(new[]{"TOGGLECOL", "COL", "LIFT"}, new NoArgsVoidSuffix(ToggleCOL));
            AddSuffix(new[]{"TOGGLECOT", "COT", "THRUST"}, new NoArgsVoidSuffix(ToggleCOT));
            AddSuffix("TOGGLEHUD", new NoArgsVoidSuffix(ToggleHUD));
            AddSuffix("HIDEHUD", new NoArgsVoidSuffix(HideHUD));
            AddSuffix("SHOWHUD", new NoArgsVoidSuffix(ShowHUD));
            AddSuffix("HUD", new SetSuffix<BooleanValue>(new SuffixGetDlg<BooleanValue>(IsHUDVisible), new SuffixSetDlg<BooleanValue>(SetHUD)));
            AddSuffix("VERSION", new Suffix<StringValue>(GetVersion));
        }

        private StringValue GetVersion()
        {
            return new StringValue(version);
        }

        private void HideToolbar()
        {
            if (ApplicationLauncher.Instance != null) {
                ApplicationLauncher.Instance.Hide();
            }
            else
            {
                throw new kOS.Safe.Exceptions.KOSException("The toolbar is not instanciated.");
            }
        }

        private void ShowToolbar()
        {
            if (ApplicationLauncher.Instance != null) {
                ApplicationLauncher.Instance.Show();
            }
            else
            {
                throw new kOS.Safe.Exceptions.KOSException("The toolbar is not instanciated.");
            }
        }

        private void SelectInfo(StringValue infoName)
        {
            if (FlightUIModeController.Instance == null)
            {
                throw new kOS.Safe.Exceptions.KOSException("Flight UI is not instanciated.");
            }
            else
            {
                string infoNameStr = infoName.ToString();
                switch (infoNameStr.ToUpperInvariant())
                {
                    case "STAGING":
                    case "INPUT":
                    case "DEFAULT":
                        FlightUIModeController.Instance.SetMode(FlightUIMode.STAGING);
                        break;
                    case "MANEUVER":
                    case "ORBIT":
                    case "ORBITAL":
                        FlightUIModeController.Instance.SetMode(FlightUIMode.MANEUVER_INFO);
                        break;
                    case "DOCK":
                    case "DOCKING":
                        FlightUIModeController.Instance.SetMode(FlightUIMode.DOCKING);
                        break;
                    default:
                        throw new kOS.Safe.Exceptions.KOSException("'" + infoNameStr + "'is invalid.");
                }
            }
        }

        private void ToggleTimeMode()
        {
            var metDisplay = UnityEngine.Object.FindObjectOfType<global::KSP.UI.Screens.Flight.METDisplay>();
            if (metDisplay != null)
            {
                metDisplay.ToggleTimeMode();
            }
            else
            {
                throw new kOS.Safe.Exceptions.KOSException("Failed to fetch the METDisplay instance");
            }
        }

        private void SelectTime(StringValue timeFormat) {
            var metDisplay = UnityEngine.Object.FindObjectOfType<global::KSP.UI.Screens.Flight.METDisplay>();
            string timeFormatStr = timeFormat.ToString();

            if (metDisplay != null) {
                switch (timeFormatStr.ToUpperInvariant())
                {
                    case "UT":
                    case "UNIVERSAL":
                    case "UNIVERSALTIME":
                    case "CALENDAR":
                        metDisplay.SetTimeMode(true);
                        break;
                    case "MET":
                    case "MISSION":
                    case "MISSIONTIME":
                        metDisplay.SetTimeMode(false);
                        break;
                    default:
                        throw new kOS.Safe.Exceptions.KOSException("Failed to set time mode to '" + timeFormatStr + "'");
                }
            }
            else
            {
                throw new kOS.Safe.Exceptions.KOSException("Failed to fetch the METDisplay instance");
            }
        }

        private void ToggleIVACutouts()
        {
            Vessel v = FlightGlobals.ActiveVessel;
            if (v != null)
            {
                var activeCutouts = UnityEngine.Object.FindObjectsOfType<global::InternalSpaceOverlay>();
                
                if (activeCutouts.Length == 0)
                {
                    foreach (var cutout in activeCutouts)
                    {
                        cutout.Dismiss();
                    }
                }
                else
                {
                    global::InternalSpaceOverlay.Create(FlightGlobals.ActiveVessel, null);
                }
            }
        }

        private void ToggleCOM()
        {
            FlightVesselOverlays.fetch.ToggleCoM();
        }

        private void ToggleCOL()
        {
            FlightVesselOverlays.fetch.ToggleCoL();
        }

        private void ToggleCOT()
        {
            FlightVesselOverlays.fetch.ToggleCoT();
        }

        private void ShowResources(BooleanValue show)
        {
            if (ResourceDisplay.Instance != null && ResourceDisplay.Instance.appLauncherButton != null)
            {
                if (show.Value == true)
                {
                    ResourceDisplay.Instance.appLauncherButton.SetTrue();
                }
                else
                {
                    ResourceDisplay.Instance.appLauncherButton.SetFalse();
                }
            }
        }

        private BooleanValue IsResourcesShowing(){
            if (ResourceDisplay.Instance != null && ResourceDisplay.Instance.appLauncherButton != null)
            {
                var button = ResourceDisplay.Instance.appLauncherButton;
                if (button.toggleButton != null)
                {
                    return new BooleanValue(button.toggleButton.Value);
                }
            }
            throw new kOS.Safe.Exceptions.KOSException("Failed to read resources state");
        }

        private BooleanValue IsHUDVisible()
        {
            if (global::KSP.UI.UIMasterController.Instance != null)
            {
                return new BooleanValue(global::KSP.UI.UIMasterController.Instance.isUIShowing);
            }
            throw new kOS.Safe.Exceptions.KOSException("UIMasterController is not instanciated");
        }

        private void ShowHUD()
        {
            global::GameEvents.onShowUI.Fire();
        }

        private void HideHUD()
        {
            global::GameEvents.onHideUI.Fire();
        }

        private void ToggleHUD()
        {
            if (IsHUDVisible().Value == true)
            {
                HideHUD();
            }
            else
            {
                ShowHUD();
            }
        }

        private void SetHUD(BooleanValue show)
        {
            if (show.Value == true)
            {
                ShowHUD();
            }
            else
            {
                HideHUD();
            }
        }
    }

    [KOSNomenclature("WPKSPTerminal")]
    public class WPKSPTerminal : TerminalStruct
    {
        new private readonly SharedObjects shared;
        public WPKSPTerminal(SharedObjects shared) : base(shared)
        {
            this.shared = shared;
            this.InitializeSuffixes();
        }

        new private void InitializeSuffixes()
        {
            base.AddSuffix("X", new SetSuffix<ScalarDoubleValue>(new SuffixGetDlg<ScalarDoubleValue>(GetX), new SuffixSetDlg<ScalarDoubleValue>(SetX)));
            base.AddSuffix("Y", new SetSuffix<ScalarDoubleValue>(new SuffixGetDlg<ScalarDoubleValue>(GetY), new SuffixSetDlg<ScalarDoubleValue>(SetY)));
            base.AddSuffix("TITLE", new Suffix<StringValue>(GetTitle));

            base.AddSuffix("CURSOR", new SetSuffix<BooleanValue>(new SuffixGetDlg<BooleanValue>(GetCursor), new SuffixSetDlg<BooleanValue>(SetCursor)));
            base.AddSuffix("SCROLL", new OneArgsSuffix<ScalarIntValue>(DoScroll));
            base.AddSuffix("FOCUS", new NoArgsVoidSuffix(DoFocus));
        }

        protected internal SharedObjects WPKSPShared
        {
            get
            {
                return shared;
            }
        }

        private ScalarDoubleValue GetX()
        {
            return new ScalarDoubleValue(this.WPKSPShared.Window.GetRect().x);
        }

        private void SetX(ScalarDoubleValue x)
        {
            this.WPKSPShared.Window.windowRect.x = Convert.ToSingle(x.Value);
        }

        private ScalarDoubleValue GetY()
        {
            return new ScalarDoubleValue(this.WPKSPShared.Window.GetRect().y);
        }

        private void SetY(ScalarDoubleValue y)
        {
            this.WPKSPShared.Window.windowRect.y = Convert.ToSingle(y.Value);
        }

        private StringValue GetTitle()
        {
            return this.WPKSPShared.Window.TitleText;
        }

        private BooleanValue GetCursor()
        {
            return new BooleanValue(this.WPKSPShared.Window.ShowCursor);
        }

        private void SetCursor(BooleanValue show)
        {
            this.WPKSPShared.Window.ShowCursor = show.Value;
        }

        private void DoScroll(ScalarIntValue rows)
        {
            if (rows.IsInt)
            {
                WPKSPShared.Screen.ScrollVertical((int)rows.Value);
            }
            else
            {
                throw new kOS.Safe.Exceptions.KOSException("Expected an integer");
            }
        }

        private void DoFocus()
        {
            WPKSPShared.Window.GetFocus();
        }
    }
}

namespace kOS.Binding
{
    [Binding("ksp")]
    public class WPKSPTerminalSettings : Binding
    {
        private WPKSPTerminal terminalInstance = null;

        public override void AddTo(kOS.SharedObjects shared)
        {
            shared.BindingMgr.AddGetter("TERMINAL", () => terminalInstance ?? (terminalInstance = new WPKSPTerminal(shared)));
        }
    }
}