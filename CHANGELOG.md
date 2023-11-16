# Changelog

## [1.0.8] - 2022-11-16

-Added Condition class that can be used to create boolean logic comparisons of a subset of BUCK Basics Reference variable types defined in the inspector
-Added many new extension methods: 
    CanvasGroup.SetVisible() 
    VectorInt to/from regular Vector transformation methods
    Math calculations for RectTransforms, Bounds, and Screen Space
    Basic animation methods for floats: EaseOut, EaseIn, and Smoothstep
    String.Truncate()
    Color.Multiply()
-Added new ScriptableObject Variables and Reference classes for the following types:
    UnityEngine.Color, Vector2, Vector4, Vector2Int, Vector3Int, Double, GameObject, Texture2D, Sprite, and Material
-Fixed potential errors with GameEvents constructed at runtime by adding null checks

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