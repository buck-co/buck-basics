using Buck;
using NUnit.Framework;
using UnityEngine;

public class NumberOperationTests
{
    NumberOperation GetNumberOperation(float numA, float numB, float numC, NumberOperation.Operations operation,
        NumberOperation.RightHandArithmetic rightHandArithmetic, NumberOperation.RoundingType roundingType)
    {
        NumberOperation numberOperation = new NumberOperation();
        FloatVariable numberA = ScriptableObject.CreateInstance<FloatVariable>();
        numberA.Value = numA;
        
        FloatVariable numberB = ScriptableObject.CreateInstance<FloatVariable>();
        numberB.Value = numB;
        NumberReference numberReferenceB = new NumberReference
        {
            UseVariable = true,
            Variable = numberB
        };

        FloatVariable numberC = ScriptableObject.CreateInstance<FloatVariable>();
        numberC.Value = numC;
        NumberReference numberReferenceC = new NumberReference
        {
            UseVariable = true,
            Variable = numberC
        };
        
        numberOperation.SetValues(numberA,
            numberReferenceB,
            numberReferenceC, 
            operation,
            rightHandArithmetic,
            roundingType);
        
        return numberOperation;
    }

    [Test]
    public void NumberOperationSetTo()
    {
        NumberOperation numberOperation = GetNumberOperation(1f, 2f, 3f, NumberOperation.Operations.SetTo, NumberOperation.RightHandArithmetic.None, NumberOperation.RoundingType.RoundToInt);
        Assert.AreNotEqual(numberOperation.NumberA.ValueFloat, numberOperation.NumberB.ValueFloat);
        numberOperation.Execute();
        Assert.AreEqual(numberOperation.NumberA.ValueFloat, numberOperation.NumberB.ValueFloat);
    }
    
    [Test]
    public void NumberOperationAdditionAssignment()
    {
        NumberOperation numberOperation = GetNumberOperation(1f, 2f, 3f, NumberOperation.Operations.AdditionAssignment, NumberOperation.RightHandArithmetic.None, NumberOperation.RoundingType.RoundToInt);
        Assert.AreNotEqual(numberOperation.NumberA.ValueFloat, 3f);
        numberOperation.Execute();
        Assert.AreEqual(numberOperation.NumberA.ValueFloat, 3f);
    }
    
    [Test]
    public void NumberOperationSubtractionAssignment()
    {
        NumberOperation numberOperation = GetNumberOperation(1f, 2f, 3f, NumberOperation.Operations.SubtractionAssignment, NumberOperation.RightHandArithmetic.None, NumberOperation.RoundingType.RoundToInt);
        Assert.AreNotEqual(numberOperation.NumberA.ValueFloat, -1f);
        numberOperation.Execute();
        Assert.AreEqual(numberOperation.NumberA.ValueFloat, -1f);
    }
    
    [Test]
    public void NumberOperationMultiplicationAssignment()
    {
        NumberOperation numberOperation = GetNumberOperation(1f, 2f, 3f, NumberOperation.Operations.MultiplicationAssignment, NumberOperation.RightHandArithmetic.None, NumberOperation.RoundingType.RoundToInt);
        Assert.AreNotEqual(numberOperation.NumberA.ValueFloat, 2f);
        numberOperation.Execute();
        Assert.AreEqual(numberOperation.NumberA.ValueFloat, 2f);
    }
    
    [Test]
    public void NumberOperationDivisionAssignment()
    {
        NumberOperation numberOperation = GetNumberOperation(1f, 2f, 3f, NumberOperation.Operations.DivisionAssignment, NumberOperation.RightHandArithmetic.None, NumberOperation.RoundingType.RoundToInt);
        Assert.AreNotEqual(numberOperation.NumberA.ValueFloat, 0.5f);
        numberOperation.Execute();
        Assert.AreEqual(numberOperation.NumberA.ValueFloat, 0.5f);
    }

    [Test]
    public void NumberOperationDivisionAssignmentByZero()
    {
        NumberOperation numberOperation = GetNumberOperation(1f, 0f, 0f, NumberOperation.Operations.DivisionAssignment, NumberOperation.RightHandArithmetic.None, NumberOperation.RoundingType.RoundToInt);
        Assert.AreNotEqual(numberOperation.NumberA.ValueFloat, Mathf.Infinity);
        numberOperation.Execute();
        Assert.AreEqual(numberOperation.NumberA.ValueFloat, Mathf.Infinity);
    }
    
    [Test]
    public void NumberOperationPowAssignment()
    {
        NumberOperation numberOperation = GetNumberOperation(2f, 3f, 3f, NumberOperation.Operations.PowAssignment, NumberOperation.RightHandArithmetic.None, NumberOperation.RoundingType.RoundToInt);
        Assert.AreNotEqual(numberOperation.NumberA.ValueFloat, 8f);
        numberOperation.Execute();
        Assert.AreEqual(numberOperation.NumberA.ValueFloat, 8f);
    }
    
    [Test]
    public void NumberOperationSetToWithRightHandAddition()
    {
        NumberOperation numberOperation = GetNumberOperation(1f, 2f, 3f, NumberOperation.Operations.SetTo, NumberOperation.RightHandArithmetic.Addition, NumberOperation.RoundingType.RoundToInt);
        Assert.AreNotEqual(numberOperation.NumberA.ValueFloat, 5f);
        numberOperation.Execute();
        Assert.AreEqual(numberOperation.NumberA.ValueFloat, 5f);
    }
    
    [Test]
    public void NumberOperationSetToWithRightHandSubtraction()
    {
        NumberOperation numberOperation = GetNumberOperation(1f, 2f, 3f, NumberOperation.Operations.SetTo, NumberOperation.RightHandArithmetic.Subtraction, NumberOperation.RoundingType.RoundToInt);
        Assert.AreNotEqual(numberOperation.NumberA.ValueFloat, -1f);
        numberOperation.Execute();
        Assert.AreEqual(numberOperation.NumberA.ValueFloat, -1f);
    }
    
    [Test]
    public void NumberOperationSetToWithRightHandMultiplication()
    {
        NumberOperation numberOperation = GetNumberOperation(1f, 2f, 3f, NumberOperation.Operations.SetTo, NumberOperation.RightHandArithmetic.Multiplication, NumberOperation.RoundingType.RoundToInt);
        Assert.AreNotEqual(numberOperation.NumberA.ValueFloat, 6f);
        numberOperation.Execute();
        Assert.AreEqual(numberOperation.NumberA.ValueFloat, 6f);
    }
    
    [Test]
    public void NumberOperationSetToWithRightHandDivision()
    {
        NumberOperation numberOperation = GetNumberOperation(1f, 2f, 3f, NumberOperation.Operations.SetTo, NumberOperation.RightHandArithmetic.Division, NumberOperation.RoundingType.RoundToInt);
        Assert.AreNotEqual(numberOperation.NumberA.ValueFloat, 0.666666687f);
        numberOperation.Execute();
        Assert.AreEqual(numberOperation.NumberA.ValueFloat, 0.666666687f);
    }
    [Test]
    public void NumberOperationSetToWithRightHandDivisionByZero()
    {
        NumberOperation numberOperation = GetNumberOperation(1f, 2f, 0f, NumberOperation.Operations.SetTo, NumberOperation.RightHandArithmetic.Division, NumberOperation.RoundingType.RoundToInt);
        Assert.AreNotEqual(numberOperation.NumberA.ValueFloat, Mathf.Infinity);
        numberOperation.Execute();
        Assert.AreEqual(numberOperation.NumberA.ValueFloat, Mathf.Infinity);
    }
}