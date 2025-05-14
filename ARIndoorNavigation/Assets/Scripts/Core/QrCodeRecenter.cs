using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;
using ZXing;
using Unity.Collections;
using Unity.XR.CoreUtils;
using System.Collections;

public class QrCodeRecenter : MonoBehaviour
{
    
    [SerializeField] private ARSession arSession;
    [SerializeField] private XROrigin xrOrigin;
    [SerializeField] private ARCameraManager arCameraManager;
    [SerializeField] private GameObject scanningPanel;
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject nextPanel;
    [SerializeField] private float successDelay = 2f; 
    [SerializeField] private TargetHandler targetHandler;

    private Texture2D cameraImageTexture;
    private IBarcodeReader barcodeReader = new BarcodeReader();
    private bool isScanning = false;

    public void Initialize(ARCameraManager cameraManager, TargetHandler handler)
    {
        arCameraManager = cameraManager;
        targetHandler = handler;
    }

    private void OnEnable() => arCameraManager.frameReceived += OnCameraFrameReceived;
    private void OnDisable() => arCameraManager.frameReceived -= OnCameraFrameReceived;

    public void ToggleScanning()
    {
        isScanning = !isScanning;
        scanningPanel.SetActive(isScanning);
        
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        if (!isScanning || !arCameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            return;

        ProcessImage(image);
    }

    private void ProcessImage(XRCpuImage image)
    {
        var conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, image.width, image.height),
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),
            outputFormat = TextureFormat.RGBA32,
            transformation = XRCpuImage.Transformation.MirrorY
        };

        int bufferSize = image.GetConvertedDataSize(conversionParams);
        var buffer = new NativeArray<byte>(bufferSize, Allocator.Temp);

        image.Convert(conversionParams, buffer);
        image.Dispose();

        CreateTextureFromBuffer(buffer, conversionParams);
        buffer.Dispose();

        ScanQrCode();
    }

    private void CreateTextureFromBuffer(NativeArray<byte> buffer, XRCpuImage.ConversionParams parameters)
    {
        cameraImageTexture = new Texture2D(
            parameters.outputDimensions.x,
            parameters.outputDimensions.y,
            parameters.outputFormat,
            false);

        cameraImageTexture.LoadRawTextureData(buffer);
        cameraImageTexture.Apply();
    }

    private void ScanQrCode()
    {
        var result = barcodeReader.Decode(cameraImageTexture.GetPixels32(), 
            cameraImageTexture.width, cameraImageTexture.height);

        if (result != null)
            HandleQrDetection(result.Text);
    }

    private void HandleQrDetection(string qrText)
    {
        var target = targetHandler.GetCurrentTargetByTargetText(qrText);
        if (target == null) return;

        RecenterARSession(target);
        ShowSuccessUI();
        ToggleScanning();
    }


    private void RecenterARSession(TargetFacade target)
{
    StartCoroutine(SmoothRecenter(target));
}

private IEnumerator SmoothRecenter(TargetFacade target)
{
    arSession.Reset();  // Reset AR tracking

    yield return new WaitForSeconds(0.1f); // give AR time to reset

    xrOrigin.transform.SetPositionAndRotation(
        target.transform.position,
        target.transform.rotation
    );
}

    private void ShowSuccessUI()
    {
        

        successPanel.SetActive(true);
        scanningPanel.SetActive(false);
        
        Invoke(nameof(ShowNextPanel), successDelay);
    }

    private void ShowNextPanel()
    {
        FindFirstObjectByType<BackButtonManager>().ShowPanel(nextPanel);
        
       
        successPanel.SetActive(false);
        nextPanel.SetActive(true);
    }
} 
