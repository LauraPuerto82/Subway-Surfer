using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] int countDown = 3;
    
    GameObject gameOverPanel;
    [SerializeField] PlayerController myPlayerController;
    [SerializeField] Animator playerAnimator;

    private bool _gameStarted;
    private bool _gameOver;    

    public bool IsGameStarted { get => _gameStarted; set => _gameStarted = value; }
    public bool IsGameOver { get => _gameOver; set => _gameOver = value; }

    void Awake()
    {
        instance = this;
        gameOverPanel = GameObject.Find("GameOverPanel");
        gameOverPanel.gameObject.SetActive(false);
    }

    void Start()
    {        
        myPlayerController = GameObject.Find("Player").GetComponent<PlayerController>();        
        if (myPlayerController.MyCharacterController != null)
        {
            myPlayerController.MyCharacterController.enabled = false;
        }

        playerAnimator = myPlayerController.GetComponent<Animator>();
        playerAnimator.enabled = false;

        InitGame();        
    }

    void Update()
    {
        if (countDown == 0)
        {            
            StartGame();
        }

        if(_gameOver)
        {
            GameOver();
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 50;
        if (countDown > 0)
        {
            GUI.Label(new Rect(15, 15, 300, 75), countDown.ToString(), style);
        }

        if (countDown == 0)
        {            
            if (GUI.Button(new Rect(15, 75, 100, 30), "Restart"))
            {                
                LoadScene(0);
                InitGame();
            }
        }
    }

    private void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    IEnumerator TimeCountDown()
    {
        yield return new WaitForSecondsRealtime(1);
        if (countDown > 0)
        {
            countDown--;
            StartCoroutine(TimeCountDown());
        }
    }

    void InitGame()
    {       
        _gameStarted = false;
        StartCoroutine(TimeCountDown());
    }

    void StartGame()
    {                
        myPlayerController.MyCharacterController.enabled = true;
        playerAnimator.enabled = true;
        _gameStarted = true;
    }

    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);         
    }
}