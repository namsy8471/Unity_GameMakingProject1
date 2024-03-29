using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VisualManager
{
    // This Manager will be manager visual effect like Image, Line Renderer, particle etc.
    // 이 매니저는 이미지나 라인 렌더러, 파티클 등의 시각 효과를 담당합니다.

    private GameObject _imageCanvas;
    
    private LineRenderer _lineRenderer;
    
    private GameObject _circleImage;
    private float _circleSize = 50f;
    private float _rotationOffset = 0;
    
    public void Init()
    {
        _imageCanvas = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/ImageCanvas"));

        TargetingCircleInit();
        LineRendererInit();

        #region Action Binding
        
        // These functions use in Targeting System.
        Managers.Game.TargetingSystem.SetDrawCircleAction(DrawCircleOnEnemy);
        Managers.Game.TargetingSystem.SetDrawLineAction(DrawLineToEnemy);
        Managers.Game.TargetingSystem.SetClearTargetingCircle(ClearTargetingCircle);
        Managers.Game.TargetingSystem.SetClearTargetingLineRenderer(ClearTargetingLineRenderer);
        
        #endregion
    }

    private void TargetingCircleInit()
    {
        _circleImage = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/TargetingCircle/TargetingCircle"),
            _imageCanvas.transform);
        
        _circleImage.SetActive(false);
    }

    private void LineRendererInit()
    {
        _lineRenderer = Managers.ManagersGO.AddComponent<LineRenderer>();
        _lineRenderer.material = Resources.Load<Material>("Materials/LineRenderer");
        _lineRenderer.startColor = Color.cyan;
        _lineRenderer.endColor = Color.cyan;
        _lineRenderer.startWidth = 0.05f;
        _lineRenderer.endWidth = 0.05f;
        _lineRenderer.positionCount = 0;

        _lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
    }

    private void DrawCircleOnEnemy(Vector3 currentTargetPos)
    {
        if (Managers.Game.TargetingSystem.Target == null) return;
        _circleImage.gameObject.SetActive(true);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTargetPos);

        _circleImage.transform.position = screenPos;

        _rotationOffset += 1;
        _circleImage.gameObject.transform.rotation = Quaternion.Euler(0, 0, _rotationOffset);
        
        float dist = (Managers.Game.TargetingSystem.Target.transform.position - Camera.main.transform.position).magnitude;
        float scaleFactor = _circleSize / dist;
        _circleImage.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f);
    }
    
    private void DrawLineToEnemy()
    {
        Vector3 mousePos = Managers.Ray.RayHitPoint;
        Vector3 targetPosition = Managers.Game.TargetingSystem.Target.transform.position;

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, mousePos);
        _lineRenderer.SetPosition(1, targetPosition);
    }

    private void ClearTargetingCircle()
    {
        _circleImage.SetActive(false);
    }

    private void ClearTargetingLineRenderer()
    {
        _lineRenderer.positionCount = 0;
    }
}
