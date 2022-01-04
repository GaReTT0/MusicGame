using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoteEditor.DTO;
using UnityEngine.UI;


public class MusicManager : MonoBehaviour
{

	[SerializeField] private GameManager gameManager;

	//ゲーム(曲)がスタートしてからの時間
	public double elapsedTime;

	//perfect,good,bad,missのそれぞれの数を格納する
	public int[] judgement = new int[4];

	//コンボの数
	public int combo;

	//最大コンボ数
	public int maxCombo = 0;

	//選択された曲
	private AudioSource audioSource;

	//選択された曲の名前
	[SerializeField] string musicName;

	//曲の譜面情報を取り出す
	public MusicDTO.EditData fumen;

	//曲の譜面のファイルパス
	private string fumenPath;

	//ノーツのPrefabを格納
	[SerializeField] GameObject notesPrefab;

	//特殊ノーツのPrefabを格納
	[SerializeField] GameObject XnotesPrefab;

	//ノーツのPrefabにアタッチしているスクリプトからノーツのスピードを取得
	private NotesMove notesMove;

	//ノーツの初期生成位置をずらす
	[SerializeField] float distance;

	//曲の再生時間をずらす時間
	private float offsetTime;

	//ノーツのタイミングを格納する
	public ArrayList[] timing ={
		new ArrayList(),
		new ArrayList(),
		new ArrayList(),
		new ArrayList(),
		new ArrayList()
	};

	//ノーツのオブジェクトを格納する
	public List<GameObject>[] notes ={
	new List<GameObject>(),
	new List<GameObject>(),
	new List<GameObject>(),
	new List<GameObject>(),
	new List<GameObject>()
};

	//コンボを表示する
	public Text comboText;


	void OnEnable()
	{
		notesMove = notesPrefab.GetComponent<NotesMove>();
		audioSource = GameObject.FindWithTag("MusicPlayer").GetComponent<AudioSource>();
		comboText = GameObject.FindWithTag("ComboText").GetComponent<Text>();
		musicName = gameManager.stageNum.ToString();
		offsetTime = this.distance / notesMove.noteSpeed;
		elapsedTime = -offsetTime;

		for (int i = 0; i < judgement.Length; i++)
		{
			judgement[i] = 0;
		}
		combo = 0;
		maxCombo = 0;
		for (int i = 0; i < notes.Length; i++)
		{
			timing[i].Clear();
			notes[i].Clear();
		}


		//elapsedTime -= offsetTime;
		SetMusic();
		SetJson();
		LoadJson();
		NotesInfomationAdd();
		GenerateNotes();
		StartCoroutine(Offset());
		Debug.Log(notes[0].Count.ToString());
	}

	//曲と譜面のずれを調整する
	private IEnumerator Offset()
	{
		//曲と譜面のずれを調整するため 0.2f を引いた
		yield return new WaitForSeconds(offsetTime - 0.2f);
		PlayMusic();
	}

	// Update is called once per frame
	void Update()
	{
		elapsedTime += Time.deltaTime;
	}

	//曲をセットする
	private void SetMusic()
	{
		audioSource.clip = Resources.Load<AudioClip>("Music/Songs/Song_" + musicName);
	}

	//曲を再生する
	private void PlayMusic()
	{
		audioSource.Play();
	}

	//譜面情報のファイルパスをセットする
	private void SetJson()
	{
		fumenPath = "Music/Json/Song_" + musicName;
	}

	//譜面情報を読み込む
	private void LoadJson()
	{
		this.fumen = JsonUtility.FromJson<MusicDTO.EditData>(Resources.Load<TextAsset>(this.fumenPath).ToString());
	}

	//引数の番号のノーツを叩くタイミングを計算する
	private double calculateTiming(int noteNum)
	{
		return fumen.notes[noteNum].num * (60.0 / (fumen.BPM * 4.0));
	}

	//レーンごとにノーツの叩くタイミングをArrayListに格納する
	private void NotesInfomationAdd()
	{
		for (int i = 0; i < fumen.notes.Count; i++)
		{
			switch (fumen.notes[i].block)
			{
				case 0:
					timing[0].Add(calculateTiming(i));
					break;
				case 1:
					timing[1].Add(calculateTiming(i));
					break;
				case 2:
					timing[2].Add(calculateTiming(i));
					break;
				case 3:
					timing[3].Add(calculateTiming(i));
					break;
				case 4:
					timing[4].Add(calculateTiming(i));
					break;
			}
		}

		Debug.Log(timing[0].Count.ToString());

		// for (int j = 0; j < timing[1].Count; j++)
		// {
		// 	if (timing[1][j] != null)
		// 		Debug.Log((double)timing[1][j]);
		// }
	}

	//ノーツの生成する座標を計算し、その後ノーツを生成する
	private void GenerateNotes()
	{
		double notesSpeed = notesMove.noteSpeed;

		//変数に座標を格納し、Instantiateでノーツを生成する
		double notesCoordinate;

		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < timing[i].Count; j++)
			{
				notesCoordinate = ((double)timing[i][j] * notesSpeed) + (-4.8f) + this.distance;
				if (i == 4)
				{
					notes[i].Add(Instantiate(XnotesPrefab, new Vector2(-5, (float)notesCoordinate), Quaternion.identity));
					notes[i].Add(Instantiate(XnotesPrefab, new Vector2(5, (float)notesCoordinate), Quaternion.identity));
				}
				else
				{
					notes[i].Add(Instantiate(notesPrefab, new Vector2((-3) + (i * 2), (float)notesCoordinate), Quaternion.identity));
				}
			}
		}
	}


}
