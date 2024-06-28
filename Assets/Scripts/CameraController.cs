using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class RoomNavigator : MonoBehaviour
{
  public Transform[] viewPoints;
  public Canvas[] roomUIs;        
  public Canvas startMenu;       
  public GameObject exitButton;   
  public Slider transitionSlider;   
  private int currentViewIndex = -1;
  private float transitionDuration = 2f; 

  void Start()
  {
    DeactivateAllUIs();
    exitButton.SetActive(true);
    startMenu.gameObject.SetActive(true);

    transitionSlider.onValueChanged.AddListener(UpdateTransitionTime);
  }

  void DeactivateAllUIs()
  {
    foreach (Canvas ui in roomUIs)
    {
      ui.gameObject.SetActive(false);
    }
  }

  public void BeginGame()
  {
    currentViewIndex = 0;
    MoveToView();
  }

  void UpdateTransitionTime(float value)
  {
    transitionDuration = value;
  }

  void MoveToView()
  {
    if (currentViewIndex >= 0 && currentViewIndex < viewPoints.Length && viewPoints[currentViewIndex] != null)
    {
      transform.DOMove(viewPoints[currentViewIndex].position, transitionDuration).SetEase(Ease.InOutSine);
      transform.rotation = viewPoints[currentViewIndex].rotation;

      ActivateUI(currentViewIndex);
    }
  }

  void ActivateUI(int index)
  {
    if (index >= 0 && index < roomUIs.Length)
    {
      DeactivateAllUIs();
      roomUIs[index].gameObject.SetActive(true);
    }
  }

  public void NextView()
  {
    currentViewIndex++;
    if (currentViewIndex >= viewPoints.Length)
    {
      currentViewIndex = 0;
      MoveToView();
      startMenu.gameObject.SetActive(true);

      foreach (Canvas ui in roomUIs)
      {
        if (ui != startMenu)
        {
          ui.gameObject.SetActive(false);
        }
      }

      RestartGame();
    }
    else
    {
      MoveToView();
    }

    exitButton.SetActive(true);
  }

  public void PreviousView()
  {
    currentViewIndex--;
    if (currentViewIndex < 0)
    {
      currentViewIndex = viewPoints.Length - 1;
    }
    MoveToView();
  }

  public void ExitApplication()
  {
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #else
    Application.Quit();
    #endif
  }

  void RestartGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
}
