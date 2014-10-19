2D Unity Player Script
======================

Set up attacks easily for GameObjects in Unity for **Top Down or Platformer** 2D games

PlayerScript.cs
===============

This script is defined as a monobehaviour that can be dragged and dropped onto any object in the unity project.  The purpose of the PlayerScript class is to provide all the basic operations needed in setting up a player in a Unity 2D game.  The PlayerScript class maintains 2 weapons, custom attack keys and one of 2 different attack methods for each weapon, item inventory, player movement, health, damage, and direction.

There is also a WeaponScript.cs included in this tool, which sets up the collisions for jabs and bombs based on the positioning of the weapon at the time of collision. 

Settings
========
The default values for each of the following variables has been shown. Customize each of these values 

* All enemies **must** have tag "Enemy"
* The ground/floor (mainly in platformer games) **must** have a tag "Ground"
* Any items which need to be collected in the inventory need to be added to the inventory class


```C#
/***************************************************************************
 * PLAYER
 **************************************************************************/
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
		
```

Usage
=============
This is just like any other script used in Unity where you can drag and drop it onto an object and the script will appear in that objects inspector. The methods shown below are public and can be called from other classes as well. 

**Make sure to add WeaponScript.cs as an asset to the project as well.** If you choose to use your own Weapon Collision Script, replace the line in initWeapon() which adds WeaponScript as a component to each generated weapon.

Public Methods/Classes
======================

```C#
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

    //Weapon Copy Constructor
    public Weapon (GameObject w, string a, bool s, bool j, float p)
    {
        weapon = w;
        attackKey = a;
        isBombable = s;
        isJabbable = j;
        strength = p;
    }
}

//REQUIRES: Weapon
//MODIFIES: nothing
//EFFECTS: Either jabs weapon or throws bomb from player location
//RETURNS: nothing
public void attack (Weapon w);
		
//REQUIRES: Weapon, direction faced by player
//MODIFIES: nothing
//EFFECTS: Instantiates weapon for as long as attackKey is pressed
//RETURNS: nothing
public void jab (Weapon w, Vector3 dir);
    
//REQUIRES: Weapon, direction faced by player
//MODIFIES: nothing
//EFFECTS: Destroys all enemies within a 5.0f range of bomb location
//RETURNS: nothing
public void shoot (Weapon w, Vector3 dir);

```

Private Methods
===============

```C#
//REQUIRES: nothing
//MODIFIES: Player health, damage, collider + settings, rigidbody + settings
//EFFECTS: initializes player with settings
//RETURNS: nothing
private void initPlayer ();

//REQUIRES: weapon gameobject, attack key, is bombable, is jabbale
//MODIFIES: Weapon attack key, script, collider + settings, 
//          rigidbody + settings
//EFFECTS: initializes weapon with settings
//RETURNS: Weapon object
private Weapon initWeapon (GameObject weapon, string key, bool bomb, bool jab);

//REQUIRES: nothing
//MODIFIES: nothing
//EFFECTS: Checks that all required player and weapon settings have been set
//RETURNS: nothing
private void checkSettings ();

//REQUIRES: error title and message
//MODIFIES: nothing
//EFFECTS: Shows error dialog and stops game build in editor
//RETURNS: nothing
private void errorMessage (string title, string msg);

//REQUIRES: error title and message
//MODIFIES: nothing
//EFFECTS: Shows warning dialog in editor
//RETURNS: nothing
private void warningMessage (string title, string msg);

//REQUIRES: nothing
//MODIFIES: player transform position
//EFFECTS: moves player according
//RETURNS: nothing
private void getMovement ();

//REQUIRES: nothing
//MODIFIES: player transform position
//EFFECTS: moves player in grid-like manner
//RETURNS: nothing
private void gridMovement ();

//REQUIRES: nothing
//MODIFIES: player transform position
//EFFECTS: moves player allowing for diagonal movement
//RETURNS: nothing
private void fluidMovement ();

//REQUIRES: Weapon object
//MODIFIES: anything colliding with weapon or enemy
//EFFECTS: Gets attack and fires off respective method 
//RETURNS: nothing
private void getAttack (Weapon w);

//REQUIRES: inventory list, collided item
//MODIFIES: nothing
//EFFECTS: finds index of object in inventory
//RETURNS: index of object in inventory
private int findObjInInventory (List<GameObject> inventory, GameObject obj);

```

WeaponScript
============

```C#
public class WeaponScript : MonoBehaviour
{ 
    public bool isBombable = false;
    public float timeLeft;
    public float range = 5.0f;

    void OnCollisionEnter2D (Collision2D coll);
  
    //REQUIRES: time to wait
    //MODIFIES: enemies within the radius of bomb
    //EFFECTS: waits t seconds then destroys all objects w/in range of bomb, then
    //         the bomb self destructs
    //RETURNS: nothing
    public IEnumerator wait (float t);
}

```