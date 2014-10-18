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
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
		/****************************************************************************
		 * 
		 * WEAPON CLASS
		 * 
		 ****************************************************************************/
		public class Weapon
		{
				public GameObject weapon;
				public GameObject attack;
				public string attackKey;
				public bool isShootable = false;
				public bool isJabbable = false;
		
				public float speed = 8f;
				public bool weaponOut = false;
				public float timeSinceShoot = 0f;

				public float strength;
		
				public Weapon (GameObject w, string a, bool s, bool j, float p)
				{
						weapon = w;
						attackKey = a;
						isShootable = s;
						isJabbable = j;
						strength = p;
				}
		}
	
		/****************************************************************************
		 * PLAYER
	   ****************************************************************************/
		public float health = 6f;
		public float damage = 0.5f;  
		public List<GameObject> inventory;
		public List<int> itemsCollected;
	
		//Top Down Settings
		public bool allowDiagonalMovement = false;
	
		//Platformer Settings
		public bool isPlatformer = false;
		public float jumpHeight = 50f;
		public float weight = 2f;
		public bool jetpackEnabled = false;

		//Player private variables
		bool grounded;
		Vector2 faceDirection;
		SpriteRenderer spriteRenderer;
	
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
		public float w1Strength;
	
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
		public float w2Strength;
	
		void Start ()
		{
				checkSettings ();
				initPlayer ();

				for (int i = 0; i < inventory.Count; i++) {
						itemsCollected.Add (0);
				}
		
				w1 = initWeapon (weapon1, attackKey1.ToLower (), w1IsShootable, w1IsJabbable, w1Strength);
				w2 = initWeapon (weapon2, attackKey2.ToLower (), w2IsShootable, w2IsJabbable, w2Strength);
		}
	
		void FixedUpdate ()
		{
				//Get Player Movement
				getMovement ();
		
				//Get Player Attacks
				getAttack (w1);
				getAttack (w2);
		}	

		void OnCollisionEnter2D (Collision2D coll)
		{
				if (coll.gameObject.tag == "Enemy") {
						if (w1.weaponOut || w2.weaponOut)
								Destroy (coll.gameObject);
						else
								health -= damage;
				} else if (coll.gameObject.tag == "Ground" && isPlatformer) {
						rigidbody2D.velocity = Vector2.zero;
						grounded = true;
				} else if (coll.gameObject.tag != "Weapon") {
						int i = findObjInInventory (inventory, coll.gameObject);

						if (i < 0) {
								Debug.Log (coll.gameObject.name + " not collectable");
								return;
						}

						itemsCollected [i]++;
						Destroy (coll.gameObject);
				}
		}
	
		/****************************************************************************
		 * 
		 * PUBLIC METHODS
		 * 
		 ****************************************************************************/
	
		public void attack (Weapon w)
		{
				if (w.isJabbable)
						jab (w, new Vector3 (faceDirection.x, faceDirection.y, 0));
				else if (w.isShootable)
						shoot (w, new Vector3 (faceDirection.x, faceDirection.y, 0));
		}
	
		public void jab (Weapon w, Vector3 dir)
		{		
				if (!w.weaponOut) {
						rotateWeapon (w);
						w.attack = Instantiate (w.weapon, transform.position + dir, Quaternion.identity) as GameObject;
			
						if (!w.attack.transform.parent)
								w.attack.transform.parent = transform;

						w.weaponOut = true;
				}
		}
	
		public void shoot (Weapon w, Vector3 dir)
		{
				if (!w.weaponOut) {
						w.attack = Instantiate (w.weapon, transform.position + dir, Quaternion.identity) as GameObject;
						w.weapon.rigidbody2D.velocity += faceDirection * w.speed;
						w.weaponOut = true;
				}
		}

		public void wait (float t)
		{
				float time = 0;
				while (time < t) {
						time += Time.fixedDeltaTime;
				}
		}
	
		/****************************************************************************
		 * 
		 * PRIVATE METHODS
		 * 
		 ****************************************************************************/
	
		private void initPlayer ()
		{
				if (health == 0)
						health = 6f;
				if (damage == 0)
						damage = 0.5f;

				if (!collider2D)
						gameObject.AddComponent<PolygonCollider2D> ();
		
				if (!rigidbody2D)
						gameObject.AddComponent<Rigidbody2D> ();

				rigidbody2D.fixedAngle = true;

				if (isPlatformer) {
						if (!rigidbody2D)
								gameObject.AddComponent<Rigidbody2D> ();
			
						rigidbody2D.angularDrag = 0;
						rigidbody2D.isKinematic = false;
						rigidbody2D.fixedAngle = true;

						if (weight > 0)
								rigidbody2D.gravityScale = weight;

						allowDiagonalMovement = true;
				} else {
						rigidbody2D.gravityScale = 0;
				}
		}
	
		private Weapon initWeapon (GameObject weapon, string key, bool shoot, bool jab, float strength)
		{
				if (!weapon.collider2D)
						weapon.AddComponent<PolygonCollider2D> ();
		
				if (!weapon.rigidbody2D)
						weapon.AddComponent<Rigidbody2D> ();

				weapon.rigidbody2D.gravityScale = 0;
				weapon.rigidbody2D.mass = 0;
				weapon.rigidbody2D.angularDrag = 0;
				weapon.rigidbody2D.fixedAngle = true;
		
				if (shoot) {
						weapon.rigidbody2D.isKinematic = true;
						weapon.collider2D.isTrigger = true;
						weapon.rigidbody2D.gravityScale = 1;
				}

				weapon.gameObject.tag = "Weapon";
		
				return new Weapon (weapon, key, shoot, jab, strength);
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

				if (inventory.Count == 0) 
						warningMessage ("Warning", "No collectable items have been set!");
		}
	
		private void errorMessage (string title, string msg)
		{
				if (Application.isEditor) {
						EditorUtility.DisplayDialog (title, msg, "Ok");
						EditorApplication.isPlaying = false;
				}
		}
	
		private void warningMessage (string title, string msg)
		{
				if (Application.isEditor) {
						EditorUtility.DisplayDialog (title, msg, "Ok");
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
				if (allowDiagonalMovement) 
						fluidMovement ();
				else
						gridMovement ();
		
				if (isPlatformer && Input.GetKeyDown (KeyCode.Space)) {
						if (jetpackEnabled) {
								rigidbody2D.velocity = new Vector2 (0, jumpHeight);
						} else if (grounded) {
								rigidbody2D.velocity = new Vector2 (0, jumpHeight);
								grounded = false;
						}
				}
		
				if (isPlatformer && Input.GetKeyUp (KeyCode.Space)) {
						if (!grounded) {
								rigidbody2D.velocity = Vector2.zero;
						}
				}
		}

		private void gridMovement ()
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
		}

		private void fluidMovement ()
		{
		
				if (Input.GetKey (KeyCode.UpArrow)) {
						transform.position = new Vector3 (transform.position.x, transform.position.y + 0.1f, 0f);
						faceDirection = Vector2.up;
				}
				if (Input.GetKey (KeyCode.DownArrow)) {
						transform.position = new Vector3 (transform.position.x, transform.position.y - 0.1f, 0f);
						faceDirection = -Vector2.up;
				}
				if (Input.GetKey (KeyCode.RightArrow)) {
						transform.position = new Vector3 (transform.position.x + 0.1f, transform.position.y, 0f);
						faceDirection = Vector2.right;
				}
				if (Input.GetKey (KeyCode.LeftArrow)) {
						transform.position = new Vector3 (transform.position.x - 0.1f, transform.position.y, 0f);
						faceDirection = -Vector2.right;
				}
		}
	
		private void getAttack (Weapon w)
		{
				if (Input.GetKeyDown (w.attackKey)) {
						attack (w);
				} else if (Input.GetKeyUp (w.attackKey) && w.isJabbable) {
						w.weaponOut = false;
						Destroy (w.attack);
				} 
			
				if (w.isShootable)
						checkShotWeapon (w);
		}

		//TODO: rotate jab image based on direction 
		private void rotateWeapon (Weapon w)
		{
//				if (faceDirection.y > 0) {
//						w.weapon.transform.RotateAround (Vector3.zero, Vector3.up, 0);
//				} else if (faceDirection.y < 0) {
//						w.weapon.transform.RotateAround (Vector3.zero, Vector3.up, 180);
//				} else if (faceDirection.x > 0) {
//						w.weapon.transform.RotateAround (Vector3.zero, Vector3.up, -90);
//				} else if (faceDirection.x < 0) {
//						w.weapon.transform.RotateAround (Vector3.zero, Vector3.up, 90);
//				}
////				Debug.Log ("FACE DIRECTION: " + faceDirection);
//				Debug.Log (w.weapon.transform.rotation);
		}

		private int findObjInInventory (List<GameObject> inventory, GameObject obj)
		{
				for (int i = 0; i < inventory.Count; i++) {
						if (obj.name == inventory [i].name)
								return i;
				}
				return -1;
		}
}