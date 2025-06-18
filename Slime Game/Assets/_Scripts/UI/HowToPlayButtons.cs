using System;
using TMPro;
using UnityEngine;

public class HowToPlayButtons : MonoBehaviour
{
    [SerializeField] GameObject UIItems;
    [SerializeField] GameObject fwdButton;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject startButton;

    private RectTransform items;

    void Start()
    {
        items = UIItems.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (items.position.x == 0)
        {
            fwdButton.SetActive(true);
            backButton.SetActive(false);
            startButton.SetActive(false);
        }
        else if (items.position.x == -2000)
        {
            fwdButton.SetActive(false);
            backButton.SetActive(true);
            startButton.SetActive(false);
        }
        else if (items.position.x == -4000)
        {
            fwdButton.SetActive(false);
            backButton.SetActive(true);
            startButton.SetActive(false);
        }
        else if (items.position.x == -6000)
        {
            fwdButton.SetActive(false);
            backButton.SetActive(true);
            startButton.SetActive(true);
        }
    }

    public void FWD()
    {
        var targetPos= new Vector3(items.position.x + -2000f, 0);
        items.position = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 3);
    }
    public void BACK()
    {
        var targetPos = new Vector3(items.position.x + 2000f, 0);
        items.position = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 3);
    }
    public void GoToMapSelect()
    {
        GameManager.Instance.SwitchState(GameState.MapSelect);
    }
}
