using UnityEngine;

public class LightMapController : MonoBehaviour, ILookAroundSubscriber
{
    [SerializeField] private Texture2D[] _lightAO;
    [SerializeField] private Texture2D[] _dirAO;
    [SerializeField] private Texture2D[] _lightDirect;
    [SerializeField] private Texture2D[] _dirDirect;

    private LightmapData[] _aOData, _directData;

    private GameInput _gameInput;
    public void Init(GameInput gameInput)
    {
        return;
        _gameInput = gameInput;
        _gameInput.RegistrateLookAround(this);

        _aOData = new LightmapData[_lightAO.Length];
        for (int i = 0; i < _lightAO.Length; i++)
        {
            _aOData[i] = new LightmapData { lightmapColor = _lightAO[i], lightmapDir = _dirAO[i] };
        }

        _directData = new LightmapData[_lightDirect.Length];
        for (int i = 0; i < _lightDirect.Length; i++)
        {
            _directData[i] = new LightmapData { lightmapColor = _lightDirect[i], lightmapDir = _dirDirect[i] };
        }
    }
    public void OnLookAround(bool value)
    {
        if (value)
        {
            LightmapSettings.lightmaps = _aOData;
        }
        else
        {
            LightmapSettings.lightmaps = _directData;
        }
    }

    private void OnDestroy()
    {
        //_gameInput.UnregistrateLookAround(this);
    }
}
