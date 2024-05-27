using RL2.ID;
using RL2.ModLoader;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextureSwapGlobalEnemy : GlobalEnemy
{
	public override Dictionary<int, EnemyRank[]> AppliesToEnemy => new Dictionary<int, EnemyRank[]>() 
	{
		{ EnemyID.DancingBoss, new EnemyRank[] { EnemyRank.Basic, EnemyRank.Advanced, EnemyRank.Expert } }
	};

	Texture2D texture = new Texture2D(1, 1);

	public override void OnSpawn()
	{
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
			string[] options = Rank == EnemyRank.Advanced ? config.PrimeTextures : config.RegularTextures;
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

		foreach (Renderer renderer in Enemy.RendererArray)
		{
			Texture oldTexture = renderer.material.GetTexture("_DiffuseTexture");
			if (oldTexture != null)
			{
				renderer.material.SetTexture("_DiffuseTexture", texture);
        	}
        }
	}
}