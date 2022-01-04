using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{
	//tap1音
	[SerializeField] AudioClip tap1;
	//tap2音
	[SerializeField] AudioClip tap2;
	//damage音
	[SerializeField] AudioClip damage;
	//AudioSourceを取得
	private AudioSource SeSource;

	// Start is called before the first frame update
	void Start()
	{
		SeSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	//tap1音を再生
	public void TapFirstPlay()
	{
		SeSource.PlayOneShot(tap1);
	}

	//tap2音を再生
	public void TapSecondPlay()
	{
		SeSource.PlayOneShot(tap2);
	}

	//damage音を再生
	public void DamagePlay()
	{
		SeSource.PlayOneShot(damage);
	}
}
