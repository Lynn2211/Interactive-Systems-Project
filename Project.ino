#include <Servo.h>

//Set the pins that are used
#define BUTTON_PIN 12
#define SERVO_PIN 0

//True for output as hex
//False for output as ascii
bool hexOutput = true;

//Length of one dot, the minimal signal
int dotLength = 500;
//Start and end of a button press
long startTime, endTime;
//The list of dots and dashes of a letter
String inputList = "";

bool lightOn = false;

Servo SERVO;

void setup() {
  //Set the serial monitor to 9600 to enable output
  Serial.begin(9600);
  
  //Attach the servo pin
  SERVO.attach(SERVO_PIN, 544, 2500);

  //Set the button to pullup
  pinMode(BUTTON_PIN, INPUT_PULLUP);
  pinMode(LED_BUILTIN, OUTPUT);
}

void loop() {
  //Wait for a button press and determine the length
  while(digitalRead(BUTTON_PIN) == HIGH){
    takeInputFromUnity();
  }
  startTime = millis();
  while(digitalRead(BUTTON_PIN) == LOW){yield();}
  endTime = millis();

  long duration = endTime - startTime;

  //Determine whether a dot or dash was sent
  char morseChar;
  if(duration <= dotLength){
    morseChar = '.';
    }
    else{
      morseChar = '-';
    }

  inputList += morseChar;

  //Wait for 3 dots if the user sends another signal 
  //If the user does not send another signal the inputList will be mapped to a letter
  while((millis() - endTime) < (dotLength * 3)){
    if(digitalRead(BUTTON_PIN) == LOW){
      //Go back to the start of the loop and add another dot or dash
      return;
      }
    }

  //Determine which combination the user pressed
  //Letters are stored as decimal: A = 65, B = 66, ..., Z = 90
  int decRepresentation = morseToDecimal(inputList);
  char letter = morseToChar(inputList);

  inputList = "";
  
  if(decRepresentation == -1){
    return;
    }
    
  if(hexOutput){
    //Convert the letter to hexadecimal and output it to the serial monitor
    String outputAsHex = String(decRepresentation, HEX);
    Serial.println(outputAsHex);
    }else{
      //Print the ascii letter
      Serial.println(letter);
      }
  

  //Move the servo to the corresponding position of the letter
  SERVO.write(map(decRepresentation, 65, 90, 0, 180));
  
  //Wait for 7 dots if the user sends another signal 
  //If the user does not send another signal a space will be added
  while((millis() - endTime) < (dotLength * 7)){
    yield();
    if(digitalRead(BUTTON_PIN) == LOW){
      //Go back to the start of the loop to add another letter
      return;
      }
    }
    
  if(hexOutput){
    //Print space as hexadecimal
    Serial.println("20");
    }else{
      //Print a space
      Serial.println(" ");
      }
  
}


void takeInputFromUnity(){
  if(Serial.available() > 0){
      if(Serial.read() == '1'){
        if(lightOn){
          digitalWrite(LED_BUILTIN, LOW);
          lightOn = false;
        }
        else{
         digitalWrite(LED_BUILTIN, HIGH);
         lightOn = true;
        }
      }
      if(Serial.read() == '2'){
        //add another function
      }
    }
}

//Converts a morse input to a decimal of the corresponding letter
//If the input doesn't match to a letter -1 is returned for error detection
int morseToDecimal(String input){
  switch(input.length()){
    case 1: 
      if(input == "."){return 69;}
      if(input == "-"){return 84;}
      break;
    case 2:
      if(input == ".-"){return 65;}
      if(input == ".."){return 73;}
      if(input == "--"){return 77;}
      if(input == "-."){return 78;}
      break;
    case 3:
      if(input == "-.."){return 68;}
      if(input == "--."){return 71;}
      if(input == "-.-"){return 75;}
      if(input == "---"){return 79;}
      if(input == "..."){return 83;}
      if(input == "..-"){return 85;}
      if(input == ".--"){return 87;}
      break;
    case 4:
      if(input == "-..."){return 66;}
      if(input == "-.-."){return 67;}
      if(input == "..-."){return 70;}
      if(input == "...."){return 72;}
      if(input == ".---"){return 74;}
      if(input == ".-.."){return 76;}
      if(input == ".--."){return 80;}
      if(input == "--.-"){return 81;}
      if(input == ".-."){return 82;}
      if(input == "...-"){return 86;}
      if(input == "-..-"){return 88;}
      if(input == "-.--"){return 89;}
      if(input == "--.."){return 90;}
      break;
    default:
      return -1;
      break;
  }
}

//Converts a morse input to a decimal of the corresponding letter
//If the input doesn't match to a letter e is returned for error detection
char morseToChar(String input){
  switch(input.length()){
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
     break;
  }
}
