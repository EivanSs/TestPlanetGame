using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rand : MonoBehaviour
{
	private static System.Random SystemRandom;
	static Rand()
	{
		SystemRandom = new System.Random((int)(Time.realtimeSinceStartup * 1000));
	}
	public static float GetRandomFloat(float Min, float Max)
	{
		int RandomInt = SystemRandom.Next(0, Int32.MaxValue);
		return Mathf.Lerp(Min, Max, (float)RandomInt / Int32.MaxValue);
	}
	public static int GetRandomInt(int Min, int Max)
	{
		int RandomInt = SystemRandom.Next(0, Mathf.Abs(Mathf.Abs(Max) - Min));
		return (RandomInt - Max) * -1;
	}
}
