using System;
using System.Collections.Generic;
using BepInEx.Configuration;

namespace Memoria.Disciples.Configuration;

public sealed class CompanionsConfiguration
{
    public LimitsConfiguration Limits { get; }
    public BacklineConfiguration Backline { get; }

    public CompanionsConfiguration(ConfigFileProvider fileProvider)
    {
        Limits = new LimitsConfiguration(fileProvider);
        Backline = new BacklineConfiguration(fileProvider);
    }

    public sealed class LimitsConfiguration
    {
        private const String Section = "Companions.Limits";

        private readonly ConfigEntry<Boolean> _classAdvancementIncreasesCompanionLimit;
        private readonly Dictionary<object, object> _bonusCompanionLimit;

        public LimitsConfiguration(ConfigFileProvider fileProvider)
        {
            ConfigFile file = fileProvider.Get(Section);

            _classAdvancementIncreasesCompanionLimit = file.Bind(
                Section,
                key: nameof(ClassAdvancementIncreasesCompanionLimit),
                defaultValue: false,
                description: $"Allows you to specify the number of bonus companions for each class.{Environment.NewLine}Note: The value will be subtracted from the current number of companions in the squad.");

            _bonusCompanionLimit = ResolveBonuses(file);
        }

        public Boolean ClassAdvancementIncreasesCompanionLimit => _classAdvancementIncreasesCompanionLimit.Value;

        public Int32 GetBonusLimit(EHeroClass heroClass)
        {
            if (!ClassAdvancementIncreasesCompanionLimit)
                return 0;

            if (!_bonusCompanionLimit.ContainsKey(heroClass))
                return 0;

            ConfigEntry<Int32> entry = (ConfigEntry<Int32>)_bonusCompanionLimit[heroClass];
            return entry.Value;
        }

        private static Dictionary<Object, Object> ResolveBonuses(ConfigFile file)
        {
            Dictionary<object, object> result = new();

            Array values = Enum.GetValues(typeof(EHeroClass));
            foreach (EHeroClass heroClass in values)
            {
                ConfigEntry<Int32> configEntry = file.Bind(
                    Section,
                    $"{heroClass}",
                    ResolveDefaultBonusCompanionLimit(heroClass),
                    description: $"The number of bonus companions for {heroClass} class.");

                result.Add(heroClass, configEntry);
            }

            return result;
        }

        private static Int32 ResolveDefaultBonusCompanionLimit(EHeroClass heroClass)
        {
            return heroClass switch
            {
                EHeroClass.Mercenary => 0,
                EHeroClass.Warlord => 1,
                EHeroClass.Hexblade => 1,
                EHeroClass.Seer => 1,
                EHeroClass.Witch => 1,
                EHeroClass.AdvancedWarlord => 2,
                EHeroClass.AdvancedHexblade => 2,
                EHeroClass.AdvancedSeer => 2,
                EHeroClass.AdvancedWitch => 2,
                _ => 0
            };
        }

    }

    public sealed class BacklineConfiguration
    {
        private const String Section = "Companions.Backline";

        private readonly ConfigEntry<Boolean> _companionsCanStandInBackline;
        private readonly Dictionary<object, object> _nameToAbility;

        public BacklineConfiguration(ConfigFileProvider fileProvider)
        {
            ConfigFile file = fileProvider.Get(Section);

            _companionsCanStandInBackline = file.Bind(
                Section,
                key: nameof(CompanionsCanStandInBackline),
                defaultValue: false,
                description: $"You can send companions to the backline.{Environment.NewLine}Note: Specific abilities can be specified below.");

            _nameToAbility = ResolveAbilities(file);
        }

        public Boolean CompanionsCanStandInBackline => _companionsCanStandInBackline.Value;

        public BacklineAbilityId GetBacklineAbility(CompanionUnitName unitName)
        {
            if (!CompanionsCanStandInBackline)
                return BacklineAbilityId.None;

            if (!_nameToAbility.ContainsKey(unitName))
                return 0;

            ConfigEntry<BacklineAbilityId> entry = (ConfigEntry<BacklineAbilityId>)_nameToAbility[unitName];
            return entry.Value;
        }

        private Dictionary<Object, Object> ResolveAbilities(ConfigFile file)
        {
            Dictionary<object, object> result = new();

            Array companionNames = Enum.GetValues(typeof(CompanionUnitName));
            foreach (CompanionUnitName companionName in companionNames)
            {
                ConfigEntry<BacklineAbilityId> configEntry = file.Bind(
                    Section,
                    $"{companionName}",
                    ResolveDefaultCompanionBacklineAbility(companionName),
                    description: $"{companionName}'s backline ability.");

                result.Add(companionName, configEntry);
            }

            return result;
        }

        private BacklineAbilityId ResolveDefaultCompanionBacklineAbility(CompanionUnitName companionName)
        {
            return companionName switch
            {
                CompanionUnitName.Orion => BacklineAbilityId.BecomeTheShadows,
                CompanionUnitName.Ejamar => BacklineAbilityId.StrengthOfTheDead,
                CompanionUnitName.Ormeriel => BacklineAbilityId.GloriousDisplay,
                CompanionUnitName.Bagthal => BacklineAbilityId.Immolate,
                CompanionUnitName.Corisandre => BacklineAbilityId.HealOfTheHighfather,
                CompanionUnitName.Sebastien => BacklineAbilityId.ManaHigh,
                CompanionUnitName.Sharlea => BacklineAbilityId.VampireBite,
                CompanionUnitName.Illmeren => BacklineAbilityId.FatalShot,
                CompanionUnitName.Malendrach => BacklineAbilityId.GalleanArmor,
                _ => BacklineAbilityId.None
            };
        }
    }
}