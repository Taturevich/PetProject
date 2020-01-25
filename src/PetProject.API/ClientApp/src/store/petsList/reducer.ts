import * as actionTypes from './actionTypes';
import { PetListActionTypes } from './actions';
import { PetsListState } from './state';

export const petsListInitialState: PetsListState = {
    data: []
};

export function petsListReducer (state = petsListInitialState, action: PetListActionTypes) {
    switch (action.type) {
        case actionTypes.REQUEST_PETS_LIST : {
            return {
                ...state,
            };
        }
        case actionTypes.RECEIVE_PETS_LIST : {
            return {
                ...state,
                data: action.pets
            };
        }
        default : {
            return state;
        }
    }
}
