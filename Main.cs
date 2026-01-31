using HarmonyLib;
using ModLoader;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace UnknownMod
{
    public class Main : Mod
    {
        public override string ModNameID => "UnknownMod";
        public override string DisplayName => "Unknown Mod";
        public override string Author => "Mr. Unknown";
        public override string MinimumGameVersionNecessary => "1.5.10";
        public override string ModVersion => "1.2.1";
        public override string Description => "Ported old mod to new ModLoader";

        public override Dictionary<string, string> Dependencies => new Dictionary<string, string>
        {
            { "UITools", "1.1.5" }
        };

        public static Harmony patcher;
        public GameObject main;

        

        public override void Early_Load()
        {
            patcher = new Harmony("mods.Unknown.Mod");
            patcher.PatchAll();
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("[UnknownMod] Mod Loaded");
        }

        public override void Load()
        {
            main = new GameObject("Unknown");
            UnityEngine.Object.DontDestroyOnLoad(main);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "World_PC")
            {
                main.SetActive(true);

                if (main.GetComponent<SetThrust>() == null)
                {
                    main.AddComponent<SetThrust>();
                }
                
                if (main.GetComponent<CalculateOrbitalVelocity>() == null)
                {
                    main.AddComponent<CalculateOrbitalVelocity>();
                }
            }
            else
            {
                main.SetActive(false);
            }
        }
    }
}