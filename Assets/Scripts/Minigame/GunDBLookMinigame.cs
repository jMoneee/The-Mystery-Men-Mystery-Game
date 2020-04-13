using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDBLookMinigame : LookMinigameHandler
{
    public GameObject CompareObject;

    public override void HandleMovement()
    {
        for (int i = 0; i < lookObjects.Count; i++)
        {
            if (i == lookedAtObject)
            {
                lookObjects[i].gameObject.SetActive(true);
            }
            else
            {
                lookObjects[i].gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            lookedAtObject = lookedAtObject + 1 < lookObjects.Count ? lookedAtObject + 1 : 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            lookedAtObject = lookedAtObject > 0 ? lookedAtObject - 1 : lookObjects.Count - 1;
        }

    }

    public override void HandleMinigameDetails(MinigameListing details)
    {
        correctAnswer = details.correctIndex;
        correctText = details.correct;
        incorrectText = details.incorrect;

        CompareObject.GetComponent<Renderer>().material.mainTexture = textureFromSprite(details.objectMatch);
    }

    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }

}
