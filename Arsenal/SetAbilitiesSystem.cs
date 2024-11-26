using Arsenal.Config;
using RL2.ModLoader;
using RL2.API;

namespace Arsenal;

public class SetAbilitiesSystem : IRegistrable
{
	AbilityType[] AllWeapons() => CharacterCreator.GetAvailableWeapons(ClassType.CURIO_SHOPPE_CLASS).Add(AbilityType.KunaiWeapon);
	AbilityType[] AllSpells() => CharacterCreator.GetAvailableSpells(ClassType.CURIO_SHOPPE_CLASS);
	AbilityType[] AllTalents() => CharacterCreator.GetAvailableTalents(ClassType.CURIO_SHOPPE_CLASS).Add(AbilityType.KiStrikeTalent);
	ArsenalConfig Config => Arsenal.Instance.Config;

	public void Register() {
		Player.HeirGeneration.ModifyCharacterRandomization += ModifyCharacterRandomization;
		Player.HeirGeneration.ModifyCharacterData +=  ModifyGeneratedCharacter;
	}

	public void ModifyCharacterRandomization(CharacterData characterData) {
		if (Config.WeaponsOnly.AppliesToAllClasses || Config.WeaponsOnly.AppliesToClasses.IndexOf(characterData.ClassType) != -1) {
			AbilityType[] Weapons = AllWeapons();
			characterData.Weapon = Weapons[RNGManager.GetRandomNumber(RngID.Lineage, "Arsenal: GetRandomWeaponSlot", 0, Weapons.Length)];
			characterData.Spell = Weapons[RNGManager.GetRandomNumber(RngID.Lineage, "Arsenal: GetRandomSpellSlot", 0, Weapons.Length)];
			characterData.Talent = Weapons[RNGManager.GetRandomNumber(RngID.Lineage, "Arsenal: GetRandomTalentSlot", 0, Weapons.Length)];
		}

		if (Config.SpellsOnly.AppliesToAllClasses || Config.SpellsOnly.AppliesToClasses.IndexOf(characterData.ClassType) != -1) {
			AbilityType[] Spells = AllSpells();
			characterData.Weapon = Spells[RNGManager.GetRandomNumber(RngID.Lineage, "Arsenal: GetRandomWeaponSlot", 0, Spells.Length)];
			characterData.Spell = Spells[RNGManager.GetRandomNumber(RngID.Lineage, "Arsenal: GetRandomSpellSlot", 0, Spells.Length)];
			characterData.Talent = Spells[RNGManager.GetRandomNumber(RngID.Lineage, "Arsenal: GetRandomTalentSlot", 0, Spells.Length)];
		}

		if (Config.TalentsOnly.AppliesToAllClasses || Config.TalentsOnly.AppliesToClasses.IndexOf(characterData.ClassType) != -1) {
			AbilityType[] Talents = AllTalents();
			characterData.Weapon = Talents[RNGManager.GetRandomNumber(RngID.Lineage, "Arsenal: GetRandomWeaponSlot", 0, Talents.Length)];
			characterData.Spell = Talents[RNGManager.GetRandomNumber(RngID.Lineage, "Arsenal: GetRandomSpellSlot", 0, Talents.Length)];
			characterData.Talent = Talents[RNGManager.GetRandomNumber(RngID.Lineage, "Arsenal: GetRandomTalentSlot", 0, Talents.Length)];
		}
	}

	public void ModifyGeneratedCharacter(CharacterData characterData, bool classLocked, bool spellLocked)
	{
		ModifyCharacterRandomization(characterData);
		if (Config.SpellsOnly.AppliesToAllClasses || Config.SpellsOnly.AppliesToClasses.IndexOf(characterData.ClassType) != -1) {
			if (characterData.TraitOne == TraitType.None) {
				characterData.TraitOne = TraitType.BonusMagicStrength;
			}
			else if (characterData.TraitOne != TraitType.BonusMagicStrength){
				characterData.TraitTwo = TraitType.BonusMagicStrength;
			}
		}
	}
}