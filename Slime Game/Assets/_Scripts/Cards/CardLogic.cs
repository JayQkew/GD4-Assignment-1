using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
    public Card card;
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image image;
    public void SetCard(Card c) {
        card = c;
        //get ui and make it match
        header.text = c.cardName;
        description.text = c.description;
        image.sprite = c.icon;
    }
}
