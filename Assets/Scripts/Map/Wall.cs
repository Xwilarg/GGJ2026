using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool IsTransparent = false;
    public bool BeingLookedAt = false;
    public void HandleTransparency()
    {
        if (IsTransparent && BeingLookedAt)
        {
            print("Making wall transparent");
        }
        else
        {
            print("Making wall opaque");
        }
    }
}
