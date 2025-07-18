![Unity](https://img.shields.io/badge/Unity-2021.3%2B-blue)
![License](https://img.shields.io/badge/License-MIT-green)
![Version](https://img.shields.io/github/v/release/buck-co/buck-basics)

# BUCK Basics
_BUCK Basics_ is [BUCK](https://buck.co)'s Unity package that provides a foundation for scalable game architecture using ScriptableObject-based systems, comprehensive extension methods, and essential utility classes. It helps developers build cleaner, more maintainable Unity projects by reducing hard-coded dependencies and providing battle-tested tools refined across multiple productions.

### Features
- ⚡ **ScriptableObject Architecture**: Variables and Events as assets that enable decoupled communication between game systems
- 🔗 **150+ Extension Methods**: Time-saving utilities for vectors, colors, collections, geometry, and more
- 🔢 **Conditions & Operations**: Visual scripting-like functionality for logic and math operations without writing code
- ♻️ **Object Pooling**: High-performance pooling system with multiple overflow behaviors
- 📚 **Runtime Sets**: Dynamic collections that automatically track active game objects
- 🎲 **Battle-Tested**: Used in production on multiple shipped games including [_Let's! Revolution!_](https://www.letsrevolution.com/) and [_The Electric State: Kid Cosmo_](https://www.netflix.com/games/81746201)

# Getting Started
> [!NOTE]
> This package works with **Unity 2021.3 and above**.

### Install the _BUCK Basics_ Package

1. Copy the git URL of this repository: `https://github.com/buck-co/buck-basics.git`
2. In Unity, open the Package Manager from the menu by going to `Window > Package Manager`
3. Click the plus icon in the upper left and choose `Add package from git URL...`
4. Paste the git URL into the text field and click the `Add` button.

### Basic Workflow
The BUCK Basics package provides several independent systems that can be used together or separately:

1. **For ScriptableObject Variables**: Create variables via `Assets > Create > BUCK > Variables`, reference them in your scripts, and use GameEventListeners to react to changes
2. **For Extension Methods**: Simply add `using Buck;` to access 150+ extension methods on Unity types
3. **For Object Pooling**: Add an `ObjectPooler` component and configure your pooled prefabs
4. **For Conditions & Operations**: Define logic in the Inspector without code using the provided classes

### Included Samples

This package includes a comprehensive sample project demonstrating RPG-like mechanics using Variables, Events, Conditions, and Operations. Install it from the Unity Package Manager by selecting the package and clicking `Import` in the `Samples` tab.

# Core Systems

## ScriptableObject Variables & Events

BUCK Basics provides a powerful architecture pattern where variables and events exist as ScriptableObject assets. This approach, pioneered by Ryan Hipple at Schell Games, enables truly decoupled game systems.

> [!TIP]
> 📺 Watch [Ryan Hipple's Unite Austin 2017 talk](https://www.youtube.com/watch?v=raQ3iHhE_Kk) to understand the inspiration and philosophy behind this architecture.

### Creating Variables
Right-click in the Project window and navigate to `Create > BUCK > Variables` to create any supported type:
- **Primitives**: Bool, Int, Float, Double, String
- **Unity Types**: Vector2/3/4, Vector2Int/3Int, Quaternion, Color
- **References**: GameObject, Material, Sprite, Texture2D

### Using Variables in Code
```csharp
using Buck;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] IntVariable m_health;
    [SerializeField] IntVariable m_maxHealth;
    
    void TakeDamage(int damage)
    {
        m_health.Value -= damage;
        m_health.Raise(); // Notify listeners
        
        if (m_health.Value <= 0)
            HandleDeath();
    }
}
```

### Variable References
Use Reference types for maximum flexibility - they can be either a constant value or a ScriptableObject variable:

```csharp
[SerializeField] FloatReference m_moveSpeed; // Can be constant OR variable

void Update()
{
    transform.position += Vector3.forward * m_moveSpeed.Value * Time.deltaTime;
}
```

### GameEvents & Listeners
Every Variable inherits from GameEvent, allowing you to listen for changes:

```csharp
public class HealthBar : MonoBehaviour
{
    [SerializeField] IntVariable m_playerHealth;
    [SerializeField] Image m_fillImage;
    
    void OnEnable()
    {
        // Listen for health changes
        var listener = gameObject.AddComponent<GameEventListener>();
        listener.Event = m_playerHealth;
        listener.Response.AddListener(UpdateHealthBar);
    }
    
    void UpdateHealthBar()
    {
        m_fillImage.fillAmount = m_playerHealth.Value / 100f;
    }
}
```

## Conditions & Operations

Define game logic visually in the Inspector without writing code.

### Conditions
Create boolean logic that can be evaluated at runtime:

```csharp
[SerializeField] Condition m_canAttack; // Configure in Inspector
[SerializeField] Condition[] m_winConditions;

void Update()
{
    if (m_canAttack.PassCondition)
        PerformAttack();
        
    if (m_winConditions.PassConditions()) // All must be true
        TriggerVictory();
}
```

Supported comparisons: `==`, `!=`, `<`, `<=`, `>`, `>=`

### Operations
Execute mathematical operations on variables without code:

```csharp
[SerializeField] IntOperation m_scoreOperation; // Configured to add 10 points
[SerializeField] BoolOperation m_togglePause;

void OnEnemyDefeated()
{
    m_scoreOperation.Execute(); // Adds to score and raises event
}

void OnPausePressed()
{
    m_togglePause.Execute(); // Toggles pause state
}
```

Supported operations vary by type:
- **Bool**: Set To, Toggle
- **Number**: Set To, Add, Subtract, Multiply, Divide, Power (with optional rounding for integers)
- **Vector**: Set To, Add, Subtract, Scalar Multiply/Divide

## Extension Methods

Over 150 extension methods to make Unity development faster and cleaner:

```csharp
using Buck;

// Collections
myList.Shuffle(); // Fisher-Yates shuffle
var random = myList.Random(); // Get random element

// Vectors
float distance = ExtensionMethods.ManhattanDistance(posA, posB);
Vector2Int gridPos = worldPosition.ToVector2Int();

// Colors
Color tinted = myColor.Tint(0.2f);
spriteRenderer.SetAlpha(0.5f);

// Geometry
bool visible = myRenderer.IsVisibleFrom(mainCamera);
Rect screenRect = myTransform.GetScreenRectangle();

// Math
float smooth = ExtensionMethods.Smootherstep(0, 1, t);
float angle = rotation.Angle360Positive(); // -10° becomes 350°

// Arrays
int[,] rotated = my2DArray.Rotate90();
my2DArray.Transpose();
```

## Object Pooling

High-performance pooling with automatic overflow handling:

> [!TIP]
> **What is Object Pooling?** Instead of constantly creating and destroying objects (which causes memory allocation and garbage collection), object pooling pre-creates objects and reuses them. This dramatically improves performance for frequently spawned objects like bullets, particles, or enemies.

```csharp
public class BulletSpawner : MonoBehaviour
{
    ObjectPooler m_pooler;
    
    [SerializeField] PooledObject m_bulletPool = new PooledObject
    {
        m_prefab = bulletPrefab,
        m_numberOfObjects = 50
    };
    
    void Start()
    {
        m_pooler = GetComponent<ObjectPooler>();
        m_pooler.GenerateObjects(m_bulletPool);
    }
    
    void FireBullet()
    {
        GameObject bullet = m_pooler.Retrieve(firePoint.position, firePoint.rotation);
        // Bullet is automatically activated and positioned
    }
}

// In the bullet script
void OnCollisionEnter(Collision collision)
{
    GetComponent<PoolerIdentifier>().m_pooler.Recycle(gameObject);
}
```

### PoolerIdentifier
Every pooled object automatically receives a `PoolerIdentifier` component that tracks its originating pool. This allows objects to be recycled from anywhere without needing direct references:

```csharp
// Any script can recycle a pooled object
void OnTriggerEnter(Collider other)
{
    var identifier = other.GetComponent<PoolerIdentifier>();
    if (identifier != null)
        identifier.m_pooler.Recycle(other.gameObject);
}
```

### Overflow Behaviors
When the pool runs out of objects:
- **Recycle Oldest**: Reuse the oldest active object (default)
- **Double Size**: Expand the pool (may cause GC spikes)
- **Warn**: Log a warning and return null

## Additional Utilities

### Singleton Pattern
```csharp
public class GameManager : Singleton<GameManager>
{
    // Automatically creates a persistent instance
    public void RestartLevel() { }
}

// Access from anywhere
GameManager.Instance.RestartLevel();
```

### Soft Singleton
For singletons that should only exist for the duration of a currently loaded scene:
```csharp
public class EnemyManager : SoftSingleton<EnemyManager>
{
    // Can be destroyed and recreated
}
```

### Runtime Sets
Automatically track collections of objects:
```csharp
[CreateAssetMenu(menuName = "BUCK/Runtime Sets/Enemy Set")]
public class EnemyRuntimeSet : RuntimeSet<Enemy> { }

// Enemies register themselves
void OnEnable() => m_enemySet.Add(this);
void OnDisable() => m_enemySet.Remove(this);

// Query active enemies from anywhere
int enemyCount = m_enemySet.Items.Count;
```

### BaseScriptableObject & GUIDs
All BUCK Basics ScriptableObjects inherit from `BaseScriptableObject`, which provides persistent GUIDs (globally unique identifiers) for each asset:

```csharp
// Find specific ScriptableObjects by their GUID
var myVariable = BaseScriptableObject.FindByGuid<IntVariable>(guid, "Assets/Variables");

// GUIDs persist across renames and moves
bool isSameAsset = variableA.Guid == variableB.Guid;

// Useful for save systems or detecting duplicates
List<Guid> collectedItems = new List<Guid>();
if (!collectedItems.Contains(item.Guid))
    collectedItems.Add(item.Guid);
```

This GUID system ensures your references remain stable even when assets are renamed or reorganized, making it invaluable for save systems, inventory management, and asset tracking.


# Contributing

Found a bug or have a feature request? We'd love to hear from you!

- [Open an issue](https://github.com/buck-co/buck-basics/issues) for problems or suggestions
- [Create a pull request](https://github.com/buck-co/buck-basics/pulls) if you'd like to contribute code
- Check our [contribution guidelines](CONTRIBUTING.md) before submitting

# Authors

* **Nick Pettit** - [nickpettit](https://github.com/nickpettit)
* **Ian Sundstrom** - [iwsundstrom](https://github.com/iwsundstrom)

See the full list of [contributors](https://github.com/buck-co/buck-basics/contributors).

# Acknowledgments

* [Ryan Hipple's Unite Austin 2017 talk](https://www.youtube.com/watch?v=raQ3iHhE_Kk) for the foundational architecture patterns

# License

MIT License - Copyright (c) 2025 BUCK Design LLC [buck-co](https://github.com/buck-co)

---

_[BUCK](https://buck.co) is a global creative company that brings brands, stories, and experiences to life through art, design, and technology. If you're a Game Developer or Creative Technologist or want to get involved with our work, reach out and say hi via [Github](https://github.com/buck-co), [Instagram](https://www.instagram.com/buck_design/?hl=en) or our [Careers page](https://buck.co/careers). 👋_
