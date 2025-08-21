using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Controller/CameraValues")]
public class CameraValues : ScriptableObject
{
    public float turnSmooth = .1f;
    public float moveSpeed = 9;
    public float aimSpeed = 25;
    public float yRotateSpeed = 16;
    public float xRotateSpeed = 16;
    public float minAngle = -35;
    public float maxAngle = 35;
    public float normalZ = -3f;
    public float normalX;
    public float aimZ = -1f;
    public float aimX = 0;
    public float normalY;
    public float crouchY;
    public float adaptSpeed = 9;
}
