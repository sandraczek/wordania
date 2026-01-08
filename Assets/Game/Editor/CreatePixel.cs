using UnityEngine;
using UnityEditor;
using System.IO;

public class CreatePixel
{
    [MenuItem("Assets/Create/1x1 White Pixel")]
    public static void Create()
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/WhitePixel1x1.png", bytes);
        AssetDatabase.Refresh();
    }
}