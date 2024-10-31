using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuDataItems : MonoBehaviour
{
    [SerializeField] private GameObject _dataItemPrefab;
    [SerializeField] private LoadingWidget _loadingWidget;
    [SerializeField] private int _itemsPerPage;
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private Transform _buttonsParent;
    [SerializeField] private Button _buttonPrev;
    [SerializeField] private Button _buttonNext;

    private Dictionary<int,IList<DataItem>> _cachedData = new ();
    private int _serverDataAmount;
    private List<DataItemPresenter> _presenterList = new ();
    private int _currentIndex;
    private IDataServer _dataServer;

    public void Init(IDataServer dataServer)
    {
        _dataServer = dataServer;
        _buttonPrev.onClick.AddListener(OnPrevClicked);
        _buttonNext.onClick.AddListener(OnNextClicked);
        FetchDataFromServer(UpdateData);
    }

    private void OnPrevClicked()
    {
        _currentIndex -= _itemsPerPage;
        UpdateData();
    }

    private void OnNextClicked()
    {
        _currentIndex += _itemsPerPage;
        UpdateData();
    }

    private async void FetchDataFromServer(Action onDataAvaliable)
    {
        _loadingWidget.ShowLoading();
        var token = this.destroyCancellationToken;
        var dataAvailable = await _dataServer.DataAvailable(token);
        if (dataAvailable == _serverDataAmount)
        {
            _loadingWidget.HideLoading();
            onDataAvaliable?.Invoke();
            return;
        }

        _serverDataAmount = dataAvailable;
        await FetchDataPageFromServer(_currentIndex, _itemsPerPage, token, onDataAvaliable);
    }

    private async Task FetchDataPageFromServer(int index, int itemsPerPage, CancellationToken token, Action onDataAvaliable)
    {
        if (_serverDataAmount <= 0)
        {
            onDataAvaliable?.Invoke();
            return;
        }
        _loadingWidget.ShowLoading();
        var itemsToRequest = Mathf.Min(_serverDataAmount - index, itemsPerPage);
        var dataBlock = await _dataServer.RequestData(index, itemsToRequest, token);
        _cachedData[index] = dataBlock;
        _loadingWidget.HideLoading();
        onDataAvaliable?.Invoke();
    }

    private async void FetchCurrentDataPage()
    {
        await FetchDataPageFromServer(_currentIndex, _itemsPerPage, this.destroyCancellationToken, UpdateData);
    }

    private void UpdateData()
    {
        _buttonPrev.interactable = _currentIndex >= _itemsPerPage;
        _buttonNext.interactable = _currentIndex + _itemsPerPage < _serverDataAmount;

        if (_serverDataAmount <= 0)
        {
            return;
        }

        if (!_cachedData.ContainsKey(_currentIndex))
        {
            FetchCurrentDataPage();
            return;
        }

        if (_presenterList == null || _presenterList.Count < _itemsPerPage)
        {
            FillPresenterList();
        }

        var data = _cachedData[_currentIndex];

        for(int i = 0; i < _itemsPerPage; i++)
        {
            var item = i < data.Count ? data[i] : null;
            var presenter = _presenterList[i];
            if (item != null)
            {
                presenter.UpdateModel(item);
                var itemIndex = i + _currentIndex + 1;
                presenter.ShowData(itemIndex);
            }
            else
            {
                presenter.HideData();
            }
        }
    } 

    private void FillPresenterList()
    {
        if(_presenterList == null)
        {
            _presenterList = new List<DataItemPresenter>(_itemsPerPage);
        }

        while(_presenterList.Count < _itemsPerPage)
        {
            _presenterList.Add(new DataItemPresenter(_dataItemPrefab, _itemsParent));
        }

        _buttonsParent.SetAsLastSibling();
    }
}
