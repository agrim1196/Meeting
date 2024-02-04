using JWTAuth.WebApi.Models;
using MeetingPlannerAPI.DAL;
using MeetingPlannerAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Sieve.Services;
using WebApi.Models;
using WebApi.Services;

namespace MeetingPlannerAPI.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IRepository<Employees> _empRepo;
        private readonly IRepository<MeetingRooms> _meetingRoom;
        private readonly IRepository<MeetingsPlanned> _meetingsPlannedRepo;
        private readonly IRepository<Users> _usersRepo;
        private readonly SieveProcessor _sieveProcessor;
        private readonly DatabaseContext dbContext;
        private readonly IUsersService _usersService;
        public HomeController(IUsersService usersService, IRepository<Users> usersRepo, IRepository<Employees> empRepo, IRepository<MeetingRooms> meetingRooms, SieveProcessor sieveProcessor, IRepository<MeetingsPlanned> meetingsPlannedRepo, DatabaseContext _dbContext)
        {
            _usersRepo = usersRepo;
            _empRepo = empRepo;
            _meetingRoom = meetingRooms;
            _sieveProcessor = sieveProcessor;
            _meetingsPlannedRepo = meetingsPlannedRepo;
            dbContext = _dbContext;
            _usersService = usersService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = await _usersService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpGet]
        [Route("employees")]
        public async Task<List<Employees>> GetEmployees()
        {

            return await _empRepo.getAllAsync();
        }

        [HttpGet]
        [Route("users")]
        public async Task<List<Users>> GetUsers()
        {
            return await _usersRepo.getAllAsync();
        }

        // TODO: How to make it asynchronous
        //[HttpGet]
        //[Route("meetingRooms")]
        //public async Task<IQueryable<MeetingRooms>> GetMeetingRooms(SieveModel sieveModel)
        //{
        //    // As no Tracking Makes read-only queries faster
        //    // Returns `result` after applying the sort/filter/page query in `SieveModel` to it
        //    var result = await _sieveProcessor.Apply(sieveModel, await _meetingRoom.getAllAsync());
        //    return result;
        //}

        [HttpGet]
        [Route("meetingRoomInformation/{meetingRoomNo}")]
        public async Task<IEnumerable<MeetingsPlanned>> GetMeetingRoomInformation(int meetingRoomNo)
        {
            var result = (await _meetingsPlannedRepo.getAllAsync()).Where(planned => planned.RoomNo == meetingRoomNo);
            return result;
        }

        [HttpGet]
        [Route("plannedMeetings/{selectedDate}")]
        public async Task<IEnumerable<MeetingsPlanned>> GetPlannedMeetings(DateTime selectedDate)
        {
            var allMeetingRooms = await _meetingRoom.getAllAsync();
            var dt = selectedDate.Date;
            //var result = _meetingsPlannedRepo.getAll().AsNoTracking().Where(planned => planned.MeetingScheduledOn.Value.Date == dt);
            var result = from v in allMeetingRooms
                         join u in (await _meetingsPlannedRepo.getAllAsync()).Where(m => m.MeetingScheduledOn.HasValue && m.MeetingScheduledOn.Value.Date == dt) on v.Roomno equals u.RoomNo into joinedRooms
                         from m in joinedRooms.DefaultIfEmpty()
                         select new MeetingsPlanned { RoomNo = v.Roomno, MeetingScheduledOn = m.MeetingScheduledOn, NoOfParticipants = v.Capacity };
            //select new MeetingsPlanned{ RoomNo =  v.Roomno  };
            return result;
        }

        [HttpPost]
        [Route("scheduleMeeting/{meetingRoomNo}/{meetingDateTime}/{meetingHours}/{capacity}")]
        public async Task<IActionResult> ScheduleMeetingAsync(int meetingRoomNo, DateTime meetingDateTime, int meetingHours, int capacity)
        {
            // Validate if meeting room no is valid or not 
            //Validating  capacity for the supplied meeting room
            var allMeetingRoomData = await _meetingRoom.getAllAsync();
            var meetingsPlannedRepo = allMeetingRoomData.Where(meeting => meeting.Roomno == meetingRoomNo && capacity <= meeting.Capacity);
            if (!meetingsPlannedRepo.Any())
            {
                return BadRequest();
            }
            var allMeetingsPlanned = await _meetingsPlannedRepo.getAllAsync();

            var plannedDateOverlap = allMeetingsPlanned.Where(planned => planned.RoomNo == meetingRoomNo
            && DateTime.Now < meetingDateTime
            && planned.MeetingScheduledOn.Value.AddHours(planned.MeetingScheduledFor.Value) < meetingDateTime);
            //var plannedDateOverlap = allMeetingsPlanned.Where(planned => planned.MeetingRoomNo == meetingRoomNo && DateTime.Now < meetingDateTime );

            //Validate if no meeting is scheduled on the supplied datetime.
            if (plannedDateOverlap.Any())
            {
                return BadRequest();
            }

            var scheduledMeetingRecord = new MeetingsPlanned()
            {
                RoomNo = meetingRoomNo,
                MeetingScheduledFor = meetingHours,
                MeetingScheduledOn = meetingDateTime
            };

            _meetingsPlannedRepo.Add(scheduledMeetingRecord);
            dbContext.SaveChanges();
            return Ok(scheduledMeetingRecord);
        }
    }
}
