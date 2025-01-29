namespace Application.Models;

public class PagedResultBase<T>
{
    public uint ItemsFrom { get; set; }
    public uint ItemsTo { get; set; }
    public uint PagesCount { get; set; }
    public uint ItemsCount { get; set; }
    public IList<T> Items { get; set; } = [];

    public PagedResultBase(IList<T> items, uint itemsCount, uint pageSize, uint pageNumber)
    {
        Items = items;
        ItemsCount = itemsCount;
        ItemsFrom = pageSize * (pageNumber - 1) + 1;
        ItemsTo = Math.Min(ItemsFrom + pageSize - 1, itemsCount);
        PagesCount = (uint) Math.Ceiling(itemsCount / (double)pageSize);
    }

    public PagedResultBase()
    {}
}
