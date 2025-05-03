using UnityEngine;

namespace Atom
{
    public class GameModeContainer : MonoSingleton<GameModeContainer>
    {
        private GameObject _managers;
        public GameObject Manager
        {
            get
            {
                return _managers;
            }
        }

        public void InitGame()
        {
            _managers = new GameObject("Managers");
        }

        public void Clean()
        {
            DeleteAllChild(_managers.transform);
        }

        public void DeleteAllChild(Transform container)
        {
            foreach (Transform child in container)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}