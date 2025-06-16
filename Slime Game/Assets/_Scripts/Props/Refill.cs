using System;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(CircleCollider2D))]
public class Refill : MonoBehaviour
{
    private CircleCollider2D col;

    [SerializeField] private float currRefill;
    [SerializeField] private float currTime;

    [Space(10)]
    [SerializeField, Tooltip("Largest to Smallest")]
    private RefillState[] refillStates;

    private float maxTime = 0;

    [Header("GUI")]
    private GameObject gui;

    private void Awake() {
        gui = transform.GetChild(0).gameObject;
        col = GetComponent<CircleCollider2D>();
    }

    private void Start() {
        foreach (RefillState state in refillStates) {
            maxTime += state.windUpTime;
        }
    }

    private void Update() {
        if (currTime < maxTime) RefillUpdate();
    }

    private void RefillUpdate() {
        currTime += Time.deltaTime;

        foreach (RefillState state in refillStates) {
            if (currTime >= state.windUpTime) {
                currRefill = state.amount;
                gui.SetActive(true);
                gui.transform.localScale = Vector3.one * state.size;
                col.enabled = true;
                col.radius = state.size / 2;
                return;
            }
        }

        gui.SetActive(false);
        col.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Movement>().AirRefill(currRefill);
            col.enabled = false;
            currTime = 0;
            currRefill = 0;
        }
    }
}

[Serializable]
public class RefillState
{
    public float amount;
    public float windUpTime;
    public float size;
}