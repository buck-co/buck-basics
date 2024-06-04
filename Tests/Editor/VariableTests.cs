using Buck;
using NUnit.Framework;
using UnityEngine;

public class VariableTests
{
    [Test]
    public void Vector2Variable()
    {
        var vector2Variable = ScriptableObject.CreateInstance<Vector2Variable>();
        Assert.IsTrue(vector2Variable.Value == Vector2.zero);
        vector2Variable.Value = new Vector2(1, 2);
        Assert.IsTrue(vector2Variable.Value == new Vector2(1, 2));
        Assert.IsTrue(vector2Variable.VectorLength == 2);
        Assert.IsTrue(vector2Variable.IsAVectorInt == false);
        Assert.IsTrue(vector2Variable.ValueVector2 == new Vector2(1, 2));
        Assert.IsTrue(vector2Variable.ValueVector3 == new Vector3(1, 2, 0));
        Assert.IsTrue(vector2Variable.ValueVector4 == new Vector4(1, 2, 0, 0));
        Assert.IsTrue(vector2Variable.ValueVector2Int == new Vector2Int(1, 2));
        Assert.IsTrue(vector2Variable.ValueVector3Int == new Vector3Int(1, 2, 0));
    }
    
    [Test]
    public void Vector3Variable()
    {
        var vector3Variable = ScriptableObject.CreateInstance<Vector3Variable>();
        Assert.IsTrue(vector3Variable.Value == Vector3.zero);
        vector3Variable.Value = new Vector3(1, 2, 3);
        Assert.IsTrue(vector3Variable.Value == new Vector3(1, 2, 3));
        Assert.IsTrue(vector3Variable.VectorLength == 3);
        Assert.IsTrue(vector3Variable.IsAVectorInt == false);
        Assert.IsTrue(vector3Variable.ValueVector2 == new Vector2(1, 2));
        Assert.IsTrue(vector3Variable.ValueVector3 == new Vector3(1, 2, 3));
        Assert.IsTrue(vector3Variable.ValueVector4 == new Vector4(1, 2, 3, 0));
        Assert.IsTrue(vector3Variable.ValueVector2Int == new Vector2Int(1, 2));
        Assert.IsTrue(vector3Variable.ValueVector3Int == new Vector3Int(1, 2, 3));
    }
    
    [Test]
    public void Vector4Variable()
    {
        var vector4Variable = ScriptableObject.CreateInstance<Vector4Variable>();
        Assert.IsTrue(vector4Variable.Value == Vector4.zero);
        vector4Variable.Value = new Vector4(1, 2, 3, 4);
        Assert.IsTrue(vector4Variable.Value == new Vector4(1, 2, 3, 4));
        Assert.IsTrue(vector4Variable.VectorLength == 4);
        Assert.IsTrue(vector4Variable.IsAVectorInt == false);
        Assert.IsTrue(vector4Variable.ValueVector2 == new Vector2(1, 2));
        Assert.IsTrue(vector4Variable.ValueVector3 == new Vector3(1, 2, 3));
        Assert.IsTrue(vector4Variable.ValueVector4 == new Vector4(1, 2, 3, 4));
        Assert.IsTrue(vector4Variable.ValueVector2Int == new Vector2Int(1, 2));
        Assert.IsTrue(vector4Variable.ValueVector3Int == new Vector3Int(1, 2, 3));
    }
    
    [Test]
    public void Vector2IntVariable()
    {
        var vector2IntVariable = ScriptableObject.CreateInstance<Vector2IntVariable>();
        Assert.IsTrue(vector2IntVariable.Value == Vector2Int.zero);
        vector2IntVariable.Value = new Vector2Int(1, 2);
        Assert.IsTrue(vector2IntVariable.Value == new Vector2Int(1, 2));
        Assert.IsTrue(vector2IntVariable.VectorLength == 2);
        Assert.IsTrue(vector2IntVariable.IsAVectorInt == true);
        Assert.IsTrue(vector2IntVariable.ValueVector2 == new Vector2(1, 2));
        Assert.IsTrue(vector2IntVariable.ValueVector3 == new Vector3(1, 2, 0));
        Assert.IsTrue(vector2IntVariable.ValueVector4 == new Vector4(1, 2, 0, 0));
        Assert.IsTrue(vector2IntVariable.ValueVector2Int == new Vector2Int(1, 2));
        Assert.IsTrue(vector2IntVariable.ValueVector3Int == new Vector3Int(1, 2, 0));
    }
    
    [Test]
    public void Vector3IntVariable()
    {
        var vector3IntVariable = ScriptableObject.CreateInstance<Vector3IntVariable>();
        Assert.IsTrue(vector3IntVariable.Value == Vector3Int.zero);
        vector3IntVariable.Value = new Vector3Int(1, 2, 3);
        Assert.IsTrue(vector3IntVariable.Value == new Vector3Int(1, 2, 3));
        Assert.IsTrue(vector3IntVariable.VectorLength == 3);
        Assert.IsTrue(vector3IntVariable.IsAVectorInt == true);
        Assert.IsTrue(vector3IntVariable.ValueVector2 == new Vector2(1, 2));
        Assert.IsTrue(vector3IntVariable.ValueVector3 == new Vector3(1, 2, 3));
        Assert.IsTrue(vector3IntVariable.ValueVector4 == new Vector4(1, 2, 3, 0));
        Assert.IsTrue(vector3IntVariable.ValueVector2Int == new Vector2Int(1, 2));
        Assert.IsTrue(vector3IntVariable.ValueVector3Int == new Vector3Int(1, 2, 3));
    }
    
    [Test]
    public void IntVariable()
    {
        var intVariable = ScriptableObject.CreateInstance<IntVariable>();
        Assert.IsTrue(intVariable.Value == 0);
        intVariable.Value = 1;
        Assert.IsTrue(intVariable.Value == 1);
        intVariable.Value = -1;
        Assert.IsTrue(intVariable.Value == -1);
        Assert.IsTrue(intVariable.ValueInt == -1);
        Assert.IsTrue(intVariable.ValueFloat == -1f);
        Assert.IsTrue(intVariable.ValueDouble == -1d);
        intVariable.Value = int.MaxValue;
        Assert.IsTrue(intVariable.Value == int.MaxValue);
        intVariable.Value = int.MinValue;
        Assert.IsTrue(intVariable.Value == int.MinValue);
    }
    
    [Test]
    public void FloatVariable()
    {
        var floatVariable = ScriptableObject.CreateInstance<FloatVariable>();
        Assert.IsTrue(floatVariable.Value == 0f);
        floatVariable.Value = 1f;
        Assert.IsTrue(floatVariable.Value == 1f);
        floatVariable.Value = -1f;
        Assert.IsTrue(floatVariable.Value == -1f);
        Assert.IsTrue(floatVariable.ValueInt == -1);
        Assert.IsTrue(floatVariable.ValueFloat == -1f);
        Assert.IsTrue(floatVariable.ValueDouble == -1d);
        floatVariable.Value = float.MaxValue;
        Assert.IsTrue(floatVariable.Value == float.MaxValue);
        floatVariable.Value = float.MinValue;
        Assert.IsTrue(floatVariable.Value == float.MinValue);
    }
    
    [Test]
    public void DoubleVariable()
    {
        var doubleVariable = ScriptableObject.CreateInstance<DoubleVariable>();
        Assert.IsTrue(doubleVariable.Value == 0d);
        doubleVariable.Value = 1d;
        Assert.IsTrue(doubleVariable.Value == 1d);
        doubleVariable.Value = -1d;
        Assert.IsTrue(doubleVariable.Value == -1d);
        Assert.IsTrue(doubleVariable.ValueInt == -1);
        Assert.IsTrue(doubleVariable.ValueFloat == -1f);
        Assert.IsTrue(doubleVariable.ValueDouble == -1d);
        doubleVariable.Value = double.MaxValue;
        Assert.IsTrue(doubleVariable.Value == double.MaxValue);
        doubleVariable.Value = double.MinValue;
        Assert.IsTrue(doubleVariable.Value == double.MinValue);
    }
    
    [Test]
    public void BoolVariable()
    {
        var boolVariable = ScriptableObject.CreateInstance<BoolVariable>();
        Assert.IsTrue(boolVariable.Value == false);
        boolVariable.Value = true;
        Assert.IsTrue(boolVariable.Value == true);
    }
    
    [Test]
    public void StringVariable()
    {
        var stringVariable = ScriptableObject.CreateInstance<StringVariable>();
        Assert.IsTrue(stringVariable.Value == null);
        stringVariable.Value = "Hello, World!";
        Assert.IsTrue(stringVariable.Value == "Hello, World!");
    }
    
    [Test]
    public void ColorVariable()
    {
        var colorVariable = ScriptableObject.CreateInstance<ColorVariable>();
        Assert.IsTrue(colorVariable.Value == Color.clear);
        colorVariable.Value = Color.red;
        Assert.IsTrue(colorVariable.Value == Color.red);
    }
    
    [Test]
    public void MaterialVariable()
    {
        var materialVariable = ScriptableObject.CreateInstance<MaterialVariable>();
        Assert.IsTrue(materialVariable.Value == null);
        materialVariable.Value = new Material(Shader.Find("Standard"));
        Assert.IsTrue(materialVariable.Value.shader.name == "Standard");
    }
    
    [Test]
    public void GameObjectVariable()
    {
        var gameObjectVariable = ScriptableObject.CreateInstance<GameObjectVariable>();
        Assert.IsTrue(gameObjectVariable.Value == null);
        gameObjectVariable.Value = new GameObject("Test");
        Assert.IsTrue(gameObjectVariable.Value.name == "Test");
    }
    
    [Test]
    public void QuaternionVariable()
    {
        var quaternionVariable = ScriptableObject.CreateInstance<QuaternionVariable>();
        quaternionVariable.Value = Quaternion.identity;
        Assert.IsTrue(quaternionVariable.Value == Quaternion.identity);
        quaternionVariable.Value = Quaternion.Euler(1f, 2f, 3f);
        Assert.IsTrue(quaternionVariable.Value == Quaternion.Euler(1f, 2f, 3f));
    }
    
    [Test]
    public void SpriteVariable()
    {
        var spriteVariable = ScriptableObject.CreateInstance<SpriteVariable>();
        Assert.IsTrue(spriteVariable.Value == null);
        spriteVariable.Value = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), Vector2.one * 0.5f);
        Assert.IsTrue(spriteVariable.Value != null);
        Assert.IsTrue(spriteVariable.Value.rect == new Rect(0, 0, 1, 1));
    }
    
    [Test]
    public void Texture2DVariable()
    {
        var texture2DVariable = ScriptableObject.CreateInstance<Texture2DVariable>();
        Assert.IsTrue(texture2DVariable.Value == null);
        texture2DVariable.Value = new Texture2D(1, 1);
        Assert.IsTrue(texture2DVariable.Value != null);
        Assert.IsTrue(texture2DVariable.Value.width == 1);
        Assert.IsTrue(texture2DVariable.Value.height == 1);
    }
}
