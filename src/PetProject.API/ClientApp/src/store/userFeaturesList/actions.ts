import axios from 'axios';

import * as actionTypes from './actionTypes';
import { AppThunk } from '../appState';
import { UserFeature } from './state';

export interface RequestUserFeaturesList {
    type: typeof actionTypes.REQUEST_USER_FEATURES_LIST;
}

export interface ReceiveUserFeaturesList {
    type: typeof actionTypes.RECEIVE_USER_FEATURES_LIST;
    userFeatures: UserFeature[];
}

export type UserFeaturesActionTypes = RequestUserFeaturesList | ReceiveUserFeaturesList;

export function requestUserFeaturesList() : RequestUserFeaturesList {
    return {
        type: actionTypes.REQUEST_USER_FEATURES_LIST,
    }
}

export function receiveUserFeaturesList(userFeatures: UserFeature[]) : ReceiveUserFeaturesList {
    return {
        type: actionTypes.RECEIVE_USER_FEATURES_LIST,
        userFeatures
    }
}

export const requestUserFeaturesListData = (): AppThunk => dispatch => {
    dispatch(requestUserFeaturesList());
    axios.get<UserFeature[]>('http://localhost:5000/api/userfeature')
        .then(response => dispatch(receiveUserFeaturesList(response.data)));
  }
