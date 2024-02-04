import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import { useReducer, useState } from "react";
import DatePicker from "react-datepicker";

var today = new Date();

const minDate = new Date(
  today.getFullYear(),
  today.getMonth(),
  today.getDate()
);

const initialState = {
  meetingRoomNo: 0,
  meetingDateTime: today,
  meetingHours: 0,
  capacity: 0,
};

export default function BookMeetingSlotPopUp(props) {
  // You can have validations here for formInput
  const formsReducer = (state, action) => {
    let value = action.payload;
    switch (action.type) {
      case "meetingRoomNo":
        return { ...state, meetingRoomNo: value };
      case "meetingDateTime": {
        return { ...state, meetingDateTime: value };
      }
      case "meetingHours":
        return { ...state, meetingHours: value };
      case "capacity":
        return { ...state, capacity: value };

      default:
        throw new Error(`Unknown action type: ${action.type}`);
    }
  };

  const [formState, dispatch] = useReducer(formsReducer, initialState);
  const [isFormValid, setIsFormValid] = useState(false);
  const hasRequired = () => {
    const isValid =
      formState.meetingRoomNo &&
      formState.meetingDateTime &&
      formState.meetingHours &&
      formState.capacity;
    setIsFormValid(isValid);
  };
  const saveMeeting = () => {
    const header = new Headers();
    header.append(
      "Authorization",
      "Bearer" + localStorage.getItem("userToken")
    );

    var requestOptions = {
      method: "POST",
      headers: header,
    };

    // Save the meeting for the selected date
    const envUrl = "https://localhost:44352/scheduleMeeting/";
    formState.meetingDateTime.toISOString();
    fetch(envUrl + formState.meetingRoomNo +'/'+ formState.meetingDateTime.toISOString() +'/'+ formState.meetingHours +'/'+ formState.capacity, requestOptions)
      .then((response) => response.json()) // Parse the response as JSON
      .then((data) => {
        console.log(data);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  console.log(isFormValid);
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
                value={formState.meetingRoomNo}
                onChange={(event) => {
                  hasRequired();
                  dispatch({
                    type: "meetingRoomNo",
                    payload: event.target.value,
                  });
                }}
              />
            </div>
            <div class="col-sm-6">
              <Form.Label>Meeting Date Time</Form.Label>

              <DatePicker
                onChange={(event) => {
                  hasRequired();
                  console.log(event);
                  dispatch({
                    type: "meetingDateTime",
                    payload: event,
                  });
                }}
                minDate={minDate}
                selected={formState.meetingDateTime}
              />
            </div>
            <div class="col-sm-6">
              <Form.Label>Meeting Hours</Form.Label>
              <Form.Control
                type="text"
                required={true}
                value={formState.meetingHours}
                onChange={(event) => {
                  hasRequired();
                  dispatch({
                    type: "meetingHours",
                    payload: event.target.value,
                  });
                }}
              />
            </div>
            <div class="col-sm-6">
              <Form.Label>Capacity</Form.Label>
              <Form.Control
                type="text"
                required={true}
                value={formState.capacity}
                onChange={(event) => {
                  hasRequired();
                  dispatch({
                    type: "capacity",
                    payload: event.target.value,
                  });
                }}
              />
            </div>
          </div>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={props.close}>
          Close
        </Button>
        <Button variant="primary" disabled={!isFormValid} onClick={saveMeeting}>
          Save Changes
        </Button>
      </Modal.Footer>
    </Modal>
  );
}
