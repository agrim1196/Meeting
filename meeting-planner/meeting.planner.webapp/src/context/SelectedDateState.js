import SelectedDateContext from './SelectedDateContext';
import React, { useState } from "react";

const SelectedDateState = ( props ) => {
  const [selectedDate, setSelectedDate] = useState(new Date());
  return (
    <SelectedDateContext.Provider
      value={{
        selectedDate,
        setSelectedDate,
      }}
    >
      {props.children}
    </SelectedDateContext.Provider>
  );
};

export default SelectedDateState;