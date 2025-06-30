// NewPlayerTeleporter.cs
using UnityEngine;
using DoorScript;  // damit wir auf Door.ResetDoor() zugreifen können

public class NewPlayerTeleporter : MonoBehaviour
{
    public enum TeleportMode { Advance, Retreat }
    [Tooltip("Advance: Vorwärts teleportieren; Retreat: Rückwärts teleportieren und jeweils den passenden Check ausführen.")]
    public TeleportMode mode;

    [Tooltip("Transform, an den der Player teleportiert wird")]
    public Transform TeleportZoneObject;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // 1) Offset & Rotation berechnen
        Vector3 localOffset = transform.InverseTransformPoint(other.transform.position);
        Quaternion relativeRot = TeleportZoneObject.rotation * Quaternion.Inverse(transform.rotation);

        // 2) CharacterController temporär deaktivieren
        var cc = other.GetComponent<CharacterController>();
        if (cc == null) return;
        cc.enabled = false;

        // 3) Teleportieren
        other.transform.position = TeleportZoneObject.TransformPoint(localOffset);
        other.transform.rotation = relativeRot * other.transform.rotation;

        // 4) Türen zurücksetzen
        ResetAllDoors();

        // 5) Controller reaktivieren
        cc.enabled = true;

        // 6) Je nach Mode den passenden Check auslösen
        switch (mode)
        {
            case TeleportMode.Advance:
                var adv = Object.FindFirstObjectByType<AdvanceCheck>();
                if (adv != null) adv.ExecuteCheck();
                else Debug.LogWarning("NewPlayerTeleporter: Kein AdvanceCheck gefunden!");
                break;

            case TeleportMode.Retreat:
                var ret = Object.FindFirstObjectByType<RetreatCheck>();
                if (ret != null) ret.ExecuteCheck();
                else Debug.LogWarning("NewPlayerTeleporter: Kein RetreatCheck gefunden!");
                break;
        }
    }

    private void ResetAllDoors()
    {
        // Richtiges Aufrufen von FindObjectsByType:
        // nur das SortMode-Argument ist nötig, um wie vorher alle aktiven Türen zu holen.
        var allDoors = Object.FindObjectsByType<Door>(FindObjectsSortMode.None);
        foreach (var door in allDoors)
            door.ResetDoor();
    }
}
