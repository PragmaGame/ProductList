using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class View : MonoBehaviour
    {
        [SerializeField] private Button _addedButton;
        [SerializeField] private Button _removeButton;
        [SerializeField] private Button _editButton;
        
        [SerializeField] private Model _model;
        [SerializeField] private FactoryCell _factoryCell;

        [SerializeField] private GameObject _panelProducts;
        [SerializeField] private GameObject _panelAddedProducts;

        private List<Product> _products;
        private Product _productSelected;

        private void Awake()
        {
            _products = new List<Product>();
        }

        private void OnEnable()
        {
            _addedButton.onClick.AddListener(OnClickAddedProduct);
            _removeButton.onClick.AddListener(OnClickRemoveProduct);
            _editButton.onClick.AddListener(OnClickEditProduct);
        }

        private void OnDisable()
        {
            _addedButton.onClick.RemoveListener(OnClickAddedProduct);
            _removeButton.onClick.RemoveListener(OnClickRemoveProduct);
            _editButton.onClick.RemoveListener(OnClickEditProduct);
            
        }

        private void Start()
        {
            var products = _model.Loading();
            
            foreach (var product in products)
            {
                var newProduct = _factoryCell.GetCell();
                newProduct.SetProductData(product);
                AddProduct(newProduct);
            }
        }

        private void AddProduct(Product product)
        {
            _products.Add(product);
            product.ProductSelectedEvent += OnProductSelected;
        }

        private void RemoveProduct(Product product)
        {
            _products.Remove(product);
            product.ProductSelectedEvent -= OnProductSelected;
        }

        private void OnProductSelected(Product product)
        {
            _productSelected = product;
        }

        private void OnClickAddedProduct()
        {
            _panelProducts.SetActive(false);
            _panelAddedProducts.SetActive(true);
            var viewEdit = _panelAddedProducts.GetComponent<ViewEditProduct>();
            viewEdit.Active();
            viewEdit.SaveProductEvent += OnSaveChanges;
        }

        private void OnClickRemoveProduct()
        {
            if (_productSelected is null)
            {
                return;
            }
            
            RemoveProduct(_productSelected);
            Destroy(_productSelected.gameObject);
        }

        private void OnClickEditProduct()
        {
            if (_productSelected is null)
            {
                return;
            }
            
            _panelProducts.SetActive(false);
            _panelAddedProducts.SetActive(true);
            var viewEdit = _panelAddedProducts.GetComponent<ViewEditProduct>();
            viewEdit.Active(_productSelected.ProductData);
            viewEdit.SaveProductEvent += OnSaveNewProduct;
        }

        private void OnSaveChanges(ProductData productData)
        {
            _panelProducts.SetActive(true);
            _panelAddedProducts.SetActive(false);
            var viewEdit = _panelAddedProducts.GetComponent<ViewEditProduct>();
            viewEdit.SaveProductEvent -= OnSaveChanges;
            
            var newProduct = _factoryCell.GetCell();
            newProduct.SetProductData(productData);
            AddProduct(newProduct);
        }

        private void OnSaveNewProduct(ProductData productData)
        {
            _panelProducts.SetActive(true);
            _panelAddedProducts.SetActive(false);
            var viewEdit = _panelAddedProducts.GetComponent<ViewEditProduct>();
            viewEdit.SaveProductEvent -= OnSaveNewProduct;
            
            _productSelected.SetProductData(productData);
        }

        private void OnApplicationQuit()
        {
            var productData = new ProductData[_products.Count];
            
            for (var i = 0; i < _products.Count; i++)
            {
                productData[i] = _products[i].ProductData;
            }
            
            _model.Save(productData);
        }
    }
}