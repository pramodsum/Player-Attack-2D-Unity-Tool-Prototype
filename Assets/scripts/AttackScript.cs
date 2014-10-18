using UnityEngine;
using UnityEditor;
using System.Collections;

public class AttackScript : MonoBehaviour
{
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

		enum Direction
		{
				North,
				East,
				South,
				West}
		;

		//Weapon 1
		Weapon w1;
		public GameObject weapon1;
		public string attackKey1;
		public bool w1IsShootable = false;
		public bool w1IsJabbable = false;
	
		//Divider
		public bool ____________________;
	
		//Weapon 2
		Weapon w2;
		public GameObject weapon2;
		public string attackKey2;
		public bool w2IsShootable = false;
		public bool w2IsJabbable = false;

		//Weapons Out
		ArrayList weaponsOut;	
		Vector3 prevPos;
		Direction faceDirection;
		Vector3 dir;
	
		void Start ()
		{
				checkSettings ();
				prevPos = transform.position;
				faceDirection = getFaceDirection ();

				w1 = initWeapon (weapon1, attackKey1.ToLower (), w1IsShootable, w1IsJabbable);
				w2 = initWeapon (weapon2, attackKey2.ToLower (), w2IsShootable, w2IsJabbable);
		}
	
		void FixedUpdate ()
		{
				if (!string.IsNullOrEmpty (w1.attackKey) && !string.IsNullOrEmpty (w2.attackKey)) {
						if (Input.GetKeyDown (w1.attackKey)) {
								attack (w1);
						} else if (Input.GetKeyUp (w1.attackKey) && w1.isJabbable) {
								w1.weaponOut = false;
								Destroy (w1.attack);
						}

						if (Input.GetKeyDown (w2.attackKey)) {
								attack (w2);
						} else if (Input.GetKeyUp (w2.attackKey) && w2.isJabbable) {
								w2.weaponOut = false;
								Destroy (w2.attack);
						} 
			
						if (w1.isShootable)
								checkShotWeapon (w1);			
						if (w2.isShootable)
								checkShotWeapon (w2);
				}
		}
	
		private Weapon initWeapon (GameObject weapon, string key, bool shoot, bool jab)
		{
				if (!weapon.collider2D)
						weapon.AddComponent<PolygonCollider2D> ();

				if (!weapon.rigidbody2D)
						weapon.AddComponent<Rigidbody2D> ();

				weapon.rigidbody2D.gravityScale = 0;
				weapon.rigidbody2D.angularDrag = 0;

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

		protected void attack (Weapon w)
		{
				if (w.isJabbable)
						jab (w);
				else if (w.isShootable)
						shoot (w);
		}

		protected void jab (Weapon w)
		{		
				if (!w.weaponOut) {
						w.attack = Instantiate (w.weapon, transform.position + prevPos, Quaternion.identity) as GameObject;
			
						if (!w.attack.transform.parent)
								w.attack.transform.parent = transform;
						w.weaponOut = true;
				}
		}

		protected void shoot (Weapon w)
		{
				if (!w.weaponOut) {
						w.attack = Instantiate (w.weapon, transform.position + prevPos, Quaternion.identity) as GameObject;
						w.weapon.rigidbody2D.velocity = transform.TransformDirection (Vector3.forward * w.speed);
						w.weaponOut = true;
				}
		}

		private void errorMessage (string title, string msg)
		{
				if (Application.isEditor) {
						EditorUtility.DisplayDialog (title, msg, "Ok");
						EditorApplication.isPlaying = false;
				}
		}

		private Direction getFaceDirection ()
		{
				if (prevPos != transform.position) {
						prevPos = transform.position;
						if (transform.position.x > prevPos.x) {
								return Direction.North;
						}
						if (transform.position.x < prevPos.x) {
								return Direction.South;
						}
						if (transform.position.y > prevPos.y) {
								return Direction.East;
						}
						if (transform.position.y > prevPos.y) {
								return Direction.West;
						}
				}
				return Direction.East;
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
}
