using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;  // Das übergeordnete Objekt, das horizontal rotiert wird

    private float xRotation = 0f;

    void Start()
    {
        // Cursor verstecken und fixieren
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Mausbewegung abfragen
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertikale Rotation der Kamera (Pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontale Rotation: Das gesamte Player-Objekt (Capsule) dreht sich
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
