using UnityEngine;

namespace DefaultNamespace
{
    public interface IMoveable
    {
        bool IsMoving { get; set; }
        char Symbol { get; set; }
        float MoveDuration { get; }
        Transform Transf { get; set; }
    }
}