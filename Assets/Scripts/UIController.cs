using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController instance;
    [SerializeField] private TMP_Text playerManaText, playerHealthText, enemyHealthText;
    [SerializeField] private GameObject manaWarning;
    [SerializeField] private Button drawCardButton, endTurnButton;
    public UIDamageIndicator playerDamage, enemyDamage;
    private float manaWarningTime = 2f;
    private float manaWarningCounter;

    private void Awake() {
        instance = this;
        manaWarning.SetActive(false);

        drawCardButton.onClick.AddListener(() => {
            DrawCard();
        });
        endTurnButton.onClick.AddListener(() => {
            EndPlayerTurn();
        });
    }

    private void Update() {
        if (manaWarningCounter > 0) {
            manaWarningCounter -= Time.deltaTime;
            if (manaWarningCounter <= 0) {
                manaWarning.SetActive(false);
            }
        }
    }

    public void SetPlayerManaText(int manaAmount) {
        playerManaText.text = "Mana: " + manaAmount;
    }

    public void SetPlayerHealthText(int healthAmount) {
        playerHealthText.text = "Player Health " + healthAmount;
    }

    public void SetEnemyHealthText(int healthAmount) {
        enemyHealthText.text = "Enemy Health " + healthAmount;
    }

    public void ShowManaWarning() {
        manaWarning.SetActive(true);
        manaWarningCounter = manaWarningTime;
    }

    public void DrawCard() {
        DeckController.instance.DrawCardForMana();
    }

    public void EndPlayerTurn() {
        BattleController.instance.EndPlayerTurn();
    }

    public void ShowEndTurnButton() {
        endTurnButton.gameObject.SetActive(true);
    }
    public void HideEndTurnButton() {
        endTurnButton.gameObject.SetActive(false);
    }
    public void ShowDrawCardButton() {
        drawCardButton.gameObject.SetActive(true);
    }
    public void HideDrawCardButton() {
        drawCardButton.gameObject.SetActive(false);
    }
    

}