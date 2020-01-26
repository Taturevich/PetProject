import * as actionTypes from './actionTypes';
import { TaskTypesFiltersListActionTypes } from './actions';
import { TaskTypesFiltersListState } from './state';

export const taskTypesListInitialState: TaskTypesFiltersListState = {
    data: []
};

export function taskTypesFiltersListReducer (state = taskTypesListInitialState, action: TaskTypesFiltersListActionTypes) {
    switch (action.type) {
        case actionTypes.REQUEST_TASK_TYPES_FILTERS_LIST : {
            return {
                ...state,
            };
        }
        case actionTypes.RECEIVE_TASK_TYPES_FILTERS_LIST : {
            return {
                ...state,
                data: action.taskTypesFilters
            };
        }
        default : {
            return state;
        }
    }
}
