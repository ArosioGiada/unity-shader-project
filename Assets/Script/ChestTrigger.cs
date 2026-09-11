using UnityEngine;
public class ChestTrigger : MonoBehaviour
{
    public bool m_opened = false;
    public Animation m_chestAnim;
    public ParticleSystem m_burnEffect;
    public Light m_burnLight;
    public AudioSource m_chestAudio;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the trigger and the chest is not already opened
        if (m_chestAnim.isPlaying == false && m_opened == false)
        {
            // Play the chest opening animation
            m_chestAnim.Play();
            // Set the chest as opened to prevent re-triggering
            m_opened = true;

            // Play the opening sound, synced with the lid animation
            if (m_chestAudio != null)
            {
                m_chestAudio.Play();
            }
            // Get the length of the animation clip to time the burn effect
            float animLength = m_chestAnim.clip.length;

            // Grant the player the ability to burn spiders
            PlayerPowers player = other.GetComponent<PlayerPowers>();
            if (player != null)
            {
                player.CanBurnSpiders = true;
            }
            // Schedule the burn effects to play after the animation has finished
            Invoke(nameof(PlayBurnEffects), animLength);
        }
    }
    // Method to play burn effects after the chest opening animation
    private void PlayBurnEffects()
    {
        // Play the burn particle effect
        if (m_burnEffect != null) m_burnEffect.Play();

        // Activate the burn light
        if (m_burnLight != null) m_burnLight.gameObject.SetActive(true);
    }
}