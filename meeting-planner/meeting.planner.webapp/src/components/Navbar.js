import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import React, { useContext } from "react";
import SelectedDateContext from "../context/SelectedDateContext";

export default function Navbar() {

  const setSelectedDate = useContext(SelectedDateContext).setSelectedDate;
  const selectedDate = useContext(SelectedDateContext).selectedDate;

  const handleUpdateSelectedDate = async (date) => {
    setSelectedDate(date);
    console.log(selectedDate);
  };
  const today = new Date();
  const minDate = new Date(
    today.getFullYear(),
    today.getMonth(),
    today.getDate()
  );

  return (
    <>
      <div className="d-flex flex-nowrap">
        <div
          className="d-flex flex-column flex-shrink-0 p-3 text-bg-dark"
          style={{ width: `20%` }}
        >
          <div className="mx-3 my-5 pt-5">
            <button type="button" className="btn btn-primary">
              New Event
            </button>
          </div>

          <div className="mx-3 my-1 pt-1">
            <DatePicker
              selected={selectedDate}
              onChange={(date) => handleUpdateSelectedDate(date)}
              minDate={minDate}
            />
          </div>
        </div>
      </div>
    </>
  );
}
