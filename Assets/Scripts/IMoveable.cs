using UnityEngine;

namespace DefaultNamespace
{
    public interface IMoveable
    {
        bool IsMoving { get; set; }
        char Symbol { get; set; }
        Transform Transf { get; set; }
    }
}