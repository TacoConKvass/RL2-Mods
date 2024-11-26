using MonoMod.RuntimeDetour;
using Rewired.Utils.Libraries.TinyJson;
using RL_Windows;
using RL2.ModLoader;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IGTDisplay;

[ModEntrypoint]
public class IGTDisplay
{
	public static string Path { get; private set; }

	public static DisplayConfig Config { get; set; }

	[Command("IGTD:UpdateConfig")]
	public static void UpdateConfig(object[] args) {
		Config = ReadConfig();
		ModLoader.Log("Updated IGTD config");
	}

	public static DisplayConfig ReadConfig() {
		if (!File.Exists(Path + "config.json")) {
			DisplayConfig config = new DisplayConfig() {
				OffsetX = 150,
				OffsetY = 120,
				Length = 130,
				Height = 30,
			};

			File.WriteAllText(Path + "config.json", JsonWriter.ToJson(config).Prettify());
		}

		return JsonParser.FromJson<DisplayConfig>(File.ReadAllText(Path + "config.json"));
	}

	public IGTDisplay() {
		ModLoader.OnLoad += () => {
			AttachIGTDisplay_Hook.Apply();
			ShowInPauseMenu_Hook.Apply();
			HideOnQuitPauseMenu_Hook.Apply();
		};

		ModLoader.OnUnload += () => {
			AttachIGTDisplay_Hook.Undo();
			ShowInPauseMenu_Hook.Undo();
			HideOnQuitPauseMenu_Hook.Undo();
		};

		Path = ModLoader.ModManifestToPath[ModLoader.ModManifestToPath.Keys.Where(manifest => manifest.Name == "In Game Time Display").FirstOrDefault()] + "\\";
		Config = ReadConfig();
	}

	public Hook AttachIGTDisplay_Hook = new Hook(	
		typeof(GameManager).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance),
		(Action<GameManager> orig, GameManager self) => {
			if (!GameManager.IsGameManagerInstantiated) {
				self.gameObject.AddComponent<IGT_Displayer>();
			}
			orig(self);
		},
		new HookConfig() {
			ID = "IGTD::AttachIGTDisplay"
		}
	);

	public Hook ShowInPauseMenu_Hook = new Hook(
		typeof(PauseWindowController).GetMethod("OnOpen", BindingFlags.NonPublic | BindingFlags.Instance),
		(Action<PauseWindowController> orig, PauseWindowController self) => {
			orig(self);
			IGT_Displayer.Visible = true;
		},
		new HookConfig() {
			ID = "IGTD::ShowInPauseMenu"
		}
	); 
	
	public Hook HideOnQuitPauseMenu_Hook = new Hook(
		typeof(PauseWindowController).GetMethod("OnClose", BindingFlags.NonPublic | BindingFlags.Instance),
		(Action<PauseWindowController> orig, PauseWindowController self) => {
			orig(self);
			IGT_Displayer.Visible = false;
		},
		new HookConfig() {
			ID = "IGTD::HideOnQuitPauseMenu"
		}
	);
}
