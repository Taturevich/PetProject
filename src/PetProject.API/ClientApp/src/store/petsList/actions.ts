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
    console.log(pets);
    return {
        type: actionTypes.RECEIVE_PETS_LIST,
        pets
    }
}

export const requestPetsListData = (): AppThunk => dispatch => {
    dispatch(requestPetsList());
    axios.get<Pet[]>('localhost:5000/api/pet')
        .then(response => dispatch(receivePetsList(response.data)));
  }
