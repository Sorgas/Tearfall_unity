using System;
using Leopotam.Ecs;
using types;
using UnityEngine;

namespace game.view.util {
// TODO add status icons, thought cloud, 
// unit sprites should be faced left by default
public class UnitGoHandler : MonoBehaviour {
    private static readonly Vector3 spritePos = new(0, 0, 0);
    private static readonly Vector3 spritePosFlipped = new(1, 0, 0);
    private static readonly Vector3 spritePosBack = new(0, 0, 0.01f);
    private static readonly Vector3 spritePosFront = new(0, 0, -0.01f);
    // unit
    private static readonly Vector3 DEFAULT_SCALE = new(0, 0.1f, 1);
    public SpriteRenderer headSprite;
    public SpriteRenderer bodySprite;
    public SpriteMask mask;

    // progress bar
    public SpriteRenderer background;
    public SpriteRenderer progressBar;
    public GameObject actionProgressBarHolder;
    public RectTransform actionProgressBar;

    public EcsEntity unit; // for selecting with mouse

    // orientation
    private SpriteOrientations orientation;

    // action animation
    public Animator actionAnimator;
    
    public void updateSpriteSorting(int value) {
        headSprite.sortingOrder = value;
        bodySprite.sortingOrder = value;
        background.sortingOrder = value;
        progressBar.sortingOrder = value;
        mask.frontSortingOrder = value + 2;
        mask.backSortingOrder = value + 1;
    }

    public void toggleProgressBar(bool enabled) {
        actionProgressBarHolder.SetActive(enabled);
        actionProgressBar.localScale = DEFAULT_SCALE;
    }

    public void setProgress(float value) {
        Vector3 scale = actionProgressBar.localScale;
        scale.x = value;
        if (Single.IsNaN(scale.x)) {
            Debug.LogError(value);
        }

        actionProgressBar.localScale = scale;
    }

    public void setMaskEnabled(bool value) {
        mask.enabled = value;
    }

    public void mirrorX(bool value) {
        headSprite.flipX = value;
        bodySprite.flipX = value;
    }

    // FL, FR, BL, BR
    public void setOrientation(SpriteOrientations orientation) {
        if (this.orientation == orientation) return;
        this.orientation = orientation;
        bool right = SpriteOrientationsUtil.isRight(orientation);
        bool front = SpriteOrientationsUtil.isFront(orientation);
        headSprite.flipX = right;
        bodySprite.flipX = right;
        if (right) {
            bodySprite.transform.localPosition = spritePosFlipped;
            headSprite.transform.localPosition = spritePosFlipped + (front ? spritePosFront : spritePosBack);
        } else {
            bodySprite.transform.localPosition = spritePos;
            headSprite.transform.localPosition = spritePos + (front ? spritePosFront : spritePosBack);
        }
        // Debug.Log("new orientation " + orientation);
    }

    // Standing straight orientation is N
    public void rotate(Orientations orientation) {
        RectTransform transform = headSprite.gameObject.GetComponent<RectTransform>();
        switch (orientation) {
            case Orientations.N:
                transform.localPosition = Vector3.zero;
                transform.rotation = Quaternion.Euler(Vector3.zero);
                break;
            case Orientations.S:
                transform.localPosition = new Vector3(1, 1, 0);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
            case Orientations.E:
                transform.localPosition = new Vector3(0, 1, 0);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                break;
            case Orientations.W:
                transform.localPosition = new Vector3(1, 0, 0);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
        }
    }

    public void setAnimation(string name) {
        
    }
}
}