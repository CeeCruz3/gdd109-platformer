using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float collectableScore = 50f;
    public float gameSpeed { get; private set; }

    public float duration = 1f;
    public AnimationCurve curve;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject camTarget;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hiscoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button retryButton;
    [SerializeField] private AudioSource death;
    [SerializeField] private AudioSource flip;


    private Player player;
    private Spawner spawner;

    private float score;
    public float Score => score;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>(); 


        NewGame();
    }

    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        Collectable[] collectables = FindObjectsOfType<Collectable>();

        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }
        foreach (var collectable in collectables)
        {
            Destroy(collectable.gameObject);
        }



        score = 0f;
        gameSpeed = initialGameSpeed;
        enabled = true;

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        flip.Play();

        UpdateHiscore();
    }

    public void GameOver()
    {
        death.Play();
        gameSpeed = 0f;
        enabled = false;
        StartCoroutine(
                routine: Shake(cam));
        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        UpdateHiscore();
    }

    IEnumerator Shake(Camera cam)
    {
        
        Vector2 startPos = camTarget.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float strength = curve.Evaluate(elapsed / duration);
            camTarget.transform.position = startPos + Random.insideUnitCircle * strength;
            cam.transform.position = new Vector3(camTarget.transform.position.x, camTarget.transform.position.y, -10f);
            yield return null;
        }

        cam.transform.position = new Vector3(startPos.x, startPos.y, -10f); ;
    }

    public void scoreUp()
    {
        score += collectableScore;
    }

    private void Update()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
    }

    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }

}
