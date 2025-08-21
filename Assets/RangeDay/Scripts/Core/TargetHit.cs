using UnityEngine;

namespace Rangeday.Core
{
    public class TargetHit : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float hitVolume = 0.8f;
        
        [Header("Visual Feedback")]
        [SerializeField] private float flashDuration = 0.1f;
        [SerializeField] private Color flashColor = Color.white;
        
        private Renderer targetRenderer;
        private Color originalColor;
        private bool isFlashing = false;

        private void Awake()
        {
            // Get components
            audioSource = GetComponent<AudioSource>();
            targetRenderer = GetComponent<Renderer>();
            
            if (targetRenderer != null)
            {
                originalColor = targetRenderer.material.color;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // For now, any collision triggers hit
            // Later we'll make this more specific to bullets
            HandleHit(collision.contacts[0].point);
        }

        public void HandleHit(Vector3 hitPoint)
        {
            // Play audio
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.volume = hitVolume;
                audioSource.pitch = Random.Range(0.95f, 1.05f); // Slight random pitch
                audioSource.Play();
            }
            
            // Visual feedback
            if (targetRenderer != null && !isFlashing)
            {
                StartCoroutine(FlashTarget());
            }
            
            // Debug output
            Debug.Log($"Target hit at {hitPoint} on {gameObject.name}");
        }

        private System.Collections.IEnumerator FlashTarget()
        {
            isFlashing = true;
            
            // Flash to white/bright color
            targetRenderer.material.color = flashColor;
            
            yield return new WaitForSeconds(flashDuration);
            
            // Return to original color
            targetRenderer.material.color = originalColor;
            
            isFlashing = false;
        }
    }
}