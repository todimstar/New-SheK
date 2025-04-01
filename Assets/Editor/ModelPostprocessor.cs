#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.IO;

public class ModelPostprocessor : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;
        if (modelImporter != null)
        {
            modelImporter.materialImportMode = ModelImporterMaterialImportMode.ImportViaMaterialDescription;
            modelImporter.materialLocation = ModelImporterMaterialLocation.External;
            modelImporter.materialName = ModelImporterMaterialName.BasedOnMaterialName;
            modelImporter.materialSearch = ModelImporterMaterialSearch.RecursiveUp;
        }
    }

    void OnPostprocessModel(GameObject model)
    {
        if (EditorApplication.isUpdating || AssetDatabase.IsAssetImportWorkerProcess())
        {
            string path = assetPath; // 关键：记录资源路径而非GameObject
            EditorApplication.delayCall += () =>
            {
                var reloadedModel = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (reloadedModel != null) OnPostprocessModel(reloadedModel);
            };
            return;
        }
        // 原有处理逻辑
        HandleImportTextures(model);
    }

    /// <summary>检查并创建材质目录</summary>
    private void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>处理导入模型的贴图</summary>
    private void HandleImportTextures(GameObject model)
    {
        string texturesPath = Path.Combine("Assets", "Model", "Textures");
        // 确保贴图目录存在
        EnsureDirectoryExists(texturesPath);

        // 遍历所有渲染组件
        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.sharedMaterials)
            {
                if (material == null) continue;

                // 获取材质所有纹理属性
                string[] textureNames = material.GetTexturePropertyNames();
                foreach (string texName in textureNames)
                {
                    Texture texture = material.GetTexture(texName);
                    if (texture == null)
                    {
                        continue;
                    }
                    // 获取原始贴图路径
                    string sourcePath = AssetDatabase.GetAssetPath(texture);
                    if (string.IsNullOrEmpty(sourcePath))
                    {
                        Debug.Log($"没有找到 {texName} 对应的贴图");
                        continue;
                    }

                    // 构建目标路径
                    string targetPath = Path.Combine(texturesPath, Path.GetFileName(sourcePath));

                    // 检查贴图是否已存在
                    Texture existing = AssetDatabase.LoadAssetAtPath<Texture>(targetPath);
                    if (existing != null)
                    {
                        material.SetTexture(texName, existing);
                    }
                    else
                    {
                        // 复制贴图到新位置
                        AssetDatabase.CopyAsset(sourcePath, targetPath);
                        existing = AssetDatabase.LoadAssetAtPath<Texture>(targetPath);
                        material.SetTexture(texName, existing);
                    }
                }
                EditorUtility.SetDirty(material);
            }
            EditorUtility.SetDirty(renderer);
        }
    }
}

#endif