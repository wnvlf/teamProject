using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Dice : MonoBehaviour
{
    public Image diceImage;

    [Header("사운드 및 이펙트")]
    public GameObject effectPrefab;
    public AudioClip rollSound;

    private DiceSkin _defaultSkin;
    public DiceAbility MyAbility { get; private set; }
    
    // 현재 눈금 저장
    public int CurrentValue { get; private set; }
    public void SetAbility(DiceAbility ability)
    {
        this.MyAbility = ability;
        // 다이스 기본 세팅
        UpdateDiceImage(1);
    }

    public void SetDefaultSkin(DiceSkin skin)
    {
        _defaultSkin = skin;
    }

    public void UpdateDiceImage(int value)
    {
        if(MyAbility != null && MyAbility.skin != null)
        {
            diceImage.sprite = MyAbility.skin.GetSprite(value);
        }
        else if(_defaultSkin != null)
        {
            diceImage.sprite = _defaultSkin.GetSprite(value);
        }
    }

    public void StartRoll(float duration)
    {
        StartCoroutine(ChangeImageDuringRoll(duration));
    }

    IEnumerator ChangeImageDuringRoll(float duration)
    {
        float timer = 0f;
        float switchinterval = 0.1f;

        while(timer < duration)
        {
            int randomValue = Random.Range(1, 7);
            UpdateDiceImage(randomValue);

            yield return new WaitForSeconds(switchinterval);
            timer += switchinterval;
        }
    }
    
    public void SetResult(int resultValue)
    {
        StopAllCoroutines();

        CurrentValue = resultValue;

        UpdateDiceImage(resultValue);
    }

    public Sprite GetCurrentSprite()
    {
        return diceImage.sprite;
    }
}
