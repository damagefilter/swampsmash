#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

public class TopMenu {

	[MenuItem("Edit/Reset Playerprefs")] public static void DeletePlayerPrefs() { PlayerPrefs.DeleteAll(); }
}
#endif
