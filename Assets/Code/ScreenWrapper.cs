using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        TeleportIfOutOfBounds();
    }
    
    private void TeleportIfOutOfBounds()
    {
        var screenPosition = _camera.WorldToScreenPoint(transform.position);
        
        //Teleport to the other side of the screen when leaving the screen
        if(screenPosition.x < 0)
            screenPosition.x = Screen.width;
        
        if(screenPosition.y < 0)
            screenPosition.y = Screen.height;
        
        if(screenPosition.x > Screen.width)
            screenPosition.x = 0;
        
        if(screenPosition.y > Screen.height)
            screenPosition.y = 0;
        
        transform.position = _camera.ScreenToWorldPoint(screenPosition);
    }
}
