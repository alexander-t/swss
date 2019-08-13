using UnityEngine;

namespace AI
{
    public interface Behavior 
    {
        void Commence();
        void Turn();
        void Attack();
        string Describe();
    }
}
