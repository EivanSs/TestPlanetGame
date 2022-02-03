using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public GameObject Body;
    public ShipCollisionDetect ShipCollisionDetectInst;

    public World WorldInst { get; set; }
    public int Target { get; set; }
    public Planet.Teams SetTeam { set { Team = value; SetColor(Team); } }
    public int ShipPlanetIndex { get { return ShipPlanetID; } set { ShipPlanetID = value; ShipCollisionDetectInst.ShipPlanetID = value; } }

    private int ShipPlanetID = -1;
    private GameObject[] Planets;
    private Vector3 TargetVector;
    private Rigidbody BodyPhys;
    private Vector3 BodyOldPosition;
    private Planet.Teams Team;

    void Start()
    {
        Planets = WorldInst.GetPlanets();
        TargetVector = Planets[Target].transform.position;
        BodyPhys = Body.GetComponent<Rigidbody>();
        BodyOldPosition = TargetVector;
    }

    void FixedUpdate()
    {
        BodyPhys.velocity = Vector3.Normalize(TargetVector - Body.transform.position) * WorldInst.ShipSpeed;
        float Angle = (float)Mathf.Atan2(Body.transform.position.z - BodyOldPosition.z, Body.transform.position.x - BodyOldPosition.x);
        Body.transform.localRotation = Quaternion.Euler(90, 0, Angle * Mathf.Rad2Deg - 90);
        BodyOldPosition = Body.transform.position;
    }

    public void ArriveToPlanet(Planet PlanetArrived)
    {
        if (PlanetArrived.PlanetIndex == Target)
        {
            if (PlanetArrived.GetTeam() == Team)
            {
                PlanetArrived.Ships++;
            }
            else
            {
                if (PlanetArrived.Ships == 0)
                {
                    PlanetArrived.SetTeam(Team);
                    PlanetArrived.Ships++;
                }
                else
                {
                    PlanetArrived.Ships--;
                }
            }
            
            Destroy(gameObject);
        }
    }

    private void SetColor(Planet.Teams Team)
    {
        Color NewColor = new Color(1, 1, 1);
        if (Team == Planet.Teams.Player)
            NewColor = new Color(0.09f, 0.24f, 0.73f);
        else
            NewColor = new Color(0.73f, 0.09f, 0.55f);
        Body.GetComponent<Renderer>().material.color = NewColor;
    }

}
