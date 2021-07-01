using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomEmail : MonoBehaviour
{
    [Header("Email")]
    public string generatedEmail;

    [Header("Nouns")]
    public string[] nouns;
    public string[] domains;

    private void Start()
    {
        IntializeLists();
    }

    private void Update()
    {
        if (Input.GetKeyDown("9"))
            GenerateRandomEmail();
    }

    public void IntializeLists()
    {
        TextAsset txt = (TextAsset)Resources.Load("Nouns");

        if (txt)
            nouns = txt.text.Split("\n"[0]);
        else
            print("failed to find nouns");

        txt = (TextAsset)Resources.Load("Domains");

        if (txt)
            domains = txt.text.Split("\n"[0]);
        else
            print("failed to find nouns");
    }

    public void GenerateRandomEmail()
    {
        string newEmail = "";
        string rand;
        string temp;

        // generate either 1 or 2 nouns
        if (Random.value >= 0.5f)
        {
            rand = nouns[Random.Range(0, nouns.Length)];

            // Chance to captilize
            if (Random.value >= 0.5f)
            {
                temp = char.ToUpper(rand[0]) + rand.Substring(1);
                newEmail += temp;
            }
            else
                newEmail += rand;

            rand = nouns[Random.Range(0, nouns.Length)];

            // Chance to captilize
            if (Random.value >= 0.5f)
            {
                temp = char.ToUpper(rand[0]) + rand.Substring(1);
                newEmail += temp;
            }
            else
                newEmail += rand;
        }
        else
        {
            newEmail += nouns[Random.Range(0, nouns.Length)];
        }

        // add random number
        if (Random.value >= 0.5f)
        {
            int randNum = Random.Range(0, 10000);
            newEmail += randNum.ToString();
        }

        // add @ 
        newEmail += "@";


        // generate noun for domain
        rand = nouns[Random.Range(0, nouns.Length)];

        if (Random.value >= 0.5f)
        {
            temp = char.ToUpper(rand[0]) + rand.Substring(1);
            newEmail += temp;
        }
        else
            newEmail += rand;

        // pick random .com
        rand = domains[Random.Range(0, domains.Length)];
        newEmail += rand;


        generatedEmail = newEmail;

    }
}
