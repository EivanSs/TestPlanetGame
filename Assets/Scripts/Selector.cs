using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Selector : MonoBehaviour
{
    private SelectorUI SlctrUI;
    private World WorldInst;
    private bool InHoverSelecting;

    private int HoveredPlanet = -1;
    private int SelectedPlanet = -1;

    void Start()
    {
        SlctrUI = gameObject.GetComponent<SelectorUI>();
        WorldInst = gameObject.GetComponent<World>();
    }

    void Update()
    {

        GameObject[] Planets = WorldInst.GetPlanets();

        HoveredPlanet = -1;

        Vector3 MousePos = Input.mousePosition;
        MousePos -= new Vector3(Screen.width / 2, Screen.height / 2, 0);
        MousePos *= WorldInterface.ScreenToCanvasCaf;

        for (int i = 0; i < WorldInst.PlanetsCount; i++)
        {
            Vector3 PlanetCanvPos = Planets[i].transform.position * WorldInterface.RelToCnvCaf;
            float Distance = Vector2.Distance(new Vector2(PlanetCanvPos.x, PlanetCanvPos.z), new Vector2(MousePos.x, MousePos.y));
            if (Distance < (WorldInst.GetPlanetScale(i) / 2 ) * WorldInterface.RelToCnvCaf)
            {
                HoveredPlanet = i;
            }
        }

        if (HoveredPlanet != -1 && !InHoverSelecting)
        {
            SlctrUI.SetCursor(Planets[HoveredPlanet].transform.position, WorldInst.GetPlanetScale(HoveredPlanet), false);
            InHoverSelecting = true;
        }
        if (HoveredPlanet == -1 && InHoverSelecting)
        {
            SlctrUI.HideCursor(false);
            InHoverSelecting = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Spawn Ship
            if (HoveredPlanet != -1 && SelectedPlanet != -1 && HoveredPlanet != SelectedPlanet)
            {
                Planets[SelectedPlanet].GetComponent<Planet>().SpawnShips(HoveredPlanet);
            }
            else
            {
                // Check For Selecting
                if (HoveredPlanet != -1)
                {
                    if (WorldInst.CanPlanetSelect(HoveredPlanet))
                    {
                        SelectedPlanet = HoveredPlanet;
                        SlctrUI.SetCursor(Planets[HoveredPlanet].transform.position, WorldInst.GetPlanetScale(HoveredPlanet), true);
                    }
                }
                else
                {
                    SlctrUI.HideCursor(true);
                    SelectedPlanet = -1;
                }
            }
        }

        // Hide & SetLine
        if (SelectedPlanet != -1 && HoveredPlanet != -1)
            SlctrUI.SetLine(Planets[SelectedPlanet].transform.position, Planets[HoveredPlanet].transform.position);
        else
            SlctrUI.HideLine();

    }
}
