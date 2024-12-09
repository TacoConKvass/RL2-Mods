using RL2.API;
using System;

namespace FastStart;

public class FastStart : Mod {
	public override void OnLoad() {
		Player.OnSpawn += GrantBonuses;
	}
	public override void OnUnload() {
		Player.OnSpawn -= GrantBonuses;
	}

	public static void GrantBonuses(PlayerController player) {
		foreach (HeirloomType val in new HeirloomType[] {
			HeirloomType.UnlockVoidDash,
			HeirloomType.UnlockAirDash,
			HeirloomType.UnlockDoubleJump,
			HeirloomType.UnlockBouncableDownstrike,
			HeirloomType.CaveLantern,
			HeirloomType.UnlockMemory,
			HeirloomType.UnlockEarthShift,
		}) {
			SaveManager.PlayerSaveData.SetHeirloomLevel(val, 1, false, false);
		}
		SaveManager.PlayerSaveData.SetInsightState(InsightType.ForestBoss_DoorOpened, InsightState.ResolvedButNotViewed, true);
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CaveMiniboss_BlackDoor_Opened, true);
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CaveMiniboss_WhiteDoor_Opened, true);
	}
}