using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRCharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float sprintMultiplier = 2f;
    public float jumpSpeed = 3f;
    public float gravity = 9.81f;

    [Header("Rotation")]
    public float rotationSpeed = 60f;

    
    [Header("Game Systems")]
    public PauseMenuManager pauseMenuManager;

    private CharacterController cc;
    private Transform head;
    private float verticalVelocity = 0f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        head = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        
        if (OVRInput.GetDown(OVRInput.Button.Four)) 
        {
            if (PauseMenuManager.isPaused)
            {
                pauseMenuManager.ResumeGame();
            }
            else
            {
                pauseMenuManager.PauseGame();
            }
            return; 
        }

        
        if (PauseMenuManager.isPaused) return;


        
        Vector2 rotInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        float yaw = rotInput.x;
        transform.Rotate(0f, yaw * rotationSpeed * Time.deltaTime, 0f);

        
        Vector2 moveInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector3 forward = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(head.right, Vector3.up).normalized;
        Vector3 horizontal = forward * moveInput.y + right * moveInput.x;

        
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
            horizontal *= sprintMultiplier;

        
        if (cc.isGrounded)
        {
            verticalVelocity = -0.1f;
            if (OVRInput.GetDown(OVRInput.Button.Two))
                verticalVelocity = jumpSpeed;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        
        Vector3 motion = horizontal * moveSpeed + Vector3.up * verticalVelocity;
        cc.Move(motion * Time.deltaTime);
    }
}