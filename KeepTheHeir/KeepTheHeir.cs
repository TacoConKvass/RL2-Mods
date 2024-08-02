using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RL2.ModLoader;
using System;
using System.Collections;
using System.Reflection;

namespace KeepTheHeir;

[ModEntrypoint]
public class KeepTheHeir
{
	public ILHook RedHoodNPCDialogueEdit = new ILHook(
		typeof(NewGamePlusShop).GetNestedType("<OpenNGPlusShopCoroutine>d__27", BindingFlags.NonPublic).GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance),
		(ILContext il) => { 
			ILCursor cursor = new ILCursor(il);

			cursor.GotoNext(
				i => i.MatchLdloc(1), i => i.MatchCall<NewGamePlusShop>("RunEndingDialogue")
			);

			cursor.RemoveRange(3);
		}
	);

	public ILHook DontResetSaveData = new ILHook(
		typeof(NewGamePlusShop).GetNestedType("<StartNewGamePlusCoroutine>d__34", BindingFlags.NonPublic).GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance),
		(ILContext il) => { 
			ILCursor cursor = new ILCursor(il);

			cursor.GotoNext(
				MoveType.After,
				i => i.MatchLdsfld(typeof(SaveManager).GetField("PlayerSaveData", BindingFlags.Public | BindingFlags.Static))
			);

			cursor.RemoveRange(4);

			cursor.Emit(OpCodes.Ldc_I4_0);
		}
	);

	public KeepTheHeir() {
		ModLoader.OnLoad += (() => {
			DontResetSaveData.Apply();
			RedHoodNPCDialogueEdit.Apply();
		});

		ModLoader.OnUnload += (() => {
			DontResetSaveData.Undo();
			RedHoodNPCDialogueEdit.Undo();
		});
	}
}
