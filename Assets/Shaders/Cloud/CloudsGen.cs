using UnityEngine;


[ExecuteInEditMode]
public class CloudsGen : MonoBehaviour
{
    public int cloudsResolution = 20;
    public float cloudsHeight;
    public Mesh cloudMesh;
    public Material cloudMaterial;
    private float offset;
    private Camera sceneCamera;
    private Matrix4x4 cloudsPosMatrix;
    public bool shadowCasting, shadowReceive, useLightProbes;

    private void OnEnable()
    {
        sceneCamera = Camera.main;
    }

    void Update()
    {
        var currentTransform = transform;
        cloudMaterial.SetFloat("CloudsWorldPos", currentTransform.position.y);
        cloudMaterial.SetFloat("CloudHeight", cloudsHeight);
        offset = cloudsHeight / cloudsResolution / 2f;
        var initPos = transform.position + (Vector3.up * (offset * cloudsResolution / 2f));
        for (var i = 0; i < cloudsResolution; i++)
        {
            //Take into consideration translation, rotation, scale of clouds-gen object
            cloudsPosMatrix = Matrix4x4.TRS(initPos - (Vector3.up * offset * i),
                currentTransform.rotation, currentTransform.localScale);

            //Push mesh data to render without editor overhead of managing multiple objects
            Graphics.DrawMesh(cloudMesh, cloudsPosMatrix, cloudMaterial, 0, sceneCamera, 0,
                null, shadowCasting, shadowReceive, useLightProbes);
        }
    }
}