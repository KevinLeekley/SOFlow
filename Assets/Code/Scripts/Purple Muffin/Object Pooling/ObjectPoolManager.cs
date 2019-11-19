// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections.Generic;
using PurpleMuffin.Data.Primitives;
using UnityEngine;

namespace PurpleMuffin.ObjectPooling
{
    public class ObjectPoolManager<T> : ScriptableObject where T : Object, IPoolObject<T>
    {
	    /// <summary>
	    ///     The list of available objects.
	    /// </summary>
	    public List<T> AvailableObjects = new List<T>();

	    /// <summary>
	    ///     The initial object pool size per object ID.
	    /// </summary>
	    public IntField InitialPoolSizePerObject;

	    /// <summary>
	    ///     The object pool.
	    /// </summary>
	    public ObjectPool<T> ObjectPool;

	    /// <summary>
	    ///     The amount of object to instantiate when the pool runs out.
	    /// </summary>
	    public IntField PoolSizeExtensionAmount;

	    /// <summary>
	    ///     Initializes the pool.
	    /// </summary>
	    /// <param name="objectPool"></param>
	    /// <param name="poolContainer"></param>
	    public void InitializePool(ObjectPool<T> objectPool, Transform poolContainer)
        {
            ObjectPool                   = objectPool;
            ObjectPool.ObjectPoolManager = this;

            ObjectPool.Pool.Clear();

            foreach(T _object in AvailableObjects)
            {
                if(!ObjectPool.Pool.ContainsKey(_object.ID))
                    ObjectPool.Pool.Add(_object.ID, new List<T>(InitialPoolSizePerObject));

                for(int i = 0; i < InitialPoolSizePerObject; i++)
                    ObjectPool.Pool[_object.ID].Add(Instantiate(_object, poolContainer));
            }
        }

	    /// <summary>
	    ///     Generates a list of objects for the provided object ID.
	    /// </summary>
	    /// <param name="id"></param>
	    /// <param name="poolContainer"></param>
	    /// <returns></returns>
	    public List<T> GenerateObjects(int id, Transform poolContainer)
        {
            T objectToGenerate = AvailableObjects.Find(_object => _object.ID == id);

            if(objectToGenerate == null)
            {
                Debug.LogWarning($"Object with ID [{id}] not available in pool.");

                return null;
            }

            List<T> generatedObjects = new List<T>(PoolSizeExtensionAmount);

            for(int i = 0; i < PoolSizeExtensionAmount; i++)
                generatedObjects.Add(Instantiate(objectToGenerate, poolContainer));

            return generatedObjects;
        }
    }
}