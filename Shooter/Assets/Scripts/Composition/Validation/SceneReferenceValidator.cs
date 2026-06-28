using System;
using UnityEngine;

namespace Shooter.Composition
{
    public static class SceneReferenceValidator
    {
        public static T Require<T>(T reference, string owner, string fieldName) where T : UnityEngine.Object
        {
            if (reference == null)
            {
                throw new InvalidOperationException($"{owner} requires a scene reference for '{fieldName}'.");
            }

            return reference;
        }

        public static T RequireComponent<T>(Component root, string owner, string fieldName) where T : Component
        {
            if (root == null)
            {
                throw new InvalidOperationException($"{owner} requires a scene reference for '{fieldName}'.");
            }

            T component = root.GetComponent<T>();
            if (component == null)
            {
                throw new InvalidOperationException($"{owner} requires {typeof(T).Name} on '{fieldName}'.");
            }

            return component;
        }
    }
}
