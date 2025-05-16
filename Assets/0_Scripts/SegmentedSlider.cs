using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent (typeof(Slider))]
public class SegmentedSlider : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int segmentCount;
    [SerializeField] int currentActive;
    [SerializeField] float separation;
    [SerializeField] Color defaultColor;
    [SerializeField] Color activeColor;
    [SerializeField] float pixelsPerUnit;

    [Header("Events")]
    public UnityEvent<float> OnValueChange;

    [Header("Components")]
    [SerializeField] Image sliderSegment;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite activeSprite;
    [SerializeField] Transform holder;
    HorizontalLayoutGroup holderHLG;

    Slider slider;
    List<Image> segments = new();
    float internalSliderValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        holderHLG = holder.GetComponent<HorizontalLayoutGroup>();
    }
    private void Update()
    {
        if (segments.Count != segmentCount)
        {
            GenerateSlider();
        }

        if (holderHLG.spacing != separation) holderHLG.spacing = separation;
    }

    public void AddToSlider(int amount)
    {
        float step = (slider.maxValue - slider.minValue) / segmentCount;
        currentActive = currentActive + amount > segmentCount ?  segmentCount : currentActive = currentActive + amount < 0 ? 0 : currentActive + amount; ;  
        
        slider.value = slider.minValue + step * currentActive;

        SetSegmentsValues();
        OnValueChange.Invoke(slider.value);
    }
    public void SetValue(float amount)
    {
        if(slider) slider.value = amount;

        float step = (slider.maxValue - slider.minValue) / segmentCount;
        currentActive = Mathf.FloorToInt(amount / step);

        SetSegmentsValues();

        Debug.Log("VALUE SET TO " + slider.value);
    }
    public void SetValueToSegment(int index)
    {
        float step = (slider.maxValue - slider.minValue) / segmentCount;
        currentActive = index > segmentCount ? segmentCount : currentActive = index < 0 ? 0 : index; ;

        slider.value = slider.minValue + step * currentActive;

        SetSegmentsValues();
        OnValueChange.Invoke(slider.value);
    }

    void GenerateSlider()
    {
        foreach (Transform segment in holder.GetComponentsInChildren<Transform>())
        {
            if (segment != holder)
            {
                if (segment != sliderSegment.transform) Destroy(segment.gameObject);
                else segment.gameObject.SetActive(false);
            }

        }

        segments.Clear();

        for (int i = 0; i < segmentCount; i++)
        {
            int index = i;

            Image instance = Instantiate(sliderSegment, holder);
            segments.Add(instance);

            instance.GetComponent<Button>().onClick.AddListener(() => SetValueToSegment(index + 1));

            instance.pixelsPerUnitMultiplier = pixelsPerUnit;
        }

        SetSegmentsValues();
    }
    void SetSegmentsValues()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            if (i < currentActive)
            {
                segments[i].sprite = activeSprite;
                segments[i].color = activeColor;

            }
            else
            {
                segments[i].sprite = defaultSprite;
                segments[i].color = defaultColor;

            }
        }
    }
}
