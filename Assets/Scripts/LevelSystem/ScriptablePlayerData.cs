using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class ScriptablePlayerData : ScriptableObject
{
    public float health;
    public float stamina;
    public int healthPotion;
}
