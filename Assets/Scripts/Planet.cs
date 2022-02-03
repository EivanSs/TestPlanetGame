using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public enum Teams
    {
        War = -1,
        Neutral,
        Player
    }

    public GameObject ShipPrefab;
    public GameObject VisualSphere;
    public ShipCollisionDetect ShipCollisionDetectInst;

    public float Scale { get; set; }
    public int Ships { get; set; }
    public World WorldInst { get; set; }
    public int PlanetIndex { get; set; }

    private Teams Owner;
    private float ShipCreatingSpeedLeft;
    private List<GameObject> ShipsObjects = new List<GameObject>();

    void Start()
    {
        Scale = Rand.GetRandomFloat(0.4f, 0.8f);
        VisualSphere.transform.localScale = new Vector3(Scale, Scale, Scale);
        ShipCreatingSpeedLeft = 1f / WorldInst.ShipCreatingSpeed;
    }

    void Update()
    {
        if (Owner != Teams.Neutral)
            CreatingShip();
    }

    public Teams GetTeam()
    {
        return Owner;
    }

    public void SetTeam(Teams NewTeam)
    {
        Owner = NewTeam;
        Color NewColor = new Color(1, 1, 1);
        switch (Owner)
        {
            case Teams.War:
                NewColor = new Color(0.73f, 0.09f, 0.55f);
                break;
            case Teams.Neutral:
                NewColor = new Color(0.4f, 0.4f, 0.4f);
                break;
            case Teams.Player:
                NewColor = new Color(0.09f, 0.24f, 0.73f);
                break;
        }
        VisualSphere.GetComponent<Renderer>().material.color = NewColor;
    }

    public void DestroyAllShips()
    {
        foreach (GameObject ship in ShipsObjects)
        {
            Destroy(ship);
        }
    }

    public void SpawnShips(int Target)
    {
        int ShipsSend = Ships / 2;
        Vector3 TargetPos = WorldInst.GetPlanets()[Target].transform.position;
        float AngleWithPlanets = (float)Mathf.Atan2(TargetPos.z - transform.position.z, TargetPos.x - transform.position.x);
        Vector3 NormalizeDirectionFotTarget = Vector3.Normalize(TargetPos - transform.position);
        Vector3[] ShipsPos = GetShipPositions(NormalizeDirectionFotTarget, ShipsSend);
        for (int i = 0; i < ShipsSend; i++)
        {
            if (Owner == Teams.Player) ShipsPos[i] += new Vector3(0, 0.15f, 0);
            else ShipsPos[i] -= new Vector3(0, 0.15f, 0);
            GameObject ShipsObj = Instantiate(ShipPrefab, ShipsPos[i], Quaternion.identity);
            Ship ShipExmp = ShipsObj.GetComponent<Ship>();
            ShipExmp.Body.transform.localRotation = Quaternion.Euler(90, 0, AngleWithPlanets * Mathf.Rad2Deg - 90);
            ShipExmp.WorldInst = WorldInst;
            ShipExmp.ShipPlanetIndex = PlanetIndex;
            ShipExmp.Target = Target;
            ShipExmp.SetTeam = Owner;

            ShipsObjects.Add(ShipsObj);
        }
        Ships -= ShipsSend;
    }

    private void CreatingShip()
    {
        if (ShipCreatingSpeedLeft < 0)
        {
            Ships++;
            ShipCreatingSpeedLeft = 1f / WorldInst.ShipCreatingSpeed;
        }
        else
            ShipCreatingSpeedLeft -= Time.deltaTime;
    }

    private Vector3[] GetShipPositions(Vector3 TargetDirection, int Count)
    {
        Vector3[] Vectors = new Vector3[Count];
        Vector3 PlanetPos = transform.position;
        for (int i = 0; i < Count; i++)
        {
            Vector3 RandomPos = new Vector3(Rand.GetRandomFloat(-0.1f, 0.1f), 0, Rand.GetRandomFloat(-0.1f, 0.1f));
            Vectors[i] = PlanetPos + TargetDirection / 1.25f + RandomPos;
        }
        int ItersCount = 0;
        for (int iters = 0; iters < ItersCount; iters++)
        {
            for (int i = 0; i < Count; i++)
            {
                for (int i2 = 0; i2 < Count; i2++)
                {
                    float Distance = Vector3.Distance(Vectors[i], Vectors[i2]) - 0.5f;
                    float AngleWith = Mathf.Atan2(Vectors[i].z - Vectors[i2].z, Vectors[i].x - Vectors[i2].x);
                    Vector3 MathVector = new Vector3(Mathf.Cos(AngleWith) * Distance, 0, Mathf.Sin(AngleWith) * Distance);
                    Vectors[i] -= MathVector;
                    Vectors[i2] += MathVector;
                }
            }
        }
        return Vectors;
    }


}
