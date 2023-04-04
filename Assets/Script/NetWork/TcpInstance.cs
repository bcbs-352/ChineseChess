using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TcpInstance : MonoBehaviour
{
    public static TcpInstance _instance;
    TcpClient tcpClient;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        tcpClient = gameObject.AddComponent<TcpClient>();
        tcpClient.InitSocket();
    }
    private void Update()
    {

    }
    void GetServerData()
    {
        string serverData = tcpClient.GetRecvStr();

    }
    void SendServerData()
    {

    }
    private void OnApplicationQuit()
    {
        tcpClient.SocketQuit();
        Debug.Log("TcpClient¹Ø±Õ");
    }

}