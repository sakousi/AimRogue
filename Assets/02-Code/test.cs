
using UnityEngine;
public class test : MonoBehaviour
{
    public Material change;
    private void Awake()
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Invoke("changeColor", 5f);

    }

    // Update is called once per frame
    void Update()
    {
        change = GetComponent<MeshRenderer>().material;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            change.color = Random.ColorHSV();
        }
    }

    public void changeColor()
    {
        change.color = Random.ColorHSV();
    }
}
