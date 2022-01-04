using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyList
{
	public List<EnemyData> data;
}

[System.Serializable]
public class EnemyData
{
	public string name;
	public float hp;
	public int num;
}
