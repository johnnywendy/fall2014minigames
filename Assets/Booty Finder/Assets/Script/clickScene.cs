using UnityEngine;
using System.Collections;

public class clickScene : MonoBehaviour {
	
	public string treasureIsIn;
	public GameObject c;
	public ParticleSystem explosion;

	// Use this for initialization
	void Start () {
		explosion.Stop ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit = new RaycastHit ();
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.transform.tag == treasureIsIn) {
					Debug.Log ("This is where the treasure is!");
					treasureClick();
				} else if(hit.collider.transform.tag != "backgroundNotChest") {
					Debug.Log ("This is not where the treasure is!");
					notTreasureClick();
				}
			}
		}
	}
	
	void treasureClick() {
		GameObject alert = (GameObject)Instantiate(Resources.Load("Alert", typeof(GameObject)),Vector3.zero,Quaternion.identity);
		alert.transform.parent = c.transform;
		alert.transform.localScale = new Vector3(1,1,1);
		alert.transform.localPosition = new Vector3(0,0,0);
		AlertBox alertBox = alert.GetComponent<AlertBox>();
		alertBox.title = "Winner!";
		alertBox.message = "You figured out where the treasure is!";
		alertBox.leftButtonText = "Go back to Finder Menu";
		alertBox.rightButtonText = "Go back to Main Menu";
		alertBox.SetLeftAction ("loadscene","bootyMenu");
		alertBox.SetRightAction ("loadscene","MainMenu");
		explosion.Play ();
	}

	void notTreasureClick() {
		GameObject alert = (GameObject)Instantiate(Resources.Load("Alert", typeof(GameObject)),Vector3.zero,Quaternion.identity);
		alert.transform.parent = c.transform;
		alert.transform.localScale = new Vector3(1,1,1);
		alert.transform.localPosition = new Vector3(0,0,0);
		AlertBox alertBox = alert.GetComponent<AlertBox>();
		alertBox.title = "Sorry!";
		alertBox.message = "The treasure wasn't there. Oops. Sorry. Try again.";
		alertBox.leftButtonText = "Go back to Finder Menu";
		alertBox.rightButtonText = "Go back to Main Menu";
		alertBox.SetLeftAction ("loadscene","bootyMenu");
		alertBox.SetRightAction ("loadscene","MainMenu");
	}
}
