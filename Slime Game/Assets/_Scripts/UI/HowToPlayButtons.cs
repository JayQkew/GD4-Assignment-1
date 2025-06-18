using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HowToPlayButtons : MonoBehaviour
{
    [SerializeField] private GameObject UIItems;
    [SerializeField] private GameObject fwdButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject startButton;

    [SerializeField] private float delta;
    private Vector3 targetPos;
    [SerializeField] private int currIndex = 0;
    [SerializeField] private float[] xPos = Array.Empty<float>();

    private RectTransform items;
    private Camera cam;

    void Start() {
        cam = Camera.main;
        items = UIItems.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update() {
        if (Mathf.Approximately(cam.transform.position.x, xPos[0])) {
            fwdButton.GetComponent<Button>().interactable = true;
            backButton.GetComponent<Button>().interactable = false;
            startButton.SetActive(false);
            EventSystem.current.SetSelectedGameObject(fwdButton);
        }
        else if (Mathf.Approximately(cam.transform.position.x, xPos[1])) {
            fwdButton.GetComponent<Button>().interactable = true;
            backButton.GetComponent<Button>().interactable = true;
            startButton.SetActive(false);
        }
        else if (Mathf.Approximately(cam.transform.position.x, xPos[2])) {
            fwdButton.GetComponent<Button>().interactable = true;
            backButton.GetComponent<Button>().interactable = true;
            startButton.SetActive(false);
        }
        else if (Mathf.Approximately(cam.transform.position.x, xPos[3])) {
            fwdButton.GetComponent<Button>().interactable = true;
            backButton.GetComponent<Button>().interactable = true;
            startButton.SetActive(false);
        }
        else if (Mathf.Approximately(cam.transform.position.x, xPos[4])){
            fwdButton.GetComponent<Button>().interactable = true;
            backButton.GetComponent<Button>().interactable = true;
            startButton.SetActive(false);
        }
        else if (Mathf.Approximately(cam.transform.position.x, xPos[5])){
            fwdButton.GetComponent<Button>().interactable = false;
            backButton.GetComponent<Button>().interactable = true;
            startButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(startButton);
        }
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, targetPos, delta * Time.deltaTime);
    }

    public void Fwd() {
        currIndex++;
        if (currIndex > xPos.Length - 1) currIndex = 0;
        targetPos = new Vector3(xPos[currIndex], 0, -10);
    }

    public void Back() {
        currIndex--;
        if (currIndex < 0) currIndex = xPos.Length - 1;
        targetPos = new Vector3(xPos[currIndex], 0, -10);
    }

    public void GoToMapSelect() {
        GameManager.Instance.SwitchState(GameState.MapSelect);
    }
}