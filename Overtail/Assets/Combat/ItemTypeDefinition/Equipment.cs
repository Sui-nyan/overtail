using UnityEngine;

[CreateAssetMenu(fileName = "equipment_generic", menuName = "Create Equipment/generic")]
public class Equipment : ScriptableObject
{
    private new string name;
    public string Name;
    [TextArea]
    public string Description;
    public Stats Stats;
}


