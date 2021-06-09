using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [SerializeField] TextMeshProUGUI speedometerText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] Slider speedSlider;
    [SerializeField] Button restartButton;

    private bool isSlow = true;
    private bool gameOver = false;
    private float forwardSpeed;
    private float score;
    private float startPos;

    private Rigidbody playerRb;
    private WheelCollider wheelFR;
    private WheelCollider wheelFL;
    private WheelCollider wheelRR;
    private WheelCollider wheelRL;
    [SerializeField] GameObject centerOfMass;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] AudioClip explosionAudio;
    AudioSource playerAudio;


    [SerializeField] float wheelTorque;
    [SerializeField] float brakeTorque;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        wheelFR = GameObject.Find("Wheel_fr").GetComponent<WheelCollider>();
        wheelFL = GameObject.Find("Wheel_fl").GetComponent<WheelCollider>();
        wheelRR = GameObject.Find("Wheel_rr").GetComponent<WheelCollider>();
        wheelRL = GameObject.Find("Wheel_rl").GetComponent<WheelCollider>();
        playerRb.centerOfMass = centerOfMass.transform.position;

        startPos = transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameOver == false)
        {
            TurnVehicle();

            AccelerateVehicle();

            //update speed slider
            speedSlider.value = forwardSpeed / 100f;

            CalculateSpeed();

            UpdateScore();

            //game over if fall off
            if (transform.position.y < -10f)
            {
                GameOver();
            }

            CheckSpeed();
        }
    }

    private void GameOver()
    {
        gameOver = true;
        StopAllCoroutines();
        displayText.SetText("Game Over!");
        displayText.gameObject.SetActive(true);
        speedometerText.text = "Speed:";
        speedSlider.value = 0f;
        explosion.transform.position = transform.position + new Vector3(-.82f, 0.38f, -0.14f);
        explosion.gameObject.SetActive(true);
        playerAudio.PlayOneShot(explosionAudio);
        restartButton.gameObject.SetActive(true);
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = false;
        }
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
    }

    private IEnumerator GameOverTimer()
    {
        isSlow = true;
        displayText.SetText("Speed Up!\n3");
        yield return new WaitForSeconds(1f);
        displayText.SetText("Speed Up!\n2");
        yield return new WaitForSeconds(1f);
        displayText.SetText("Speed Up!\n1");
        yield return new WaitForSeconds(1f);
        GameOver();
    }

    private void TurnVehicle()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        // limit turn angle to 20 degrees
        float wheelTurnAngle = Mathf.Clamp(wheelFR.steerAngle + horizontalInput, -20.0f, 20.0f);
        // striaghten the wheels if no input
        if (horizontalInput == 0.0f && Mathf.Abs(wheelTurnAngle) > 0.0f)
        {
            wheelTurnAngle /= 1.1f;
        }
        wheelFR.steerAngle = wheelTurnAngle;
        wheelFL.steerAngle = wheelTurnAngle;
        wheelFR.transform.localRotation = Quaternion.Euler(0, wheelTurnAngle, 0);
        wheelFL.transform.localRotation = Quaternion.Euler(0, wheelTurnAngle, 0);
    }

    private void AccelerateVehicle()
    {
        // Rear wheel drive.
        float forwardInput = Input.GetAxis("Vertical");

        if(forwardInput < 0.0f)
        {
            wheelRR.brakeTorque = brakeTorque;
            wheelRL.brakeTorque = brakeTorque;
            wheelFR.brakeTorque = brakeTorque;
            wheelFR.brakeTorque = brakeTorque;

        }
        else
        {
            wheelRR.brakeTorque = 0f;
            wheelRL.brakeTorque = 0f;
            wheelFR.brakeTorque = 0f;
            wheelFR.brakeTorque = 0f;
            wheelRR.motorTorque = wheelTorque * forwardInput;
            wheelRL.motorTorque = wheelTorque * forwardInput;

        }

    }

    public void Restart()
    {
        SceneManager.LoadScene("Prototype 1");
    }

    private void CalculateSpeed()
    {
        forwardSpeed = Mathf.Round(playerRb.velocity.z * 3.6f);
        speedometerText.SetText("Speed: " + forwardSpeed + " km/h");
    }

    private void UpdateScore()
    {
        score = transform.position.z - startPos;
        scoreText.SetText("Score: " + Mathf.Round(score));
    }

    private void CheckSpeed()
    {
        if (forwardSpeed > 20f)
        {
            isSlow = false;
            StopAllCoroutines();
            displayText.SetText("");
        }
        else if (forwardSpeed < 20f && isSlow == false)
        {
            StartCoroutine(GameOverTimer());
        }
    }
}
