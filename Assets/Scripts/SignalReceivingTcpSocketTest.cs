using DodoCafe.Networking.Sockets;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SignalReceivingTcpSocketTest : MonoBehaviour
{
    public Text MainText;
    public Text ServerApplicationIpv4Text;
    public int ServerApplicationPortNumber;

    private string m_strText;

    public void TestConcurrency()
    {
        m_strText = null;
        TestSignalReceivingTcpSocketAsync();
        if ( m_strText == null )
        {
            Interlocked.Exchange( ref m_strText, "Doing something else while concurrently testing signal receiving TCP socket." );
        }
    }

    private async Task TestSignalReceivingTcpSocketAsync()
    {
        try
        {
            var kSocket = new CSignalReceivingTcpSocket();
            await kSocket.ConnectAsync( ServerApplicationIpv4Text.text, ServerApplicationPortNumber );
            await kSocket.ReceiveSignalAsync();
            kSocket.Disconnect();
            Interlocked.Exchange( ref m_strText, "Connected, received signal, and disconnected from the destined server application." );
        }
        catch ( Exception kException )
        {
            Interlocked.Exchange( ref m_strText, kException.Message );
        }
    }

    private void Update()
    {
        MainText.text = m_strText;
    }
}