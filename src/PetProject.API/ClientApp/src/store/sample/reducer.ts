import * as actionTypes from './actionTypes';
import { SampleActionTypes } from './actions';

export const sampleInitialState = {
    done: false,
};

export function sampleReducer (state = sampleInitialState, action: SampleActionTypes) {
    switch (action.type) {
        case actionTypes.SAMPLE_REQUEST : {
            return {
                ...state,
            };
        }
        case actionTypes.SAMPLE_RESPONSE : {
            return {
                ...state,
            };
        }
        default : {
            return state;
        }
    }
}
