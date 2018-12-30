using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class XSplitRBG_AEditor{


    [MenuItem("Assets/Depart RGB&A")]
    static void SeperateAllTexturesRGBandAlphaChannel()
    {
        string outputRGBPath = EditorUtility.SaveFilePanel("SaveRGBResource", Application.dataPath, "New Resource", "png");
        string outputAlphaPath = EditorUtility.SaveFilePanel("SaveAlphaResource", Application.dataPath, "New Resource", "png");
        
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);
        importer.isReadable = true;
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        Texture2D sourcetex = Selection.activeObject as Texture2D;

        if (sourcetex == null)
        {
            Debug.LogError("Load Texture Failed");
            return;
        }
        Texture2D rgbTex = new Texture2D(sourcetex.width, sourcetex.height, TextureFormat.RGB24, false);
        rgbTex.SetPixels(sourcetex.GetPixels());
        rgbTex.Apply();

        Color[] sourceColors = sourcetex.GetPixels();
        Color[] colorsAlpha = new Color[sourceColors.Length];
        for (int i = 0; i < sourceColors.Length; ++i)
        {
            colorsAlpha[i].r = sourceColors[i].a;
            colorsAlpha[i].g = sourceColors[i].a;
            colorsAlpha[i].b = sourceColors[i].a;
        }
        Texture2D alphaTex = new Texture2D(sourcetex.width, sourcetex.height, TextureFormat.RGB24, false);
        alphaTex.SetPixels(colorsAlpha);
        alphaTex.Apply();

        byte[] bytes = rgbTex.EncodeToPNG();
        File.WriteAllBytes(outputRGBPath, bytes);
        byte[] alphabytes = alphaTex.EncodeToPNG();
        File.WriteAllBytes(outputAlphaPath, alphabytes);

        importer.isReadable = false;
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
    }
}
