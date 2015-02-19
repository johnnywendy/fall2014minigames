using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LGManager_1 : MonoBehaviour {

	private bool isHoldingObj = false;
	private bool isHoldingPow = false;
	private bool isPositioningObj = false;
	private bool isConnectingObj = false;
	private bool isEditing = false;

	private GameObject plugObj;
	private GameObject socketObj;
	private GameObject heldObj;

	// Inventory Stuff
	public List<GameObject> buttons;
	public int[] InvAmounts = new int[] {1,1,1,1,1,1,1};
	public bool[] InvEnabled = new bool[] {true,true,true,true,true,true,true};
	private int screenWidth = 0;

	// Level Management
	public int level = 1;
	private List<List<bool>> truthTable = null;
	private List<PowerSource> powerSources = new List<PowerSource> {};
	private GoalGate goalGate = null;

	List<List<bool>> level1;
	int[] inv1 = new int[] {0,0,0,2,0,0,0};
	string message1 = "Congratulations, you just created a NOT gate.";
	List<List<bool>> level2;
	int[] inv2 = new int[] {0,0,2,2,0,0,0};
	string message2 = "Congratulations, you just created an AND gate.";
	List<List<bool>> level3;
	int[] inv3 = new int[] {2,0,2,2,0,0,0};
	string message3 = "Congratulations, you just created an OR gate.";

	// Use this for initialization
	void Start () {
		screenWidth = Screen.width;
		goalGate = GameObject.Find("GoalBlock").GetComponent<GoalGate>() as GoalGate;

		for (int i = 0; i < InvAmounts.Length; i++) {
			buttons[i].SetActive(InvEnabled[i]);
			buttons[i].transform.FindChild("Amount").GetComponent<Text>().text = InvAmounts[i].ToString();
		}

		List<bool> row1 = new List<bool>() {false,true};
		List<bool> row2 = new List<bool>() {true,false};
		level1 = new List<List<bool>>() {row1,row2};

		row1 = new List<bool>() {false,false,false};
		row2 = new List<bool>() {true,false,false};
		List<bool> row3 = new List<bool>() {true,true,true};
		List<bool> row4 = new List<bool>() {false,true,false};
		level2 = new List<List<bool>>() {row1,row2,row3,row4};

		row1 = new List<bool>() {false,false,false};
		row2 = new List<bool>() {true,false,true};
		row3 = new List<bool>() {true,true,true};
		row4 = new List<bool>() {false,true,false};
		level3 = new List<List<bool>>() {row1,row2,row3,row4};

		SetupNewLevel(level);
	}

	// Update is called once per frame
	void Update () {
		if (isHoldingObj) {
			heldObj.transform.position = VisibleMousePosition();
		}

		// On Left Click
		if (Input.GetMouseButtonDown (0)) {
			if (isHoldingObj || isEditing) {
				if (isHoldingObj) {
					isHoldingObj = false;
					LogicGate gate = heldObj.GetComponent<LogicGate> ();
					if (gate != null) {
						gate.enabled = true;
						gate.WasPutDown();
						heldObj = null;
					}
				}
				if (isEditing) {
					RaycastHit hit = new RaycastHit();
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					
					if (Physics.Raycast(ray, out hit)) {
						if (hit.collider.transform.gameObject.name == "Reset") {
							Debug.Log("hit");
							hit.collider.transform.parent.gameObject.GetComponent<LogicGate>().resetConnections();
						}
						if (hit.collider.transform.gameObject.name == "Destroy") {
							hit.collider.transform.parent.gameObject.GetComponent<LogicGate>().resetConnections();
							int index = hit.collider.transform.parent.gameObject.GetComponent<LogicGate>().logicMode;
							InvAmounts[index]++;
							buttons[index].transform.FindChild("Amount").GetComponent<Text>().text = InvAmounts[index].ToString();
							Destroy(hit.collider.transform.parent.gameObject);
						}
					}
				}
			}
			else {
				RaycastHit hit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit)) {
					if (!isConnectingObj) {
						if (hit.collider.transform.gameObject.name == "Output") {
							Debug.Log("> Output selected");
							plugObj = hit.collider.transform.parent.gameObject;
							if (hit.collider.tag == "IO") {
								plugObj.GetComponent<LogicGate>().NeedNewCable();
							}
							else if (hit.collider.tag == "Power") {
								isHoldingPow = true;
								plugObj.GetComponent<PowerSource>().NeedNewCable();
							}
							isConnectingObj = true;
						}
						else if (hit.collider.tag == "Power") {
							hit.collider.gameObject.GetComponent<PowerSource>().FlipOutput();
						}
						if (hit.collider.transform.gameObject.tag == "Gate") {
							HoldObject(hit.collider.transform.gameObject);
						}
					}
					else if (isConnectingObj && (hit.collider.transform.gameObject.name == "Input1" || hit.collider.transform.gameObject.name == "Input2")) {
						Debug.Log(plugObj.tag);
						Debug.Log(plugObj.name);
						if (plugObj.tag == "Power") {
							Debug.Log("> Input selected");
							socketObj = hit.collider.transform.gameObject;
							isConnectingObj = false;
							isHoldingPow = false;
							PowerSource pow = plugObj.GetComponent<PowerSource>();

							if (hit.collider.transform.gameObject.name == "Input1") {
								if (socketObj.transform.parent.GetComponent<LogicGate>() != null) {
									socketObj.transform.parent.GetComponent<LogicGate>().HookUpPowerSource(pow,"left");
									pow.PlugIntoGate(socketObj.transform.parent.GetComponent<LogicGate>(), "left");
								}
								else {
									plugObj.GetComponent<PowerSource>().DestroyNewCable();
									isConnectingObj = false;
									isHoldingPow = false;
								}
							}
							if (hit.collider.transform.gameObject.name == "Input2") {
								socketObj.transform.parent.GetComponent<LogicGate>().HookUpPowerSource(pow,"right");
								pow.PlugIntoGate(socketObj.transform.parent.GetComponent<LogicGate>(), "right");
							}
						}
						else if (plugObj.tag == "Gate" && hit.collider.tag != "Goal") {
							Debug.Log("> Input selected");
							socketObj = hit.collider.transform.gameObject;
							isConnectingObj = false;
							if (hit.collider.transform.gameObject.name == "Input1")
								plugObj.GetComponent<LogicGate>().PlugIntoGate(socketObj.transform.parent.GetComponent<LogicGate>(),"left");
							if (hit.collider.transform.gameObject.name == "Input2")
								plugObj.GetComponent<LogicGate>().PlugIntoGate(socketObj.transform.parent.GetComponent<LogicGate>(),"right");
						}
						else if (hit.collider.tag == "Goal") {
							socketObj = hit.collider.transform.gameObject;
							isConnectingObj = false;
							plugObj.GetComponent<LogicGate>().PlugIntoGoal(socketObj.transform.parent.GetComponent<GoalGate>());
						}
					}
				}
				else if (isConnectingObj) {
					if (isHoldingPow)
						plugObj.GetComponent<PowerSource>().DestroyNewCable();
					else
						plugObj.GetComponent<LogicGate>().DestroyNewCable();
					isConnectingObj = false;
					isHoldingPow = false;
				}
			}
		}
		// On Right Click
		if (Input.GetMouseButtonDown (1)) {
			if (!isHoldingObj) {
				if (isEditing)
					isEditing = false;
				else
					isEditing = true;
				foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Gate")) {
					obj.GetComponent<LogicGate>().SetEditMode(isEditing);
				}
			}
		}
	}

	public Vector3 VisibleMousePosition() {
		Vector3 mousPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		return new Vector3 (mousPos.x, mousPos.y, -0.11636f);
	}

	public bool CanHoldObj() {
		if (isHoldingObj || isConnectingObj || isHoldingPow || isPositioningObj)
			return false;
		else
			return true;
	}

	public void HoldObject(GameObject obj) {
		isHoldingObj = true;
		LogicGate gate = obj.GetComponent<LogicGate> ();
		if (gate != null) {
			gate.WasPickedUp();
			gate.shouldDisable(true);
			heldObj = obj;
		}
	}

	public void ButtonClicked(GameObject clickedObj) {
		isEditing = false;
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Gate")) {
			obj.GetComponent<LogicGate>().SetEditMode(isEditing);
		}
		if (CanHoldObj()) {
			bool shouldDelete = false;
			GameObject newGate = (GameObject)Instantiate(Resources.Load("LogicGate", typeof(GameObject)),Camera.main.ScreenToWorldPoint(Input.mousePosition),Quaternion.identity);
			if (clickedObj.name == "AndButton" && InvAmounts[0] > 0) {
				InvAmounts[0] -= 1;
				newGate.GetComponent<LogicGate>().mode = 0;
				buttons[0].transform.FindChild("Amount").GetComponent<Text>().text = InvAmounts[0].ToString();
			}
			else if (clickedObj.name == "OrButton" && InvAmounts[1] > 0) {
				InvAmounts[1] -= 1;
				newGate.GetComponent<LogicGate>().mode = 1;
				buttons[1].transform.FindChild("Amount").GetComponent<Text>().text = InvAmounts[1].ToString();
			}
			else if (clickedObj.name == "NotButton" && InvAmounts[2] > 0) {
				InvAmounts[2] -= 1;
				newGate.GetComponent<LogicGate>().mode = 2;
				buttons[2].transform.FindChild("Amount").GetComponent<Text>().text = InvAmounts[2].ToString();
			}
			else if (clickedObj.name == "NandButton" && InvAmounts[3] > 0) {
				InvAmounts[3] -= 1;
				newGate.GetComponent<LogicGate>().mode = 3;
				buttons[3].transform.FindChild("Amount").GetComponent<Text>().text = InvAmounts[3].ToString();
			}
			else if (clickedObj.name == "NorButton" && InvAmounts[4] > 0) {
				InvAmounts[4] -= 1;
				newGate.GetComponent<LogicGate>().mode = 4;
				buttons[4].transform.FindChild("Amount").GetComponent<Text>().text = InvAmounts[4].ToString();
			}
			else if (clickedObj.name == "XorButton" && InvAmounts[5] > 0) {
				InvAmounts[5] -= 1;
				newGate.GetComponent<LogicGate>().mode = 5;
				buttons[5].transform.FindChild("Amount").GetComponent<Text>().text = InvAmounts[5].ToString();
			}
			else if (clickedObj.name == "XnorButton" && InvAmounts[6] > 0) {
				InvAmounts[6] -= 1;
				newGate.GetComponent<LogicGate>().mode = 6;
				buttons[6].transform.FindChild("Amount").GetComponent<Text>().text = InvAmounts[6].ToString();
			}
			else {
				Destroy(newGate);
				return;
			}
			newGate.GetComponent<LogicGate>().preferredEditMode = isEditing;
			HoldObject(newGate);
		}
	}

	void ClearFloor() {
		foreach (GameObject gate in GameObject.FindGameObjectsWithTag("Gate")) {
			LogicGate lGate = gate.GetComponent<LogicGate>();
			lGate.resetConnections();
			int index = lGate.logicMode;
			InvAmounts[index]++;
			buttons[index].transform.FindChild("Amount").GetComponent<Text>().text = InvAmounts[index].ToString();
			Destroy(gate);
		}
		foreach (PowerSource pow in powerSources) {
			foreach (Cable cable in pow.cables) {
				GameObject.Destroy(cable.gameObject);
			}
			GameObject.Destroy(pow.gameObject);
		}
		powerSources = new List<PowerSource>();
	}
	
	public void SetupNewLevel(int level) {
		ClearFloor ();
		if (level == 1) {
			truthTable = level1;
			InvAmounts = inv1;
		}
		if (level == 2) {
			truthTable = level2;
			InvAmounts = inv2;
		}
		if (level == 3) {
			truthTable = level3;
			InvAmounts = inv3;
		}
		for (int i = 0; i < InvAmounts.Length; i++) {
			buttons[i].SetActive(InvEnabled[i]);
			buttons[i].transform.FindChild("Amount").GetComponent<Text>().text = InvAmounts[i].ToString();
		}
		int numberOfSources = truthTable[0].Count-1;
		if (numberOfSources == 1) {
			GameObject powerSource1 = (GameObject)Instantiate(Resources.Load("PowerSource", typeof(GameObject)),new Vector3(-4.8f,1.7f,-0.5f),Quaternion.identity);
			powerSources.Add(powerSource1.GetComponent<PowerSource>());
		}
		if (numberOfSources == 2) {
			GameObject powerSource1 = (GameObject)Instantiate(Resources.Load("PowerSource", typeof(GameObject)),new Vector3(-4.8f,2.85f,-0.5f),Quaternion.identity);
			powerSources.Add(powerSource1.GetComponent<PowerSource>());
			GameObject powerSource2 = (GameObject)Instantiate(Resources.Load("PowerSource", typeof(GameObject)),new Vector3(-4.8f,0.55f,-0.5f),Quaternion.identity);
			powerSources.Add(powerSource2.GetComponent<PowerSource>());
		}
		if (numberOfSources == 3) {
			GameObject powerSource1 = (GameObject)Instantiate(Resources.Load("PowerSource", typeof(GameObject)),new Vector3(-4.8f,4f,-0.5f),Quaternion.identity);
			powerSources.Add(powerSource1.GetComponent<PowerSource>());
			GameObject powerSource2 = (GameObject)Instantiate(Resources.Load("PowerSource", typeof(GameObject)),new Vector3(-4.8f,1.7f,-0.5f),Quaternion.identity);
			powerSources.Add(powerSource2.GetComponent<PowerSource>());
			GameObject powerSource3 = (GameObject)Instantiate(Resources.Load("PowerSource", typeof(GameObject)),new Vector3(-4.8f,-0.6f,-0.5f),Quaternion.identity);
			powerSources.Add(powerSource3.GetComponent<PowerSource>());
		}
	}

	public void CheckAnswer() {
		List<List<bool>> currentTable = new List<List<bool>>();
		foreach (PowerSource input in powerSources) {
			input.SetOutput(false);
		}
		if (powerSources.Count == 1) {
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			currentTable.Add(GetRow());
		}
		if (powerSources.Count == 2) {
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[1].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			currentTable.Add(GetRow());
		}
		if (powerSources.Count == 3) {
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[1].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[2].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[1].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			powerSources[1].FlipOutput();
			powerSources[2].FlipOutput();
			currentTable.Add(GetRow());
		}
		CheckCompletion(currentTable);
	}

	List<bool> GetRow() {
		List<bool> currentScenario = new List<bool>();
		foreach (PowerSource input in powerSources)
			currentScenario.Add (input.output);
		currentScenario.Add(goalGate.input);
		return currentScenario;
	}

	public void CheckScenario() {
		List<bool> currentScenario = new List<bool>();
		foreach (PowerSource input in powerSources)
			currentScenario.Add (input.output);
		UpdateTruthTable(currentScenario,goalGate.input);
	}

	public void UpdateTruthTable(List<bool> currentScenario, bool currentOutput) {
		int scenarioIndex = 0;
		if (currentScenario[0] == true)
			scenarioIndex += 1;
		if (currentScenario.Count == 2)
			if (currentScenario[1] == true)
				scenarioIndex += 10;
		if (currentScenario.Count == 3)
			if (currentScenario[2] == true)
				scenarioIndex += 100;
		List<int> indexMap = new List<int> {0,1,10,11,100,101,110,111};
		int actualIndex = -1;
		for (int i = 0; i < indexMap.Count; i++)
			if (indexMap[i] == scenarioIndex)
				actualIndex = indexMap[i];
		if (actualIndex > -1) {
			currentScenario.Add(currentOutput);
			CheckScenarioValidity(actualIndex,currentScenario);
		}
	}

	void CheckScenarioValidity(int index,List<bool> currentScenario) {
		bool correct = true;
		for (int i = 0; i < currentScenario.Count; i++) {
			Debug.Log(truthTable[index][i]+" --- "+currentScenario[i]);
		}
		for (int i = 0; i < currentScenario.Count; i++) {
			if (truthTable[index][i] != currentScenario[i])
				correct = false;
		}
		if (correct)
			Debug.Log("CORRECT");
		else
			Debug.Log("INCORRECT");
	}

	void CheckCompletion(List<List<bool>> currentAnswer) {
		bool correct = true;
		for (int i = 0; i < currentAnswer.Count; i++) {
			for (int j = 0; j < currentAnswer[i].Count; j++) {
				if (truthTable[i][j] != currentAnswer[i][j])
					correct = false;
			}
		}
		if (correct) {
			Debug.Log("CORRECT");
			GameObject alert = (GameObject)Instantiate(Resources.Load("Alert", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			AlertBox alertBox = alert.GetComponent<AlertBox>();
			alertBox.title = "Correct";
			if (level == 1)
				alertBox.message = message1;
			if (level == 2)
				alertBox.message = message2;
			if (level == 3)
				alertBox.message = message3;
			alertBox.SetLeftAction("destroy");
			level++;
			if (level == 3) {
				alertBox.rightButtonText = "MENU";
				alertBox.SetRightAction("loadscene","MainMenu");
			}
			else {
				alertBox.rightButtonText = "NEXT";
				alertBox.SetRightAction(gameObject,"LGManager_1","SetupNewLevel",level);
			}
		}
		else {
			Debug.Log("INCORRECT");
			GameObject alert = (GameObject)Instantiate(Resources.Load("Alert", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			AlertBox alertBox = alert.GetComponent<AlertBox>();
			alertBox.title = "Incorrect";
			alertBox.message = "Hmm, this doesn't look correct, try again.";
			alertBox.SetLeftAction("destroy");
			alertBox.SetRightAction("destroy");
		}
	}
}
