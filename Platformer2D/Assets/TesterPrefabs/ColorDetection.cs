using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDetection : MonoBehaviour
{
	Rigidbody2D rb;
	public GameObject back;
	public LayerMask bg;
	private SpriteRenderer rend;
	public static RaycastHit2D hit;

	void Start(){
		rend = gameObject.GetComponent<SpriteRenderer>();
		//rend.color = new Color (1, 0, 0, 1);
	}

    void FixedUpdate()
    {
		//RaycastHit hit;
		Ray ray = new Ray(transform.position, Vector3.forward);
		Vector3 fwd = this.transform.TransformDirection(0, 0, 1f);
		Debug.DrawRay(this.transform.position, fwd, Color.green);

		if (Physics2D.GetRayIntersection(ray, 1f, bg)){
			Debug.Log("Dis: Background!");
		}

    }
}
