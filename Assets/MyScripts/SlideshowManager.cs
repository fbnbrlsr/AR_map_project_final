using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideshowManager : MonoBehaviour
{     
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] Image imageContainer;
    [SerializeField] List<Sprite> images;
    int imageIndex;
    

    void Start()
    {
        leftButton.onClick.AddListener(OnLeftButtonClicked);
        rightButton.onClick.AddListener(OnRightButtonClicked);

        imageIndex = 0;
    }

    private void OnRightButtonClicked()
    {
        imageIndex = (imageIndex + 1) % images.Count;

        imageContainer.sprite = images[imageIndex];
    }

    private void OnLeftButtonClicked()
    {
        imageIndex = (imageIndex - 1) % images.Count;
        imageIndex = imageIndex<0 ? imageIndex+images.Count : imageIndex;

        imageContainer.sprite = images[imageIndex];
    }
}
