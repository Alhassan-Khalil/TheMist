using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Stamina : MonoBehaviour
{
    public SafeArea safeArea;

    public Slider StaminaBar;
    private Rigidbody RB;

    public bool unlimtedStamina = false;

    public float MaxStamina = 100f;
    public float CurrentStamina;
    public float staminaFallMult;
    public float staminaFallRate;


    public Volume volume;
    private Vignette vignette;
    public float VignetteFallRate;

    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        VolumeProfile PostProcessing = volume.sharedProfile;
        volume.profile.TryGet(out vignette);

        CurrentStamina = MaxStamina;

        StaminaBar.maxValue = MaxStamina;
        StaminaBar.value = CurrentStamina;

        staminaFallRate = 1;
    }

    private void Update()
    {
        if (!unlimtedStamina)
        {
            if (!safeArea.inSafeArea)
            {
                Debug.Log("numnum");
                if(RB.velocity.magnitude >= 1)
                {
                    StaminaBar.value -= Time.deltaTime / staminaFallRate * staminaFallMult;
                    if(StaminaBar.value <= 50f)
                    {
                        vignette.intensity.value += Time.deltaTime / 70f;
                    }
                }

            }

            if(StaminaBar.value <= 0)
            {
                Debug.Log("i am tired");
            }
        }
    }
}
