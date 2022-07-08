import React, { Component } from 'react';
import { Provider } from 'react-redux';

import { store } from './store/appState';
import Header from './components/header/Header';
import { Footer } from './components/footer/Footer';

import MainPage from './pages/mainPage/MainPage';
import { HelpWantedPage } from './pages/helpWanted/HelpWantedPage';
import { PetFoundPage } from './pages/petFound/PetFoundPage';

import { Routes, Route } from 'react-router-dom'
const Layout = () => (
    <main>
        <Routes>
            <Route path='/' element={<MainPage/>} />
            <Route path='/WantToHelp' element={<HelpWantedPage/>} />
            <Route path='/PetFound' element={<PetFoundPage/>} />
        </Routes>
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
