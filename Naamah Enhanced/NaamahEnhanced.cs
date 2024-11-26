using RL2.API;
using System.IO;
using UnityEngine;

public class NaamahEnhanced : Mod
{
	public static NaamahEnhanced Instance;

	public static string Config => Instance.Path + "\\naamah-texture-config.json";
	
	public override void OnLoad()
	{
		Instance = this;
		
		if (!File.Exists(Config))
		{
			File.WriteAllText(Config, JsonUtility.ToJson(new ConfigFile(), true));
		}
	}
}
