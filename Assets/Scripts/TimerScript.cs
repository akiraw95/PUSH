using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {
	public Text timerText;
	public Text gameText;


	public float defaultTime = 120;
	public float currentTime;

	public float randStartTime;

	private float startTime;
	private bool finish = false;
	private bool start = false;
	// Use this for initialization
	void Start () {
		startTime = Time.time;
		currentTime = defaultTime;
		timerText.color = Color.white;

		gameText.text = "Ready!";
		randStartTime = Random.Range (0.5f, 3.0f);


		float displayTime = currentTime;
		if (displayTime > 0) {
			int minute = ((int)displayTime / 60);
			string minuteS = minute.ToString ();
			if (minute < 10) {
				minuteS = "0" + minuteS;
			}

			float second = (displayTime % 60);
			string secondS = second.ToString ("f2");
			if (second < 10) {
				secondS = "0" + secondS;
			}

			timerText.text = (minuteS + ":" + secondS);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!start) {
			if (randStartTime > 0) {
				randStartTime -= Time.deltaTime;
			} else if (randStartTime <= 0) {
				Begin ();
				SendMessage ("Init");
				gameText.text = "Go!";
			}
		}


		if ((finish) || (!start))
			return;


		float timeEllapsed = Time.time - startTime;
		if (timeEllapsed > 5) {
			gameText.text = " ";
		}
		float displayTime = currentTime - timeEllapsed;
		if (displayTime > 0) {
			int minute = ((int)displayTime / 60);
			string minuteS = minute.ToString ();
			if (minute < 10) {
				minuteS = "0" + minuteS;
			}

			float second = (displayTime % 60);
			string secondS = second.ToString ("f2");
			if (second < 10) {
				secondS = "0" + secondS;
			}

			timerText.text = (minuteS + ":" + secondS);
		} else {
			timerText.text = ("00:00.00");
			SendMessage ("EndGame");
			gameText.text = "Tie! ESC to restart";
			Finish ();
		}
	}

	public void Begin() {
		start = true;
		startTime = Time.time;
		timerText.color = Color.red;

	}

	public void GameOver() {
		gameText.text = "Game over! ESC to restart";
		Finish ();
	}

	public void WinTimer() {
		gameText.text = "Game over! ESC to restart";
		Finish ();
	}

	public void Finish() {
		timerText.color = Color.white;
		finish = true;
	}

}
