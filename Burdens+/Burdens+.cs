using MonoMod.RuntimeDetour;
using RL2.ModLoader;
using System;
using System.Reflection;

namespace BurdensPlus;

[ModEntrypoint]
public class BurdensPlus
{
	public BurdensPlus() {
		ModLoader.OnLoad += GetBurdenDataHook.Apply;
		ModLoader.OnUnload += GetBurdenDataHook.Undo;
	}

	public Hook GetBurdenDataHook = new Hook(
		typeof(BurdenLibrary).GetMethod("GetBurdenData", BindingFlags.Public | BindingFlags.Static),
		(Func<BurdenType, BurdenData> orig, BurdenType type) => {
			BurdenData burdenData = orig(type);
			burdenData.MaxBurdenLevel = 99;
			burdenData.Disabled = false;
			return burdenData;
		}
    );
}
