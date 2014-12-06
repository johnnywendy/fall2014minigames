using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject shadow;
	public GameObject exitToMenu;
	private bool enabled = false;

	// Use this for initialization
	void Start () {
		Vector3 pos = transform.localPosition;
		transform.localPosition = new Vector3(pos.x,pos.y,Camera.main.transform.position.z+1);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			enabled = !enabled;
			shadow.SetActive(enabled);
			exitToMenu.SetActive(enabled);
		}
	}

	public void LoadScene(string sceneName) {
		Application.LoadLevel (sceneName);
	}
}
