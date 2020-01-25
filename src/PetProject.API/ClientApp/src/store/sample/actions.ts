import axios from 'axios';

import * as actionTypes from './actionTypes';
import { AppThunk } from '../appState';

export interface SampleRequest {
    type: typeof actionTypes.SAMPLE_REQUEST;
    payload: string;
}

export interface SampleResponse {
    type: typeof actionTypes.SAMPLE_RESPONSE;
    response: string;
}

export type SampleActionTypes = SampleRequest | SampleResponse;

export function doSampleRequest(payload: string) : SampleRequest {
    return {
        type: actionTypes.SAMPLE_REQUEST,
        payload
    }
}

export function doSampleResponse(response: string) : SampleResponse {
    return {
        type: actionTypes.SAMPLE_RESPONSE,
        response
    }
}

export const sendRequest = (message: string): AppThunk => dispatch => {
    axios.get<string>('localhost:5001')
        .then(
            response => dispatch(doSampleResponse(response.data)),
            error => dispatch(doSampleResponse(error))
        );
  }
