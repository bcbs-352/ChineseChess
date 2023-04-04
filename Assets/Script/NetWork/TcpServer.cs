using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Text;
using System;

public class TcpServer : MonoBehaviour
{
    string ipStr = "127.0.0.1";
    int port = 8088;

    IPAddress serverIP;
    IPEndPoint ipEnd;
    byte[] recvBuffer, sendBuffer;
    string recvStr, sendStr;
    int recvLength;
    Thread connectThread;
    Socket listenSocket, connectSocket;

    private bool isSendData = false;

    private void Awake()
    {
        //SocketConnect();
    }

    /// <summary>
    /// 初始化监听Socket
    /// </summary>
    public void SocketConnect()
    {
        Debug.Log("开始监听");

        // 创建负责监听的Socket
        listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /*
        // AddressList[2]:本机IPv4地址，IPAddress格式
        IPHostEntry IPHost = Dns.GetHostEntry(Dns.GetHostName());
        string ip = Convert.ToString(IPHost.AddressList[2]);
        ipStr = ip;
        */

        // 监听Socket绑定IP地址和端口
        //ipEnd = new IPEndPoint(IPHost.AddressList[2], port);
        serverIP = IPAddress.Parse(ipStr);
        ipEnd = new IPEndPoint(serverIP, port);
        listenSocket.Bind(ipEnd);

        // 开始监听，最大连接 5 台客户端
        listenSocket.Listen(5);

        // 创建监听函数线程，传入linstenSocket对象
        Thread linstenThread = new Thread(SocketListen);
        linstenThread.IsBackground = true;
        linstenThread.Start(listenSocket);
    }


    /// <summary>
    /// 监听函数，等待客户端连接，并创建与客户端通信的Socket
    /// </summary>
    /// <param name="obj"></param>
    void SocketListen(object inSocket)
    {
        Socket listenSocket = inSocket as Socket;
        while (true)
        {
            //  等待客户端连接
            connectSocket = listenSocket.Accept();
            Debug.Log(connectSocket.RemoteEndPoint.ToString() + ":" + "连接成功");

            // 接收数据线程
            Thread recvThread = new Thread(SocketReceived);
            recvThread.IsBackground = true;
            recvThread.Start(connectSocket);
            // 发送数据线程
            Thread sendThread = new Thread(SocketSend);
            sendThread.IsBackground = true;
            sendThread.Start(connectSocket);
        }

    }

    void SocketReceived(object inSocket)
    {
        Socket sendSocket = inSocket as Socket;
        while (true)
        {
            lock (this)
            {
                recvBuffer = new byte[1024 * 1024];
                recvLength = sendSocket.Receive(recvBuffer);
                if (recvLength == 0)
                    break;
            }
            recvStr = Encoding.UTF8.GetString(recvBuffer, 0, recvLength);
            Debug.Log("接收到信息:" + recvStr);

        }
    }

    void SocketSend(object inSocket)
    {
        connectSocket = inSocket as Socket;
        while (true)
        {
            if (isSendData)
            {
                isSendData = false;
                if (sendStr != null && sendStr.Length > 0)
                {
                    sendBuffer = Encoding.UTF8.GetBytes(sendStr);
                    Debug.Log("发送数据:" + sendStr);
                }
                if (sendBuffer != null && sendBuffer.Length > 0)
                    connectSocket.Send(sendBuffer);

                sendBuffer = null;
                sendStr = null;
            }

        }
    }

    public void SocketQuit()
    {
        if (listenSocket != null)
            listenSocket.Close();
        if (connectSocket != null)
            connectSocket.Close();

        if(connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
    }

    public void SetSendStr(string str)
    {
        sendStr = str;
        isSendData = true;
    }
    public void SetSendBytes(byte[] bytes)
    {
        sendBuffer = bytes;
        isSendData = true;
    }

}
