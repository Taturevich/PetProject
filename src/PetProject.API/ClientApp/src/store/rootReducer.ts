import { combineReducers } from 'redux';

import { sampleReducer } from './sample/reducer';

export const rootReducer = combineReducers({
    sample: sampleReducer,
});
