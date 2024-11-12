using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RL2.ModLoader;
using SceneManagement_RL;
using System;
using System.Reflection;

namespace TutorialSkip;

[ModEntrypoint]
public class TutorialSkip
{
	public TutorialSkip() {
		ModLoader.OnLoad += () => {
			SkipTutorialCutscene_ILHook.Apply();
			ResetSaveFile_Hook.Apply();
			ResetNewGameFlag_Hook.Apply();
		};

		ModLoader.OnUnload += () => {
			SkipTutorialCutscene_ILHook.Undo();
			ResetSaveFile_Hook.Undo();
			ResetNewGameFlag_Hook.Undo();
		};
	}

	public static bool NewGame = false;

	public ILHook SkipTutorialCutscene_ILHook = new ILHook(
		typeof(MainMenuWindowController).GetMethod("LoadGame", BindingFlags.Public | BindingFlags.Instance),
		(ILContext il) => {
			ILCursor cursor = new ILCursor(il);

			cursor.GotoNext(
				MoveType.Before,
				i => i.MatchLdarg(0), i => i.MatchLdarg(0)
			);
			
			cursor.RemoveRange(5);

			cursor.EmitDelegate(( ) => {
				TutorialSkip.NewGame = true;
				SceneLoader_RL.LoadScene(SceneID.Lineage, TransitionID.FadeToBlackWithLoading);
			});
		}
	);

	public Hook ResetSaveFile_Hook = new Hook(
		typeof(LineageWindowController).GetMethod("ConfirmQuitSelection", BindingFlags.NonPublic | BindingFlags.Instance),
		(Action<LineageWindowController> orig, LineageWindowController self) => {
			if (TutorialSkip.NewGame) {
				SpecialMode_EV.ResetSpecialModeSaveFile();
			}
			orig(self);
		}
	);

	public Hook ResetNewGameFlag_Hook = new Hook(
		typeof(LineageWindowController).GetMethod("ConfirmHeirSelection", BindingFlags.NonPublic | BindingFlags.Instance),
		(Action<LineageWindowController> orig, LineageWindowController self) => {
			orig(self);
			if (TutorialSkip.NewGame) {
				TutorialSkip.NewGame = false;
			}
		}
	);
}
