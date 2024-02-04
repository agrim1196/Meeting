import Form from "react-bootstrap/Form";
import "bootstrap/dist/css/bootstrap.min.css";
import Button from "react-bootstrap/Button";
import Bgr from "../assets/Bgr-image.jpg";
import { Link, useNavigate } from "react-router-dom";
import React, { useReducer, useEffect, useMemo } from "react";

const initialState = {
  email: "",
  password: "",
};

//Will enter here when email/pwd value changed
const formsReducer = (state, action) => {
  let value = action.payload;
  switch (action.type) {
    case "email":
      return { ...state, email: value };
    case "password":
      return { ...state, password: value };
    default:
      throw new Error(`Unknown action type: ${action.type}`);
  }
};

export default function SignIn() {
  const navigate = useNavigate();

  const redirectToDashboard = () => {
    navigate("/dashboard");
  };
  const [formState, dispatch] = useReducer(formsReducer, initialState);
  const login = async (event) => {
    try {
      event.preventDefault();
     const header = new Headers();
     header.append("Is-Sign-In", true);
     header.append('Content-Type', 'application/json');
      if (formState.email && formState.password) {
        var requestOptions = {
          method: "POST",
          headers: header,
          body: JSON.stringify({
            Username: formState.email,
            Password: formState.password,
          }),
        };

        fetch("https://localhost:44352/authenticate", requestOptions)
          .then((response) => response.text())
          .then((result) => {
            // Storing the token
            localStorage.setItem("userToken", result);
            redirectToDashboard();
            localStorage.setItem("userName", JSON.parse(result).username);
          })
          .catch((error) => console.log("error", error));
      }
    } catch (error) {
      throw error;
    }
  };
  const memoizedLogin = useMemo(() => login, []);
  useEffect(() => {
    // Call the effect when the submit button is clicked
    const form = document.querySelector("form");
    form.addEventListener("submit", memoizedLogin);

    // Return a cleanup function to remove the event listener
    return () => {
      form.removeEventListener("submit", memoizedLogin);
    };
  }, []);

  return (
    <>
      <section style={{ backgroundImage: `url(${Bgr})` }}>
        <div className="container d-flex flex-column">
          <div className="row align-items-center justify-content-center gx-0 min-vh-100">
            <div className="col-12 col-md-6 col-lg-4 py-8 py-md-11">
              <h1>Sign In</h1>
              <p className="font-weight-light font-italic">
                If you already have account then lets book your meeting !
              </p>
              <Form onSubmit={login}>
                <Form.Group
                  className="mb-3"
                  controlId="exampleForm.ControlInput1"
                >
                  <Form.Label>Email address</Form.Label>
                  <Form.Control
                    type="email"
                    placeholder="name@example.com"
                    required={true}
                    value={formState.email}
                    onChange={(event) =>
                      dispatch({ type: "email", payload: event.target.value })
                    }
                  />
                </Form.Group>
                <Form.Group
                  className="mb-3"
                  controlId="exampleForm.ControlTextarea1"
                >
                  <Form.Label>Password</Form.Label>
                  <Form.Control
                    type="password"
                    required={true}
                    value={formState.password}
                    onChange={(event) =>
                      dispatch({
                        type: "password",
                        payload: event.target.value,
                      })
                    }
                  />
                </Form.Group>
                <div
                  className="float-right 
              "
                >
                  <Button variant="primary" type="submit">
                    Sign In
                  </Button>
                </div>
                <Link to="signup">New ? Please register first !</Link>
              </Form>
            </div>
          </div>
        </div>
      </section>
    </>
  );
}
