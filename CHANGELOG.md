# Changelog

## [3.1.2] - 2025-07-30
- Added .meta files back for GitHub files since their absence was causing errors in Unity due to package folders being immutable.

## [3.1.1] - 2025-07-16
- First public release! Added a code of conduct, MIT license, and license information to all files.

## [3.1.0] - 2025-04-14
New Feature: The ObjectPooler component now has a "Pooler Behavior" option that determines what happens when the pooler is out of objects. Previously, the ObjectPooler would just give a warning. Now, it includes the following options:
- Recycle Oldest: Recycle the oldest object in the pool and use it immediately. This is the new default behavior.
- Double Size: Double the size of the pool and then use newly created objects (which is generally not ideal because calling Instantiate can cause GC spikes)
- Warn: Log a warning that the pooler is out of objects and do nothing.

Additionally, the ObjectPooler will now call ClearAll() and destroy all of its pooled objects when its OnDestroy method is called. This is to prevent any generated objects from continuing to exist when their parent pooler has been destroyed.

## [3.0.8] - 2025-03-06
- Removed final catch block of exception handling in GameEvent.Raise() to allow event listeners to display Unity's normal stack trace and exception information.

## [3.0.7] - 2024-12-13
- Fixed an issue in GameEvent.Raise() where ArgumentOutOfRange exceptions could be thrown while iterating over a GameEvent's listeners if the collection is modified while iterating.
- Added some exception handling in GameEvent.Raise() to allow event listeners to continue gracefully even if some items in the collection are null.

## [3.0.6] - 2024-08-14
- Fixed an issue where the Log Value button would throw an error on IntVariables and FloatVariables.
- Fixed some formatting inconsistencies when inspecting variables.

## [3.0.5] - 2024-08-08
- BaseVariable.ResetValueToDefault() is now a public method.

## [3.0.4] - 2024-08-07
- Added constructors for Operations and Conditions so that they can be created and added from C# scripts.
- Added constructors to all variable reference types that accept their ScriptableObject Variable type.
- Added get property accessors to Conditions and Operations.

## [3.0.3] - 2024-06-04
- Updated GenericOperation.cs methods. Created ExecuteIfPassedAndReturn() which returns a bool if conditions are passed, also added ExecuteIfPassed() which is void and can be used with UnityEvents
- Added TriangleNumber() to ExtensionMethods

## [3.0.2] - 2024-05-10
- Fixed an issue where variable ToString() methods would return a null reference exception if the Value property was null.

## [3.0.1] - 2024-05-09
- Fixed an issue where Inspectors for VectorVariable types Vector3Variable, Vector2Variable, Vector3IntVariable, and Vector2IntVariable were displaying as the higher precision `Vector4` type.
- Fixed an issue where Inspectors for NumberVariable types IntVariable and FloatVariable were displaying as the higher precision `double` type.
- Simplified some of the inheritance for variable type custom Inspectors.

## [3.0.0] - 2024-04-04
- Extensive refactor of variable classes to use a generic base class, BaseVariable<T>, which reduces the amount of code needed in each child class.
- Added SoftSingleton class as alternate to Singleton, which allows destroying and refilling the Instance during runtime.
- Removed SetValue() methods from all variable classes, as it was redundant with the Value property.
- Implemented the IFormattable interface on all variable classes, allowing for more standard string formatting.
- Added the GameEventListenerReference class which allows non-MonoBehaviour classes to subscribe to GameEvents.
- All types inheriting from BaseVariable<T> now have a list of "reset" events. If any of the events in the list are raised, the variable will reset to its initial value.
- Added unit coverage using the Unity Test Framework package for Variable.Value properties, Conditions, and Operations.
- Minor spelling fixes and code formatting cleanup.

## [2.0.4] - 2024-03-08
- Added extension methods for operating on Color members contained in the types Graphic, SpriteRenderer, and Material

## [2.0.3] - 2024-03-07
- Added GenericOperation MonoBehaviour
- RuntimeSet now inherits from GameEvent and can be subscribed to or raised.
- Added a couple more casting methods to the Vector ExtensionMethods: Vector3.ToVector2Int() and Vector2Int.ToVector3()
- Cleaned up the code style formatting and comments in nearly every file.

## [2.0.2] - 2024-01-04
- Added Angle360Positive method to extension methods
- All NumberVariable classes now support ValueAsStringFormatted() methods which support returning the variable's value as a string using particular formatters.

## [2.0.1] - 2023-12-11
- Updated the Singleton class to use the more modern Object.FindAnyObjectByType method, since Object.FindObjectOfType is becoming obsolete.
- Required Unity version has been bumped to 2021.3 to support Object.FindAnyObjectByType.
- Added this keyword to some extension method arguments in order to support implicit calls.

## [2.0.0] - 2023-11-16
- Added Condition class that can be used to create boolean logic comparisons of a subset of BUCK Basics Reference variable types defined in the inspector
- Added many new extension methods: 
    CanvasGroup.SetVisible() 
    VectorInt to/from regular Vector transformation methods
    Math calculations for RectTransforms, Bounds, and Screen Space
    Basic animation methods for floats: EaseOut, EaseIn, and Smoothstep
    String.Truncate()
    Color.Multiply()
- ExtensionMethods.cs was split into multiple .cs files using partial class implementation. Purely an organizational improvement of the source code of the package with no impact on usage.
- Added new ScriptableObject Variables and Reference classes for the following types:
    UnityEngine.Color, Vector2, Vector4, Vector2Int, Vector3Int, Double, GameObject, Texture2D, Sprite, and Material
- Added new extension methods for converting Guids into serializable byte arrays and vice versa
- Fixed potential errors with GameEvents constructed at runtime by adding null checks

## [1.0.7] - 2022-09-05
- Added FindByGuid() methods to the BaseScriptableObject class.

## [1.0.6] - 2022-07-23
- Added serialized GUIDs to SO variables via a new BaseScriptableObject class.

## [1.0.5] - 2022-07-18
- Fixed an issue in the Remap() methods that could cause them to return NaN.

## [1.0.4] - 2022-02-08
- Added Singleton class and object pooler classes.

## [1.0.3] - 2022-02-08
- Added Runtime Sets.

## [1.0.2] - 2022-02-08
- Fixed compilation errors resulting from the UnityEditor namespace not being wrapped in pragma definitions.

## [1.0.1] - 2022-01-20
- Added scriptable object variables and game events.

## [1.0.0] - 2022-01-19
- Initial commit.