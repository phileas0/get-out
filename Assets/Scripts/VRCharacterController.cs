using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRCharacterController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Grundgeschwindigkeit in m/s")]
    public float moveSpeed = 2f;
    [Tooltip("Multiplikator beim Sprinten")]
    public float sprintMultiplier = 2f;
    [Tooltip("Sprunggeschwindigkeit")]
    public float jumpSpeed = 3f;
    [Tooltip("Schwerkraft (positiv nach unten)")]
    public float gravity = 9.81f;

    [Header("Rotation")]
    [Tooltip("Drehgeschwindigkeit (°/s) beim Blicken mit rechtem Stick")]
    public float rotationSpeed = 60f;
    [Tooltip("Maus-Sensitivität (wenn kein Headset)")]
    public float mouseLookSpeed = 2f;

    private CharacterController cc;
    private Transform head;
    private float verticalVelocity = 0f;
    private float pitch = 0f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        head = GetComponentInChildren<Camera>().transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    void Update()
    {
        // --- 1) Rotation Y über rechten Stick oder Maus X ---
        Vector2 rotInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        float yaw = rotInput.x;
        // Fallback auf Maus X
        if (Mathf.Abs(yaw) < 0.01f)
            yaw = Input.GetAxis("Mouse X") * mouseLookSpeed;

        transform.Rotate(0f, yaw * rotationSpeed * Time.deltaTime, 0f);

        // --- 2) Pitch über Maus Y (nur im Editor) ---
        float mouseY = Input.GetAxis("Mouse Y");
        if (Mathf.Abs(rotInput.y) < 0.01f && Mathf.Abs(mouseY) > 0.01f)
        {
            pitch += -mouseY * mouseLookSpeed;
            pitch = Mathf.Clamp(pitch, -80f, 80f);
            head.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        // --- 3) Bewegung über linken Stick oder WASD ---
        Vector2 moveInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        // Fallback auf WASD
        if (moveInput.sqrMagnitude < 0.01f)
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Richtung relativ zum Head (nur XZ-Ebene)
        Vector3 f = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
        Vector3 r = Vector3.ProjectOnPlane(head.right,   Vector3.up).normalized;
        Vector3 horizontal = f * moveInput.y + r * moveInput.x;

        // --- 4) Sprint per Stick-Click oder Shift ---
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick) || Input.GetKey(KeyCode.LeftShift))
            horizontal *= sprintMultiplier;

        // --- 5) Springen per B-Button oder Space ---
        if (cc.isGrounded)
        {
            verticalVelocity = -0.1f;
            bool jumpOVR   = OVRInput.GetDown(OVRInput.Button.Two);
            bool jumpKey   = Input.GetKeyDown(KeyCode.Space);
            if (jumpOVR || jumpKey)
                verticalVelocity = jumpSpeed;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // --- 6) Final Move ---
        Vector3 motion = horizontal * moveSpeed + Vector3.up * verticalVelocity;
        cc.Move(motion * Time.deltaTime);
    }
}
