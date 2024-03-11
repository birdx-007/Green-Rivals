using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerController : Player
{
    private void Awake()
    {
        gameObject.name = "Cleaner";
        type = PlayerType.Cleaner;
        animator = GetComponent<Animator>();
    }
    public override async UniTask Action(Enemy enemy)
    {
        int sprayAnimationLength = 1167+200;
        if (enemy.type == EnemyType.Pollution || enemy.type == EnemyType.Radiation)
        {
            ManageSystem.instance.interactTimes.Add((float)sprayAnimationLength/1000 + 0.8f);
            animator.SetTrigger("spray");
            await UniTask.Delay(sprayAnimationLength);
            enemy.isAlive = false;
            await PlayCleanFinishAnimation();
        }
        else if (enemy.type == EnemyType.Recyclable || enemy.type == EnemyType.Harmless)
        {
            ManageSystem.instance.interactTimes.Add(0.8f);
            DetachCell();
            await PlayDeathAnimation();
            isAlive = false;
            Destroy(gameObject);
        }
    }
    private async UniTask PlayCleanFinishAnimation()
    {
        await transform.DOJump(transform.position, 0.5f, 2, 0.8f).AsyncWaitForCompletion();
    }
    private async UniTask PlayDeathAnimation()
    {
        float time = 0.8f;
        GetComponent<SpriteRenderer>().DOColor(Color.red, time);
        await transform.DORotate(new Vector3(0, 0, -90), time).SetEase(Ease.OutBounce).AsyncWaitForCompletion();
    }
}
