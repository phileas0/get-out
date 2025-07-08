// NewPlayerTeleporter.cs
using UnityEngine;
using DoorScript;

public class NewPlayerTeleporter : MonoBehaviour
{
    public enum TeleportMode { Advance, Retreat }
    public TeleportMode mode;
    public Transform TeleportZoneObject;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var cc = other.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // Teleport
        Vector3 localOffset = transform.InverseTransformPoint(other.transform.position);
        Quaternion relativeRot = TeleportZoneObject.rotation * Quaternion.Inverse(transform.rotation);
        other.transform.position = TeleportZoneObject.TransformPoint(localOffset);
        other.transform.rotation = relativeRot * other.transform.rotation;

        ResetAllDoors();
        if (cc != null) cc.enabled = true;

        // Check ausführen
        switch (mode)
        {
            case TeleportMode.Advance:
                var adv = Object.FindFirstObjectByType<AdvanceCheck>();
                if (adv != null) adv.ExecuteCheck();
                break;
            case TeleportMode.Retreat:
                var ret = Object.FindFirstObjectByType<RetreatCheck>();
                if (ret != null) ret.ExecuteCheck();
                break;
        }
    }

    private void ResetAllDoors()
    {
        var allDoors = Object.FindObjectsByType<Door>(FindObjectsSortMode.None);
        foreach (var door in allDoors)
            door.ResetDoor();
    }
}
