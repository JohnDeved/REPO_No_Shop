using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
namespace REPO_No_Shop;


[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("REPO.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private Harmony harmony;

    // Static instance for script access
    public static Plugin Instance { get; private set; }

    private void Awake()
    {
        // Set instance for script access
        Instance = this;

        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        // Initialize and apply Harmony patches
        harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll(typeof(Plugin));

        Logger.LogInfo("Harmony patches applied!");
    }

    [HarmonyPatch(typeof(RunManager), nameof(RunManager.ChangeLevel))]
    [HarmonyPostfix]
    public static void ChangeLevelPostfix(RunManager __instance)
    {
        if (__instance.levelCurrent == __instance.levelShop) {
            Logger.LogInfo("Skipping shop level");
            __instance.levelCurrent = __instance.levelLobby;
        }
    }
}
