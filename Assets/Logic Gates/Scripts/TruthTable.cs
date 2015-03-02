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
		UpdateTable(newTable);
	}

	public void UpdateTable(List<List<bool>> table) {
		for (int i = 0; i < table.Count; i++ ) {
			for (int j = 0; j < table[0].Count; j++) {
				switch(j) {
					case 0:
						HexColor.SetColor(As[i],(table[i][j] ? GameColors.on : GameColors.off));
						break;
					case 1:
						if (table[0].Count == 2)
							HexColor.SetColor(Outs[i],(table[i][j] ? GameColors.on : GameColors.off));
						else
							HexColor.SetColor(Bs[i],(table[i][j] ? GameColors.on : GameColors.off));
						break;
					case 2:
						HexColor.SetColor(Outs[i],(table[i][j] ? GameColors.on : GameColors.off));
						break;
					default:
						break;
				}
			}
			//Res[i].GetComponent<CheckOrX>().val = (table[i] == goalTable[i]);
			//HexColor.SetColor(Res[i],((table[i] == goalTable[i]) ? GameColors.on2 : GameColors.off2));
		}
	}

}
