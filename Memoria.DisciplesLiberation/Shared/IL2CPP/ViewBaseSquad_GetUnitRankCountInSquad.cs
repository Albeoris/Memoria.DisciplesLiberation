using HarmonyLib;
using Memoria.Disciples.Configuration;
using Int32 = System.Int32;
using IntPtr = System.IntPtr;

namespace Memoria.Disciples.IL2CPP;

[HarmonyPatch(typeof(ViewBaseSquad), nameof(ViewBaseSquad.GetUnitRankCountInSquad))]
public sealed class ViewBaseSquad_GetUnitRankCountInSquad : Il2CppSystem.Object
{
    public ViewBaseSquad_GetUnitRankCountInSquad(IntPtr ptr) : base(ptr)
    {
        ModComponent.Log.LogInfo($"[ViewBaseSquad_GetUnitRankCountInSquad] Ctor");
    }

    public static void Postfix(ECharacterRank rank, ref Int32 __result)
    {
        if (rank != ECharacterRank.Companion)
            return;

        CompanionsConfiguration config = ModComponent.Instance.Config.Companions;
        if (!config.Limits.ClassAdvancementIncreasesCompanionLimit)
            return;

        Int32 bonusLimit = config.Limits.GetBonusLimit(DataCentralManager.Player.PlayerClass);
        Int32 newValue = __result - bonusLimit;

        if (newValue < 0)
            newValue = 0;

        __result = newValue;
    }
}