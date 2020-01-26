import * as actionTypes from './actionTypes';
import { TaskTypesListActionTypes } from './actions';
import { TaskTypesListState } from './state';

export const taskTypesListInitialState: TaskTypesListState = {
    data: []
};

export function taskTypesListReducer (state = taskTypesListInitialState, action: TaskTypesListActionTypes) {
    switch (action.type) {
        case actionTypes.REQUEST_TASK_TYPES_LIST : {
            return {
                ...state,
            };
        }
        case actionTypes.RECEIVE_TASK_TYPES_LIST : {
            return {
                ...state,
                data: action.taskTypes
            };
        }
        default : {
            return state;
        }
    }
}
