using DodoCafe.Networking.Sockets;
using System;
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
        TaskScheduler unityMainThreadTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        TestSignalReceivingTcpSocketAsync().ContinueWith( new Action< Task >( UpdateTextAfterTestingSignalReceivingTcpSocket ), unityMainThreadTaskScheduler );
        Text.text = "Doing something else while concurrently testing signal receiving TCP socket.";
    }

    private async Task TestSignalReceivingTcpSocketAsync()
    {
        var socket = new CSignalReceivingTcpSocket();
        await socket.ConnectAsync( ServerApplicationIpv4, ServerApplicationPortNumber );
        await socket.ReceiveSignalAsync();
        socket.Disconnect();
    }

    private void UpdateTextAfterTestingSignalReceivingTcpSocket( Task antecedentTask )
    {
        Text.text = "Connected, received signal, and disconnected from the destined server application.";
    }
}