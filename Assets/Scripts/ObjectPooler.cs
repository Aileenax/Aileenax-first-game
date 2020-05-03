using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;

    [SerializeField]
    private GameObject _enemyToPool;

    [SerializeField]
    private GameObject _powerupToPool;

    [SerializeField]
    private GameObject _projectileToPool;

    [SerializeField]
    private int _enemyAmountToPool;

    [SerializeField]
    private int _powerupAmountToPool;

    [SerializeField]
    private int _projectileAmountToPool;

    private List<GameObject> _pooledEnemies;
    private List<GameObject> _pooledPowerups;
    private List<GameObject> _pooledProjectiles;
    private List<GameObject> _pooledListToUse;

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Loop through list of pooled objects, deactivating them and adding them to the list 
        _pooledEnemies = new List<GameObject>();
        _pooledPowerups = new List<GameObject>();
        _pooledProjectiles = new List<GameObject>();

        for (int i = 0; i < _enemyAmountToPool; i++)
        {
            GameObject obj = Instantiate(_enemyToPool);
            obj.SetActive(false);
            _pooledEnemies.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager
        }

        for (int i = 0; i < _powerupAmountToPool; i++)
        {
            GameObject obj = Instantiate(_powerupToPool);
            obj.SetActive(false);
            _pooledPowerups.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager
        }

        for (int i = 0; i < _projectileAmountToPool; i++)
        {
            GameObject obj = Instantiate(_projectileToPool);
            obj.SetActive(false);
            _pooledProjectiles.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager
        }
    }

    public GameObject GetPooledObject(string pooledObject)
    {
        if (pooledObject == "Enemy")
        {
            _pooledListToUse = _pooledEnemies;
        }
        else if (pooledObject == "Powerup")
        {
            _pooledListToUse = _pooledPowerups;
        }
        else if (pooledObject == "Projectile")
        {
            _pooledListToUse = _pooledProjectiles;
        }

        // For as many objects as are in the pooledObjects list
        for (int i = 0; i < _pooledListToUse.Count; i++)
        {
            // if the pooled objects is NOT active, return that object 
            if (!_pooledListToUse[i].activeInHierarchy)
            {
                return _pooledListToUse[i];
            }
        }

        // otherwise, return null   
        return null;
    }
}
