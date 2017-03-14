using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icondrawer : MonoBehaviour {
	public GameObject rockSprite;
	public GameObject paperSprite;
	public GameObject scissorsSprite;

	void Start() {
//		int i = Random.Range (0, 3);
//		switch (i) {
//		case 0:
//			DrawRock ();
//			break;
//		case 1:
//			DrawPaper ();
//			break;
//		case 2:
//			DrawScissors ();
//			break;
//		}
	}
	// Use this for initialization
	void DrawRock(){
		rockSprite.GetComponent<SpriteRenderer> ().enabled = true;
		paperSprite.GetComponent<SpriteRenderer> ().enabled = false;
		scissorsSprite.GetComponent<SpriteRenderer> ().enabled = false;
	}
	void DrawPaper(){
		rockSprite.GetComponent<SpriteRenderer> ().enabled = false;
		paperSprite.GetComponent<SpriteRenderer> ().enabled = true;
		scissorsSprite.GetComponent<SpriteRenderer> ().enabled = false;
	}
	void DrawScissors(){
		rockSprite.GetComponent<SpriteRenderer> ().enabled = false;
		paperSprite.GetComponent<SpriteRenderer> ().enabled = false;
		scissorsSprite.GetComponent<SpriteRenderer> ().enabled = true;
	}


	void Draw(int i) {
		switch (i) {
		case 1:
			DrawRock ();
			break;
		case 2:
			DrawPaper ();
			break;
		case 3:
			DrawScissors ();
			break;
		}
	}
}
