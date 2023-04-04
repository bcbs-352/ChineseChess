using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class TcpClient : MonoBehaviour
{
    string ipStr = "127.0.0.1"; //本机IP
    int port = 8088;

    Socket serverSocket;
    IPAddress serverIP;     // 服务器IP
    IPEndPoint ipEnd;       // IPEndPoint:将网络终结点表示为IP地址和端口号
    byte[] recvBuffer,sendBuffer;
    string recvStr, sendStr;
    int recvLength;
    Thread connectThread;   // 连接线程

    public void InitSocket()
    {
        serverIP = IPAddress.Parse(ipStr);
        ipEnd = new IPEndPoint(serverIP, port);

        recvBuffer = new byte[1024];

        connectThread = new Thread(new ThreadStart(SocketRecv));
        connectThread.Start();
    }
    public void SocketSend(string sendStr)
    {
        sendBuffer = new byte[1024];
        sendBuffer = Encoding.ASCII.GetBytes(sendStr);
        serverSocket.Send(sendBuffer, sendBuffer.Length, SocketFlags.None);
    }

    void SocketConnect()
    {
        if (serverSocket != null)
            serverSocket.Close();
        // 在子线程中定义Soeket类型
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
        Debug.Log("Socket初始化完成");

        serverSocket.Connect(ipEnd);
        recvLength = serverSocket.Receive(recvBuffer);
        recvStr = Encoding.ASCII.GetString(recvBuffer, 0, recvLength);  // 将byte[] 转为 string

    }
    void SocketRecv()
    {
        SocketConnect();
        // 该线程不断接收数据
        while (true)
        {
            recvBuffer = new byte[1024];
            recvLength = serverSocket.Receive(recvBuffer);
            if (recvLength == 0)
            {
                SocketConnect();
                continue;
            }
            recvStr = Encoding.ASCII.GetString(recvBuffer, 0, recvLength);
            
        }
    }
    public string GetRecvStr()
    {
        string ret;
        // 线程锁
        lock (this)
        {
            ret = recvStr;
        }
        return ret;
    }

    /// <summary>
    /// 中断线程，关闭Socket
    /// </summary>
    public void SocketQuit()
    {
        if (connectThread != null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }

        if (serverSocket != null)
        {
            serverSocket.Close();
        }
    }

}
