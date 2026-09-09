using UnityEngine;
using UnityEngine.Rendering;

public class LightMapController : MonoBehaviour, ILookAroundSubscriber
{
    [SerializeField] private bool _isUseScenario = false;
    [SerializeField] private string _dayScenarioName = "Day";
    [SerializeField] private string _nightScenarioName = "Night";
    
    [SerializeField][Range(0f, 1f)] private float _timeOfDay = 0.25f;
    [SerializeField] private float _timeSpeed = 0.02f;
    [SerializeField] private Light _sunLight;
    [SerializeField] private float _timeStep = 0.5f;
    private float _startTime;
    private GameInput _gameInput;
    private ProbeReferenceVolume _probeVolume;

    public void OnEnable()
    {
        if (_isUseScenario)
        {
            _probeVolume = ProbeReferenceVolume.instance;
            _probeVolume.lightingScenario = _nightScenarioName;
            _probeVolume.lightingScenario = _nightScenarioName;
            _sunLight.enabled = false;
        }
    }

    public void Init(GameInput gameInput)
    {
        _gameInput = gameInput;
        _gameInput.RegistrateLookAround(this);
        _startTime = 0;
        if (!_isUseScenario) return;
    }

    private void Update()
    {
        return;
        if (_probeVolume == null)
        {
            return;
        }
        _startTime += Time.deltaTime;
        if (_startTime < _timeStep)
        {
            return;
        }
        else
        {
            _startTime = 0;
        }

        _timeOfDay += Time.deltaTime * _timeSpeed;
        if (_timeOfDay > 1f)
        {
            _timeOfDay = 0f;
        }

        //float sunAngle = (_timeOfDay * 360f) - 135;
        //_sunLight.transform.localRotation = Quaternion.Euler(sunAngle, 0, 0);

        float blendWeight = 0f;

        if (_timeOfDay >= 0.25f && _timeOfDay <= 0.75f)
        {
            blendWeight = Mathf.Abs(_timeOfDay - 0.5f) / 0.25f;
        }
        else
        {
            blendWeight = 1f;
        }

        blendWeight = Mathf.SmoothStep(0f, 1f, blendWeight);

        _probeVolume.BlendLightingScenario(_nightScenarioName, blendWeight);
    }

    public void OnLookAround(bool value)
    {
        if (!_isUseScenario) return;
        if (!value)
        {

            _probeVolume.BlendLightingScenario(_nightScenarioName, 0);
            _sunLight.enabled = true;
            //_timeOfDay -= 1;
        }
        else
        {
            _probeVolume.BlendLightingScenario(_nightScenarioName, 1);
            _sunLight.enabled = false;
            //_timeOfDay += 1;
        }
    }

    private void OnDestroy()
    {
        _gameInput.UnregistrateLookAround(this);
    }
}
