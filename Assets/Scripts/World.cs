using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	public int PlanetsCount = 30;
	public float ShipCreatingSpeed = 5f;
	public float ShipSpeed = 5;
	public int MinStartPlanetShips = 5;
	public int MaxStartPlanetShips = 50;
	public bool BotVSBot = true;
	private const float MaxPlanetXOffset = 6.5f;
	private const float MaxPlanetZOffset = 4.5f;

	public GameObject PlanetPrefab;
	public GameObject ShipPrefab;

	private GameObject[] WolrdPlanets = new GameObject[999];

	void Start()
	{
		CreateWrold();
	}

	void Update()
    {
		CheckForFinaly();
	}

	private void CheckForFinaly()
    {
		bool PlayerLive = false;
		bool BotLive = false;
        for (int i = 0; i < PlanetsCount; i++)
        {
			Planet PlanetInst = WolrdPlanets[i].GetComponent<Planet>();
			if (PlanetInst.GetTeam() == Planet.Teams.Player)
				PlayerLive = true;
			if (PlanetInst.GetTeam() == Planet.Teams.War)
				BotLive = true;
		}
		if (!PlayerLive || !BotLive)
			GameFinaly();
	}

	private void ClearWorld()
	{
		for (int i = 0; i < WolrdPlanets.Length; i++)
		{
			WolrdPlanets[i]?.GetComponent<Planet>().DestroyAllShips();
			Destroy(WolrdPlanets[i]);
		}
	}

	private Vector3 GetRandomPlanetPosition(GameObject[] CreatedPlanets)
	{
		float RandomX;
		float RandomZ;
		float MinDistance;
		int MaxIters = 100;
		do
		{
			MinDistance = float.MaxValue;
			RandomX = Rand.GetRandomFloat(-MaxPlanetXOffset, MaxPlanetXOffset);
			RandomZ = Rand.GetRandomFloat(-MaxPlanetZOffset, MaxPlanetZOffset);
            for (int i = 0; i < CreatedPlanets.Length; i++)
            {
				if (CreatedPlanets[i] != null)
                {
					Vector3 PlanetPos = CreatedPlanets[i].transform.position;
					float Distance = Vector2.Distance(new Vector2(RandomX, RandomZ), new Vector2(PlanetPos.x, PlanetPos.z));
					if (Distance < MinDistance)
						MinDistance = Distance;
				}
            }
			MaxIters--;
		} while (MinDistance < 1f && MaxIters > 0);

		Vector3 CreatedPosition = new Vector3(RandomX, 0, RandomZ);
		return CreatedPosition;
	}

	public void GameFinaly()
    {
		WorldInterface InterfaceInst = gameObject.GetComponent<WorldInterface>();
		InterfaceInst.ShowFinaly();
		Time.timeScale = 0;
	}

	public void CreateWrold()
	{
		ClearWorld();

		Time.timeScale = 1;

		gameObject.GetComponent<Bot>().Reset();

		int PlayerFirstPlanet = Rand.GetRandomInt(0, PlanetsCount);
		int WarFirstPlanet;

		do
			WarFirstPlanet = Rand.GetRandomInt(0, PlanetsCount);
		while (WarFirstPlanet == PlayerFirstPlanet);

		for (int i = 0; i < PlanetsCount; i++)
		{
			WolrdPlanets[i] = Instantiate(PlanetPrefab, GetRandomPlanetPosition(WolrdPlanets), Quaternion.identity);
			Planet PlanetInst = WolrdPlanets[i].GetComponent<Planet>();
			PlanetInst.PlanetIndex = i;

			if (i == PlayerFirstPlanet)
			{
				PlanetInst.SetTeam(Planet.Teams.Player);
				PlanetInst.Ships = 50;
			}
			else if (i == WarFirstPlanet)
			{
				PlanetInst.SetTeam(Planet.Teams.War);
				PlanetInst.Ships = 50;
			}
			else
			{
				PlanetInst.SetTeam(Planet.Teams.Neutral);
				PlanetInst.Ships = Rand.GetRandomInt(MinStartPlanetShips, MaxStartPlanetShips);
			}

			PlanetInst.WorldInst = this;
			PlanetInst.ShipPrefab = ShipPrefab;
		}

		PlanetCountersUI PlanetCountrUI = gameObject.GetComponent<PlanetCountersUI>();
		PlanetCountrUI.CreateCounters();

	}

	public GameObject[] GetPlanets()
	{
		return WolrdPlanets;
	}

	public float GetPlanetScale(int index)
	{
		return WolrdPlanets[index].GetComponent<Planet>().Scale;
	}

	public bool CanPlanetSelect(int index)
    {
		return WolrdPlanets[index].GetComponent<Planet>().GetTeam() == Planet.Teams.Player;
	}
}
