import "bootstrap/dist/css/bootstrap.min.css";
export default function Header({ name }) {

    var greeting;
    var username;
    function getGreeting()
    {
        username = localStorage.getItem("userName");
        var currentDate = new Date();
        var currentTime = currentDate.getHours();

        if (currentTime <= 12) {
            greeting = "Good Morning !";
        }
        if (currentTime > 12 && currentTime < 22) {
            greeting = "Good Evening !";
        }
        if (currentTime >=22 && currentTime < 24) {
            greeting = "Good Night !";
        }
    }
    getGreeting();
    return (

        <div className="container">
            <header className="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 mb-4 border-bottom">
                <div className="col-md-3 mb-2 mb-md-0">
                    <a href="/" className="d-inline-flex link-body-emphasis text-decoration-none">
                    </a>
                </div>

                <ul className="nav col-12 col-md-auto mb-2 justify-content-center mb-md-0">
                    <li><a href="#" className="nav-link px-2 link-primary">Dashboard</a></li>
                </ul>

                <div className="col-md-3">
                    <button type="button" className="btn btn-secondary">Sign-out</button>
                </div>
                <div className="col-md-2">
                <h5>{greeting + " " + username}</h5>
                </div>
            </header>
        </div>
    )
}