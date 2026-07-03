using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField]
    private Camera playerCam;
    public float shake = 0;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    public Transform startingPos;
    public bool shakeCam;

    private void Start()
    {
        playerCam = GetComponent<Camera>();
    }

    void Update()
    {
        if (!shakeCam) return;

        if (shake > 0)
        {
            playerCam.transform.localPosition = Random.insideUnitSphere * shakeAmount;
            shake -= Time.deltaTime * decreaseFactor;

        }
        else
        {
            shake = 0.0f;
            playerCam.transform.position = startingPos.position;
            shakeCam = false;
            //test
        }
    }
}
