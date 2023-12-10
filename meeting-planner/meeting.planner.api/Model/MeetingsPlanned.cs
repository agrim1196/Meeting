using System;
using System.Collections.Generic;

namespace MeetingPlannerAPI.Model;

public partial class MeetingsPlanned
{
    public int Id { get; set; }

    public int RoomNo { get; set; }

    public int? NoOfParticipants { get; set; }

    public DateTime? MeetingScheduledOn { get; set; }

    public int? MeetingScheduledFor { get; set; }

    public virtual MeetingRooms RoomNoNavigation { get; set; } = null!;
}
