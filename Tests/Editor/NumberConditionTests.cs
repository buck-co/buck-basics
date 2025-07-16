// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using Buck;
using NUnit.Framework;
using UnityEngine;

public class NumberConditionTests
{
    Condition GetNumberCondition(float numberA, float numberB, Condition.BooleanComparisons operation)
    {
        FloatVariable numberVariableA = ScriptableObject.CreateInstance<FloatVariable>();
        numberVariableA.Value = numberA;
        NumberReference numberReferenceA = new NumberReference
        {
            UseVariable = true,
            Variable = numberVariableA
        };
        
        FloatVariable numberVariableB = ScriptableObject.CreateInstance<FloatVariable>();
        numberVariableB.Value = numberB;
        NumberReference numberReferenceB = new NumberReference
        {
            UseVariable = true,
            Variable = numberVariableB
        };
        
        Condition numberCondition = new Condition();
        numberCondition.SetValues(numberReferenceA, numberReferenceB, operation);
        
        return numberCondition;
    }
    
    [Test]
    public void NumberConditionEqualTo()
    {
        Condition numberCondition = GetNumberCondition(1f, 1f, Condition.BooleanComparisons.EqualTo);
        Assert.IsTrue(numberCondition.PassCondition);
        
        Condition numberCondition2 = GetNumberCondition(1f, 2f, Condition.BooleanComparisons.EqualTo);
        Assert.IsFalse(numberCondition2.PassCondition);
        
        Condition numberCondition3 = GetNumberCondition(2f, 2f, Condition.BooleanComparisons.EqualTo);
        Assert.IsTrue(numberCondition3.PassCondition);
    }
    
    [Test]
    public void NumberConditionNotEqualTo()
    {
        Condition numberCondition = GetNumberCondition(1f, 1f, Condition.BooleanComparisons.NotEqualTo);
        Assert.IsFalse(numberCondition.PassCondition);
        
        Condition numberCondition2 = GetNumberCondition(1f, 2f, Condition.BooleanComparisons.NotEqualTo);
        Assert.IsTrue(numberCondition2.PassCondition);
        
        Condition numberCondition3 = GetNumberCondition(2f, 2f, Condition.BooleanComparisons.NotEqualTo);
        Assert.IsFalse(numberCondition3.PassCondition);
    }
    
    [Test]
    public void NumberConditionLessThan()
    {
        Condition numberCondition = GetNumberCondition(1f, 1f, Condition.BooleanComparisons.LessThan);
        Assert.IsFalse(numberCondition.PassCondition);
        
        Condition numberCondition2 = GetNumberCondition(1f, 2f, Condition.BooleanComparisons.LessThan);
        Assert.IsTrue(numberCondition2.PassCondition);
        
        Condition numberCondition3 = GetNumberCondition(2f, 2f, Condition.BooleanComparisons.LessThan);
        Assert.IsFalse(numberCondition3.PassCondition);
        
        Condition numberCondition4 = GetNumberCondition(2f, 1f, Condition.BooleanComparisons.LessThan);
        Assert.IsFalse(numberCondition4.PassCondition);
    }
    
    [Test]
    public void NumberConditionLessThanOrEqualTo()
    {
        Condition numberCondition = GetNumberCondition(1f, 1f, Condition.BooleanComparisons.LessThanOrEqualTo);
        Assert.IsTrue(numberCondition.PassCondition);
        
        Condition numberCondition2 = GetNumberCondition(1f, 2f, Condition.BooleanComparisons.LessThanOrEqualTo);
        Assert.IsTrue(numberCondition2.PassCondition);
        
        Condition numberCondition3 = GetNumberCondition(2f, 2f, Condition.BooleanComparisons.LessThanOrEqualTo);
        Assert.IsTrue(numberCondition3.PassCondition);
        
        Condition numberCondition4 = GetNumberCondition(2f, 1f, Condition.BooleanComparisons.LessThanOrEqualTo);
        Assert.IsFalse(numberCondition4.PassCondition);
    }
    
    [Test]
    public void NumberConditionGreaterThan()
    {
        Condition numberCondition = GetNumberCondition(1f, 1f, Condition.BooleanComparisons.GreaterThan);
        Assert.IsFalse(numberCondition.PassCondition);
        
        Condition numberCondition2 = GetNumberCondition(1f, 2f, Condition.BooleanComparisons.GreaterThan);
        Assert.IsFalse(numberCondition2.PassCondition);
        
        Condition numberCondition3 = GetNumberCondition(2f, 2f, Condition.BooleanComparisons.GreaterThan);
        Assert.IsFalse(numberCondition3.PassCondition);
        
        Condition numberCondition4 = GetNumberCondition(2f, 1f, Condition.BooleanComparisons.GreaterThan);
        Assert.IsTrue(numberCondition4.PassCondition);
    }
    
    [Test]
    public void NumberConditionGreaterThanOrEqualTo()
    {
        Condition numberCondition = GetNumberCondition(1f, 1f, Condition.BooleanComparisons.GreaterThanOrEqualTo);
        Assert.IsTrue(numberCondition.PassCondition);
        
        Condition numberCondition2 = GetNumberCondition(1f, 2f, Condition.BooleanComparisons.GreaterThanOrEqualTo);
        Assert.IsFalse(numberCondition2.PassCondition);
        
        Condition numberCondition3 = GetNumberCondition(2f, 2f, Condition.BooleanComparisons.GreaterThanOrEqualTo);
        Assert.IsTrue(numberCondition3.PassCondition);
        
        Condition numberCondition4 = GetNumberCondition(2f, 1f, Condition.BooleanComparisons.GreaterThanOrEqualTo);
        Assert.IsTrue(numberCondition4.PassCondition);
    }
}
