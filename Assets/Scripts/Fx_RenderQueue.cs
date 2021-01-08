using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fx_RenderQueue : MonoBehaviour
{

    public int m_RenderQueue;

    void Start()
    {
        MeshRenderer meshRenderer;
        Transform[] trans = transform.GetComponentsInChildren<Transform>();
        foreach (Transform tr in trans)
        {
            meshRenderer = tr.gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null && meshRenderer.sharedMaterial != null)
            {
                meshRenderer.sharedMaterial.renderQueue = m_RenderQueue;
            }
        }
    }
}
