#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class HudEditor
{
    [MenuItem("Assets/Create/AtlasMapping", false, 350)]
    static void AtlasMapping()
    {
        SpriteAtlas spriteAtlas = Selection.activeObject as SpriteAtlas;
        string path = AssetDatabase.GetAssetPath(spriteAtlas);
        string dirpath = Path.GetDirectoryName(path);
        string filename = Path.GetFileNameWithoutExtension(path);
        string mappath = Path.Combine(dirpath, filename + ".asset");
        AtlasMapping atlasMapping = ScriptableObject.CreateInstance<AtlasMapping>();
        SerializedObject so = new SerializedObject(atlasMapping);
        SerializedProperty sp = so.FindProperty("m_SpriteAtlas");
        sp.objectReferenceValue = spriteAtlas;
        so.ApplyModifiedProperties();
        AssetDatabase.CreateAsset(atlasMapping, mappath);
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Create/AtlasMapping", true, 350)]
    static bool ValidateAtlasMapping()
    {
        return Selection.activeObject is SpriteAtlas;
    }

    static Texture2D GetFontMapping(TMP_FontAsset tmp_fontAtlas)
    {
        string path = AssetDatabase.GetAssetPath(tmp_fontAtlas);
        Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);
        for (int i = 0; i < objs.Length; i++)
        {
            Texture2D tex2d = objs[i] as Texture2D;
            if (tex2d != null&& tex2d.name == "FontAtlasMapping") return tex2d;
        }
        return null;
    }

    [MenuItem("Assets/Create/FontAtlasMapping", false, 350)]
    static void FontAtlasMapping()
    { 
        //TMP_FontAsset tmp_fontAtlas = Selection.activeObject as TMP_FontAsset;
//         Texture2D fontmapping = GetFontMapping(tmp_fontAtlas);
//         if (fontmapping != null)
//         {
//             tmp_fontAtlas.SetFontAtlasMapping(fontmapping);
//             return;
//         }
//         Texture2D m_FontAtlasMappingTex = new Texture2D(64, 64, TextureFormat.RGBA32, false, PlayerSettings.colorSpace == ColorSpace.Linear);
//         m_FontAtlasMappingTex.wrapMode = TextureWrapMode.Clamp;
//         m_FontAtlasMappingTex.filterMode = FilterMode.Point;
//         m_FontAtlasMappingTex.name = "FontAtlasMapping";
//         tmp_fontAtlas.SetFontAtlasMapping(m_FontAtlasMappingTex);
//         string path = AssetDatabase.GetAssetPath(tmp_fontAtlas);
//         AssetDatabase.AddObjectToAsset(m_FontAtlasMappingTex, path);
//         EditorUtility.SetDirty(tmp_fontAtlas);
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Create/FontAtlasMapping", true, 350)]
    static bool ValidateFontAtlasMapping()
    {
        return Selection.activeObject is TMP_FontAsset;
    }

    [MenuItem("Assets/Create/HudMesh", false, 351)]
    static void HudMesh()
    {
        Mesh mesh = new Mesh();
        List<int> indexs = new List<int>();
        List<Vector3> listVectors = new List<Vector3>();
        List<Vector2> listUVs0 = new List<Vector2>();
        List<Vector2> listUVs1 = new List<Vector2>();
        for (int i = 0; i < 9; i++)
        {
            listVectors.Add(new Vector3(0, 0, 0));
            listVectors.Add(new Vector3(0, 1, 0));
            listVectors.Add(new Vector3(1, 1, 0));
            listVectors.Add(new Vector3(1, 0, 0));

            listUVs0.Add(new Vector2(0, 0));
            listUVs0.Add(new Vector2(0, 1));
            listUVs0.Add(new Vector2(1, 1));
            listUVs0.Add(new Vector2(1, 0));

            listUVs1.Add(new Vector2(i, i));
            listUVs1.Add(new Vector2(i, i));
            listUVs1.Add(new Vector2(i, i));
            listUVs1.Add(new Vector2(i, i));

            indexs.Add(i * 4 + 0);
            indexs.Add(i * 4 + 1);
            indexs.Add(i * 4 + 2);
            indexs.Add(i * 4 + 0);
            indexs.Add(i * 4 + 2);
            indexs.Add(i * 4 + 3);
        }
        mesh.SetVertices(listVectors);
        mesh.SetUVs(0, listUVs0);
        mesh.SetUVs(1, listUVs1);
        mesh.SetIndices(indexs, MeshTopology.Triangles, 0);
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (Directory.Exists(path))
        {
            path = Path.Combine(path, "HudMesh.mesh");
            AssetDatabase.CreateAsset(mesh, path);
            AssetDatabase.Refresh();
            return;
        }
        path = Path.GetDirectoryName(path);
        path = Path.Combine(path, "HudMesh.mesh");
        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.Refresh();
    }
}
#endif 
