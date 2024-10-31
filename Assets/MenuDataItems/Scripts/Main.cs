using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private GameObject _menuPrefab;

    // Start is called before the first frame update
    void Start()
    {
        var menu = Instantiate(_menuPrefab).GetComponent<MenuDataItems>();
        menu.Init(new DataServerMock());
    }
}
