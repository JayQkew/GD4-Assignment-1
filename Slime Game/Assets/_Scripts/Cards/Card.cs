using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Card
{
    public string cardName;
    public Modifier[] modifiers;
    public UnityEvent<GameObject> onTriggerAbility;
    public string description;
    
}
