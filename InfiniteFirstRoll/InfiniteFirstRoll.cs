using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RL2.ModLoader;
using SceneManagement_RL;
using System;
using System.Reflection;
using UnityEngine;

namespace InfiniteFirstRoll;

[ModEntrypoint]
public class InfiniteFirstRoll
{
	public InfiniteFirstRoll() {
		ModLoader.OnLoad += () => {
			SkipTutorialCutscene_ILHook.Apply();
			ResetNewGameFlag_Hook.Apply();
			GiveInfiniteRerolls_Hook.Apply();
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, PlayerExitRoom);
		};

		ModLoader.OnUnload += () => {
			SkipTutorialCutscene_ILHook.Undo();
			ResetNewGameFlag_Hook.Undo();
			GiveInfiniteRerolls_Hook.Undo();
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, PlayerExitRoom);
		};
	}

	public static bool NewGame = false;

	public ILHook SkipTutorialCutscene_ILHook = new ILHook(
		typeof(MainMenuWindowController).GetMethod("LoadGame", BindingFlags.Public | BindingFlags.Instance),
		(ILContext il) => {
			ILCursor cursor = new ILCursor(il);

			cursor.GotoNext(
				MoveType.Before,
				i => i.MatchLdarg(0), i => i.MatchLdcI4(1)
			);

			cursor.EmitDelegate(( ) => {
				InfiniteFirstRoll.NewGame = true;
			});
		}
	);

	public Hook ResetNewGameFlag_Hook = new Hook(
		typeof(LineageWindowController).GetMethod("ConfirmHeirSelection", BindingFlags.NonPublic | BindingFlags.Instance),
		(Action<LineageWindowController> orig, LineageWindowController self) => {
			orig(self);
			if (InfiniteFirstRoll.NewGame) {
				InfiniteFirstRoll.NewGame = false;
			}
		}
	);

	public Hook GiveInfiniteRerolls_Hook = new Hook(
		typeof(LineageWindowController).GetMethod("UpdateRerollHeirsNav", BindingFlags.NonPublic | BindingFlags.Instance),
		(Action<LineageWindowController> orig, LineageWindowController self) => {
			if (InfiniteFirstRoll.NewGame) {
				typeof(LineageWindowController).GetField("m_numRerolls", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(self, 5);
				RNGManager.Reset();
			}
			orig(self);
		}
	);

	public void PlayerExitRoom(MonoBehaviour sender, EventArgs args) {
		InfiniteFirstRoll.NewGame = false;
	}

	[Command("ifr:debug")]
	public static void DEBUG(string[] args) {
		ModLoader.Log(InfiniteFirstRoll.NewGame);
	}
}
