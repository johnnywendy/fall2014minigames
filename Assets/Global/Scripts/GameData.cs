using UnityEngine;
using System.Collections;

public static class GameData {

	// Makes sure the level data for a game has been initilized
	public static void RegisterGame(int gameNumber, int maxLevel) {
		maxLevel -= 1;
		if (!PlayerPrefs.HasKey("game"+gameNumber.ToString())) {
			PlayerPrefs.SetInt("game"+gameNumber.ToString(),maxLevel);
			for (int i = 0; i < maxLevel; i++)
				PlayerPrefs.SetInt("game"+gameNumber.ToString()+"level"+i.ToString(),0);
		}
	}

	// --- USE IN TESTING ONLY ---
	// Deletes all previous level data for a game and re-initlizes it with new params
	public static void ForceReRegisterGame(int gameNumber, int maxLevel) {
		maxLevel -= 1;
		int oldLevels = PlayerPrefs.GetInt("game"+gameNumber.ToString());
		for (int i = 0; i < oldLevels; i++)
			PlayerPrefs.DeleteKey("game"+gameNumber.ToString()+"level"+i.ToString());
		PlayerPrefs.SetInt("game"+gameNumber.ToString(),maxLevel);
		for (int i = 0; i < maxLevel; i++)
			PlayerPrefs.SetInt("game"+gameNumber.ToString()+"level"+i.ToString(),0);
	}

	// Sets all levels for a game to uncomplete
	public static void ResetGame(int gameNumber) {
		int maxLevel = PlayerPrefs.GetInt("game"+gameNumber.ToString());
		PlayerPrefs.SetInt("game"+gameNumber.ToString(),maxLevel);
		for (int i = 0; i < maxLevel; i++)
			PlayerPrefs.SetInt("game"+gameNumber.ToString()+"level"+i.ToString(),0);
	}

	// Returns the number of levels in a game (count starts at 1)
	public static int GetLevelCount(int gameNumber) {
		return PlayerPrefs.GetInt("game"+gameNumber.ToString())+1;
	}

	// Sets a specific level for a game to uncomplete
	public static void ResetLevel(int gameNumber, int level) {
		level -= 1;
		PlayerPrefs.SetInt("game"+gameNumber.ToString()+"level"+level.ToString(),0);
	}

	// Sets a specific level for a game to complete
	public static void CompletedLevel(int gameNumber, int level) {
		level -= 1;
		PlayerPrefs.SetInt("game"+gameNumber.ToString()+"level"+level.ToString(),1);
	}

	// Returns the number of levels completed
	public static int GetCompletedCount(int gameNumber) {
		int maxLevel = PlayerPrefs.GetInt("game"+gameNumber.ToString());
		int count = 0;
		for (int i = 0; i < maxLevel; i++)
			count += PlayerPrefs.GetInt("game"+gameNumber.ToString()+"level"+i.ToString());
		return count;
	}

	// Returns an array of bools with a true for each complete level in a game (false for uncomplete)
	public static bool[] GetLevelStatuses(int gameNumber) {
		int maxLevel = PlayerPrefs.GetInt("game"+gameNumber.ToString());
		bool[] levelStatuses = new bool[maxLevel+1];
		for (int i = 0; i < maxLevel; i++)
			levelStatuses[i] = PlayerPrefs.GetInt("game"+gameNumber.ToString()+"level"+i.ToString())==1?true:false;
		return levelStatuses;
	}

	// Sets the current game selected by the user
	public static void SetCurrentGame(int gameNumber) {
		PlayerPrefs.SetInt("currentGame",gameNumber);
	}

	// Sets the current level selected by the user
	public static void SetCurrentLevel(int levelNumber) {
		PlayerPrefs.SetInt("currentLevel",levelNumber);
	}

	// Returns the value of the last level selected by the user
	public static int GetCurrentLevel() {
		return PlayerPrefs.GetInt("currentLevel");
	}

	// Returns the value of the last game selected by the user
	public static int GetCurrentGame() {
		return PlayerPrefs.GetInt("currentGame");
	}

}
