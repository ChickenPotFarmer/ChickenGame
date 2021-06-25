using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueIdMaster : MonoBehaviour
{
    [Header("Strains")]
    public Dictionary<string, string> breedingDictionary = new Dictionary<string, string>();
    public List<string> strainIds;


    public static UniqueIdMaster instance;
    [HideInInspector]
    public GameObject uniqueIdMaster;

    private void Awake()
    {
        instance = this;
        uniqueIdMaster = gameObject;
    }

    public string GetID(string _femaleId, string _maleId)
    {
        string newStrainID;
        string parentIds = _femaleId + _maleId;

        if (breedingDictionary.ContainsKey(parentIds))
        {
            newStrainID = breedingDictionary[parentIds];
            print("Dictionary checked " + parentIds + ". Returned " + newStrainID);

        }
        else
        {
            newStrainID = GetNewID();
            breedingDictionary.Add(parentIds, newStrainID);
            print("Dictionary checked " + parentIds + ". Created a new ID:  " + newStrainID);

        }

        return newStrainID;
    }

    private string GetNewID()
    {
        int _id;
        bool idGood = true;
        string parsed = "derp";

        do
        {
            _id = Random.Range(1, 100000000);
            parsed = _id.ToString("00000000");

            for (int i = 0; i < strainIds.Count; i++)
            {
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
