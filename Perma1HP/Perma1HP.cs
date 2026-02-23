using API = RL2.API;
namespace Perma1HP;

public class Perma1HP : API.Mod, API.IRegistrable
{
	public override void OnLoad() => Log("Perma 1HP loaded");

	void API.IRegistrable.Register()
	{
		API.Player.HeirGeneration.ModifyCharacterData.Event += (CharacterData data, bool _, bool _) =>
		{
			if (SaveManager.PlayerSaveData.SpecialModeType != SpecialModeType.None) return;
			data.TraitOne = TraitType.OneHitDeath;
		};
	}
}