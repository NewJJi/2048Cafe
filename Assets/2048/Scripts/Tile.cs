﻿using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

// a sound controller is needed
[RequireComponent(typeof(AudioSource))]
public class Tile : MonoBehaviour 
{
	public AudioClip FX;

	public int tileValue = 2;

	public bool combined;
	
    public int gridX;
    public int gridY;

    Image image;
    public RectTransform rectTransform;

    private void Awake()
    {
        image = this.GetComponent<Image>();
        rectTransform = this.GetComponent<RectTransform>();
        tileValue = 2;
    }

    public void SetGrid(int x, int y)
    {
        gridX = x;
        gridY = y;
    }

    public void Change(int newValue, Sprite sprite)
    {
        tileValue = newValue;
        image.sprite = sprite;
    }

    public void GrowTile(Vector2 size)
    {
        if(rectTransform == null)
        {
            image = this.GetComponent<Image>();
            rectTransform = this.GetComponent<RectTransform>();
        }
        rectTransform.sizeDelta = size / 2;
        rectTransform.DOSizeDelta(size, 0.5f);
    }
}