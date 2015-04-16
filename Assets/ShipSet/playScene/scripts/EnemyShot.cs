using UnityEngine;
using System.Collections;

public class EnemyShot : MonoBehaviour
{
	float animRate = 0.2f;
	float animOn = 0f;
	const float frameHeight = 0.25f;
	public int speed = 8;
	

	
	// Use this for initialization
	void Start ()
	{
		Vector3 tempAngles = transform.eulerAngles;
		tempAngles.z = Random.Range (145, 205);
		transform.eulerAngles = tempAngles;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time > (animOn + animRate)) {
			
			animOn = Time.time;
			float frameOffset = GetComponent<Renderer>().material.mainTextureOffset.y;
			frameOffset -= frameHeight;
			if (frameOffset < 0)
				frameOffset = .75f;
		
			GetComponent<Renderer>().material.mainTextureOffset = new Vector2 (GetComponent<Renderer>().material.mainTextureOffset.x, frameOffset);
		}
		
		transform.Translate (new Vector3 (0, speed * Time.deltaTime, 0));
	}

	void OnCollisionEnter (Collision col)
	{
		if ("Bottom" == col.gameObject.tag || "Right" == col.gameObject.tag || "Left" == col.gameObject.tag) {
			Destroy (gameObject);
		} 
	}
}

