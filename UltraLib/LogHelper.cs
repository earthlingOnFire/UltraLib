using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;

namespace UltraLib;

[PublicAPI]
public static class LogHelper
{
    private static readonly PropertyInfo p_BaseUnityPlugin_Logger
        = AccessTools.Property(typeof(BaseUnityPlugin), "Logger");

    private static readonly ConcurrentDictionary<MethodBase, Assembly> _methodAssemblyCache = new();
    private static readonly ConcurrentDictionary<Assembly, ManualLogSource> _logSourceCache = new();

    public static ManualLogSource Logger
    {
        get
        {
            var currentStack = new StackTrace();
            var frames = currentStack.GetFrames();
            // frame[0] = this property getter
            // frame[1] = the caller of this property
            var callerAssembly = _methodAssemblyCache.GetOrAdd(frames![1].GetMethod(),
                static mi => mi.DeclaringType?
                                .Assembly
                          ?? throw new InvalidOperationException("Could not determine caller assembly for LogHelper.Logger"));

            return _logSourceCache.GetOrAdd(callerAssembly, static a =>
            {
                var bupObj = a.GetTypes()
                              .FirstOrDefault(static t => t.IsSubclassOf(typeof(BaseUnityPlugin)));
                return (ManualLogSource)p_BaseUnityPlugin_Logger.GetValue(bupObj);
            });
        }
    }
}