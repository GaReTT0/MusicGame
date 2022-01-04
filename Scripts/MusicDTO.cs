using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoteEditor.DTO
{
	//Jsonファイルの構造を表現するクラス
	public class MusicDTO
	{
		[System.Serializable]
		public class EditData
		{
			public string name;
			public int maxBlock;
			public int BPM;
			public int offset;
			public int musicNum;
			public float songTime;
			public List<Note> notes;
		}

		[System.Serializable]
		public class Note
		{
			public int LPB;
			public int num;
			public int block;
		}
	}
}
