import React, { Component } from 'react';
import { Provider } from 'react-redux';

import { store } from './store/appState';
import Header from './components/header/header';
import { Footer } from './components/footer/Footer';

import MainPage from './pages/mainPage/MainPage';
import { HelpWantedPage } from './pages/helpWanted/HelpWantedPage';
import { PetFoundPage } from './pages/petFound/PetFoundPage';

import { Switch, Route } from 'react-router-dom'
const Layout = () => (
    <main>
        <Switch>
            <Route path='/' component={MainPage} />
            <Route path='/WantToHelp' component={HelpWantedPage} />
            <Route path='/PetFound' component={PetFoundPage} />
        </Switch>
    </main>
)


class App extends Component {
    render() {
        return (
            <Provider store={store}>
                <Header></Header>
                <MainPage />
                <Footer></Footer>
            </Provider>
        );
    }
}

export default App;
