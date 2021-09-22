using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicManager : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        dropdown.ClearOptions();
        List<string> options = new List<string>(QualitySettings.names);

        dropdown.AddOptions(options);
        dropdown.value = QualitySettings.GetQualityLevel(); ;
        dropdown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetGraphic(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
    }
}
