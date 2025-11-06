using BepInEx;
using BepInEx.Logging;
using UnityEngine;

namespace UltraLib;

#pragma warning disable BepInEx001
public abstract class PluginBase : BaseUnityPlugin
#pragma warning restore BepInEx001
{
    internal new static ManualLogSource Logger { get; private set; } = null!;
    protected void Awake()
    {
        Logger = base.Logger;
        gameObject.hideFlags = HideFlags.DontSaveInEditor;
    }
}