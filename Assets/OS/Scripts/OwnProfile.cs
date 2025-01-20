using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OwnProfile : MonoBehaviour
{
    public RawImage Pfp;
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] TextMeshProUGUI date;
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI timeAMPM;
    [SerializeField] TextMeshProUGUI description;
    void Start()
    {
        Pfp.texture = FileBrowserUpdate.instance.rawImage.texture;
        username.text = Login.instance.user1.text;
        description.text = Login.instance.description.text;
    }

    void Update()
    {
        time.text = System.DateTime.Now.ToString("hh:mm");
        timeAMPM.text = System.DateTime.Now.ToString("tt");
        date.text = System.DateTime.Now.ToString("dd/MM/yyyy");
    }
}
