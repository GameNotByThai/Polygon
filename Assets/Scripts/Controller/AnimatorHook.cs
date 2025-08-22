using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHook : MonoBehaviour
{
    Animator anim;
    StatesManager statesManager;

    float m_h_weight;
    float o_h_weight;
    float l_weight;
    float b_weight;

    Transform rh_target;
    public Transform lh_target;
    Transform shoulder;
    Transform aimPivot;
    Vector3 lookDir;

    public bool onIdleDisableOh;
    public bool disable_o_h;
    public bool disable_m_h;

    RuntimeWeapon curWeapon;

    public void Init(StatesManager st)
    {
        statesManager = st;
        anim = statesManager.anim;

        shoulder = anim.GetBoneTransform(HumanBodyBones.RightShoulder).transform;
        aimPivot = new GameObject().transform;
        aimPivot.name = "AimPivot";
        aimPivot.transform.parent = statesManager.transform;
        rh_target = new GameObject().transform;
        rh_target.name = "RightHandTarget";
        rh_target.parent = aimPivot;
        statesManager.inp.aimPosition = statesManager.transform.position + transform.forward * 15;
        statesManager.inp.aimPosition.y += 1.4f;
    }

    public void EquipWeapon(RuntimeWeapon rw)
    {
        Weapon w = rw.w_actual;
        lh_target = rw.w_hook.leftHandIK;

        rh_target.localPosition = w.m_h_ik.pos;
        rh_target.localEulerAngles = w.m_h_ik.rot;
        onIdleDisableOh = rw.w_actual.onIdleDisableOh;
        curWeapon = rw;
    }

    private void OnAnimatorMove()
    {
        lookDir = statesManager.inp.aimPosition - aimPivot.position;
        HandleShoulder();
    }

    void HandleShoulder()
    {
        HandleShoulderPosition();
        HandleShoulderRotation();
    }

    void HandleShoulderPosition()
    {
        aimPivot.position = shoulder.position;

    }

    void HandleShoulderRotation()
    {
        Vector3 targetDir = lookDir;
        if (targetDir == Vector3.zero)
            targetDir = aimPivot.forward;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        aimPivot.rotation = Quaternion.Slerp(aimPivot.rotation, tr, statesManager.delta * 15);
    }

    void HandleWeights()
    {
        if (statesManager.states.isInteracting)
        {
            m_h_weight = 0;
            o_h_weight = 0;
            l_weight = 0;
            return;
        }

        float t_l_weight = 0;
        float t_m_weight = 0;

        if(statesManager.states.isAiming)
        {
            t_m_weight = 1;
            b_weight = 0.4f;
        }
        else
        {
            b_weight = .3f;
        }

        if (disable_m_h)
            t_m_weight = 0;

        if (lh_target != null)
            o_h_weight = 1;
        else
            o_h_weight = 0;

        if (disable_o_h)
            o_h_weight = 0;

        Vector3 ld = statesManager.inp.aimPosition - statesManager.mTransfrom.position;
        float angle = Vector3.Angle(statesManager.mTransfrom.forward, ld);
        if (angle < 76)
            t_l_weight = 1;
        else
            t_l_weight = 0;

        if (angle > 45)
            t_m_weight = 0;

        if (!statesManager.states.isAiming)
        {
            if (onIdleDisableOh)
                o_h_weight = 0;
        }

        l_weight = Mathf.Lerp(l_weight, t_l_weight, statesManager.delta * 3);
        m_h_weight = Mathf.Lerp(m_h_weight, t_m_weight, statesManager.delta * 9);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        HandleWeights();

        anim.SetLookAtWeight(l_weight, b_weight, 1, 1, 1);
        anim.SetLookAtPosition(statesManager.inp.aimPosition);

        if (lh_target != null)
        {
            UpdateIK(AvatarIKGoal.LeftHand, lh_target, o_h_weight);
        }

        UpdateIK(AvatarIKGoal.RightHand, rh_target, m_h_weight);
    }

    void UpdateIK(AvatarIKGoal goal, Transform t, float w)
    {
        anim.SetIKPositionWeight(goal, w);
        anim.SetIKRotationWeight(goal, w);
        anim.SetIKPosition(goal, t.position);
        anim.SetIKRotation(goal, t.rotation);
    }

    public void Tick(float dt)
    {
        RecoilActual();
    }

    #region Recoil
    float recoilT;
    Vector3 offsetPosition;
    Vector3 offsetRotation;
    Vector3 basePosition;
    Vector3 baseRotation;
    bool recoilIsInit;

    public void RecoilAnim()
    {
        if(!recoilIsInit)
            recoilIsInit = true;
        recoilT = 0;
        offsetPosition= Vector3.zero;

    }

    public void RecoilActual()
    {
        if (recoilIsInit)
        {
            recoilT += statesManager.delta * 3;
            if (recoilT > 1)
            {
                recoilT = 1;
                recoilIsInit = false;
            }

            offsetPosition = Vector3.forward * curWeapon.w_actual.recoilZ.Evaluate(recoilT);
            offsetRotation = Vector3.right * 90 * curWeapon.w_actual.recoilY.Evaluate(recoilT);

            rh_target.localPosition = basePosition + offsetPosition;
            rh_target.localEulerAngles = baseRotation + offsetRotation;
        }
    }
    #endregion
}
