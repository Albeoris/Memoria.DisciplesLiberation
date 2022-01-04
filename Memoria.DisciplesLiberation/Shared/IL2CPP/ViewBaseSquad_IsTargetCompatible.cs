using System;
using HarmonyLib;

namespace Memoria.Disciples.IL2CPP;

[HarmonyPatch(typeof(ViewBaseSquad), nameof(ViewBaseSquad.IsTargetCompatible))]
public sealed class ViewBaseSquad_IsTargetCompatible : Il2CppSystem.Object
{
    public ViewBaseSquad_IsTargetCompatible(IntPtr ptr) : base(ptr)
    {
        ModComponent.Log.LogInfo($"[ViewBaseSquad_IsTargetCompatible] Ctor");
    }

    public static void Postfix(ViewBaseSquad __instance, SquadPlacementCursorViewItem cursorViewItem, SquadPlacementViewItem targetViewItem, ref bool __result)
    {
        if (__result)
            return;
        
        if (!targetViewItem.IsBackline)
            return;
            
        ICharacterUnit characterUnit = cursorViewItem.PlayerUnit.Cast<ICharacterUnit>();
        CharacterScriptableObject character = DataMapUtil.GetCharacter(characterUnit.CharacterGuid.Guid);
        if (character.Rank != ECharacterRank.Companion)
            return;

        foreach (var ability in character.Playback.Abilities)
        {
            if (ability.AbilityType == EAbilitySlot.Backline)
            {
                __result = true;
                return;
            }
        }
    }
}