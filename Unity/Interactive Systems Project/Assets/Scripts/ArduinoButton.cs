using UnityEngine;
using UnityEngine.UI;

public class ArduinoButton : MonoBehaviour {
     
     private float startTime, endTime, waitCharStart, waitLetterStart;

     [SerializeField] private float dotLength = 500;

     //Visualization texts
     public Text textAscii;
     public Text textTimer;
     public Text timerLeftToClick;
     public Text textHexa;

     //Servo game object
     public GameObject shaft;

     private string inputList = "";
     
     private int decimalRepresentation;
     private char letter;
     private string hexOutput;

     //Locks to skip certain steps in the Update function
     private bool clickStarted;
     private bool clickEnded;
     private bool waitForNewChar = false;
     private bool waitForNewLetter = false;
     private bool signalAdded = false;
     private bool letterCorrect = false;

     // Use this for initialization
     void Start ()
     {
         textAscii.text = "Ascii-Message: ";
         textHexa.text = "Hex-Message: ";
         shaft.transform.rotation = new Quaternion(0,0,0,0);
     }
     
     // Update is called once per frame
     private void Update () {
         //Fire a raycast at the screen
         var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         RaycastHit Hit;
         
         float duration = 0;

         //Detect if the button was pressed
         if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject && !clickStarted)
         {
             //Take the start time and enable lock to skip this step
             startTime = Time.time;
             clickStarted = true;
             clickEnded = false;
         }

         //Update text if the user is not pressing
         if (!clickStarted)
         {
             timerLeftToClick.text = "Waiting for Input";
             return;
         }
         
         //Detect if the button press ended
         if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject && !clickEnded)
         {
             //Take the end time and enable lock to skip this step
             endTime = Time.time;
             clickEnded = true;
         }

         //Detect if the mouse left the button
         if (!(Input.GetMouseButton(0) && Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject) && clickStarted && !clickEnded)
         {
             //Take the end time and enable lock to skip this step
             endTime = Time.time;
             clickEnded = true;
         }

         //Visualization
         textTimer.text = "Length of press: " + (Time.time - startTime);

         //Only pass this line if a press was measured
         if (!clickEnded) return;
         
         duration = endTime - startTime;
         
         //Visualization
         textTimer.text = "Length of press: " + duration;

         //Detect if the signal was a dot or a dash
         if (!signalAdded)
         {
             //Append the char to the input list and lock this step
             inputList += duration <= dotLength * 3 ? '.' : '-';
             signalAdded = true;
         }

         //Wait for 3 dots if the user wants to add another signal
         if (Time.time - endTime < dotLength*3)
         {
             timerLeftToClick.text = "Time for new morse part: " + (dotLength*3 - (Time.time - endTime));
             if (!Input.GetMouseButtonDown(0)) return;
             //If the user presses again reset locks and start a new click
             ResetLocks();
             startTime = Time.time;
             clickStarted = true;
             return;
         }
        
         //Convert the Morse code to decimal and hexadecimal and clear the input list
         if (!letterCorrect)
         {
             decimalRepresentation = MorseToDecimal(inputList);
             letter = MorseToChar(inputList);
             inputList = "";
             hexOutput = decimalRepresentation.ToString("X");
         }
        
         //Check if the Morse code was invalid and reset if true
         if (decimalRepresentation == -1)
         {
             ResetLocks();
             return;
         }

         //Output the hex and letter and rotate the Servo
         if (!letterCorrect)
         {
             textAscii.text +=  letter;
             textHexa.text += hexOutput;
             var mappedValue = Mathf.Lerp(0, 180, Mathf.InverseLerp(65, 90, decimalRepresentation));
             shaft.transform.localEulerAngles = new Vector3(0, 0, mappedValue);
             letterCorrect = true;
         }

         //Wait for 3 dots if the user wants to add another signal
         if (Time.time - endTime < dotLength * 7)
         {
             timerLeftToClick.text = "Time for new letter: " + ((dotLength * 7) - (Time.time - endTime));
             if (!Input.GetMouseButtonDown(0)) return;
             //If the user presses again reset locks and start a new click
             ResetLocks();
             startTime = Time.time;
             clickStarted = true;
             return;
         }
         
         //Reset the locks
         ResetLocks();
         
         //Add a space to hex and ASCII message
         textAscii.text += " ";
         textHexa.text += "20";
         
         //Rotate Servo back to <space>
         shaft.transform.localEulerAngles = new Vector3(0, 0, 0);
     }

     private void ResetLocks()
     {
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

     private static int MorseToDecimal(string input)
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