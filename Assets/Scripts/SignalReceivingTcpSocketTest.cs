using DodoCafe.Networking.Sockets;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SignalReceivingTcpSocketTest : MonoBehaviour
{
    public Text Text;
    public string ServerApplicationIpv4;
    public int ServerApplicationPortNumber;

    private string m_strText;

    private void Start()
    {
        TestConcurrency();
    }

    private void TestConcurrency()
    {
        TestSignalReceivingTcpSocketAsync();
        Interlocked.Exchange( ref m_strText, "Doing something else while concurrently testing signal receiving TCP socket." );
    }

    private async Task TestSignalReceivingTcpSocketAsync()
    {
        try
        {
            var socket = new CSignalReceivingTcpSocket();
            await socket.ConnectAsync( ServerApplicationIpv4, ServerApplicationPortNumber );
            await socket.ReceiveSignalAsync();
            socket.Disconnect();
            Interlocked.Exchange( ref m_strText, "Connected, received signal, and disconnected from the destined server application." );
        }
        catch ( Exception kException )
        {
            Interlocked.Exchange( ref m_strText, kException.Message );
        }
    }

    private void Update()
    {
        Text.text = m_strText;
    }
}