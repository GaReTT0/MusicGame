using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NotesMove : MonoBehaviour
{
	//ノーツの移動速度
	public float noteSpeed;

	//ノーツの物理演算可能にする
	private Rigidbody2D rb;

	// Update is called once per frame
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		rb.velocity = new Vector2(rb.velocity.x, (-1) * noteSpeed);
	}
}
