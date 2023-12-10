import Form from "react-bootstrap/Form";
import "bootstrap/dist/css/bootstrap.min.css";
import Button from "react-bootstrap/Button";
import { Link } from "react-router-dom";

export default function SignUp() {
  return (
    <>
      <div className="container d-flex flex-column">
        <div className="row align-items-center justify-content-center gx-0 min-vh-100">
          <div className="col-12 col-md-6 col-lg-4 py-8 py-md-11">
            <h1>Sign Up</h1>
            <p className="font-weight-light">
              If you already have account then lets book your meeting !
              <Link to="/">Sign In</Link>
            </p>
            <Form>
              <Form.Group
                className="mb-3"
                controlId="exampleForm.ControlInput1"
              >
                <Form.Label>Employee Id</Form.Label>
                <Form.Control type="email" placeholder="name@example.com" />
              </Form.Group>
              <Form.Group
                className="mb-3"
                controlId="exampleForm.ControlInput2"
              >
                <Form.Label>Employee Name</Form.Label>
                <Form.Control type="name" />
              </Form.Group>
              <Form.Group
                className="mb-3"
                controlId="exampleForm.ControlInput3"
              >
                <Form.Label>Password</Form.Label>
                <Form.Control type="password" />
              </Form.Group>
              <Form.Group
                className="mb-3"
                controlId="exampleForm.ControlInput4"
              >
                <Form.Label>Client</Form.Label>
                <Form.Control type="name" />
              </Form.Group>
              <Form.Group
                className="mb-3"
                controlId="exampleForm.ControlInput5"
              >
                <Form.Label>Business Unit</Form.Label>
                <Form.Control type="name" />
              </Form.Group>
              <div
                className="float-right 
              "
              >
                <Button variant="primary">Register</Button>
              </div>
            </Form>
          </div>
        </div>
      </div>
    </>
  );
}
