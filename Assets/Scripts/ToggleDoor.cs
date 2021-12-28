using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDoor : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    private void Update()
    {
        DoorInteraction();
    }

    private Tuple<Door, float> GetDistanceAndObjectDoor()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.parent.CompareTag("Door"))
            {
                return Tuple.Create(hit.transform.parent.GetComponent<Door>(), hit.distance);
            }
            return null;
        }
        return null;
    }

    private void DoorInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Tuple<Door, float> hitInfo = GetDistanceAndObjectDoor();
            if (hitInfo != null)
            {
                Door hitDoor = hitInfo.Item1;
                float distance = hitInfo.Item2;
                if (distance <= 30f && distance >= 0)
                {
                    if (hitDoor.IsAvailable())
                    {
                        ActivateDoor(hitDoor);
                    }
                }
            }
        }
    }

    private void ActivateDoor(Door door)
    {
        door.Activate();
    }
}
