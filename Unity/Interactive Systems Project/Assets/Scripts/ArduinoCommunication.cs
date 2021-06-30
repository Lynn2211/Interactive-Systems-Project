using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoCommunication : MonoBehaviour
{
    private SerialPort arduinoPort = new SerialPort("\\\\.\\COM3", 9600);
    public string input;

    // Start is called before the first frame update
    private void Start()
    {
        arduinoPort.ReadTimeout = 1;
        arduinoPort.Open();
    }

    // Update is called once per frame
    private void Update()
    {
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
        print("port closed");
        arduinoPort.Close();
    }

    public void LightUpLED()
    {
        arduinoPort.Write("1");
    }
}
