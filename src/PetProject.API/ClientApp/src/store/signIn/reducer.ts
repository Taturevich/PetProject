import * as actionTypes from './actionTypes';
import { LoginRequestActionTypes } from './actions';
import { TokenState } from './state';

export const signInListInitialState: TokenState = {
    token: ''
};

export function petsListReducer(state = signInListInitialState, action: LoginRequestActionTypes) {
    switch (action.type) {
        case actionTypes.SEND_LOGIN_REQUEST: {
            return {
                ...state,
            };
        }
        case actionTypes.RECEIVE_AUTHENTICATION_TOKEN: {
            return {
                ...state,
                token: action.token
            };
        }
        default : {
            return state;
        }
    }
}
