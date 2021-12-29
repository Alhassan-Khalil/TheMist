using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour, IMovementModifier
{
    [Header("References")]
    [SerializeField] private CharacterController controller = null;
    [SerializeField] private MovementHandler movementHandler = null;

    [Header("Settings")]
    [SerializeField] private float mass = 1f;
    [SerializeField] private float drag = 5f;


    private InputHandler InputHandler;

    private bool wasGroundedLastFrame;
    public Vector3 Value { get; private set; }
    private void OnEnable() => movementHandler.AddModifier(this);
    private void OnDisable() => movementHandler.RemoveModifier(this);



    private void Update()
    {

        if (!wasGroundedLastFrame && controller.isGrounded)
        {
            Value = new Vector3(Value.x, 0f, Value.z);
        }
        wasGroundedLastFrame = controller.isGrounded;

        if (Value.magnitude < 0.2f)
        {
            Value = Vector3.zero;
        }
        Value = Vector3.Lerp(Value, Vector3.zero, drag * Time.deltaTime);
    }

    public void AddForce()
    {
        Vector3 force = Vector3.up * 15f;  
        Value += force / mass;
    }
}


