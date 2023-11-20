using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using CactusPie.CustomContainerPriority.Data;
using EFT.InventoryLogic;

namespace CactusPie.CustomContainerPriority.Patches
{
    public class CustomContainerPriorityPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            MethodInfo method = typeof(GClass2556).GetMethod("GetPrioritizedGridsForLoot", BindingFlags.Public | BindingFlags.Static);
            return method;
        }

        [PatchPrefix]
        public static bool PatchPostfix(
            ref IEnumerable<GClass2318> __result,
            object __instance,
            EquipmentClass equipment, 
            Item item)
        {
            if (!CustomContainerPriorityPlugin.CustomLootPriority.Value)
            {
                return true;
            }
            
            Slot slot = equipment.GetSlot(EquipmentSlot.TacticalVest);
            Slot slot2 = equipment.GetSlot(EquipmentSlot.Backpack);
            Slot slot3 = equipment.GetSlot(EquipmentSlot.Pockets);
            Slot slot4 = equipment.GetSlot(EquipmentSlot.SecuredContainer);
            GClass2497 gClass = slot.ContainedItem as GClass2497;
            GClass2496 gClass2 = slot2.ContainedItem as GClass2496;
            GClass2495 gClass3 = slot3.ContainedItem as GClass2495;
            ItemContainerClass itemContainerClass = slot4.ContainedItem as ItemContainerClass;
            GClass2318[] array = gClass != null ? gClass.Grids : Array.Empty<GClass2318>();
            GClass2318[] array2 = gClass2 != null ? gClass2.Grids : Array.Empty<GClass2318>();
            GClass2318[] array3 = gClass3 != null ? gClass3.Grids : Array.Empty<GClass2318>();
            GClass2318[] array4 = itemContainerClass != null ? itemContainerClass.Grids : Array.Empty<GClass2318>();

            __result = new ArrayPriority[4]
                {
                    new ArrayPriority(array, CustomContainerPriorityPlugin.VestPriority.Value),
                    new ArrayPriority(array2, CustomContainerPriorityPlugin.BackpackPriority.Value),
                    new ArrayPriority(array3, CustomContainerPriorityPlugin.PocketsPriority.Value),
                    new ArrayPriority(array4, CustomContainerPriorityPlugin.SecureContainerPriority.Value),
                }
                .OrderBy(x => x.Priority)
                .SelectMany(x => x.Array);

            return false;
        }
    }
}