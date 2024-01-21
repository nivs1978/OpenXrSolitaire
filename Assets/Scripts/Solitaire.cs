using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solitaire : MonoBehaviour
{
    public Texture2D[] cardFaces;
    public GameObject cardPrefab;
    public GameObject deckPrefab;

    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    public Stack<string> deck;
    List<string> deckSorted = GenerateDeck();

    // Start is called before the first frame update
    void Start()
    {
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
        print("Finding index for card: " + card);
        int i = 0;
        foreach (string cardName in deckSorted)
        {
            if (card == cardName)
            {
                print("Card index found: " + i);
                return i;
            }
            i++;
        }
        print("Card index not found");
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
            string card = deck.Pop();
            print("Next card: " + card);
            GameObject newCard = Instantiate(cardPrefab, new Vector3(deckPrefab.transform.position.x, deckPrefab.transform.position.y + 0.01f, deckPrefab.transform.position.z), Quaternion.identity);
            newCard.name = card;
            print("Card set as fresh from deck");
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
        } else
        {
            print("No more cards in deck");
        }
    }
}
