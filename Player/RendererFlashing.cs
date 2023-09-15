using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to make renderers flash
/// </summary>
public class RendererFlashing : MonoBehaviour
{
    //Renderers
    private MeshRenderer[] meshRenderers;
    private SkinnedMeshRenderer[] skinMeshRenderers;

    //timers
    private float flashTimer = 0;
    private static readonly float flashSpeed = 0.05f;

    /// <summary>
    /// Start, get all renderers in children
    /// </summary>
    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        skinMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    /// <summary>
    /// Update, activate and deactivate renderers
    /// </summary>
    void Update()
    {
        if (flashTimer > 0) {
            flashTimer = Mathf.Max(0, flashTimer - Time.deltaTime);

            int flash = Mathf.CeilToInt(flashTimer / flashSpeed);

            SetRendererState(flash % 2 == 0);
        }
    }

    /// <summary>
    /// Sets all renderers to a state.
    /// </summary>
    /// <param name="_active">active state</param>
    private void SetRendererState(bool _active)
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = _active;
        }

        foreach (SkinnedMeshRenderer renderer in skinMeshRenderers)
        {
            renderer.enabled = _active;
        }
    }

    /// <summary>
    /// Sets the flashing time.
    /// </summary>
    /// <param name="_time">flashing time</param>
    public void SetFlashTime(float _time)
    {
        flashTimer = _time;
    }
}
