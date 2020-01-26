import * as actionTypes from './actionTypes';
import { UserFeaturesActionTypes } from './actions';
import { UserFeaturesListState } from './state';

export const userFeaturesListInitialState: UserFeaturesListState = {
    data: []
};

export function userFeaturesListReducer (state = userFeaturesListInitialState, action: UserFeaturesActionTypes) {
    switch (action.type) {
        case actionTypes.REQUEST_USER_FEATURES_LIST : {
            return {
                ...state,
            };
        }
        case actionTypes.RECEIVE_USER_FEATURES_LIST : {
            return {
                ...state,
                data: action.userFeatures
            };
        }
        default : {
            return state;
        }
    }
}
