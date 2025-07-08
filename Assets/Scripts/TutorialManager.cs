using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Tooltip("Zieh hier deine 4 Tutorial-Panels rein")]
    public List<GameObject> panels;

    
    public OVRInput.Button hideButton = OVRInput.Button.Three;

    void Start()
    {
        
        foreach (var p in panels)
            if (p != null)
                p.SetActive(true);
    }

    void Update()
    {
        
        if (OVRInput.GetDown(hideButton))
        {
            
            foreach (var p in panels)
                if (p != null)
                    p.SetActive(false);

            
            Destroy(gameObject);
        }
    }
}
