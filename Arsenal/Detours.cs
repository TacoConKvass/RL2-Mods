using MonoMod.RuntimeDetour;
using RL2.API;
using System;
using System.Reflection;

namespace Arsenal.Detours;

public class Detours : IRegistrable 
{
	public Hook ShotgunDetour;
	public Hook PistolDetour;
	public Hook InitialCharacterCreation;

	public int TalentTimer = 0;
	public int SpellTimer = 0;

	public void Register() {
		ShotgunDetour = new Hook(
			typeof(Shotgun_Ability).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance),
			new Action<Action<Shotgun_Ability>, Shotgun_Ability>((Action<Shotgun_Ability> orig, Shotgun_Ability self) => {
				// Run vanilla Update
				orig(self);

				// If this ability is in the Talent slot
				if (self.CastAbilityType == CastAbilityType.Talent && self.CurrentAmmo == 0) {
					TalentTimer++;
					if (TalentTimer == 120) {
						self.CurrentAmmo = self.MaxAmmo;
						TalentTimer = 0;
					}
				}

				// If this ability is in the Spell slot
				if (self.CastAbilityType == CastAbilityType.Spell && self.CurrentAmmo == 0) {
					SpellTimer++;
					if (SpellTimer == 120) {
						self.CurrentAmmo = self.MaxAmmo;
						SpellTimer = 0;
					}
				}
			})
		);

		PistolDetour = new Hook(
			typeof(PistolWeapon_Ability).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance),
			new Action<Action<PistolWeapon_Ability>, PistolWeapon_Ability>((Action<PistolWeapon_Ability> orig, PistolWeapon_Ability self) => {
				// Run vanilla Update
				orig(self);

			   // If this ability is in the Talent slot
			   if (self.CastAbilityType == CastAbilityType.Talent && self.CurrentAmmo == 0) {
					TalentTimer++;
					if (TalentTimer == 120)
					{
						self.CurrentAmmo = self.MaxAmmo;
						TalentTimer = 0;
					}
				}

				// If this ability is in the Spell slot
				if (self.CastAbilityType == CastAbilityType.Spell && self.CurrentAmmo == 0) {
					SpellTimer++;
					if (SpellTimer == 120) {
						self.CurrentAmmo = self.MaxAmmo;
						SpellTimer = 0;
					}
				}
			})
		);

        InitialCharacterCreation = new Hook(
			typeof(TutorialRoomController).GetMethod("CreateStartingCharacter", BindingFlags.NonPublic | BindingFlags.Instance),
			new Action<Action<TutorialRoomController>, TutorialRoomController>((Action<TutorialRoomController> orig, TutorialRoomController self) => {
				// Run vanilla logic
				orig(self);
                PlayerController playerController = PlayerManager.GetPlayerController();
                CharacterData characterData = SaveManager.PlayerSaveData.CurrentCharacter;
				GameManager.Instance.GetComponent<SetAbilitiesSystem>().ModifyGeneratedCharacter(characterData);
                playerController.CharacterClass.SetAbility(CastAbilityType.Weapon, SaveManager.PlayerSaveData.CurrentCharacter.Weapon);
                playerController.CharacterClass.SetAbility(CastAbilityType.Spell, SaveManager.PlayerSaveData.CurrentCharacter.Spell);
                playerController.CharacterClass.SetAbility(CastAbilityType.Talent, SaveManager.PlayerSaveData.CurrentCharacter.Talent);
                playerController.ResetCharacter();
                playerController.SetMana(0f, additive: false, runEvents: true);
            })
		);
	}
}