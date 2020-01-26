import { createStore, applyMiddleware, Action } from 'redux';
import thunk, { ThunkAction } from "redux-thunk";
import { composeWithDevTools } from 'redux-devtools-extension';

import { rootReducer } from './rootReducer';
import { PetsListState } from './petsList/state';
import { FeaturesListState } from './featuresList/state';
import { UserFeaturesListState } from './userFeaturesList/state';
import { TaskTypesListState } from './taskTypesList/state';
import { TaskTypesFiltersListState } from './taskTypesFilters/state';
import { UserInfoListState } from './userInfo/state';

export interface AppState {
  pets: PetsListState;
  features: FeaturesListState;
  userFeatures: UserFeaturesListState;
  usersInfo: UserInfoListState;
  taskTypes: TaskTypesListState;
  taskTypesFilters: TaskTypesFiltersListState;
}

const initialState: AppState = {
  pets: {
    data: []
  },
  features: {
    data: []
  },
  userFeatures: {
    data: []
  },
  taskTypes: {
    data: []
  },
  taskTypesFilters: {
    data: []
  },
  usersInfo: {
    data: []
  }
};

export const store = createStore(rootReducer, initialState, composeWithDevTools(applyMiddleware(thunk)));

export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  typeof store,
  null,
  Action<string>
>;
