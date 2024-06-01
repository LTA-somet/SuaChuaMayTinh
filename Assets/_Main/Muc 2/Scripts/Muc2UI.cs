using System.Collections;
using System.Collections.Generic;
using Tu.Mohinh3D;
using UnityEngine;
using UnityEngine.UI;

public class Muc2UI : MonoBehaviour
{
    [SerializeField] Button _btnTheory;
    [SerializeField] Button _btnSimulation;
    private void Start()
    {
        _btnTheory.onClick.AddListener(ClickTheory);
        _btnSimulation.onClick.AddListener(ClickSimulation);
    }
    private void ClickSimulation()
    {
        MainUI mainUI = MainUI.Instance;
        mainUI.Theory.SetActive(false);
        mainUI.Simulation3D.SetActive(true);
        mainUI.Simulation3D.GetComponent<Simulation3DUI>().Muc3Simulate.OpenSlide(0);
        mainUI.Top.SetActive(true);
    }
    private void ClickTheory()
    {
        MainUI mainUI = MainUI.Instance;
        mainUI.Theory.SetActive(true);
        mainUI.Theory.GetComponent<TheoryUI>().Muc3UI.GetComponent<Muc3UI>().OpenSlide(0);
        mainUI.Simulation3D.SetActive(false);
        mainUI.Top.SetActive(true);
    }
    private void Reset()
    {
        _btnTheory = transform.Find("Btn Theory").GetComponent<Button>();
        _btnSimulation = transform.Find("Btn Simulation").GetComponent<Button>();
    }
}