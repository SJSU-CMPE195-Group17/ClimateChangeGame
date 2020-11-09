using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class multiplechoicequestions: MonoBehaviour
{
    public GameObject buttonA, buttonB, buttonC, buttonD;
    public Text QuestionText;
    private string[] allAnswers = new string[8];
    private string[] allQuestions = new string[2];

     void Start()
    {
        InitAllAnswers();
        AddAnswers(0);
    }

    public void GenericQuestionHandler(){
        gameObject.SetActive(false); 
    }

    public void AddAnswers(int startingID)
    {
        buttonA.GetComponentInChildren<Text>().text = allAnswers[startingID];
        buttonB.GetComponentInChildren<Text>().text = allAnswers[startingID+1];
        buttonC.GetComponentInChildren<Text>().text = allAnswers[startingID+2];
        buttonD.GetComponentInChildren<Text>().text = allAnswers[startingID+3];
        QuestionText.text=allQuestions[startingID/4];
    }

    private void InitAllAnswers(){
        allAnswers[0]="Prepare mass cloud-seeding deployments";
        allAnswers[1]="Begin a region-wide stockpile and hope for the best";
        allAnswers[2]="Create international network of supplies to support Austrailia";
        allAnswers[3]="Hope the models are wrong";
        allAnswers[4]="5 answer drought";
        allAnswers[5]="6 answer drought";
        allAnswers[6]="7 answer drought";
        allAnswers[7]="8 answer drought";

        allQuestions[0]="Droughts in Austrailia, Models predict a catastrophic 4-8 year drought in Austraila. What is your response?";
        allQuestions[1]="Questions2 drought";

    }
}