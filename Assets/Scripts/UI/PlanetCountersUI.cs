using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class PlanetCountersUI : MonoBehaviour
{
    public GameObject PlanetTextPrefab;
    public GameObject ForCounters;

    private GameObject[] Counters = new GameObject[999];
    private GameObject[] Planets;
    private World WorldInst;

    void Start()
    {
        WorldInst = gameObject.GetComponent<World>();
    }

    void Update()
    {
        WorldInst = gameObject.GetComponent<World>();
        for (int i = 0; i < WorldInst.PlanetsCount; i++)
        {
            Counters[i].GetComponent<Text>().text = Planets[i].GetComponent<Planet>().Ships.ToString();
        }
    }

    public void CreateCounters()
    {
        WorldInst = gameObject.GetComponent<World>();
        Planets = WorldInst.GetPlanets();
        for (int i = 0; i < WorldInst.PlanetsCount; i++)
        {
            if (Counters[i] != null)
                Destroy(Counters[i]);
        }
        for (int i = 0; i < WorldInst.PlanetsCount; i++)
        {
            Vector3 PlanetPos = Planets[i].transform.position;
            Counters[i] = Instantiate(PlanetTextPrefab, new Vector3(PlanetPos.x * WorldInterface.RelToCnvCaf, PlanetPos.z * WorldInterface.RelToCnvCaf, 0), Quaternion.identity);
            Counters[i].transform.SetParent(ForCounters.transform, false);
        }
    }

}
