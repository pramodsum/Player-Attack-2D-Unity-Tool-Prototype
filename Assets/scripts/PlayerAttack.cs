using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
		//Weapons to attack with
		public GameObject weapon1;
		public GameObject weapon2;

		void Start ()
		{
				weapon1.AddComponent<PolygonCollider2D> ();
				weapon2.AddComponent<PolygonCollider2D> ();

				//Adjust Rigidbody2D
//				weapon1.AddComponent<Rigidbody2D> ();
//				weapon2.AddComponent<Rigidbody2D> ();
				weapon1.rigidbody2D.gravityScale = 0;
				weapon1.rigidbody2D.angularDrag = 0;
				weapon2.rigidbody2D.gravityScale = 0;
				weapon2.rigidbody2D.angularDrag = 0;

				weapon1 = Instantiate (weapon1, transform.position, Quaternion.identity) as GameObject;
				weapon2 = Instantiate (weapon2, transform.position, Quaternion.identity) as GameObject;
		}

		void FixedUpdate ()
		{
	
		}
}
