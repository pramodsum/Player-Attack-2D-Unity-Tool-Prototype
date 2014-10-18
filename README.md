2D Unity Player Script
======================

Set up attacks easily for GameObjects in Unity for **Top Down or Platformer** 2D games

PlayerScript.cs
===============

This script is defined as a monobehaviour that can be dragged and dropped onto any object in the unity project.  The purpose of the PlayerScript class is to provide all the basic operations needed in setting up a player in a Unity 2D game.  The PlayerScript class maintains 2 weapons, custom attack keys and one of 3 different attack methods for each weapon, item inventory, player movement, health, damage, and direction.

Settings
========
The default values for each of the following variables has been shown. Customize each of these values 

* All enemies **must** have tag "Enemy"
* The ground (in platformer games) **must** have a tag "Ground"
*

```
/***************************************************************************
 * PLAYER
 **************************************************************************/
public float health = 6f;
public float damage = 0.5f;  

//Top Down Settings
public bool allowDiagonalMovement = false;

//Platformer Settings
public bool isPlatformer = false;
public float jumpHeight = 50f;
public float weight = 2f;
public bool jetpackEnabled false;
    
/***************************************************************************
 * WEAPON 1
 **************************************************************************/
public GameObject weapon1;
public string attackKey1;
public bool w1IsShootable = false;
public bool w1IsJabbable = false;
public float w1Strength;  

/***************************************************************************
 * WEAPON 2
 **************************************************************************/
public GameObject weapon2;
public string attackKey2;
public bool w2IsShootable = false;
public bool w2IsJabbable = false;
public float w2Strength;
		
```

Usage
=============
This is just like any other script used in Unity where you can drag and drop it onto an object and the script will appear in that objects inspector. The methods shown below are public and can be called from other classes as well. 

Public Functions
================

```
//REQUIRES: 
//MODIFIES: 
//EFFECTS: 
//RETURNS: 
public void attack (Weapon w);
		
//REQUIRES: 
//MODIFIES: 
//EFFECTS: 
//RETURNS: 
public void jab (Weapon w, Vector3 dir);

//REQUIRES: 
//MODIFIES: 
//EFFECTS: 
//RETURNS: 
public void shoot (Weapon w, Vector3 dir);

```