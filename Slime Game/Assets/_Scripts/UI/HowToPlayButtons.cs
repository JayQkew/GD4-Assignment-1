using System;
using TMPro;
using UnityEngine;

public class HowToPlayButtons : MonoBehaviour
{
    [SerializeField] private GameObject UIItems;
    [SerializeField] private GameObject fwdButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject startButton;
    [SerializeField] private Vector3 targetPos;

    private RectTransform items;
    [SerializeField] private bool Changed = false;

    void Start()
    {
        items = UIItems.GetComponent<RectTransform>();
        targetPos = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (items.position.x == 0 && !Changed)
        {
            fwdButton.SetActive(true);
            backButton.SetActive(false);
            startButton.SetActive(false);
            Changed = !Changed;
        }
        else if (items.position.x == -2000 && !Changed)
        {
            fwdButton.SetActive(false);
            backButton.SetActive(true);
            startButton.SetActive(false);
            Changed = !Changed;
        }
        else if (items.position.x == -4000 && !Changed)
        {
            fwdButton.SetActive(false);
            backButton.SetActive(true);
            startButton.SetActive(false);
            Changed = !Changed;
        }
        else if (items.position.x == -6000 && !Changed)
        {
            fwdButton.SetActive(false);
            backButton.SetActive(true);
            startButton.SetActive(true);
            Changed = !Changed;
        }
        items.position = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 3);
    }

    public void FWD()
    {
        if (Changed)
        {
            Debug.Log("Run");
            targetPos= new Vector3(targetPos.x + -2000f, 0);
            Changed = !Changed;
        }
    }
    public void BACK()
    {
        if (Changed)
        {
            targetPos = new Vector3(targetPos.x + 2000f, 0);
            Changed = !Changed;
        }
    }
    public void GoToMapSelect()
    {
        GameManager.Instance.SwitchState(GameState.MapSelect);
    }
}
