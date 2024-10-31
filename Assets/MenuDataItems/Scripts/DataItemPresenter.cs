using UnityEngine;

public class DataItemPresenter
{
    private DataItem _model;
    private DataItemView _view;

    public DataItemPresenter(GameObject prefab, Transform parent)
    {
        _view = GameObject.Instantiate(prefab, parent).GetComponent<DataItemView>();
    }

    public DataItemPresenter(GameObject prefab, DataItem model, Transform parent)
    {
        _model = model;
        _view = GameObject.Instantiate(prefab, parent).GetComponent<DataItemView>();
    }

    public DataItemPresenter(GameObject prefab, DataItem.CategoryType category, string description, bool special, Transform parent)
    {
        _model = new DataItem(category, description, special);
        _view = GameObject.Instantiate(prefab, parent).GetComponent<DataItemView>();
    }

    public void UpdateModel(DataItem model)
    {
        _model = new DataItem(model.Category, model.Description, model.Special);
    }

    public void UpdateModel(DataItem.CategoryType category, string description, bool special)
    {
        _model = new DataItem(category, description, special);
    }

    public void ShowData(int index)
    {
        _view.Setup((int)_model.Category, _model.Special, _model.Description, index.ToString());
        _view.Show();
    }

    public void HideData()
    {
        _view.Hide();
    }
}
