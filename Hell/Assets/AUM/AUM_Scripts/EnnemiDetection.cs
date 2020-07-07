using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiDetection : MonoBehaviour
{
    public List<EnnemiController> ennemiControllers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ennemi")
            ennemiControllers.Add(collision.GetComponent<EnnemiController>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ennemi")
            ennemiControllers.Remove(collision.GetComponent<EnnemiController>());
    }
}
