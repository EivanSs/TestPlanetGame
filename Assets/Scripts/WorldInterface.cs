using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public sealed class WorldInterface : MonoBehaviour
{
	public const float MaxRealPosition = 5;
	public const float MaxCanvasPosition = 250;
	public const float RelToCnvCaf = MaxCanvasPosition / MaxRealPosition;
	public const float CnvToRelCaf = MaxRealPosition / MaxCanvasPosition;

	public Button CreateWroldButton;
	public GameObject FinalyScreen;
	public Button RestartButton;
	public static float ScreenToCanvasCaf { get; set; }

	private int ScreenWidth = 0;
	private int ScreenHeight = 0;
	private World WorldInst;

	public void ShowFinaly()
    {
		FinalyScreen.SetActive(true);
	}

	private void Restart()
    {
		FinalyScreen.SetActive(false);
		WorldInst.CreateWrold();
	}

	void Start()
	{
		WorldInst = gameObject.GetComponent<World>();
		CreateWroldButton.onClick.AddListener(WorldInst.CreateWrold);
		UnityAction RestartAction = new UnityAction(Restart);
		RestartButton.onClick.AddListener(RestartAction);
	}

	void Update()
	{
		if (ScreenWidth != Screen.width || ScreenHeight != Screen.height)
		{
			ScreenWidth = Screen.width;
			ScreenHeight = Screen.height;
			ScreenToCanvasCaf = (MaxCanvasPosition * 2) / ScreenHeight;
		}
	}

}
