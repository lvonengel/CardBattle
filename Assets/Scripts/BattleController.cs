using UnityEngine;
using UnityEngine.InputSystem;

public class BattleController : MonoBehaviour {
    public static BattleController instance {get; private set;}
    
    public int startingMana = 4, maxMana = 12;
    public int playerMana;
    private int currentPlayerMaxMana;
    private int startingCardsAmount = 5;

    public enum TurnOrder {playerActive, playerCardAttacks, enemyActive, enemyCardAttacks}
    public TurnOrder currentPhase;
    private int cardsToDrawPerTurn = 2;
    public Transform discardPoint;
    public int playerHealth, enemyHealth;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        currentPlayerMaxMana = startingMana;
        FillPlayerMana();
        DeckController.instance.DrawMultipleCards(startingCardsAmount);
        UIController.instance.SetPlayerHealthText(playerHealth);
        UIController.instance.SetEnemyHealthText(enemyHealth);
    }

    private void Update() {
        if (Keyboard.current.tKey.wasPressedThisFrame) {
            AdvanceTurn();
        }
    }

    public void SpendPlayerMana(int amountToSpend) {
        playerMana -= amountToSpend;
        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void FillPlayerMana() {
        playerMana = currentPlayerMaxMana;
        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void AdvanceTurn() {
        currentPhase++;

        if ((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length) {
            currentPhase = 0;
        }

        switch (currentPhase) {
            
            case TurnOrder.playerActive:
                UIController.instance.ShowEndTurnButton();
                UIController.instance.ShowDrawCardButton();

                if (currentPlayerMaxMana < maxMana) {
                    currentPlayerMaxMana++;
                }
                FillPlayerMana();
                DeckController.instance.DrawMultipleCards(cardsToDrawPerTurn);
                break;
            case TurnOrder.playerCardAttacks:
                CardPointsController.instance.PlayerAttack();
                break;
            case TurnOrder.enemyActive:
                CardPointsController.instance.EnemyAttack();
                break;
            case TurnOrder.enemyCardAttacks:
                Debug.Log("Skipping enemy card attacks");
                AdvanceTurn();
                break;
        }
    }

    public void EndPlayerTurn() {
        UIController.instance.HideEndTurnButton();
        UIController.instance.HideDrawCardButton();
        AdvanceTurn();
    }

    public void DamagePlayer(int damageAmount) {
        if (playerHealth > 0) {
            playerHealth -= damageAmount;
            if (playerHealth <= 0) {
                playerHealth = 0;

                //end battle
            }
            UIController.instance.SetPlayerHealthText(playerHealth);

            UIDamageIndicator damageClone = Instantiate(UIController.instance.playerDamage, UIController.instance.playerDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }
    public void DamageEnemy(int damageAmount) {
        if (enemyHealth > 0) {
            enemyHealth -= damageAmount;
            if (enemyHealth <= 0) {
                enemyHealth = 0;

                //end battle
            }
            UIController.instance.SetEnemyHealthText(enemyHealth);

            UIDamageIndicator damageClone = Instantiate(UIController.instance.enemyDamage, UIController.instance.enemyDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }

}
