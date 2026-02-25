using Unity.VisualScripting;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Material change;
    public TriggerCustomEvent triggerEvent;

    private void OnTriggerEnter(Collider other) {
        
        other.GetComponent<test>().changeColor();     
    }

}
