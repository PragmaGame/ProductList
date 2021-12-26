using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class Product : MonoBehaviour
    {
        [SerializeField] private Button _productButton;
        
        [SerializeField] private Text _mameText;
        [SerializeField] private Text _typeText;
        [SerializeField] private Text _priceText;

        private ProductData _productData;
        
        public event Action<Product> ProductSelectedEvent;
        
        public ProductData ProductData => _productData;

        private void OnEnable()
        {
            _productButton.onClick.AddListener(OnProductSelected);
        }

        private void OnDisable()
        {
            _productButton.onClick.RemoveListener(OnProductSelected);
        }

        public void SetProductData(ProductData productData)
        {
            _productData = productData;
            _mameText.text = productData.name;
            _typeText.text = productData.type;
            _priceText.text = productData.price + " BYN";
        }

        private void OnProductSelected()
        {
            ProductSelectedEvent?.Invoke(this);
        }
    }
}