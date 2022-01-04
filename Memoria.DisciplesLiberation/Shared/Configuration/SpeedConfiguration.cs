using System;
using BepInEx.Configuration;
using Il2CppSystem.Collections;
using KeyCode = UnityEngine.KeyCode;

namespace Memoria.Disciples.Configuration
{
    public sealed class SpeedConfiguration
    {
        private const String Section = "Speed";

        public ConfigEntry<KeyCode> ToggleKey { get; }
        public ConfigEntry<KeyCode> HoldKey { get; }
        public ConfigEntry<Single> ToggleFactor { get; }
        public ConfigEntry<Single> HoldFactor { get; }

        public SpeedConfiguration(ConfigFileProvider fileProvider)
        {
            ConfigFile file = fileProvider.Get(Section);
            
            ToggleKey = file.Bind(Section, nameof(ToggleKey), KeyCode.F1,
                $"Speed up toggle key.{Environment.NewLine}https://docs.unity3d.com/ScriptReference/KeyCode.html");

            HoldKey = file.Bind(Section, nameof(HoldKey), KeyCode.None,
                $"Speed up hold key.{Environment.NewLine}https://docs.unity3d.com/ScriptReference/KeyCode.html");

            ToggleFactor = file.Bind(Section, nameof(ToggleFactor), 3.0f,
                "Speed up toggle factor.",
                0.01f, 10.0f);

            HoldFactor = file.Bind(Section, nameof(HoldFactor), 5.0f,
                "Speed up hold factor.",
                0.01f, 10.0f);
        }
    }
}