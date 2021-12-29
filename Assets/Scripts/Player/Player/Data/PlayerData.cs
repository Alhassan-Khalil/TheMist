using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerDate", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("word")]
    public bool applyGravity = true;
    public float Gravity =-9.81f;
    public float antiBumpFactor = .75f;
    public float groundedPullMagnitude = 5f;
    public float drag = 5f;
    
    [Header("Move state")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2f;
    public float acceleration = 5f;

    [Header("jump state")]
    //public float JumpFoce = 3f;
    public float jumpSpeed = 8.0f;
    public float inAirmove = 3f;
    public float mass = 1f;

    [Header("Mouse Look Settings")]
    public Vector2 sensitivity = new Vector2(20f, 20f);
    public float sensMultiplier = 0.2f;
    public float maxAngle = 90f;
}
