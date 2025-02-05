using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject _continentBox;
    [Header("Colors")]
    [SerializeField] private Color _neutralColor;
    [SerializeField] private Color _authoritarianismColor;
    [SerializeField] private Color _democracyColor;
    [SerializeField] private Color _conspiracyColor;
    [SerializeField] private Color _scienceColor;
    // ***** ***** *****
    private List<Image> _neutralProvinceList = new List<Image>();
    private List<Image> _authoritarianismProvinceList = new List<Image>();
    private List<Image> _democracyProvinceList = new List<Image>();
    private List<Image> _conspiracyProvinceList = new List<Image>();
    private List<Image> _scienceProvinceList = new List<Image>();


}
