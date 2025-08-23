using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesManager : MonoBehaviour
{
    public ResourcesManager resourcesManager;
    public ControllerStats stats;
    public ControllerStates states;
    public InputVariables inp;
    public WeaponManager weaponManager;
    public Character character;

    [System.Serializable]
    public class InputVariables
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public Vector3 moveDirection;
        public Vector3 aimPosition;
        public Vector3 rotateDirection;
    }

    [System.Serializable]
    public class ControllerStates
    {
        public bool onGround;
        public bool isAiming;
        public bool isCrouching;
        public bool isRunning;
        public bool isInteracting;
    }

    #region References
    public Animator anim;
    public GameObject activeModel;

    [HideInInspector]
    public AnimatorHook a_hook;
    [HideInInspector]
    public Rigidbody rigid;
    [HideInInspector]
    public Collider controllerCollider;

    List<Collider> ragdollColliders = new List<Collider>();
    List<Rigidbody> ragdollRigids = new List<Rigidbody>();

    [HideInInspector]
    public LayerMask ignoreLayers;
    [HideInInspector]
    public LayerMask ignoreForGround;

    //[HideInInspector]
    //public Transform referencesParent;
    [HideInInspector]
    public Transform mTransfrom;
    public CharState curStates;

    public float delta;
    #endregion

    #region Init
    public void Init()
    {
        resourcesManager.Init();
        mTransfrom = transform;

        SetupAnimator();

        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = false;
        rigid.drag = 4;
        rigid.angularDrag = 999;
        rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        controllerCollider = GetComponent<Collider>();

        SetupRagdoll();

        ignoreLayers = ~(1 << 9);
        ignoreForGround = ~(1 << 10 | 1 << 9);

        a_hook = activeModel.GetComponent<AnimatorHook>();
        a_hook.Init(this);
        Init_WeaponManager();

        character = GetComponent<Character>();
        character.Init(this);
    }

    void SetupAnimator()
    {
        if (activeModel == null)
        {
            anim = GetComponentInChildren<Animator>();
            activeModel = anim.gameObject;
        }

        if (anim == null)
            anim = activeModel.GetComponent<Animator>();

        anim.applyRootMotion = false;
    }

    void SetupRagdoll()
    {
        Rigidbody[] rigids = activeModel.GetComponentsInChildren<Rigidbody>();
        foreach (var r in rigids)
        {
            if (r == rigid) continue;

            Collider c = r.gameObject.GetComponent<Collider>();
            c.isTrigger = true;
            ragdollRigids.Add(r);
            ragdollColliders.Add(c);
            r.isKinematic = true;
            r.gameObject.layer = 10;
        }
    }
    #endregion

    #region FixedUpdate
    public void FixedTick(float dt)
    {
        delta = dt;
        switch (curStates)
        {
            case CharState.normal:
                states.onGround = OnGround();
                if (states.isAiming)
                    MovementAiming();
                else
                    MovementNormal();
                
                RotationNormal();
                break;
            case CharState.onAir:
                rigid.drag = 0;
                states.onGround = OnGround();
                break;
            case CharState.cover:
                break;
            case CharState.vaulting:
                break;
            default:
                break;
        }
    }

    void MovementNormal()
    {
        if (inp.moveAmount > 0.05f)
            rigid.drag = 0;
        else 
            rigid.drag = 4;

        float speed = stats.walkSpeed;
        if (states.isRunning)
            speed = stats.runSpeed;

        if (states.isCrouching)
            speed = stats.crouchSpeed;

        Vector3 dir = Vector3.zero;
        dir = mTransfrom.forward * (speed * inp.moveAmount);
        rigid.velocity = dir;
    }

    void RotationNormal()
    {
        if (!states.isAiming)
            inp.rotateDirection = inp.moveDirection;

        Vector3 targetDir = inp.rotateDirection;
        targetDir.y = 0;
        
        if (targetDir == Vector3.zero)
            targetDir = mTransfrom.forward;

        Quaternion lookDir = Quaternion.LookRotation(targetDir);
        Quaternion targetRot = Quaternion.Slerp(mTransfrom.rotation, lookDir, stats.rotateSpeed * delta);
        mTransfrom.rotation = targetRot;
    }

    void MovementAiming()
    {
        float speed = stats.aimSpeed;
        Vector3 v = inp.moveDirection * speed;
        rigid.velocity = v;
    }
    #endregion

    #region Update

    float rT;
    public void Tick(float dt)
    {
        delta = dt;
        switch (curStates)
        {
            case CharState.normal:
                states.onGround = OnGround();
                HandleAnimationsAll();
                a_hook.Tick(dt);

                if (states.isInteracting)
                {
                    rT += delta;
                    if (rT > 2)
                    {
                        states.isInteracting = false;
                        rT = 0;
                    }
                }
                break;
            case CharState.onAir:
                states.onGround = OnGround();

                break;
            case CharState.cover:
                break;
            case CharState.vaulting:
                break;
            default:
                break;
        }
    }

    void HandleAnimationsAll()
    {
        anim.SetBool(StaticStrings.sprint, states.isRunning);
        anim.SetBool(StaticStrings.aiming, states.isAiming);
        anim.SetBool(StaticStrings.crouch, states.isCrouching);

        if(states.isAiming)
        {
            HandleAnimationsAiming();
        }
        else
        {
            HandleAnimationsNormal();
        }
    }

    void HandleAnimationsNormal()
    {
        if (inp.moveAmount > 0.05f)
            rigid.drag = 0;
        else
            rigid.drag = 4;

        float anim_v = inp.moveAmount;
        anim.SetFloat(StaticStrings.vertical, anim_v, 0.15f, delta);
    }

    void HandleAnimationsAiming()
    {
        float v = inp.vertical;
        float h = inp.horizontal;

        anim.SetFloat(StaticStrings.horizontal, h, 0.2f, delta);
        anim.SetFloat(StaticStrings.vertical, v, 0.2f, delta);
    }

    #endregion

    #region ManagerFunctions
    public void Init_WeaponManager()
    { 
        CreateRuntimeWeapon(weaponManager.mainWeaponId, ref weaponManager.m_weapon);
        EquipRuntimeWeapon(weaponManager.m_weapon);
    }

    public void CreateRuntimeWeapon(string id, ref RuntimeWeapon r_w_m)
    {
        Weapon w = resourcesManager.GetWeapon(id); 
        RuntimeWeapon rw = resourcesManager.runtime.WeaponToRuntimeWeapon(w);

        GameObject go = Instantiate(w.modelPrefabs);
        rw.m_instance = go;
        rw.w_actual = w;
        rw.w_hook = go.GetComponent<WeaponHook>();
        go.SetActive(false);

        Transform p = anim.GetBoneTransform(HumanBodyBones.RightHand);
        go.transform.parent = p;
        go.transform.localPosition = Vector3.zero;
        go.transform.localEulerAngles = Vector3.zero;
        go.transform.localScale = Vector3.one;

        r_w_m = rw;
    }

    public void EquipRuntimeWeapon(RuntimeWeapon rw)
    {
        rw.m_instance.SetActive(true);
        a_hook.EquipWeapon(rw);
        anim.SetFloat(StaticStrings.weaponType, rw.w_actual.weaponType);
        weaponManager.SetCurrent(rw);
    }

    public bool ShootWeapon(float t)
    {
        bool retVal = false;

        RuntimeWeapon c = weaponManager.GetCurrent();
        
        if (c.curAmmo > 0)
        {
            if (t - c.lastFired > c.w_actual.fireRate)
            {
                c.lastFired = t;
                retVal = true;
                c.ShootWeapon();
                a_hook.RecoilAnim();
            }
        }

        return retVal;
    }

    public bool Reload()
    {
        bool retVal = false;
        RuntimeWeapon c = weaponManager.GetCurrent();
        if (c.curAmmo < c.w_actual.magazineAmmo)
        {
            if (c.w_actual.magazineAmmo <= c.curCarrying)
            {
                c.curAmmo = c.w_actual.magazineAmmo;
                c.curCarrying -= c.curAmmo;
            }
            else
            {
                c.curAmmo = c.curCarrying;
                c.curCarrying = 0;
            }
            retVal = true;
            anim.CrossFade("Rifle Reload", 0.2f);
            states.isInteracting = true;
        }

        return retVal;
    }

    #endregion
    bool OnGround()
    {
        Vector3 origin = mTransfrom.position;
        origin.y += 0.6f;
        Vector3 dir = -Vector3.up;
        float dis = 0.7f;
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, dis, ignoreForGround))
        {
            Vector3 tp = hit.point;
            mTransfrom.position = tp;
            return true;
        }

        return false;
    }
 
}

public enum CharState
{
    normal,
    onAir,
    cover,
    vaulting
}
