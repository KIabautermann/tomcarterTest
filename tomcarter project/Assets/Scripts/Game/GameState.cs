using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState
{
   public UnlockedAbilitiesData unlockedAbilities;
   public GameState()
   {
       unlockedAbilities = new UnlockedAbilitiesData();
   }
}
