using System;
using System.Collections.Generic;
using HarmonyLib;
using I2.Loc;
using Memoria.Disciples.Configuration;
using UnhollowerBaseLib;

namespace Memoria.Disciples.IL2CPP;

[HarmonyPatch(typeof(DataCentralManager), nameof(DataCentralManager.LoadDataCentral))]
public sealed class MasterMap_Initialize : Il2CppSystem.Object
{
    public MasterMap_Initialize(IntPtr ptr) : base(ptr)
    {
        ModComponent.Log.LogInfo($"[MasterMap_Initialize] Ctor");
    }

    public static void Postfix()
    {
        try
        {
            var backline = ModComponent.Instance.Config.Companions.Backline;
            if (!backline.CompanionsCanStandInBackline)
                return;

            BacklineAbilityId ability = backline.GetBacklineAbility(CompanionUnitName.Orion);
            if (ability != BacklineAbilityId.None)
            {
                // Орион: Слияние с тенью (Become the Shadows)
                GiveBacklineAbility(0x613665F4B3CA7E28, ability);
                GiveBacklineAbility(0x0E7E152B85BA0BD1, ability);
            }
            
            ability = backline.GetBacklineAbility(CompanionUnitName.Ejamar);
            if (ability != BacklineAbilityId.None)
            {
                // Эйямар: Сила мертвецов
                GiveBacklineAbility(0x2C8C25AC94AAB90B, ability);
                GiveBacklineAbility(0x4D09BAA41732CE2C, ability);
            }
            
            ability = backline.GetBacklineAbility(CompanionUnitName.Ormeriel);
            if (ability != BacklineAbilityId.None)
            {
                // Ормериэль: Славное выступление (Glorious Display)
                GiveBacklineAbility(0x78782AF2F90367FB, ability);
                GiveBacklineAbility(0x0DFC1C9F994A620A, ability);
            }
            
            ability = backline.GetBacklineAbility(CompanionUnitName.Bagthal);
            if (ability != BacklineAbilityId.None)
            {
                // Баталь: Испепеление (Immolate)
                GiveBacklineAbility(0x03C09B6FA5D250C5, ability);
                GiveBacklineAbility(0x59E5C2E5C816C9BD, ability);
                GiveBacklineAbility(0x406E0F9B08DA5D81, ability);
            }
            
            ability = backline.GetBacklineAbility(CompanionUnitName.Corisandre);
            if (ability != BacklineAbilityId.None)
            {
                // Корисандра: Исцеление Всеотца
                GiveBacklineAbility(0x18948C8060D0BF7D, ability);
                GiveBacklineAbility(0x38A16D8F0B3AAC87, ability);
            }
            
            ability = backline.GetBacklineAbility(CompanionUnitName.Sebastien);
            if (ability != BacklineAbilityId.None)
            {
                // Себастьен: Прилив маны (Mana High)
                GiveBacklineAbility(0x62981018D5DB6C1F, ability);
                GiveBacklineAbility(0x7C68F6501B88F265, ability);
                GiveBacklineAbility(0x1380BA1754E0E202, ability);
            }
            
            ability = backline.GetBacklineAbility(CompanionUnitName.Sharlea);
            if (ability != BacklineAbilityId.None)
            {
                // Шарлея: Укус вампира (Vampire Bite)
                GiveBacklineAbility(0x14DA75F1D49E7468, ability);
                GiveBacklineAbility(0x0E8B6AE1CFDC47FF, ability);
            }
            
            ability = backline.GetBacklineAbility(CompanionUnitName.Illmeren);
            if (ability != BacklineAbilityId.None)
            {
                // Ильмерен: Смертельный выстрел (Fatal Shot)
                GiveBacklineAbility(0x1CBDE3812B981AD4, ability);
                GiveBacklineAbility(0x13EEB2BE2F075F53, ability);
            }
            
            ability = backline.GetBacklineAbility(CompanionUnitName.Malendrach);
            if (ability != BacklineAbilityId.None)
            {
                // Мелендрах: Доспех Галлеана (Gallean's Armor)
                GiveBacklineAbility(0x455C00A393FF8D7E, ability);
                GiveBacklineAbility(0x6EBF28ACB23F0165, ability);
            }
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogError($"[MasterMap_Initialize] Failed to import file []: {ex}");
        }
    }

    private static void GiveBacklineAbility(Int64 characterId, BacklineAbilityId abilityId)
    {
        try
        {
            var characterGuid = new SerializableGuid(characterId);
            var abilityGuid = new SerializableGuid((Int64)abilityId);

            CharacterObjectMap abilityMap = GetAbility(abilityGuid);

            var character = DataCentralManager.MasterMap.CharacterMap.LookupDictionary[characterGuid];
            Il2CppReferenceArray<CharacterObjectMap> oldAbilities = character.Playback.Abilities;
            Il2CppReferenceArray<CharacterObjectMap> newAbilities = new Il2CppReferenceArray<CharacterObjectMap>(oldAbilities.Length + 1);
            for (int i = 0; i < oldAbilities.Length; i++)
            {
                CharacterObjectMap old = oldAbilities[i];
                if (old.Ability.Guid.Guid == abilityGuid.Guid)
                    return;
                newAbilities[i] = old;
            }

            newAbilities[newAbilities.Length - 1] = abilityMap;
            character.Playback.Abilities = newAbilities;
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogError($"[MasterMap_Initialize] Failed to give the backline ability {abilityId} to the character {characterId}");
            ModComponent.Log.LogError($"{ex}");
        }
    }

    private static CharacterObjectMap GetAbility(SerializableGuid abilityGuid)
    {
        foreach (var character in DataCentralManager.MasterMap.CharacterMap.Items)
        {
            CharacterPlaybackScriptableObject playback = character.Playback;
            if (playback is null)
                continue;

            foreach (CharacterObjectMap map in playback.Abilities)
            {
                if (map.Ability.Guid.Guid == abilityGuid.Guid)
                    return map;
            }

        }

        throw new Exception($"Cannot find abilityId: {abilityGuid.Guid}");
    }
}