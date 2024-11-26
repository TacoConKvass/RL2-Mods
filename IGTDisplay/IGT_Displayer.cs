using System.IO;
using UnityEngine;

namespace IGTDisplay;

public class IGT_Displayer : MonoBehaviour {
	public static bool Visible = false;

	public const string TexturePath = "Ribbon.png";

	private Texture2D texture = new Texture2D(1, 1);

	public GUIStyle Style;

	public void Awake() {
		texture.LoadImage(File.ReadAllBytes(IGTDisplay.Path + TexturePath));
		Style = new GUIStyle() {
			normal = new GUIStyleState() {
				background = texture
			}
		}; 
	}

	public void OnGUI() {
		if (!Visible) {
			return;
		}
		uint secondsPlayed = SaveManager.PlayerSaveData.SecondsPlayed;
		DisplayConfig config = IGTDisplay.Config;
		GUILayout.BeginArea(new Rect(Screen.width - config.OffsetX, config.OffsetY, config.Length, config.Height), Style);
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label($"{(int)(secondsPlayed/3600)}:{(int)(secondsPlayed % 3600 / 60)}:{secondsPlayed%60:D2}");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}
