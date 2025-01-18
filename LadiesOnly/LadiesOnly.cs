using MonoMod.RuntimeDetour;
using RL2.ModLoader;
using System;
using System.Reflection;

namespace LadiesOnly;

[ModEntrypoint]
public class LadiesOnly
{
	public LadiesOnly() {
		ModLoader.OnLoad += HeirIsAlwaysFemale_Hook.Apply;
		ModLoader.OnUnload += HeirIsAlwaysFemale_Hook.Undo;
    }

	public Hook HeirIsAlwaysFemale_Hook = new Hook(
		typeof(CharacterCreator).GetMethod("GetRandomGender", BindingFlags.Public | BindingFlags.Static),
		(Func<bool, bool> orig, bool useUnityRandom) => true,
		new HookConfig() {
			ID = "LadiesOnly::HeirIsAlwaysFemale",
			ManualApply = true
		}
	);
}
