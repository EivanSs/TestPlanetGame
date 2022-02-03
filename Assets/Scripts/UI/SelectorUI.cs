using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class SelectorUI : MonoBehaviour
{
    public GameObject SelecerLineObj;
    public GameObject HoverCursor;
    public GameObject SelectingCursor;

    public void SetCursor(Vector3 PlanetPos, float PlanetScale, bool IsSelecting)
    {
        GameObject Obj = IsSelecting ? SelectingCursor : HoverCursor;
        Obj.transform.localPosition = new Vector2(PlanetPos.x * WorldInterface.RelToCnvCaf, PlanetPos.z * WorldInterface.RelToCnvCaf);
        Obj.GetComponent<RectTransform>().sizeDelta = new Vector2(PlanetScale * WorldInterface.RelToCnvCaf * 2f, PlanetScale * WorldInterface.RelToCnvCaf * 2f);
        Obj.SetActive(true);
    }

    public void HideCursor(bool IsSelecting)
    {
        GameObject Obj = IsSelecting ? SelectingCursor : HoverCursor;
        Obj.SetActive(false);
    }

    public void SetLine(Vector3 FromPlanet, Vector3 ToPlanet)
    {
        Vector3[] Points = { FromPlanet, ToPlanet };
        SelecerLineObj.GetComponent<LineRenderer>().SetPositions(Points);
        SelecerLineObj.SetActive(true);
    }

    public void HideLine()
    {
        SelecerLineObj.SetActive(false);
    }
}
