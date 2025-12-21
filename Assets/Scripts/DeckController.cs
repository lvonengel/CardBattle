using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeckController : MonoBehaviour {
    public static DeckController instance {get; private set;}
    
    public List<CardSO> deckToUse = new List<CardSO>();
    private List<CardSO> activeCards = new List<CardSO>();
    public Card cardToSpawn;
    private int drawCardCost = 2;
    private float waitBetweenDrawingCards = .25f;


    private void Awake() {
        instance = this;
    }

    private void Start() {
        SetupDeck();
    }

    private void Update() {
        // if (Keyboard.current.tKey.wasPressedThisFrame) {
        //     DrawCardToHand();
        // }
    }

    public void SetupDeck() {
        activeCards.Clear();

        List<CardSO> tempDeck = new List<CardSO>();
        tempDeck.AddRange(deckToUse);

        int iterations = 0;
        while (tempDeck.Count > 0 && iterations < 500) {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);
            iterations++;
        }
    }

    public void DrawCardToHand() {
        if (activeCards.Count == 0) {
            SetupDeck();
        }
        Card newCard = Instantiate(cardToSpawn, transform.position, transform.rotation);
        newCard.cardSO = activeCards[0];
        newCard.SetupCard();

        activeCards.RemoveAt(0);
        HandController.instance.AddCardToHand(newCard);
    }

    public void DrawCardForMana() {
        if (BattleController.instance.playerMana >= drawCardCost) {
            DrawCardToHand();
            BattleController.instance.SpendPlayerMana(drawCardCost);
        } else {
            UIController.instance.ShowManaWarning();
        }
    }

    public void DrawMultipleCards(int amountToDraw) {
        StartCoroutine(DrawMultipleCo(amountToDraw));
    }

    IEnumerator DrawMultipleCo(int amountToDraw) {
        for (int i = 0; i < amountToDraw; i++) {
            DrawCardToHand();

            yield return new WaitForSeconds(waitBetweenDrawingCards);
        }
    }

}
