import axios from 'axios';

import * as actionTypes from './actionTypes';
import { AppThunk } from '../appState';
import { LoginData, TokenData } from './state';

export interface SendLoginRequest {
    type: typeof actionTypes.SEND_LOGIN_REQUEST;
    phone: string;
    password: string;
}

export interface ReceiveTokenRequest {
    type: typeof actionTypes.RECEIVE_AUTHENTICATION_TOKEN;
    token: string;
}

export interface CancelLoginRequest {
    type: typeof actionTypes.CANCEL_LOGIN_REQUEST;
}

export type LoginRequestActionTypes = SendLoginRequest | ReceiveTokenRequest | CancelLoginRequest;

export function sendLoginDetails(loginData: LoginData): SendLoginRequest {
    return {
        type: actionTypes.SEND_LOGIN_REQUEST,
        phone: loginData.phone,
        password: loginData.password
    }
}

export function receiveToken(token: TokenData): ReceiveTokenRequest {
    return {
        type: actionTypes.RECEIVE_AUTHENTICATION_TOKEN,
        token: token.token
    }
}

export const sendLoginRequest = (loginData: LoginData): AppThunk => dispatch => {
    dispatch(sendLoginDetails(loginData));
    axios.post('http://localhost:5000/api/account/token', loginData)
        .then(response => dispatch(receiveToken(response.data)));
  }
