import { combineReducers } from 'redux';

import { petsListReducer } from './petsList/reducer';

export const rootReducer = combineReducers({
    pets: petsListReducer,
});
