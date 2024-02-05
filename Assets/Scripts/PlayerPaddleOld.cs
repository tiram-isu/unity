using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;


public class PlayerPaddleOld : Paddle
{    
    private Vector2 direction;
    private PoseDataAccess poseDataAccess;
    
    private void Start()
    {
        UnityServer server = new UnityServer();
        Debug.Log("Starting server...");
        server.StartServer();
    }

    /*private void Update()
    {
        float[] poseData = PoseDataAccess.GetPoseData();
        float shoulder = poseData[16];
        float hip = poseData[25];
        float leftHand = poseData[22];
        float anchorPoint = (shoulder - hip) / 2 + hip;

        direction = new Vector2(0, (leftHand - shoulder) * (-14));
        if (direction.y < -8)
            direction = new Vector2(0, -8);
        if (direction.y > 8)
            direction = new Vector2(0, 8);

    }*/

    private void FixedUpdate()
    {
        if (direction.sqrMagnitude != 0) {
            rb.AddForce(direction * speed);
        }
    }

}

public class Locals
{
    private static readonly object LockObject = new object();
    private static float[] poseData = new float[75];

    public static float[] GetPoseData()
    {
        lock (LockObject)
        {
            // Return a copy to prevent external modification of the original array
            return (float[])poseData.Clone();
        }
    }

    public static void SetPoseData(float[] newData)
    {
        lock (LockObject)
        {
            // Copy the new data into the poseData array
            Array.Copy(newData, poseData, 75);
        }
    }
}

public class UnityServer
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
            stream.Read(buffer, 0, buffer.Length);
            float[] floatArray = new float[byteCount / 4];
            for (int i = 0; i < byteCount; i += 4)
            {
                floatArray[i / 4] = BitConverter.ToSingle(buffer, i);
            }
            Locals.SetPoseData(floatArray);

        }
        Console.WriteLine("Client disconnected");
    }

}


