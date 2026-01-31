using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool IsTransparent = false;
    public bool BeingLookedAt = false;
    private Renderer _renderer;

    private float _transitionStartTime;
    private float _transitionDuration = 1.5f;
    private bool _isTransitioning = false;
    private float _targetTransition;

    private void Start() => _renderer = GetComponent<Renderer>();
    public void HandleTransparency()
    {
        bool shouldBeTransparent = BeingLookedAt;

        if (shouldBeTransparent && !IsTransparent)
        {
            SetTransparency(true);
            IsTransparent = true;
        }
        else if (!shouldBeTransparent && IsTransparent)
        {
            SetTransparency(false);
            IsTransparent = false;
        }
    }

    private void Update()
    {
        HandleTransparency();

        if (_isTransitioning)
        {
            float elapsed = Time.time - _transitionStartTime;
            float progress = Mathf.Clamp01(elapsed / _transitionDuration);

            float currVal = _renderer.material.GetFloat("_Transition");
            float newVal = Mathf.Lerp(currVal, _targetTransition, progress);

            _renderer.material.SetFloat("_Transition", newVal);

            if (progress >= 1f)
            {
                _isTransitioning = false;
                _renderer.material.SetFloat("_Transition", _targetTransition);
            }
        }
    }

    private void SetTransparency(bool makeTransparent)
    {
        _targetTransition = makeTransparent ? .4f : 0f;
        _transitionStartTime = Time.time;
        _isTransitioning = true;
    }
}