using MovieBooking.Domain.Enums;

namespace MovieBooking.Domain.Layouts;

public class LayoutSeatType
{
    public Guid LayoutId { get; private set; }
    public int RowNumber { get; private set; }   // 1-based
    public int SeatNumber { get; private set; }  // 1-based
    public SeatType SeatType { get; private set; }

    public Layout? Layout { get; private set; }

    private LayoutSeatType() { } // EF

    public LayoutSeatType(Guid layoutId, int rowNumber, int seatNumber, SeatType seatType)
    {
        LayoutId = layoutId;
        RowNumber = rowNumber;
        SeatNumber = seatNumber;
        SeatType = seatType;
    }
}
