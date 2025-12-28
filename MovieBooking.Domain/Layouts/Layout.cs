namespace MovieBooking.Domain.Layouts;

public class Layout
{
    public Guid LayoutId { get; private set; }
    public string Name { get; private set; } = null!;
    public int RowCount { get; private set; }
    public int SeatsPerRow { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public ICollection<LayoutSeatType> SeatTypes { get; private set; } = new List<LayoutSeatType>();

    private Layout() { } // EF

    public Layout(string name, int rowCount, int seatsPerRow)
    {
        LayoutId = Guid.NewGuid();
        Name = name;
        RowCount = rowCount;
        SeatsPerRow = seatsPerRow;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}
