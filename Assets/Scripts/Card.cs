using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Card : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler {
    [SerializeField] private CardSO cardSO;
    [SerializeField] private TMP_Text attackText, healthText, manaCostText;
    [SerializeField] private TMP_Text nameText, actionDescriptionText, loreText;
    [SerializeField] private Image characterArt, bgArt;

    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float rotateSpeed = 720f;
    [SerializeField] private LayerMask whatIsDesktop, whatIsPlacement;

    public bool inHand;
    public int handPosition;

    private Vector3 targetPoint;
    private Quaternion targetRot;

    private bool isHovered;
    private bool isDragging;

    private HandController theHC;
    public CardPlacePoint assignedPlace;

    private void Start() {
        SetupCard();
        theHC = FindAnyObjectByType<HandController>();
    }

    private void Update() {
        transform.position = Vector3.Lerp(
            transform.position,
            targetPoint,
            moveSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            rotateSpeed * Time.deltaTime
        );
    }


    public void OnPointerEnter(PointerEventData eventData) {
        if (!inHand || isDragging || isHovered) return;

        isHovered = true;

        MoveToPoint(theHC.cardPositions[handPosition] + new Vector3(0f, 1f, 0.4f),Quaternion.identity);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!inHand || isDragging || !isHovered) return;

        isHovered = false;

        ReturnToHand();
    }


    public void OnBeginDrag(PointerEventData eventData) {
        if (!inHand) return;

        isDragging = true;
        isHovered = false;
    }

    public void OnDrag(PointerEventData eventData) {
        if (!isDragging || !inHand) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, whatIsDesktop)) {
            MoveToPoint(hit.point + Vector3.up * 2f, Quaternion.identity);
        }

    }

    public void OnEndDrag(PointerEventData eventData) {
        isDragging = false;
        // if user stops dragging on a placement point, place card there
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, whatIsPlacement)) {
            CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();

            if (selectedPoint.activeCard == null && selectedPoint.isPlayerPoint) {
                selectedPoint.activeCard = this;
                assignedPlace = selectedPoint;

                MoveToPoint(selectedPoint.transform.position, Quaternion.identity);
                inHand = false;
                theHC.RemoveCardFromHand(this);
            } else {
                ReturnToHand();
            }

        } else {
            ReturnToHand();
        }
    }

    private void ReturnToHand() {
        MoveToPoint(theHC.cardPositions[handPosition], theHC.minPos.rotation);
    }

    public void MoveToPoint(Vector3 point, Quaternion rot) {
        targetPoint = point;
        targetRot = rot;
    }

    private void SetupCard() {
        attackText.text = cardSO.attackPower.ToString();
        healthText.text = cardSO.currentHealth.ToString();
        manaCostText.text = cardSO.manaCost.ToString();

        nameText.text = cardSO.name;
        actionDescriptionText.text = cardSO.actionDescription;
        loreText.text = cardSO.cardLore;

        characterArt.sprite = cardSO.characterSprite;
        bgArt.sprite = cardSO.bgSprite;
    }
}
