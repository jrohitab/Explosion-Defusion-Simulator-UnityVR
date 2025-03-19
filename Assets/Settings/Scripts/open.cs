using System.Collections;
using UnityEngine;

public class WireCut : MonoBehaviour
{
    public Animator animator;  // Reference to the Animator
    private bool isCut = false; // Ensures animation plays only once

    private void OnTriggerEnter(Collider other)
    {
        if (!isCut && other.CompareTag("WireCutter")) // Check if WireCutter collides
        {
            isCut = true;  // Prevent re-triggering
            animator.SetTrigger("Cut"); // Trigger animation
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Wait for animation to finish
        Destroy(gameObject); // Destroy object after animation
    }
}
