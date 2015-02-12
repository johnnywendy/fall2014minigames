using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {

	private bool shouldFadeOut = false;
	private bool shouldFadeIn = false;
	private SpriteRenderer spriteRenderer;
	private Vector3 startPos;
	private bool isEnabled = true;
	private bool canInteract = false;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.color = new Color(1,1,1,0);
	}

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		transform.position = new Vector3(transform.position.x+0.3f,transform.position.y,transform.position.z);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (shouldFadeIn) {
			if (transform.position.x < startPos.x+0.06f)
				transform.position = Vector3.Slerp(transform.position,new Vector3(transform.position.x-0.3f,transform.position.y,transform.position.z),Time.deltaTime/2);
			else
				transform.position = new Vector3(startPos.x,transform.position.y,transform.position.z);
			spriteRenderer.color = (Color.Lerp(spriteRenderer.color,new Color(1,1,1,1),Time.deltaTime*3));
			if (spriteRenderer.color.a > 0.9f) {
				spriteRenderer.color = new Color(1,1,1,1);
				shouldFadeIn = false;
			}
		}
		else if (shouldFadeOut) {
			if (transform.position.x > startPos.x-0.28f)
				transform.position = Vector3.Slerp(transform.position,new Vector3(transform.position.x-0.3f,transform.position.y,transform.position.z),Time.deltaTime/3);
			else
				transform.position = new Vector3(startPos.x-0.3f,transform.position.y,transform.position.z);
			spriteRenderer.color = (Color.Lerp(spriteRenderer.color,new Color(1,1,1,0),Time.deltaTime*5));
			if (spriteRenderer.color.a < 0.1f) {
				spriteRenderer.color = new Color(1,1,1,0);
				transform.position = new Vector3(startPos.x+0.6f,startPos.y,startPos.z);
				shouldFadeOut = false;
			}
		}
	}

	public void FadeIn() {
		isEnabled = true;
		shouldFadeIn = true;
		canInteract = false;
		StartCoroutine(InteractOn());
	}

	public void FadeOut() {
		isEnabled = false;
		shouldFadeOut = true;
	}

	void OnMouseUp() {
		if (isEnabled && canInteract) {
			Camera.main.GetComponent<MenuManager>().SlideUpFromBottom();
			FadeOut();
		}
	}

	IEnumerator InteractOn() {
		yield return new WaitForSeconds(1f);
		canInteract = true;
	}
}
