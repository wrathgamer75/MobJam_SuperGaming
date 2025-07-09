using UnityEditor;
using UnityEngine;

public class ClearPlayerPrefs 
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    public static void ClearingPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared.");
    }
}
