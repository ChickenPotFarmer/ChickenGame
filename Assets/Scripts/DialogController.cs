using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [Header("Settings")]
    public float secsBetweenLetters;
    public float endOfSentencePause;
    public float openDelay;
    public Text dialogTxt;
    public Image whoIsSpeakingImage;
    public AudioSource dialogAudio;
    public Animator dialogAnimator;

    [Header("NPCs")]
    public Sprite dax;


    [Header("String")]
    public string builtMessage;
    public char[] chars;

    [Header("Testing")]
    public string testString;


    private void Start()
    {
        dialogTxt.text = null;
        //Testing
        //DisplayMessage(testString);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown("1"))
    //        DisplayMessage(testString);
    //}

    public void DisplayMessage(Sprite _whoIsSpeaking, string _message)
    {
        StartCoroutine(DisplayMessageRoutine(_whoIsSpeaking, _message));
    }

    public void DisplayMessage(string _message)
    {
        StartCoroutine(DisplayMessageRoutine(_message));
    }

    private IEnumerator DisplayMessageRoutine(Sprite _whoIsSpeaking, string _message)
    {
        //whoIsSpeakingImage.color = new Color(0, 0, 0, 0);
        builtMessage = null;

        BuildCharArray(_message);
        dialogAnimator.Play("Enter");
        whoIsSpeakingImage.sprite = _whoIsSpeaking;

        //StartCoroutine(FadeInSprite()); // This doesn't actually work, revisit later.
        //whoIsSpeakingImage.color = new Color(255, 255, 255, 255);

        yield return new WaitForSeconds(openDelay);
        
        dialogAudio.Play();

        for (int i = 0; i < chars.Length; i++)
        {
            builtMessage += chars[i];
            if(dialogTxt)
                dialogTxt.text = builtMessage;
            yield return new WaitForSeconds(secsBetweenLetters);

            if (chars[i] == '.' || chars[i] == '?' || chars[i] == '!' || chars[i] == ',')
            {
                dialogAudio.Pause();
                yield return new WaitForSeconds(endOfSentencePause);
                dialogAudio.Play();
            }

        }
        dialogAudio.Pause();
    }

    private IEnumerator DisplayMessageRoutine(string _message)
    {
        builtMessage = null;

        BuildCharArray(_message);
        dialogAnimator.Play("Enter");

        yield return new WaitForSeconds(openDelay);

        dialogAudio.Play();

        for (int i = 0; i < chars.Length; i++)
        {
            builtMessage += chars[i];
            if (dialogTxt)
                dialogTxt.text = builtMessage;
            yield return new WaitForSeconds(secsBetweenLetters);

            if (chars[i] == '.' || chars[i] == '?' || chars[i] == '!')
            {
                dialogAudio.Pause();
                yield return new WaitForSeconds(endOfSentencePause);
                dialogAudio.Play();
            }

        }
        dialogAudio.Pause();
    }

    public IEnumerator FadeInSprite()
    {
        int num = 0;

        do
        {
            num += 2;
            whoIsSpeakingImage.color = new Color(num, num, num, num);
            yield return new WaitForSeconds(0.1f);
        } while (num < 255);
    }

    public void CloseDialog()
    {
        dialogTxt.text = null;
        whoIsSpeakingImage.color = new Color(0, 0, 0, 0);
        dialogAnimator.Play("Exit");
    }

    public void BuildCharArray(string _message)
    {
        chars = _message.ToCharArray();
    }

}
