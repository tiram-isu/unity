using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;


public class LeftPlayerPaddle : Paddle
{
    private Vector2 direction;
    private float normalizedRelativePosY;

    private void Start()
    {
        /*UnityServer server = new UnityServer();
        Debug.Log("Starting server...");
        server.StartServer();*/
    }

    private void Update()
    {
        float[] leftPerson = PoseDataAccess.GetLeftPersonPoseData();
        float leftHand = leftPerson[22];
        float leftShoulder = leftPerson[16];
        float hip = leftPerson[25];
        float armLength = Math.Abs(hip - leftShoulder) / 2;
        if (armLength == 0)
            return;
        normalizedRelativePosY = (leftShoulder - leftHand) / armLength;
        direction = new Vector2(0, adjustedVelocity(normalizedRelativePosY));
    }

    private float adjustedVelocity(float val)
    {
        return (float)(Math.Pow(val, 3) * paddleSpeed);
    }

    private void FixedUpdate()
    {
        if (direction.sqrMagnitude != 0)
        {
            rb.AddForce(direction * speed);
        }
    }

}
