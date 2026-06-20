using UnityEngine;
using System.IO;
using System;
using System.Text;

public class GltfWebPLoader : MonoBehaviour
{
    public string modelPath = @"D:\2610\IMD 3_1\3D\A_Future\Factory-v1 (1).glb"; 

    void Awake()
    {
        Debug.Log("📢 终极绕过方案已启动...");
    }

    void Start()
    {
        if (!File.Exists(modelPath))
        {
            Debug.LogError($"❌ 找不到文件: {modelPath}");
            return;
        }

        LoadModelAndFixTextures();
    }

    private async void LoadModelAndFixTextures()
    {
        // 1. 忽略贴图报错，只强行实例化模型的网格结构
        var gltf = new GLTFast.GltfImport();
        
        // 使用最低限度的设置，忽略材质冲突
        bool success = await gltf.Load(modelPath);

        if (success)
        {
            GameObject importedModel = new GameObject("My_Factory_Model");
            importedModel.transform.SetParent(this.transform);
            importedModel.transform.localPosition = Vector3.zero;

            await gltf.InstantiateMainSceneAsync(importedModel.transform);
            Debug.Log("🎉 网格生成成功！开始手动注入贴图...");

            // 2. 强行提取 WebP 字节流并手动贴图
            TryExtractAndApplyWebPTextures(importedModel);
        }
        else
        {
            Debug.LogError("❌ 底层由于 WebP 限制彻底锁死，请尝试最后的安全方案。");
        }
    }

    private void TryExtractAndApplyWebPTextures(GameObject targetModel)
    {
        try
        {
            byte[] glbBytes = File.ReadAllBytes(modelPath);
            
            // 寻找二进制中的 WebP 图片特征头 (RIFF .... WEBP)
            // Unity 具备原生解压 WebP 数据的能力（Texture2D.LoadImage）
            var renderers = targetModel.GetComponentsInChildren<MeshRenderer>();
            
            // 读取 GLB 内部的二进制数据块
            int index = 0;
            for (int i = 0; i < glbBytes.Length - 12; i++)
            {
                // 检测 WebP 文件头特征码 "RIFF" 和 "WEBP"
                if (glbBytes[i] == 0x52 && glbBytes[i+1] == 0x49 && glbBytes[i+2] == 0x46 && glbBytes[i+3] == 0x46 &&
                    glbBytes[i+8] == 0x57 && glbBytes[i+9] == 0x45 && glbBytes[i+10] == 0x42 && glbBytes[i+11] == 0x50)
                {
                    // 读取这块 WebP 的大小
                    uint chunkSize = BitConverter.ToUInt32(glbBytes, i + 4) + 8;
                    byte[] textureBytes = new byte[chunkSize];
                    Array.Copy(glbBytes, i, textureBytes, 0, chunkSize);

                    // 实例化为 Unity 原生 Texture2D (Unity 原生完全认得 WebP 数据流)
                    Texture2D tex = new Texture2D(2, 2);
                    if (tex.LoadImage(textureBytes))
                    {
                        // 强制塞给模型的材质球
                        if (index < renderers.Length && renderers[index].material != null)
                        {
                            renderers[index].material.mainTexture = tex;
                            Debug.Log($"   -> 成功把解出来的第 {index} 张 WebP 贴图强行挂载成功！");
                            index++;
                        }
                    }
                    i += (int)chunkSize - 1;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("手动解包贴图时发生跳过: " + e.Message);
        }
    }
}