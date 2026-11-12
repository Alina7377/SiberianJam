using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoMenuControl : MonoBehaviour
{
    [SerializeField] private bool _isShowInfo = false;
    [SerializeField] private List<string> _moveText;
    [SerializeField] private GameObject _changeRegim;
    [SerializeField] private GameObject _changeModele;
    [SerializeField] private List<string> _interactLKM;
    [SerializeField] private List<string> _interactPKM;
    [SerializeField] private GameObject _containerInfo;
    [SerializeField] private TMP_Text _textShowInfo;
    [SerializeField] private TMP_Text _textLKMInfo;
    [SerializeField] private TMP_Text _textPKMInfo;
    [SerializeField] private TMP_Text _textMovementInfo;

    private string _showInfo = "X - показать подсказки";
    private string _dontShowInfo = "X - скрыть подсказки";
   // private bool _isShowInfo = false;
    private bool _isShereMode = true;
    private int _indexMode = -1;
    private int _maxIndMode = -1;

    public void ChangeShow()
    {
        _isShowInfo = !_isShowInfo;
        if (_isShowInfo)
        {
            _textShowInfo.text = _dontShowInfo;
        }
        else
        {
            _textShowInfo.text = _showInfo;
        }

        _containerInfo.SetActive(_isShowInfo);
    }


    public void CanUseChangeRegim(bool isCan, bool isCanView)
    {
        if ((isCan && (isCanView || _maxIndMode>-1)) || !isCan)
            _changeRegim.SetActive(isCan);
    }

    public void CanUseChangeModule(bool isCan)
    {
        _changeModele.SetActive(isCan);
    }


    public void CurrentRegime(bool isShereMode)
    {
        if (isShereMode)
        {
            _textMovementInfo.text = _moveText[0];
            _textLKMInfo.text = "";
            _textPKMInfo.text = "";
            _isShereMode = true;
            _changeModele.SetActive(false);
        }
        else
        {
            _textMovementInfo.text = _moveText[1];
            _isShereMode = false;
            if (_maxIndMode > 0)
                _changeModele.SetActive(true);
            CurrentModule(_indexMode);
        }

    }

    public void CurrentModule(int indexModul)
    {
        if (indexModul < 0)
        {
            _textLKMInfo.text = "";
            _textPKMInfo.text = "";
            return;
        }

        if (indexModul >= _interactLKM.Count)
            _indexMode = _interactLKM.Count - 1;
        else
            _indexMode = indexModul;

        if (_indexMode > _maxIndMode)
            _maxIndMode = _indexMode;    

        _textLKMInfo.text = _interactLKM[_indexMode];
        _textPKMInfo.text = _interactPKM[_indexMode];
    }
}
