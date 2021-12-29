using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    #region check transform
    public Transform GroundCheck
    {
        get => GenericsNotImplemntedError<Transform>.TryGet(groundCheck, core.transform.parent.name);
        private set => groundCheck = value;
    }




    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }



    [SerializeField] private Transform groundCheck = default;
    [SerializeField] private float groundCheckRadius = default;
    [SerializeField] private LayerMask whatIsGround = default;

    #endregion


    #region check fun
/*    public bool Ground
    {
        get => core.Movement.controller.isGrounded;
    }*/
    public bool Ground
    {
        get => Physics.CheckSphere(GroundCheck.position, groundCheckRadius, whatIsGround);
    }

    #endregion


}
