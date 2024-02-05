using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;


public class RightPlayerPaddle : Paddle
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
        float[] rightPerson = PoseDataAccess.GetRightPersonPoseData();
        float rightHand = rightPerson[13];
        float rightShoulder = rightPerson[7];
        float hip = rightPerson[25];
        float armLength = Math.Abs(hip - rightShoulder) / 2;
        if (armLength == 0)
            return;
        normalizedRelativePosY = (rightShoulder - rightHand) / armLength;
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