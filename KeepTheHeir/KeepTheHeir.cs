using KeepTheHeir.Config;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Rewired.Utils.Libraries.TinyJson;
using RL2.ModLoader;
using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace KeepTheHeir;

[ModEntrypoint]
public partial class KeepTheHeir
{
	public static KeepTheHeir Instance { get; private set; }

	public static string ConfigPath => ModLoader.ModPath + "\\KeepTheHeir.config.json";

	public KeepTheHeirConfig Config { get; private set; }

	public KeepTheHeir() {
		if (Instance == null) {
			Instance = this;
		}

		ModLoader.OnLoad += (() => {
			DontResetSaveData.Apply();
			RedHoodNPCDialogueEdit.Apply();

			if (!File.Exists(ConfigPath)) {
				File.WriteAllText(ConfigPath, JsonWriter.ToJson(new KeepTheHeirConfig() { GiveMoneyToCharonWhenLooping = false }).Prettify());
			}

			Config = JsonParser.FromJson<KeepTheHeirConfig>(File.ReadAllText(ConfigPath));
		});

		ModLoader.OnUnload += (() => {
			DontResetSaveData.Undo();
			RedHoodNPCDialogueEdit.Undo();
		});
	}
}
