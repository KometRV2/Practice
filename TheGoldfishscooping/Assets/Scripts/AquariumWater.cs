using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquariumWater : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == LayerMask.NameToLayer("Fish"))
        {
            GoldFish fish = collider.gameObject.GetComponent<GoldFish>();
            fish.InWater();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.layer == LayerMask.NameToLayer("Fish"))
        {
            GoldFish fish = collider.gameObject.GetComponent<GoldFish>();
            fish.OutWater();
        }
    }
}
