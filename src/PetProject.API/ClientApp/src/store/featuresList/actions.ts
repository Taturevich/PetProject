import axios from 'axios';

import * as actionTypes from './actionTypes';
import { AppThunk } from '../appState';
import { Feature } from './state';

export interface RequestFeaturesList {
    type: typeof actionTypes.REQUEST_FEATURES_LIST;
}

export interface ReceiveFeaturesList {
    type: typeof actionTypes.RECEIVE_FEATURES_LIST;
    features: Feature[];
}

export type PetListActionTypes = RequestFeaturesList | ReceiveFeaturesList;

export function requestFeaturesList() : RequestFeaturesList {
    return {
        type: actionTypes.REQUEST_FEATURES_LIST,
    }
}

export function receiveFeaturesList(features: Feature[]) : ReceiveFeaturesList {
    return {
        type: actionTypes.RECEIVE_FEATURES_LIST,
        features
    }
}

export const requestFeaturesListData = (): AppThunk => dispatch => {
    dispatch(requestFeaturesList());
    axios.get<Feature[]>('http://localhost:5000/api/petfeature')
        .then(response => dispatch(receiveFeaturesList(response.data)));
  }
