using RL2.ModLoader;

namespace CursedGambling;

public class ForceGambler : ModSystem {
	public override void ModifyGeneratedCharacterData(CharacterData characterData, bool classLocked, bool spellLocked) {
		characterData.TraitOne = TraitType.BonusChestGold;
	}
}