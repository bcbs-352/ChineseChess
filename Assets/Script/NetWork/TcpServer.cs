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

    // 监听接口和线程只有一个，单独拿出来
    Thread listenThread;
    Socket listenSocket, clientSocket;

    private bool isSendData = false,isClose = false;

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
        listenThread = new Thread(SocketListen);
        listenThread.IsBackground = true;
        listenThread.Start(listenSocket);

        ThreadPool.SetMaxThreads(10, 10);
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
            clientSocket = listenSocket.Accept();
            Debug.Log(clientSocket.RemoteEndPoint.ToString() + ":" + "连接成功");


            // 接收数据线程
            ThreadPool.QueueUserWorkItem(new WaitCallback(SocketReceived), clientSocket);
            //Thread recvThread = new Thread(SocketReceived);
            //recvThread.IsBackground = true;
            //recvThread.Start(clientSocket);


            // 发送数据线程
            ThreadPool.QueueUserWorkItem(new WaitCallback(SocketSend), clientSocket);
            //Thread sendThread = new Thread(SocketSend);
            //sendThread.IsBackground = true;
            //sendThread.Start(clientSocket);
        }

    }

    void SocketReceived(object inSocket)
    {
        Socket recvSocket = inSocket as Socket;
        while (true)
        {
            recvBuffer = new byte[1024 * 1024];
            recvLength = recvSocket.Receive(recvBuffer);
            if (recvLength == 0)
                break;
            recvStr = Encoding.UTF8.GetString(recvBuffer, 0, recvLength);
            Debug.Log("接收到信息:" + recvStr);

            if (isClose)
                return;
        }
    }

    void SocketSend(object inSocket)
    {
        clientSocket = inSocket as Socket;
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
                    clientSocket.Send(sendBuffer);

                sendBuffer = null;
                sendStr = null;
            }

            if (isClose)
                return;
        }
    }

    /// <summary>
    /// 中断、终止所有线程，关闭所有接口
    /// </summary>
    public void SocketQuit()
    {
        if (listenThread != null)
        {
            listenThread.Interrupt();
            listenThread.Abort();
        }
        ThreadPool.SetMaxThreads(0, 0);
        isClose = true;
        //for (int i = 0; i < sendThreadPool.Count; ++i)
        //{
        //    Debug.Log("终止线程:" + sendThreadPool[i].Name);
        //    sendThreadPool[i].Interrupt();
        //    sendThreadPool[i].Abort();
        //    recvThreadPool[i].Interrupt();
        //    recvThreadPool[i].Abort();
        //}

        if (listenSocket != null)
            listenSocket.Close();
        if (clientSocket != null)
            clientSocket.Close();
    }

    // 发送UTF8字符串
    public void SetSendStr(string str)
    {
        sendStr = str;
        isSendData = true;
    }

    // 发送字节流
    public void SetSendBytes(byte[] bytes)
    {
        sendBuffer = bytes;
        Debug.Log("字节流长度:"+bytes.Length);
        isSendData = true;
    }

}
