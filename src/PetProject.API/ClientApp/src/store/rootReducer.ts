import { combineReducers } from 'redux';

import { petsListReducer } from './petsList/reducer';
import { featuresListReducer } from './featuresList/reducer';
import { userFeaturesListReducer } from './userFeaturesList/reducer';

export const rootReducer = combineReducers({
    pets: petsListReducer,
    features: featuresListReducer,
    userFeatures: userFeaturesListReducer
});
