using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueIdMaster : MonoBehaviour
{
    [Header("Strains")]
    public List<string> strainIds;


    public static UniqueIdMaster instance;
    [HideInInspector]
    public GameObject uniqueIdMaster;

    private void Awake()
    {
        instance = this;
        uniqueIdMaster = gameObject;
    }

    public string GetID()
    {
        int _id;
        bool idGood = true;
        string parsed = "derp";
        _id = Random.Range(1, 100000000);
        parsed = _id.ToString("00000000");


        do
        {
            _id = Random.Range(1, 100000000);

            for (int i = 0; i < strainIds.Count; i++)
            {
                parsed = _id.ToString("00000000");

                if (parsed.Equals(strainIds[i]))
                {
                    idGood = false;
                    break;
                }

            }
        } while (!idGood);

        if (idGood)
            strainIds.Add(parsed);


        return parsed;
    }
}
