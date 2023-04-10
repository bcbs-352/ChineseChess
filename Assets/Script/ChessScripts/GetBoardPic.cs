using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

public class GetBoardPic : MonoBehaviour
{
    public Button takeBtn;
    public RawImage imgCam;
    public GameObject serverInstance;

    private byte[] bytes;
    void Start()
    {
        //takeBtn.onClick.AddListener(OnTakePicture);

    }

    public void OnTakePicture()
    {
        string dirPath = Application.dataPath + $"/BoardShot/picture.jpg";
        SaveTexture(dirPath, (RenderTexture)imgCam.texture);
    }

    public void SaveTexture(string path, RenderTexture renderTexture)
    {
        int width = renderTexture.width;
        int height = renderTexture.height;
        //Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);  // 空白贴图
        Texture2D tex = new Texture2D(width, height);
        var backupActive = RenderTexture.active;
        RenderTexture.active = renderTexture;   // 将渲染贴图信息放入缓存
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);    // 复制缓存信息
        tex.Apply();
        RenderTexture.active = backupActive;
        bytes = tex.EncodeToJPG();
        File.WriteAllBytes(path, bytes);

        Debug.Log("发送图像");
        //serverInstance.GetComponent<TcpServerInstance>().SendStr("666");
        serverInstance.GetComponent<TcpServerInstance>().SendBytes(bytes);
    }
}