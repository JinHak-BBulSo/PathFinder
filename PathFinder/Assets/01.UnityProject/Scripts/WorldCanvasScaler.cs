using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WorldCanvasScaler : MonoBehaviour
{
    private Canvas worldCanvas = default;
    private Vector2 cameraSize = default;

    [SerializeField]
    private Vector2 canvasAspect = default;

    void Start()
    {
        worldCanvas = gameObject.GetComponent<Canvas>();
        cameraSize = GFunc.GetCameraSize();

        Vector2 canvasSize = worldCanvas.gameObject.GetRectSizeDelta();

        // ī�޶� �������� ĵ���� ������ ������ ũ�� �� ���Ѵ�.
        // width�� height �� �� �ϳ��� ������ ������ �����Ѵ�
        canvasAspect.x = cameraSize.x / canvasSize.x;
        canvasAspect.y = canvasAspect.x;

        // ���� ĵ������ ���� �������� ���ؼ� ������ ������ ����
        worldCanvas.transform.localScale = canvasAspect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
