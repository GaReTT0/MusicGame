using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTextManager : MonoBehaviour
{
	[SerializeField] GameObject[] perfect = new GameObject[4];
	[SerializeField] GameObject[] good = new GameObject[4];
	[SerializeField] GameObject[] bad = new GameObject[4];
	[SerializeField] GameObject[] miss = new GameObject[4];

	//判定テキストを表示する時間
	[SerializeField] float displayTime = 0.1f;
	public void SelectText(int type, int lane)
	{
		switch (type)
		{
			case 0:
				StartCoroutine(DisplayJudgeText(perfect[lane]));
				break;
			case 1:
				StartCoroutine(DisplayJudgeText(good[lane]));
				break;
			case 2:
				StartCoroutine(DisplayJudgeText(bad[lane]));
				break;
			case 3:
				StartCoroutine(DisplayJudgeText(miss[lane]));
				break;
		}
	}

	private IEnumerator DisplayJudgeText(GameObject textObj)
	{
		textObj.SetActive(true);
		yield return new WaitForSeconds(displayTime);
		textObj.SetActive(false);
	}



}
