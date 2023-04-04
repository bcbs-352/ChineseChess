using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class TcpClient : MonoBehaviour
{
    string ipStr = "127.0.0.1"; //����IP
    int port = 8088;

    Socket serverSocket;
    IPAddress serverIP;     // ������IP
    IPEndPoint ipEnd;       // IPEndPoint:�������ս���ʾΪIP��ַ�Ͷ˿ں�
    byte[] recvBuffer,sendBuffer;
    string recvStr, sendStr;
    int recvLength;
    Thread connectThread;   // �����߳�

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
        // �����߳��ж���Soeket����
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
        Debug.Log("Socket��ʼ�����");

        serverSocket.Connect(ipEnd);
        recvLength = serverSocket.Receive(recvBuffer);
        recvStr = Encoding.ASCII.GetString(recvBuffer, 0, recvLength);  // ��byte[] תΪ string

    }
    void SocketRecv()
    {
        SocketConnect();
        // ���̲߳��Ͻ�������
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
        // �߳���
        lock (this)
        {
            ret = recvStr;
        }
        return ret;
    }

    /// <summary>
    /// �ж��̣߳��ر�Socket
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
