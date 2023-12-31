﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CactusPie.CustomContainerPriority.Patches;
using JetBrains.Annotations;

namespace CactusPie.CustomContainerPriority
{
    [BepInPlugin("com.cactuspie.customcontainerpriority", "CactusPie.CustomContainerPriority", "1.2.1")]
    public class CustomContainerPriorityPlugin : BaseUnityPlugin
    {
        internal static ConfigEntry<bool> CustomLootPriority { get; set; }
        
        internal static ConfigEntry<bool> ReverseFillBackpack { get; set; }
        
        internal static ConfigEntry<bool> ReverseFillVest { get; set; }
        
        internal static ConfigEntry<bool> ReverseFillPockets { get; set; }
        
        internal static ConfigEntry<bool> ReverseFillSecureContainer { get; set; }
        
        internal static ConfigEntry<int> BackpackPriority { get; set; }

        internal static ConfigEntry<int> VestPriority { get; set; }
        
        internal static ConfigEntry<int> PocketsPriority { get; set; }
        
        internal static ConfigEntry<int> SecureContainerPriority { get; set; }
        
        internal static ManualLogSource PluginLogger { get; private set; }

        [UsedImplicitly]
        internal void Start()
        {
            PluginLogger = Logger;

            CustomLootPriority = Config.Bind
            (
                "Main Setting",
                "Enable custom loot priority",
                true,
                new ConfigDescription
                (
                    "Enable custom target container for loot priority"
                )
            );
            
            const string fillOrderSectionName = "Reverse fill order";

            ReverseFillBackpack = Config.Bind
            (
                fillOrderSectionName,
                "Reverse fill backpack",
                false,
                new ConfigDescription
                (
                    "Fill the backpack starting from the last cell"
                )
            );
            
            ReverseFillPockets = Config.Bind
            (
                fillOrderSectionName,
                "Reverse fill pockets",
                false,
                new ConfigDescription
                (
                    "Fill pockets starting from the last cell"
                )
            );
            
            ReverseFillSecureContainer = Config.Bind
            (
                fillOrderSectionName,
                "Reverse fill secure container",
                false,
                new ConfigDescription
                (
                    "Fill the secure container starting from the last cell"
                )
            );
            
            ReverseFillVest = Config.Bind
            (
                fillOrderSectionName,
                "Reverse fill vest",
                false,
                new ConfigDescription
                (
                    "Fill the vest starting from the last cell"
                )
            );
            
            const string sectionName = "Container priority";
            
            BackpackPriority = Config.Bind
            (
                sectionName,
                "Backpack priority",
                1,
                new ConfigDescription
                (
                    "The priority with which the backpack will be selected as a target - the lower, the sooner it's gonna be selected",
                    new AcceptableValueRange<int>(0, 20)
                )
            );
            
            VestPriority = Config.Bind
            (
                sectionName,
                "Vest priority",
                2,
                new ConfigDescription
                (
                    "The priority with which the vest will be selected as a target - the lower, the sooner it's gonna be selected",
                    new AcceptableValueRange<int>(0, 20)
                )
            );
            
            PocketsPriority = Config.Bind
            (
                sectionName,
                "Pockets priority",
                3,
                new ConfigDescription
                (
                    "The priority with which pockets will be selected as a target - the lower, the sooner it's gonna be selected",
                    new AcceptableValueRange<int>(0, 20)
                )
            );
            
            SecureContainerPriority = Config.Bind
            (
                sectionName,
                "Secure container priority",
                4,
                new ConfigDescription
                (
                    "The priority with which the secure container will be selected as a target - the lower, the sooner it's gonna be selected",
                    new AcceptableValueRange<int>(0, 20)
                )
            );
            
            new CustomContainerPriorityPatch().Enable();
            new ReverseContainerFillPatch().Enable();
        }
    }
}
