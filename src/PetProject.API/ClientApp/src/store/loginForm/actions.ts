import axios from 'axios';

import * as actionTypes from './actionTypes';
import { AppThunk } from '../appState';
import { LoginData } from './state';
import { TokenData } from './state';

export interface SendLoginRequest {
    type: typeof actionTypes.SEND_LOGIN_REQUEST;
    phone: '';
    password: '';
}

export interface ReceiveTokenRequest {
    type: typeof actionTypes.RECEIVE_AUTHENTICATION_TOKEN;
    token: '';
}

export interface CancelLoginRequest {
    type: typeof actionTypes.CANCEL_LOGIN_REQUEST;
}

export type LoginRequestActionTypes = SendLoginRequest | ReceiveTokenRequest | CancelLoginRequest;

export function sendLoginDetails(loginData): SendLoginRequest {
    return {
        type: actionTypes.SEND_LOGIN_REQUEST,
        phone: loginData.phone,
        password: loginData.password
    }
}

export function receiveToken(token): ReceiveTokenRequest {
    console.log(token);
    return {
        type: actionTypes.RECEIVE_AUTHENTICATION_TOKEN,
        token
    }
}

export const sendLoginRequest = (loginData): AppThunk => dispatch => {
    dispatch(sendLoginDetails(loginData));
    axios.post('http://localhost:5000/api/account/token', loginData)
        .then(response => dispatch(receiveToken(response.data)));
  }
