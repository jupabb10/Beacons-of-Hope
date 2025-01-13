using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedAspectRatio : MonoBehaviour
{
    [SerializeField] private float targetAspectWidth = 9f;
    [SerializeField] private float targetAspectHeight = 16f;

    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        UpdateAspectRatio();
    }

    void Update()
    {
        // Actualiza si el tamaño de la ventana cambia
        if (Mathf.Abs(_camera.aspect - (targetAspectWidth / targetAspectHeight)) > 0.01f)
        {
            UpdateAspectRatio();
        }
    }

    private void UpdateAspectRatio()
    {
        // Calcula el aspect ratio deseado
        float targetAspect = targetAspectWidth / targetAspectHeight;
        float windowAspect = (float)Screen.width / Screen.height;

        if (Mathf.Approximately(windowAspect, targetAspect))
        {
            // Si el aspect ratio ya coincide, usamos el modo predeterminado
            _camera.rect = new Rect(0, 0, 1, 1);
            return;
        }

        // Ajuste de viewport
        if (windowAspect > targetAspect)
        {
            // Ventana más ancha que el target aspect
            float scaleHeight = targetAspect / windowAspect;
            _camera.rect = new Rect(0, (1 - scaleHeight) / 2, 1, scaleHeight);
        }
        else
        {
            // Ventana más alta que el target aspect
            float scaleWidth = windowAspect / targetAspect;
            _camera.rect = new Rect((1 - scaleWidth) / 2, 0, scaleWidth, 1);
        }
    }
}
