using UnityEngine;
using System.Collections;

public class UnderwaterEffect : MonoBehaviour
{
    //This script enables underwater effects. Attach to main camera.
    //Define variable
    public float underwaterLevel;

    //The scene's default fog settings
    private Camera camera;
    private bool defaultFog;
    private Color defaultFogColor;
    private float defaultFogDensity;
//  private Material defaultSkybox = RenderSettings.skybox;
//  private Material noSkybox;

    void Start()
    {
        camera = GetComponent<Camera>();
        underwaterLevel = GameObject.FindGameObjectWithTag("Ocean").transform.position.y;
        //Set the background color
        defaultFog = RenderSettings.fog;
        defaultFogDensity = RenderSettings.fogDensity;
        defaultFogColor = RenderSettings.fogColor;
        camera.backgroundColor = new Color(0, 0.4f, 0.7f, 1.0f);
    }

    void Update()
    {
        if (transform.position.y < underwaterLevel)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0, 0.4f, 0.7f, 0.6f);
            RenderSettings.fogDensity = 0.04f;
//          RenderSettings.skybox = noSkybox;
        }
        else
        {
            RenderSettings.fog = defaultFog;
            RenderSettings.fogColor = defaultFogColor;
            RenderSettings.fogDensity = defaultFogDensity;
//          RenderSettings.skybox = defaultSkybox;
        }
    }
}