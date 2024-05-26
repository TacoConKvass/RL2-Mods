using System.IO;
using RL2.ModLoader;
using UnityEngine;

public class NaamahEnhanced : Mod
{
	public static NaamahEnhanced Instance;

	public static string Config => Instance.Path + "\\naamah-texture-config.json";
	
	public NaamahEnhanced()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public override void OnLoad()
	{
		if (!File.Exists(Config))
		{
			File.WriteAllText(Config, JsonUtility.ToJson(new ConfigFile(), true));
		}
	}
}