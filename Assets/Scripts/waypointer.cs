using UnityEngine;
using TMPro;

public class WaypointIndicator : MonoBehaviour
{
    public Transform player; // Assign the player
    public Transform waypoint; // Assign the waypoint

    public RectTransform indicatorUI; // UI Element (Image + Text)
    public TextMeshProUGUI distanceText; // Text for distance

    public float hideDistance = 2f; // Hide UI when close

    private Camera cam;

    void Start()
    {
        cam = Camera.main; // Get main camera
    }

    void Update()
    {
        if (waypoint == null || player == null)
            return;

        // Calculate distance
        float distance = Vector3.Distance(player.position, waypoint.position);
        distanceText.text = distance.ToString("F1") + "m";

        // Hide indicator if close
        indicatorUI.gameObject.SetActive(distance > hideDistance);

        // Convert world position to screen space
        Vector3 screenPos = cam.WorldToScreenPoint(waypoint.position);

        // Check if behind the camera
        if (screenPos.z < 0)
        {
            indicatorUI.gameObject.SetActive(false);
        }
        else
        {
            indicatorUI.gameObject.SetActive(true);
            indicatorUI.position = screenPos;
        }
    }
}
