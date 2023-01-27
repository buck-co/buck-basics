# BUCK Basics

_BUCK Basics_ is BUCK's general Unity code package that includes broadly applicable extension methods, utility scripts, and a basic framework for architecting applications.

## Requirements

This package works with Unity 2020.3 and above.

## Installation

1. Copy the git URL of this repository.
2. In Unity, open the Package Manager from the menu by going to `Window > Package Manager`
3. Click the plus icon in the upper left and choose `Add package from git URL...`
4. Paste the git URL into the text field and click the `Add` button.

## What's Included

### Extension Methods

Over several projects, BUCK has collected many useful extension methods that are applicable to a wide variety of scenarios. These are available in the Buck namespace in the [ExtensionMethods](Runtime/ExtensionMethods.cs) class. Here are some highlights:

- [void Shuffle(IList<T> list)](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L11) - Effectively randomizes the order of elements in a C# List using the [Fisher–Yates shuffle](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle) algorithm.
- [T[,] Rotate90<T>(this T[,] arr)](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L174) - Returns a 2D array that has been rotated 90 degrees CW. There are numerous other array manipulation methods like this one that can transpose rows, columns, and more.
- [bool IsVisibleFrom(Renderer renderer, Camera camera)](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L112) - Returns true if a Renderer is visible from the provided Camera.
- [Vector2 RandomPointOnUnitCircle()](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L328) - Returns a random point on a unit circle.
- [Color Tint(this Color value, float tint)](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L306) - Adds a tint to a Color.
- [Transform NearestTransform(this Transform origin, List<Transform> positions)](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L346) - Given a list of Transforms, return the one that is nearest to an origin Transform.
- [float Smootherstep(float from, float to, float x)](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L397) - [Ken Perlin's better smooth step](https://en.wikipedia.org/wiki/Smoothstep#Variations) with 1st and 2nd order derivatives at x = 0 and 1

Extension methods can be used in two ways. You can use the methods directly from the class.

```cs
// Remap a value from a min and max to the 0-1 range
float myValue = Buck.ExtensionMethods.Remap01(originalVar, minValue, maxValue);
```

Alternatively, you can call extension methods on existing types. For example, there are several methods that extend Unity's Color class.

```cs
// Set the alpha value of a color.
Color myTransparentColor = myColor.SetAlpha(0.5f);
```

### Scriptable Object Variables and Events

Several Unity projects at BUCK are built on an architecture that makes heavy use of variables and events in the form of Unity's Scriptable Objects. There are numerous benefits to this approach, but generally speaking, it allows for variables and events to be shared among components without specific object references. This tends to help avoid a web of component dependencies and numerous static singleton classes.

This is a fairly involved subject, but fortunately it's well documented in several places, [including this Unite Austin 2017 talk from its creator, Ryan Hipple of Schell Games](https://www.youtube.com/watch?v=raQ3iHhE_Kk). Be sure to watch this video before attempting to use SO Variables and Events. 
