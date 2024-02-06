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

    private void Update()
    {
        float[] rightPerson = PoseDataAccess.GetRightPersonPoseData(); // get all keypoints of the rightmost person 
        
        float rightHand = rightPerson[13]; // get coordinates of right hand
        float rightShoulder = rightPerson[7]; // get coordinates of right shoulder
        float hip = rightPerson[25]; // get coordinates of hip
        
        float armLength = Math.Abs(hip - rightShoulder) / 2; // calculate approximate arm length as distance from hip to shoulder
        
        if (armLength == 0)
            return; // return if there is no data yet
            
        normalizedRelativePosY = (rightShoulder - rightHand) / armLength; // normalize the distance between shoulder and hand with the arm length
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
