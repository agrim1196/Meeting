import "bootstrap/dist/css/bootstrap.min.css";
export default function Header({ name }) {
  var greeting;
  var username;
  function getGreeting() {
    username = localStorage.getItem("userName");
    var currentDate = new Date();
    var currentTime = currentDate.getHours();

    if (currentTime <= 12) {
      greeting = "Good Morning !";
    }
    if (currentTime > 12 && currentTime < 22) {
      greeting = "Good Evening !";
    }
    if (currentTime >= 22 && currentTime < 24) {
      greeting = "Good Night !";
    }
  }
  getGreeting();
  return (
    <div class="navbar navbar-dark sticky-top bg-dark flex-md-nowrap p-0 shadow">
      <a class="navbar-brand col-md-3 col-lg-2 me-0 px-3 fs-6" href="#">
        Meeting Planner
      </a>
      <h6 class="pt-3" style={{ color: `aqua` }}>{greeting + " " + username}</h6>
      <button
        class="navbar-toggler position-absolute d-md-none collapsed"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#sidebarMenu"
        aria-controls="sidebarMenu"
        aria-expanded="false"
        aria-label="Toggle navigation"
      >
        <span class="navbar-toggler-icon"></span>
      </button>

      <div class="navbar-nav">
        <div class="nav-item text-nowrap">
          <a class="nav-link px-3" href="#">
            Sign out
          </a>
        </div>
      </div>
    </div>
  );
}
