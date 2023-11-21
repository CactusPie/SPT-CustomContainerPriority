using System.Collections.Generic;
using System.Reflection;
using Aki.Reflection.Patching;
using CactusPie.CustomContainerPriority.Helpers;
using Comfort.Common;
using EFT;
using EFT.InventoryLogic;

namespace CactusPie.CustomContainerPriority.Patches
{
    public class ReverseContainerFillPatch : ModulePatch
    {
         protected override MethodBase GetTargetMethod()
        {
            MethodInfo method = typeof(GClass2318).GetMethod("method_11", BindingFlags.NonPublic | BindingFlags.Instance);
            return method;
        }

        [PatchPrefix]
        public static bool PatchPrefix(
            ref LocationInGrid __result,
            GClass2318 __instance,
            int itemMainSize,
            int itemSecondSize,
            ItemRotation rotation,
            int firstDimensionSize,
            int secondDimensionSize,
            List<int> firstDimensionSpaces,
            List<int> secondDimensionSpaces,
            bool invertDimensions = false)
        {
            if (!CustomContainerPriorityPlugin.ReverseFillBackpack.Value)
            {
                return true;
            }

            Item containerItem = __instance?.ParentItem;
            
            bool isStash = false;

            if (containerItem is GClass2496)
            {
                if (!CustomContainerPriorityPlugin.ReverseFillBackpack.Value)
                {
                    return true;
                }
            }
            else if (containerItem is GClass2497)
            {
                if (!CustomContainerPriorityPlugin.ReverseFillVest.Value)
                {
                    return true;
                }
            }
            else if (containerItem is GClass2495)
            {
                if (!CustomContainerPriorityPlugin.ReverseFillPockets.Value)
                {
                    return true;
                }
            }
            else if (containerItem is ItemContainerClass)
            {
                if (!CustomContainerPriorityPlugin.ReverseFillSecureContainer.Value)
                {
                    return true;
                }
            }
            else
            {
                isStash = containerItem is StashClass;
                if (isStash)
                {
                    if (!CustomContainerPriorityPlugin.ReverseFillStash.Value)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }

            }

            if (CustomContainerPriorityPlugin.OnlyReverseFillInRaid.Value && !isStash)
            {
                if (!GameHelper.IsInGame())
                {
                    return true;
                }
            }

            // Only inverted the loop order compared to the original code + moved one check in the second loop to line 42
            for (int i = firstDimensionSize - 1; i >= 0; i--)
            {
                for (int j = secondDimensionSize - itemSecondSize; j >= 0; j--)
                {
                    int num = (invertDimensions ? secondDimensionSpaces[j * firstDimensionSize + i] : secondDimensionSpaces[i * secondDimensionSize + j]);
                    if (num < itemSecondSize && num != -1)
                    {
                        continue;
                    }
                    bool flag = true;
                    int num2 = j;
                    while (flag && num2 < j + itemSecondSize)
                    {
                        int num3 = (invertDimensions ? firstDimensionSpaces[num2 * firstDimensionSize + i] : firstDimensionSpaces[i * secondDimensionSize + num2]);
                        flag = flag && (num3 >= itemMainSize || num3 == -1);
                        num2++;
                    }
                    if (flag)
                    {
                        if (!invertDimensions)
                        {
                            __result = new LocationInGrid(j, i, rotation);
                            return false;
                        }
                        __result = new LocationInGrid(i, j, rotation);
                        return false;
                    }
                }
            }

            __result = null;
            return false;
        }
    }
}
