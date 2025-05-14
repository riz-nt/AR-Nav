using Unity.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;
using UnityEngine.UI;
using UnityEngine.Android;


public class AppFlowController : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARSession arSession;
    [SerializeField] private XROrigin xrOrigin;
    [SerializeField] private ARCameraManager arCameraManager;

    [Header("UI Components")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject scanningPanel;
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject nextPanel;
    [SerializeField] private GameObject scanningIndicator;
    [SerializeField] private float successDelay = 2f;

    [Header("Dependencies")]
    [SerializeField] private QrCodeRecenter qrCodeRecenter;
    [SerializeField] private TargetHandler targetHandler;

    private void Start()
    {
        InitializeApplication();
    }

    private void InitializeApplication()
    {
        // Disable AR components initially
        arSession.gameObject.SetActive(false);
        xrOrigin.gameObject.SetActive(false);
        
        // Set up UI states
        FindFirstObjectByType<BackButtonManager>().ShowPanel(startPanel);

        // startPanel.SetActive(true);
        // scanningPanel.SetActive(false);
        // successPanel.SetActive(false);
        // nextPanel.SetActive(false);
    }

    public void OnStartButtonClicked()
    {
        #if UNITY_ANDROID
        RequestCameraPermission();
        #else
        InitializeARSession();
        #endif
    }

    private void RequestCameraPermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            var callbacks = new PermissionCallbacks();
            callbacks.PermissionGranted += _ => InitializeARSession();
            callbacks.PermissionDenied += _ => HandlePermissionDenied();
            Permission.RequestUserPermission(Permission.Camera, callbacks);
        }
        else
        {
            InitializeARSession();
        }
    }

    private void InitializeARSession()
    {
        startPanel.SetActive(false);
        scanningIndicator.SetActive(true);
        arSession.gameObject.SetActive(true);
        xrOrigin.gameObject.SetActive(true);
        
      
        
        Invoke(nameof(ShowNextPanel), successDelay);
    }
    private void ShowNextPanel()
    {
        
        FindFirstObjectByType<BackButtonManager>().ShowPanel(scanningPanel);

        scanningIndicator.SetActive(false);
        scanningPanel.SetActive(true);
        
        // Initialize QR scanner
        qrCodeRecenter.Initialize(arCameraManager, targetHandler);
        qrCodeRecenter.ToggleScanning();
    }

    private void HandlePermissionDenied()
    {
        // Show error message or retry UI
        Debug.LogError("Camera permission required for AR functionality");
    }
}