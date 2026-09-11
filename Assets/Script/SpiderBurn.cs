using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiderBurn : MonoBehaviour
{
    // The body and every eye piece each have their own renderer, so I need
    // to dissolve all of them together, not just the main body.
    [Header("All renderers to dissolve (body + every eye piece)")]
    public Renderer[] m_renderersToBurn;

    // This is the collider that physically blocks the player from walking
    // through the spider. I switch it off once the spider starts burning,
    // so the player doesn't get stuck on a spider that's disappearing.
    [Header("Physical Collider to disable after burning")]
    public Collider m_physicalCollider;

    [Header("Animator")]
    public Animator m_animator;

    // Small pause before the dissolve kicks in, so the player actually sees
    // the spider fall over (Death animation) before it starts burning away.
    [Tooltip("Delay before the dissolve starts, so the Death fall animation plays first")]
    public float m_deathAnimDelay = 0.6f;

    [Header("Timing")]
    public float m_burnDuration = 2f;

    // These values come from testing the dissolve shader directly on the
    // spider in Object Space - they mark where the mesh starts (fully
    // visible) and ends (fully dissolved) along its local Y axis.
    [Header("CutoffHeight Range (Object Space)")]
    public float m_cutoffStart = -0.02f;
    public float m_cutoffEnd = 0.03f;

    // Each renderer gets its own material instance, so burning one spider
    // never affects any other spider sharing the same base material.
    private List<Material> m_materialInstances = new List<Material>();
    private bool m_isBurning = false;

    private void Awake()
    {
        // Grab a unique copy of the material for every renderer up front,
        // so I can safely animate CutoffHeight on this spider only.
        foreach (Renderer r in m_renderersToBurn)
        {
            if (r != null)
                m_materialInstances.Add(r.material);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore new triggers if this spider is already burning.
        if (m_isBurning) return;

        if (other.CompareTag("Player"))
        {
            PlayerPowers player = other.GetComponent<PlayerPowers>();

            if (player != null && player.CanBurnSpiders)
            {
                // Player has the power from the chest: start the burn sequence.
                StartCoroutine(BurnRoutine());
            }
            else
            {
                // Player got close without the power yet
                // so just play ascare reaction.
                if (m_animator != null)
                {
                    m_animator.SetTrigger("Scare");
                }
            }
        }
    }

    private IEnumerator BurnRoutine()
    {
        m_isBurning = true;

        // Stop blocking the player immediately, so they don't get stuck
        // walking into a spider that's about to disappear.
        if (m_physicalCollider != null)
            m_physicalCollider.enabled = false;

        // Play the death/fall animation first.
        if (m_animator != null)
            m_animator.SetTrigger("Die");

        // Give the fall animation a moment to actually play before the
        // dissolve effect starts covering it up.
        yield return new WaitForSeconds(m_deathAnimDelay);

        // Gradually raise CutoffHeight over time on every material instance,
        // which drives the dissolve shader's burn effect.
        float elapsed = 0f;

        while (elapsed < m_burnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / m_burnDuration;
            float currentCutoff = Mathf.Lerp(m_cutoffStart, m_cutoffEnd, t);

            foreach (Material mat in m_materialInstances)
            {
                mat.SetFloat("_CutoffHeight", currentCutoff);
            }

            yield return null;
        }

        // Fully dissolved now, so the spider can be removed from the scene.
        gameObject.SetActive(false);
    }
}