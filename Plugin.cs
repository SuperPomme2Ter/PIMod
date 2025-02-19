using BepInEx;
using Alexandria;
using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PIWeapon;
using System.Collections;
using Gungeon;
using MonoMod;

namespace PIWeapon
{
    [BepInDependency(Alexandria.Alexandria.GUID)] // this mod depends on the Alexandria API: https://enter-the-gungeon.thunderstore.io/package/Alexandria/Alexandria/
    [BepInDependency(ETGModMainBehaviour.GUID)]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "sp2t.etg.piweapon";
        public const string NAME = "PIWeapon";
        public const string VERSION = "1.0.2";
        public const string TEXT_COLOR = "#00FFFF";

        public void Start()
        {
            ETGModMainBehaviour.WaitForGameManagerStart(new Action<GameManager>(GMStart));
           
        }

        public void GMStart(GameManager g)
        {
            PI.Add();
            Log($"{NAME} v{VERSION} started successfully.", TEXT_COLOR);
            
        }

        public static void Log(string text, string color="#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }
    }
}
