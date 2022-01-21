# BUCK Basics

This is BUCK's general Unity code package. It includes extension methods, utility scripts, and debugging tools.

## Requirements

This package works with Unity 2020.3 and above.

## Installation

1. Copy the git URL of this repository.
2. In Unity, open the Package Manager from the menu by going to `Window > Package Manager`
3. Click the plus icon in the upper left and choose `Add package from git URL...`
4. Paste the git URL into the text field and click the `Add` button.

## What's Included

### Extension Methods

Over several projects, BUCK has collected many useful extension methods that are useful across a wide variety of projects, ranging from math calculations like calculating the volume or surface area of a sphere, to handy algorithms that can rotate 2D arrays. These are available in the BUCK namespace in the [ExtensionMethods](Runtime/ExtensionMethods.cs) class.

Extension methods can be used in two ways. You can use the methods directly from the class.

```cs
// Remap a value from a min and max to the 0-1 range
float myValue = BUCK.ExtensionMethods.Remap01(originalVar, minValue, maxValue);
```

Alternatively, you can call extension methods on existing types. For example, there are several methods that extend Unity's Color class.

```cs
// Set the alpha value of a color.
Color myTransparentColor = myColor.SetAlpha(0.5f);
```

### Scriptable Object Variables and Events

Several Unity projects at BUCK are built on an architecture that makes heavy use of variables and events in the form of Unity's Scriptable Objects. There are numerous benefits to this approach, but generally speaking, it allows for variables and events to be shared among components without specific object references. This tends to help avoid a web of component dependencies and numerous static singleton classes.

This is a fairly involved subject, but fortunately it's well documented in several places, [including this Unite Austin 2017 talk from its creator, Ryan Hipple of Schell Games](https://www.youtube.com/watch?v=raQ3iHhE_Kk). Be sure to watch this video before attempting to use SO Variables and Events. 
