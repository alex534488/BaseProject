using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

[System.Serializable]
public enum ConditionType
{
    AlwaysTrue = 0,
    AlwaysFalse = 1,
    StaticBoolFunctionCall = 2
}
[System.Serializable]
public class Condition
{
    ConditionType type;
    string[] parameters = null;
    public Condition(ConditionType type, params string[] parameters)
    {
        this.type = type;
        this.parameters = parameters;
    }

    static public Condition BoolFunctionCall(string className, string functionName, string parameterOne)
    {
        return new Condition(ConditionType.StaticBoolFunctionCall, className, functionName, parameterOne);
    }
    static public Condition BoolFunctionCall(string className, string functionName, string parameterOne, string parameterTwo)
    {
        return new Condition(ConditionType.StaticBoolFunctionCall, className, functionName, parameterOne, parameterTwo);
    }
    static public Condition BoolFunctionCall(string className, string functionName, string parameterOne, string parameterTwo, string parameterThree)
    {
        return new Condition(ConditionType.StaticBoolFunctionCall, className, functionName, parameterOne, parameterTwo, parameterThree);
    }

    public bool Execute()
    {
        switch (type)
        {
            default:
            case ConditionType.AlwaysFalse:
                return false;
            case ConditionType.AlwaysTrue:
                return true;
            case ConditionType.StaticBoolFunctionCall:
                object[] objParameters = new object[parameters.Length-2];
                for (int i = 2; i < parameters.Length; i++)
                    objParameters[i - 2] = parameters[i];
                MethodInfo method = Type.GetType(parameters[0]).GetMethod(parameters[1]);
                if (method.ReturnType != typeof(bool))
                    throw new System.Exception("Return type of " + parameters[0] + ": " + parameters[1] + " is not bool.");
                return (bool)method.Invoke(null, objParameters);

        }
    }
}
