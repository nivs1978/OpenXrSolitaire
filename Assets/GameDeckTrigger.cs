using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDeckTrigger : MonoBehaviour
{
    private Solitaire solitaire;
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
    }

    void OnTriggerEnter(Collider other)
    {
//        print("Touching deck: " + other.name);
    }

    void OnTriggerStay(Collider other)
    {
//        print("Inside deck: " + other.name);
    }

    void OnTriggerExit(Collider other)
    {
        print("Card removed from deck: " + other.name);
        other.gameObject.GetComponent<Card>().leftDeck = true;
    }
}
