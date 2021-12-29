using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData = default;


    #region Components 
    public Core Core { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public CharacterController Controller { get; private set; }
    public Animator Anim { get; private set; }



    [Header("Mouse Look Settings")]
    [SerializeField] public Transform playerCamera;
    [SerializeField] public Transform orientation;
    [SerializeField] [Range(0.0f, 0.5f)] float mousesmooth = 0.03f;

    float cameraPitch = 0f;
    [SerializeField] float sensitivity;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVeloccity = Vector2.zero;


    private float xRotation = 0f;
    private Vector2 MouseInput;

    #endregion


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Core = GetComponentInChildren<Core>();

    }
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        InputHandler = GetComponent<InputHandler>();
        Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();

    }









    public void UpdateMouseLook()
    {
        MouseInput = InputHandler.MousetInput;

        //MouseInput = MouseInput * playerData.sensitivity * Time.fixedDeltaTime * playerData.sensMultiplier;

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, MouseInput, ref currentMouseDeltaVeloccity, mousesmooth);

        cameraPitch -= currentMouseDelta.y * sensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * sensitivity);
    }





}
