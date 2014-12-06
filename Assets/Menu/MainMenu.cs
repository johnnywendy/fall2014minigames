using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Alert Box Usage Example
		/*GameObject alert = (GameObject)Instantiate(Resources.Load("Alert", typeof(GameObject)),Vector3.zero,Quaternion.identity);
		AlertBox alertBox = alert.GetComponent<AlertBox>();
		alertBox.title = "Hey Listen";
		alertBox.message = "Go to a scene?";
		alertBox.SetLeftAction("destroy");
		alertBox.SetRightAction("loadscene","LG-1");
		// OR
		//alertBox.SetRightAction(gameObject,"MainMenu","LoadScene","LG-1");*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadScene(string sceneName) {
		Application.LoadLevel (sceneName);
	}
}
