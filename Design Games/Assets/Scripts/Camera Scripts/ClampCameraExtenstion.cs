using UnityEngine;
using Unity.Cinemachine;

[ExecuteAlways]
public class ClampCameraExtension : CinemachineExtension
{
    public float minX;
    public float maxX;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            Vector3 pos = state.RawPosition;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            state.RawPosition = pos;
        }
    }
}