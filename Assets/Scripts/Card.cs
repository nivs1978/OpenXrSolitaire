using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Solitaire solitaire;
    private Rigidbody rb;
    public bool freshFromDeck = false;
    public bool leftDeck = false;
    // Variable that holds the position where the card was picked up
    public Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var speed = rb.velocity.magnitude;
        if (speed < 0.01)
        {
            rb.velocity = Vector3.zero; // gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            startPosition = transform.position;

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
            transform.position = startPosition;
/*            var deck = GameObject.Find("GameDeck");
            var pos = deck.transform.position;
            transform.position = new Vector3(pos.x, pos.y+0.1f, pos.z);*/
        }
    }

    private IEnumerator delayInstantiateNewCard()
    {
        yield return new WaitForSeconds(0.5f);
        solitaire.InstantiateCardOnTopOfDeck();
    }
}
