import React, { Component } from 'react';
import { Provider } from 'react-redux';

import { store } from './store/appState';
import MainPage from './pages/mainPage/MainPage';

class App extends Component {
  render() {
    return (
      <Provider store={store}>
          <MainPage />
      </Provider>
    );
  }
}

export default App;
