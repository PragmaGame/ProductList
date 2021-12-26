using UnityEngine;

namespace Core
{
    public class FactoryCell : MonoBehaviour
    {
        [SerializeField] private Transform _containerCell;
        [SerializeField] private Product productPrefab;

        public Product GetCell()
        {
            return Instantiate(productPrefab, _containerCell);
        }
    }
}