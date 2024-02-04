using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Solitaire : MonoBehaviour
{
    public Texture2D[] cardFaces;
    public GameObject cardPrefab;
    public GameObject deckPrefab;
    public List<GameObject> cardSlots;

    Text hudText = null;

    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    public Stack<string> deck;
    List<string> deckSorted = GenerateDeck();

    float heightStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        heightStep = (deckPrefab.transform.GetComponent<Renderer>().bounds.size.y*deckPrefab.transform.localScale.y)/52.0f;
        hudText = GameObject.Find("HudText").GetComponent<Text>();
        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayCards()
    {
        deck = ShuffleAndConvertToStack(GenerateDeck());
        InstantiateCardOnTopOfDeck();
    }

    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string s in suits)
        {
            foreach (string v in values)
            {
                newDeck.Add(s + v);
            }
        }
        return newDeck;
    }

    private Stack<string> ShuffleAndConvertToStack(List<string> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k=random.Next(n);
            n--;
            string temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
        Stack<string> stack = new Stack<string>();
        foreach (string card in list)
        {
            stack.Push(card);
        }
        return stack;
    }

    int GetCardIndex(string card)
    {
        int i = 0;
        foreach (string cardName in deckSorted)
        {
            if (card == cardName)
            {
                return i;
            }
            i++;
        }
        return -1;
    }

    public bool HasMoreCardsInDeck()
    {
        return deck.Count > 0;
    }

    public void InstantiateCardOnTopOfDeck()
    {
        if (deck.Count > 0)
        {
            hudText.text = "Cards left: " + deck.Count;
            string card = deck.Pop();
//            var localScale = deckPrefab.transform.localScale;
            var pos = deckPrefab.transform.position;
//            localScale.y -= 1.5f/52;
            pos.y -= 0.00028f;
            deckPrefab.transform.position = pos;
//            deckPrefab.transform.localScale = localScale;
            GameObject newCard = Instantiate(cardPrefab, new Vector3(deckPrefab.transform.position.x, deckPrefab.transform.position.y + 0.01f, deckPrefab.transform.position.z), Quaternion.identity);
            newCard.name = card;
            newCard.GetComponent<Card>().freshFromDeck = true;
            newCard.GetComponent<Rigidbody>().useGravity = true;
            newCard.transform.rotation = Quaternion.Euler(180, 0, 0);
            var renderer = newCard.GetComponentInChildren<Renderer>();
            for (int i=0; i<renderer.materials.Length; i++)
            {
                var material = renderer.materials[i];
                if (material.name.StartsWith("CardFace"))
                {
                    int idx = GetCardIndex(newCard.name);
                    material.SetTexture("_BaseMap", cardFaces[idx]);
                    break;
                }
            }
            if (deck.Count == 0)
            {
                hudText.text = "No more cards in deck";
                deckPrefab.SetActive(false);
            }
        }
    }
}
