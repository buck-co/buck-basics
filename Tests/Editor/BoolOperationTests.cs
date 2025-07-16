// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using Buck;
using NUnit.Framework;
using UnityEngine;

public class BoolOperationTests
{
    BoolOperation GetBoolOperation(bool boolA, bool boolB, BoolOperation.Operations operation)
    {
        BoolOperation boolOperation = new BoolOperation();
        BoolVariable boolVariableA = ScriptableObject.CreateInstance<BoolVariable>();
        boolVariableA.Value = boolA;
        
        BoolVariable boolVariableB = ScriptableObject.CreateInstance<BoolVariable>();
        boolVariableB.Value = boolB;
        BoolReference boolReferenceB = new BoolReference
        {
            UseVariable = true,
            Variable = boolVariableB
        };
        
        boolOperation.SetValues(boolVariableA, boolReferenceB, operation);
        
        return boolOperation;
    }
    
    [Test]
    public void BoolOperationSetTo()
    {
        BoolOperation boolOperation = GetBoolOperation(true, false, BoolOperation.Operations.SetTo);
        Assert.IsFalse(boolOperation.GetResult());
        boolOperation.Execute();
        Assert.IsFalse(boolOperation.BoolA.Value);
        
        BoolOperation boolOperation2 = GetBoolOperation(false, false, BoolOperation.Operations.SetTo);
        Assert.IsFalse(boolOperation2.GetResult());
        boolOperation2.Execute();
        Assert.IsFalse(boolOperation2.BoolA.Value);
        
        BoolOperation boolOperation3 = GetBoolOperation(false, true, BoolOperation.Operations.SetTo);
        Assert.IsTrue(boolOperation3.GetResult());
        boolOperation3.Execute();
        Assert.IsTrue(boolOperation3.BoolA.Value);
    }
    
    [Test]
    public void BoolOperationToggle()
    {
        BoolOperation boolOperation = GetBoolOperation(true, false, BoolOperation.Operations.Toggle);
        
        Assert.IsFalse(boolOperation.GetResult());
        boolOperation.Execute();
        Assert.IsFalse(boolOperation.BoolA.Value);
        
        Assert.IsTrue(boolOperation.GetResult());
        boolOperation.Execute();
        Assert.IsTrue(boolOperation.BoolA.Value);
    }
}