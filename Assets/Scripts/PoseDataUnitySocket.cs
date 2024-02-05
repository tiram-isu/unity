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
    int bufferSize = 4096;  // 32,768 bytes - max file people will be detected, equals ~ 15kByte

    public void StartServer()
    {
        server = new TcpListener(IPAddress.Parse(ip), connectionPort);
        server.Start();
        Debug.Log("Server started");
        client = server.AcceptTcpClient(); // makes unity crash sometimes
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
            //stream.Read(buffer, 0, buffer.Length);
            // float[] floatArray = new float[byteCount / 4];
            int maxKeyPoints = 150; // ensures, that only keypoints for two people are being used (Body 25)
            float[] floatArray = new float[maxKeyPoints];
            int maxByteCount = 4 * maxKeyPoints; // one float = 4 byte, we need 150 float keypoints from the buffer filled with floats as raw bytes
            for (int i = 0; i < maxByteCount; i += 4)
            {
                floatArray[i / 4] = BitConverter.ToSingle(buffer, i);
            }
            PoseDataAccess.SetPoseData(floatArray);

            /*String output = "";
            foreach (var item in floatArray)
            {
                output += item.ToString() + "; ";
                //Debug.Log(item.ToString());
            }
            Debug.Log("output: " + output);*/

            //Debug.Log("[{0}]", string.Join(", ", PoseDataAccess.GetPoseData()));
            //Debug.Log(PoseDataAccess.GetPoseData() );

        }
        Console.WriteLine("Client disconnected");
    }

}


