using UnityEngine;
using System.Collections;

public static class HexColor {

	private static Shader shaderGUItext = Shader.Find("GUI/Text Shader");

	public static Color AdjustHexToColor(string hex,int amount) {
		string rs = hex[0].ToString() + hex[1].ToString();
		string gs = hex[2].ToString() + hex[3].ToString();
		string bs = hex[4].ToString() + hex[5].ToString();
		int r = System.Convert.ToInt32(rs,16)-amount;
		int g = System.Convert.ToInt32(gs,16)-amount;
		int b = System.Convert.ToInt32(bs,16)-amount;
		return new Color(r/255.0f, g/255.0f, b/255.0f, 1.0f);
	}
	
	public static Color HexToColor(string hex) {
		string rs = hex[0].ToString() + hex[1].ToString();
		string gs = hex[2].ToString() + hex[3].ToString();
		string bs = hex[4].ToString() + hex[5].ToString();
		int r = System.Convert.ToInt32(rs,16);
		int g = System.Convert.ToInt32(gs,16);
		int b = System.Convert.ToInt32(bs,16);
		return new Color(r/255.0f, g/255.0f, b/255.0f, 1.0f);
	}
	
	public static Color HexToColorWithAlpha(string hex,float alpha) {
		string rs = hex[0].ToString() + hex[1].ToString();
		string gs = hex[2].ToString() + hex[3].ToString();
		string bs = hex[4].ToString() + hex[5].ToString();
		int r = System.Convert.ToInt32(rs,16);
		int g = System.Convert.ToInt32(gs,16);
		int b = System.Convert.ToInt32(bs,16);
		return new Color(r/255.0f, g/255.0f, b/255.0f, alpha);
	}

	public static void SetColor(GameObject obj, string hexCode) {
		if (obj != null) {
			obj.GetComponent<SpriteRenderer>().material.shader = shaderGUItext;
			obj.GetComponent<SpriteRenderer>().color = HexColor.HexToColor(hexCode);
		}
	}
}
