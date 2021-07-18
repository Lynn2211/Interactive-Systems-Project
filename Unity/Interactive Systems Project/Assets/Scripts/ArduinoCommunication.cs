using System;
using UnityEngine;
using System.IO.Ports;

public class ArduinoCommunication : MonoBehaviour
{
    //Define our port as COM3 with baudRate 9600
    private SerialPort arduinoPort = new SerialPort("\\\\.\\COM3", 9600);

    // Start is called before the first frame update
    private void Start()
    {
        arduinoPort.ReadTimeout = 1;
        arduinoPort.Open();
    }

    // Update is called once per frame
    private void Update()
    {
        //We try to read a line from the port and print it to the console
        try
        {
            print(arduinoPort.ReadLine());
        }
        catch (TimeoutException e)
        {
            //Nothing
        }
    }

    private void OnDisable()
    {
        //Close the port after closing
        print("port closed");
        arduinoPort.Close();
    }

    public void LightUpLED()
    {
        arduinoPort.Write("1");
    }
}
