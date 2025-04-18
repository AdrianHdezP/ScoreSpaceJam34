using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueVisualizer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;

    public void SetText(float sliderValue)
    {
        textMesh.text = (sliderValue * 100f).ToString("0");
    }
}
