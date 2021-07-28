using System;
using UnityEngine;

[CreateAssetMenu]
public class StringVariable : ScriptableObject, ISerializationCallbackReceiver
{
    public string InitialValue;

    [NonSerialized]
    public string RuntimeValue;

    public void OnAfterDeserialize()
    {
        RuntimeValue = InitialValue;
    }

    public void OnBeforeSerialize() { }
}