Attack Tool Script Unity
========================

Set up attacks easily for GameObjects in Unity for 2D games

AttackScript.cs
===============

This script is defined as a monobehaviour that can be dragged and dropped onto any object in the unity project.  The purpose of the ExperienceSystem class is to provide an experience point based system of leveling up.  The ExperienceSystem class maintains the objects current experience points, current level, maximum level, an experience table, an experience point multiplier, and other bookkeeping variables.

There are two ways of using the ExperienceSystem class in a Unity project.  You can either use it as a Monobehaviour which can be dragged and dropped onto another object in the unity project/hierarchy pane or it can be defined as a standalone class in a "has-a" relationship to your objects.  The two are demonstrated below.

AttackScript as a Monobehaviour
==================================
This is just like any other script used in Unity where you can drag and drop it onto an object and the script will appear in that objects inspector.  If you also used the Editor/ExpInspector.cs file properly you will see a custom inspector area for the ExperienceSystem script in the objects inspector pane.  More about ExpInspector.cs below.

If you simply drag and drop the script onto the object you can access the experience system by doing the following.

- Declare an ExperienceSystem variable in your objects script.
- variableName = this.GetComponent<ExperienceSystem> ();
- variableName.initialize(parameters)

There are a couple things to note here.  

First that you MUST call .initialize on your ExperienceSystem object once you have it.  It is undefined what happens if you were to use the object without initializing it.  Since Monobehaviours do not allow constructors via new this is doing the work a constructor would do.

Second this means that if you are using a monobehaviour version of the ExperienceSystem there should only be one copy of it.  Therefore if you want different ExperienceSystems for different abilities this will not work.  If you want to have multiple ExperienceSystem objects I would recommend the ExperienceSystem as standalone class below.

ExperienceSystem as standalone class
====================================
If you want to be able to have as many ExperienceSystem objects as you want this is the best route to go.  You can make your object have various instances of the class (a "has-a" relationship). You will have to modify the code briefly to do so.  To fix the code do the following:

- Delete the ": Monobehaviour" in the class definition 
- Uncomment the constructors/destructor at the bottom of the code

You should be good to go to create ExperienceSystem objects through code rather than the inspector now.  You do not need to call initialize on these objects becaues the constructors do the same job.


Example Usage as Monobehaviour
==============
```
public class Player : Monobehaviour {

  ExperienceSystem expSystem;

  void Start(){
    //assuming the script has been dropped on the Player object
    expSystem = this.GetComponent<ExperienceSystem> ();
  
    //currentExp, expWorth, currentLevel, maxLevel, multiplier, callback function
    expSystem.initialize(0f, 200f, 1, 20, 1f, levelUpCallback);
  }

  void levelUpCallback(){
    //This runs when the object levels up.
  }
  
  void someFunction(){
    expSystem.addExp(100f);
    expSystem.printExpState();
  }
}
```

Public Functions
================

```

//Function declaration of the callback function.  Function 
//must return void and take no parameters
public delegate void LevelUpCallback();

//Requires: Nothing
//Modifies: The state of the ExperienceSystem object
//Effects:  Sets the 3 parameters in the object.  The objects current exp is
//      automatically set to 0, current level is set to 1, and the multiplier
//      is set to one.
//Returns:  Nothing
public void initialize(float expWorth, int maxLevel, LevelUpCallback levelUpFunction)

//Requires: Nothing
//Modifies: The state of the ExperienceSystem object
//Effects:  Sets the parameters in the object.  The multiplier is set to 1f if
//      the value provided is less than zero.
//Returns:  Nothing
public void initialize(float currentExp, float expWorth, int currentLevel, int maxLevel,
                   float multiplier, LevelUpCallback levelUpFunction)

//Requires: Nothing
//Modifies: The state of the ExperienceSystem object
//Effects:  Sets the parameters in the current object to that of other.
//      Note it does NOT copy the experience table, it defaults to
//      the original exp table.
//Returns:  Nothing
public void initialize(ExperienceSystem other)

//Requires: LevelUpCallback function prototype is void LevelUpCallback()
//      So you need to pass a function that takes no arguments and returns void.
//Modifies: The objects level up function callback
//Effects:  When the object levels up this function is called so the user
//      can be notified when an object has leveled up.
//Returns:  Nothing
public void setLevelUpCallbackFunction(LevelUpCallback levelUpFunction)

//Requires: Nothing
//Modifies: current exp and current level
//Effects:  Adds the desired exp to the object.
//      if the object levels up will call
//      the levelupcallback function.  Cannot
//      add more exp than the max level in the
//      exp table will allow.  Note: will factor
//      in the object exp multiplier when adding
//      exp.  
//      Note: This takes into account the objects exp multiplier.
//Returns:  -1 if val < 0 or the objectis at the max 
//      level or returns the new current exp
public float addExp(float val)

//Requires: Nothing
//Modifies: current exp and current level
//Effects:  Adds the exp worth of the other object to 
//      this object. If the object levels up will call
//      the levelupcallback function.  Cannot
//      add more exp than the max level in the
//      exp table will allow. Note: will factor
//      in the object exp multiplier when adding
//      exp.
//      Note: This takes into account the objects exp multiplier.
//Returns:  -1 if val < 0 or the objectis at the max 
//      level or returns the new current exp
public float addExp(ExperienceSystem other)

//Requires: Val >= 0
//Modifies: currentExp
//Effects:  Substracts the given value from the objects
//      exp.  If the exp were to drop below the current
//      levels lower exp bound it will set the current
//      exp to the lower bound.  For example if the
//      object exp level is from 400 to 500 and current
//      exp is 450.  If you try to subtract 100 the current
//      exp will be set to 400 (the lower bound).
//      Note: This takes into account the objects exp multiplier.
//Returns:  -1 if val < 0 or the object is already at the max level
//      or returns the new current exp.
public float subtractExp(float val)

//Requires: Nothing
//Modifies: Nothing
//Effects:  Returns the objects current exp.
//Returns:  The objects current exp.
public float getCurrentExp()


//Requires: Nothing
//Modifies: Nothing
//Effects:  Returns the objects current level.
//Returns:  The objects current level.
public int getCurrentLevel()

//Requires: Nothing
//Modifies: Nothing
//Effects:  Returns the objects max level.
//Returns:  The objects max level.
public int getMaxLevel()

//Requires: Nothing
//Modifies: Nothing
//Effects:  Returns the lower limit of the current level.  If
//      the current level goes from 400 to 500 exp this would
//      return 400.
//Returns:  -1 if the object is at max level or the lower limit
//      exp of the current level.
public float getLowerExpOfLevel()

//Requires: Nothing
//Modifies: Nothing
//Effects:  Returns the upper limit of the current level.  If
//      the current level goes from 400 to 500 exp this would
//      return 500.
//Returns:  -1 if the object is at max level or the upper limit
//      exp of the current level.
public float getUpperExpOfLevel()

//Requires: Nothing
//Modifies: Nothing
//Effects:  Returns the total exp required to level up at the current 
//      level.  So if the current level goes from 400 to 500 exp
//      this function would return 100.
//Returns:  -1 if the object is at max level or the total
//      exp required at the current level
public float getExpToLevelUp()

//Requires: Nothing
//Modifies: Nothing
//Effects:  Returns the current exp required to level up at the current 
//      level.  So if the current level goes from 400 to 500 exp and
//      the object is at 480 exp this function would return 20.
//Returns:  -1 if the object is at max level or the current
//      exp required at the current level
public float getExpToLevelUpFromCurrent()

//Requires: Nothing
//Modifies: Nothing
//Effects:  Returns the amount of Exp required to get from
//      the current exp to the max level exp
//Returns:  -1 if object is at max level, exp otherwise
public float getExpTilMaxLevel()

//Requires: Nothing
//Modifies: Current exp and current level
//Effects:  Sets the objects current exp to zero and
//      current level to one. 
//Returns:  Nothing
public void resetExp()

//Requires: Level to be a valid index in the exp table
//Modifies: Object current exp and current level
//Effects:  Resets the object exp to the lower limit of a given level 
//      as long as it's not greater than maxLevel.  Note that 
//      this does not call the levelup callback function.  Also it does 
//      not check that the level requested is a valid entry
//      in the exp table
//Returns:  Nothing
public void resetExpToLevel(int level)

//Requires: Nothing
//Modifies: Current exp
//Effects:  Resets the objects current exp to the lower level limit.
//      If object is at max level does not do anything
//Returns:  Nothing
public void resetExpToLowerLimit()

//Requires: Nothing
//Modifies: Nothing
//Effects:  Returns a bool if the object is at the current
//      max level.  True if yes, false otherwise
//Returns:  True if current level == max level, false otherwise
public bool isMaxLevel()

//Requires: Nothing
//Modifies: Object current exp & level
//Effects:  Increments the objects level and sets the
//      current exp to the lower limit of that level.
//      Calls the level up callback function.  Does nothing
//      if object is already at the max level.
//Returns:  Nothing
public void instantLevelUp()

//The following 'calc' functions are used to help you decide how 
//much an object should be worth.  There are 3 paramaters in the following
//functions: percent, number, and level.  Percent is used for when you say 
//'I want this object to be worth <percent>% of a level/total level etc.'.  Number
//is used when you say 'I want the <number> of objects to be worth a level or
//total level'.  Lastly <level> is used to make calculations based off a given
//level in the exp table.
//Note: the calc functions do NOT take into account the exp multiplier

//Requires: Percent must be in the form 0.####. 43.25% = 0.4325 and percent > 0
//Modifies: Nothing
//Effects:  Given a percentage this calculates how much exp an object
//      should be worth based on the current levels total exp.  So
//      if the current level exp is from 400 to 500 and percent is 0.73
//      this would return 73.  
//Returns:  -1 if object is at max level otherwise exp value
public float calcWorthPercentOfCurrentLevel(float percent)

//Requires: Number > 0
//Modifies: Nothing
//Effects:  Given a number of objects this calculates how much exp an object
//      should be worth based on the current levels total exp.  So
//      if the current level exp is from 400 to 500 and number is 20
//      this would return 5.  Note: this does not check if number > 0
//Returns:  -1 if object is at max level otherwise exp value
public float calcWorthNumberOfCurrentLevel(int number)

//Requires: Percent must be in the form 0.####. 43.25% = 0.4325 and percent > 0
//Modifies: Nothing
//Effects:  Given a percentage this calculates how much exp an object
//      should be worth based on the total/max levels total exp.  So
//      if the max level exp is 1000 and percent is 0.73
//      this would return 730.  
//Returns:  -1 if object is at max level otherwise exp value
public float calcWorthPercentOfTotalLevel(float percent)

//Requires: Number > 0
//Modifies: Nothing
//Effects:  Given a number of objects this calculates how much exp an object
//      should be worth based on the max/total levels exp.  So
//      if the max level exp is 1000 and number is 20
//      this would return 50.  Note: this does not check if number > 0
//Returns:  -1 if object is at max level otherwise exp value
public float calcWorthNumberOfTotalLevel(int number)

//Requires: Percent must be in the form 0.####. 43.25% = 0.4325 and percent > 0
//Modifies: Nothing
//Effects:  Given a percentage this calculates how much exp an object
//      should be worth based on a given levels total exp.  So
//      if the givens level exp is 500-650 and percent is 0.5
//      this would return 75.  
//Returns:  -1 if object is at max level otherwise exp value
public float calcWorthPercentAtLevel(float percent, int level)

//Requires: Number > 0
//Modifies: Nothing
//Effects:  Given a number of objects this calculates how much exp an object
//      should be worth based on a given levels exp.  So
//      if the given levels exp is 500-650 and number is 50
//      this would return 3.  Note: this does not check if number > 0
//Returns:  -1 if object is at max level otherwise exp value
public float calcWorthNumberOfAtLevel(int number, int level)

//Requires: Nothing
//Modifies: Nothing
//Effects:  Returns how much exp this object is worth
//Returns:  This objects exp worth.
public float getWorth()

//Requires: Worth is >= 0
//Modifies: This objects exp worth
//Effects:  Sets this objects exp worth to the given value.
//      Does not check if worth is < 0.
//Returns:  Nothing
public void setWorth(float worth)

//Requires: Nothing
//Modifies: Nothing
//Effects:  Returns the objects multiplier
//Returns:  This objects multiplier.
public float getMultiplier()

//Requires: Multiplier is > 0
//Modifies: This objects multiplier
//Effects:  Sets this objects multiplier to the given value.
//      Does not check if multiplier is < 0.
//Returns:  Nothing
public void setMultiplier(float multiplier)

//Requires: Max level is valid in the exp table && >= 0 && current level <= max level
//Modifies: The players max level
//Effects:  Sets the objects max level to the one provided.  If
//      any of the required criteria above are not met the function
//      returns false and prints a debug message.
//Returns:  False if level was not set, true if it was.
public bool setMaxLevel(int maxLevel)

//Requires: Nothing
//Modifies: Nothing
//Effects:  Prints to the console log the current experience state of
//      the object.  The debug statements are labeled with an integer
//      in increasing order to help identify them in the log.
//Returns:  Nothing
public void printExpState()

//Requires: level <= max level
//Modifies: Nothing
//Effects:  Prints the 'calc' functions to the console.  These
//      statements are not identified with a print statement.
//      Does not print if the requires criteria is violated
//      and prints a warning statement about it.
//Returns:  Nothing
public void printCalcExp(float percent, int number, int level)

//Requires: newExptable to be a valid and allocated list
//Modifies: The exp table, current exp, current level
//Effects:  Adds newExpTable into the objects exp table. The array
//      should be sorted and newExpTable[i] < newExpTable[i+1].
//      The resulting table will always have expTable[0] = 0
//      followed by expTable[1] = newExpTable[0] etc.
//      Note: this resets the objects exp to 0 and level to 1 and max level to 
//      newExpTable.count.  Does nothing if newExpTable.count < 1
//Returns:  Nothing
public void setNewExpTable(List<float> newExpTable)



```