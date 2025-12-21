using UnityEngine;

public class BattleController : MonoBehaviour {
    public static BattleController instance {get; private set;}
    
    public int startingMana = 4, maxMaxa = 12;
    public int playerMana;
    private int startingCardsAmount = 5;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        playerMana = startingMana;
        UIController.instance.SetPlayerManaText(playerMana);
        DeckController.instance.DrawMultipleCards(startingCardsAmount);
    }

    public void SpendPlayerMana(int amountToSpend) {
        playerMana -= amountToSpend;
        UIController.instance.SetPlayerManaText(playerMana);
    }

}
