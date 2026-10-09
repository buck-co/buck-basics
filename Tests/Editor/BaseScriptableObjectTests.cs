// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using Buck;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class BaseScriptableObjectTests
{
    [Test]
    public void OnValidateSkipsInstancesThatAreNotAssets()
    {
        // An instance made with CreateInstance has no asset path or GUID. Applying a SerializedObject
        // change makes Unity call OnValidate on it, which must not log an exception.
        BoolVariable boolVariable = ScriptableObject.CreateInstance<BoolVariable>();
        SerializedObject serializedObject = new SerializedObject(boolVariable);
        serializedObject.FindProperty("m_debugChanges").boolValue = true;
        Assert.IsTrue(serializedObject.ApplyModifiedPropertiesWithoutUndo());

        LogAssert.NoUnexpectedReceived();
    }
}
