using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CollecterController : Player
{
    private void Awake()
    {
        gameObject.name = "Collecter";
        type = PlayerType.Collecter;
        animator = GetComponent<Animator>();
    }
    public override async UniTask Action(Enemy enemy)
    {
        int getAnimationLength = 1000+200;
        if (enemy.type == EnemyType.Recyclable)
        {
            ManageSystem.instance.interactTimes.Add((float)getAnimationLength/1000 + 2.4f);
            SoundSystem.shouldPlayCollectSound = true;
            animator.SetTrigger("get");
            await UniTask.Delay(getAnimationLength);
            enemy.isAlive = false;
            ManageSystem.instance.energy++;
            DetachCell();
            await PlayCollectFinishAnimation();
            isAlive = false;
            Destroy(gameObject);
        }
        else if (enemy.type == EnemyType.Harmless)
        {
            ManageSystem.instance.interactTimes.Add((float)getAnimationLength/1000 + 2.4f);
            SoundSystem.shouldPlayCollectSound = true;
            animator.SetTrigger("get");
            await UniTask.Delay(getAnimationLength);
            enemy.isAlive = false;
            DetachCell();
            await PlayCollectFinishAnimation();
            isAlive = false;
            Destroy(gameObject);
        }
        else if (enemy.type == EnemyType.Radiation)
        {
            ManageSystem.instance.interactTimes.Add(0.8f);
            DetachCell();
            await PlayDeathAnimation();
            isAlive = false;
            Destroy(gameObject);
        }
    }

    private async UniTask PlayCollectFinishAnimation()
    {
        await transform.DOJump(transform.position, 0.5f, 2, 0.8f).AsyncWaitForCompletion();
        await transform.DOScaleX(-1, 0.4f).AsyncWaitForCompletion();
        GetComponent<SpriteRenderer>().DOColor(new Color(1,1,1,0), 0.6f).SetEase(Ease.InQuad);
        await transform.DOMoveX(-10f, 1.2f).SetEase(Ease.InQuad).AsyncWaitForCompletion();
    }

    private async UniTask PlayDeathAnimation()
    {
        float time = 0.8f;
        GetComponent<SpriteRenderer>().DOColor(Color.red, time);
        await transform.DORotate(new Vector3(0, 0, 90), time).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
    }
}
