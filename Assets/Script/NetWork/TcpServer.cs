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
    /// ��ʼ������Socket
    /// </summary>
    public void SocketConnect()
    {
        Debug.Log("��ʼ����");

        // �������������Socket
        listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /*
        // AddressList[2]:����IPv4��ַ��IPAddress��ʽ
        IPHostEntry IPHost = Dns.GetHostEntry(Dns.GetHostName());
        string ip = Convert.ToString(IPHost.AddressList[2]);
        ipStr = ip;
        */

        // ����Socket��IP��ַ�Ͷ˿�
        //ipEnd = new IPEndPoint(IPHost.AddressList[2], port);
        serverIP = IPAddress.Parse(ipStr);
        ipEnd = new IPEndPoint(serverIP, port);
        listenSocket.Bind(ipEnd);

        // ��ʼ������������� 5 ̨�ͻ���
        listenSocket.Listen(5);

        // �������������̣߳�����linstenSocket����
        Thread linstenThread = new Thread(SocketListen);
        linstenThread.IsBackground = true;
        linstenThread.Start(listenSocket);
    }


    /// <summary>
    /// �����������ȴ��ͻ������ӣ���������ͻ���ͨ�ŵ�Socket
    /// </summary>
    /// <param name="obj"></param>
    void SocketListen(object inSocket)
    {
        Socket listenSocket = inSocket as Socket;
        while (true)
        {
            //  �ȴ��ͻ�������
            connectSocket = listenSocket.Accept();
            Debug.Log(connectSocket.RemoteEndPoint.ToString() + ":" + "���ӳɹ�");

            // ���������߳�
            Thread recvThread = new Thread(SocketReceived);
            recvThread.IsBackground = true;
            recvThread.Start(connectSocket);
            // ���������߳�
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
            Debug.Log("���յ���Ϣ:" + recvStr);

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
                    Debug.Log("��������:" + sendStr);
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
