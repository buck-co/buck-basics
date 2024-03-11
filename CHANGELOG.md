# Changelog

## [2.0.5] - ????-??-??
-Added SoftSingleton class as alternate to Singleton, which allows destroying and refilling the Instance during runtime.

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