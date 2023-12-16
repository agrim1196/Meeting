import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import { useState } from "react";

export default function BookMeetingSlotPopUp(props) {
 
var date = new Date();

  const meeting = {
    meetingRoomNo:0,
    meetingDateTime: date,
    meetingHours: 0,
    capacity: 0
  }; 

  const[meetingRequest, setMeetingRequest] = useState(meeting);


  return (
    <Modal
      show={props.show}
      cancel={props.close}
      aria-labelledby="contained-modal-title-vcenter"
      centered
    >
      <Modal.Header closeButton onClick={props.close}>
        <Modal.Title>Book Meeting</Modal.Title>
      </Modal.Header>
      <Modal.Body>
      <Form>
          <div class="row g-3">
            <div class="col-sm-6">
              <Form.Label>Meeting Room Number</Form.Label>
              <Form.Control
                    type="text"
                    required={true}
                    value={meetingRequest.meetingRoomNo}
                  />
            </div>
            <div class="col-sm-6">
              <Form.Label>Meeting Date Time</Form.Label>
              <Form.Control
                    type="text"
                    required={true}
                    value={meetingRequest.meetingDateTime}
                  />
            </div>
            <div class="col-sm-6">
              <Form.Label>Meeting Hours</Form.Label>
              <Form.Control
                    type="text"
                    required={true}
                    value={meetingRequest.meetingHours}
                  />
            </div>
            <div class="col-sm-6">
              <Form.Label>Capacity</Form.Label>
              <Form.Control
                    type="text"
                    required={true}
                    value={meetingRequest.capacity}
                  />
            </div>
          </div>
        </Form>
        </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={props.close}>Close</Button>
        <Button variant="primary">Save Changes</Button>
      </Modal.Footer>
    </Modal>
  );
}
