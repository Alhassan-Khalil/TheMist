using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInputProcessor : MonoBehaviour, IMovementModifier
{
    [Header("References")]
    [SerializeField] private CharacterController controller = null;
    [SerializeField] private MovementHandler movementHandler = null;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float acceleration = 0.1f;


    private float currentSpeed;

    private Vector3 previousVelocity;
    private Vector2 previousInputDirection;

    private Transform mainCameraTransform;

    public Vector3 Value { get; private set; }

    private void OnEnable() => movementHandler.AddModifier(this);

    private void OnDisable() => movementHandler.RemoveModifier(this);

    private void Update() => Move();

    public void SetMovementInput(Vector2 input)
    {
        previousInputDirection = input;
    }

    private void Move()
    {
        float targetSpeed = movementSpeed * previousInputDirection.magnitude;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);


        Vector3 movementDirection;

        if (targetSpeed != 0f)
        {
            movementDirection = transform.forward * previousInputDirection.y + transform.right * previousInputDirection.x;
        }
        else 
        { 
            movementDirection = previousVelocity.normalized;
        }
        Value = movementDirection* currentSpeed;

        previousVelocity = new Vector3(controller.velocity.x, 0f, controller.velocity.z);
        currentSpeed =previousVelocity.magnitude;
    }
}
