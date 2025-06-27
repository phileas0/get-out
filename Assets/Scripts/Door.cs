using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Door : MonoBehaviour
    {
        [Tooltip("Ist die Tür aktuell offen?")]
        public bool open;

        [Tooltip("Wie weich die Animation ablaufen soll")]
        public float smooth = 1.0f;

        // Winkel für offen/zu
        private float DoorOpenAngle  = -90.0f;
        private float DoorCloseAngle =   0.0f;

        [Tooltip("Audio Source auf diesem GameObject")]
        public AudioSource asource;

        [Tooltip("Sound beim Öffnen bzw. Schließen")]
        public AudioClip openDoor, closeDoor;

        void Start()
        {
            asource = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (open)
            {
                var target = Quaternion.Euler(0, DoorOpenAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
            }
            else
            {
                var target = Quaternion.Euler(0, DoorCloseAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
            }
        }

        /// <summary>Öffnet oder schließt die Tür per Knopfdruck.</summary>
        public void OpenDoor()
        {
            open = !open;
            asource.clip = open ? openDoor : closeDoor;
            asource.Play();
        }

        /// <summary>Bringt die Tür sofort in den geschlossenen Zustand zurück.</summary>
        public void ResetDoor()
        {
            open = false;
            transform.localRotation = Quaternion.Euler(0, DoorCloseAngle, 0);
        }
    }
}
