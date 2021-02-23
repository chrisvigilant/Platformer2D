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
		RaycastHit2D hit;
		Ray ray = new Ray(transform.position, Vector3.forward);
		Vector3 fwd = this.transform.TransformDirection(0, 0, 1f);
		Debug.DrawRay(this.transform.position, fwd, Color.green);
		hit = Physics2D.GetRayIntersection(ray, 1f, bg);

		if (hit){
			back = hit.transform.gameObject;
			if (back.CompareTag("Blue"))
            {
                rend.color = Color.blue;
			}

			else if (back.CompareTag("Yellow"))
			{
				rend.color = Color.yellow;
			}

			else if (back.CompareTag("Magenta"))
			{
				rend.color = Color.magenta;
			}
			//Debug.Log("Dis: Background!");
		}

    }
}
