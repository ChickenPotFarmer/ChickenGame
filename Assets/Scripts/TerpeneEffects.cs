using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerpeneEffects
{
    [Header("Effect Lists")]
    public static string[] caryophylleneEffects = new string[] { "Mood Boosting", "Anti-Inflammatory", "Pain Relief"};
    public static string[] limoneneEffects = new string[] { "Stress Relief", "Mood Boosting", "Sleep Aid"};
    public static string[] linaloolEffects = new string[] { "Calming", "Stress Relief", "Anti-Depressant", "Pain Relief"};
    public static string[] myrceneEffects = new string[] { "Mood Boosting", "THC Amplifying", "Anti-Depressant"};
    public static string[] pineneEffects = new string[] { "Memory Boosting", "Creativity Boosting", "Anti-Depressant", "Anti-Inflammatory"};
    public static string[] terpinoleneEffects = new string[] { "Calming", "Sleep Aid", "Pain Relief"};

    public static string GetRandomEffect(int _terpeneInt)
    {
        string effect;
        int rand;
        switch (_terpeneInt)
        {
            case 0:
                rand = Random.Range(0, caryophylleneEffects.Length);
                effect = caryophylleneEffects[rand];
                break;

            case 1:
                rand = Random.Range(0, limoneneEffects.Length);
                effect = limoneneEffects[rand];
                break;

            case 2:
                rand = Random.Range(0, linaloolEffects.Length);
                effect = linaloolEffects[rand];
                break;

            case 3:
                rand = Random.Range(0, myrceneEffects.Length);
                effect = myrceneEffects[rand];
                break;

            case 4:
                rand = Random.Range(0, pineneEffects.Length);
                effect = pineneEffects[rand];
                break;

            case 5:
                rand = Random.Range(0, terpinoleneEffects.Length);
                effect = terpinoleneEffects[rand];
                break;

            default:
                effect = "TERPENE EFFECT ERROR";
                break;
        }

        return effect;
    }
}
