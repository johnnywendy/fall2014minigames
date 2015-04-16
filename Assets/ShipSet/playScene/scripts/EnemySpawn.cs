using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
	
	int direction = 1;
	int speed = 3;
	float lastSpawn;
	float nextSpawn = .6f;
	public bool isOn = true;
	public Transform enemyShip;
	public Transform friendlyShip;
	bool flag = true;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isOn) {
			float d = direction * speed * Time.deltaTime;
			transform.Translate (new Vector3 (d, 0, 0));
		
			if (Time.time > (lastSpawn + (nextSpawn / GameManager.instance.level))) {
				lastSpawn = Time.time + nextSpawn;
				Transform es;
				float limitX = 8;
				float limitY = 8;
				if(flag) {
					es = (Transform)Instantiate (enemyShip, transform.position,
					                             Quaternion.Euler (0,0, 0));
					flag = false;
				} else {
					es = (Transform)Instantiate (friendlyShip, transform.position,
				    Quaternion.Euler (0, 0, 0));
					flag = true;
				}
				GameManager.instance.totalShips++;
				//Debug.Log ("instantiating at" + transform.position);
				es.Translate (0, -1, 0);
			}
		}

		/*
		if (!bossLaunched && GameManager.instance!=null && GameManager.instance.level == 3 && isOn) {
			isOn = false;
			Instantiate (boss, new Vector3 (0, 8, 0), Quaternion.identity);
			bossLaunched = true;
		}
		
		if (bossLaunched && bossKilled) {
			Application.LoadLevel(1);
		}
		*/
	}
	
	//these are inverted because of the bizarre 180° rotation 
	void OnCollisionEnter (Collision col)
	{
	
		if ("Left" == col.gameObject.tag) {
			
			direction = 1;
			//nextSpawn = Random.Range(1.0,3.0);
			
		} else if ("Right" == col.gameObject.tag) {
			direction = -1;
			//nextSpawn = Random.Range(1.0,3.0);
		}
	}
}
