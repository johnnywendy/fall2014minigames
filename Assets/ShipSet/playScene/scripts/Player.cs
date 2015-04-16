using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	
	float speed = 4;
	GameObject collided_with;
	public Transform laser_pref;
	float fireRate = 0.1f;
	float shotOn = 0f;
	public bool shield = false;
	Vector3 point;
	bool dragFlag = false;
	float dragSpeed;
	
	// Use this for initialization
	void Start ()
	{

	}

	void FixedUpdate ()
	{
		//transform.Translate(new Vector3(Input.GetAxis("Horizontal")*speed*Time.deltaTime,0,0));
		
		if (Input.GetAxis ("Vertical") > 0 && transform.position.y < 4.2) {
		
			if (collided_with != null && "Bottom" == collided_with.tag)
				return;
			transform.Translate (new Vector3 (0, 1 * speed * Time.deltaTime, 0));
		}

		if (Input.GetAxis ("Vertical") < 0 && transform.position.y > -4.2) {
		
			if (collided_with != null && "Top" == collided_with.tag)
				return;
			transform.Translate (new Vector3 (0, -1 * speed * Time.deltaTime, 0));
		}
 
		if (Input.GetAxis ("Horizontal") > 0) {
		
			if (collided_with != null && "Left" == collided_with.tag)
				return;
			transform.Translate (new Vector3 (1 * speed * Time.deltaTime, 0, 0));
		}

		if (Input.GetAxis ("Horizontal") < 0) {
		
			if (collided_with != null && "Right" == collided_with.tag)
				return;
			transform.Translate (new Vector3 (-1 * speed * Time.deltaTime, 0, 0));
			
		}
		
		if (Input.GetMouseButtonDown(0) && Time.time > (shotOn + fireRate)) {
			shotOn = Time.time;
			Instantiate (laser_pref, new Vector3 (transform.up.x, transform.up.y, 0), transform.rotation);
		}

		if (Input.GetMouseButtonUp (0)) {
			dragFlag = false;
		}
		
	}
	
	void OnCollisionEnter (Collision col)
	{
		collided_with = col.gameObject;
		
		if ("Enemy" == col.gameObject.tag) {
			Destroy (col.gameObject);
			if (!shield) {
				GameManager.instance.playerLifes--;
			} else {
				shield = false;
				GetComponent<Renderer>().material.mainTextureOffset = new Vector2 (0.0f, 0.5f);
			}
		}
		
		if ("Shield" == col.gameObject.tag) {
			Destroy (col.gameObject);
			shield = true;
			GetComponent<Renderer>().material.mainTextureOffset = new Vector2 (0.0f, 0.0f);
		}
		
	}
	
	void OnCollisionExit (Collision col)
	{
		collided_with = null;
	}
}
