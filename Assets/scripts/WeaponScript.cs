using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponScript : MonoBehaviour
{	
		public bool isBombable = false;
		public float timeLeft;

		void OnCollisionEnter2D (Collision2D coll)
		{
				if (coll.gameObject.tag == "Enemy" && !isBombable)
						Destroy (coll.gameObject);

				if (isBombable)
						StartCoroutine (wait (2f));
		}
	
		public IEnumerator wait (float t)
		{
				yield return new WaitForSeconds (t);

				Collider2D[] colliders = Physics2D.OverlapCircleAll (transform.position, 5f);
		
				foreach (Collider2D col in colliders) {
						Debug.Log (col.name + " in range of bomb");
						if (col.tag == "Enemy") {
								Destroy (col.collider2D.gameObject);
				
						}
				}
				Destroy (this.gameObject);
		}
}
