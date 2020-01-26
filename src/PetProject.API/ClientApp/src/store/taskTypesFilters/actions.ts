import * as actionTypes from './actionTypes';
import { TaskTypeFilter } from './state';

export interface RequestTaskTypesFiltersList {
    type: typeof actionTypes.REQUEST_TASK_TYPES_FILTERS_LIST;
}

export interface ReceiveTaskTypesFiltersList {
    type: typeof actionTypes.RECEIVE_TASK_TYPES_FILTERS_LIST;
    taskTypesFilters: TaskTypeFilter[];
}

export type TaskTypesFiltersListActionTypes = RequestTaskTypesFiltersList | ReceiveTaskTypesFiltersList;

export function requestTaskTypesList() : RequestTaskTypesFiltersList {
    return {
        type: actionTypes.REQUEST_TASK_TYPES_FILTERS_LIST,
    }
}

export function receiveTaskTypesList(taskTypesFilters: TaskTypeFilter[]) : ReceiveTaskTypesFiltersList {
    return {
        type: actionTypes.RECEIVE_TASK_TYPES_FILTERS_LIST,
        taskTypesFilters
    }
}
