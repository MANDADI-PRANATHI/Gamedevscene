using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class DayNightSystem : MonoBehaviour
{

    public Light directionalLight;
    public float dayDurationInSeconds = 24.0f;
    public int currentHour;
    float currentTimeOfDay=0.40f;
    public List<SkyboxTimeMapping>timeMappings;
    float blendedValue=0.0f;
    bool lockNextDayTrigger =false;

    public TextMeshProUGUI timeUI;

    void Update()
    {
        currentTimeOfDay +=Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay%=1;
        currentHour = Mathf.FloorToInt(currentTimeOfDay*24);

        timeUI.text = $"{currentHour}:00";
        directionalLight.transform.rotation =Quaternion.Euler(new Vector3((currentTimeOfDay * 360) - 90,170,0));

        UpdateSkybox();
    }
    private void UpdateSkybox()
    {
        Material currentSkybox = null;
        foreach(SkyboxTimeMapping mapping in timeMappings)
        {
            if(currentHour == mapping.hour)
            {
                currentSkybox=mapping.skyboxMaterial;
                if(currentSkybox.shader != null){
                if(currentSkybox.shader.name =="Custom/SkyboxTransition")
                {
                    blendedValue+=Time.deltaTime;
                    blendedValue =  Mathf.Clamp01(blendedValue);
                    currentSkybox.SetFloat("_TransitionFactor",blendedValue);
                }
                else
                {
                    blendedValue =0;
                }
                }
                break;
            }
        }

        if(currentHour == 0 && lockNextDayTrigger == false)
        {

            //TimeManager.Instance.TriggerNextDay();
            if(TimeManager.Instance !=null)
            {
                TimeManager.Instance.TriggerNextDay();
            }
            lockNextDayTrigger=true;
        }

        if(currentHour!=0)
        {
            lockNextDayTrigger=false;
        }



        if(currentSkybox !=null)
        {
            RenderSettings.skybox = currentSkybox;
        }

    }
}


[System.Serializable]
public class SkyboxTimeMapping
{
    public string phaseName;
    public int hour;
    public Material skyboxMaterial;
}