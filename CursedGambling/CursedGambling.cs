using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RL2.API;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CursedGambling;

public class CursedGambling : Mod
{
	public static readonly List<string> NoGoldMessages = ["Nuh uh!", "YOU FOOL!", "Cursed! No gold for you!"];

	public Hook DontAllowGoldHook = new Hook(
		typeof(Economy_EV).GetMethod("GetGoldGainMod", BindingFlags.Public | BindingFlags.Static),
		(Func<float> orig) => {
			return Math.Min(orig(), -1f);
		}
	);

	public Hook GamblerGivesNoGoldHook = new Hook(
		typeof(ItemDropManager).GetMethod("Internal_DropGold", BindingFlags.NonPublic | BindingFlags.Instance),
		(Action<ItemDropManager, int, UnityEngine.Vector3, bool, bool, bool> orig, ItemDropManager self, int amount, UnityEngine.Vector3 position, bool largeSpurt, bool forceMagnetize, bool fromChest) => { 
			if (!TraitManager.IsTraitActive(TraitType.BonusChestGold) || !fromChest) {
				orig(self, amount, position, largeSpurt, forceMagnetize, fromChest);
				return;
			}

			int chosenLine = UnityEngine.Random.Range(0, NoGoldMessages.Count);
			TextPopupManager.DisplayTextDefaultPos(TextPopupType.GoldCollected, NoGoldMessages[chosenLine], PlayerManager.GetPlayerController(), attachToSource: true);
		}
	);

	public Hook SirLeeIsAGamblingAddict = new Hook(
		typeof(TutorialRoomController).GetMethod("CreateStartingCharacter", BindingFlags.NonPublic | BindingFlags.Instance),
		(Action<TutorialRoomController> orig, TutorialRoomController self) => {
			orig(self);
			SaveManager.PlayerSaveData.CurrentCharacter.TraitOne = TraitType.BonusChestGold;
			typeof(TraitManager).GetMethod("InstantiateTrait", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(TraitManager.Instance, [TraitType.BonusChestGold, true]);
		}
	);

	public override void OnLoad() {
		DontAllowGoldHook.Apply();
		GamblerGivesNoGoldHook.Apply();
		SirLeeIsAGamblingAddict.Apply();
	}

	public override void OnUnload() {
		DontAllowGoldHook.Undo();
		GamblerGivesNoGoldHook.Undo();
		SirLeeIsAGamblingAddict.Undo();
	}
}