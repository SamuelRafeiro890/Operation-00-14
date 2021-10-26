using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// using UnityEngine.Tilemaps;

/// <summary>
/// La classe qui gere l'instantiation des salles et objets differents et de leur placement
/// </summary>
public class GestNiveau : MonoBehaviour
{
    [SerializeField] private GameObject[] _tPrefabSalles;
    private Vector2Int _tailleNiveau = new Vector2Int(6,2);
    public Vector2Int tailleNiveau => _tailleNiveau;
    private int _salleChoisieObjets;

    private UnityEvent _evenementBbAraignee = new UnityEvent();
    public UnityEvent evenementBbAraignee => _evenementBbAraignee;

    private UnityEvent _evenementLevier = new UnityEvent();
    public UnityEvent evenementLevier => _evenementLevier; 

    private UnityEvent _evenementOxygene = new UnityEvent();
    public UnityEvent evenementOxygene => _evenementOxygene;

    private UnityEvent _evenementViePlus = new UnityEvent();
    public UnityEvent evenementViePlus => _evenementViePlus;

    private UnityEvent _evenementVieMoins = new UnityEvent();
    public UnityEvent evenementVieMoins => _evenementVieMoins;

    private UnityEvent _evenementVieMoinsE = new UnityEvent();
    public UnityEvent evenementVieMoinsE => _evenementVieMoinsE; 

    private static GestNiveau _instance;
    public static GestNiveau instance => _instance; 
    
    [SerializeField] private GameObject _perso; 
    public GameObject Perso => _perso;

    private List<GestSalle> _lesGestSalle = new List<GestSalle>();
    private int _dimensions;
    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _salleChoisieObjets = Random.Range(0, 2);
        _dimensions = GestSalle.tailleSalle - 1;

        if (_tPrefabSalles != null)
        {
            _dimensions = GestSalle.tailleSalle - 1;
            for (int i = 0; i < _tailleNiveau.x; i++)
            {
                for (int e = 0; e < _tailleNiveau.y; e++)
                {
                    Vector2 pos = new Vector3(i * _dimensions, e * _dimensions, 0) + transform.position;
                    int salleChoisie = Random.Range(0, _tPrefabSalles.Length);
                    GameObject salle = Instantiate(_tPrefabSalles[salleChoisie], transform);
                    salle.transform.position = pos;
                    salle.name = "Salle_" + e + i;
                    GestSalle leGestSalle = salle.GetComponent<GestSalle>();
                    _lesGestSalle.Add(leGestSalle);
                    if(i == 0) leGestSalle.FermerExtremite(Direction.gauche);
                    if(e == 0) leGestSalle.FermerExtremite(Direction.bas);
                    if(i == _tailleNiveau.x-1) leGestSalle.FermerExtremite(Direction.droite);
                    if(e == _tailleNiveau.y-1) leGestSalle.FermerExtremite(Direction.haut);
                }
            }
            _lesGestSalle[DeciderSalleCle()].PlacerLaCle();
            _lesGestSalle[DefinirSallePorte()].PlacerLaPorte();
            _lesGestSalle[Random.Range(0, _tailleNiveau.x * _tailleNiveau.y)].PlacerEnergie();
            _lesGestSalle[Random.Range(0, _tailleNiveau.x * _tailleNiveau.y)].PlacerLevier();
            for (int i = 0; i < 3; i++)
            {
                _lesGestSalle[Random.Range(0, _tailleNiveau.x * _tailleNiveau.y)].PlacerPuce();
                _lesGestSalle[Random.Range(0, _tailleNiveau.x * _tailleNiveau.y)].PlacerMCarte();
                Oxygene();
            }
        }
        else
        {
            Debug.LogError("Vous devez faire le lien avec le prefab de la salle.");
        }

    }
    private void Vie()
    {
        _lesGestSalle[Random.Range(0, _tailleNiveau.x * _tailleNiveau.y)].PlacerSteak();
    }
    
    private void Oxygene()
    {
        _lesGestSalle[Random.Range(0, _tailleNiveau.x * _tailleNiveau.y)].PlacerOxygene();
    }

    private int DeciderSalleCle()
    {
        int indexCle = 0;
        if(_salleChoisieObjets == 0)
        {
            indexCle = _tailleNiveau.y-1;
        }
        if(_salleChoisieObjets == 1)
        {
            indexCle = _lesGestSalle.Count-_tailleNiveau.y;
        }
        return indexCle;
    }

    private int DefinirSallePorte()
    {
        int indexPorte = 0;
        if(_salleChoisieObjets == 0)
        {
            indexPorte = _lesGestSalle.Count-1;
        }
        return indexPorte;
    }
}
