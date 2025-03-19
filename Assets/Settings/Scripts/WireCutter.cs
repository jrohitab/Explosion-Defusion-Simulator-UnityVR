using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WireCutter : MonoBehaviour
{
    public GameObject explosionPrefab;
    public GameObject smokePrefab; // Smoke effect after explosion
    public AudioClip warningSound; // Changed to AudioClip
    public AudioClip explosionSound;
    public AudioClip cuttingSound; // Added wire cutting sound

    private int currentWireIndex = 0;
    public GameObject[] wires;
    public GameObject bomb;

    public GameObject missionFailedCanvas;
    public GameObject missionPassedCanvas;

    private bool isWaitingForInput = false;
    private bool isGameOver = false;

    private void Start()
    {
        // Ensure all wires are active at start
        for (int i = 0; i < wires.Length; i++)
        {
            wires[i].SetActive(true);
        }

        missionFailedCanvas.SetActive(false);
        missionPassedCanvas.SetActive(false);

        if (bomb != null)
        {
            bomb.SetActive(true);
        }
    }

    private void Update()
    {
        if (isGameOver && IsAnyButtonPressed())
        {
            SceneManager.LoadScene(0);
        }
    }

    private bool IsAnyButtonPressed()
    {
        return OVRInput.GetDown(OVRInput.Button.One) ||
               OVRInput.GetDown(OVRInput.Button.Two) ||
               OVRInput.GetDown(OVRInput.Button.Three) ||
               OVRInput.GetDown(OVRInput.Button.Four) ||
               OVRInput.GetDown(OVRInput.Button.Start) ||
               OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) ||
               OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) ||
               OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) ||
               OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger);
    }

    private void OnTriggerEnter(Collider other)
    {
        int wireIndex = System.Array.IndexOf(wires, other.gameObject);

        if (other.CompareTag("WireTag") && !isWaitingForInput)
        {
            StartCoroutine(WaitForTriggerPress(other.gameObject, wireIndex));
        }
    }

    private IEnumerator WaitForTriggerPress(GameObject wire, int wireIndex)
    {
        isWaitingForInput = true;

        // Wait for player to press trigger button
        while (!OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) &&
               !OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            yield return null;
        }

        // Play cutting sound when wire is cut
        if (cuttingSound != null)
        {
            AudioSource.PlayClipAtPoint(cuttingSound, wire.transform.position);
        }

        // Correct wire cut
        if (wireIndex == currentWireIndex)
        {
            Debug.Log("Correct Wire Cut!");
            wire.SetActive(false);
            TriggerHapticFeedback(0.2f, 0.5f);
            currentWireIndex++;

            if (currentWireIndex >= wires.Length)
            {
                Debug.Log("All wires cut successfully!");
                StartCoroutine(DelayedMissionPassed());
            }
        }
        else // Wrong wire cut (trigger explosion)
        {
            Debug.Log("Wrong Wire Cut! Triggering Explosion!");
            wire.SetActive(false);
            TriggerHapticFeedback(0.5f, 1.0f);
            StartCoroutine(TriggerExplosionWithDelay(wire.transform.position));
        }

        isWaitingForInput = false;
    }

    private IEnumerator TriggerExplosionWithDelay(Vector3 position)
    {
        // Play warning sound before explosion
        if (warningSound != null)
        {
            AudioSource.PlayClipAtPoint(warningSound, position);
        }

        yield return new WaitForSeconds(4f);

        // Hide the bomb after explosion
        if (bomb != null)
        {
            bomb.SetActive(false);
        }

        // Spawn explosion effect
        Instantiate(explosionPrefab, position, Quaternion.identity);

        // Play explosion sound
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, position);
        }

        // Spawn smoke effect
        if (smokePrefab != null)
        {
            yield return new WaitForSeconds(1f);
            Instantiate(smokePrefab, position, Quaternion.identity);
        }

        yield return new WaitForSeconds(3f);
        ShowMissionFailed();
    }

    private IEnumerator DelayedMissionPassed()
    {
        yield return new WaitForSeconds(2f);
        ShowMissionPassed();
    }

    private void ShowMissionFailed()
    {
        missionFailedCanvas.SetActive(true);
        isGameOver = true;
    }

    private void ShowMissionPassed()
    {
        missionPassedCanvas.SetActive(true);
        if (bomb != null)
        {
            bomb.SetActive(false);
        }
        isGameOver = true;
    }

    private void TriggerHapticFeedback(float duration, float strength)
    {
        StartCoroutine(HapticPulse(duration, strength));
    }

    private IEnumerator HapticPulse(float duration, float strength)
    {
        OVRInput.SetControllerVibration(1, strength, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(1, strength, OVRInput.Controller.LTouch);
        yield return new WaitForSeconds(duration);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
    }
}
