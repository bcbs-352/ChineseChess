using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

public class GetBoardPng : MonoBehaviour
{
    public Button takeBtn;
    public RawImage imgCam;
    void Start()
    {
        takeBtn.onClick.AddListener(OnTakePicture);
    }

    void OnTakePicture()
    {
        string dirPath = Application.dataPath + $"/ScreenShot/picture.png";
        SaveTexture(dirPath, (RenderTexture)imgCam.texture);
    }

    void SaveTexture(string path,RenderTexture renderTexture)
    {
        int width = renderTexture.width;
        int height = renderTexture.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);  // 空白贴图
        var backupActive = RenderTexture.active;
        RenderTexture.active = renderTexture;   // 将渲染贴图信息放入缓存
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);    // 复制缓存信息
        tex.Apply();
        RenderTexture.active = backupActive;
        var bytes = tex.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }
}