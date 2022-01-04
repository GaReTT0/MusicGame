using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesJudge : MonoBehaviour
{

	//タイミングのリストを取得する
	//ゲーム(曲)が開始した時間を取得する
	//レーンと対応したキーが入力されているかを判定 => 今まで流れたノーツの数を利用して、タイミングと開始時間の誤差を算出
	//=> 近いノーツを取得してノーツを消す

	//GameManagerを参照
	private GameManager gameManager;

	//MusicManagerを参照
	protected MusicManager musicManager;

	//MusicTextManagerを参照
	private MusicTextManager musicTextManager;

	//Enemyクラスを参照
	public Enemy enemy;

	//Tap音を再生する
	public SEPlayer sePlayer;

	//perfectの時間
	private float _perfectTime = 0.25f;

	//goodの時間
	private float _goodTime = 0.5f;

	//badの時間
	private float _badTime = 0.8f;

	//missの時間
	private float _missTime = 1.5f;

	//判定のプロパティ
	public float perfectTime
	{
		get { return _perfectTime; }
	}
	public float goodTime
	{
		get { return _goodTime; }
	}

	public float badTime
	{
		get { return _badTime; }
	}

	public float missTime
	{
		get { return _missTime; }
	}

	//押されるkey
	[SerializeField] protected KeyCode key;

	//レーンの番号
	[SerializeField] private int laneNum;

	//流れたノーツのカウント
	private int notesCount;
	//キーを押すタイミングとゲーム(曲)がスタートしてからの時間の差
	private double difference;

	//キーを押した時間
	protected float keyDownTiming;



	// Start is called before the first frame update
	private void Start()
	{
		gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
		musicManager = GameObject.FindWithTag("MusicManager").GetComponent<MusicManager>();
		musicTextManager = GameObject.FindWithTag("MusicTextManager").GetComponent<MusicTextManager>();
		enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();
		sePlayer = GameObject.FindWithTag("SEPlayer").GetComponent<SEPlayer>();
	}

	private void OnEnable()
	{
		notesCount = 0;
	}


	// Update is called once per frame
	private void Update()
	{
		//レーンごとのノーツの個数と判定したノーツの個数を比較する
		if (musicManager.timing[laneNum].Count > notesCount)
		{
			//押すべきタイミングと経過時間の差を比較する
			difference = (double)musicManager.timing[laneNum][notesCount] - musicManager.elapsedTime;
		}
		IsBeat();
		ThroughJudgement();
		GameJudge();
	}

	//キーを押したかどうかを判定
	public virtual void IsBeat()
	{
		if (Input.GetKeyDown(key))
		{
			// keyDownTiming = (float)musicManager.elapsedTime;
			Judgement();
			sePlayer.TapFirstPlay();
		}
	}

	//キーを押したタイミングと押すべきタイミングの誤差を評価する
	public void Judgement()
	{
		Debug.Log(notesCount.ToString());
		if (musicManager.timing[laneNum].Count > notesCount)
		{
			float difAbs = Mathf.Abs((float)difference);
			// double t = (double)musicManager.timing[laneNum][notesCount];
			// difference = Mathf.Abs((float)t - keyDownTiming);
			//Debug.Log("押すべき時間とのズレ" + difference);

			if (difAbs <= perfectTime)
			{
				//Debug.Log("perfect");
				IncreaseJudgement(0);
				AttackEnemy();
				EraseNotes();
				CountCombo(true);
				musicTextManager.SelectText(0, laneNum);
				notesCount++;
			}
			else if (difAbs <= goodTime)
			{
				//Debug.Log("good");
				IncreaseJudgement(1);
				AttackEnemy();
				EraseNotes();
				CountCombo(true);
				musicTextManager.SelectText(1, laneNum);
				notesCount++;
			}
			else if (difAbs <= badTime)
			{
				//Debug.Log("bad");
				IncreaseJudgement(2);
				EraseNotes();
				CountCombo(false);
				musicTextManager.SelectText(2, laneNum);
				notesCount++;
			}
			else if (difAbs <= missTime)
			{
				//Debug.Log("miss");
				IncreaseJudgement(3);
				EraseNotes();
				CountCombo(false);
				musicTextManager.SelectText(3, laneNum);
				notesCount++;
			}

		}
	}

	private void ThroughJudgement()
	{
		//differenceの数値が負の場合は、判定するバーよりも下に来ている。missTimeの値よりも絶対値が大きくなっている場合、Miss判定とする
		if (missTime < -difference && (musicManager.timing[laneNum].Count > notesCount))
		{
			Debug.Log("通り過ぎた");
			IncreaseJudgement(3);
			CountCombo(false);
			musicTextManager.SelectText(3, laneNum);
			notesCount++;
		}
	}

	//ノーツの判定ごとにそれぞれの判定の個数をカウントする
	public void IncreaseJudgement(int judgeNum)
	{
		musicManager.judgement[judgeNum] += 1;
	}

	//コンボをカウントする、コンボを0にする
	public void CountCombo(bool flag)
	{
		if (flag)
		{
			musicManager.combo += 1;
			if (musicManager.maxCombo < musicManager.combo)
			{
				musicManager.maxCombo = musicManager.combo;
			}
		}
		else
		{
			musicManager.combo = 0;
		}

		musicManager.comboText.text = "Combo " + musicManager.combo.ToString();
		//Debug.Log(musicManager.combo);
	}

	private void EraseNotes()
	{
		if (laneNum == 4)
		{

			musicManager.notes[laneNum][notesCount * 2].SetActive(false);
			musicManager.notes[laneNum][notesCount * 2 + 1].SetActive(false);
		}
		else
		{
			musicManager.notes[laneNum][notesCount].SetActive(false);
		}
	}

	//判定したGood以上のノーツはEnemyにダメージを与える
	private void AttackEnemy()
	{
		enemy.Damage();
		sePlayer.DamagePlay();
	}

	private void GameJudge()
	{
		if ((float)musicManager.fumen.songTime < musicManager.elapsedTime)
		{
			for (int i = 0; i < musicManager.notes.Length; i++)
			{
				for (int j = 0; j < musicManager.notes[i].Count; j++)
				{
					Destroy(musicManager.notes[i][j]);
				}
			}
			if (enemy.currentHP <= 0)
			{
				gameManager.GameFinish(true, musicManager.judgement, musicManager.maxCombo);
			}
			else
			{
				gameManager.GameFinish(false, musicManager.judgement, musicManager.maxCombo);
			}
		}
	}

}
