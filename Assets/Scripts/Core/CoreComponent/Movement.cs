using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public CharacterController controller { get; private set; }
    [SerializeField]
    private PlayerData playerData = default;
    
    
    [HideInInspector]
    public Vector3 moveDirection= Vector3.zero;
    Vector3 jumpedDir;
    public Vector3 jump = Vector3.zero;
    private float jumpPower;


    protected override void Awake()
    {
        base.Awake();
        controller = GetComponentInParent<CharacterController>();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }



    #region set fun 

    public void Move(Vector3 dir,bool sprint,bool crouching)
    {
        float speed = (!sprint) ? playerData.walkSpeed : playerData.runSpeed;
        if (crouching) speed = playerData.crouchSpeed;

        if (core.CollisionSenses.Ground)
        {
            moveDirection = dir * speed;
            UpdateJump();
        }
        else
        {
            jumpedDir += dir * Time.fixedDeltaTime * jumpPower * 2f;
            jumpedDir = Vector3.ClampMagnitude(jumpedDir, jumpPower);
            moveDirection.x = jumpedDir.x;
            moveDirection.z = jumpedDir.z;
        }
        // Apply gravity
        if (playerData.applyGravity)
        {
           moveDirection.y += playerData.Gravity * Time.deltaTime;
        }
        // Move the controller, and set grounded true or false depending on whether we're standing on something
        controller.Move(moveDirection * Time.deltaTime);

        //Debug.Log(moveDirection);
    }

    public void Jump(Vector3 dir)
    {
        jump = Vector3.up * 1;
        //Move(dir, false, false);
    }
    public void Jump(Vector3 dir, float mult)
    {
        jump = dir * mult;
    }
    public void UpdateJump()
    {
        if (jump != Vector3.zero)
        {
            Vector3 dir = (jump * playerData.jumpSpeed);
            if (dir.x != 0) moveDirection.x = dir.x;
            if (dir.y != 0) moveDirection.y = dir.y;
            if (dir.z != 0) moveDirection.z = dir.z;

            Vector3 move = moveDirection;
            jumpedDir = move; move.y = 0;
            jumpPower = Mathf.Min(move.magnitude, playerData.jumpSpeed);
            jumpPower = Mathf.Max(jumpPower, playerData.walkSpeed);
        }
        else
            jumpedDir = Vector3.zero;
        jump = Vector3.zero;
    }




    #endregion
}
