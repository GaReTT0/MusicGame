using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{

	[System.Serializable]
	private struct StageNumData
	{
		public int stageNum;
	}

	// ファイルパス
	private string _dataPath;

	// //stageの番号を格納
	// public int stageNum;

	//GameManagerを参照
	private GameManager gameManager;

	// Start is called before the first frame update
	void Start()
	{
		gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	//Stageを選択し、stageNumをMusicManagerにわたす
	public void SelectStage(int num)
	{
		gameManager.stageNum = num;
		StartCoroutine(gameManager.GameStart());

	}
}
