/****************************************************************************
 * 
 * PLAYER SCRIPT
 * -------------
 * Provides all the basic operations needed for a player in a 2D game.
 * -------------
 * AUTHOR: SUMEDHA PRAMOD
 * EMAIL: pramods@umich.edu
 * 
 ****************************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
		/****************************************************************************
		 * 
		 * WEAPON CLASS
		 * 
		 ****************************************************************************/
		protected class Weapon
		{
				public GameObject weapon;
				public GameObject attack;
				public string attackKey;
				public bool isShootable = false;
				public bool isJabbable = false;
		
				public float speed = 8f;
				public bool weaponOut = false;
				public float timeSinceShoot = 0f;
		
				public Weapon (GameObject w, string a, bool s, bool j)
				{
						weapon = w;
						attackKey = a;
						isShootable = s;
						isJabbable = j;
				}
		}
	
		/****************************************************************************
		 * PLAYER
		 ****************************************************************************/
		public bool isPlatformer = false;
		public float jumpHeight = 5f;
		public float weight = 0f;
	
		//Divider
		public bool _______________________;
	
		/****************************************************************************
		 * WEAPON 1
		 ****************************************************************************/
		Weapon w1;
		public GameObject weapon1;
		public string attackKey1;
		public bool w1IsShootable = false;
		public bool w1IsJabbable = false;
	
		//Divider
		public bool ________________________;
	
		/****************************************************************************
		 * WEAPON 2
		 ****************************************************************************/
		Weapon w2;
		public GameObject weapon2;
		public string attackKey2;
		public bool w2IsShootable = false;
		public bool w2IsJabbable = false;
	
		//Weapons Out
		Vector2 faceDirection;
	
		void Start ()
		{
				checkSettings ();
				initPlayer ();
		
				w1 = initWeapon (weapon1, attackKey1.ToLower (), w1IsShootable, w1IsJabbable);
				w2 = initWeapon (weapon2, attackKey2.ToLower (), w2IsShootable, w2IsJabbable);
		}
	
		void FixedUpdate ()
		{
				//Get Player Movement
				getMovement ();
		
				//Get Player Attacks
				getAttack (w1);
				getAttack (w2);
		}	
	
		/****************************************************************************
		 * 
		 * PROTECTED METHODS
		 * 
		 ****************************************************************************/
	
		protected void attack (Weapon w)
		{
				Vector3 dir = new Vector3 (faceDirection.x, faceDirection.y, 0);
				if (w.isJabbable)
						jab (w, dir);
				else if (w.isShootable)
						shoot (w, dir);
		}
	
		protected void jab (Weapon w, Vector3 dir)
		{		
				if (!w.weaponOut) {
						rotateWeapon (w);
						w.attack = Instantiate (w.weapon, transform.position + dir, Quaternion.identity) as GameObject;
			
						if (!w.attack.transform.parent)
								w.attack.transform.parent = transform;
						w.weaponOut = true;
				}
		}
	
		protected void shoot (Weapon w, Vector3 dir)
		{
				if (!w.weaponOut) {
						w.attack = Instantiate (w.weapon, transform.position + dir, Quaternion.identity) as GameObject;
						w.weapon.rigidbody2D.velocity += faceDirection * 7.5f;
						w.weaponOut = true;
				}
		}
	
		/****************************************************************************
		 * 
		 * PRIVATE METHODS
		 * 
		 ****************************************************************************/
	
		private void initPlayer ()
		{
				if (isPlatformer) {
						if (!rigidbody2D)
								gameObject.AddComponent<Rigidbody2D> ();
			
						rigidbody2D.angularDrag = 0;
						rigidbody2D.isKinematic = false;
						rigidbody2D.fixedAngle = true;

						if (weight > 0)
								rigidbody2D.gravityScale = weight;
				}
		}
	
		private Weapon initWeapon (GameObject weapon, string key, bool shoot, bool jab)
		{
				if (!weapon.collider2D)
						weapon.AddComponent<PolygonCollider2D> ();
		
				weapon.collider2D.isTrigger = false;
		
				if (!weapon.rigidbody2D)
						weapon.AddComponent<Rigidbody2D> ();
		
				if (!isPlatformer)
						weapon.rigidbody2D.gravityScale = 0;
		
				weapon.rigidbody2D.angularDrag = 0;
				weapon.rigidbody2D.isKinematic = false;
		
				return new Weapon (weapon, key, shoot, jab);
		}
	
		private void checkSettings ()
		{
				//Ensure attack keys are set
				if (string.IsNullOrEmpty (attackKey1) || string.IsNullOrEmpty (attackKey2)) 
						errorMessage ("Assign Attack Keys", "Please Assign Attack Keys for Player");
		
				if (w1IsJabbable == w1IsShootable) 
						errorMessage ("Weapon 1 Error", "Please pick only one action type for weapon 1");
		
				if (w2IsJabbable == w2IsShootable) 
						errorMessage ("Weapon 2 error", "Please pick only one action type for weapon 2");
		}
	
		private void errorMessage (string title, string msg)
		{
				if (Application.isEditor) {
						EditorUtility.DisplayDialog (title, msg, "Ok");
						EditorApplication.isPlaying = false;
				}
		}
	
		private void checkShotWeapon (Weapon w)
		{
				if (w.timeSinceShoot < 1) {
						w.timeSinceShoot += Time.fixedDeltaTime;
						return;
				}
		
				w.timeSinceShoot = 0;
				w.weaponOut = false;
				Destroy (w.attack);
		}

		//TODO: flip player image based on left/right movement
		private void getMovement ()
		{
				if (Input.GetKey (KeyCode.UpArrow)) {
						transform.position = new Vector3 (transform.position.x, transform.position.y + 0.1f, 0f);
						faceDirection = Vector2.up;
				} else if (Input.GetKey (KeyCode.DownArrow)) {
						transform.position = new Vector3 (transform.position.x, transform.position.y - 0.1f, 0f);
						faceDirection = -Vector2.up;
				} else if (Input.GetKey (KeyCode.RightArrow)) {
						transform.position = new Vector3 (transform.position.x + 0.1f, transform.position.y, 0f);
						faceDirection = Vector2.right;
				} else if (Input.GetKey (KeyCode.LeftArrow)) {
						transform.position = new Vector3 (transform.position.x - 0.1f, transform.position.y, 0f);
						faceDirection = -Vector2.right;
				}
		
				if (isPlatformer && Input.GetKey (KeyCode.Space)) {
						transform.position = new Vector3 (transform.position.x, transform.position.y + jumpHeight / 20, 0f);
				}
		}
	
		private void getAttack (Weapon w)
		{
				if (!string.IsNullOrEmpty (w.attackKey)) {
						if (Input.GetKeyDown (w.attackKey)) {
								attack (w);
						} else if (Input.GetKeyUp (w.attackKey) && w.isJabbable) {
								w.weaponOut = false;
								Destroy (w.attack);
						} 
			
						if (w.isShootable)
								checkShotWeapon (w);
				}
		}

		//TODO: rotate jab image based on direction 
		private void rotateWeapon (Weapon w)
		{
				if (faceDirection == Vector2.up) {
						w.weapon.transform.rotation.Set (0, 0, 0, 0);
				} else if (faceDirection == -Vector2.up) {
						w.weapon.transform.rotation.Set (0, 0, -180, 0);
				} else if (faceDirection == Vector2.right) {
						w.weapon.transform.rotation.Set (0, 0, -90, 0);
				} else if (faceDirection == -Vector2.right) {
						w.weapon.transform.rotation.Set (0, 0, 90, 0);
				}
				Debug.Log ("FACE DIRECTION: " + faceDirection);
				Debug.Log (w.weapon.transform.rotation);
		}
}