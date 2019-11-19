// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PurpleMuffin.ObjectPooling
{
    public abstract class ObjectPoolBase : ScriptableObject
    {
        /// <summary>
        ///     Gets the object pool enumerable of this instance.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable GetPool();
    }

    public class ObjectPool<T> : ObjectPoolBase where T : Object, IPoolObject<T>
    {
        /// <summary>
        ///     The pool.
        /// </summary>
        public Dictionary<int, List<T>> Pool = new Dictionary<int, List<T>>();

        /// <summary>
        ///     The object pool manager.
        /// </summary>
        public ObjectPoolManager<T> ObjectPoolManager
        {
            get;
            set;
        }

        /// <inheritdoc />
        public override IEnumerable GetPool()
        {
            return Pool;
        }

        /// <summary>
        ///     Gets an object from the pool with the provided ID.
        /// </summary>
        /// <param name="poolReference"></param>
        /// <param name="poolContainer"></param>
        /// <returns></returns>
        public T GetObjectFromPool(T poolReference, Transform poolContainer)
        {
            T       _object = default;
            List<T> poolObjects;

            if(Pool.TryGetValue(poolReference.ID, out poolObjects))
            {
                if(poolObjects.Count <= 0)
                {
                    List<T> generatedObjects = ObjectPoolManager.GenerateObjects(poolReference.ID, poolContainer);

                    if(generatedObjects != null)
                    {
                        poolObjects.AddRange(generatedObjects);

                        _object = poolObjects[0];
                        poolObjects.RemoveAt(0);
                    }
                }
                else
                {
                    _object = poolObjects[0];
                    poolObjects.RemoveAt(0);
                }
            }
            else
            {
                List<T> newPoolObject = new List<T>();
                Pool.Add(poolReference.ID, newPoolObject);

                List<T> generatedObjects = ObjectPoolManager.GenerateObjects(poolReference.ID, poolContainer);

                if(generatedObjects != null)
                {
                    newPoolObject.AddRange(generatedObjects);

                    _object = newPoolObject[0];
                    newPoolObject.RemoveAt(0);
                }
            }

            _object?.ActivateObject();

            return _object;
        }

        /// <summary>
        ///     Returns the given object to the pool.
        /// </summary>
        /// <param name="_object"></param>
        public void ReturnObjectToPool(T _object)
        {
            _object.DeactivateObject();
            List<T> poolObjects;

            if(Pool.TryGetValue(_object.ID, out poolObjects))
            {
                poolObjects.Add(_object);
            }
            else
            {
                List<T> newPoolObjects = new List<T>();
                Pool.Add(_object.ID, newPoolObjects);

                newPoolObjects.Add(_object);
            }
        }
    }
}