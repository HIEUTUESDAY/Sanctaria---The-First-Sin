using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTranform;

    [Header("Flip Rotation Status")]
    [SerializeField] private float _flipYRotationTime = 0.5f;

    private Coroutine _turnCoroutine;

    private bool _isFacingRight;

    private void Update()
    {
        Player _player = FindObjectOfType<Player>();

        _playerTranform = _player.transform;

        _isFacingRight = _player.IsFacingRight;

        //make camera follow the player position
        transform.position = _playerTranform.position;
    }

    public void CallTurn()
    {
        _turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startCoroutine = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < _flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            //Lerp the y rotation
            yRotation = Mathf.Lerp(startCoroutine, endRotationAmount, (elapsedTime / _flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;

        if(_isFacingRight)
        {
            return 0f;
        }
        else
        {
            return 180f;
        }
    }

}
