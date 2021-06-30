 using System;
 using System.Collections;
 using System.Collections.Generic;
 using System.Diagnostics;
 using System.Runtime.InteropServices.WindowsRuntime;
 using UnityEngine;
 using UnityEngine.Events;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;
 using Debug = UnityEngine.Debug;

 public class ArduinoButton : MonoBehaviour {
     
     private float startTime, endTime, waitCharStart, waitLetterStart;

     [SerializeField] private float dotLength = 500;

     public Text text;
     public Text textTimer;
     public Text timerLeftToClick;

     public GameObject shaft;

     private string inputList = "";

     private int decimalRepresentation;
     private char letter;

     private bool clickStarted;
     private bool clickCanceled;
     private bool clickEnded;
     private bool waitForNewChar = false;
     private bool waitForNewLetter = false;
     private bool signalAdded = false;
     private bool letterCorrect = false;

     // Use this for initialization
     void Start ()
     {
         text.text = "";
     }
     
     // Update is called once per frame
     private void Update () {
         var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         RaycastHit Hit;
         float duration = 0;

         if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject && !clickStarted)
         {
             startTime = Time.time;
             clickStarted = true;
             clickCanceled = false;
             clickEnded = false;
         }

         if (!clickStarted)
         {
             timerLeftToClick.text = "Waiting for Input";
             return;
         }
         
         if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject && !clickEnded)
         {
             endTime = Time.time;
             clickEnded = true;
         }

         if (!(Input.GetMouseButton(0) && Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject) && clickStarted && !clickEnded)
         {
             endTime = Time.time;
             clickEnded = true;
         }

         textTimer.text = "Length of press: " + (Time.time - startTime);

         if (!clickEnded) return;
         duration = endTime - startTime;
         textTimer.text = "Length of press: " + duration;

         if (!signalAdded)
         {
             inputList += duration <= dotLength * 3 ? '.' : '-';
             signalAdded = true;
         }
       
         if (!waitForNewChar)
         {
             waitCharStart = Time.time;
             waitForNewChar = true;
         }

         if (Time.time - waitCharStart < dotLength * 3)
         {
             timerLeftToClick.text = "Time for new morse part: " + ((dotLength * 3) - (Time.time - waitCharStart));
             if (!Input.GetMouseButtonDown(0)) return;
             ResetLocks();
             startTime = Time.time;
             clickStarted = true;
             return;
         }

         if (!letterCorrect)
         {
             decimalRepresentation = MorseToDecimal(inputList);
             letter = MorseToChar(inputList);
             inputList = "";
         }

         if (decimalRepresentation == -1)
         {
             ResetLocks();
             return;
         }

         if (!letterCorrect)
         {
             text.text += letter;
             var mappedValue = Mathf.Lerp(0, 180, Mathf.InverseLerp(65, 90, decimalRepresentation));
             shaft.transform.rotation = new Quaternion(0,mappedValue,0,0);
             letterCorrect = true;
         }
         
         if (!waitForNewLetter)
         {
             waitLetterStart = Time.time;
             waitForNewLetter = true;
         }
         
         if (Time.time - waitLetterStart < dotLength * 7)
         {
             timerLeftToClick.text = "Time for new letter: " + ((dotLength * 7) - (Time.time - waitLetterStart));
             if (!Input.GetMouseButtonDown(0)) return;
             ResetLocks();
             startTime = Time.time;
             clickStarted = true;
             return;
         }
         
         ResetLocks();
         text.text += "-";
     }

     private void ResetLocks()
     {
         clickCanceled = false;
         clickEnded = false;
         clickStarted = false;
         waitForNewLetter = false;
         waitForNewChar = false;
         signalAdded = false;
         letterCorrect = false;
     }
     
     private static char MorseToChar(string input)
     {
         switch(input.Length){
             case 1:
                 if(input == "."){return 'E';}
                 if(input == "-"){return 'T';}
                 break;
             case 2:
                 if(input == ".-"){return 'A';}
                 if(input == ".."){return 'I';}
                 if(input == "--"){return 'M';}
                 if(input == "-."){return 'N';}
                 break;
             case 3:
                 if(input == "-.."){return 'D';}
                 if(input == "--."){return 'G';}
                 if(input == "-.-"){return 'K';}
                 if(input == "---"){return 'O';}
                 if(input == ".-."){return 'R';}
                 if(input == "..."){return 'S';}
                 if(input == "..-"){return 'U';}
                 if(input == ".--"){return 'W';}
                 break;
             case 4:
                 if(input == "-..."){return 'B';}
                 if(input == "-.-."){return 'C';}
                 if(input == "..-."){return 'F';}
                 if(input == "...."){return 'H';}
                 if(input == ".---"){return 'J';}
                 if(input == ".-.."){return 'L';}
                 if(input == ".--."){return 'P';}
                 if(input == "--.-"){return 'Q';}
                 if(input == "...-"){return 'V';}
                 if(input == "-..-"){return 'X';}
                 if(input == "-.--"){return 'Y';}
                 if(input == "--.."){return 'Z';}
                 break;
             default:
                 return 'e';
         }
         return 'e';
     }

     private int MorseToDecimal(string input)
     {
         switch(input.Length){
             case 1: 
                 switch (input)
                 {
                     case ".":
                         return 69;
                     case "-":
                         return 84;
                 }
                 break;
             case 2:
                 switch (input)
                 {
                     case ".-":
                         return 65;
                     case "..":
                         return 73;
                     case "--":
                         return 77;
                     case "-.":
                         return 78;
                 }
                 break;
             case 3:
                 switch (input)
                 {
                     case "-..":
                         return 68;
                     case "--.":
                         return 71;
                     case "-.-":
                         return 75;
                     case "---":
                         return 79;
                     case "...":
                         return 83;
                     case "..-":
                         return 85;
                     case ".--":
                         return 87;
                 }
                 break;
             case 4:
                 switch (input)
                 {
                     case "-...":
                         return 66;
                     case "-.-.":
                         return 67;
                     case "..-.":
                         return 70;
                     case "....":
                         return 72;
                     case ".---":
                         return 74;
                     case ".-..":
                         return 76;
                     case ".--.":
                         return 80;
                     case "--.-":
                         return 81;
                     case ".-.":
                         return 82;
                     case "...-":
                         return 86;
                     case "-..-":
                         return 88;
                     case "-.--":
                         return 89;
                     case "--..":
                         return 90;
                 }
                 break;
             default:
                 return -1;
         }
         return -1;
     }
 }