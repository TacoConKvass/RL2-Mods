using RL2.API;
using System.IO;
using UnityEngine;

public class TextureSwapGlobalEnemy : IRegistrable
{
	static Texture2D texture = new Texture2D(1, 1);

	public void Register() {
		Enemy.OnSpawn += OnSpawn;
	}

	public static void OnSpawn(EnemyController enemy)
	{
		if (enemy.EnemyType != EnemyType.DancingBoss) {
			return;
		}

		ConfigFile config = JsonUtility.FromJson<ConfigFile>(File.ReadAllText(NaamahEnhanced.Config));

		if (config.Forced != "")
		{
			if (File.Exists(NaamahEnhanced.Instance.Path + $"\\Assets\\{config.Forced}"))
			{
				texture.LoadImage(File.ReadAllBytes(NaamahEnhanced.Instance.Path + $"\\Assets\\{config.Forced}"));
			}
			else
			{
				Mod.Log($"<color=red>Forced texture {config.Forced} was not found. Aborting texture change. </color=red>");
			}
		}
		else
		{
			string[] options = enemy.EnemyRank == EnemyRank.Advanced ? config.PrimeTextures : config.RegularTextures;
			int index = Random.Range(0, options.Length);
			if (File.Exists(NaamahEnhanced.Instance.Path + $"\\Assets\\{options[index]}"))
			{
				texture.LoadImage(File.ReadAllBytes(NaamahEnhanced.Instance.Path + $"\\Assets\\{options[index]}"));
			}
			else
			{
				Mod.Log($"<color=red>Chosen texture {config.Forced} was not found. Aborting texture change. </color=red>");
			}
		}

		foreach (Renderer renderer in enemy.RendererArray)
		{
			Texture oldTexture = renderer.material.GetTexture("_DiffuseTexture");
			if (oldTexture != null)
			{
				renderer.material.SetTexture("_DiffuseTexture", texture);
			}
		}
	}
}
