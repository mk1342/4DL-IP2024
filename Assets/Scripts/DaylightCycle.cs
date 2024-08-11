using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaylightCycle : MonoBehaviour
{
    public Light directionalLight;      // The sun light
    public float dayDuration = 120f;     // Duration of a full day in seconds
    public Gradient lightColor;         // Gradient to control the light color over time
    public AnimationCurve lightIntensity; // Curve to control light intensity over time
    public Material skyboxMaterial;     // The skybox material
    public float startTime;
    public AnimationCurve cloudsIntensity;
    public Material cloudsLow;
    public Material cloudsHigh;

    private float timeOfDay = 0f;

    void Update()
    {
        // Calculate the current time of day
        timeOfDay += Time.deltaTime / dayDuration;
        if (timeOfDay > 1f)
        {
            timeOfDay -= 1f;  // Loop the cycle back to the start
        }

        // Rotate the sun (directional light) based on time of day
        float sunAngle = timeOfDay * 360 - startTime; // -90 to start at sunrise
        directionalLight.transform.rotation = Quaternion.Euler(sunAngle, -130f, -20);

        // Change the light color based on the time of day
        directionalLight.color = lightColor.Evaluate(timeOfDay);

        // Adjust the light intensity
        directionalLight.intensity = lightIntensity.Evaluate(timeOfDay);

        // Optionally, adjust the skybox exposure or tint
        if (skyboxMaterial != null)
        {
            RenderSettings.skybox.SetFloat("_Exposure", lightIntensity.Evaluate(timeOfDay));
            // You can also tweak other properties like _TintColor based on timeOfDay
        }
        Color colorLow = cloudsLow.GetColor("_Color");
        colorLow.a = lightIntensity.Evaluate(timeOfDay);
        cloudsLow.SetColor("_Color", colorLow);
        Color colorHigh = cloudsHigh.GetColor("_Color");
        colorHigh.a = lightIntensity.Evaluate(timeOfDay*3);
        cloudsHigh.SetColor("_Color", colorHigh);
    }
}
