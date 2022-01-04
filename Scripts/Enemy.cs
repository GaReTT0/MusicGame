using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	//MusicManagerクラスの変数を取得
	public MusicManager musicManager;

	//JsonファイルからEnemyのデータを取り出す
	public EnemyList enemyList;

	//EnemyのSprite
	private Sprite enemySprite;

	//EnemyのImage
	public Image enemyImage;

	// 振動の大きさ
	public int velocity;

	//経過時間
	private float time;

	//Imageのもとの位置
	private Vector2 originLocation;

	//Enemyの最大HP
	private float maxHP;

	//Enemyの現在のHP
	public float currentHP;

	//HPバー
	public Slider slider;


	// Start is called before the first frame update
	void Start()
	{
	}

	private void OnEnable()
	{
		musicManager = GameObject.FindWithTag("MusicManager").GetComponent<MusicManager>();
		LoadEnemyData();
		SetEnemyImage();
		originLocation = enemyImage.GetComponent<RectTransform>().localPosition;
		slider.value = 1;
		maxHP = currentHP = (float)enemyList.data[musicManager.fumen.musicNum].hp;
	}

	void Update() { }

	//Enemyのデータを含むJsonを読み込む
	private void LoadEnemyData()
	{
		enemyList = JsonUtility.FromJson<EnemyList>(Resources.Load<TextAsset>("Enemy/Json/EnemyData").ToString());
	}

	//EnemyのSpriteをImageに変換して表示させる
	private void SetEnemyImage()
	{
		Debug.Log(musicManager.fumen.musicNum.ToString());
		enemySprite = Resources.Load<Sprite>("Enemy/Image/" + enemyList.data[musicManager.fumen.musicNum].name);
		enemyImage = this.GetComponent<Image>();
		enemyImage.sprite = enemySprite;
	}

	//Imageを振動させる
	private IEnumerator Vibrate()
	{
		//振動させる間隔
		float span = 0.3f;
		float magnitude = 0.5f;
		while (time < span)
		{
			//-velocity ~ velocity の乱数
			// int value1 = Random.Range(velocity * -1, velocity + 1);
			// int value2 = Random.Range(velocity * -1, velocity + 1);

			float value1 = Random.Range(-1f, 1f) * magnitude;
			float value2 = Random.Range(-1f, 1f) * magnitude;


			// value1, value2 の分だけ移動
			Vector2 m_pos = enemyImage.GetComponent<RectTransform>().localPosition;
			m_pos.x += value1;
			m_pos.y += value2;
			enemyImage.GetComponent<RectTransform>().localPosition = m_pos;
			time += Time.deltaTime;
			yield return null;
		}
		//もとの位置へもどす
		enemyImage.GetComponent<RectTransform>().localPosition = originLocation;
		time = 0;
	}

	public void Damage()
	{
		int damage = 10;
		currentHP -= damage;
		slider.value = currentHP / maxHP;
		StartCoroutine(Vibrate());
	}

}




