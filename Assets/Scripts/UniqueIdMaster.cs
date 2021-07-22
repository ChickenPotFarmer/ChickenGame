using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueIdMaster : MonoBehaviour
{
    [Header("Strains")]
    public Dictionary<string, string> breedingDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> nameDictionary = new Dictionary<string, string>();
    public List<string> strainIds;
    public List<StrainProfile> masterStrainProfileList;

    [Header("Setup")]
    [SerializeField] private NewStrainPanel newStrainPanel;


    public static UniqueIdMaster instance;
    [HideInInspector]
    public GameObject uniqueIdMaster;

    private void Awake()
    {
        instance = this;
        uniqueIdMaster = gameObject;
    }

    public void GetID(string _femaleId, string _maleId, StrainProfile _strain)
    {
        string newStrainID;
        string parentIds = _femaleId + _maleId;

        if (breedingDictionary.ContainsKey(parentIds))
        {
            newStrainID = breedingDictionary[parentIds];
            _strain.SetUniqueID(newStrainID);

            print("Dictionary checked " + parentIds + ". Returned " + newStrainID);

        }
        else
        {
            newStrainID = GetNewID();
            _strain.SetUniqueID(newStrainID);
            breedingDictionary.Add(parentIds, newStrainID);
            nameDictionary.Add(newStrainID, "New " + _strain.GetStrainType() + " Strain");
            print("Dictionary checked " + parentIds + ". Created a new ID:  " + newStrainID);
            newStrainPanel.OpenNewStrainPanel(_strain);

        }
        
        //return newStrainID;
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

    public string GetName(string _id)
    {
        if (nameDictionary.ContainsKey(_id))
            return nameDictionary[_id];
        else
            return "";
    }

    public void SetNewName(string _id, string _name)
    {
        for (int i = 0; i < masterStrainProfileList.Count; i++)
        {
            //if (masterStrainProfileList[i] == null)
            //{
            //    masterStrainProfileList.RemoveAt(i);
            //    i = -1;
            //}
            //else
            //{
            if (masterStrainProfileList[i].strainID.Equals(_id))
            {
                masterStrainProfileList[i].SetStrainName(_name);

            }
            //}
        }

        nameDictionary[_id] = _name;

    }

    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(10, 10, 100, 50), "Rename strain"))
    //        SetNewName("42042069", "IT WORKED!");

    //    if (GUI.Button(new Rect(110, 10, 100, 50), "Rename strain 2"))
    //        SetNewName("42042069", "IT WORKED TWICE!");
    //}
}
