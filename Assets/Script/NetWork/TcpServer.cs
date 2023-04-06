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

    // �����ӿں��߳�ֻ��һ���������ó���
    Thread listenThread;
    Socket listenSocket, clientSocket;

    private bool isSendData = false,isClose = false;

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
        listenThread = new Thread(SocketListen);
        listenThread.IsBackground = true;
        listenThread.Start(listenSocket);

        ThreadPool.SetMaxThreads(10, 10);
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
            clientSocket = listenSocket.Accept();
            Debug.Log(clientSocket.RemoteEndPoint.ToString() + ":" + "���ӳɹ�");


            // ���������߳�
            ThreadPool.QueueUserWorkItem(new WaitCallback(SocketReceived), clientSocket);
            //Thread recvThread = new Thread(SocketReceived);
            //recvThread.IsBackground = true;
            //recvThread.Start(clientSocket);


            // ���������߳�
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
            Debug.Log("���յ���Ϣ:" + recvStr);

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
                    Debug.Log("��������:" + sendStr);
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
    /// �жϡ���ֹ�����̣߳��ر����нӿ�
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
        //    Debug.Log("��ֹ�߳�:" + sendThreadPool[i].Name);
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

    // ����UTF8�ַ���
    public void SetSendStr(string str)
    {
        sendStr = str;
        isSendData = true;
    }

    // �����ֽ���
    public void SetSendBytes(byte[] bytes)
    {
        sendBuffer = bytes;
        Debug.Log("�ֽ�������:"+bytes.Length);
        isSendData = true;
    }

}
