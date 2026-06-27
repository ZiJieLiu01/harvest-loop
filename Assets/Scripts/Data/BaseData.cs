using UnityEngine;

public class BaseData : ScriptableObject
{
    [Header("Base Data")]
    public string id;           // UNIQUE key for database
    public string displayName;  // UI name
}
