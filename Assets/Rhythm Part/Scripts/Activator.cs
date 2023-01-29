using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;



public class Activator : NetworkBehaviour
{
    public KeyCode key;
    bool active = false;
    GameObject note;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(key) && active)
        {
            Destroy(note.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsOwner) return;
        active = true;
        Debug.Log("touched");
        if (other.gameObject.tag == "Note")
        {
            note = other.gameObject;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!IsOwner) return;
        active = false;
    }
}
