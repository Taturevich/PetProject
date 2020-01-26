import { combineReducers } from 'redux';

import { petsListReducer } from './petsList/reducer';
import { featuresListReducer } from './featuresList/reducer';
import { userFeaturesListReducer } from './userFeaturesList/reducer';
import { taskTypesListReducer } from './taskTypesList/reducer';

export const rootReducer = combineReducers({
    pets: petsListReducer,
    features: featuresListReducer,
    userFeatures: userFeaturesListReducer,
    taskTypes: taskTypesListReducer
});
