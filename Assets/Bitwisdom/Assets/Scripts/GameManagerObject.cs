using UnityEngine;
using UnityEngine.UI;
using Bitwise;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GameManagerObject : MonoBehaviour {
	
	//-----------------------------------------
	//------------- Initialization ------------
	//-----------------------------------------
	
	public BitwiseInt num1,
					  num2,
					  num3,
					  num4,
					  num5;
	
	public int max,
			current_lvl;
	
	public GameObject c,
					  btn_and,
					  btn_or,
					  btn_not,
					  btn_xor,
					  btn_num1,
					  btn_num2,
					  btn_num3,
					  btn_num4,
					  btn_num5,
					  targetNum;
	
	void Start () {
		if (!PlayerPrefs.HasKey ("BW_PuzzleLevelUnlocked")) {
			PlayerPrefs.SetInt ("BW_PuzzleLevelUnlocked",1);
		}
		
		if (!PlayerPrefs.HasKey ("BW_CurrentLevel")) {
			PlayerPrefs.SetInt ("BW_CurrentLevel", 1);
			current_lvl = 1;
		} else {
			current_lvl = PlayerPrefs.GetInt ("BW_CurrentLevel");
		}


		string path = System.IO.Directory.GetCurrentDirectory ()+"/Assets/Bitwisdom/Assets/Resources/bw_puzzleList.txt";
		string[] fileText = System.IO.File.ReadAllLines (path),
				 lvlLoadout;
		lvlLoadout = fileText[current_lvl-1].Split (' ');

		//-----------------------------------------
		//------------- Level Building ------------
		//-----------------------------------------
		int operators;
		string target;

		bool _and,
	 		 _or,
			 _not,
			 _xor;

		max = System.Convert.ToInt32 (lvlLoadout [0]);
		operators = System.Convert.ToInt32 (lvlLoadout [1]);
		num1 = new BitwiseInt(System.Convert.ToInt32 (lvlLoadout [2]), max);
		num2 = new BitwiseInt(System.Convert.ToInt32 (lvlLoadout [3]), max);
		num3 = new BitwiseInt(System.Convert.ToInt32 (lvlLoadout [4]), max);
		num4 = new BitwiseInt(System.Convert.ToInt32 (lvlLoadout [5]), max);
		num5 = new BitwiseInt(System.Convert.ToInt32 (lvlLoadout [6]), max);
		target = lvlLoadout [7];

		if ((operators / 8) == 1) {
			_xor = true;
			operators -= 8;
		} else {
			_xor = false;
				}
		if ((operators / 4) == 1) {
			_not = true;
			operators -= 4;
		} else {
			_not = false;
		}
		if ((operators / 2) == 1) {
			_or = true;
			operators -= 2;
		} else {
			_or = false;
		}
		if ((operators / 1) == 1) {
			_and = true;
			operators -= 1;
		} else {
			_and = false;
		}

		btn_and.GetComponent<UnityEngine.UI.Button>().interactable = _and;
		btn_or.GetComponent<UnityEngine.UI.Button>().interactable = _or;
		btn_not.GetComponent<UnityEngine.UI.Button>().interactable = _not;
		btn_xor.GetComponent<UnityEngine.UI.Button>().interactable = _xor;

		btn_num1.GetComponentInChildren<Text>().text = num1.value.ToString ();
		btn_num2.GetComponentInChildren<Text>().text = num2.value.ToString ();
		btn_num3.GetComponentInChildren<Text>().text = num3.value.ToString ();
		btn_num4.GetComponentInChildren<Text>().text = num4.value.ToString ();
		btn_num5.GetComponentInChildren<Text>().text = num5.value.ToString ();

		targetNum.GetComponentInChildren<Text>().text = target;
	}
	
	//-----------------------------------------
	//------------ Input Management -----------
	//-----------------------------------------
	
	public GameObject inputField;		

	public void OnClickInputButton(GameObject srcBtn){
		Text fieldText,
			 buttonText;
		
		fieldText = inputField.GetComponentInChildren<Text> ();
		buttonText = srcBtn.GetComponentInChildren<Text> ();
		fieldText.text = fieldText.text + buttonText.text + " ";
		
		if (srcBtn.CompareTag ("Number")) {
			UnityEngine.UI.Button btn = srcBtn.GetComponent<UnityEngine.UI.Button>();
			btn.interactable = false;
		}
	}

	public void OnClickSumbitButton(){
		BitwiseInt result;
		int target = System.Int32.Parse (targetNum.GetComponentInChildren<Text> ().text);
		string formula = inputField.GetComponentInChildren<Text> ().text.Replace ("∧", "&").Replace ("∨", "|").Replace ("⊕", "^");
		
		FormulaConverter fc = new FormulaConverter ();
		result = fc.Eval (formula);
		if (fc.valid) {
			CheckSubmission(result.value, target);
		}
		else {
			GameObject alert = (GameObject)Instantiate(Resources.Load("Alert", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			alert.transform.parent = c.transform;
			alert.transform.localScale = new Vector3(1,1,1);
			AlertBox alertBox = alert.GetComponent<AlertBox>();
			alertBox.title = "Syntax Error";
			alertBox.message = "There was an issue with the equation you entered. Please try again.";
			alertBox.leftButtonText = "Close";
			alertBox.rightButtonText = "Retry?";
			alertBox.SetLeftAction ("destroy");
			alertBox.SetRightAction ("loadscene","PuzzleMode");
		}
	}
	
	public void ClearInputField(){
		inputField.GetComponentInChildren<Text> ().text = "";
		ResetButtons ();
	}
	
	public void ResetButtons(){
		UnityEngine.UI.Button btn1 = btn_num1.GetComponent<UnityEngine.UI.Button>(),
							  btn2 = btn_num2.GetComponent<UnityEngine.UI.Button>(),
							  btn3 = btn_num3.GetComponent<UnityEngine.UI.Button>(),
							  btn4 = btn_num4.GetComponent<UnityEngine.UI.Button>(),
							  btn5 = btn_num5.GetComponent<UnityEngine.UI.Button>();
		
		btn1.interactable = true;
		btn2.interactable = true;
		btn3.interactable = true;
		btn4.interactable = true;
		btn5.interactable = true;
	}
	
	public void CheckSubmission(int i1, int i2){
		if (i1 == i2) {
			if (PlayerPrefs.GetInt ("BW_PuzzleLevelUnlocked") == current_lvl)
			{
				PlayerPrefs.SetInt ("BW_PuzzleLevelUnlocked", ++current_lvl);
			}
			PlayerPrefs.SetInt ("BW_CurrentLevel",current_lvl);
			GameObject alert = (GameObject)Instantiate(Resources.Load("Alert", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			alert.transform.parent = c.transform;
			alert.transform.localScale = new Vector3(1,1,1);
			AlertBox alertBox = alert.GetComponent<AlertBox>();
			alertBox.title = "Correct!";
			alertBox.message = "You got it right!";
			alertBox.leftButtonText = "Main Menu";
			alertBox.rightButtonText = "Next Level";
			alertBox.SetLeftAction ("loadscene", "MainMenu");
			alertBox.SetRightAction ("loadscene","PuzzleMode");
		} else {
			GameObject alert = (GameObject)Instantiate(Resources.Load("Alert", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			alert.transform.parent = c.transform;
			alert.transform.localScale = new Vector3(1,1,1);
			AlertBox alertBox = alert.GetComponent<AlertBox>();
			alertBox.title = "Incorrect";
			alertBox.message = "That wasn't quite right";
			alertBox.leftButtonText = "Main Menu";
			alertBox.rightButtonText = "Retry?";
			alertBox.SetLeftAction ("loadscene", "MainMenu");
			alertBox.SetRightAction ("loadscene","PuzzleMode");
		}
	}
}

//-----------------------------------------
//----------- Formula Management ----------
//-----------------------------------------

public class FormulaConverter
{
	private string[] _operators = { "&", "|", "^", "~" };
	private  System.Func<BitwiseInt, BitwiseInt, BitwiseInt>[] _operations = {
		(a1, a2) => a1 & a2,
		(a1, a2) => a1 | a2,
		(a1, a2) => a1 ^ a2,
		(a1, a2) => ~a1
	};
	public bool valid = true;
	
	public BitwiseInt Eval(string expression)
	{
		List<string> tokens = getTokens(expression);
		Stack<BitwiseInt> operandStack = new Stack<BitwiseInt>();
		Stack<string> operatorStack = new Stack<string>();
		int tokenIndex = 0;
		
		while (tokenIndex < tokens.Count) {
			if(!valid){
				break;
			}
			string token = tokens[tokenIndex];
			if (token == "(") {
				string subExpr = getSubExpression(tokens, ref tokenIndex);
				operandStack.Push(Eval(subExpr));
				continue;
			}
			if (token == ")") {
				valid = false;
				break;
			}
			//If this is an operator  
			if (System.Array.IndexOf(_operators, token) >= 0) {
				while (operatorStack.Count > 0 && System.Array.IndexOf(_operators, token) < System.Array.IndexOf(_operators, operatorStack.Peek())) {
					string op = operatorStack.Pop();
					BitwiseInt arg1,arg2;
					if (op == "~"){
						arg2 = new BitwiseInt(0);
						arg1 = operandStack.Pop();
					}
					else{
						arg2 = operandStack.Pop();
						arg1 = operandStack.Pop();
					}
					operandStack.Push(_operations[System.Array.IndexOf(_operators, op)](arg1, arg2));
				}
				operatorStack.Push(token);
			} else {
				operandStack.Push(new BitwiseInt(System.Int32.Parse (token),PlayerPrefs.GetInt("max")));
			}
			tokenIndex += 1;
		}
		
		if (valid) {
			while (operatorStack.Count > 0) {
				string op = operatorStack.Pop ();
				BitwiseInt arg1, arg2;
				if (op == "~") {
					arg2 = new BitwiseInt (0);
					arg1 = operandStack.Pop ();
				} else {
					arg2 = operandStack.Pop ();
					arg1 = operandStack.Pop ();
				}
				operandStack.Push (_operations [System.Array.IndexOf (_operators, op)] (arg1, arg2));
			}
			return operandStack.Pop ();
		} else {
			return new BitwiseInt(-1);
		}
	}
	
	private string getSubExpression(List<string> tokens, ref int index)
	{
		StringBuilder subExpr = new StringBuilder();
		int parenlevels = 1;
		index += 1;
		while (index < tokens.Count && parenlevels > 0) {
			string token = tokens[index];
			if (tokens[index] == "(") {
				parenlevels += 1;
			}
			
			if (tokens[index] == ")") {
				parenlevels -= 1;
			}
			
			if (parenlevels > 0) {
				subExpr.Append(token);
			}
			
			index += 1;
		}
		
		if ((parenlevels > 0)) {
			valid = false;
		}
		return subExpr.ToString();
	}
	
	private List<string> getTokens(string expression)
	{
		string operators = "()&|^~";
		List<string> tokens = new List<string>();
		StringBuilder sb = new StringBuilder();
		
		foreach (char c in expression.Replace(" ", string.Empty)) {
			if (operators.IndexOf(c) >= 0) {
				if ((sb.Length > 0)) {
					tokens.Add(sb.ToString());
					sb.Length = 0;
				}
				tokens.Add(c.ToString ());
			} else {
				sb.Append(c);
			}
		}
		
		if ((sb.Length > 0)) {
			tokens.Add(sb.ToString());
		}
		return tokens;
	}
}
