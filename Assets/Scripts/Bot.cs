using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    const float BotActionDelay = 4;

    private float BotActionDelayLeft = BotActionDelay;

    private World WorldInst;

    void Start()
    {
        WorldInst = gameObject.GetComponent<World>();
    }

    void Update()
    {
        BotActionCheck();
    }

    public void Reset()
    {
        BotActionDelayLeft = BotActionDelay;
    }

    private void BotActionCheck()
    {
        if (BotActionDelayLeft < 0)
        {
            BotAction();
            BotActionDelayLeft = BotActionDelay;
        }
        else
            BotActionDelayLeft -= Time.deltaTime;
    }

    private void BotAction()
    {
        GameObject[] AllPlanets = WorldInst.GetPlanets();
     
        int BotMaxShips = int.MinValue;
        int BotMaxShipPlanetId = -1;
        for (int i = 0; i < World.PlanetsCount; i++)
        {
            Planet PlanetInst = AllPlanets[i].GetComponent<Planet>();
            if (PlanetInst.GetTeam() == Planet.Teams.War)
            {
                if (PlanetInst.Ships > BotMaxShips)
                {
                    BotMaxShips = PlanetInst.Ships;
                    BotMaxShipPlanetId = i;
                }
            }
        }

        int MinShips = int.MaxValue;
        int MinShipsPlanetId = -1;
        for (int i = 0; i < World.PlanetsCount; i++)
        {
            Planet PlanetInst = AllPlanets[i].GetComponent<Planet>();
            if (PlanetInst.GetTeam() != Planet.Teams.War)
            {
                if (PlanetInst.Ships < MinShips)
                {
                    MinShips = PlanetInst.Ships;
                    MinShipsPlanetId = i;
                }
            }
        }
        Planet BotMaxShipsPlanetInst = AllPlanets[BotMaxShipPlanetId].GetComponent<Planet>();
        BotMaxShipsPlanetInst.SpawnShips(MinShipsPlanetId);

    }

}
