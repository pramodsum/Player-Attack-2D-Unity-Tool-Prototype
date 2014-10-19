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
				public bool isBombable = false;
				public bool isJabbable = false;
		
				public float speed = 8f;
				public bool weaponOut = false;
				public float timeSinceBomb = 0f;
		
				public Weapon (GameObject w, string a, bool s, bool j)
				{
						weapon = w;
						attackKey = a;
						isBombable = s;
						isJabbable = j;
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
		public bool w1IsBombable = false;
		public bool w1IsJabbable = false;
	
		//Divider
		public bool ________________________;
	
		/****************************************************************************
		 * WEAPON 2
		 ****************************************************************************/
		Weapon w2;
		public GameObject weapon2;
		public string attackKey2;
		public bool w2IsBombable = false;
		public bool w2IsJabbable = false;
	
		void Start ()
		{
				checkSettings ();
				initPlayer ();

				//add count for each possible item in inventory
				for (int i = 0; i < inventory.Count; i++) {
						itemsCollected.Add (0);
				}
		
				//Initialize weapons
				w1 = initWeapon (weapon1, attackKey1.ToLower (), w1IsBombable, w1IsJabbable);
				w2 = initWeapon (weapon2, attackKey2.ToLower (), w2IsBombable, w2IsJabbable);
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
				Debug.Log ("Player collided w/ " + coll.gameObject.name);
				
				//Depletes player health on collision with enemy
				if (coll.gameObject.tag == "Enemy") {
						health -= damage;
				} 
				//Grounds player after jump in Platformer 2D games
				else if (coll.gameObject.tag == "Ground" && isPlatformer) {
						rigidbody2D.velocity = Vector2.zero;
						grounded = true;
				} 
				//Collects inventory items
				else if (coll.gameObject.tag != "Weapon") {
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
	

		//REQUIRES: Weapon
		//MODIFIES: nothing
		//EFFECTS: Either jabs weapon or throws bomb from player location
		//RETURNS: nothing
		public void attack (Weapon w)
		{
				//Only attack if that weapon isn't already being used
				if (w.weaponOut) {
						if (w.isJabbable)
								jab (w, new Vector3 (faceDirection.x, faceDirection.y, 0));
						else if (w.isBombable)
								bomb (w, new Vector3 (faceDirection.x, faceDirection.y, 0));
				}
		}
		
		//REQUIRES: Weapon, direction faced by player
		//MODIFIES: nothing
		//EFFECTS: Instantiates weapon for as long as attackKey is pressed
		//RETURNS: nothing
		public void jab (Weapon w, Vector3 dir)
		{		
				w.attack = Instantiate (w.weapon, transform.position + dir, Quaternion.identity) as GameObject;
		
				//Sets weapon parent as player
				if (!w.attack.transform.parent)
						w.attack.transform.parent = transform;

				w.weaponOut = true;
		}
	
		//REQUIRES: Weapon, direction faced by player
		//MODIFIES: nothing
		//EFFECTS: Destroys all enemies within a 5.0f range of bomb location
		//RETURNS: nothing
		public void bomb (Weapon w, Vector3 dir)
		{
				w.attack = Instantiate (w.weapon, transform.position + dir, Quaternion.identity) as GameObject;
				w.weaponOut = true;
		}
	
		/****************************************************************************
		 * 
		 * PRIVATE METHODS
		 * 
		 ****************************************************************************/

		//REQUIRES: nothing
		//MODIFIES: Player health, damage, collider + settings, rigidbody + settings
		//EFFECTS: initializes player with settings
		//RETURNS: nothing
		private void initPlayer ()
		{
				//Sets default values for player health and damage
				if (health == 0)
						health = 6f;
				if (damage == 0)
						damage = 0.5f;

				//Adds colliders and rigidbody if not already added
				if (!collider2D)
						gameObject.AddComponent<PolygonCollider2D> ();
				if (!rigidbody2D)
						gameObject.AddComponent<Rigidbody2D> ();

				//Ensures that player doesn't rotate on collisions
				rigidbody2D.fixedAngle = true;
				rigidbody2D.isKinematic = false;
				collider2D.isTrigger = false;
				gameObject.tag = "Player";

				//Platformer Player Settings
				if (isPlatformer) {
						rigidbody2D.angularDrag = 0;

						//Sets player mass to specified weight
						if (weight > 0)
								rigidbody2D.gravityScale = weight;

						//Default disables grid-like movement
						allowDiagonalMovement = true;
				} else {
						//Player shouldn't have gravity in top down games
						rigidbody2D.gravityScale = 0;
				}
		}
	
		//REQUIRES: weapon gameobject, attack key, is bombable, is jabbale
		//MODIFIES: Weapon attack key, script, collider + settings, 
		//					rigidbody + settings
		//EFFECTS: initializes weapon with settings
		//RETURNS: Weapon object
		private Weapon initWeapon (GameObject weapon, string key, bool bomb, bool jab)
		{
				//Adds colliders and rigidbody if not already added
				if (!weapon.collider2D)
						weapon.AddComponent<PolygonCollider2D> ();
				if (!weapon.rigidbody2D)
						weapon.AddComponent<Rigidbody2D> ();

				//Weapon shouldn't have gravity or mass in top down games
				if (!isPlatformer) {
						weapon.rigidbody2D.gravityScale = 0;
						weapon.rigidbody2D.mass = 0;
				} else {
						weapon.rigidbody2D.gravityScale = 1;
						weapon.rigidbody2D.mass = 1;
				}

				//Adds weapon script it not already added
				if (!weapon.GetComponent<WeaponScript> ())
						weapon.AddComponent<WeaponScript> ();

				weapon.rigidbody2D.angularDrag = 0;
				weapon.rigidbody2D.fixedAngle = true;
				weapon.gameObject.tag = "Weapon";
				weapon.rigidbody2D.isKinematic = false;
				weapon.collider2D.isTrigger = false;

				//Ensures WeaponScript knows if weapon is a bomb
				if (bomb)
						weapon.GetComponent<WeaponScript> ().isBombable = true;
		
				return new Weapon (weapon, key, bomb, jab);
		}
	
		//REQUIRES: nothing
		//MODIFIES: nothing
		//EFFECTS: Checks that all required player and weapon settings have been set
		//RETURNS: nothing
		private void checkSettings ()
		{
				//Ensure attack keys are set
				if (string.IsNullOrEmpty (attackKey1) || string.IsNullOrEmpty (attackKey2)) 
						errorMessage ("Assign Attack Keys", "Please Assign Attack Keys for Player");
		
				//Weapon 1 can't be jabbable AND bombable
				if (w1IsJabbable == w1IsBombable) 
						errorMessage ("Weapon 1 Error", "Please pick only one action type for weapon 1");
		
				//Weapon 2 can't be jabbable AND bombable
				if (w2IsJabbable == w2IsBombable) 
						errorMessage ("Weapon 2 error", "Please pick only one action type for weapon 2");

				//Warns if no items are inventory
				if (inventory.Count == 0) 
						warningMessage ("Warning", "No collectable items have been set!");
		}
		
		//REQUIRES: error title and message
		//MODIFIES: nothing
		//EFFECTS: Shows error dialog and stops game build in editor
		//RETURNS: nothing
		private void errorMessage (string title, string msg)
		{
				if (Application.isEditor) {
						EditorUtility.DisplayDialog (title, msg, "Ok");
						EditorApplication.isPlaying = false;
				}
		}
		
		//REQUIRES: error title and message
		//MODIFIES: nothing
		//EFFECTS: Shows warning dialog in editor
		//RETURNS: nothing
		private void warningMessage (string title, string msg)
		{
				if (Application.isEditor) {
						EditorUtility.DisplayDialog (title, msg, "Ok");
				}
		}

		//REQUIRES: nothing
		//MODIFIES: player transform position
		//EFFECTS: moves player according
		//RETURNS: nothing
		private void getMovement ()
		{
				//Allows player to move diagonally
				if (allowDiagonalMovement) 
						fluidMovement ();
				//Restricts player to grid movement
				else 
						gridMovement ();
		
				//Makes player jump in Platformer 2D games
				if (isPlatformer && Input.GetKeyDown (KeyCode.Space)) {
						//Allows for constant flying on holding space key 
						if (jetpackEnabled) {
								rigidbody2D.velocity = new Vector2 (0, jumpHeight);
						} 
						//Jumps normally if player can't fly and is grounded
						else if (grounded) {
								rigidbody2D.velocity = new Vector2 (0, jumpHeight);
								grounded = false;
						}
				}
		
				//Resets Platformer player velocity to 0 when already in the ground
				if (isPlatformer && Input.GetKeyUp (KeyCode.Space)) {
						if (!grounded) {
								rigidbody2D.velocity = Vector2.zero;
						}
				}
		}

		//REQUIRES: nothing
		//MODIFIES: player transform position
		//EFFECTS: moves player in grid-like manner
		//RETURNS: nothing
		private void gridMovement ()
		{
				//Up
				if (Input.GetKey (KeyCode.UpArrow)) {
						transform.position = new Vector3 (transform.position.x, transform.position.y + 0.1f, 0f);
						faceDirection = Vector2.up;
				} 
				//Down
				else if (Input.GetKey (KeyCode.DownArrow)) {
						transform.position = new Vector3 (transform.position.x, transform.position.y - 0.1f, 0f);
						faceDirection = -Vector2.up;
				} 
				//Right
				else if (Input.GetKey (KeyCode.RightArrow)) {
						transform.position = new Vector3 (transform.position.x + 0.1f, transform.position.y, 0f);
						faceDirection = Vector2.right;
				} 
				//Left
				else if (Input.GetKey (KeyCode.LeftArrow)) {
						transform.position = new Vector3 (transform.position.x - 0.1f, transform.position.y, 0f);
						faceDirection = -Vector2.right;
				}
		}

		//REQUIRES: nothing
		//MODIFIES: player transform position
		//EFFECTS: moves player allowing for diagonal movement
		//RETURNS: nothing
		private void fluidMovement ()
		{
				//Up
				if (Input.GetKey (KeyCode.UpArrow)) {
						transform.position = new Vector3 (transform.position.x, transform.position.y + 0.1f, 0f);
						faceDirection = Vector2.up;
				}
				//Down
				if (Input.GetKey (KeyCode.DownArrow)) {
						transform.position = new Vector3 (transform.position.x, transform.position.y - 0.1f, 0f);
						faceDirection = -Vector2.up;
				}
				//Right
				if (Input.GetKey (KeyCode.RightArrow)) {
						transform.position = new Vector3 (transform.position.x + 0.1f, transform.position.y, 0f);
						faceDirection = Vector2.right;
				}
				//Left
				if (Input.GetKey (KeyCode.LeftArrow)) {
						transform.position = new Vector3 (transform.position.x - 0.1f, transform.position.y, 0f);
						faceDirection = -Vector2.right;
				}
		}

		//REQUIRES: Weapon object
		//MODIFIES: anything colliding with weapon or enemy
		//EFFECTS: Gets attack and fires off respective method 
		//RETURNS: nothing
		private void getAttack (Weapon w)
		{
				//Jabs weapon
				if (Input.GetKeyDown (w.attackKey)) {
						attack (w);
				} 
				//Retracts jabbable weapon
				if (Input.GetKeyUp (w.attackKey) && w.isJabbable) {
						w.weaponOut = false;
						Destroy (w.attack);
				} 
		}

		//REQUIRES: inventory list, collided item
		//MODIFIES: nothing
		//EFFECTS: finds index of object in inventory
		//RETURNS: index of object in inventory
		private int findObjInInventory (List<GameObject> inventory, GameObject obj)
		{
				for (int i = 0; i < inventory.Count; i++) {
						//matches object to inventory objects by name
						if (obj.name == inventory [i].name)
								return i;
				}
				return -1;
		}
}