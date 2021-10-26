using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perso : MonoBehaviour
{

    [SerializeField] public float _vitesse = 5f;
    [SerializeField] private float _saut = 1.5f;
    [SerializeField] private int _nbFramesMaxSaut = 10;
    [SerializeField] private LayerMask _layerPlancher;
    [SerializeField] private Vector2 _position;
    [SerializeField] private Vector2 _grosseurRect;
    private GameObject _cle;
    private GameObject _porte;
    private GameObject _energie;
    private float _axeHorizontal;
    private int _nbFramesActSaut = 0;
    private bool _possedeCle = false;
    private bool _auSol = false;
    private bool _veutSauter;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    public static Perso instance;
    private Animator _anim;
    private Vector3 _axis;
    
    void Start()
    {
        
        _cle = GameObject.FindGameObjectWithTag("Cle");
        _porte = GameObject.FindGameObjectWithTag("Porte");
        _energie = GameObject.FindGameObjectWithTag("Energie");
        instance = this;
        _grosseurRect = new Vector2(0.1f,0.15f);
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        _axeHorizontal =  Input.GetAxis("Horizontal");
        _veutSauter = Input.GetButton("Jump");
        _axis.x = Input.GetAxis("Horizontal");
        _anim.SetFloat("vitesseX", _axis.x);

        if (_axeHorizontal<0)
        {
            _sr.flipX = true;
        }
        else if (_axeHorizontal>0)
        {
            _sr.flipX = false;
        }
    }
    
    void FixedUpdate()
    {
        VerifierSol();
        _rb.velocity = new Vector2 (_axeHorizontal*_vitesse, _rb.velocity.y);
        if ((_auSol && _nbFramesActSaut == 1 && _veutSauter) || (_nbFramesActSaut < 1 && _nbFramesActSaut <= _nbFramesMaxSaut && _veutSauter))
        {
            int nbFramesSautClamp = Mathf.Clamp(_nbFramesActSaut,0,_nbFramesMaxSaut);
            float puissance = (float)(_nbFramesMaxSaut-nbFramesSautClamp)/_nbFramesMaxSaut;
            _rb.AddForce(Vector2.up*_saut*puissance);
            _nbFramesActSaut++;
        }
        else if (_auSol == true)
        {
            _nbFramesActSaut = 0;
        }
        if(_veutSauter)
        {
            /*Debug.Log("Je veux sauter = " + _veutSauter + " Je suis au sol = " + _auSol + " nombre de frames " + _nbFramesActSaut);*/
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_position, _grosseurRect);
    }

    private void VerifierSol()
    {
        Collider2D col = Physics2D.OverlapBox(_position, _grosseurRect, 0, _layerPlancher);
        _position = new Vector2(_rb.position.x, (_rb.position.y-0.45f));
        _auSol = col;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Cle"))
        {
            _possedeCle = true;
            Destroy(_cle);
        }
        if(other.CompareTag("Porte") && _possedeCle == true)
        {
            ChangerScene.instance.Aller("Scene_2_Samuel");
        }
        if(other.CompareTag("Energie"))
        {
            _vitesse *= 2;
            Destroy(_energie);
            Invoke("StopEnergie",3f);
        }
    }

    private void StopEnergie()
    {
        _vitesse /= 2;
    }
}
