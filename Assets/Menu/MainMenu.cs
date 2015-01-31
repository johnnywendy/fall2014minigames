using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {

		// Alert Box Usage Example
		/*if (!PlayerPrefs.HasKey("CompletedFirstRun")) {

			GameObject alert = (GameObject)Instantiate(Resources.Load("Alert", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			AlertBox alertBox = alert.GetComponent<AlertBox>();
			alertBox.title = "Welcome";
			alertBox.message = "This is an example of an alert message.";
			alertBox.SetLeftAction("destroy");
			alertBox.SetRightAction("destroy");
			// OR
			//alertBox.SetRightAction(gameObject,"MainMenu","LoadScene","LG-1");

			PlayerPrefs.SetString("CompletedFirstRun","YES");
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadScene(string sceneName) {
		Application.LoadLevel (sceneName);
	}
}
