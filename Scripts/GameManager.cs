using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
	Title,
	Select,
	Game,
	Result
}

public class GameManager : MonoBehaviour
{
	//現在の状態
	public GameState gameState = GameState.Title;

	//stageの番号を格納
	public int stageNum;

	//遷移するシーンの名前
	private string sceneName;
	//ゲームクリアの真偽
	private bool gameFinish;

	//タイトル画面canvas
	[SerializeField] GameObject TitleCanvas;
	//音楽ゲームをするcanvas
	[SerializeField] GameObject MusicGameCanvas;
	//曲を選択するcanvas
	[SerializeField] GameObject StageSelectCanvas;
	//リザルトを表示するcanvas
	[SerializeField] GameObject ResultCanvas;

	[SerializeField] Text result;
	[SerializeField] Text maxCombo;
	[SerializeField] Text perfectNum;
	[SerializeField] Text goodNum;
	[SerializeField] Text badNum;
	[SerializeField] Text missNum;
	// Start is called before the first frame update

	// Start is called before the first frame update
	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		if (gameState == GameState.Title || gameState == GameState.Result)
		{
			if (Input.anyKey)
			{
				ChangeState(GameState.Select);
			}
		}
	}

	public IEnumerator GameStart()
	{
		ChangeState(GameState.Game);
		yield return new WaitForSeconds(1);
		Debug.Log("3");
		yield return new WaitForSeconds(1);
		Debug.Log("2");
		yield return new WaitForSeconds(1);
		Debug.Log("1");
		yield return new WaitForSeconds(0.3f);
		Debug.Log("START!");
	}

	//ゲームクリア、ゲームオーバー判定
	public void GameFinish(bool gamefinish, int[] judgeNum, int maxComboNum)
	{
		ChangeState(GameState.Result);
		if (gamefinish)
		{
			result.text = "Result : CLEAR";
		}
		else
		{
			result.text = "Result : FAILED";
		}
		maxCombo.text = maxComboNum.ToString();
		perfectNum.text = judgeNum[0].ToString();
		goodNum.text = judgeNum[1].ToString();
		badNum.text = judgeNum[2].ToString();
		missNum.text = judgeNum[3].ToString();

	}

	//シーンを移動させる
	public void ChangeScene(string sName)
	{
		SceneManager.LoadScene(sName);
	}

	private void ChangeState(GameState state)
	{
		gameState = state;
		switch (state)
		{
			case GameState.Title:
				TitleCanvas.SetActive(true);
				StageSelectCanvas.SetActive(false);
				MusicGameCanvas.SetActive(false);
				ResultCanvas.SetActive(false);
				break;
			case GameState.Select:
				TitleCanvas.SetActive(false);
				StageSelectCanvas.SetActive(true);
				MusicGameCanvas.SetActive(false);
				ResultCanvas.SetActive(false);
				break;
			case GameState.Game:
				TitleCanvas.SetActive(false);
				StageSelectCanvas.SetActive(false);
				MusicGameCanvas.SetActive(true);
				ResultCanvas.SetActive(false);
				break;
			case GameState.Result:
				TitleCanvas.SetActive(false);
				StageSelectCanvas.SetActive(false);
				MusicGameCanvas.SetActive(false);
				ResultCanvas.SetActive(true);
				break;
		}
	}

}
