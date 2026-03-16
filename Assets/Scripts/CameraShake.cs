using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float duration = 1f;
    public AnimationCurve curve;
    [SerializeField] private Camera cam;

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Shake(Camera cam)
    {
        Vector2 startPos = cam.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float strength = curve.Evaluate(elapsed/ duration);
            cam.transform.position = startPos + Random.insideUnitCircle *strength;
            yield return null;
        }

        cam.transform.position = startPos; 
    }
}
