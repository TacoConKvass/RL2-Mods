using Arsenal.Config;
using Arsenal.Utils;
using Rewired.Utils.Libraries.TinyJson;
using RL2.ModLoader;
using System.IO;

namespace Arsenal;

public class Arsenal : Mod
{
	public static Arsenal Instance { get; private set; }
	public ArsenalConfig Config { get; private set; }
	public string ConfigPath => Path + "\\arsenal-config.json";

	public override void OnLoad() {
		Instance = this;
		if (!File.Exists(ConfigPath))
		{
			Config = new ArsenalConfig() {
				WeaponsOnly = new ModeConfig() {
					AppliesToAllClasses = true,
					AppliesToClasses = new ClassType[] { }
				},
				SpellsOnly = new ModeConfig() {
					AppliesToClasses = new ClassType[] { ClassType.MagicWandClass }
				},
				TalentsOnly = new ModeConfig() {
					AppliesToClasses = new ClassType[] { ClassType.SaberClass }
				}
			};
			File.WriteAllText(ConfigPath, JsonWriter.ToJson(Config).FormatJson("    "));
			Log("Created Arsenal config");
			return;
		}
		Config = JsonParser.FromJson<ArsenalConfig>(File.ReadAllText(ConfigPath));
		Log("Loaded Arsenal config");
	}
}