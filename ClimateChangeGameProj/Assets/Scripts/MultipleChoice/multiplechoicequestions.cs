using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class multiplechoicequestions: MonoBehaviour
{
    struct Answer
    {
        public string AnswerString;
        public int moneyVal;
        public int scienceVal;
        public int globalcoopVal;
        public int educationVal;
        public float allTempsDelta;
    }
    public MainGameBackend myMainGameBackend;
    public GameObject buttonA, buttonB, buttonC, buttonD;
    public Text QuestionText;
    private Answer[] allAnswers = new Answer[8];
    private string[] allQuestions = new string[2];

     void Start()
    {
        InitAllAnswers();
        AddAnswers(0);     
    }

    public void GenericQuestionHandler(){
        GameObject mybutton = EventSystem.current.currentSelectedGameObject;
        int buttonId = mybutton.GetComponent<buttonId>().myButtonId;
        Answer buttonAnswer = allAnswers[buttonId];
        myMainGameBackend.changeResourcesAndStatistics(buttonAnswer.moneyVal,buttonAnswer.scienceVal, buttonAnswer.globalcoopVal, buttonAnswer.educationVal);
        print("buttonid" + mybutton.GetComponent<buttonId>().myButtonId);
        gameObject.SetActive(false);
    }

    public void AddAnswers(int startingID)
    {
        buttonA.GetComponentInChildren<Text>().text = allAnswers[startingID].AnswerString;
        buttonA.GetComponent<buttonId>().myButtonId = startingID;
        buttonB.GetComponentInChildren<Text>().text = allAnswers[startingID+1].AnswerString;
        buttonB.GetComponent<buttonId>().myButtonId = startingID+1;
        buttonC.GetComponentInChildren<Text>().text = allAnswers[startingID+2].AnswerString;
        buttonC.GetComponent<buttonId>().myButtonId = startingID + 2;
        buttonD.GetComponentInChildren<Text>().text = allAnswers[startingID+3].AnswerString;
        buttonD.GetComponent<buttonId>().myButtonId = startingID + 3;
        QuestionText.text=allQuestions[startingID/4];
    }

    private void InitAllAnswers(){
        allAnswers[0]= CreateAnswer("Prepare mass cloud-seeding deployments (-$300M ) ", -300,0,0,0,2) ;
        allAnswers[1]=CreateAnswer("Begin a region-wide stockpile and hope for the best (-50 )",0,-50,0,0,1.5f);
        allAnswers[2]=CreateAnswer("Create international network of supplies to support Austrailia (-200 )", 0,0,-200,0,1);
        allAnswers[3]= CreateAnswer("Hope the models are wrong",0,0,0,0,0);
        allAnswers[4]= CreateAnswer("1", 0, 0, 0, 0, 0); 
        allAnswers[5]= CreateAnswer("2", 0, 0, 0, 0, 0); 
        allAnswers[6]= CreateAnswer("3", 0, 0, 0, 0, 0); 
        allAnswers[7]= CreateAnswer("4", 0, 0, 0, 0, 0); 

        allQuestions[0]="Droughts in Austrailia, Models predict a catastrophic 4-8 year drought in Austraila. What is your response?";
        allQuestions[1]="Questions2 drought";

    }

    private Answer CreateAnswer(string myAnswerString,  int myMoneyVal, int myScienceVal, int myGlobalcoopVal,int myEducationVal,float myAllTempsDelta)
    {
        Answer myAnswer = new Answer();
        myAnswer.AnswerString = myAnswerString;
        myAnswer.moneyVal = myMoneyVal;
        myAnswer.scienceVal = myScienceVal;
        myAnswer.globalcoopVal = myGlobalcoopVal;
        myAnswer.educationVal = myEducationVal;
        myAnswer.allTempsDelta = myAllTempsDelta;
        return myAnswer;
    }
}