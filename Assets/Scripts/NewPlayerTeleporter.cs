using UnityEngine;
using DoorScript;  // damit wir auf Door.ResetDoor() zugreifen können

public class NewPlayerTeleporter : MonoBehaviour
{
    [Tooltip("Transform, an den der Player teleportiert wird (Start-Punkt)")]
    public Transform TeleportZoneObject;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // berechne Offset relativ zum Teleporter
        Vector3 localOffset = transform.InverseTransformPoint(other.transform.position);
        Quaternion relativeRotation = TeleportZoneObject.rotation * Quaternion.Inverse(transform.rotation);

        // hol den CharacterController des Players
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc == null)
            return;

        // ausschalten, teleportieren, Türen zurücksetzen, wieder einschalten
        cc.enabled = false;

        other.transform.position = TeleportZoneObject.TransformPoint(localOffset);
        other.transform.rotation = relativeRotation * other.transform.rotation;

        ResetAllDoors();

        cc.enabled = true;
    }

    /// <summary>
    /// Schließt alle Türen in der Szene unmittelbar.
    /// </summary>
    private void ResetAllDoors()
    {
        // FindObjectsOfType sortiert die Ergebnisse nicht zwingend,
        // aber für unsere Zwecke reicht es.
        foreach (var door in FindObjectsOfType<DoorScript.Door>())
        {
            door.ResetDoor();
        }
    }
}
