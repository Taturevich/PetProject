import axios from 'axios';

import * as actionTypes from './actionTypes';
import { AppThunk } from '../appState';
import { TaskType } from './state';

export interface RequestTaskTypesList {
    type: typeof actionTypes.REQUEST_TASK_TYPES_LIST;
}

export interface ReceiveTaskTypesList {
    type: typeof actionTypes.RECEIVE_TASK_TYPES_LIST;
    taskTypes: TaskType[];
}

export type TaskTypesListActionTypes = RequestTaskTypesList | ReceiveTaskTypesList;

export function requestTaskTypesList() : RequestTaskTypesList {
    return {
        type: actionTypes.REQUEST_TASK_TYPES_LIST,
    }
}

export function receiveTaskTypesList(taskTypes: TaskType[]) : ReceiveTaskTypesList {
    return {
        type: actionTypes.RECEIVE_TASK_TYPES_LIST,
        taskTypes
    }
}

export const requestTaskTypesListData = (): AppThunk => dispatch => {
    dispatch(requestTaskTypesList());
    axios.get<TaskType[]>('http://localhost:5000/api/taskType')
        .then(response => dispatch(receiveTaskTypesList(response.data)));
  }
