using System.Collections.ObjectModel;
using Managing.Models;
using Managing.Services;

namespace Managing.Pages;

public partial class DashboardPage : ContentPage
{
    private readonly ProductService _productService;
    private readonly ObservableCollection<Product> _products = new();

    public DashboardPage(ProductService productService)
    {
        InitializeComponent();
        _productService = productService;
        BuildUI();
        _ = LoadProducts();
    }

    private async Task LoadProducts()
    {
        _products.Clear();
        var products = await _productService.GetProductsAsync();
        foreach (var p in products)
        {
            _products.Add(p);
        }
    }
}