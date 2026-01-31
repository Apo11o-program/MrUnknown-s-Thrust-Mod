using SFS.UI.ModGUI;
using SFS.World;
using SFS.Parts.Modules;
using UnityEngine; 
using SFS.WorldBase;
using SFS.UI; 
using System.Collections.Generic;

namespace UnknownMod
{
    public class SetThrust : MonoBehaviour
    {
        private Window window;
        private GameObject holder;
        
        private Container thrustPage;
        private Container ispPage;

        private string thrustText = "1";
        private string ispInputText = "1";
        private float currentFuelMultiplier = 1.0f;
        
        private Label manualPercentagePreview;
        private Label stepperPercentageLabel;

        private Dictionary<EngineModule, float> originalISPs = new Dictionary<EngineModule, float>();
        private Rocket currentRocket;

        private void Start()
        {

        }

        private void Update()
        {
            if (PlayerController.main == null) return;

            if (holder == null)
            {
                InitializeUI();
            }
        }

        private void InitializeUI()
        {
            holder = Builder.CreateHolder(Builder.SceneToAttach.CurrentScene, "ThrustMod Holder");

            window = Builder.CreateWindow(
                holder.transform, 
                Builder.GetRandomID(), 
                380, 350, 
                200, 200, 
                true, true, 0.95f,      
                "Thrust Control"
            );

            CaptureOriginalStats();

            window.CreateLayoutGroup(Type.Vertical, TextAnchor.UpperCenter, 5f);
            Container tabButtons = Builder.CreateContainer(window);
            tabButtons.CreateLayoutGroup(Type.Horizontal, TextAnchor.MiddleCenter, 5f);
            Builder.CreateButton(tabButtons, 150, 40, 0, 0, () => OpenTab(0), "Thrust");
            Builder.CreateButton(tabButtons, 150, 40, 0, 0, () => OpenTab(1), "Fuel Usage");
            Builder.CreateSeparator(window, 360);

            thrustPage = Builder.CreateContainer(window);
            thrustPage.CreateLayoutGroup(Type.Vertical, TextAnchor.UpperCenter, 10f);
            
            Builder.CreateLabel(thrustPage, 300, 30, 0, 0, "Thrust Multiplier");
            Container thrustInputBox = Builder.CreateContainer(thrustPage);
            thrustInputBox.CreateLayoutGroup(Type.Horizontal, TextAnchor.MiddleCenter, 10f);
            Builder.CreateTextInput(thrustInputBox, 100, 50, 0, 0, thrustText, (value) => thrustText = value); // Use variable
            Builder.CreateButton(thrustInputBox, 120, 50, 0, 0, () => ApplyThrust(), "Set Thrust");

            Container thrustPresets = Builder.CreateContainer(thrustPage);
            thrustPresets.CreateLayoutGroup(Type.Horizontal, TextAnchor.MiddleCenter, 5f);
            Builder.CreateButton(thrustPresets, 90, 40, 0, 0, () => SetThrottle(1f), "x1");
            Builder.CreateButton(thrustPresets, 90, 40, 0, 0, () => SetThrottle(5f), "x5");
            Builder.CreateButton(thrustPresets, 90, 40, 0, 0, () => SetThrottle(10f), "x10");

            ispPage = Builder.CreateContainer(window);
            ispPage.CreateLayoutGroup(Type.Vertical, TextAnchor.UpperCenter, 10f);

            Builder.CreateLabel(ispPage, 360, 30, 0, 0, "Manual Entry");

            Container manualRow = Builder.CreateContainer(ispPage);
            manualRow.CreateLayoutGroup(Type.Horizontal, TextAnchor.MiddleCenter, 5f);
            
            Builder.CreateTextInput(manualRow, 90, 45, 0, 0, ispInputText, (value) => UpdatePreviewLabel(value));
            
            manualPercentagePreview = Builder.CreateLabel(manualRow, 80, 45, 0, 0, (currentFuelMultiplier * 100f).ToString("F0") + "%");
            
            Builder.CreateButton(manualRow, 100, 45, 0, 0, () => ApplyFromText(), "Apply");

            Builder.CreateSpace(ispPage, 0, 10);
            Builder.CreateLabel(ispPage, 360, 30, 0, 0, "Fine Tuning");

            Container stepperRow = Builder.CreateContainer(ispPage);
            stepperRow.CreateLayoutGroup(Type.Horizontal, TextAnchor.MiddleCenter, 2f);

            Builder.CreateButton(stepperRow, 35, 35, 0, 0, () => StepFuel(-1.0f), "<<<");
            Builder.CreateButton(stepperRow, 30, 35, 0, 0, () => StepFuel(-0.1f), "<<");
            Builder.CreateButton(stepperRow, 25, 35, 0, 0, () => StepFuel(-0.01f), "<");

            stepperPercentageLabel = Builder.CreateLabel(stepperRow, 80, 35, 0, 0, (currentFuelMultiplier * 100f).ToString("F0") + "%");

            Builder.CreateButton(stepperRow, 25, 35, 0, 0, () => StepFuel(0.01f), ">");
            Builder.CreateButton(stepperRow, 30, 35, 0, 0, () => StepFuel(0.1f), ">>");
            Builder.CreateButton(stepperRow, 35, 35, 0, 0, () => StepFuel(1.0f), ">>>");

            OpenTab(0);
        }

        public void UpdatePreviewLabel(string text)
        {
            ispInputText = text;
            if (float.TryParse(text, out float val))
            {
                float percentage = val * 100f;
                manualPercentagePreview.Text = percentage.ToString("F0") + "%";
            }
            else
            {
                manualPercentagePreview.Text = "---";
            }
        }

        public void ApplyFromText()
        {
            if (float.TryParse(ispInputText, out float val))
            {
                ForceSetFuel(val);
            }
        }

        public void StepFuel(float amount)
        {
            currentFuelMultiplier += amount;
            currentFuelMultiplier = (float)System.Math.Round(currentFuelMultiplier, 2);
            if (currentFuelMultiplier < 0) currentFuelMultiplier = 0; 
            
            ForceSetFuel(currentFuelMultiplier);
        }

        public void ForceSetFuel(float val)
        {
            currentFuelMultiplier = val;
            
            stepperPercentageLabel.Text = (val * 100f).ToString("F0") + "%";
            manualPercentagePreview.Text = (val * 100f).ToString("F0") + "%";
            
            ApplyFuelMultiplierLogic();
        }

        private void CaptureOriginalStats()
        {
            if (PlayerController.main == null || PlayerController.main.player.Value == null) return;

            if (PlayerController.main.player.Value is Rocket rocket)
            {
                currentRocket = rocket;
                originalISPs.Clear();
                foreach (var part in rocket.partHolder.parts)
                {
                    var engine = part.GetComponent<EngineModule>();
                    if (engine != null && !originalISPs.ContainsKey(engine))
                    {
                        originalISPs.Add(engine, engine.ISP.Value);
                    }
                }
            }
        }

        public void ApplyFuelMultiplierLogic()
        {
            if (PlayerController.main == null || PlayerController.main.player.Value == null) return;

            if (PlayerController.main.player.Value is Rocket rocket && rocket != currentRocket)
                CaptureOriginalStats();
            
            foreach (var kvp in originalISPs)
            {
                EngineModule engine = kvp.Key;
                float originalISP = kvp.Value;

                if (engine != null)
                {
                    if (currentFuelMultiplier <= 0.001f)
                    {
                        engine.ISP.Value = 1000000f; // Infinite
                    }
                    else
                    {
                        engine.ISP.Value = originalISP / currentFuelMultiplier;
                    }
                }
            }
            MsgDrawer.main.Log("Fuel Rate: " + (currentFuelMultiplier * 100).ToString("F0") + "%");
        }

        public void OpenTab(int id)
        {
            thrustPage.gameObject.SetActive(id == 0);
            ispPage.gameObject.SetActive(id == 1);
            window.gameObject.SetActive(false); 
            window.gameObject.SetActive(true);
        }

        public void ApplyThrust()
        {
            if (float.TryParse(thrustText, out float val)) SetThrottle(val);
        }

        public void SetThrottle(float value)
        {
            if (PlayerController.main == null || PlayerController.main.player.Value == null) return;

            if (PlayerController.main.player.Value is Rocket rocket)
            {
                rocket.throttle.throttlePercent.Value = value;
                MsgDrawer.main.Log("Thrust: " + value + "x");
            }
        }

        private void OnDestroy()
        {
            if (holder != null) Object.Destroy(holder);
        }
    }
}
