using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkrimpInterface;
using static Oracle;

public class PortalMover : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private SpriteRenderer portalSprite;
    [SerializeField] private SpriteRenderer flowerSprite;
    [SerializeField] private SpriteRenderer guacSprite;
    [SerializeField] private SpriteRenderer catSprite;
    [SerializeField] private ButtonHeld held;

    private void Start()
    {
        var position = transform.position;
        position.x = oracle.saveData.level.portalLocation;
        transform.position = position;
        SelectSkin();
        UpdatePortalSize();
    }

    private void OnEnable()
    {
        skrimpInterface.UpdateSkrimps += UpdatePortalSize;
    }

    private void OnDisable()
    {
        skrimpInterface.UpdateSkrimps -= UpdatePortalSize;
    }

    private void SelectSkin()
    {
        switch (oracle.saveData.preferences.skinSelection)
        {
            case SkinSelection.Random:
                SetSkin(1);
                break;
            case SkinSelection.Skrimp:
                SetSkin(1);
                break;
            case SkinSelection.Rain:
                SetSkin(2);
                break;
            case SkinSelection.BsGames:
                SetSkin(3);
                break;
            case SkinSelection.Avocado:
                SetSkin(4);
                break;
            case SkinSelection.Cat:
                SetSkin(5);
                break;
        }
    }

    private void SetSkin(int skin)
    {
        switch (skin)
        {
            case 1:
                portalSprite.gameObject.SetActive(true);
                break;
            case 2:
                flowerSprite.gameObject.SetActive(true);
                break;
            case 3:
                portalSprite.gameObject.SetActive(true);
                break;
            case 4:
                guacSprite.gameObject.SetActive(true);
                break;
            case 5:
                catSprite.gameObject.SetActive(true);
                break;
            default:
                goto case 1;
        }
    }

    private void UpdatePortalSize()
    {
        switch (oracle.saveData.preferences.skinSelection)
        {
            case SkinSelection.Random:
                ResizePortal();
                break;
            case SkinSelection.Skrimp:
                ResizePortal();
                break;
            case SkinSelection.Rain:
                ResizeFlowers();
                break;
            case SkinSelection.BsGames:
                ResizePortal();
                break;
            case SkinSelection.Avocado:
                ResizeGuac();
                break;
            case SkinSelection.Cat:
                ResizeCat();
                break;
        }


        var vector2 = GetComponent<BoxCollider2D>().size;
        vector2.x = skrimpInterface.level.portalSize;
        GetComponent<BoxCollider2D>().size = vector2;
    }

    private void ResizeCat()
    {
        var catSpriteSize = catSprite.size;
        catSpriteSize.x = skrimpInterface.level.portalSize + 0.4f;
        catSprite.size = catSpriteSize;
    }


    private void ResizeGuac()
    {
        var guacSpriteSize = guacSprite.size;
        guacSpriteSize.x = skrimpInterface.level.portalSize + 0.1f;
        guacSprite.size = guacSpriteSize;
    }

    private void ResizeFlowers()
    {
        var flowerBoxSpriteSize = flowerSprite.size;
        flowerBoxSpriteSize.x = skrimpInterface.level.portalSize;
        flowerSprite.size = flowerBoxSpriteSize;
    }

    private void ResizePortal()
    {
        var portalSpriteSize = portalSprite.size;
        portalSpriteSize.y = skrimpInterface.level.portalSize;
        portalSprite.size = portalSpriteSize;
    }

    private void Update()
    {
        if (!oracle.saveData.level.portalLocked) MovePortal();
    }

    public void SetPortalToCenter()
    {
        var transform1 = transform;
        var position = transform1.position;
        position.x = 0;
        transform1.position = position;
    }

    private void MovePortal()
    {
        var position = transform.position;
        var touchPosition = position;
        if (held.buttonHeld)
            touchPosition.x = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x;

        touchPosition.z = 0;


        position = Vector3.MoveTowards(position, touchPosition, movementSpeed * Time.deltaTime);
        transform.position = position;
    }
}