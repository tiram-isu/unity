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

    private void Update()
    {
        float[] leftPerson = PoseDataAccess.GetLeftPersonPoseData(); // get all keypoints of the leftmost person 
        
        float leftHand = leftPerson[22]; // get coordinates of left hand
        float leftShoulder = leftPerson[16]; // get coordinates of left shoulder
        float hip = leftPerson[25]; // get coordinates of hip
        
        float armLength = Math.Abs(hip - leftShoulder) / 2; // calculate approximate arm length as distance from hip to shoulder
        
        if (armLength == 0)
            return; // return if there is no data yet
            
        normalizedRelativePosY = (leftShoulder - leftHand) / armLength; // normalize the distance between shoulder and hand with the arm length
        direction = new Vector2(0, adjustedVelocity(normalizedRelativePosY)); // set paddle direction
    }

    private float adjustedVelocity(float val)
    {
        return (float)(Math.Pow(val, 3) * paddleSpeed); // adjust the velocity by using a cubic function
    }

    private void FixedUpdate()
    {
        if (direction.sqrMagnitude != 0)
        {
            rb.AddForce(direction * speed);
        }
    }

}
