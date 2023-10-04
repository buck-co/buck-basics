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

Over several projects, BUCK has collected many useful extension methods that are applicable to a wide variety of scenarios. These are available in the `Buck` namespace in the [`ExtensionMethods`](Runtime/ExtensionMethods.cs) class. Here are some highlights:

- [`void Shuffle(IList<T> list)`](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L11) - Effectively randomizes the order of elements in a C# List using the [Fisher–Yates shuffle](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle) algorithm.
- [`T[,] Rotate90<T>(T[,] arr)`](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L174) - Returns a 2D array that has been rotated 90 degrees CW. There are numerous other array manipulation methods like this one that can transpose rows, columns, and more.
- [`bool IsVisibleFrom(Renderer renderer, Camera camera)`](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L112) - Returns true if a Renderer is visible from the provided Camera.
- [`Vector2 RandomPointOnUnitCircle()`](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L328) - Returns a random point on a unit circle.
- [`Color Tint(Color value, float tint)`](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L306) - Adds a tint to a Color.
- [`Transform NearestTransform(Transform origin, List<Transform> positions)`](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L346) - Given a list of Transforms, return the one that is nearest to an origin Transform.
- [`float Smootherstep(float from, float to, float x)`](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/ExtensionMethods.cs#L397) - [Ken Perlin's better smooth step](https://en.wikipedia.org/wiki/Smoothstep#Variations) with 1st and 2nd order derivatives at x = 0 and 1.

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

### Scriptable Objects for Variables, Events, and Runtime Sets

Several Unity projects at BUCK make heavy use of variables and events that are built on Unity's [`ScriptableObject`](https://docs.unity3d.com/Manual/class-ScriptableObject.html) class. In other words, the variables and events have associated Scriptable Object assets that then store their values at runtime. There are numerous benefits to this approach, but generally speaking, it allows for data to be shared between MonoBehaviour components without hard coded object references. This tends to help avoid a tangled mess of component dependencies and numerous static singleton classes.

This is a fairly involved subject, but fortunately it's already well documented in several places, [most notably in this Unite Austin 2017 talk from its creator, Ryan Hipple of Schell Games](https://www.youtube.com/watch?v=raQ3iHhE_Kk). We strongly recommend you watch this video before attempting to use SO Variables and Events in a production project.


### Singleton Class

While the Scriptable Object based architecture mentioned above can help avoid Singletons and some of the bad practices that can stem from them (single instances of a class, strong connections between classes, dependency spaghetti, and more), sometimes the [singleton pattern](https://en.wikipedia.org/wiki/Singleton_pattern) is still too convenient to ignore, especially during any rapid prototyping phases of a project. We've reviewed many different singleton implementations in C# around the web, and [this one](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/Utility/Singleton.cs) seems to be the de facto standard. We've used it on several projects and it seems to work very well. While this exact implementation can be found in many places, we're unsure of its origins, so if you know whom to credit for this, please let us know!
  
  
### BaseScriptableObject Class
  
Many of our Scriptable Object types inherit from our [`BaseScriptableObject`](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/Utility/BaseScriptableObject.cs) class. The primary reason for this is so that we can attach a [GUID](https://learn.microsoft.com/en-us/dotnet/api/system.guid?view=net-7.0) (globally unique identifier) to any of our Scriptable Objects (SO). The GUID attached to an SO will not change, which makes them useful for comparisons and for finding duplicate items in a list, for example. The `BaseScriptableObject` class also has associated methods like [FindByGuid<T>()](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/Utility/BaseScriptableObject.cs#L25) that make it easier to find a subset of a given SO type on disk.
  
  
### Object Pooling
  
We cultivated our own [`ObjectPooler`](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/Utility/ObjectPooler.cs) class over several projects, following as many Unity best practices as we could find for [object pooling](https://en.wikipedia.org/wiki/Object_pool_pattern), and put them into one place. This class also has a companion class, [`PoolerIdentifier`](https://github.com/buck-co/unity-pkg-buck-basics/blob/main/Runtime/Utility/ObjectPooler.cs), which is a MonoBehaviour component that gets attached to any object created by the object pooler. This way, if another script needs to grab a pooled object and potentially recycle it back to its original object pooler, it can identify the pooler from which the object was generated. This allows pooled objects to be instantiated anywhere in the Unity hierarchy without relying on their parent GameObject for identification.
  
