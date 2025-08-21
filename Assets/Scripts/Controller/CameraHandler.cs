using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform camTrans;
    public Transform target;
    public Transform pivot;
    public Transform mTransform;
    public bool leftPivot;
    float delta;

    public float mouseX;
    public float mouseY;
    public float smoothX;
    public float smoothY;
    float smoothXvelocity;
    float smoothYvelocity;
    float lookAngle;
    float tiltAngle;

    public CameraValues values;

    StatesManager statesManager;

    public void Init(InputHandler inp)
    {
        mTransform = transform;
        statesManager = inp.statesManager;
        target = statesManager.mTransfrom;

    }

    public void FixedTick(float dt)
    {
        delta = dt;

        if (target == null) return;

        HandlePositions();
        HandleRotation();

        float speed = values.moveSpeed;
        if (statesManager.states.isAiming)
            speed = values.aimSpeed;

        Vector3 targetPosition = Vector3.Lerp(mTransform.position, target.position, delta * speed);
        mTransform.position = targetPosition;
    }

    void HandlePositions()
    {
        float targetX = values.normalX;
        float targetY = values.normalY;
        float targetZ = values.normalZ;

        if (statesManager.states.isCrouching)
            targetY = values.crouchY;

        if (statesManager.states.isAiming)
        {
            targetX = values.aimX;
            targetZ = values.aimZ;
        }

        if (leftPivot)
            targetX = -targetX;

        Vector3 newPivotPosition = pivot.localPosition;
        newPivotPosition.x = targetX;
        newPivotPosition.y = targetY;

        Vector3 newCamPosion = camTrans.localPosition;
        newCamPosion.z = targetZ;

        float t = delta * values.adaptSpeed;
        pivot.localPosition = Vector3.Lerp(pivot.localPosition, newPivotPosition, t);
        camTrans.localPosition = Vector3.Lerp(camTrans.localPosition, newCamPosion, t);   
    }

    void HandleRotation()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        if (values.turnSmooth > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, mouseX, ref smoothXvelocity, values.turnSmooth);
            smoothY = Mathf.SmoothDamp(smoothY, mouseY, ref smoothYvelocity, values.turnSmooth);
        }
        else
        {
            smoothX = mouseX;
            smoothY = mouseY;
        }

        lookAngle += smoothX * values.yRotateSpeed;
        Quaternion targetRot = Quaternion.Euler(0, lookAngle, 0);
        mTransform.rotation = targetRot;

        tiltAngle -= smoothY * values.xRotateSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, values.minAngle, values.maxAngle);
        pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
    }
}
