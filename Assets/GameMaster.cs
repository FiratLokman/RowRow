using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {
	public static int Score = 0;
	public Text ScoreText;
	public static bool GameOver = false;

    void Start () {
        FindObjectOfType<Spawner>().spawnNext();
    }

	void Update(){
		ScoreText.text = Score.ToString ();
	}

	private void Awake()
	{
		ScoreText = FindObjectOfType<Canvas>().GetComponentInChildren<Text> ();
	}
}
