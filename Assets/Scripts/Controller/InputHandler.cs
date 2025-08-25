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
    bool updateUI;

    private void Start()
    {
        InitGame();
    }

    public void InitGame()
    {
        p_references.Init();
        statesManager.Init();
        camHolder.Init(this);

        UpdatePlayerReferencesForWeapon(statesManager.weaponManager.GetCurrent());

        isInit = true;
        updateUI = true;
    }

    #region FixedUpdate
    private void FixedUpdate()
    {
        if (!isInit) return;

        delta = Time.fixedDeltaTime;
        GetInput_FixedUpdate();
        InGame_UpdateStates_FixedUpdate();
        statesManager.FixedTick(delta);
        //camHolder.FixedTick(delta);

        if (statesManager.rigid.velocity.sqrMagnitude > 0.5f)
            p_references.targetSpread.value = 45;
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

        if (updateUI)
        {
            updateUI = false;
            UpdatePlayerReferencesForWeapon(statesManager.weaponManager.GetCurrent());
            p_references.e_UpdateUI.Raise();
        }
    }
    void GetInput_Update()
    {
        aimInput = Input.GetMouseButton(1);
        shootInput = Input.GetMouseButton(0);
        pivotInput = Input.GetButtonDown(StaticStrings.Pivot);
        crouchInput = Input.GetKey(KeyCode.C);
        reloadInput = Input.GetButtonDown(StaticStrings.Reload);
    }

    void InGame_UpdateStates_Update()
    {
        if (reloadInput)
        {
            bool isReloading = statesManager.Reload();
            if (isReloading)
            {
                aimInput = false;
                shootInput = false;
                updateUI = true;
            }
        }

        statesManager.states.isAiming = aimInput;

        if (shootInput)
        {
            statesManager.states.isAiming = true;
            bool shootActual = statesManager.ShootWeapon(Time.realtimeSinceStartup);
            if (shootActual)
            {
                p_references.targetSpread.value += 15;
                updateUI = true;
            }
        }

        p_references.isAming.value = statesManager.states.isAiming;

        if (pivotInput)
            p_references.isLeftPivot.value = !p_references.isLeftPivot.value;

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

    #region ManagerFunctions
    public void UpdatePlayerReferencesForWeapon(RuntimeWeapon r)
    {
        p_references.curAmmo.value = r.curAmmo;
        p_references.curCarrying.value = r.curCarrying;
    }

    #endregion
}

public enum GamePhase
{
    inGame,
    inMenu,
}
