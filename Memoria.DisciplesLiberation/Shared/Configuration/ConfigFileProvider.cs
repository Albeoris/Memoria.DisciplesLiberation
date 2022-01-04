using System;
using BepInEx;
using BepInEx.Configuration;
using Memoria.Disciples.Core;

namespace Memoria.Disciples.Configuration;

public sealed class ConfigFileProvider
{
    public ConfigFile Get(String sectionName)
    {
        String configPath = GetConfigurationPath(sectionName);
        return new ConfigFile(configPath, true, ownerMetadata: null);
    }
        
    private static String GetConfigurationPath(String sectionName)
    {
        return Utility.CombinePaths(Paths.ConfigPath, ModConstants.Id, sectionName + ".cfg");
    }
}