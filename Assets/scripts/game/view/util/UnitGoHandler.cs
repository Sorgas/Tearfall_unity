using System;
using Leopotam.Ecs;
using TMPro;
using types;
using UnityEngine;

namespace game.view.util {
// TODO add status icons, thought cloud, 
// unit sprites should be faced left by default
public class UnitGoHandler : MonoBehaviour {
    private static readonly Vector3 spritePosWest = new(0, 0, 0);
    private static readonly Vector3 spritePosEast = new(1, 0, 0); // for restoring flipped sprites position
    private static readonly Vector3 spritePosBack = new(0, 0, 0.01f);
    private static readonly Vector3 spritePosFront = new(0, 0, -0.01f);

    // unit
    public RectTransform spriteHolder;
    public SpriteRenderer headSprite;
    public SpriteRenderer bodySprite;
    public SpriteMask mask;
    public TextMeshPro nameText;
    
    // progress bar
    public GameObject actionProgressBarHolder;
    public SpriteRenderer background;
    public SpriteRenderer progressBar;
    public const float progressBarFullSize = 0.75f;

    public EcsEntity unit; // for selecting with mouse

    // orientation
    private UnitOrientations orientation;

    // action animation
    public Animator actionAnimator;
    public Animator attackAnimator;

    public void updateSpriteSorting(int value) {
        if (!GlobalSettings.USE_SPRITE_SORTING_LAYERS) return;
        // headSprite.sortingOrder = value;
        // bodySprite.sortingOrder = value;
        // background.sortingOrder = value;
        // progressBar.sortingOrder = value;
        // mask.frontSortingOrder = value + 2;
        // mask.backSortingOrder = value + 1;
    }

    public void toggleProgressBar(bool enabled) {
        actionProgressBarHolder.SetActive(enabled);
        setProgress(0);
    }

    public void setProgress(float value) {
        Vector2 size = progressBar.size;
        size.x = value * progressBarFullSize;
        progressBar.size = size;
    }

    public void setMaskEnabled(bool value) {
        mask.enabled = value;
    }

    public void mirrorX(bool value) {
        headSprite.flipX = value;
        bodySprite.flipX = value;
    }

    // Unit can move in 8 directions, but sprites can be oriented only in 4 diagonal directions
    public void setOrientation(UnitOrientations orientation) {
        if (this.orientation == orientation) return;
        Debug.Log("updating orientation");
        rotateAttackAnimation(orientation);
        bool right = isRight(this.orientation, orientation);
        bool front = isFront(orientation);
        headSprite.flipX = right;
        bodySprite.flipX = right;
        this.orientation = orientation;
        if (right) {
            bodySprite.GetComponent<RectTransform>().anchoredPosition3D = spritePosEast;
            headSprite.GetComponent<RectTransform>().anchoredPosition3D = spritePosEast + (front ? spritePosFront : spritePosBack);
        } else {
            bodySprite.GetComponent<RectTransform>().anchoredPosition3D = spritePosWest;
            headSprite.GetComponent<RectTransform>().anchoredPosition3D = spritePosWest + (front ? spritePosFront : spritePosBack);
        }
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

    public void rotateAttackAnimation(UnitOrientations orientation) {
        int rotation = orientation switch {
            UnitOrientations.NW => 315,
            UnitOrientations.N => 270,
            UnitOrientations.NE => 225,
            UnitOrientations.E => 180,
            UnitOrientations.SE => 135,
            UnitOrientations.S => 90,
            UnitOrientations.SW => 45,
            UnitOrientations.W => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
        };
        gameObject.transform.rotation = Quaternion.Euler(0, 0, rotation); 
        spriteHolder.rotation = Quaternion.Euler(0, 0, 0); 
    }

    private bool isRight(UnitOrientations previous, UnitOrientations orientation) {
        bool east = UnitOrientationsUtil.isEast(orientation);
        bool west = UnitOrientationsUtil.isWest(orientation);
        if (!east && !west) {
            east = UnitOrientationsUtil.isEast(previous);
        }
        return east;
    }

    private bool isFront(UnitOrientations orientation) {
        bool north = UnitOrientationsUtil.isNorth(orientation);
        return !north;
    }
}
}