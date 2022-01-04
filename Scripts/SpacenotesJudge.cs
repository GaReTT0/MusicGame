using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacenotesJudge : NotesJudge
{
	//キーを押したかどうかを判定
	public override void IsBeat()
	{
		if (Input.GetKeyDown(key))
		{
			keyDownTiming = (float)musicManager.elapsedTime;
			Judgement();
			sePlayer.TapSecondPlay();
		}
	}
}
