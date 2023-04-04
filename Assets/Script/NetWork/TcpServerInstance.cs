using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TcpServerInstance : MonoBehaviour
{
    public static TcpServerInstance instance;
    TcpServer tcpServer;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        tcpServer = gameObject.AddComponent<TcpServer>();
        tcpServer.SocketConnect();
    }

    void Update()
    {
        
    }

    public void SendStr(string str)
    {
        tcpServer.SetSendStr(str);
    }

    public void SendBytes(byte[] bytes)
    {
        tcpServer.SetSendBytes(bytes);
    }
    private void OnApplicationQuit()
    {
        tcpServer.SocketQuit();
        Debug.Log("TcpServer¹Ø±Õ");
    }
}
