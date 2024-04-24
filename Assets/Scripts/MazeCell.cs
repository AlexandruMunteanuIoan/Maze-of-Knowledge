using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    public GameObject _leftWall;

    [SerializeField]
    public GameObject _rightWall;

    [SerializeField]
    public GameObject _frontWall;

    [SerializeField]
    public GameObject _backWall;

    [SerializeField]
    public GameObject _unvisitedBlock;

    [SerializeField]
    public GameObject _backDecor;

    [SerializeField]
    public GameObject _frontDecor;

    public GameObject topDecorPlaceHolder;

    public bool IsVisited {get; private set;}
    public void SetDecor()
    {
        _frontDecor.SetActive(false);
        _backDecor.SetActive(false);
    }
    public void Visit()
    {
        IsVisited = true;
        _unvisitedBlock.SetActive(false);
    }
    public void ClearLeftWall()
    {
        _leftWall.SetActive(false);
        if (_frontWall.activeSelf && Random.Range(0, 10) == 0)
        {
            _frontDecor.SetActive(true);
        }

        if (_backWall.activeSelf && Random.Range(0, 10) == 0)
        {
            _backDecor.SetActive(true);
        }
    }

    public void ClearRightWall()
    {
        _rightWall.SetActive(false);
        if (_frontWall.activeSelf && Random.Range(0, 10) == 0)
        {
            _frontDecor.SetActive(true);
        }
        if (_backWall.activeSelf && Random.Range(0, 10) == 0)
        {
            _backDecor.SetActive(true);
        }
    }

    public void ClearFrontWall()
    {
        _frontWall.SetActive(false);
        _frontDecor.SetActive(false);
        if (_backWall.activeSelf && Random.Range(0, 10) == 0)
        {
            _backDecor.SetActive(true);
        }
    }

    public void ClearBackWall()
    {
        _backWall.SetActive(false);
        _backDecor.SetActive(false);
        if (_frontWall.activeSelf && Random.Range(0, 10) == 0)
        {
            _frontDecor.SetActive(true);
        }
    }

}
