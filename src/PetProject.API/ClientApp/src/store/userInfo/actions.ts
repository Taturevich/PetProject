import axios from 'axios';

import * as actionTypes from './actionTypes';
import { AppThunk } from '../appState';
import { UserInfo } from './state';

export interface SendUserInfo {
    type: typeof actionTypes.SEND_USER_INFO;
}

export type PetListActionTypes = SendUserInfo;

// export function requestFeaturesList() : RequestFeaturesList {
//     return {
//         type: actionTypes.REQUEST_FEATURES_LIST,
//     }
// }

// export function receiveFeaturesList(features: Feature[]) : ReceiveFeaturesList {
//     return {
//         type: actionTypes.RECEIVE_FEATURES_LIST,
//         features
//     }
// }

// export const requestFeaturesListData = (): AppThunk => dispatch => {
//     dispatch(requestFeaturesList());
//     axios.post<UserInfo[]>('http://localhost:5000/api/user/')
//         .then(response => dispatch(receiveFeaturesList(response.data)));
//   }
