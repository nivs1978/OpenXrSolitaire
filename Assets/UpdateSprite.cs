using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    /*
    public Texture2D cardFace;
//    public TextureRenderer textureRenderer;
//    private Selectable selectable;
    private Solitaire solitaire;

    public Texture2D cardBack;
    */
    // Start is called before the first frame update
    void Start()
    {
        /*
        print("Card face: " + this.name);
        List<string> deck = Solitaire.GenerateDeck();
        solitaire = FindObjectOfType<Solitaire>();
        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                print("Card face found");
                cardFace = solitaire.cardFaces[i];
                break;
            }
            i++;
        }
        if (cardFace!=null)
        {
            print("Updating texture");
            var renderer = GetComponent<Renderer>();
            var newMaterial = new Material(renderer.material);
            newMaterial.mainTexture = cardFace;
            renderer.material = newMaterial;
        }
        */
//        selectable = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
