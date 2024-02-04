using Sieve.Attributes;

namespace MeetingPlannerAPI.Model;

public partial class MeetingRooms
{
    public int Id { get; set; }

    [Sieve(CanFilter = true)]
    public int Roomno { get; set; }

    public int Capacity { get; set; }

    public bool? IsOccupied { get; set; }

    public virtual ICollection<MeetingsPlanned> MeetingsPlanneds { get; } = new List<MeetingsPlanned>();
}
