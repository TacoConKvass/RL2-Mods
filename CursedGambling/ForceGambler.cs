using RL2.API;

namespace CursedGambling;

public class ForceGambler : IRegistrable {
	public void Register() {
		Player.HeirGeneration.ModifyCharacterData.Event += (CharacterData characterData, bool classLocked, bool spellLocked) => {
			characterData.TraitOne = TraitType.BonusChestGold;
		};
	}	
}