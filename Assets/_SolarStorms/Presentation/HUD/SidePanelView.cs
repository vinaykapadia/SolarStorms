using System.Collections.Generic;
using SolarStorms.Gameplay;
using SolarStorms.Simulation;
using UnityEngine;
using UnityEngine.UI;

namespace SolarStorms.Presentation
{
    /// <summary>
    /// Placeholder side panel: four reactor bars, in the design doc's order (Weapons, Engines, Sensors, Shields),
    /// and a heat bar that flashes with an OVERHEAT label during a lockout.
    /// Built in code at startup, fitted over the side-panel area each frame, and updated only from reactor events.
    /// </summary>
    public sealed class SidePanelView : MonoBehaviour
    {
        static readonly ReactorSystem[] PanelOrder =
        {
            ReactorSystem.Weapons, ReactorSystem.Engines, ReactorSystem.Sensors, ReactorSystem.Shields,
        };

        static readonly Dictionary<ReactorSystem, string> ShortNames = new Dictionary<ReactorSystem, string>
        {
            { ReactorSystem.Weapons, "WPN" },
            { ReactorSystem.Engines, "ENG" },
            { ReactorSystem.Sensors, "SNS" },
            { ReactorSystem.Shields, "SHD" },
        };

        [SerializeField] CameraRigData layout;
        [SerializeField] ShipReactor reactor;
        [SerializeField] ReactorFeedbackData feedback;

        readonly Dictionary<ReactorSystem, RectTransform> barFills = new Dictionary<ReactorSystem, RectTransform>();
        readonly Dictionary<ReactorSystem, Text> barValues = new Dictionary<ReactorSystem, Text>();

        Font font;
        RectTransform panel;
        RectTransform heatFill;
        Image heatFillImage;
        Text heatValue;
        Text overheatLabel;
        bool lockedOut;

        void Awake()
        {
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            Build();
        }

        void OnEnable()
        {
            ReactorModel model = reactor.Model;
            model.PowerChanged += OnPowerChanged;
            model.HeatChanged += OnHeatChanged;
            model.OverheatStarted += OnOverheatStarted;
            model.OverheatEnded += OnOverheatEnded;
        }

        void OnDisable()
        {
            ReactorModel model = reactor.Model;
            model.PowerChanged -= OnPowerChanged;
            model.HeatChanged -= OnHeatChanged;
            model.OverheatStarted -= OnOverheatStarted;
            model.OverheatEnded -= OnOverheatEnded;
        }

        void Start()
        {
            ShowPower();
            ShowHeat(reactor.Model.Heat);
            lockedOut = reactor.Model.IsLockedOut;
        }

        void LateUpdate()
        {
            Rect area = layout.PanelRect((float)Screen.width / Screen.height);
            panel.anchorMin = area.min;
            panel.anchorMax = area.max;

            // Flash while locked out; the lockout state itself only changes from events.
            float flash = Mathf.PingPong(Time.unscaledTime * feedback.overheatFlashRate * 2f, 1f);
            heatFillImage.color = lockedOut ? Color.Lerp(feedback.heatColor, feedback.overheatColor, flash) : feedback.heatColor;
            overheatLabel.enabled = lockedOut && flash > 0.3f;
        }

        void OnPowerChanged(PowerChange change) => ShowPower();
        void OnHeatChanged(float heat) => ShowHeat(heat);
        void OnOverheatStarted() => lockedOut = true;
        void OnOverheatEnded() => lockedOut = false;

        void ShowPower()
        {
            ReactorModel model = reactor.Model;
            foreach (ReactorSystem system in PanelOrder)
            {
                int level = model.Level(system);
                barFills[system].anchorMax = new Vector2(1f, (float)level / model.Data.capLevel);
                barValues[system].text = level.ToString();
            }
        }

        void ShowHeat(float heat)
        {
            heatFill.anchorMax = new Vector2(heat / reactor.Model.Data.maxHeat, 1f);
            heatValue.text = Mathf.CeilToInt(heat).ToString();
        }

        void Build()
        {
            var canvasObject = new GameObject("Side Panel Canvas", typeof(Canvas), typeof(CanvasScaler));
            canvasObject.transform.SetParent(transform, worldPositionStays: false);
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 1f;

            panel = CreateRect(canvasObject.transform, "Panel", Vector2.zero, Vector2.one, feedback.panelBackgroundColor);
            CreateText(panel, "Title", "REACTOR", new Vector2(0.05f, 0.9f), new Vector2(0.95f, 0.97f), TextAnchor.MiddleCenter);

            ReactorData data = reactor.Model.Data;
            float standardMark = (float)data.standardLevel / data.capLevel;
            for (int i = 0; i < PanelOrder.Length; i++)
            {
                ReactorSystem system = PanelOrder[i];
                float left = 0.08f + i * 0.22f;
                float right = left + 0.16f;

                barValues[system] = CreateText(panel, $"{system} Value", "", new Vector2(left, 0.855f), new Vector2(right, 0.895f), TextAnchor.MiddleCenter);
                RectTransform bar = CreateRect(panel, $"{system} Bar", new Vector2(left, 0.4f), new Vector2(right, 0.85f), feedback.barBackgroundColor);
                barFills[system] = CreateRect(bar, "Fill", Vector2.zero, Vector2.one, feedback.ColorFor(system));
                RectTransform standardLine = CreateRect(bar, "Standard Mark", new Vector2(0f, standardMark), new Vector2(1f, standardMark), feedback.panelTextColor);
                standardLine.sizeDelta = new Vector2(0f, 2f);
                CreateText(panel, $"{system} Label", ShortNames[system], new Vector2(left - 0.03f, 0.33f), new Vector2(right + 0.03f, 0.39f), TextAnchor.MiddleCenter);
            }

            CreateText(panel, "Heat Label", "HEAT", new Vector2(0.08f, 0.25f), new Vector2(0.5f, 0.3f), TextAnchor.MiddleLeft);
            heatValue = CreateText(panel, "Heat Value", "", new Vector2(0.5f, 0.25f), new Vector2(0.92f, 0.3f), TextAnchor.MiddleRight);
            RectTransform heatBar = CreateRect(panel, "Heat Bar", new Vector2(0.08f, 0.19f), new Vector2(0.92f, 0.245f), feedback.barBackgroundColor);
            heatFill = CreateRect(heatBar, "Fill", Vector2.zero, Vector2.one, feedback.heatColor);
            heatFillImage = heatFill.GetComponent<Image>();
            float releaseMark = data.lockoutReleaseHeat / data.maxHeat;
            RectTransform releaseLine = CreateRect(heatBar, "Release Mark", new Vector2(releaseMark, 0f), new Vector2(releaseMark, 1f), feedback.panelTextColor);
            releaseLine.sizeDelta = new Vector2(2f, 0f);

            overheatLabel = CreateText(panel, "Overheat Label", "OVERHEAT", new Vector2(0.05f, 0.08f), new Vector2(0.95f, 0.16f), TextAnchor.MiddleCenter);
            overheatLabel.color = feedback.overheatColor;
            overheatLabel.enabled = false;
        }

        static RectTransform CreateRect(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Color? color = null)
        {
            var rect = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
            rect.SetParent(parent, worldPositionStays: false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            if (color.HasValue)
            {
                var image = rect.gameObject.AddComponent<Image>();
                image.color = color.Value;
                image.raycastTarget = false;
            }

            return rect;
        }

        Text CreateText(Transform parent, string name, string text, Vector2 anchorMin, Vector2 anchorMax, TextAnchor alignment)
        {
            RectTransform rect = CreateRect(parent, name, anchorMin, anchorMax);
            var label = rect.gameObject.AddComponent<Text>();
            label.font = font;
            label.text = text;
            label.alignment = alignment;
            label.color = feedback.panelTextColor;
            label.resizeTextForBestFit = true;
            label.resizeTextMinSize = 8;
            label.resizeTextMaxSize = 40;
            label.raycastTarget = false;
            return label;
        }
    }
}
