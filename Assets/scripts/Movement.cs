using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
		void FixedUpdate ()
		{
				if (Input.GetKey (KeyCode.UpArrow)) {
						transform.position = new Vector3 (transform.position.x, transform.position.y + 0.1f, 0f);
				} else if (Input.GetKey (KeyCode.DownArrow)) {
						transform.position = new Vector3 (transform.position.x, transform.position.y - 0.1f, 0f);
				} else if (Input.GetKey (KeyCode.RightArrow)) {
						transform.position = new Vector3 (transform.position.x + 0.1f, transform.position.y, 0f);
				} else if (Input.GetKey (KeyCode.LeftArrow)) {
						transform.position = new Vector3 (transform.position.x - 0.1f, transform.position.y, 0f);
				}
		}
}
