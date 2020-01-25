import { combineReducers } from 'redux';

import { petsListReducer } from './petsList/reducer';
import { featuresListReducer } from './featuresList/reducer';

export const rootReducer = combineReducers({
    pets: petsListReducer,
    features: featuresListReducer,
});
