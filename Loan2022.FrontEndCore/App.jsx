import React from 'react';

import { AppProvider } from '@front-end/contexts';
// import { LOCALSTORAGE_KEY } from '@front-end/core';
import {uuidV4} from '@front-end/utils/validation';

const App = (props) => {

  // if (!localStorage.getItem(LOCALSTORAGE_KEY.UUID)) {
  //   localStorage.setItem(LOCALSTORAGE_KEY.UUID, uuidV4());
  // }
  
  return (
    <AppProvider initialValues={{ global: window.GLOBAL || {} }}>
      {props.children}
    </AppProvider>
  );
}

export default App;
