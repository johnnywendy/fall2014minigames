using UnityEngine;
using System.Collections;

public class ResetGameData : MonoBehaviour {

	private bool mouseDown = false;

	void OnMouseDown() {
		mouseDown = true;
	}

	void OnMouseUp() {
		if (mouseDown) {
			GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			MessageBox alertBox = alert.GetComponent<MessageBox>();
			alertBox.message = "Are you sure you want\nto reset all game data?";
			alertBox.rightButtonText = "RESET";
			alertBox.SetLeftAction("destroy");
			alertBox.SetRightAction(this.gameObject,"ResetGameData","ResetConfirmed");
		}
		mouseDown = false;
	}

	void OnMouseExit() {
		mouseDown = false;
	}

	void OnMouseEnter() {
		mouseDown = true;
	}

	public void ResetConfirmed() {
		GameData.ForceReRegisterGame(0,10);
		GameData.ForceReRegisterGame(1,8);
		GameData.ForceReRegisterGame(2,6);
		GameData.ForceReRegisterGame(3,8);
		GameData.ForceReRegisterGame(4,6);
		GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
		MessageBox alertBox = alert.GetComponent<MessageBox>();
		Camera.main.GetComponent<MenuManager>().SetPercentage();
		alertBox.message = "Game data reset successfully";
		alertBox.SetLeftAction("destroy");
		alertBox.SetRightAction("destroy");
	}
}
