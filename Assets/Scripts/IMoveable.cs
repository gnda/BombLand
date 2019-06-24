using UnityEngine;

namespace DefaultNamespace
{
    public interface IMoveable
    {
        bool IsMoving { get; set; }
        bool IsDestroyed { get; set; }
        float MoveDuration { get; set; }
        Transform Transf { get; set; }
    }
}