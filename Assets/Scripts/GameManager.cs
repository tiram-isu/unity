using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private Paddle playerPaddle;
    [SerializeField] private Paddle computerPaddle;
    [SerializeField] private Text playerScoreText;
    [SerializeField] private Text computerScoreText;

    private Animator playerPaddleAnimator;
    private Animator computerPaddleAnimator;
    private Animator playerTextAnimator;
    private Animator computerTextAnimator;
    private Animator gameOverAnimator;

    private Text gameOverText;

    private int playerScore;
    private int computerScore;
    private static bool noPlayerDetected;

    private Rigidbody2D playerRigidbody;
    private Rigidbody2D computerRigidbody;
    private Rigidbody2D ballRigidbody;

    private void Start()
    {
        GameObject playerPaddleObject = GameObject.Find("Player Paddle");
        GameObject computerPaddleObject = GameObject.Find("Computer Paddle");

        gameOverText = GameObject.Find("Game Over").GetComponent<Text>();

        playerPaddleAnimator = playerPaddleObject.GetComponent<Animator>();
        computerPaddleAnimator = computerPaddleObject.GetComponent<Animator>();
        playerTextAnimator = GameObject.Find("Player Score").GetComponent<Animator>();
        computerTextAnimator = GameObject.Find("Computer Score").GetComponent<Animator>();
        gameOverAnimator = GameObject.Find("Game Over").GetComponent<Animator>();

        playerRigidbody = playerPaddleObject.GetComponent<Rigidbody2D>();
        computerRigidbody = computerPaddleObject.GetComponent<Rigidbody2D>();
        ballRigidbody = GameObject.Find("Ball").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            NewGame();
        }
    }

    public void NewGame()
    {
        gameOverText.color = new Color(255, 0, 0, 0);
        SetPlayerScore(5);
        SetComputerScore(5);
        NewRound();
    }

    public void NewRound()
    {
        playerPaddle.ResetPosition();
        computerPaddle.ResetPosition();
        ball.ResetPosition();

        CancelInvoke();
        Invoke(nameof(StartRound), 1f);
    }

    private void StartRound()
    {
        playerRigidbody.constraints = ~RigidbodyConstraints2D.FreezePositionY;
        computerRigidbody.constraints = ~RigidbodyConstraints2D.FreezePositionY;
        ballRigidbody.constraints = ~RigidbodyConstraints2D.FreezePosition;
        ball.AddStartingForce();
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        computerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        ballRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        gameOverText.color = new Color(255, 0, 0, 255);
    }

    public void OnComputerScored()
    {
        SetPlayerScore(playerScore - 1);
        playerPaddleAnimator.Play("ComputerScored");
        playerTextAnimator.Play("ComputerScored");
        if (playerScore == 0)
            GameOver();
        else
            NewRound();
    }

    public void OnPlayerScored()
    {
        SetComputerScore(computerScore - 1);
        computerPaddleAnimator.Play("PlayerScored");
        computerTextAnimator.Play("PlayerScored");
        if (computerScore == 0)
            GameOver();
        else
            NewRound();
    }

    private void SetPlayerScore(int score)
    {
        playerScore = score;
        playerScoreText.text = score.ToString();
    }

    private void SetComputerScore(int score)
    {
        computerScore = score;
        computerScoreText.text = score.ToString();
    }

}
