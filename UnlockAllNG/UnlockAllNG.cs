using RL2.ModLoader;
using BindingFlags = System.Reflection.BindingFlags;
using Hook = MonoMod.RuntimeDetour.Hook;

namespace UnlockAllNG;

[ModEntrypoint]
public class UnlockAllNG
{
	static Hook UnlockAllNGHook = new Hook(
		typeof(NewGamePlusOmniUIEquipButton).GetMethod("GetHighestAllowedNGPlus", BindingFlags.NonPublic | BindingFlags.Instance),
		Patch
	);

	static int Patch(System.Func<NewGamePlusOmniUIEquipButton, int> orig, NewGamePlusOmniUIEquipButton self) {
		_ = orig(self);
		return int.MaxValue;
	}

	public UnlockAllNG() {
		ModLoader.OnLoad += UnlockAllNGHook.Apply;
		ModLoader.OnUnload += UnlockAllNGHook.Undo;
	}
}
