import React, { Component } from 'react';
import Button from '@material-ui/core/Button';

import { LoginModal, RegisterModal } from '../../components/modals';
import { signInListInitialState } from '../../store/signIn/reducer';

interface FooterProps {
}

interface FooterState {
    isLogInOpen: boolean;
    isRegisterOpen: boolean;
}

export class Footer extends React.Component<FooterProps, FooterState> {
    constructor(props: FooterProps) {
        super(props);
        this.state = {
            isLogInOpen: false,
            isRegisterOpen: false
        };
    }
    render() {
        const LoginModalCloser = () => {
            this.setState((state, props) => {
                return { isLogInOpen: false, isRegisterOpen: state.isRegisterOpen };
            });
        };

        if (!!signInListInitialState.token) {
            return (
                <div>
                    SignIn
                    {<LoginModal open={this.state.isLogInOpen} handleCancel={LoginModalCloser} handleLogin={LoginModalCloser} />}
                    {/* <RegisterModal open={true} handleCancel={() => {}} handleRegister={() => {}} /> */}
                </div>
            );
        }
        else {
            return (
                <div>
                    SignOut
            </div>
            )
        }
    }
}
