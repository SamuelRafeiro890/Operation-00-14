using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class LampeTorche : MonoBehaviour
{

    [SerializeField] private AudioClip _sonLampeT;
    [SerializeField] Slider _slider;

    [SerializeField] Transform _persoTransform;
    [SerializeField] Collider2D _colliderLampe;
    [SerializeField] Light2D _pointLight2D;
    [SerializeField] SpriteRenderer _sr;

    private bool _lampeAllumee = true;
    private AudioSource _source;
    void Start()
    {
        _source = GetComponent<AudioSource>();
        StartCoroutine(BatterieLampeTorche());

        _pointLight2D = gameObject.GetComponent<Light2D>();
        _colliderLampe = gameObject.GetComponent<PolygonCollider2D>();
        _pointLight2D.intensity = 2.18f;
        _slider.value = 100;
    }
    void Update()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 positionCamGauche = new Vector2((_persoTransform.position.x + -0.1f), _persoTransform.position.y);
        Vector2 positionCamDroite = new Vector2((_persoTransform.position.x + 0.1f), _persoTransform.position.y);
        transform.position = positionCamDroite;
        float angle = AngleBetween2Vector2right(_persoTransform.position,cursorPos);

        if(cursorPos.x>_persoTransform.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, angle-90); 
            transform.position = positionCamDroite;
        }
        if(cursorPos.x<_persoTransform.transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, angle-90);
            transform.position = positionCamGauche;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            _lampeAllumee = !_lampeAllumee;
            if(_lampeAllumee) 
            {
                StartCoroutine(BatterieLampeTorche());
            }
            _source.PlayOneShot(_sonLampeT);
        }

        if(_slider.value <= 0 || _lampeAllumee == false)
        {
            _colliderLampe.enabled = false;
            _pointLight2D.intensity = 0;
            StopCoroutine(BatterieLampeTorche());
        }
        if(_slider.value >= 1 && _lampeAllumee)
        {
            _colliderLampe.enabled = true;
            _pointLight2D.intensity = 2.18f;
        }
    }
    public static float AngleBetween2Vector2right(Vector2 pos, Vector2 target)
    {
    if (target.y < pos.y)
        return Vector2.Angle(Vector2.right, target - pos) * -1.0f;
    else
        return Vector2.Angle(Vector2.right, target - pos);
    }

    IEnumerator BatterieLampeTorche()
    {
        while(true && VerifierLampeAllumee())
        {
            yield return new WaitForSeconds(0.1f);
            _slider.value -= 0.2f;
        }
    }
    
    private bool VerifierLampeAllumee()
    {
        return _lampeAllumee;
    }
}
