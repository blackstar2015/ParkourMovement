using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParkourController : MonoBehaviour
{

    [BoxGroup("Parkour Action Area")]
    public List<NewParkourAction> NewParkourActions;
    
    [field: SerializeField, FoldoutGroup("Components")] private EnvironmentChecker _evironmentChecker;
    [field: SerializeField, FoldoutGroup("Components")] private Animator _animator;
    [field: SerializeField, FoldoutGroup("Components")] private PlayerController _playerController;
    
    private bool _playerInAction = false;

    private void OnValidate()
    {
        _evironmentChecker = GetComponent<EnvironmentChecker>();
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    public void OnJump(InputValue inputValue)
    {
        Debug.Log("asd");
        if(_playerInAction) { return; }
        ObstacleInfo hitData = _evironmentChecker.CheckObstacle();

        if (hitData.HitFound)
        {
            foreach (NewParkourAction action in NewParkourActions)
            {
                if(action.CheckIfAvailable(hitData, transform))
                {
                    StartCoroutine(PerformParkourAction(action));
                    break;
                }
            }

        }
    }

    IEnumerator PerformParkourAction(NewParkourAction action)
    {
        _playerInAction = true;

        _playerController.SetControl(false);

        _animator.CrossFade(action.AnimationName, .1f);
        yield return null;

        AnimatorStateInfo animationState = _animator.GetNextAnimatorStateInfo(0);
        if (!animationState.IsName(action.AnimationName)) Debug.Log("Animation name is incorrect");

        yield return new WaitForSeconds(animationState.length);
        _playerController.SetControl(true);
        _playerInAction = false;
    }
}
