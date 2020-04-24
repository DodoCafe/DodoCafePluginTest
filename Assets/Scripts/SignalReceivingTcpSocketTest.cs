using DodoCafe.Networking.Sockets;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SignalReceivingTcpSocketTest : MonoBehaviour
{
    public Text Text;
    public string ServerApplicationIpv4;
    public int ServerApplicationPortNumber;

    private void Start()
    {
        TestAsynchronousOperation();
    }

    private void TestAsynchronousOperation()
    {
        Thread workerThread = new Thread( async () =>
        {
            await TestSignalReceivingTcpSocketAsync();
            UnityMainThreadDispatcher.Instance().Enqueue( NotifyWorkerThreadCompletion() );
        } );
        workerThread.Start();
        Text.text = "Doing something else while concurrently testing signal receiving TCP socket.";
    }

    private async Task TestSignalReceivingTcpSocketAsync()
    {
        var socket = new CSignalReceivingTcpSocket();
        await socket.ConnectAsync( ServerApplicationIpv4, ServerApplicationPortNumber );
        await socket.ReceiveSignalAsync();
        socket.Disconnect();
    }

    private IEnumerator NotifyWorkerThreadCompletion()
    {
        Text.text = "Connected, received signal, and disconnected from the destined server application.";
        yield return null;
    }
}