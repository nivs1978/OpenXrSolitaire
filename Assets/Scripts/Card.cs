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
    private GameObject snapTo = null;
    private GameObject rotateTo = null;
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
        if (snapTo != null)
        {
            var destinationPosition = snapTo.transform.position - transform.position;
            transform.position += destinationPosition * Time.deltaTime * 2f;
            if ((transform.position - snapTo.transform.position).magnitude < 0.001f) // Stop when close enough
            {
                snapTo = null;
            }
        }
        if (rotateTo != null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateTo.transform.rotation, Time.deltaTime * 2f);
            if ((transform.rotation * Quaternion.Inverse(rotateTo.transform.rotation)).eulerAngles.magnitude < 0.1f) // Stop when close enough
            {
                rotateTo = null;
            }
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
            if (distanceToCardSlot < minDistance)
            {
                minDistance = distanceToCardSlot;
                target = cardSlot;
            }
        }


        rotateTo = snapTo = target;
    }
}
