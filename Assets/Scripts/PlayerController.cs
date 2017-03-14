using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour {
	enum RPS {	Rock 		= 1,
				Paper 		= 2,
				Scissors 	= 3};

	public GameObject P1Drawer;
	public GameObject P2Drawer;

	private RPS P1RPSValue;
	private RPS P2RPSValue;
	private float P1ToggleCD = 0;
	private float P2ToggleCD = 0;

	public float ToggleCooldown = 0.2f;

	public float BoostTime = 0.4f;
	public float BoostDuration = 1;
	public float BoostValue = 0.2f;
	public float P1Boost = 0;
	public float P2Boost = 0;



	public KeyCode P1ToggleUp = KeyCode.UpArrow;
	public KeyCode P1ToggleDown = KeyCode.DownArrow;
	public KeyCode P2ToggleUp = KeyCode.W;
	public KeyCode P2ToggleDown = KeyCode.S;

	public KeyCode p1Push = KeyCode.KeypadEnter;
	public KeyCode p2Push = KeyCode.Space;
	public KeyCode ResetKey = KeyCode.Escape;

	public ParticleSystem P1System;
	public ParticleSystem P2System;


	public float acceleration = 0.5f;
	public float maxSpeed 	  = 20f;
	public float idleDecay    = 0.05f;
	public float RPSAdvantageModifier = 0.5f;

	private bool init	= false;
	private bool gameOver = false;
	//private bool P1Wins = true;
	private float playerSpeed;


	private CharacterController controller;
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();

		//initialize RPS values
		int i = Random.Range(1, 4);
		RPS tempRPS = RPS.Rock;
		switch (i) {
		case 1:
			tempRPS = RPS.Rock;
			break;
		case 2:
			tempRPS = RPS.Paper;
			break;
		case 3:
			tempRPS = RPS.Scissors;
			break;
		}
		P1Drawer.SendMessage ("Draw", i);
		P2Drawer.SendMessage ("Draw", i);
		P1RPSValue = tempRPS;
		P2RPSValue = tempRPS;

		playerSpeed = 0;
		//Init ();
	}
	
	// Update is called once per frame
	void FixedUpdate () { 

		//WASDInput ();

		if (Input.GetKeyDown (ResetKey)) {
			Scene scene = SceneManager.GetActiveScene ();
			SceneManager.LoadScene (scene.name);
		}

		bool P1PUSH = Input.GetKeyDown (p1Push);
		bool P2PUSH = Input.GetKeyDown (p2Push);
		if (!init) {
			if (P1PUSH) {
				P1Boost = -1 * BoostValue;
			}
			if (P2PUSH) {
				P2Boost = -1 * BoostValue;
			}

		} else {	

			if (BoostTime > 0) {
				if ((P1PUSH) && (P1Boost >= 0)) {
					P1Boost = BoostValue;
				}
				if ((P2PUSH) && (P2Boost >= 0)) {
					P2Boost = BoostValue;
				}
				BoostTime -= Time.deltaTime;
			}

			bool input = false;
			if (!gameOver) {
				ToggleControls ();

				int CompResult = compareRPS (P1RPSValue, P2RPSValue);

				if (P1PUSH) {

					float tPush = acceleration;
					if (CompResult == -1) {
						tPush += acceleration * RPSAdvantageModifier;
					}
					if (BoostDuration > 0) {
						tPush += acceleration * P1Boost;
					}

					playerSpeed = Mathf.Max (playerSpeed - tPush, -maxSpeed);

					if (P1System.isStopped) {
						P1System.Play ();
					}
					input = true;
				} else if (!P1System.isStopped)  {
					P1System.Stop ();
				}
				if (P2PUSH) {

					float tPush = acceleration;
					if (CompResult == 1) {
						tPush += acceleration * RPSAdvantageModifier;
					}
					if (BoostDuration > 0) {
						tPush += acceleration * P2Boost;
					}

					playerSpeed = Mathf.Min (playerSpeed + tPush, maxSpeed);
					input = true;
					if (P2System.isStopped) {
						P2System.Play ();
					}
				} else if (!P2System.isStopped)  {
					P2System.Stop ();
				}
			}

			if (BoostDuration > 0) {
				BoostDuration -= Time.deltaTime;
			}

			if (!input) {
				if (playerSpeed > 0) {
					playerSpeed = Mathf.Max (playerSpeed - idleDecay, 0);
				} else if (playerSpeed < 0) {
					playerSpeed = Mathf.Min (playerSpeed + idleDecay, 0);
				}

			}
			Vector3 movement = Vector3.right * playerSpeed;
			controller.Move (movement * Time.deltaTime);
		}
	}

	void ToggleControls() {
		
		if (P1ToggleCD <= 0) {
			if (Input.GetKeyDown (P1ToggleUp)) {
				P1RPSValue = toggleUP (P1RPSValue);
				P1ToggleCD = ToggleCooldown;
				P1Drawer.SendMessage("Draw", (int) P1RPSValue);
			} else if (Input.GetKeyDown (P1ToggleDown)) {
				P1RPSValue = toggleDOWN (P1RPSValue);
				P1ToggleCD = ToggleCooldown;
				P1Drawer.SendMessage("Draw", (int) P1RPSValue);
			}
		} else {
			P1ToggleCD = Mathf.Max (0, P1ToggleCD - Time.deltaTime);
		}

		if (P2ToggleCD <= 0) {
			if (Input.GetKeyDown (P2ToggleUp)) {
				P2RPSValue = toggleUP (P2RPSValue);
				P2ToggleCD = ToggleCooldown;
				P2Drawer.SendMessage("Draw", (int) P2RPSValue);
			} else if (Input.GetKeyDown (P2ToggleDown)) {
				P2RPSValue = toggleDOWN (P2RPSValue);
				P2ToggleCD = ToggleCooldown;
				P2Drawer.SendMessage("Draw", (int) P2RPSValue);
			}
		} else {
			P2ToggleCD = Mathf.Max (0, P2ToggleCD - Time.deltaTime);
		}
	}

	void Init() {
		init = true;
		//SendMessage ("Begin");
	}


	void EndGame() {
		gameOver = true;
	}
		

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("P1EndZone")) {
			SendMessage ("GameOver");
			EndGame ();
		} else if (other.CompareTag("P2EndZone")) {
			SendMessage ("GameOver");
			//P1Wins = false;
			EndGame ();
		}
	}

	private int compareRPS(RPS p1val, RPS p2val) {
		if ((((int) p1val) % 3 + 1) == ((int) p2val) ) {
			return -1;
		} else if ((((int) p2val) % 3 + 1) == ((int) p1val) ) {
			return 1;
		} else {
			return 0;
		}

	}

	private RPS toggleUP(RPS val) {
		int rval = (int)val + 1;
		switch (rval) {
		case 4:
			return RPS.Rock;
		case 2:
			return RPS.Paper;
		case 3:
			return RPS.Scissors;
		}
		return RPS.Rock;
	}

	private RPS toggleDOWN(RPS val) {
		int rval = (int)val - 1;
		switch (rval) {
		case 1:
			return RPS.Rock;
		case 2:
			return RPS.Paper;
		case 0:
			return RPS.Scissors;
		}
		return RPS.Rock;
	}
		
}
