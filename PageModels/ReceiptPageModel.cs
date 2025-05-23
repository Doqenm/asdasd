using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Managing.Data;
using Managing.Models;
using System.Collections.ObjectModel;

namespace Managing.PageModels;

public partial class ReceiptPageModel : BasePageModel
{
    private readonly ApplicationDbContext _db;

    [ObservableProperty] string supplier;
    [ObservableProperty] string lotNumber;
    [ObservableProperty] int quantity;

    public ObservableCollection<MaterialReceipt> Receipts { get; } = new();

    public ReceiptPageModel(ApplicationDbContext db)
    {
        _db = db;
        LoadReceipts();
    }

    private void LoadReceipts()
    {
        var list = _db.MaterialReceipts.OrderByDescending(r => r.ReceivedAt).ToList();
        Receipts.Clear();
        foreach (var r in list)
            Receipts.Add(r);
    }

    [RelayCommand]
    public async Task RegisterReceiptAsync()
    {
        var receipt = new MaterialReceipt
        {
            Supplier = Supplier,
            LotNumber = LotNumber,
            Quantity = Quantity,
            ReceivedAt = DateTime.Now
        };
        _db.MaterialReceipts.Add(receipt);
        await _db.SaveChangesAsync();

        Supplier = LotNumber = string.Empty;
        Quantity = 0;
        LoadReceipts();
    }
}
