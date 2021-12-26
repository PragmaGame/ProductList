using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class ViewEditProduct : MonoBehaviour
    {
        [SerializeField] private Button _saveProductButton;

        [SerializeField] private Text _startName;
        [SerializeField] private Text _startType;
        [SerializeField] private Text _startPrice;
        
        [SerializeField] private Text _nameEdit;
        [SerializeField] private Text _typeEdit;
        [SerializeField] private Text _priceEdit;
        
        public event Action<ProductData> SaveProductEvent; 
        
        private void OnEnable()
        {
            _saveProductButton.onClick.AddListener(OnClickSave);
        }

        private void OnDisable()
        {
            _saveProductButton.onClick.RemoveListener(OnClickSave);
        }

        private void OnClickSave()
        {
            var productData = new ProductData(_nameEdit.text, _typeEdit.text, Convert.ToInt32(_priceEdit.text));
            SaveProductEvent?.Invoke(productData);
        }

        public void Active()
        {
            _startName.text = "Name...";
            _startType.text = "Type...";
            _startPrice.text = "Price...";
        }

        public void Active(ProductData product)
        {
            _startName.text = product.name;
            _startType.text = product.type;
            _startPrice.text = product.price.ToString();
        }
    }
}