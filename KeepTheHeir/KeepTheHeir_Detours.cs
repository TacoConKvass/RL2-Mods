using KeepTheHeir.Config;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RL2.ModLoader;
using System;
using System.Collections;
using System.Reflection;

namespace KeepTheHeir;

public partial class KeepTheHeir {	
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
			ILLabel endpoint = cursor.DefineLabel();

			cursor.GotoNext(
				MoveType.After,
				i => i.MatchLdsfld(typeof(SaveManager).GetField("PlayerSaveData", BindingFlags.Public | BindingFlags.Static))
			);

			cursor.RemoveRange(4);

			cursor.Emit(OpCodes.Ldc_I4_0);
		
			cursor.GotoNext(
				MoveType.After,
				i => i.MatchLdnull(), i => i.MatchLdnull()
			);

			// TODO: Actually match to the previous instruction, but whatever
			cursor.Index++;
			cursor.EmitDelegate(() => PlayerManager.GetPlayerController().CurrentlyInRoom.BiomeType == BiomeType.DriftHouse ? true : KeepTheHeir.Instance.Config.GiveMoneyToCharonWhenLooping);
			cursor.Emit(OpCodes.Brfalse, endpoint);

			cursor.GotoNext(
				MoveType.After,
				i => i.MatchLdsfld(typeof(SaveManager).GetField("PlayerSaveData", BindingFlags.Public | BindingFlags.Static)), i => i.MatchLdloc(1), i => i.MatchCallvirt<PlayerSaveData>("GiveMoneyToCharon") 
			);

			cursor.MarkLabel(endpoint);
		}
	);

}