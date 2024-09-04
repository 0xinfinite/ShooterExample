using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonViewSwitcher : MonoBehaviour
{
    public Transform character;
    public Renderer[] firstPersonRenderers;
    public Renderer[] thirdPersonRenderers;
    private Transform camTransform;
    public Vector3 facePosOffset;

    public float distanceThreshold = 1f;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main ? Camera.main.transform : Camera.allCameras[0].transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float distance = Vector3.Distance(character.TransformPoint(facePosOffset), camTransform.position);
        //Debug.Log(distance);
        if ( distance > distanceThreshold)
        {
            foreach(Renderer tp in thirdPersonRenderers) {
                tp.enabled = true;
            }
            foreach (Renderer fp in firstPersonRenderers)
            {
                fp.enabled = false;
            }
        }
        else
        {
            foreach (Renderer tp in thirdPersonRenderers)
            {
                tp.enabled = false;
            }
            foreach (Renderer fp in firstPersonRenderers)
            {
                fp.enabled = true;
            }
        }
    }
}
