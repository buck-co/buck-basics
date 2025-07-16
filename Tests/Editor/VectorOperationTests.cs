// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using Buck;
using NUnit.Framework;
using UnityEngine;

public class VectorOperationTests
{
    VectorOperation GetVectorOperation(Vector2 vectorA, Vector2 vectorB, Vector2 vectorC, int scalar, VectorOperation.Operations operation,
        VectorOperation.RightHandArithmetic rightHandArithmetic)
    {
        VectorOperation vectorOperation = new VectorOperation();
        Vector2Variable vectorVariableA = ScriptableObject.CreateInstance<Vector2Variable>();
        vectorVariableA.Value = vectorA;
        
        Vector2Variable vectorVariableB = ScriptableObject.CreateInstance<Vector2Variable>();
        vectorVariableB.Value = vectorB;
        VectorReference vectorReferenceB = new VectorReference
        {
            UseVariable = true,
            Variable = vectorVariableB
        };
        
        Vector2Variable vectorVariableC = ScriptableObject.CreateInstance<Vector2Variable>();
        vectorVariableC.Value = vectorC;
        VectorReference vectorReferenceC = new VectorReference
        {
            UseVariable = true,
            Variable = vectorVariableC
        };
        
        IntVariable scalarVariable = ScriptableObject.CreateInstance<IntVariable>();
        scalarVariable.Value = scalar;
        NumberReference scalarReference = new NumberReference
        {
            UseVariable = true,
            Variable = scalarVariable
        };
        
        vectorOperation.SetValues(vectorVariableA,
            vectorReferenceB,
            vectorReferenceC,
            scalarReference,
            operation,
            rightHandArithmetic);
        
        return vectorOperation;
    }
    
    [Test]
    public void VectorOperationSetTo()
    {
        VectorOperation vectorOperation = GetVectorOperation(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6), 2, VectorOperation.Operations.SetTo, VectorOperation.RightHandArithmetic.None);
        Assert.AreNotEqual(vectorOperation.VectorA.ValueVector2, vectorOperation.VectorB.ValueVector2);
        vectorOperation.Execute();
        Assert.AreEqual(vectorOperation.VectorA.ValueVector2, vectorOperation.VectorB.ValueVector2);
    }
    
    [Test]
    public void VectorOperationAdditionAssignment()
    {
        VectorOperation vectorOperation = GetVectorOperation(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6), 2, VectorOperation.Operations.AdditionAssignment, VectorOperation.RightHandArithmetic.None);
        Assert.AreNotEqual(vectorOperation.VectorA.ValueVector2, new Vector2(4, 6));
        vectorOperation.Execute();
        Assert.AreEqual(vectorOperation.VectorA.ValueVector2, new Vector2(4, 6));
    }
    
    [Test]
    public void VectorOperationSubtractionAssignment()
    {
        VectorOperation vectorOperation = GetVectorOperation(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6), 2, VectorOperation.Operations.SubtractionAssignment, VectorOperation.RightHandArithmetic.None);
        Assert.AreNotEqual(vectorOperation.VectorA.ValueVector2, new Vector2(-2, -2));
        vectorOperation.Execute();
        Assert.AreEqual(vectorOperation.VectorA.ValueVector2, new Vector2(-2, -2));
    }
    
    [Test]
    public void VectorOperationAdditionAssignmentWithRightHandArithmetic()
    {
        VectorOperation vectorOperation = GetVectorOperation(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6), 2, VectorOperation.Operations.AdditionAssignment, VectorOperation.RightHandArithmetic.Addition);
        Assert.AreNotEqual(vectorOperation.VectorA.ValueVector2, new Vector2(9, 12));
        vectorOperation.Execute();
        Assert.AreEqual(vectorOperation.VectorA.ValueVector2, new Vector2(9, 12));
    }
    
    [Test]
    public void VectorOperationSubtractionAssignmentWithRightHandArithmetic()
    {
        VectorOperation vectorOperation = GetVectorOperation(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6), 2, VectorOperation.Operations.SubtractionAssignment, VectorOperation.RightHandArithmetic.Subtraction);
        Assert.AreNotEqual(vectorOperation.VectorA.ValueVector2, new Vector2(3, 4));
        vectorOperation.Execute();
        Assert.AreEqual(vectorOperation.VectorA.ValueVector2, new Vector2(3, 4));
    }
    
    [Test]
    public void VectorOperationSetToWithRightHandAddition()
    {
        VectorOperation vectorOperation = GetVectorOperation(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6), 2, VectorOperation.Operations.SetTo, VectorOperation.RightHandArithmetic.Addition);
        Assert.AreNotEqual(vectorOperation.VectorA.ValueVector2, new Vector2(8, 10));
        vectorOperation.Execute();
        Assert.AreEqual(vectorOperation.VectorA.ValueVector2, new Vector2(8, 10));
    }
    
    [Test]
    public void VectorOperationSetToWithRightHandSubtraction()
    {
        VectorOperation vectorOperation = GetVectorOperation(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6), 2, VectorOperation.Operations.SetTo, VectorOperation.RightHandArithmetic.Subtraction);
        Assert.AreNotEqual(vectorOperation.VectorA.ValueVector2, new Vector2(-2, -2));
        vectorOperation.Execute();
        Assert.AreEqual(vectorOperation.VectorA.ValueVector2, new Vector2(-2, -2));
    }
    
    [Test]
    public void VectorOperationSetToWithRightHandScalarMultiplication()
    {
        VectorOperation vectorOperation = GetVectorOperation(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6), 2, VectorOperation.Operations.SetTo, VectorOperation.RightHandArithmetic.ScalarMultiplication);
        Assert.AreNotEqual(vectorOperation.VectorA.ValueVector2, new Vector2(6, 8));
        vectorOperation.Execute();
        Assert.AreEqual(vectorOperation.VectorA.ValueVector2, new Vector2(6, 8));
    }
    
    [Test]
    public void VectorOperationSetToWithRightHandScalarDivision()
    {
        VectorOperation vectorOperation = GetVectorOperation(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6), 2, VectorOperation.Operations.SetTo, VectorOperation.RightHandArithmetic.ScalarDivision);
        Assert.AreNotEqual(vectorOperation.VectorA.ValueVector2, new Vector2(3/2f, 4/2f));
        vectorOperation.Execute();
        Assert.AreEqual(vectorOperation.VectorA.ValueVector2, new Vector2(3/2f, 4/2f));
    }
    
    [Test]
    public void VectorOperationSetToWithRightHandScalarDivisionByZero()
    {
        VectorOperation vectorOperation = GetVectorOperation(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6), 0, VectorOperation.Operations.SetTo, VectorOperation.RightHandArithmetic.ScalarDivision);
        Assert.AreNotEqual(vectorOperation.VectorA.ValueVector2, new Vector2(Mathf.Infinity, Mathf.Infinity));
        vectorOperation.Execute();
        Assert.AreEqual(vectorOperation.VectorA.ValueVector2, new Vector2(Mathf.Infinity, Mathf.Infinity));
    }
}