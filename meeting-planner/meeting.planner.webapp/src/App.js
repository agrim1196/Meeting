import "./App.css";
import SignIn from "./components/SignIn";
import SignUp from "./components/SignUp";
import Dashboard from "./components/Dashboard";
import Header from './components/Header';
import Navbar  from "./components/Navbar";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Bgr from "./assets/Bgr-image.jpg";
import SelectedDateState from "./context/SelectedDateState";

function App() {
  return (
    <>
<SelectedDateState>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<SignIn />} />
        <Route path="signup" element={<SignUp />} />
        <Route
          path="*"
          element={
            <section style={{ backgroundImage: `url(${Bgr})` }}>
            <div className="text-center pt-5">
              <h1>404 Not Found</h1>
              <p>The page you are looking for cannot be found.</p>
              <p>Please check the URL and try again.</p>
              <p>
                If you are still having trouble, please contact the website
                administrator.
              </p>
            </div>
            </section>
          }
        ></Route>
        <Route path="dashboard" element={<Dashboard />} />
        <Route path="header" element={<Header />} />
        <Route  path = "navbar" element={Navbar}></Route>
      </Routes>
    </BrowserRouter>
    </SelectedDateState>
    </>
  );
}

export default App;
