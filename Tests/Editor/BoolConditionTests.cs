using Buck;
using NUnit.Framework;
using UnityEngine;

public class BoolConditionTests
{
    Condition GetBoolCondition(bool boolA, bool boolB, Condition.BooleanComparisons operation)
    {
        BoolVariable boolVariableA = ScriptableObject.CreateInstance<BoolVariable>();
        boolVariableA.Value = boolA;
        BoolReference boolReferenceA = new BoolReference
        {
            UseVariable = true,
            Variable = boolVariableA
        };
        
        BoolVariable boolVariableB = ScriptableObject.CreateInstance<BoolVariable>();
        boolVariableB.Value = boolB;
        BoolReference boolReferenceB = new BoolReference
        {
            UseVariable = true,
            Variable = boolVariableB
        };
        
        Condition boolCondition = new Condition();
        boolCondition.SetValues(boolReferenceA, boolReferenceB, operation);
        
        return boolCondition;
    }
    
    [Test]
    public void BoolConditionEqualTo()
    {
        Condition boolCondition = GetBoolCondition(true, true, Condition.BooleanComparisons.EqualTo);
        Assert.IsTrue(boolCondition.PassCondition);
        
        Condition boolCondition2 = GetBoolCondition(true, false, Condition.BooleanComparisons.EqualTo);
        Assert.IsFalse(boolCondition2.PassCondition);
        
        Condition boolCondition3 = GetBoolCondition(false, false, Condition.BooleanComparisons.EqualTo);
        Assert.IsTrue(boolCondition3.PassCondition);
    }
    
    [Test]
    public void BoolConditionNotEqualTo()
    {
        Condition boolCondition = GetBoolCondition(true, true, Condition.BooleanComparisons.NotEqualTo);
        Assert.IsFalse(boolCondition.PassCondition);
        
        Condition boolCondition2 = GetBoolCondition(true, false, Condition.BooleanComparisons.NotEqualTo);
        Assert.IsTrue(boolCondition2.PassCondition);
        
        Condition boolCondition3 = GetBoolCondition(false, false, Condition.BooleanComparisons.NotEqualTo);
        Assert.IsFalse(boolCondition3.PassCondition);
    }
    
    [Test]
    public void BoolConditionLessThan()
    {
        Condition boolCondition = GetBoolCondition(true, true, Condition.BooleanComparisons.LessThan);
        Assert.IsFalse(boolCondition.PassCondition);
        
        Condition boolCondition2 = GetBoolCondition(true, false, Condition.BooleanComparisons.LessThan);
        Assert.IsFalse(boolCondition2.PassCondition);
        
        Condition boolCondition3 = GetBoolCondition(false, false, Condition.BooleanComparisons.LessThan);
        Assert.IsFalse(boolCondition3.PassCondition);
        
        Condition boolCondition4 = GetBoolCondition(false, true, Condition.BooleanComparisons.LessThan);
        Assert.IsTrue(boolCondition4.PassCondition);
    }
    
    [Test]
    public void BoolConditionLessThanOrEqualTo()
    {
        Condition boolCondition = GetBoolCondition(true, true, Condition.BooleanComparisons.LessThanOrEqualTo);
        Assert.IsTrue(boolCondition.PassCondition);
        
        Condition boolCondition2 = GetBoolCondition(true, false, Condition.BooleanComparisons.LessThanOrEqualTo);
        Assert.IsTrue(boolCondition2.PassCondition);
        
        Condition boolCondition3 = GetBoolCondition(false, false, Condition.BooleanComparisons.LessThanOrEqualTo);
        Assert.IsTrue(boolCondition3.PassCondition);
        
        Condition boolCondition4 = GetBoolCondition(false, true, Condition.BooleanComparisons.LessThanOrEqualTo);
        Assert.IsTrue(boolCondition4.PassCondition);
    }
    
    [Test]
    public void BoolConditionGreaterThan()
    {
        Condition boolCondition = GetBoolCondition(true, true, Condition.BooleanComparisons.GreaterThan);
        Assert.IsFalse(boolCondition.PassCondition);
        
        Condition boolCondition2 = GetBoolCondition(true, false, Condition.BooleanComparisons.GreaterThan);
        Assert.IsTrue(boolCondition2.PassCondition);
        
        Condition boolCondition3 = GetBoolCondition(false, false, Condition.BooleanComparisons.GreaterThan);
        Assert.IsFalse(boolCondition3.PassCondition);
        
        Condition boolCondition4 = GetBoolCondition(false, true, Condition.BooleanComparisons.GreaterThan);
        Assert.IsFalse(boolCondition4.PassCondition);
    }
    
    [Test]
    public void BoolConditionGreaterThanOrEqualTo()
    {
        Condition boolCondition = GetBoolCondition(true, true, Condition.BooleanComparisons.GreaterThanOrEqualTo);
        Assert.IsTrue(boolCondition.PassCondition);
        
        Condition boolCondition2 = GetBoolCondition(true, false, Condition.BooleanComparisons.GreaterThanOrEqualTo);
        Assert.IsTrue(boolCondition2.PassCondition);
        
        Condition boolCondition3 = GetBoolCondition(false, false, Condition.BooleanComparisons.GreaterThanOrEqualTo);
        Assert.IsTrue(boolCondition3.PassCondition);
        
        Condition boolCondition4 = GetBoolCondition(false, true, Condition.BooleanComparisons.GreaterThanOrEqualTo);
        Assert.IsTrue(boolCondition4.PassCondition);
    }
}