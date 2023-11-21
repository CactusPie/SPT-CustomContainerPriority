using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using CactusPie.CustomContainerPriority.Data;
using CactusPie.CustomContainerPriority.Helpers;
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
            Slot slot = equipment.GetSlot(EquipmentSlot.TacticalVest);
            Slot slot2 = equipment.GetSlot(EquipmentSlot.Backpack);
            Slot slot3 = equipment.GetSlot(EquipmentSlot.Pockets);
            Slot slot4 = equipment.GetSlot(EquipmentSlot.SecuredContainer);
            GClass2497 gClass = slot.ContainedItem as GClass2497;
            GClass2496 gClass2 = slot2.ContainedItem as GClass2496;
            GClass2495 gClass3 = slot3.ContainedItem as GClass2495;
            ItemContainerClass itemContainerClass = slot4.ContainedItem as ItemContainerClass;
            IEnumerable<GClass2318> array = gClass != null ? gClass.Grids : Array.Empty<GClass2318>();
            IEnumerable<GClass2318> array2 = gClass2 != null ? gClass2.Grids : Array.Empty<GClass2318>();
            IEnumerable<GClass2318> array3 = gClass3 != null ? gClass3.Grids : Array.Empty<GClass2318>();
            IEnumerable<GClass2318> array4 = itemContainerClass != null ? itemContainerClass.Grids : Array.Empty<GClass2318>();

            if (!CustomContainerPriorityPlugin.OnlyReverseFillInRaid.Value || GameHelper.IsInGame())
            {
                if (CustomContainerPriorityPlugin.ReverseFillBackpack.Value)
                {
                    array2 = array2.Reverse();
                }
            
                if (CustomContainerPriorityPlugin.ReverseFillPockets.Value)
                {
                    array3 = array3.Reverse();
                }
            
                if (CustomContainerPriorityPlugin.ReverseFillVest.Value)
                {
                    array = array.Reverse();
                }
            }

            if (CustomContainerPriorityPlugin.CustomLootPriority.Value)
            {
                __result = OrderUsingCustomRules(array, array2, array3, array4);
                return false;
            }
            
            __result = OrderUsingOriginalRules(item, array4, array2, array, array3);
            return false;
        }

        private static IEnumerable<GClass2318> OrderUsingCustomRules(IEnumerable<GClass2318> array, IEnumerable<GClass2318> array2, IEnumerable<GClass2318> array3, IEnumerable<GClass2318> array4)
        {
            return new ArrayPriority[4]
                {
                    new ArrayPriority(array, CustomContainerPriorityPlugin.VestPriority.Value),
                    new ArrayPriority(array2, CustomContainerPriorityPlugin.BackpackPriority.Value),
                    new ArrayPriority(array3, CustomContainerPriorityPlugin.PocketsPriority.Value),
                    new ArrayPriority(array4, CustomContainerPriorityPlugin.SecureContainerPriority.Value),
                }
                .OrderBy(x => x.Priority)
                .SelectMany(x => x.Array);
        }

        private static IEnumerable<GClass2318> OrderUsingOriginalRules(Item item, IEnumerable<GClass2318> array4, IEnumerable<GClass2318> array2, IEnumerable<GClass2318> array, IEnumerable<GClass2318> array3)
        {
            if (!(item is BulletClass) && !(item is MagazineClass))
            {
                if (item is GClass2548)
                {
                    return array4.Concat(array2).Concat(array).Concat(array3);
                }

                if (item is GrenadeClass)
                {
                    return array3.Concat(array).Concat(array2).Concat(array4);
                }

                return array2.Concat(array).Concat(array3).Concat(array4);
            }

            return array.Concat(array3).Concat(array2).Concat(array4);
        }
    }
}
