using System.Collections;
using System.Collections.Generic;
using InGame.CardSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Create Card", order = 0)]
public class CardSO : ScriptableObject
{
    public CardData Data;
}
