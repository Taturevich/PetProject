import { createStore, applyMiddleware, Action } from 'redux';
import thunk, { ThunkAction } from "redux-thunk";
import { composeWithDevTools } from 'redux-devtools-extension';

import { rootReducer } from './rootReducer';
import { PetsListState } from './petsList/state';
import { FeaturesListState } from './featuresList/state';

export interface AppState {
  pets: PetsListState;
  features: FeaturesListState;
}

const initialState: AppState = {
  pets: {
    data: []
  },
  features: {
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
