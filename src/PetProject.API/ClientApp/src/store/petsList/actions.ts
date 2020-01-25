import axios from 'axios';

import * as actionTypes from './actionTypes';
import { AppThunk } from '../appState';
import { Pet } from './state';

export interface RequestPetsList {
    type: typeof actionTypes.REQUEST_PETS_LIST;
}

export interface ReceivePetsList {
    type: typeof actionTypes.RECEIVE_PETS_LIST;
    pets: Pet[];
}

export type PetListActionTypes = RequestPetsList | ReceivePetsList;

export function requestPetsList() : RequestPetsList {
    return {
        type: actionTypes.REQUEST_PETS_LIST,
    }
}

export function receivePetsList(pets: Pet[]) : ReceivePetsList {
    return {
        type: actionTypes.RECEIVE_PETS_LIST,
        pets
    }
}

export const requestPetsListData = (): AppThunk => dispatch => {
    dispatch(requestPetsList());
    axios.get<Pet[]>('http://localhost:5000/api/pet')
        .then(response => dispatch(receivePetsList(response.data)));
}

export const requestPetsListFilteredData = (ids: string[]): AppThunk => dispatch => {
    dispatch(requestPetsList());
    const query = '?' + ids.map(id => `featureIds=${id}`).join('&');
    axios.get<Pet[]>(ids.length > 0 ? 'http://localhost:5000/api/pet/byFeatures' + query : 'http://localhost:5000/api/pet')
        .then(response => dispatch(receivePetsList(response.data)));
}
