using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

using System;
using Unity.VisualScripting;

public class Card : MonoBehaviour
{
    private Solitaire solitaire;
    private Rigidbody rb;
    public bool freshFromDeck = false;
    public bool leftDeck = false;
    public bool inHand = false;
    private GameObject gameDeck;

    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        rb = GetComponent<Rigidbody>();
        var table = GameObject.Find("/Table");
        gameDeck = GameObject.Find("/Table/GameDeck");
        transform.SetParent(table.transform);

        // Get the Xr Grab Interactable component from this object
        var grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable>();
        // Get Box Collider component from this object
        var boxCollider = GetComponent<BoxCollider>();
        // Add this object to the list of colliders in the Xr Grab Interactable component
        grabInteractable.colliders.Add(boxCollider);
        // Add a listener to the OnSelectEntered event
        grabInteractable.selectEntered.AddListener(Grabbed);
        // Add a listener to the OnSelectExited event
        grabInteractable.selectExited.AddListener(Released);
    }

    // Update is called once per frame
    void Update()
    {
        var speed = rb.velocity.magnitude;
        if (speed < 0.01)
        {
            rb.velocity = Vector3.zero; // gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            if(leftDeck && freshFromDeck) {
                
                if (freshFromDeck)
                {
                    freshFromDeck = false;
                    StartCoroutine(delayInstantiateNewCard());
                }
            }
        }

        if (transform.position.y < -10)
        {
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(gameDeck.transform.position.x-0.1f, gameDeck.transform.position.y+0.1f, gameDeck.transform.position.z);
        }
    }

    private IEnumerator delayInstantiateNewCard()
    {
        yield return new WaitForSeconds(0.5f);
        solitaire.InstantiateCardOnTopOfDeck();
    }

    void Grabbed(SelectEnterEventArgs args)
    {
        Debug.Log(this.name + " was grabbed");
        //magazine = args.interactableObject.GetComponent<Magazine>();
 
    }
    void Released(SelectExitEventArgs args)
    {
        Debug.Log(this.name + " was released");

        float minDistance = float.MaxValue;
        GameObject target = null;
        // Find closest cardslut with lowest magnitude
        foreach (var cardSlot in solitaire.cardSlots)
        {
            Debug.Log("Testing distance");
            var distanceToCardSlot = (transform.position - cardSlot.transform.position).magnitude;
            if (distanceToCardSlot > 0.1f) // If more than 10 cm away, ignore this card slot
            {
                continue;
            }
            if (distanceToCardSlot < minDistance)
            {
                minDistance = distanceToCardSlot;
                target = cardSlot;
            }
        }

        Rigidbody cardRigidbody = this.GetComponent<Rigidbody>();
        GameObject newParent = null;
        if (target != null)
        {
            if (target.name.StartsWith("Hand"))
            { // Set the hand menu as parent and move the card to the slot in the hand
                Debug.Log("Snap to hand");
                // Disable physics
                cardRigidbody.useGravity = false;
                cardRigidbody.isKinematic = true;
                newParent = GameObject.Find("HandMenu");
                this.inHand = true;
            } else
            { // Set the table as parent and move the card to the slot on the table
                Debug.Log("Snap to card slot on table");
                // Enable physics
                cardRigidbody.useGravity = false;
                cardRigidbody.isKinematic = true;
                newParent = GameObject.Find("/Table");
                this.inHand = false;
            }
            this.transform.SetParent(newParent.transform, false);
            this.transform.position = target.transform.position;
            this.transform.rotation = target.transform.rotation;
        } 
        else
        {
            Debug.Log("Drop");
            newParent = GameObject.Find("/Table");
            this.transform.SetParent(newParent.transform, true);
            cardRigidbody.useGravity = true;
            cardRigidbody.isKinematic = false; 
        }
    }
}
