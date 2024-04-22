using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;

    [SerializeField]
    private GameObject _rightWall;

    [SerializeField]
    private GameObject _frontWall;

    [SerializeField]
    private GameObject _backWall;

    [SerializeField]
    private GameObject _unvisitedBlock;

    [SerializeField]
    private GameObject _backDecor;

    [SerializeField]
    private GameObject _frontDecor;

    [SerializeField]
    private GameObject _columnDecor1;

    [SerializeField]
    private GameObject _columnDecor2;

    [SerializeField]
    private GameObject _columnDecor3;

    [SerializeField]
    private GameObject _columnDecor4;

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
