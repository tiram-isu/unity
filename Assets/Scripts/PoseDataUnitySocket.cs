using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System;


public class PoseDataUnitySocket 
{
    TcpListener server;
    int connectionPort = 13000;
    string ip = "127.0.0.1";
    public TcpClient client;
    int bufferSize = 4096;  // 32,768 bytes - max size of pose data for two detected people

    public void StartServer()
    {
        server = new TcpListener(IPAddress.Parse(ip), connectionPort);
        server.Start();
        Debug.Log("Server started");
        client = server.AcceptTcpClient();
        Debug.Log("Client connected");
        Thread thread = new Thread(() => StartClientConnection());
        thread.Start();
    }

    public void StopServer()
    {
        server.Stop();
    }

    private void StartClientConnection()
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[bufferSize];
        int byteCount;

        while ((byteCount = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            int maxKeyPoints = 150; // limit keypoints to two people (Body 25 -> 75 values per person), any more will be discarded
            float[] floatArray = new float[maxKeyPoints]; // create empty array with 150 values
            int maxByteCount = 4 * maxKeyPoints; // one float = 4 byte
            
            for (int i = 0; i < maxByteCount; i += 4)
            {
                floatArray[i / 4] = BitConverter.ToSingle(buffer, i); // convert bytes to float and write in array
            }
            
            PoseDataAccess.SetPoseData(floatArray); // update poseData array for both players
        }
        Console.WriteLine("Client disconnected");
    }

}


