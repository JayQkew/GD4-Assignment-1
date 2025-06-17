using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Serializable]
public class RoundState : GameBaseState
{
    [SerializeField] private float maxRoundTime;
    [SerializeField] private float currRoundTime;
    
    [SerializeField] private Transform[] spawns = new Transform[2];
    [SerializeField] private GameObject[] props;

    [SerializeField] private GameObject ball;
    
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Volume volume;

    private GameManager gm;
    public override void EnterState(GameManager manager) {
        currRoundTime = maxRoundTime;
        gm = manager;
        MapManager.Instance.NextMap();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void UpdateState(GameManager manager) {
        currRoundTime -= Time.deltaTime;
        PointUI.Instance?.UpdateTimer(currRoundTime);
        if (currRoundTime <= 0) {
            // check if a player is in the lead, otherwise go into sudden death
            Debug.Log("SUDDEN DEATH");
            PointManager pointManager = PointManager.Instance;
            pointManager.suddenDeath = true;
            if (pointManager.points[0] == pointManager.points[1]) return;
            pointManager.RoundWon(pointManager.points[0] > pointManager.points[1] ? 0 : 1);
        }
    }

    public override void ExitState(GameManager manager) {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PointManager.Instance.onScoreStart.RemoveListener(StartScoreCameraEffects);
        PointManager.Instance.onScoreEnd.RemoveListener(EndScoreCameraEffects);
        PointManager.Instance.suddenDeath = false;
    }

    private void ResetPlayerPos() {
        //each player goes back to spawn (move the soft bodies)
        for (int i = 0; i < spawns.Length; i++) {
            SoftBody softBody = Multiplayer.Instance.players[i].GetComponentInChildren<SoftBody>();
            softBody.MoveSoftBody(spawns[i].position);
            softBody.ResetVelocity();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        //get the spawns
        Transform spawnParent = GameObject.FindGameObjectWithTag("PlayerSpawns").transform;
        for (int i = 0; i < spawns.Length; i++) {
            spawns[i] = spawnParent.GetChild(i);
        }
        ResetPlayerPos();
        PointUI.Instance.UpdateRoundsWon();
        PointUI.Instance.UpdateAdvantage();
        PointUI.Instance.SetTimerMaxValue(maxRoundTime);
        SetUpProps();
        
        cinemachineCamera = Object.FindObjectOfType<CinemachineCamera>();
        volume = Object.FindObjectOfType<Volume>();

        PointManager.Instance.onScoreStart.AddListener(StartScoreCameraEffects);
        PointManager.Instance.onScoreEnd.AddListener(EndScoreCameraEffects);
        
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    private void SetUpProps() {
        GameObject[] propSpawns = GameObject.FindGameObjectsWithTag("PropSpawn");
        Transform propParent = GameObject.Find("Props").transform;
        foreach (GameObject spawn in propSpawns) {
            GameObject prop = props[Random.Range(0, props.Length)];
            if (prop == null) continue;
            Object.Instantiate(prop, spawn.transform.position, Quaternion.identity, propParent);
        }
    }

private void StartScoreCameraEffects() {
    // cinemachineCamera.Follow = ball.transform;
    Time.timeScale = 0.2f;
    
    // Start the transition to score effects
    gm.StartCoroutine(LerpVolumeEffects(true, 1.5f)); // true = to score effects, 1f = duration
}

private void EndScoreCameraEffects() {
    // cinemachineCamera.Follow = null;
    // cinemachineCamera.transform.position = new Vector3(0, 0, -10);
    Time.timeScale = 1;
    
    // Start the transition back to normal effects
    gm.StartCoroutine(LerpVolumeEffects(false, 0.5f)); // false = to normal effects, 0.5f = duration
}

private IEnumerator LerpVolumeEffects(bool toScoreEffects, float duration) {
    // Get the effects
    volume.profile.TryGet(out Bloom bloom);
    volume.profile.TryGet(out ChromaticAberration chromaticAberration);
    
    // Define start and end values
    float bloomStart, bloomEnd;
    float chromaStart, chromaEnd;
    
    if (toScoreEffects) {
        // Current values to score effect values
        bloomStart = bloom?.intensity.value ?? 5f;
        bloomEnd = 25f;
        chromaStart = chromaticAberration?.intensity.value ?? 0.15f;
        chromaEnd = 1.0f;
    } else {
        // Current values to normal values
        bloomStart = bloom?.intensity.value ?? 25f;
        bloomEnd = 5f;
        chromaStart = chromaticAberration?.intensity.value ?? 1.0f;
        chromaEnd = 0.15f;
    }
    
    float elapsedTime = 0f;
    
    while (elapsedTime < duration) {
        elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime since you're changing timeScale
        float t = elapsedTime / duration;
        
        // Use smoothstep for eased interpolation (optional - you can use linear t instead)
        float smoothT = t * t * (3f - 2f * t);
        
        // Lerp the values
        if (bloom != null) {
            bloom.intensity.value = Mathf.Lerp(bloomStart, bloomEnd, smoothT);
        }
        
        if (chromaticAberration != null) {
            chromaticAberration.intensity.value = Mathf.Lerp(chromaStart, chromaEnd, smoothT);
        }
        
        yield return null;
    }
    
    // Ensure final values are set exactly
    if (bloom != null) {
        bloom.intensity.value = bloomEnd;
    }
    
    if (chromaticAberration != null) {
        chromaticAberration.intensity.value = chromaEnd;
    }
}
}
