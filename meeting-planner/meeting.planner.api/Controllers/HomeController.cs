using JWTAuth.WebApi.Models;
using MeetingPlannerAPI.DAL;
using MeetingPlannerAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Sieve.Services;
using WebApi.Models;
using WebApi.Services;

namespace MeetingPlannerAPI.Controllers
{
    /// <summary>
    /// Home Controller.
    /// </summary>
    public class HomeController : ControllerBase
    {
        private readonly IRepository<Employees> _empRepo;
        private readonly IRepository<MeetingRooms> _meetingRoom;
        private readonly IRepository<MeetingsPlanned> _meetingsPlannedRepo;
        private readonly IRepository<Users> _usersRepo;
        private readonly SieveProcessor _sieveProcessor;
        private readonly DatabaseContext dbContext;
        private readonly IUsersService _usersService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
                 IUsersService usersService,
                 IRepository<Users> usersRepository,
                 IRepository<Employees> employeeRepository,
                 IRepository<MeetingRooms> meetingRoomRepository,
                 SieveProcessor sieveProcessor,
                 IRepository<MeetingsPlanned> meetingsPlannedRepository,
                 DatabaseContext dbContext,
                 ILogger<HomeController> logger)
        {
            _usersService = usersService;
            _usersRepo = usersRepository;
            _empRepo = employeeRepository;
            _meetingRoom = meetingRoomRepository;
            _sieveProcessor = sieveProcessor;
            _meetingsPlannedRepo = meetingsPlannedRepository;
            this.dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            try
            {
                Log.Information("User Authentication In Process");
                var response = await _usersService.Authenticate(model);
                if (response == null)
                {
                  return BadRequest(new { message = "Username or password is incorrect" });
                }
                Log.Information("User Authentication Successful");
                return Ok(response);
            }
            catch(Exception ex)
            {
                Log.Error($"Application returned error: { ex } ");
                return StatusCode(500, new { error = "Failed to authenticate user" });
            }
        }

        [HttpGet]
        [Route("employees")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                Log.Information("Fetching all employees.");
                var response = await _empRepo.getAllAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error($"Application returned error: {ex} ");
                return StatusCode(500, new { error = "Failed to retrieve data" });
            }
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                Log.Information("Fetching all users.");
                var response = await _usersRepo.getAllAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error($"Application returned error: {ex} ");
                return StatusCode(500, new { error = "Failed to retrieve data" });
            }
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
        public async Task<IActionResult> GetMeetingRoomInformation(int meetingRoomNo)
        {
            try
            {
                Log.Information("Fetching all meeting room related information.");
                var response = (await _meetingsPlannedRepo.getAllAsync()).Where(planned => planned.RoomNo == meetingRoomNo);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error($"Application returned error: {ex} ");
                return StatusCode(500, new { error = "Failed to retrieve data" });
            }        
        }

        [HttpGet]
        [Route("plannedMeetings/{selectedDate}")]
        public async Task<IActionResult> GetPlannedMeetings(DateTime selectedDate)
        {
            try
            {
                Log.Information("Fetching all meeting rooms.");
                var allMeetingRooms = await _meetingRoom.getAllAsync();
                var dt = selectedDate.Date;
                //var result = _meetingsPlannedRepo.getAll().AsNoTracking().Where(planned => planned.MeetingScheduledOn.Value.Date == dt);
                var result = from v in allMeetingRooms
                             join u in (await _meetingsPlannedRepo.getAllAsync()).Where(m => m.MeetingScheduledOn.HasValue && m.MeetingScheduledOn.Value.Date == dt) on v.Roomno equals u.RoomNo into joinedRooms
                             from m in joinedRooms.DefaultIfEmpty()
                             select new MeetingsPlanned { RoomNo = v.Roomno, MeetingScheduledOn = m.MeetingScheduledOn, NoOfParticipants = v.Capacity };
                //select new MeetingsPlanned{ RoomNo =  v.Roomno  };
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"Application returned error: {ex} ");
                return StatusCode(500, new { error = "Failed to retrieve data" });
            }
        }

        [HttpPost]
        [Route("scheduleMeeting/{meetingRoomNo}/{meetingDateTime}/{meetingHours}/{capacity}")]
        public async Task<IActionResult> ScheduleMeetingAsync(int meetingRoomNo, DateTime meetingDateTime, int meetingHours, int capacity)
        {
            try
            {
                Log.Information("Fetching all Scheduled Meetings.");
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
            catch(Exception ex)
            {
                Log.Error($"Application returned error: {ex} ");
                return StatusCode(500, new { error = "Failed to retrieve data" });
            }
        }
    }
}
