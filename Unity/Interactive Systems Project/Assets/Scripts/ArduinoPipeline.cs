using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArduinoPipeline : MonoBehaviour
{
    [SerializeField] private GameObject servoShaft;
    [SerializeField] private GameObject button;

    public bool isPressed;

    [SerializeField] private Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPressed()
    {
        servoShaft.transform.Rotate(0,45,0);
    }
    
    public void OnUpdateSelected(BaseEventData data)
    {
        if (isPressed)
        {
            ButtonPressed();
        }
    }
    public void OnPointerDown(PointerEventData data)
    {
        isPressed = true;
    }
    public void OnPointerUp(PointerEventData data)
    {
        isPressed = false;
    }
}
