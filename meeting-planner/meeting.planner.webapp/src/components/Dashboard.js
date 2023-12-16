// import DatePicker from "react-datepicker";
import React, { useContext, useState, useEffect } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap";
import Header from "./Header";
import Navbar from "./Navbar";
import SelectedDateContext from "../context/SelectedDateContext";
import BookMeetingSlotPopUp from  "./BookMeetingSlotPopUp";

export default function Dashboard() {

  const[showPopup, setShowPopup] = useState(false);

  var selectedDate = useContext(SelectedDateContext).selectedDate;
  const [plannedMeetingRooms, setPlannedMeetingRooms] = useState([]);

  const header = new Headers();
  header.append("Authorization", "Bearer" + localStorage.getItem("userToken"));
  const requestOptions = {
    method: "GET",
    headers: header,
  };

  const envUrl = "https://localhost:44352/plannedMeetings/";
  useEffect(() => {
    fetch(envUrl + selectedDate.toISOString(), requestOptions)
      .then((response) => response.json()) // Parse the response as JSON
      .then((data) => {
        // Check if data is already set and only set it if not
        setPlannedMeetingRooms(data);
        console.log(plannedMeetingRooms);
      })
      .catch((error) => {
        console.error(error);
      });
  }, [selectedDate]);

  return (
    <>
    <BookMeetingSlotPopUp show={showPopup} close={() => setShowPopup(false)}/>
      <Header />
      <div class="container-fluid">
        <div class="row">
          <Navbar />
          <div class="col-md-9 ms-sm-auto col-lg-10 px-md-4 pt-4">
            <div class="row row-cols-1 row-cols-md-3 mb-3 text-center">
              {plannedMeetingRooms?.map(function (room) {
                return (
                  <>
                    {
                      <div class="col">
                        <div
                          className="card mb-4 rounded-3 shadow-sm pe-auto"
                          onClick={
                            room.meetingScheduledOn ? undefined : () => setShowPopup(true)
                          }
                          style={{
                            cursor: room.meetingScheduledOn ? "" : `pointer`,
                          }}
                        >
                          <div
                            className={`card-body ${
                              room.meetingScheduledOn
                                ? "bg-danger"
                                : "bg-success"
                            }`}
                          >
                            <h4 className="my-0 font-weight-normal">
                              Meeting Room - {room.roomNo}{" "}
                            </h4>
                          </div>
                          <div className="card-body">
                            <h1 className="card-title pricing-card-title">
                              Capacity -
                              <small className="text-muted">
                                {" "}
                                {room.noOfParticipants}
                              </small>
                            </h1>
                          </div>
                        </div>
                      </div>
                    }
                  </>
                );
              })}
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
