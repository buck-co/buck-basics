// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using Buck;
using NUnit.Framework;
using UnityEngine;

public class VectorConditionTests
{
    Condition GetVectorCondition(Vector4 vectorA, Vector4 vectorB, Condition.BooleanComparisons operation)
    {
        Vector4Variable vectorVariableA = ScriptableObject.CreateInstance<Vector4Variable>();
        vectorVariableA.Value = vectorA;
        VectorReference vectorReferenceA = new VectorReference
        {
            UseVariable = true,
            Variable = vectorVariableA
        };
        
        Vector4Variable vectorVariableB = ScriptableObject.CreateInstance<Vector4Variable>();
        vectorVariableB.Value = vectorB;
        VectorReference vectorReferenceB = new VectorReference
        {
            UseVariable = true,
            Variable = vectorVariableB
        };
        
        Condition vectorCondition = new Condition();
        vectorCondition.SetValues(vectorReferenceA, vectorReferenceB, operation);
        
        return vectorCondition;
    }
    
    [Test]
    public void VectorConditionEqualTo()
    {
        Condition vectorCondition = GetVectorCondition(new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 1f, 1f, 1f), Condition.BooleanComparisons.EqualTo);
        Assert.IsTrue(vectorCondition.PassCondition);
        
        Condition vectorCondition2 = GetVectorCondition(new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 2f, 1f, 1f), Condition.BooleanComparisons.EqualTo);
        Assert.IsFalse(vectorCondition2.PassCondition);
        
        Condition vectorCondition3 = GetVectorCondition(new Vector2(1f, 1f), new Vector2(1f, 2f), Condition.BooleanComparisons.EqualTo);
        Assert.IsFalse(vectorCondition3.PassCondition);
    }
    
    [Test]
    public void VectorConditionNotEqualTo()
    {
        Condition vectorCondition = GetVectorCondition(new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 1f, 1f, 1f), Condition.BooleanComparisons.NotEqualTo);
        Assert.IsFalse(vectorCondition.PassCondition);
        
        Condition vectorCondition2 = GetVectorCondition(new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 2f, 1f, 1f), Condition.BooleanComparisons.NotEqualTo);
        Assert.IsTrue(vectorCondition2.PassCondition);
        
        Condition vectorCondition3 = GetVectorCondition(new Vector2(1f, 1f), new Vector2(1f, 2f), Condition.BooleanComparisons.NotEqualTo);
        Assert.IsTrue(vectorCondition3.PassCondition);
    }

    [Test]
    public void VectorConditionLessThan()
    {
        Condition vectorCondition = GetVectorCondition(new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 1f, 1f, 1f), Condition.BooleanComparisons.LessThan);
        Assert.IsFalse(vectorCondition.PassCondition);
        
        Condition vectorCondition2 = GetVectorCondition(new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 2f, 1f, 1f), Condition.BooleanComparisons.LessThan);
        Assert.IsTrue(vectorCondition2.PassCondition);
        
        Condition vectorCondition3 = GetVectorCondition(new Vector2(1f, 1f), new Vector2(1f, 2f), Condition.BooleanComparisons.LessThan);
        Assert.IsTrue(vectorCondition3.PassCondition);
    }

    [Test]
    public void VectorConditionLessThanOrEqualTo()
    {
        Condition vectorCondition = GetVectorCondition(new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 1f, 1f, 1f), Condition.BooleanComparisons.LessThanOrEqualTo);
        Assert.IsTrue(vectorCondition.PassCondition);
        
        Condition vectorCondition2 = GetVectorCondition(new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 2f, 1f, 1f), Condition.BooleanComparisons.LessThanOrEqualTo);
        Assert.IsTrue(vectorCondition2.PassCondition);
        
        Condition vectorCondition3 = GetVectorCondition(new Vector2(1f, 1f), new Vector2(1f, 2f), Condition.BooleanComparisons.LessThanOrEqualTo);
        Assert.IsTrue(vectorCondition3.PassCondition);
    }

    [Test]
    public void VectorConditionGreaterThan()
    {
        Condition vectorCondition = GetVectorCondition(new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 1f, 1f, 1f), Condition.BooleanComparisons.GreaterThan);
        Assert.IsFalse(vectorCondition.PassCondition);
        
        Condition vectorCondition2 = GetVectorCondition(new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 2f, 1f, 1f), Condition.BooleanComparisons.GreaterThan);
        Assert.IsFalse(vectorCondition2.PassCondition);
        
        Condition vectorCondition3 = GetVectorCondition(new Vector2(1f, 1f), new Vector2(1f, 2f), Condition.BooleanComparisons.GreaterThan);
        Assert.IsFalse(vectorCondition3.PassCondition);
    }

    [Test]
    public void VectorConditionGreaterThanOrEqualTo()
    {
        Condition vectorCondition = GetVectorCondition(new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 1f, 1f, 1f), Condition.BooleanComparisons.GreaterThanOrEqualTo);
        Assert.IsTrue(vectorCondition.PassCondition);
        
        Condition vectorCondition2 = GetVectorCondition(new Vector4(1f, 1f, 1f, 1f), new Vector4(1f, 2f, 1f, 1f), Condition.BooleanComparisons.GreaterThanOrEqualTo);
        Assert.IsFalse(vectorCondition2.PassCondition);
        
        Condition vectorCondition3 = GetVectorCondition(new Vector2(1f, 1f), new Vector2(1f, 2f), Condition.BooleanComparisons.GreaterThanOrEqualTo);
        Assert.IsFalse(vectorCondition3.PassCondition);
    }
}
