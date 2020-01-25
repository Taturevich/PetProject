import * as actionTypes from './actionTypes';
import { PetListActionTypes } from './actions';
import { FeaturesListState } from './state';

export const featuresListInitialState: FeaturesListState = {
    data: []
};

export function featuresListReducer (state = featuresListInitialState, action: PetListActionTypes) {
    switch (action.type) {
        case actionTypes.REQUEST_FEATURES_LIST : {
            return {
                ...state,
            };
        }
        case actionTypes.RECEIVE_FEATURES_LIST : {
            return {
                ...state,
                data: action.features
            };
        }
        default : {
            return state;
        }
    }
}
