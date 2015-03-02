using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TruthTable : MonoBehaviour {

	public List<GameObject> As;
	public List<GameObject> Bs;
	public List<GameObject> Outs;
	public List<GameObject> Res;

	private List<List<bool>> truthTable;
	private List<List<bool>> goalTable;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetTable(List<List<bool>> newTable) {
		goalTable = newTable;
		if (goalTable[0].Count == 2) {
			for (int i = 2; i < 4; i++) {
				As[i].SetActive(false);
				Bs[i].SetActive(false);
				Outs[i].SetActive(false);
				Res[i].SetActive(false);
			}
			if (goalTable[0].Count == 2)
				for (int i = 0; i < Bs.Count; i++) {
					Bs[i].SetActive(false);
				}
			else
				for (int i = 0; i < 2; i++)
					Bs[i].SetActive(true);
		}
		else {
			for (int i = 0; i < 4; i++) {
				As[i].SetActive(true);
				Bs[i].SetActive(true);
				Outs[i].SetActive(true);
				Res[i].SetActive(true);
			}
		}
		for (int i = 0; i < goalTable.Count; i++ ) {
			for (int j = 0; j < goalTable[0].Count; j++) {
				switch(j) {
				case 0:
					HexColor.SetColor(As[i],(goalTable[i][j] ? GameColors.on : GameColors.off));
					break;
				case 1:
					if (goalTable[0].Count == 2)
						HexColor.SetColor(Outs[i],(goalTable[i][j] ? GameColors.on : GameColors.off));
					else
						HexColor.SetColor(Bs[i],(goalTable[i][j] ? GameColors.on : GameColors.off));
					break;
				case 2:
					HexColor.SetColor(Outs[i],(goalTable[i][j] ? GameColors.on : GameColors.off));
					break;
				default:
					break;
				}
			}
		}
		UpdateTable(newTable);
	}

	public void UpdateTable(List<List<bool>> table) {
		for (int i = 0; i < table.Count; i++ ) {
			bool equals = (table[i].TrueForAll(goalTable[i].Contains) && goalTable[i].TrueForAll(table[i].Contains));
			GoalGate ggate = GameObject.Find("GoalBlock").GetComponent<GoalGate>();
			if (equals && ggate.plugged && ggate.powered) {
				Res[i].GetComponent<CheckOrX>().val = true;
				HexColor.SetColor(Res[i],GameColors.on2);
				Debug.Log("OH MY GOD ITS FUCKING TRUE");
			}
			else {
				Res[i].GetComponent<CheckOrX>().val = false;
				HexColor.SetColor(Res[i],GameColors.off2);
				Debug.Log("OH MY GOD ITS FUCKING FALSE");
			}
		}
	}

}
