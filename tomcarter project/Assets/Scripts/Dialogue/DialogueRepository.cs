using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mover a Scriptable Object...probablemente? Bah, si se inicializa via levantar data serializada no se si se puede
public static class DialogueRepository
{
    public enum NpcNames
    {
        MR_MUSHROOM
    }

    // Esta gilada deberia estar guardada serializada probablemente. Como goma escala un Dialogue System? Great question...
    private const string MrMushroomLine1 = "QUE HACES VOS ACÁ?";
    private const string MrMushroomLine2 = "Ah, the silent type. I see. Well no worries, I've mastered the art of handling one sided conversations";
    private const string MrMushroomLine3 = "Well, see you around. It's been delightful...ehm, conversing with you.";

    private static IEnumerable<string> mrMushroomLines = new List<string>() { MrMushroomLine1, MrMushroomLine2, MrMushroomLine3 };
    private static Dictionary<NpcNames, IEnumerable<string>> npcLines = new Dictionary<NpcNames, IEnumerable<string>>() 
    {
        { NpcNames.MR_MUSHROOM, mrMushroomLines }
    };

    public static IEnumerable<string> GetDialogueLines(NpcNames npc) { return npcLines[npc]; }
}
