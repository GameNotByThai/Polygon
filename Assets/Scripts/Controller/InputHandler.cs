using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    float horizontal;
    float vertical;

    bool aimInput;
    bool sprintInput;
    bool shootInput;
    bool crouchInput;
    bool reloadInput;
    bool switchInput;
    bool pivotInput;

    bool isInit;

    float delta;

    public StatesManager statesManager;
    public CameraHandler camHolder;
    public PlayerReferences p_references;

    private void Start()
    {
        InitGame();
    }

    public void InitGame()
    {
        statesManager.Init();
        camHolder.Init(this);
        isInit = true;
    }

    #region FixedUpdate
    private void FixedUpdate()
    {
        if (!isInit) return;

        delta = Time.fixedDeltaTime;    
        GetInput_FixedUpdate();
        InGame_UpdateStates_FixedUpdate();
        statesManager.FixedTick(delta);
        camHolder.FixedTick(delta);

        if (statesManager.rigid.velocity.sqrMagnitude > 0)
            p_references.targetSpread.value = 45;
        else
            p_references.targetSpread.value = 15;
    }

    void GetInput_FixedUpdate()
    {
        vertical = Input.GetAxis(StaticStrings.Vertical);
        horizontal = Input.GetAxis(StaticStrings.Horizontal);
    }

    void InGame_UpdateStates_FixedUpdate()
    {
        statesManager.inp.horizontal = horizontal;
        statesManager.inp.vertical = vertical;

        statesManager.inp.moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        Vector3 moveDir = camHolder.mTransform.forward * vertical;
        moveDir += camHolder.mTransform.right * horizontal;
        moveDir.Normalize();
        statesManager.inp.moveDirection = moveDir;

        statesManager.inp.rotateDirection = camHolder.mTransform.forward;

        
    }
    #endregion

    public bool debugAiming;

    #region Update
    private void Update()
    {
        if (!isInit) return;

        delta = Time.deltaTime;
        GetInput_Update();
        AimPosition();
        InGame_UpdateStates_Update();

        if (debugAiming) 
            statesManager.states.isAiming = true;  

        statesManager.Tick(delta);
    }
    void GetInput_Update()
    {
        aimInput = Input.GetMouseButton(1);
        crouchInput = Input.GetKey(KeyCode.C);
    }

    void InGame_UpdateStates_Update()
    {
        statesManager.states.isAiming = aimInput;
        statesManager.states.isCrouching = crouchInput;
    }

    void AimPosition()
    {
        Ray ray = new Ray(camHolder.camTrans.position, camHolder.camTrans.forward);
        statesManager.inp.aimPosition = ray.GetPoint(30);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, statesManager.ignoreLayers))
        {
            statesManager.inp.aimPosition = hit.point;
        }
    }

    #endregion
}

public enum GamePhase
{
    inGame,
    inMenu,
}
