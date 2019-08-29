using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterInfo : ScriptableObject
{
    public string characterName;
    public Sprite characterSprite;

	public string firstAbilityName;
	public string firstAbilityDescription;
	public Sprite firstAbilitySprite;

	public string secondAbilityName;
	public string secondAbilityDescription;
	public Sprite secondAbilitySprite;


	public Sprite characterIcon;
    public float zoom = 1;

}
