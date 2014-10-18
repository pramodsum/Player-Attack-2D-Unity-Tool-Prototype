using UnityEngine;
using UnityEditor;
using System.Collections;

public class AttackScript : MonoBehaviour
{
		protected class Weapon
		{
				public GameObject weapon;
				public GameObject jabAttack;
				public string attackKey;
				public bool isShootable = false;
				public bool isJabbable = false;

				public Weapon (GameObject w, string a, bool s, bool j)
				{
						weapon = w;
						attackKey = a;
						isShootable = s;
						isJabbable = j;
				}
		}

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
	
		void Start ()
		{
				checkSettings ();

				w1 = initWeapon (weapon1, attackKey1.ToLower (), w1IsShootable, w1IsJabbable);
//				GameObject.Instantiate (w1.weapon, transform.position, Quaternion.identity);
//				w1.weapon.renderer.enabled = false;
				w1.weapon.transform.parent = transform;
		
				w2 = initWeapon (weapon2, attackKey2.ToLower (), w2IsShootable, w2IsJabbable);
//				GameObject.Instantiate (w2.weapon, transform.position, Quaternion.identity);
//				w2.weapon.renderer.enabled = false;
				w2.weapon.transform.parent = transform;
		}
	
		void FixedUpdate ()
		{
				if (!string.IsNullOrEmpty (w1.attackKey) && !string.IsNullOrEmpty (w2.attackKey)) {
						if (Input.GetKeyDown (w1.attackKey)) {
								attack (w1);
						} else if (Input.GetKeyDown (w2.attackKey)) {
								attack (w2);
						} 
				}
		}
	
		private Weapon initWeapon (GameObject weapon, string key, bool shoot, bool jab)
		{
				weapon.AddComponent<PolygonCollider2D> ();
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
				w.jabAttack = Instantiate (w.weapon, transform.position, Quaternion.identity) as GameObject;
		}

		protected void shoot (Weapon w)
		{

		}

		private void errorMessage (string title, string msg)
		{
				if (Application.isEditor) {
						EditorUtility.DisplayDialog (title, msg, "Ok");
						EditorApplication.isPlaying = false;
				}
		}
}
