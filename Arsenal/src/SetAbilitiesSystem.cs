using Arsenal.Config;
using RL2.ModLoader;

namespace Arsenal;

public class SetAbilitiesSystem : ModSystem
{
	AbilityType[] AllWeapons() => CharacterCreator.GetAvailableWeapons(ClassType.CURIO_SHOPPE_CLASS).Add(AbilityType.KunaiWeapon);
	AbilityType[] AllSpells() => CharacterCreator.GetAvailableSpells(ClassType.CURIO_SHOPPE_CLASS);
	AbilityType[] AllTalents() => CharacterCreator.GetAvailableTalents(ClassType.CURIO_SHOPPE_CLASS).Add(AbilityType.KiStrikeTalent);
    ArsenalConfig Config => Arsenal.Instance.Config;

    public override void ModifyGeneratedCharacter(CharacterData characterData) {
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
}