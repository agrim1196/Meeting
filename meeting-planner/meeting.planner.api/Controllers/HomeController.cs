using JWTAuth.WebApi.Models;
using MeetingPlannerAPI.DAL;
using MeetingPlannerAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System.Globalization;
using System.Linq;
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
        public IActionResult Authenticate([FromBody]AuthenticateRequest model)
        {
            var response = _usersService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpGet]
        [Route("employees")]
        public IQueryable<Employees> GetEmployees()
        {

            return _empRepo.getAll();
        }

        [HttpGet]
        [Route("users")]
        public IQueryable<Users> GetUsers()
        {
            return _usersRepo.getAll();
        }

        [HttpGet]
        [Route("meetingRooms")]
        public IQueryable<MeetingRooms> GetMeetingRooms(SieveModel sieveModel)
        {
            // As no Tracking Makes read-only queries faster
            // Returns `result` after applying the sort/filter/page query in `SieveModel` to it
            var result = _sieveProcessor.Apply(sieveModel, _meetingRoom.getAll().AsNoTracking());
            return result;
        }

        [HttpGet]
        [Route("meetingRoomInformation/{meetingRoomNo}")]
        public IQueryable<MeetingsPlanned> GetMeetingRoomInformation(int meetingRoomNo)
        {
            var result = _meetingsPlannedRepo.getAll().AsNoTracking().Where(planned => planned.RoomNo == meetingRoomNo);
            return result;
        }

        [HttpGet]
        [Route("plannedMeetings/{selectedDate}")]
        public IQueryable<MeetingsPlanned> GetPlannedMeetings(DateTime selectedDate)
        {
            var allMeetingRooms = _meetingRoom.getAll();
            var dt = selectedDate.Date;
            //var result = _meetingsPlannedRepo.getAll().AsNoTracking().Where(planned => planned.MeetingScheduledOn.Value.Date == dt);
            var result = from v in allMeetingRooms
                         join u in _meetingsPlannedRepo.getAll().Where(m => m.MeetingScheduledOn.HasValue && m.MeetingScheduledOn.Value.Date == dt) on v.Roomno equals u.RoomNo into joinedRooms
                         from m in joinedRooms.DefaultIfEmpty()
                         select new MeetingsPlanned { RoomNo = v.Roomno, MeetingScheduledOn = m.MeetingScheduledOn, NoOfParticipants = v.Capacity };
            //select new MeetingsPlanned{ RoomNo =  v.Roomno  };
            return result;
        }

        [HttpPost]
        [Route("scheduleMeeting")]
        public IActionResult ScheduleMeeting(int meetingRoomNo, DateTime meetingDateTime, int meetingHours, int capacity)
        {
            // Validate if meeting room no is valid or not 
            //Validating  capacity for the supplied meeting room
            var allMeetingRoomData = _meetingRoom.getAll();
            var meetingsPlannedRepo = allMeetingRoomData.Where(meeting => meeting.Roomno == meetingRoomNo && meeting.Capacity <= capacity);
            if (!meetingsPlannedRepo.Any())
            {
                return BadRequest();
            }


            var allMeetingsPlanned = _meetingsPlannedRepo.getAll();
            var plannedDateOverlap = allMeetingsPlanned.Where(planned => planned.RoomNo == meetingRoomNo && DateTime.Now < meetingDateTime && (planned.MeetingScheduledOn < meetingDateTime && planned.MeetingScheduledOn.Value.AddHours(planned.MeetingScheduledFor.Value) < meetingDateTime));
            //var plannedDateOverlap = allMeetingsPlanned.Where(planned => planned.MeetingRoomNo == meetingRoomNo && DateTime.Now < meetingDateTime );

            //Validate if no meeting is scheduled on the supplied datetime.
            if (!plannedDateOverlap.Any())
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
