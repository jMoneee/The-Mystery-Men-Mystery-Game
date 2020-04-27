using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Pickupable))]
public class ShowMinigameTriggers : MonoBehaviour
{
    Pickupable Pickupable { get { return GetComponent<Pickupable>(); } }
    bool oldInteracting = false;
    // Update is called once per frame
    void Update()
    {
        bool interacting = Pickupable.interacting;

        if (interacting != oldInteracting)
        {
            MinigameStation[] minigameStations = FindObjectsOfType<MinigameStation>();
            foreach (MinigameStation minigameStation in minigameStations)
            {
                if (minigameStation.minigames.Where(m => m.triggerObject == gameObject && m.success == false).Any())
                {
                    minigameStation.GetComponent<MeshRenderer>().enabled = interacting;
                }
                else
                {
                    minigameStation.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }

        oldInteracting = interacting;
    }
}
