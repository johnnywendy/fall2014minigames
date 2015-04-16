using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour
{
	
	int speed = 5;
	public Transform shieldPowerup;
	
	
	// Use this for initialization
	void Start ()
	{
		GetComponent<AudioSource>().Play ();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		transform.Translate (Vector3.up * speed * Time.deltaTime);
	}
	
	void OnCollisionEnter (Collision col)
	{
		if ("Top" == col.gameObject.tag) {
			Destroy (gameObject);
		}
		
		if ("Enemy" == col.gameObject.tag) {
			GameManager.instance.shipsKilled++;
			Destroy (col.gameObject);
			Destroy (gameObject);
			
			
			if (GameManager.instance.player != null && !GameManager.instance.player.GetComponent<Player> ().shield && Random.Range (1, 5) == 1) {
				Instantiate (shieldPowerup, transform.position, Quaternion.identity);
			}
		}
		
		if ("Shield" == col.gameObject.tag) {
			Destroy (gameObject);
		}
	
	}
}
