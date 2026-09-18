kOS WPKSP AddOn
***
This project is an AddOn for the Kerbal Space Program mod kOS. It provides a collection of functionalities useful for WPKSP (We play Kerbal Space Program) and exposes them to kerboscript.

# Functionalities

## ADDONS:WPKSP



Suffix|Type|Get/Set/Function|Description
-|-|-|-
``Version``|String|Get|The installed version of kOS-WPKSP
``HideToolbar``|-|void Funciton|Hides the main toolbar
``ShowToolbar``|-|void Function|Shows the main toolbar
|
``Resources``|Boolean|Get/set|Is the resouces window visible [^1]
|
``Info`` ``ShowInfo``|-|void Function(String *selection*)|Select which information should be displayed in the FlightInfoUI (bottom left corner):<br><br>*"INPUT"*, *"STAGING"*, *"DEFAULT"*:<br>Steering axes<br><br>*"MANEUVER"*, *"ORBIT"*, *"ORBITAL"*:<br>Orbital pamameters<br><br>*"DOCK"*, *"DOCKING"*:<br>Docking view / Steering + Translation axes
``Time`` ``ShowTime``|-|void Function(String *selection*)|Select the time format in the METDisplay (upper left corner):<br><br>*"MET"*, *"MISSION"*, *"MISSIONTIME"*:<br>MET (Mission Elapsed Time)<br><br>*"UT"*, *"UNIVERSAL"*, *"UNIVERSALTIME"*, *"CALENDAR"*:<br>UT (Universal Time)
``ToggleTime``|-|void Function|Toggle the METDisplay (upper left corner) between MET and UT
|
``Cutout`` ``Cutouts`` ``ToggleCutouts``|-|void Function|Toggle the *InternalSpaceOverlay* (ALT+C or the tiny radial button next to the Kerbal Portraits)
|
``Mass`` ``CoM`` ``ToggleCoM``<br><br>``Lift`` ``CoL`` ``ToggleCoL``<br><br>``Thrust`` ``CoT`` ``ToggleCoT``|-|void Function|Toggle the in-flight overlay markers for *<b>CoM</b> (Center of Mass)*, *<b>CoL</b> (Center of Lift)* or *<b>CoT</b> (Center of Thrust)*
|
``HUD``|Boolean|Get/Set|Is the HUD visible (F2)
``ShowHUD``|-|void Function|Show the HUD
``HideHUD``|-|void Function|Hide the HUD
``ToggleHUD``|-|void Function|Toggle the HUD
|
|

### Example cases
You can use the suffixes directly on ADDONS:WPKSP:
```
set ADDONS:WPKSP:RESOURCES to true.
ADDONS:WPKSP:HIDETOOLBAR.
```
... or use a var to make your keyboard last longer:
```
set w to ADDONS:WPKSP.
if not w:Resources print ship:resources.
print "You can " + (choose "" if w:HUD else "not ") + "read this!".
```

## TERMINAL
The ``TERMINAL`` structure is expanded with the follwing:

Suffix|Type|Get/Set/Function|Description
-|-|-|-
``X``|Scalar|Get/Set|The x position of the terminal window
``Y``|Scalar|Get/Set|The y position of the terminal window
|
``Title``|String|Get|The title of the terminal window
``Cursor``|Boolean|Get/Set|Is the terminal drawing its cursor
|
``Scroll``|-|void Function(Int *offset*)|Scroll the terminal by *offset* number of rows<br> Positive *offset* scrolls down, Negative *offset* scrolls up
``Focus``|-|void Function|Make the terminal take the focus and bring it to the front
|
|

><b>Please note that the way the ``TERMINAL`` structure is being expanded will very likely conflict with other AddOns using the same approach. If that happens i expect only one of the AddOns to succeed without any error or notification!!</b>

### Example cases
Is the list of files longer than the terminal is high?
```
list.
terminal:scroll(-terminal:height).
```