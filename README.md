2D Unity Player Script
======================

Set up attacks easily for GameObjects in Unity for **Top Down or Platformer** 2D games

PlayerScript.cs
===============

This script is defined as a monobehaviour that can be dragged and dropped onto any object in the unity project.  The purpose of the PlayerScript class is to provide all the basic operations needed in setting up a player in a Unity 2D game.  The PlayerScript class maintains 2 weapons, custom attack keys and one of 3 different attack methods for each weapon, item inventory, player movement, health, damage, and direction.

Usage
=============
This is just like any other script used in Unity where you can drag and drop it onto an object and the script will appear in that objects inspector. 

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