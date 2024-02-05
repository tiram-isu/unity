using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;
using System;

public class PoseDataAccess : MonoBehaviour
{
    public PoseDataUnitySocket socket;
    private static readonly object LockObject = new object();
    private static float[] poseData = new float[150];

    private void Start()
    {
        PoseDataUnitySocket socket = new PoseDataUnitySocket();
        Debug.Log("Starting server...");
        socket.StartServer();
    }

    public static float[] GetRightPersonPoseData()
    {
        lock (LockObject)
        {
            float[] poseData = GetPoseData();
            float headXPersonA = poseData[0];
            float headXPersonB = poseData[75];
            float[] rightPersonPoseData = new float[75];

            if (headXPersonA < headXPersonB)
            {
                Array.Copy(poseData, 0, rightPersonPoseData, 0, 75);
                
            }
            else
            {
                Array.Copy(poseData, 75, rightPersonPoseData, 0, 75);
            }
            return rightPersonPoseData;
        }
    }

    public static float[] GetLeftPersonPoseData()
    {
        lock (LockObject)
        {
            float[] poseData = GetPoseData();
            float headXPersonA = poseData[0];
            float headXPersonB = poseData[75];
            float[] leftPersonPoseData = new float[75];

            if (headXPersonA > headXPersonB)
            {
                Array.Copy(GetPoseData(), 0, leftPersonPoseData, 0, 75);
            }
            else
            {
                Array.Copy(GetPoseData(), 75, leftPersonPoseData, 0, 75);
            }
            return leftPersonPoseData;
        }
    }

    private static float[] GetPoseData()
    {
        // Return a copy to prevent external modification of the original array
        return (float[])poseData.Clone();
    }

    public static void SetPoseData(float[] newData)
    {
        lock (LockObject)
        {
            // Copy the new data into the poseData array
            Array.Copy(newData, poseData, 150);
        }
    }

}
