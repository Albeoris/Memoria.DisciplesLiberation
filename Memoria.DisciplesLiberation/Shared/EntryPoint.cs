﻿using System;
using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Memoria.Disciples.Core;

namespace Memoria.Disciples
{
    [BepInPlugin(ModConstants.Id, "Memoria Disciples", "1.0.0.0")]
    public class EntryPoint : BasePlugin
    {
        public override void Load()
        {
            try
            {
                Log.LogMessage("Initializing...");

                TypeRegister typeRegister = new(Log);
                typeRegister.RegisterRequiredTypes();
                typeRegister.RegisterAssemblyTypes();

                SingletonInitializer singletonInitializer = new(Log);
                singletonInitializer.InitializeInGameSingleton();

                PatchMethods();
                Log.LogMessage("The mod has been successfully initialized.");
            }
            catch (Exception ex)
            {
                Log.LogError($"Failed to initialize the mod: {ex}");
                throw;
            }
        }

        private void PatchMethods()
        {
            try
            {
                Log.LogInfo("Patching methods...");
                Harmony harmony = new Harmony(ModConstants.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to patch methods.", ex);
            }
        }
    }
}