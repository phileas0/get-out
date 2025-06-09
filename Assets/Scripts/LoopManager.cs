using UnityEngine;
using UnityEngine.Events;

public class LoopManager : MonoBehaviour
{
    [Tooltip("Der Spieler/Rig, der zurückgesetzt werden soll")]
    public Transform playerTransform;

    [Tooltip("Startposition des Loops")]
    public Transform startPoint;

    [Tooltip("Collider am Ende des Levels (IsTrigger)")]
    public Collider endPointTrigger;

    [Tooltip("Wird beim Loop-Ende ausgefeuert")]
    public UnityEvent onLoopComplete;

    [HideInInspector]
    public int loopCount = 0;

    void Awake()
    {
        // Listener auf den Trigger hängen
        var trigger = endPointTrigger.gameObject.AddComponent<LoopTrigger>();
        trigger.manager = this;
    }

    public void CompleteLoop()
    {
        loopCount++;
        playerTransform.position = startPoint.position;
        playerTransform.rotation *= Quaternion.Euler(0f, 180f, 0f);
        onLoopComplete?.Invoke();
    }

    // Nested helper-Klasse
    private class LoopTrigger : MonoBehaviour
    {
        public LoopManager manager;
        void OnTriggerEnter(Collider other)
        {
            if (other.transform == manager.playerTransform)
                manager.CompleteLoop();
        }
    }
}
