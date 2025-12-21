using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController instance;
    [SerializeField] private TMP_Text playerManaText;
    [SerializeField] private GameObject manaWarning;
    [SerializeField] private Button drawCardButton;
    private float manaWarningTime = 2f;
    private float manaWarningCounter;

    private void Awake() {
        instance = this;
        manaWarning.SetActive(false);

        drawCardButton.onClick.AddListener(() => {
            DrawCard();
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

    public void ShowManaWarning() {
        manaWarning.SetActive(true);
        manaWarningCounter = manaWarningTime;
    }

    public void DrawCard() {
        DeckController.instance.DrawCardForMana();
    }
    

}