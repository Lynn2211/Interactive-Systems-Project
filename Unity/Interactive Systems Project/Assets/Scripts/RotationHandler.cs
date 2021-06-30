using UnityEngine;
using UnityEngine.EventSystems;

public class RotationHandler : MonoBehaviour,IUpdateSelectedHandler,IPointerDownHandler,IPointerUpHandler
 {
     public bool isPressed;
     public GameObject shaft;
 
     // Start is called before the first frame update
     public void OnUpdateSelected(BaseEventData data)
     {
         if (isPressed)
         {
          RotateShaft();
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

     private void RotateShaft()
     {
         shaft.transform.Rotate(0,45,0);
     }
 }