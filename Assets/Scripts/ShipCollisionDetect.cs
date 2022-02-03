using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollisionDetect : MonoBehaviour
{
    public int ShipPlanetID { get; set; }
    public Ship ShipInst;

    void OnCollisionEnter(Collision collision)
    {
        Planet PlanetEnter = collision.gameObject.GetComponent<PlanetTrigger>()?.MyPlanet;
        if (PlanetEnter != null && PlanetEnter.PlanetIndex != ShipPlanetID)
        {
            ShipInst.ArriveToPlanet(PlanetEnter);
        }
    }
}
