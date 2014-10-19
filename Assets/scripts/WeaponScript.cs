/****************************************************************************
 * 
 * WEAPON SCRIPT
 * -------------
 * Performs collision actions for weapons
 * -------------
 * AUTHOR: SUMEDHA PRAMOD
 * EMAIL: pramods@umich.edu
 * 
 ****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponScript : MonoBehaviour
{	
		public bool isBombable = false;
		public float timeLeft;
		public float range = 5.0f;

		void OnCollisionEnter2D (Collision2D coll)
		{
				if (coll.gameObject.tag == "Enemy" && !isBombable)
						Destroy (coll.gameObject);

				if (isBombable)
						StartCoroutine (wait (2f));
		}
	
		//REQUIRES: time to wait
		//MODIFIES: enemies within the radius of bomb
		//EFFECTS: waits t seconds then destroys all objects w/in range of bomb, then
		//				 the bomb self destructs
		//RETURNS: nothing
		public IEnumerator wait (float t)
		{
				yield return new WaitForSeconds (t);

				//Gets all objects within range of bomb
				Collider2D[] colliders = Physics2D.OverlapCircleAll (transform.position, range);
		
				//Destroys enemy objects in range of bomb
				foreach (Collider2D col in colliders) {
						Debug.Log (col.name + " in range of bomb");
						if (col.tag == "Enemy") {
								Destroy (col.collider2D.gameObject);
				
						}
				}

				//Self destructs bomb
				Destroy (this.gameObject);
		}
}
