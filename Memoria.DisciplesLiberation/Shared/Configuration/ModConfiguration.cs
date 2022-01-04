using System;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace Memoria.Disciples.Configuration
{
    public sealed class ModConfiguration
    {
        public SpeedConfiguration Speed { get; }
        public CompanionsConfiguration Companions { get; }

        public ModConfiguration()
        {
            using (var log = Logger.CreateLogSource("Memoria Config"))
            {
                try
                {
                    log.LogInfo($"Initializing {nameof(ModConfiguration)}");

                    ConfigFileProvider provider = new();
                    Speed = new SpeedConfiguration(provider);
                    Companions = new CompanionsConfiguration(provider);

                    log.LogInfo($"{nameof(ModConfiguration)} initialized successfully.");
                }
                catch (Exception ex)
                {
                    log.LogError($"Failed to initialize {nameof(ModConfiguration)}: {ex}");
                    throw;
                }
            }
        }
    }
}