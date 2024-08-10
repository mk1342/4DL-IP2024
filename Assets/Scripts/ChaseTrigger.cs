using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class ChaseTrigger : MonoBehaviour
{
    public Chase Chase;
    public Volume volume;
    public Color color = Color.red;
    public float intensity = 3;
    public float duration = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Chase.isChasing)
        {
            ColorAdjustments colorAdjustments;
            if (volume.profile.TryGet(out colorAdjustments))
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Join(
                    DOTween.To(
                        () => colorAdjustments.colorFilter.value,                // Getter for current color
                        x => colorAdjustments.colorFilter.value = x,              // Setter for new color
                        color,                                            // Target color
                        duration                                                   // Duration of the tween
                    )
                );
                sequence.Join(
                    DOTween.To(
                        () => colorAdjustments.postExposure.value,       // Getter for current intensity
                        x => colorAdjustments.postExposure.value = x,     // Setter for new intensity
                        intensity,                                          // Target intensity value
                        duration                                                   // Duration of the tween
                    )
                );
            }
            Chase.isChasing = true;
        }
    }
}
